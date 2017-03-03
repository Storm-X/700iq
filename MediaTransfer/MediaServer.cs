using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;

public class MediaServer
{
    public int Port { get; set; }
    Socket client;
    private byte[] byteData = new byte[65500];

    public MediaServer()
	{
        Port = 8080;
    }
    public void Start()
    {
        try
        {
            //CheckForIllegalCrossThreadCalls = false;
            //Указываем, что будем работать по протоколу UDP
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //Назначим любой IP серверу и будем слушать порт 8080 (по умолчанию)
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            //Привяжем этот адрес к серверу
            client.Bind(ipEndPoint);
            IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
            //Переменная epSender идентифицирует подключившегося клиента
            EndPoint epSender = (EndPoint)ipeSender;
            //Получаем данные
            client.BeginReceiveFrom(byteData, 0, byteData.Length,
                SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("MediaServer: " + ex.Message);
        }
    }
    public void Stop()
    {
        client.Close();
        client.Dispose();
    }
    public void OnSend(IAsyncResult ar)
    {
        try
        {
            client.EndSend(ar);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("MediaServer: " + ex.Message);
        }
    }
    private void OnReceive(IAsyncResult ar)
    {
        int blockSize = 65000; // 7168;
        int sentDataBytesCount = 0;

        IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint epSender = (EndPoint)ipeSender;
        client.EndReceiveFrom(ar, ref epSender);

        //Преобразование массива байтов, полученных от пользователя в удобную для нас форму объекта данных
        DataInfo msgReceived = new DataInfo(byteData);
        FileInfo fInfo = new FileInfo(@".\media\" + msgReceived.filename);
        if (fInfo.Exists == true)
        {
            DataInfo sendInfo = new DataInfo();
            sendInfo.filesize = (int)fInfo.Length;
            sendInfo.filename = fInfo.Name;

            int blocksCount = (int)Math.Ceiling((float)sendInfo.filesize / (float)blockSize);
            byte[] content = new byte[blockSize];
            byte[] fileContents = File.ReadAllBytes(fInfo.FullName);
            MD5 md5Hash = MD5.Create();

            string infoStr = (msgReceived.blockNum == 0) ? string.Format("Передача файла '{0}' - {1} байт. Всего {2} блока/блоков).", sendInfo.filename, sendInfo.filesize, blocksCount) :
                                                           string.Format("Файл '{0}' - повторная передача блока #{1}", sendInfo.filename, msgReceived.blockNum);
            Debug.WriteLine(infoStr);
            try
            {
                for (int i = 0; i < blocksCount; i++)
                {
                    int packetSize = Math.Min(sendInfo.filesize - sentDataBytesCount, blockSize);
                    if (msgReceived.blockNum == 0 || msgReceived.blockNum == i)
                    {
                        content = fileContents.Skip(sentDataBytesCount).Take(packetSize).ToArray();
                        sendInfo.blockHash = md5Hash.ComputeHash(content);
                        sendInfo.blockNum = i;
                        sendInfo.dataBlock = content;
                        byte[] message = sendInfo.ToByte();
                        client.BeginSendTo(message, 0, message.Length, SocketFlags.None, epSender, new AsyncCallback(OnSend), epSender);
                    }
                    sentDataBytesCount += packetSize;
                }
                Debug.WriteLine("Передано!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        else
        {
            Debug.WriteLine(string.Format("File {0} not fond!", msgReceived.filename));
            //return;
        }
        client.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);
    }
}
