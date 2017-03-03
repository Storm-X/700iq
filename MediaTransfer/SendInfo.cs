using System;
using System.Collections.Generic;
using System.Text;

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
