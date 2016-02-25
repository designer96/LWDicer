using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using static LWDicer.Control.DEF_SocketServer;

namespace LWDicer.Control
{
    public class DEF_SocketServer
    {
        public const int ERR_SOCKETSERVER_NOTCONNECTED           = 1;
        public const int ERR_SOCKETSERVER_CREATESOCKET_FAIL      = 2;
        public const int ERR_SOCKETSERVER_CONNECTSOCKET_FAIL     = 3;
        public const int ERR_SOCKETSERVER_SENDMESSAGE_FAIL       = 4;
        public const int ERR_SOCKETSERVER_RECEIVEMESSAGE_FAIL    = 5;
        public const int ERR_SOCKETSERVER_ACCEPTREQUEST_FAIL     = 6;
        public const int ERR_SOCKETSERVER_RECEIVEDQUE_EMPTY = 7;

        public class CSocketServerData
        {
            public string HostAddress;
            public int HostPort;

            public CSocketServerData(string HostAddress, int HostPort)
            {
                this.HostAddress = HostAddress;
                this.HostPort = HostPort;
            }

        }
    }

    public class AsyncObject
    {
        public Byte[] Buffer;
        public Socket WorkingSocket;
        public AsyncObject(Int32 bufferSize)
        {
            this.Buffer = new Byte[bufferSize];
        }
    }

    interface ISocketServer
    {
        int StartServer();
        int CloseServer();

        bool IsConnected();

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
        int SetData(CSocketServerData source);
        int GetData(out CSocketServerData target);

    }
}
