using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace _700IQ
{
    public class Conn  //обмен UDP сообщениями клиента с сервером (запрос клиента-ответ сервера)
    {
        #region описание переменных
        private EndPoint senderRemote;
        private Socket server;
        private byte[] byteData = new byte[65000];
        //UdpClient sender = new UdpClient();
        //IPEndPoint endPoint;
        //int port;
        string nextKom, oldKom, multiKom, lastCommand;
        bool flag;
        private bool CrazyTimer = false;
        //System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        //System.Windows.Forms.Timer mtm = new System.Windows.Forms.Timer();
        private System.Timers.Timer tm;
        private System.Timers.Timer mtm;
        public delegate void newkom(string komanda);
        public newkom onNewKom;
        delegate void getkom(string komanda);
        getkom onGetKom;
        private static object locker = new object();

        #endregion

        public Conn(IPAddress remoteIPAddress) //конструктор класса 
        {
            //IniFile fIni = new IniFile(Application.StartupPath + "\\settings.ini");
           // IPAddress remoteIPAddress = IPAddress.Parse(fIni.IniReadValue("Settings", "Server", "10.10.10.10"));
            //endPoint = new IPEndPoint(remoteIPAddress, 2050);           //адрес сервера   

            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipEndPoint = new IPEndPoint(remoteIPAddress, 2050);
            senderRemote = (EndPoint)ipEndPoint;

            //byte[] bytes = Encoding.UTF8.GetBytes("");                  //
            //sender.Send(bytes, 0, endPoint);                            //отсылаем пустой байт серверу
            //port = ((IPEndPoint)(sender.Client.LocalEndPoint)).Port;    //определяем свободный выделенный для передачи порт
            //GetKom();                                                   //запускае прослушку порта
            onGetKom += getkomback;                                     //делегат получения ответа от сервера

            tm = new System.Timers.Timer(15000);
            tm.Elapsed += callback;
            tm.AutoReset = false;
            //tm.Interval = 3000;                                         //
            //tm.Tick += new EventHandler(callback);                      //описание таймара
            //tm.Stop();

            //mtm = new System.Timers.Timer(3000);
            //mtm.Elapsed += multy;                        //описание таймера для многоразовых запросов
            //mtm.AutoReset = false;
            //mtm.Tick += new EventHandler(multy);                        //описание таймера для многоразовых запросов
            //mtm.Stop();
            lastCommand = "";
            SendUDP("");
        }
        async void GetKom()  //ожидаем ответа от сервера
        {
            while (true)
            {
                try
                {
                    /*var buffer = new ArraySegment<byte>(new byte[4096]);
                    //Ожидаем данные от сервера
                    var result = await sender.ReceiveAsync();
                    byte[] receiveBytes = result.Buffer;
                    nextKom = Encoding.UTF8.GetString(receiveBytes);              
                    if(nextKom.Length>2)  onGetKom(nextKom);                                    */
                }
                catch(SocketException ex)
                {
                    if (ex.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset)
                        MessageBox.Show("Связь с сервером потеряна!", "Проблема сети", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    onGetKom("error");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Предупреждение!");
                }
            }
        }
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                server.EndSend(ar);
                server.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref senderRemote, new AsyncCallback(OnReceive), null);
                tm.Start();
            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                Debug.WriteLine("UdpClient->OnSend: Oops... " + ex.Message);
            }
        }
        private void OnReceive(IAsyncResult ar)
        {
            //try
            //{
                if(!CrazyTimer)
                    tm.Stop();
                int bytesReaded = server.EndReceiveFrom(ar, ref senderRemote);
                //Преобразование массива байтов, полученных от пользователя в удобную для нас форму объекта данных
                //byte[] cleanBuffer = byteData.TakeWhile(b => b != 0).ToArray();
                nextKom = Encoding.UTF8.GetString(byteData, 0, bytesReaded);
                if (nextKom.Length > 2)
                    onGetKom(nextKom);
            //server.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref senderRemote, new AsyncCallback(OnReceive), null);
            //tm.Start();
            //}
            //catch (ObjectDisposedException)
            //{ }
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("UdpClient->OnReceive: " + ex.Message);
            //}
        }

        public void SendUDP(string commamd) //отправляем запрос на сервер
        {
            //tm.Stop();
            oldKom = commamd;
            //nextKom = commamd;
            flag = false;
            //byteData = new byte[4096];
            byte[] bytes = Encoding.UTF8.GetBytes(commamd);
            try
            {
                // Отправляем данные 
                server.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, senderRemote, new AsyncCallback(OnSend), null);
                //sender.Send(bytes, bytes.Length, endPoint);
                //tm.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UdpClient->SendUDP: " + ex.Message);
            }
        }
        void getkomback(string commamd) //получаем ответ от сервера и проверяем на корректность
        {
            //проверяем ответ сервера 

            //if (oldKom.Substring(1, 2) == nextKom.Substring(1, 2))
            //if (oldKom.Substring(1, 2) == commamd.Substring(1, 2) || commamd == "error")
            lock (locker)
            {
                if (lastCommand != commamd)
                {
                    lastCommand = commamd;
                    if (!CrazyTimer) tm.Stop();   //тормозим таймер               
                                                  //if(!flag)   //был ли коректный ответ?
                    {
                        flag = true;
                        onNewKom(commamd);  //передаем команду дальше     
                    }
                    //else
                    tm.Start();
                }
            }
        }
        private void callback(Object source, ElapsedEventArgs e)   //обрабатываем срабатывание таймера повторно отправляем запрос
        {
            //byte[] byteData = Encoding.UTF8.GetBytes(oldKom);
            //sender.Send(bytes, bytes.Length, endPoint);
            //flag = false;
            //server.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, senderRemote, new AsyncCallback(OnSend), null);
            SendUDP(oldKom);
        }
        public void start(string komanda, int interval) //запуск многоразового запроса обновление списка команд
        {
            //multiKom = komanda;
            //mtm.Interval = interval;
            //mtm.Start();
            CrazyTimer = true;
            SendUDP(komanda);
        }
        private void multy(Object source, ElapsedEventArgs e)
        {
            //SendUDP(multiKom);
            SendUDP("");
        }
        public void stop()
        {
            //CrazyTimer = false;
            //mtm.Stop();
        }
        public void ClearLastCommand()
        {
            getkomback(null);
        }
    }
   
}
