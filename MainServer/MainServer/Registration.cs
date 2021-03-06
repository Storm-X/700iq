﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainServer
{
    public class Registration//регистрация команды
    {

        TcpListener tcpListner;
        bool iswook = true;
        public delegate void AddNewRegistration();
        public AddNewRegistration onAddNewReg, onConnect;
        public MySqlConnection mycon { get; set; }
        public async void Server(DataTable ddt, Data data, RegData team, List<GameinZone> gZone)//Запуск сервера регистрации команд и создания таблицы
        {
            string str = "";
            MySqlCommand cm;
            MySqlDataReader rd;
            teams myTeam;
            DataTable dat;
            byte[] ServerResponseBytes;
            tcpListner = new TcpListener(IPAddress.Any, GetAvailablePort(2049));
            tcpListner.Start();
            while (iswook)//слушаем порт 2050 для приема желающих зарегистрироваться клиентов
            {
                try
                {
                    var tcpClient = await tcpListner.AcceptTcpClientAsync();//устанавливаем соединение с клиентом                
                    var networkStream = tcpClient.GetStream();
                    var buffer = new byte[4096];//читаем сообщение от клиента, 
                    var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    string direction = request.Substring(0, 2);
                    switch (direction)
                    {
                        case "hi":
                            str = "700iq";
                            //ServerResponseBytes = Encoding.UTF8.GetBytes("700iq");
                            //await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                            break;
                        case "tm":
                            /////////////////////////////////////////////
                            double clientVersion = Convert.ToDouble(request.Substring(2, request.Length-2));
                            double serverVersion = Convert.ToDouble(Application.ProductVersion.Replace(".", ""));
                            if (clientVersion!=serverVersion)
                            {
                                str = "oldvr";
                                break;
                            }
                            ///////////////////////////////////////////
                            string teamname = "SELECT name FROM teams ORDER BY name";
                            cm = new MySqlCommand(teamname, mycon);
                            rd = cm.ExecuteReader();
                            dat = new DataTable();
                            using (rd)  //если есть данные, то записываем в таблицу dat
                            {
                                if (rd.HasRows) dat.Load(rd);
                            }
                            var stringArr = dat.AsEnumerable().Select(r => r.Field<string>("Name")).ToArray();
                            str = "Teams" + JsonConvert.SerializeObject(stringArr);
                            //ServerResponseBytes = Encoding.UTF8.GetBytes(str);
                            //await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                            cm.Dispose();
                            rd.Dispose();
                            break;
                        case "rg":
                            //если сообщение от клиента на регистрацию и она возможна, то ..
                            if (team.getReg())
                            {

                                #region проверяем логин и пароль в БД
                                string[] ssi = JsonConvert.DeserializeObject<string[]>(request.Substring(2));
                                //  0            1            2            3        4           5           6            7            8             
                                string strok = "SELECT teams.team_password, teams.rating, teams.name,  teams.id, users.name, users.id, users.rating, users.surname, users.age, teams.city " +
                                   "FROM (teams INNER JOIN compositions ON teams.id=compositions.team_id ) INNER JOIN users ON compositions.user_id=users.id WHERE teams.name='" + ssi[0] + "' and teams.team_password='" + ssi[1] + "'";
                                // string teamlist = "SELECT teams.name FROM teams";
                                //запрос данных из таблицы
                                cm = new MySqlCommand(strok, mycon);
                                rd = cm.ExecuteReader();
                                dat = new DataTable();
                                using (rd)  //если есть данные, то записываем в таблицу dat
                                {
                                    if (rd.HasRows) dat.Load(rd);
                                }
                                #endregion
                                #region проверяем зарегистрирована ли команда на турнир
                                DataTable requestTable = new DataTable();
                                if (dat.Rows.Count > 0)
                                {
                                    string query = "SELECT * FROM (teams INNER JOIN (requests INNER JOIN games ON games.tournament_id=requests.tournament_id) ON teams.id=requests.team_id) WHERE teams.id='" + dat.Rows[0].ItemArray[3] + "' AND games.id = '" + data.idGame + "' AND requests.state = '1'";
                                    cm = new MySqlCommand(query, mycon);
                                    rd = cm.ExecuteReader();
                                    requestTable.Load(rd);
                                }
                                #endregion
                                if (dat.Rows.Count > 0) //если есть данные , то проверяем в таблице зарегистрированных команд
                                {
                                    if ((requestTable.Rows.Count > 0) || (dat.Rows[0].ItemArray[9].ToString() == "0"))
                                    {
                                        DataRow[] datRowN = ddt.Select("Name='" + ssi[0] + "'");
                                    string kluch = dat.Rows[0][2].ToString() + ddt.Rows.Count + DateTime.Now.ToString("hh:mm:ss:fff");
                                    #region если игра началась и команда играла, то заменяем ключ и берем значение data
                           
                                    if (team.getStart() || datRowN.Count() > 0)
                                    {
                                        if (datRowN.Count() > 0)
                                        {
             
                                            bool inGameTeam = false;
                                            for (int i = 0; i < gZone.Count; i++)//перебором игровых зон находим где находилась команда
                                                for (int j = 0; j < 3; j++)
                                                {
                                                    if (gZone[i].data.team[j].uid == Convert.ToInt32(dat.Rows[0][3]))
                                                    {
                                                        gZone[i].data.team[j].kod = kluch;  //заменяем ключ
                                                        if (gZone[i].data.team[j].name == ssi[0]) gZone[i].data.team[j].Resumption = true;
                                                        data = gZone[i].data;               //берем Data из gameZone
                                                        inGameTeam = true;
                                                    }
                                                }
                                            str = inGameTeam ? "regOk" + JsonConvert.SerializeObject(data) : "Error - team not in Game!";
                                        }
                                        else
                                        {
                                            str = "False";
                                        }

                                    }
                                    #endregion
                                    #region если игра еще не началась, то  заносим данные в таблицу 
                                    else
                                    {
                                        if (datRowN.Count() > 0)
                                        {
                                            team.deleteMem(Convert.ToInt32(datRowN[0][1]));
                                            datRowN[0].Delete();
                                        }

                                        DataRow datRow = ddt.NewRow();
                                        datRow["Zone"] = 0;                 //Игровая зона
                                        datRow["Id"] = dat.Rows[0]["id"];      //id команды
                                        datRow["Name"] = dat.Rows[0]["name"];    //имя  команды
                                        datRow["Key"] = kluch;              //уникальный ключ
                                        datRow["Rating"] = dat.Rows[0]["rating"];  //рейтинг команды    
                                        datRow["I-cash"] = 700;             //I-cash
                                        ddt.Rows.Add(datRow);

                                        data.team[0] = new teams();
                                        data.team[0].uid = Convert.ToInt32(dat.Rows[0][3]);
                                        data.team[0].name = dat.Rows[0]["name"].ToString();
                                        data.team[0].rating = Convert.ToInt32(dat.Rows[0]["rating"]);
                                        data.team[0].kod = kluch;

                                        int numbermember = 5;//количество игроков - 5 или меньше
                                        if (dat.Rows.Count < 5) numbermember = dat.Rows.Count;
                                        for (int i = 0; i < numbermember; i++)
                                        {
                                            data.team[0].member[i] = new teams.members();
                                            data.team[0].member[i].N = dat.Rows[i]["name1"].ToString();
                                            data.team[0].member[i].F = dat.Rows[i]["surname"].ToString();
                                            data.team[0].member[i].rait = Convert.ToInt32(dat.Rows[i]["rating1"]);
                                            data.team[0].member[i].id = Convert.ToInt32(dat.Rows[i]["id1"]);
                                            team.addMem(Convert.ToInt32(dat.Rows[i]["id"]), Convert.ToInt32(dat.Rows[i]["id1"]), dat.Rows[i]["name1"].ToString(), dat.Rows[i]["surname"].ToString(), Convert.ToInt16(dat.Rows[i]["rating1"]), 25);
                                        }
                                        onAddNewReg();       //обновление таблицы зарегистрированных команд    
                                        str = "regOk" + JsonConvert.SerializeObject(data);
                                    }
                                    #endregion
                                }
                                    else // команда не зарегалась на турнир
                                    {
                                    str = "noReg";
                                        //ServerResponseBytes = Encoding.UTF8.GetBytes(str);
                                        //await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                                    }
                                }
                                else //пароль или логин не верен!!!!!!!!!!!!! 
                                {
                                    str = "False";
                                }
                                //удаляем ссылки на mySQL
                                cm.Dispose();
                                rd.Dispose();
                            }
                            break;
                        default:
                            if (!team.getReg())
                            {
                                str = "No";
                                //ServerResponseBytes = Encoding.UTF8.GetBytes(str);
                                //await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                            }
                            break;
                    }
                    ServerResponseBytes = Encoding.UTF8.GetBytes(str);
                    await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
                    networkStream.Close();
                    tcpClient.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            tcpListner.Stop();
        }

        public static int GetAvailablePort(int startingPort)
        {
            var portArray = new List<int>();

            var properties = IPGlobalProperties.GetIPGlobalProperties();

            // Ignore active connections
            var connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            // Ignore active tcp listners
            var endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            // Ignore active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (var i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }
        string getSHAHash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            SHA256 md5Hasher = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


    }
}