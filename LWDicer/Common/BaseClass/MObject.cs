using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.Control
{
    public class MObject
    {
        protected CObjectInfo objInfo;
        protected MLog LogManager;

        public MObject(CObjectInfo objInfo)
        {
            this.objInfo = objInfo;
            LogManager = new MLog(objInfo.ID);
            LogManager.SetLogAttribute(objInfo.DebugTableName, objInfo.LogLevel, objInfo.LogDays);
            LogManager.SetDBInfo(CObjectInfo.DBInfo);

            string str = $"{this} Created OK";
            WriteLog(str);
        }

        public int GenerateErrorCode(int error)
        {
            if (error == SUCCESS) // Success
            {
                return SUCCESS;
            }

            int errorCode = (objInfo.ID << 16) + (objInfo.ErrorBase + error);
            return errorCode;
        }

        public override string ToString()
        {
            return $"[Object] ID : {objInfo.ID}, Name : {objInfo.Name}";
        }

        public int WriteLog(string strLog, ELogType logType = ELogType.Debug, ELogWriteType writeType = ELogWriteType.Normal, bool ShowOutputWindow = false, int skipFrames = 2)
        {
            return LogManager.WriteLog(strLog, logType, writeType, ShowOutputWindow, skipFrames);
        }

        public void WriteExLog(string strLog, ELogType logType = ELogType.Debug, ELogWriteType writeType = ELogWriteType.Error, bool ShowOutputWindow = true, int skipFrames = 3)
        {
            LogManager.WriteLog(strLog, logType, writeType, ShowOutputWindow, skipFrames);
        }

        public void Sleep<T>(T millisec)
        {
            try
            {
                int time = Convert.ToInt32(millisec);
                Thread.Sleep(time);
            }catch (Exception ex)
            {
                WriteExLog(ex.ToString());
            }
        }

        protected void Assert(bool condition)
        {
            Debug.Assert(condition);
        }

        protected void WriteLine(object msg)
        {
            Debug.WriteLine(msg.ToString());
        }

    }
}
