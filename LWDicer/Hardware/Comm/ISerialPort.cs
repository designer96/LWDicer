using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using static LWDicer.Control.DEF_SerialPort;

namespace LWDicer.Control
{
    public class DEF_SerialPort
    {
        public const int ERR_SERIALPORT_CREATEPORT_FAIL        = 1;
        public const int ERR_SERIALPORT_OPENPORT_FAIL          = 2;
        public const int ERR_SERIALPORT_CLOSEPORT_FAIL         = 3;
        public const int ERR_SERIALPORT_SENDMESSAGE_FAIL       = 4;
        public const int ERR_SERIALPORT_PORT_NOT_OPENED        = 5;
        public const int ERR_SERIALPORT_RECEIVEDQUE_EMPTY = 6;

        public class CSerialPortData
        {
            public string PortName;
            public int BaudRate;
            public Parity _Parity;
            public int DataBits;
            public StopBits _StopBits;

            public CSerialPortData(string PortName, int BaudRate, Parity _Parity, int DataBits, StopBits _StopBits)
            {
                this.PortName  = PortName;
                this.BaudRate  = BaudRate;
                this._Parity   = _Parity;
                this.DataBits  = DataBits;
                this._StopBits = _StopBits;
            }
        }
    }

    public interface ISerialPort
    {
	    int OpenPort();
        int ClosePort();

        bool IsOpened();

    	int SendMessage(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="LeftQueueSize">strMsg를 꺼내고 남은 ReceivedQueue Size </param>
        /// <returns></returns>
	    int ReceiveMessage(out string message, out int leftQueueSize);

	    void ClearReceiveQue();

        //----------- Component 공통  -----------------------
        int SetData(CSerialPortData source);
        int GetData(out CSerialPortData target);

    }
}
