using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;

using static LWDicer.Control.DEF_Error;
using static LWDicer.Control.DEF_Common;


namespace LWDicer.Control
{
    public class MLog
    {
        // Define DBs.
        public static CDBInfo DBInfo;

        // Log File Path
        string DebugTableName;

        // Tact Time Log File Path
        string TactTableName;

        // Log Level : no - normal- warning - error
        bool IsWriteLog_Normal;
        bool IsWriteLog_Warning;
        bool IsWriteLog_Error;

        // Keeping days of log files
        public int m_iLogKeepingDays { get; private set; } = DEF_MLOG_DEFAULT_KEEPING_DAYS;

        public int ObjectID { get; set; }

        public MLog(int ObjectID, bool SetDefaultAttribute = false)
        {
            this.ObjectID = ObjectID;
            if (SetDefaultAttribute == true)
            {
                SetLogAttribute("Default", LOG_ALL, LOG_DAY);
            }
        }

        int SetLogKeepingDays(int days)
        {
            if (days < 10)
                return ERR_MLOG_TOO_SHORT_KEEPING_DAYS;
            m_iLogKeepingDays = days;

            return SUCCESS;
        }

        int SetLogLevel(byte level)
        {
            if ((level & DEF_MLOG_ERROR_LOG_LEVEL) == DEF_MLOG_ERROR_LOG_LEVEL)
                IsWriteLog_Error = true;
            if ((level & DEF_MLOG_WARNING_LOG_LEVEL) == DEF_MLOG_WARNING_LOG_LEVEL)
                IsWriteLog_Warning = true;
            if ((level & DEF_MLOG_NORMAL_LOG_LEVEL) == DEF_MLOG_NORMAL_LOG_LEVEL)
                IsWriteLog_Normal = true;

            return SUCCESS;
        }

        public int SetLogAttribute(string tableName, byte ucLevel, int iDays)
        {
            DebugTableName = tableName;
            TactTableName = "Tact_" + tableName;

            SetLogLevel(ucLevel);
            m_iLogKeepingDays = iDays;

            return SUCCESS;
        }

        public int SetDBInfo(CDBInfo dbInfo)
        {
            DBInfo = dbInfo;
            return SUCCESS;
        }

        public int WriteLog(string strLog, ELogType logType, ELogWType writeType, bool bShowDebugLine = true, int skipFrames = 2 )
        {

            // 0. initialize
            DateTime now = DateTime.Now;
            StackFrame sf = new StackFrame(skipFrames, true);
            string strMethodName = sf.GetMethod().ToString();
            string sfFileName = sf.GetFileName();
            int sfErrLine = sf.GetFileLineNumber();

            if (sfFileName != null)
            {
                int iFound = sfFileName.LastIndexOf("\\");
                if (iFound == -1)
                {
                    iFound = sfFileName.LastIndexOf("/");
                }

                if (iFound >= 0)
                {
                    sfFileName = sfFileName.Substring(iFound);
                }

                sfFileName = sfFileName.Replace("\\", "");
                sfFileName = sfFileName.Replace("/", "");
            }

            // 0.5 show debug window
            strLog = strLog.Replace("\r\n", " ");
            if (bShowDebugLine)
            {
                string log = $"{now.ToString("yyyy-MM-dd HH:mm:ss.fff")}, {logType.ToString()}, {writeType.ToString()}, {strLog}, ({sfFileName}, {sfErrLine.ToString()} line)";
                Debug.WriteLine(log);
            }

            // 1. write log
            if (logType == ELogType.Debug) // Debug
            {
                if (writeType == ELogWType.Normal && IsWriteLog_Normal == false) return SUCCESS;
                if (writeType == ELogWType.Warning && IsWriteLog_Warning == false) return SUCCESS;
                if (writeType == ELogWType.Error && IsWriteLog_Error == false) return SUCCESS;

                string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableDebugLog} (Time datetime, Name string, Type string, Comment string, File string, Line string)";
                string query = $"INSERT INTO {DBInfo.TableDebugLog} VALUES ('{DBManager.DateTimeSQLite(now)}', '{DebugTableName}', '{writeType.ToString()}', '{strLog}', '{sfFileName}', '{sfErrLine.ToString()}')";

                DBManager.ExecuteNonQuerys(DBInfo.DBConn_DLog, create_query, query);
            }
            else if(logType == ELogType.Tact) // Tact
            {

            }
            else if (logType == ELogType.SECGEM) // SecGem
            {

            }
            else // Other Event
            {
                string create_query = $"CREATE TABLE IF NOT EXISTS {DBInfo.TableEventLog} (Time datetime, Name string, Type string, Comment string, File string, Line string)";
                string query = $"INSERT INTO {DBInfo.TableEventLog} VALUES ('{DBManager.DateTimeSQLite(now)}', '{logType.ToString()}', '{writeType.ToString()}', '{strLog}', '{sfFileName}', '{sfErrLine.ToString()}')";

                DBManager.ExecuteNonQuerys(DBInfo.DBConn_ELog, create_query, query);
            }

            return SUCCESS;
        }

        int DeleteOldLogFiles()
        {
            //    int pDays[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            //    struct tm * tmNewTime;
            //    time_t timetClock;
            //int iLeapYear;
            //int iDDay, iDMonth, iDYear, iDElapsedDay, iDiffYear, iToday;
            //char szLogFile[256], szDeleteFile[256], szWildCardFileName[256], szToday[20];

            //HANDLE handleFirstFile;
            //WIN32_FIND_DATA finddataFile;

            //string currentFile;

            //	        // File Name이 없는 경우는 Log 하지 않고 넘어간다.
            //	        if( (m_strLogFileName.GetLength() == 0) )
            //		        return SUCCESS;

            //            // Get time in seconds 
            //            time( &timetClock );
            //        // Convert time to struct  tm form 
            //        tmNewTime = localtime( &timetClock );

            //        iLeapYear = tmNewTime->tm_year + 1900;
            //	        if( (!(iLeapYear % 4) && (iLeapYear % 100)) || !(iLeapYear % 400) )
            //		        pDays[1] = 29;

            //	        if( m_strLogFilePath.ReverseFind('\\') == m_strLogFilePath.GetLength() - 1 )
            //	        {

            //                sprintf(szWildCardFileName, "%s*%s", _T(m_strLogFilePath), _T(m_strLogFileName) );
            //                sprintf(szLogFile,"%s", _T(m_strLogFilePath) );
            //	        }
            //	        else if( m_strLogFilePath.ReverseFind('/') == m_strLogFilePath.GetLength() - 1 )
            //	        {

            //                sprintf(szWildCardFileName, "%s*%s", _T(m_strLogFilePath), _T(m_strLogFileName) );
            //                sprintf(szLogFile,"%s", _T(m_strLogFilePath) );
            //	        }
            //	        else
            //	        {
            //                sprintf(szWildCardFileName, "%s\\*%s", _T(m_strLogFilePath), _T(m_strLogFileName));
            //                sprintf(szLogFile,"%s\\", _T(m_strLogFilePath) );
            //	        }

            //            sprintf(szToday, "%04d%02d%02d", tmNewTime->tm_year+1900, tmNewTime->tm_mon+1, tmNewTime->tm_mday );

            //iToday = atoi(szToday);

            //handleFirstFile = FindFirstFile(szWildCardFileName, &finddataFile );
            //	        if( handleFirstFile == INVALID_HANDLE_VALUE )
            //		        return SUCCESS;
            //	        else
            //	        {
            //		        currentFile.Format(finddataFile.cFileName);
            //		        iDYear = atoi(_T(currentFile.Left(4)));
            //		        iDMonth = atoi(_T(currentFile.Mid(4,2)));
            //		        iDDay = atoi(_T(currentFile.Mid(6,2)));

            //		        iDiffYear = tmNewTime ->tm_year + 1900 - iDYear;

            //		        int i = 0;
            //		        if( iDiffYear == 0 )
            //		        {
            //			        iDElapsedDay = 0;
            //			        for( i = iDMonth; i<tmNewTime->tm_mon+2 ; i++ )
            //			        {
            //				        if( iDMonth == tmNewTime->tm_mon + 1 )
            //				        {
            //					        iDElapsedDay = tmNewTime->tm_mday - iDDay;
            //					        break;
            //				        }

            //				        if( i == iDMonth )
            //					        iDElapsedDay += ( pDays[iDMonth - 1] - iDDay );
            //				        else if( i == tmNewTime->tm_mon+ 1 )
            //					        iDElapsedDay += tmNewTime->tm_mday;
            //				        else
            //					        iDElapsedDay += pDays[i - 1];
            //			        }

            //			        if( iDElapsedDay > m_iLogKeepingDays )
            //			        {

            //                        sprintf(szDeleteFile, "%s%s", szLogFile, finddataFile.cFileName );

            //                        DeleteFile(szDeleteFile);
            //			        }
            //		        }
            //		        else
            //		        {
            //			        iDElapsedDay = 0;
            //			        for( i = iDMonth; i< 13 ; i++ )
            //			        {
            //				        if( i == iDMonth ) 
            //					        iDElapsedDay += ( pDays[iDMonth - 1] - iDDay );
            //				        else
            //					        iDElapsedDay += pDays[i - 1];
            //			        }

            //			        for( i = 1; i<tmNewTime->tm_mon+2 ; i++ )
            //			        {
            //				        if( i == tmNewTime->tm_mon + 1 ) 
            //					        iDElapsedDay += tmNewTime->tm_mday;
            //				        else
            //					        iDElapsedDay += pDays[i];
            //			        }

            //			        iDElapsedDay += ( iDiffYear - 1 ) * 365;
            //			        if( iDElapsedDay > m_iLogKeepingDays )
            //			        {

            //                        sprintf(szDeleteFile, "%s%s", szLogFile, finddataFile.cFileName );

            //                        DeleteFile(szDeleteFile);
            //			        }
            //		        }
            //	        }
            //	        while( FindNextFile(handleFirstFile, &finddataFile ) )
            //	        {
            //		        currentFile.Format(finddataFile.cFileName);
            //		        iDYear = atoi(_T(currentFile.Left(4)));
            //		        iDMonth = atoi(_T(currentFile.Mid(4,2)));
            //		        iDDay = atoi(_T(currentFile.Mid(6,2)));

            //		        iDiffYear = tmNewTime ->tm_year + 1900 - iDYear;

            //		        int i = 0;
            //		        if( iDiffYear == 0 )
            //		        {
            //			        iDElapsedDay = 0;
            //			        for( i = iDMonth; i<tmNewTime->tm_mon+2 ; i++ )
            //			        {
            //				        if( iDMonth == tmNewTime->tm_mon + 1 )
            //				        {
            //					        iDElapsedDay = tmNewTime->tm_mday - iDDay;
            //					        break;
            //				        }

            //				        if( i == iDMonth )
            //					        iDElapsedDay += ( pDays[iDMonth - 1] - iDDay );
            //				        else if( i == tmNewTime->tm_mon+ 1 )
            //					        iDElapsedDay += tmNewTime->tm_mday;
            //				        else
            //					        iDElapsedDay += pDays[i - 1];
            //			        }

            //			        if( iDElapsedDay > m_iLogKeepingDays )
            //			        {

            //                        sprintf(szDeleteFile, "%s%s", szLogFile, finddataFile.cFileName );

            //                        DeleteFile(szDeleteFile);
            //			        }
            //		        }
            //		        else
            //		        {
            //			        iDElapsedDay = 0;
            //			        for( i = iDMonth; i< 13 ; i++ )
            //			        {
            //				        if( i == iDMonth ) 
            //					        iDElapsedDay += ( pDays[iDMonth - 1] - iDDay );
            //				        else
            //					        iDElapsedDay += pDays[i - 1];
            //			        }

            //			        for( i = 1; i<tmNewTime->tm_mon+2 ; i++ )
            //			        {
            //				        if( i == tmNewTime->tm_mon + 1 ) 
            //					        iDElapsedDay += tmNewTime->tm_mday;
            //				        else
            //					        iDElapsedDay += pDays[i];
            //			        }

            //			        iDElapsedDay += ( iDiffYear - 1 ) * 365;
            //			        if( iDElapsedDay > m_iLogKeepingDays )
            //			        {

            //                        sprintf(szDeleteFile, "%s%s", szLogFile, finddataFile.cFileName );

            //                        DeleteFile(szDeleteFile);
            //			        }
            //		        }
            //	        }
            return SUCCESS;

        }

    }
}
