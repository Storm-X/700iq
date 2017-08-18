using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
            tcpListner = new TcpListener(IPAddress.Any, 2050);
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
                                string strok = "SELECT teams.team_password, teams.rating, teams.name,  teams.id, users.name, users.id, users.rating, users.surname, users.age " +
                                   "FROM teams INNER JOIN users ON teams.id=users.team WHERE teams.name='" + ssi[0] + "' and teams.team_password='" + ssi[1] + "'";
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
                                if (dat.Rows.Count > 0) //если есть данные , то проверяем в таблице зарегистрированных команд
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
                                        datRow[0] = 0;                  //Игровая зона
                                        datRow[1] = dat.Rows[0][3];     //id команды
                                        datRow[2] = dat.Rows[0][2];     //имя  команды
                                        datRow[3] = kluch;              //уникальный ключ
                                        datRow[4] = dat.Rows[0][1];     //рейтинг команды    
                                        datRow[5] = 700;                //Iqesh                                                             
                                        ddt.Rows.Add(datRow);

                                        data.team[0] = new teams();
                                        data.team[0].uid = Convert.ToInt32(dat.Rows[0][3]);
                                        data.team[0].name = dat.Rows[0][2].ToString();
                                        data.team[0].rating = Convert.ToInt32(dat.Rows[0][1]);
                                        data.team[0].kod = kluch;

                                        int numbermember = 5;//количество игроков - 5 или меньше
                                        if (dat.Rows.Count < 5) numbermember = dat.Rows.Count;
                                        for (int i = 0; i < numbermember; i++)
                                        {
                                            data.team[0].member[i] = new teams.members();
                                            data.team[0].member[i].N = dat.Rows[i][4].ToString();
                                            data.team[0].member[i].F = dat.Rows[i][7].ToString();
                                            data.team[0].member[i].rait = Convert.ToInt32(dat.Rows[i][6]);
                                            data.team[0].member[i].id = Convert.ToInt32(dat.Rows[i][5]);
                                            team.addMem(Convert.ToInt32(dat.Rows[i][3]), Convert.ToInt32(dat.Rows[i][5]), dat.Rows[i][4].ToString(), dat.Rows[i][7].ToString(), Convert.ToInt16(dat.Rows[i][6]), 25);
                                        }
                                        onAddNewReg();       //обновление таблицы зарегистрированных команд    
                                        str = "regOk" + JsonConvert.SerializeObject(data);
                                    }
                                    #endregion
                                }
                                else //пароль или логин не верен!!!!!!!!!!!!!
                                {
                                    str = "False";
                                    //ServerResponseBytes = Encoding.UTF8.GetBytes(str);
                                    //await networkStream.WriteAsync(ServerResponseBytes, 0, ServerResponseBytes.Length);
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