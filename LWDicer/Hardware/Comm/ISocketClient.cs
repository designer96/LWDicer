using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using static LWDicer.Control.DEF_SocketClient;

namespace LWDicer.Control
{
    public class DEF_SocketClient
    {
        public const int ERR_SOCKETCLIENT_NOTCONNECTED = 1;
        public const int ERR_SOCKETCLIENT_CREATESOCKET_FAIL = 2;
        public const int ERR_SOCKETCLIENT_CONNECTSOCKET_FAIL = 3;
        public const int ERR_SOCKETCLIENT_SENDMESSAGE_FAIL = 4;
        public const int ERR_SOCKETCLIENT_RECEIVEMESSAGE_FAIL = 5;
        public const int ERR_SOCKETCLIENT_ACCEPTREQUEST_FAIL = 6;
        public const int ERR_SOCKETCLIENT_RECEIVEDQUE_EMPTY = 7;

        public class CSocketClientData
        {
            public string HostAddress;
            public int HostPort;

            public CSocketClientData(string HostAddress, int HostPort)
            {
                this.HostAddress = HostAddress;
                this.HostPort = HostPort;
            }

        }
    }

    interface ISocketClient
    {
        int ConnectServer();
        int DisconnectServer();

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
        int SetData(CSocketClientData source);
        int GetData(out CSocketClientData target);

    }
}
