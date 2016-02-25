using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using static LWDicer.Control.DEF_SocketServer;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;

namespace LWDicer.Control
{
    public class MSocketServer : MObject, ISocketServer, IDisposable
    {
        CSocketServerData m_Data;
        Queue<string> m_ReceivedQueue = new Queue<string>();

        private Socket m_ConnectedClient = null;
        private Socket m_ServerSocket = null;
        private AsyncCallback m_fnReceiveHandler;
        private AsyncCallback m_fnSendHandler;
        private AsyncCallback m_fnAcceptHandler;

        public MSocketServer(CObjectInfo objInfo, CSocketServerData data)
            : base(objInfo)
        {
            SetData(data);

            // 비동기 작업에 사용될 대리자를 초기화합니다.
            m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
            m_fnSendHandler = new AsyncCallback(handleDataSend);
            m_fnAcceptHandler = new AsyncCallback(handleClientConnectionRequest);
        }

        ~MSocketServer()
        {
            Dispose();
        }

        public void Dispose()
        {
            CloseServer();
        }

        public bool IsConnected()
        {
            return m_ServerSocket?.Connected ?? false;
        }

        public int StartServer()
        {
            try
            {
                // create socket
                m_ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                m_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, m_Data.HostPort));
                m_ServerSocket.Listen(5);

                // BeginAccept for process asyncronous
                m_ServerSocket.BeginAccept(m_fnAcceptHandler, null);
            } catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_SOCKETSERVER_CREATESOCKET_FAIL);
            }

            return SUCCESS;
        }

        public int CloseServer()
        {
            m_ServerSocket?.Close();

            return SUCCESS;
        }

        public int SendMessage(string message)
        {
            if(IsConnected() == false)
            {
                return GenerateErrorCode(ERR_SOCKETSERVER_NOTCONNECTED);
            }

            // 추가 정보를 넘기기 위한 변수 선언
            AsyncObject ao = new AsyncObject(1);

            ao.Buffer = Encoding.Unicode.GetBytes(message);

            ao.WorkingSocket = m_ConnectedClient;

            // 전송 시작!
            try
            {
                m_ConnectedClient.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);
            }
            catch (Exception ex)
            {
                WriteLog($"전송 중 오류 발생!\n메세지: {ex.Message}");
                return GenerateErrorCode(ERR_SOCKETSERVER_SENDMESSAGE_FAIL);
            }

            return SUCCESS;
        }

        private void handleClientConnectionRequest(IAsyncResult ar)
        {
            Socket sockClient;
            try
            {
                sockClient = m_ServerSocket.EndAccept(ar);
            }
            catch (Exception ex)
            {
                WriteLog($"연결 수락 도중 오류 발생! 메세지: {ex.Message}");
                //return GenerateErrorCode(ERR_SOCKETSERVER_ACCEPTREQUEST_FAIL);
                return;
            }

            // 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
            AsyncObject ao = new AsyncObject(4096);

            // 작업 중인 소켓을 저장하기 위해 sockClient 할당
            ao.WorkingSocket = sockClient;

            // 클라이언트 소켓 저장
            m_ConnectedClient = sockClient;

            try
            {
                // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                sockClient.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                WriteLog($"자료 수신 대기 도중 오류 발생! 메세지: {ex.Message}");
                return;
            }
        }
        private void handleDataReceive(IAsyncResult ar)
        {

            // 넘겨진 추가 정보를 가져옵니다.
            // AsyncState 속성의 자료형은 Object 형식이기 때문에 형 변환이 필요합니다~!
            AsyncObject ao = (AsyncObject)ar.AsyncState;

            // 받은 바이트 수 저장할 변수 선언
            Int32 recvBytes;

            try
            {
                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = ao.WorkingSocket.EndReceive(ar);
            }
            catch
            {
                // 예외가 발생하면 함수 종료!
                return;
            }

            // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
            if (recvBytes > 0)
            {
                // 공백 문자들이 많이 발생할 수 있으므로, 받은 바이트 수 만큼 배열을 선언하고 복사한다.
                Byte[] msgByte = new Byte[recvBytes];
                Array.Copy(ao.Buffer, msgByte, recvBytes);

                //Console.WriteLine("메세지 받음: {0}", Encoding.Unicode.GetString(msgByte));
                m_ReceivedQueue.Enqueue(Encoding.Unicode.GetString(msgByte));
            }

            try
            {
                // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                //Console.WriteLine("자료 수신 대기 도중 오류 발생! 메세지: {0}", ex.Message);
                WriteLog($"자료 수신 대기 도중 오류 발생! 메세지: {ex.Message}");
                return;
            }
        }
        private void handleDataSend(IAsyncResult ar)
        {

            // 넘겨진 추가 정보를 가져옵니다.
            AsyncObject ao = (AsyncObject)ar.AsyncState;

            // 보낸 바이트 수를 저장할 변수 선언
            Int32 sentBytes;

            try
            {
                // 자료를 전송하고, 전송한 바이트를 가져옵니다.
                sentBytes = ao.WorkingSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                WriteLog($"자료 송신 도중 오류 발생! 메세지: {ex.Message}");
                return;
            }

            if (sentBytes > 0)
            {
                // 여기도 마찬가지로 보낸 바이트 수 만큼 배열 선언 후 복사한다.
                Byte[] msgByte = new Byte[sentBytes];
                Array.Copy(ao.Buffer, msgByte, sentBytes);

                //Console.WriteLine("메세지 보냄: {0}", Encoding.Unicode.GetString(msgByte));
            }
        }

        public int ReceiveMessage(out string message, out int leftQueueSize)
        {
            leftQueueSize = m_ReceivedQueue.Count;
            if (leftQueueSize == 0)
            {
                message = "";
                return GenerateErrorCode(ERR_SOCKETSERVER_RECEIVEDQUE_EMPTY);
            }

            message = m_ReceivedQueue.Dequeue();
            return SUCCESS;
        }

        public void ClearReceiveQue()
        {
            m_ReceivedQueue.Clear();
        }

        /***************** Common Implementation *************************************/

        public int SetData(CSocketServerData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CSocketServerData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }
    }
}
