using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


using static LWDicer.Control.DEF_SerialPort;
using static LWDicer.Control.DEF_Common;
using static LWDicer.Control.DEF_Error;

using LWDicer.UI;

namespace LWDicer.Control
{
    public class MSerialPort : MObject, ISerialPort, IDisposable
    {
        CSerialPortData m_Data;
       
        SerialPort m_SerialPort;
        Queue<string> m_ReceivedQueue = new Queue<string>();

        FormLaserMaint DisScanner = new FormLaserMaint();

        public MSerialPort(CObjectInfo objInfo, CSerialPortData data) 
            : base(objInfo)
        {
            SetData(data);

            Initialize();
        }

        ~MSerialPort()
        {
            Dispose();
        }

        public void Dispose()
        {
            ClosePort();
        }

        public bool IsOpened()
        {
            return m_SerialPort?.IsOpen ?? false;
        }

        private int Initialize()
        {
            try
            {
                m_SerialPort = new SerialPort(m_Data.PortName, m_Data.BaudRate, m_Data._Parity,
                    m_Data.DataBits, m_Data._StopBits);

               // m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortReceiveEvent);
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_SERIALPORT_CREATEPORT_FAIL);
            }

            return SUCCESS;
        }

        public void SerialPortReceiveEvent(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //SerialPort sp;
                //sp = (SerialPort)sender;

                //string message = m_SerialPort.ReadLine();

                //message.Trim();

                //if (message.Length > 0)
                //{
                //    m_ReceivedQueue.Enqueue(message);
                //}

                byte[] byteData = new byte[1024];
                int byteCount = 0;
                string strText = string.Empty;
                byteCount = m_SerialPort.Read(byteData, 0, 1024);

                for (int i = 0; i < byteCount; i++)
                {
                    strText += (char)byteData[i];
                }

                strText.Trim();

                m_ReceivedQueue.Enqueue(strText);
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                //return GenerateErrorCode(ERR_SERIALPORT_CREATEPORT_FAIL);
            }
        }


        public int OpenPort()
        {
            try
            {
                if (IsOpened() == false)
                {
                    m_SerialPort?.Open();

                    m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortReceiveEvent);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_SERIALPORT_CREATEPORT_FAIL);
            }
            return SUCCESS;
        }

        public int ClosePort()
        {
            try
            {
                if (IsOpened() == true)
                {
                    m_SerialPort.Close();

                    m_SerialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPortReceiveEvent);
                }
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_SERIALPORT_CLOSEPORT_FAIL);
            }
            return SUCCESS;
        }

        public int SendMessage(string message)
        {
            if(IsOpened() == false)
            {
                return GenerateErrorCode(ERR_SERIALPORT_CLOSEPORT_FAIL);
            }

            try
            {
                //m_SerialPort.WriteLine(message);
                m_SerialPort?.Write(message);
            }
            catch (Exception ex)
            {
                WriteExLog(ex.ToString());
                return GenerateErrorCode(ERR_SERIALPORT_SENDMESSAGE_FAIL);
            }

            return SUCCESS;
        }

        public int ReceiveMessage(out string message, out int leftQueueSize)
        {
            leftQueueSize = m_ReceivedQueue.Count;
            if(leftQueueSize == 0)
            {
                message = "";
                return GenerateErrorCode(ERR_SERIALPORT_RECEIVEDQUE_EMPTY);
            }

            message = m_ReceivedQueue.Dequeue();

            return SUCCESS;
        }

        public void ClearReceiveQue()
        {
            m_ReceivedQueue.Clear();
        }

        /***************** Common Implementation *************************************/

        public int SetData(CSerialPortData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }

        public int GetData(out CSerialPortData target)
        {
            target = ObjectExtensions.Copy(m_Data);

            return SUCCESS;
        }
    }
}
