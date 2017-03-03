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
            //���������, ��� ����� �������� �� ��������� UDP
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //�������� ����� IP ������� � ����� ������� ���� 8080 (�� ���������)
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            //�������� ���� ����� � �������
            client.Bind(ipEndPoint);
            IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
            //���������� epSender �������������� ��������������� �������
            EndPoint epSender = (EndPoint)ipeSender;
            //�������� ������
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

        //�������������� ������� ������, ���������� �� ������������ � ������� ��� ��� ����� ������� ������
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

            string infoStr = (msgReceived.blockNum == 0) ? string.Format("�������� ����� '{0}' - {1} ����. ����� {2} �����/������).", sendInfo.filename, sendInfo.filesize, blocksCount) :
                                                           string.Format("���� '{0}' - ��������� �������� ����� #{1}", sendInfo.filename, msgReceived.blockNum);
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
                Debug.WriteLine("��������!");
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
            //�������������� ������� ������, ���������� �� ������������ � ������� ��� ��� ����� ������� ������
            DataInfo msgReceived = new DataInfo(byteData);
            int blocksCount = (int)Math.Ceiling((float)msgReceived.filesize / (float)maxBufferSize); // blockSize);
            if (msgReceived.blockNum == 0)
            {
                currStatus = string.Format("----���������� � ����� ��������! ������ �����: {0} ����", msgReceived.filesize);
                fileContents = new byte[msgReceived.filesize];
                checkBlocks = new Boolean[blocksCount];
                //transferProgressBar.BeginInvoke(new Action(() => transferProgressBar.Maximum = blocksCount));
            }
            MD5 md5Hash = MD5.Create();
            //int blockSize = msgReceived.dataBlock.Length; // client.ReceiveBufferSize;
            byte[] data = new byte[msgReceived.dataBlock.Length];
            //data.Take(packetSize).ToArray().CopyTo(fileContents, i * blockSize);
            if (!HashesEqual(md5Hash.ComputeHash(msgReceived.dataBlock), msgReceived.blockHash))
                currStatus = ("����������� ����� ����� �������!");
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
            currStatus = "������� �������� ����� �� �������...";
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
        //��������� ������ ��������...
        TimeOutChecker.Stop();
        //������ ������ ����������� ����
        int i = Array.IndexOf(checkBlocks, false);
        if (i >= 0)
        {
            //� ������ �������� ��� � �����-�������
            currStatus = string.Format("���� #{0} �����������!", i);
            DataInfo msgToSend = new DataInfo();
            msgToSend.filename = this.FileName;
            msgToSend.blockNum = i;
            byte[] byteData = msgToSend.ToByte();
            server.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, senderRemote, new AsyncCallback(OnSend), null);
            //���� ��������� ���� ��������, ���� ���� �� ������� ��� ����� � ������� � �����������
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
        //�������� ��� ����� ������ � ������ ��� � ���� ������
        byte[] data = md5Hash.ComputeHash(input);
        return GetHashString(data);
    }
    private string GetHashString(byte[] hashInput)
    {
        //���������� ������ ���� � ������ 00-FF
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < hashInput.Length; i++)
            sBuilder.Append(hashInput[i].ToString("x2"));
        return sBuilder.ToString();
    }
    private bool VerifyMd5Hash(MD5 md5Hash, byte[] input, string hash)
    {
        //��������� ���� � ���� ������ � ���������� �������� �����
        string hashOfInput = GetMd5Hash(md5Hash, input);
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        if (0 == comparer.Compare(hashOfInput, hash))
            return true;
        return false;
    }
    private bool HashesEqual(byte[] hash1, byte[] hash2)
    {
        //��������� ����� ��� �������������� � ������
        for (int i = 0; i < 16; i++)
            if (hash1[i] != hash2[i])
                return false;
        return true;
    }
}

/// <summary>
/// ����� ��� �������� ���������� � ������������ ������ 
/// ��������� ���������� � ������ ������� ������.
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

    //������������ ������ ���� � ������ ��� ���������
    public DataInfo(byte[] data)
    {
        //������ ������ ����� - ������ �����
        this.filesize = BitConverter.ToInt32(data, 0);

        //��������� ������ - ����� ������������� �����
        this.blockNum = BitConverter.ToInt32(data, 4);

        //��� 16 - ��� ������������� ����� ������
        this.blockHash = new byte[16];
        Buffer.BlockCopy(data, 8, this.blockHash, 0, 16);

        //������ ����� - ����� ����� �����
        int nameLen = BitConverter.ToInt32(data, 24);

        //���� ����� ����� ������ ����, �������� ��� �����
        if (nameLen > 0)
            this.filename = Encoding.Default.GetString(data, 28, nameLen);
        else
            this.filename = null;

        //��� ������ ����� �� ����� ����� ������
        int dataLen = BitConverter.ToInt32(data, 28 + nameLen);

        //�� � ����������, ���� ���� ������������� ����������
        if (dataLen > 0)
        {
            this.dataBlock = new byte[dataLen];
            Buffer.BlockCopy(data, 32 + nameLen, this.dataBlock, 0, dataLen);
        }
        else
            this.dataBlock = null;
    }

    //���������� ��������� ������ � ������ ����
    public byte[] ToByte()
    {
        List<byte> result = new List<byte>();

        //������ �����
        result.AddRange(BitConverter.GetBytes((int)filesize));

        //����� ������������� ����� ������
        result.AddRange(BitConverter.GetBytes((int)blockNum));

        //����� ������������� ����� ������
        result.AddRange(blockHash);

        //Add the length of the name
        if (filename != null)
            result.AddRange(BitConverter.GetBytes(filename.Length));
        else
            result.AddRange(BitConverter.GetBytes(0));

        //��� ������������� �����
        if (filename != null)
            result.AddRange(Encoding.Default.GetBytes(filename));

        //����� ������������� ����� ������
        if (dataBlock != null)
        {
            result.AddRange(BitConverter.GetBytes(dataBlock.Length));
            //����� ������������� ����� ������
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

