using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainServer
{
    class UDP : UdpClient
    {
        List<GameinZone> MassGameZone;

        public void ini(List<GameinZone> lgz)
        {
            MassGameZone = lgz;
        }
        public void Send(string datagram, IPEndPoint IP)
        {
         
            // Создаем UdpClient
            UdpClient sender = new UdpClient(2049);
           // if (IP) return;

            //IPAddress remoteIPAddress = IPAddress.Parse(IP);
            // Создаем endPoint по информации об удаленном хосте
           // IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, 11000);

            try
            {

                // Преобразуем данные в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);

                // Отправляем данные
                sender.Send(bytes, bytes.Length, IP);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                sender.Close();
            }

        }          
     

    }
}
