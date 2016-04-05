using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;

namespace LWDicer.Control
{
    public class CDBColumn
    {
        public string Name;
        public string Value;
    }

    public static class DBManager
    {
        static object SyncObject = new object();
        static string op_number;
        static string op_type;

        public static void SetOperator(string number, string type)
        {
            op_number = number;
            op_type = type;
        }

        private static bool IsNullOrEmpty(string str)
        {
            if (string.IsNullOrEmpty(str) || str.ToLower() == "null") return true;

            return false;
        }

        public static string DateTimeSQLite(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static bool ExecuteNonQuerys(string conninfo, params string[] queries)
        {
            lock (SyncObject)
            {
                using (SQLiteConnection conn = new SQLiteConnection(conninfo))
                {
                    try
                    {
                        // 0. initialize
                        conn.Open();
                        SQLiteCommand cmd;
                        SQLiteTransaction transaction = conn.BeginTransaction();

                        // 1. execute sql
                        foreach (string query in queries)
                        {
                            if (query != null && query != "")
                            {
                                cmd = new SQLiteCommand(query, conn);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool ExecuteNonQuerys(string conninfo, List<string> queries)
        {
            lock (SyncObject)
            {
                using (SQLiteConnection conn = new SQLiteConnection(conninfo))
                {
                    try
                    {
                        // 0. initialize
                        conn.Open();
                        SQLiteCommand cmd;
                        SQLiteTransaction transaction = conn.BeginTransaction();

                        // 1. execute sql
                        foreach (string query in queries)
                        {
                            if (query != null && query != "")
                            {
                                cmd = new SQLiteCommand(query, conn);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool GetTable(string conninfo, string query, out DataTable dataTable)
        {
            lock (SyncObject)
            {
                dataTable = new DataTable();
                using (SQLiteConnection conn = new SQLiteConnection(conninfo))
                {
                    try
                    {
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        cmd.CommandType = CommandType.Text;
                        SQLiteDataAdapter ObjDataAdapter = new SQLiteDataAdapter(cmd);
                        ObjDataAdapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("GetTable : " + query + ex);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool BackupDB(string source, DateTime time)
        {
            lock (SyncObject)
            {
                try
                {
                    string backup = source.Replace(".db3", $"{time.ToString("yyyyMMdd_HHmmss")}.db3");
                    if (File.Exists(source))
                    {
                        File.Copy(source, backup, true);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }

            return true;
        }

        public static bool DeleteDB(string source)
        {
            lock (SyncObject)
            {
                try
                {
                    if (File.Exists(source))
                    {
                        File.Delete(source);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }

        public static bool DropTable(string conninfo, string[] tablelist, bool delete_backup = false, string backup_conninfo = "")
        {
            foreach (string table in tablelist)
            {
                string cmd1_sql = $"DROP TABLE IF EXISTS {table}";
                string cmd2_sql = $"DROP TABLE IF EXISTS {table}";

                if (DBManager.ExecuteNonQuerys(conninfo, cmd1_sql) == false)
                {
                    return false;
                }

                if (delete_backup)
                {
                    if (DBManager.ExecuteNonQuerys(backup_conninfo, cmd2_sql) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool ExecuteSelectQuery(string conninfo, string query, params CDBColumn[] columns)
        {
            DataTable datatable;
            if (GetTable(conninfo, query, out datatable) == false)
                return false;

            foreach (DataRow row in datatable.Rows)
            {
                for(int i = 0; i < columns.Length; i++)
                {
                    columns[i].Value = row[columns[i].Name].ToString();
                }
                break;
            }

            return true;
        }

        public static bool InsertRow(string conninfo, string table, string key, string key_value, string data,
            bool backup = false, string backup_conninfo = "")
        {
            if (IsNullOrEmpty(key_value)) return false;
            if (IsNullOrEmpty(data)) return false;

            // 0. initialize
            DateTime now = DateTime.Now;

            // 1. select previous row for backup
            string create_query = $"CREATE TABLE IF NOT EXISTS {table} (name string primary key, created datetime, op_number string, op_type string, data string)";
            if (ExecuteNonQuerys(conninfo, create_query) == false)
                return false;

            string query;
            if(backup == true && IsNullOrEmpty(backup_conninfo) == false)
            {
                // 2. select previous and check change
                query = $"SELECT * FROM {table} WHERE ({key} = '{key_value}')";
                CDBColumn column1 = new CDBColumn();
                column1.Name = "data";
                CDBColumn column2 = new CDBColumn();
                column2.Name = "created";

                if (ExecuteSelectQuery(conninfo, query, column1, column2) == false)
                    return false;

                // if data are same, return ok
                if (data == column1.Value)
                {
                    return true;
                }

                // 3. backup
                if (IsNullOrEmpty(column1.Value) == false)
                {
                    create_query = $"CREATE TABLE IF NOT EXISTS {table} (name string, created datetime, modified datetime, op_number string, op_type string, data string)";
                    query = $"INSERT INTO {table} VALUES ('{key_value}', '{column2.Value}', '{DateTimeSQLite(now)}', '{op_number}', '{op_type}', '{column1.Value}')";

                    ExecuteNonQuerys(backup_conninfo, create_query, query);
                }
            }

            // 4. delete & insert
            query = $"DELETE FROM {table} WHERE ({key} = '{key_value}')";
            string query1 = $"INSERT INTO {table} VALUES ('{key_value}', '{DateTimeSQLite(now)}', '{op_number}', '{op_type}', '{data}')";
            if (ExecuteNonQuerys(conninfo, query, query1) == false)
                return false;

            return true;
        }

        public static bool SelectRow(string conninfo, string table, string key, string key_value, out string data)
        {
            data = "";
            if (IsNullOrEmpty(key)) return false;
            if (IsNullOrEmpty(key_value)) return false;

            string query = $"SELECT * FROM {table} WHERE ({key} = '{key_value}')";
            CDBColumn column = new CDBColumn();
            column.Name = "data";

            if (DBManager.ExecuteSelectQuery(conninfo, query, column) == false)
                return false;

            data = column.Value;
            if (IsNullOrEmpty(data)) return false;

            return true;
        }
    }
}
