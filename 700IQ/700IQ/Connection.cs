using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data;

namespace _700IQ
{

    class Connection
    {
        public delegate void dataReceive(string resp);
        public dataReceive onDataReceive;
        //public delegate void answerOk(string ans);
        //public regOk onanswerOk;

        //UdpClient receivingUdpClient;

        private IPEndPoint remoteIPAddress;
        public Connection(IPEndPoint remoteIPAddress)
        {
            this.remoteIPAddress = remoteIPAddress;
        }
        public async void Send(string datagram)
        {

            string response = "";
            //IniFile fIni = new IniFile(Application.StartupPath + "\\settings.ini");
           // remoteIPAddress = IPAddress.Parse(fIni.IniReadValue("Settings", "Server", "10.10.10.10"));
            //  MessageBox.Show(datagram);

            using (var tcpClient = new TcpClient())
            {
                try
                {
                    await tcpClient.ConnectAsync(remoteIPAddress.Address, remoteIPAddress.Port);

                    using (var networkStream = tcpClient.GetStream())
                    {

                        byte[] result;
                        result = Encoding.UTF8.GetBytes(datagram);
                        await networkStream.WriteAsync(result, 0, result.Length);

                        var buffer = new byte[4096];
                        var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                        response = Encoding.UTF8.GetString(buffer, 0, byteCount);
                        onDataReceive(response);

                        /* switch (response.Substring(0, 5))
                        {
                            case "regOk":
                                onDataReceive(response.Substring(5));
                                break;
                            case "Teams":
                                {
                                  teamLst = JsonConvert.DeserializeObject<string[]>(response.Substring(5));
                                }
                                break;
                            case "False":
                                MessageBox.Show("Неверный логин или пароль!");
                                break;
                            case "IPtwo":
                                MessageBox.Show("Повторный IP-адресс");
                                break;
                        }*/
                    }
                }
                catch (Exception ex)
                {
                    onDataReceive("Error");
                    // MessageBox.Show("Регистрация пока не открыта, пожалуйста подождите");

                }
                finally
                {
                    tcpClient.Close();
                }
            }
        }


    }

}
