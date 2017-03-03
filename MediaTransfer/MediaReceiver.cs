using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Threading;

public class MediaReceiver
{
    private EndPoint senderRemote;
    private Socket server;
    private string FileName;
    int maxBufferSize = 65000;
    private byte[] byteData = new byte[65500];
    private byte[] fileContents;
    private Boolean[] checkBlocks;
    private String currStatus;
    public String CurrentStatus { get { return currStatus; } }
    private Boolean transferComplete;
    public ProgressBar transferProgressBar;
    private System.Timers.Timer TimeOutChecker;

    public MediaReceiver(IPAddress ipAddress, int Port)
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);
        senderRemote = (EndPoint)ipEndPoint;
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        TimeOutChecker = new System.Timers.Timer(300);
        TimeOutChecker.Elapsed += OnTimedEvent;
        TimeOutChecker.AutoReset = false;
        fileContents = new byte[0];
        checkBlocks = new Boolean[0];
        transferComplete = false;
    }
    private void OnSend(IAsyncResult ar)
    {
        try
        {
            server.EndSend(ar);
            server.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref senderRemote, new AsyncCallback(OnReceive), null);
            TimeOutChecker.Start();
        }
        catch (ObjectDisposedException)
        { }
        catch (Exception ex)
        {
            TimeOutChecker.Stop();
            transferComplete = true;
            Debug.WriteLine("FileTransferClient: Oops... " + ex.Message);
        }
    }

    private void OnReceive(IAsyncResult ar)
    {
        try
        {
            TimeOutChecker.Stop();
            server.EndReceive(ar);
            //Преобразование массива байтов, полученных от пользователя в удобную для нас форму объекта данных
            DataInfo msgReceived = new DataInfo(byteData);
            int blocksCount = (int)Math.Ceiling((float)msgReceived.filesize / (float)maxBufferSize); // blockSize);
            if (msgReceived.blockNum == 0)
            {
                currStatus = string.Format("----Информация о файле получена! Размер файла: {0} байт", msgReceived.filesize);
                fileContents = new byte[msgReceived.filesize];
                checkBlocks = new Boolean[blocksCount];
                //transferProgressBar.Invoke(new Action(() => transferProgressBar.Maximum = blocksCount));
                //transferProgressBar.Maximum = blocksCount;
            }
            MD5 md5Hash = MD5.Create();
            //int blockSize = msgReceived.dataBlock.Length; // client.ReceiveBufferSize;
            byte[] data = new byte[msgReceived.dataBlock.Length];
            //data.Take(packetSize).ToArray().CopyTo(fileContents, i * blockSize);
            if (!HashesEqual(md5Hash.ComputeHash(msgReceived.dataBlock), msgReceived.blockHash))
                currStatus = ("Контрольная сумма блока неверна!");
            else
            {
                msgReceived.dataBlock.CopyTo(fileContents, maxBufferSize * msgReceived.blockNum);
                checkBlocks[msgReceived.blockNum] = true;
            }
            //transferProgressBar.Invoke(new Action(() => transferProgressBar.PerformStep()));
            //transferProgressBar.PerformStep();
            server.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref senderRemote, new AsyncCallback(OnReceive), null);
            TimeOutChecker.Start();
        }
        catch (ObjectDisposedException)
        { }
        catch (Exception ex)
        {
            Debug.WriteLine("FileTransferClient: " + ex.Message);
        }
    }
    public byte[] GetMedia(string FileName)
    {
        this.FileName = FileName;
        try
        {
            TimeOutChecker.Stop();
            DataInfo msgToSend = new DataInfo();
            msgToSend.filename = this.FileName;
            msgToSend.blockNum = 0;
            InitializeComponent();

            byte[] byteData = msgToSend.ToByte();
            server.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, senderRemote, new AsyncCallback(OnSend), null);
            currStatus = "Ожидаем передачу файла от сервера...";
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FileTransferClient: " + ex.Message);
        }
        while (!transferComplete);
        return fileContents;
    }
    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        if (CheckFile())
            transferComplete = true;
    }
    private Boolean CheckFile()
    {
        //Остановим таймер таймаута...
        TimeOutChecker.Stop();
        //Найдем первый пропущенный блок
        int i = Array.IndexOf(checkBlocks, false);
        if (i >= 0)
        {
            //И заново запросим его у медиа-сервера
            currStatus = string.Format("Блок #{0} отсутствует!", i);
            DataInfo msgToSend = new DataInfo();
            msgToSend.filename = this.FileName;
            msgToSend.blockNum = i;
            byte[] byteData = msgToSend.ToByte();
            server.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, senderRemote, new AsyncCallback(OnSend), null);
            //Пока создавать файл рановато, ждем пока не получим все блоки в целости и сохранности
            return false;
        }
        //string fileName = System.IO.Path.GetTempFileName() + this.FileName; //msgReceived.filename;
        //File.WriteAllBytes(fileName, fileContents);
        //transferProgressBar.Invoke(new Action(() => transferProgressBar.Value = 0));
        //transferProgressBar.Value = 0;
        //Process.Start(fileName);
        return true;
    }
    private string GetMd5Hash(MD5 md5Hash, byte[] input)
    {
        //Вычислим хеш блока данных и вернем его в виде строки
        byte[] data = md5Hash.ComputeHash(input);
        return GetHashString(data);
    }
    private string GetHashString(byte[] hashInput)
    {
        //Конвертнем массив байт в строку 00-FF
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < hashInput.Length; i++)
            sBuilder.Append(hashInput[i].ToString("x2"));
        return sBuilder.ToString();
    }
    private bool VerifyMd5Hash(MD5 md5Hash, byte[] input, string hash)
    {
        //Сравнение хеша в виде строки с полученным байтовым хешем
        string hashOfInput = GetMd5Hash(md5Hash, input);
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        if (0 == comparer.Compare(hashOfInput, hash))
            return true;
        return false;
    }
    private bool HashesEqual(byte[] hash1, byte[] hash2)
    {
        //Сравнение хешей без преобразования в строки
        for (int i = 0; i < 16; i++)
            if (hash1[i] != hash2[i])
                return false;
        return true;
    }
}
