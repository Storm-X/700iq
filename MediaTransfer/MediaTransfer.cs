using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

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
    //public ProgressBar transferProgressBar;
    private System.Timers.Timer TimeOutChecker;

    public MediaReceiver(IPAddress ipAddress, int Port)
    {
        //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
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
                //transferProgressBar.BeginInvoke(new Action(() => transferProgressBar.Maximum = blocksCount));
            }
            MD5 md5Hash = MD5.Create();
            //int blockSize = msgReceived.dataBlock.Length; // client.ReceiveBufferSize;
            checkBlocks[msgReceived.blockNum] = false;
            byte[] data = new byte[msgReceived.dataBlock.Length];
            //data.Take(packetSize).ToArray().CopyTo(fileContents, i * blockSize);
            if (!HashesEqual(md5Hash.ComputeHash(msgReceived.dataBlock), msgReceived.blockHash))
                currStatus = ("Контрольная сумма блока неверна!");
            else
            {
                msgReceived.dataBlock.CopyTo(fileContents, maxBufferSize * msgReceived.blockNum);
                checkBlocks[msgReceived.blockNum] = true;
            }
            //transferProgressBar.BeginInvoke(new Action(() => transferProgressBar.PerformStep()));
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
        transferComplete = false;
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
            //transferProgressBar.BeginInvoke(new Action(() => transferProgressBar.Value = 0));
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
        //transferProgressBar.BeginInvoke(new Action(() => transferProgressBar.Value = 0));
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

/// <summary>
/// Класс для отправки информации о пересылаемых байтах 
/// следующих последними в потоке сетевых данных.
/// </summary>
public class DataInfo
{
    //Default constructor
    public DataInfo()
    {
        this.filesize = 0;
        this.blockNum = 0;
        this.blockHash = new byte[16];
        this.dataBlock = null;
        this.filename = null;
    }

    //Конвертируем массив байт в нужную нам структуру
    public DataInfo(byte[] data)
    {
        //Первые четыре байта - размер файла
        this.filesize = BitConverter.ToInt32(data, 0);

        //Следующие четыре - номер передаваемого блока
        this.blockNum = BitConverter.ToInt32(data, 4);

        //Еще 16 - хэш передаваемого блока данных
        this.blockHash = new byte[16];
        Buffer.BlockCopy(data, 8, this.blockHash, 0, 16);

        //Четыре байта - длина имени файла
        int nameLen = BitConverter.ToInt32(data, 24);

        //Если длина имени больше нуля, выдернем имя файла
        if (nameLen > 0)
            this.filename = Encoding.Default.GetString(data, 28, nameLen);
        else
            this.filename = null;

        //Еще четыре байта на длину блока данных
        int dataLen = BitConverter.ToInt32(data, 28 + nameLen);

        //Ну и собственно, само тело передаваемого медиафайла
        if (dataLen > 0)
        {
            this.dataBlock = new byte[dataLen];
            Buffer.BlockCopy(data, 32 + nameLen, this.dataBlock, 0, dataLen);
        }
        else
            this.dataBlock = null;
    }

    //Конвертнем структуру данных в массив байт
    public byte[] ToByte()
    {
        List<byte> result = new List<byte>();

        //Размер файла
        result.AddRange(BitConverter.GetBytes((int)filesize));

        //Номер передаваемого блока данных
        result.AddRange(BitConverter.GetBytes((int)blockNum));

        //Номер передаваемого блока данных
        result.AddRange(blockHash);

        //Add the length of the name
        if (filename != null)
            result.AddRange(BitConverter.GetBytes(filename.Length));
        else
            result.AddRange(BitConverter.GetBytes(0));

        //Имя передаваемого файла
        if (filename != null)
            result.AddRange(Encoding.Default.GetBytes(filename));

        //Номер передаваемого блока данных
        if (dataBlock != null)
        {
            result.AddRange(BitConverter.GetBytes(dataBlock.Length));
            //Номер передаваемого блока данных
            result.AddRange(dataBlock);
        }
        else
            result.AddRange(BitConverter.GetBytes(0));

        return result.ToArray();
    }

    public string filename;
    public int filesize;
    public int blockNum;
    public byte[] blockHash;
    public byte[] dataBlock;
}

