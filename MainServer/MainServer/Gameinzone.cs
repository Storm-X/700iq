﻿using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace MainServer
{
    public class GameinZone
    {
        #region описание переменных

        #region установочные данные
        public Game gm = new Game();          //класс для описание текущего шага игры
        public Data data = new Data();        //класс для описания начльных данных игры
        int[] themeID = new int[7];           //массив id тем вопросов
        string[] theme = new string[7];     //название тем вопросов
        public string usersid;              //перечень id участников команд тройки для корректного подбора вопроса
        RND rn;                             //генератор случайных значений рулетки
        Ruletka rul = new Ruletka();        //класс рулетка для графического отображения
        string[] clientAnswer = new string[3];
        bool flajok;
        #endregion
        #region переменные для сетевого обмена данными
        SQLiteConnection conn;                      //связь с БД вопросов
        MySqlConnection mycon;                      //связь с БД команд 
        IPEndPoint[] endpoint = new IPEndPoint[3];  //таблица Endpoint команд 
        UdpClient udp = new UdpClient();
        byte[] bytes;
        #endregion
        #region вспомогательные переменные
        int Takt = 0;       //текущий такт выполнения айкона     
        int questID;
        private string key = "Qade123asdasdasdqwewqeqw423412354232343253***????///";//id вопроса
        string answerQ;     //ответ команды
        bool correct;       //правильность ответа
        bool endOfIqon;     //окончание айкона
        string tema;        //id темы или список id тем для построения запроса к БД 
        int[] stavka = new int[3] { 25, 25, 25 };          //массив ставок команд      
        string[] otvet = new string[3];     //массив ответов команд
        bool[] ok = new bool[3];            //массив логических триггеров
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();   //таймер ожидания хода команды
        Timer tmOtvet = new Timer();
        Object obj = new Object();
        private Timer deadLinetmr = new Timer();
        bool tmAktiv = false;
        bool tmOtvetAktiv = false;
        public bool stopGm = false;
        public GameStatistic gs;
        private DateTime? deadLine = null;
        //public List<int> users = new List<int>();

        #endregion

        TextBox txb = new TextBox();

        #endregion

        public GameinZone(RND Rn, SQLiteConnection Conn, MySqlConnection mycn, UdpClient udpserver)
        {
            rn = Rn;
            conn = Conn;
            mycon = mycn;
            udp = udpserver;
        }
        public void startGame(bool vozobnov)
        {
            #region задание начальных значений 
            endOfIqon = false;
            stavka = stavka.Select(x => x = 25).ToArray();
            //Array.Clear(stavka, 0, stavka.Length);
            Array.Clear(ok, 0, ok.Length);
            Array.Clear(clientAnswer, 0, clientAnswer.Length);
            //for (int i = 1; i < 3; i++) { stavka[i] = 0; ok[i] = false; }

            if (!vozobnov) gm.iCon = 1;
            //else gm.iCon++;
            gm.step = 0;
            gm.Cell = 0;
            gm.theme = 0;
            gm.activeTable = 1;
            gm.quest = "";
            //gm.o1 = 1;
            //gm.o2 = 2;
            //gm.o3 = 3;
            #endregion
            deadLine = DateTime.Now.AddSeconds(600);
            tm.Tick += Tm_Tick;
            tmOtvet.Tick += TmOtvet_Tick;
            //deadLinetmr.Tick += DeadLinetmr_Tick;
            //deadLinetmr.Start();

        }

        private void DeadLinetmr_Tick(object sender, EventArgs e)
        {
            if (deadLine <= DateTime.Now)
                nextTakt();
        }

        public void setThemes(int[] themeID, string[] theme)                            //получение тем вопросов для строки
        {
            this.themeID = themeID;
            this.theme = theme;
            tema = themeID[1].ToString();
            for (int i = 2; i < 7; i++) tema += ", " + themeID[i];
        }
        private void Tm_Tick(object sender, EventArgs e)
        {
            tm.Stop();
            //Array.ForEach(stavka, value => value = (value == 0) ? 25 : value);
            //stavka[0]  & stavka[1] & stavka[2]
            nextTakt();
        }
        private void TmOtvet_Tick(object sender, EventArgs e)
        {
            tmOtvet.Stop();
            gm.team[gm.activeTable - 1].answer = "Нет ответа.";
            gm.team[gm.activeTable - 1].correct = false;
            correct = false;
            nextTakt();
        }
        public string[] otvetInfo()                                                     //запрос информации о вопросе от тройки
        {
            string[] info = new string[6];
            info[0] = gm.zoneUID.ToString();
            info[1] = gm.activeTable.ToString();
            info[2] = questID.ToString();
            info[3] = theme[gm.theme];
            info[4] = gm.quest;
            info[5] = answerQ;
            return info;

        }
        public void wait(int step, int table, IPEndPoint point)                         //запрос клиента в режиме ожидания
        {
            endpoint[table] = point;
            switch (step)
            {
                case 1:
                    gm.step = 1;
                    //if (Takt == 3)
                        //nextTakt();
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 4:
                    if (Takt == 3) return;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);

                    break;
                case 5:
                    if (Takt == 4) return;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 6:
                    if (Takt == 5) return;
                    // gm.step = 7;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    //gm.step = 1;
                    break;
            }
        }
        public void Update(int step, int table, string otv, int stav, IPEndPoint point) //обновление ставки и команды готов
        {
            endpoint[table] = point;
            switch (step)
            {
                #region обработка команд готов
                case 0:
                case 1:
                    if (gm.step < 2)
                    {
                        //if (otv == null)
                        //    otv = "";
                        ok[table] = otv.Equals("gotov", StringComparison.OrdinalIgnoreCase);
                        if ((ok[0] & ok[1] & ok[2]) || deadLine <= DateTime.Now)
                        {
                            ContinueGame();
                            nextTakt();
                            Array.Clear(ok, 0, ok.Length);
                        }
                        else //if (ok[0] & ok[1] & ok[2] || Takt != 0)
                        {
                            bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                            udp.Send(bytes, bytes.Length, point);
                        }
                    }
                    break;
                #endregion
                #region обработка ставок
                case 2:
                    if (gm.step == step)
                    {
                        //if(stavka[table] == 0) 
                        gm.team[table].stavka = stav;
                        //if (otv == null)
                        //    otv = "";
                        if (otv.Equals("stavka", StringComparison.OrdinalIgnoreCase))
                        {
                            Array.Clear(ok, 0, ok.Length);
                            deadLine = DateTime.Now.AddSeconds(30);
                            //Send2All("ogg");
                        }
                        else
                            ok[table] = otv.Equals("st_finish", StringComparison.OrdinalIgnoreCase);
                        if (Takt == 1 && (Array.TrueForAll(ok, value => value) || deadLine <= DateTime.Now))
                            nextTakt();
                        else
                        {
                            bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                            udp.Send(bytes, bytes.Length, point);
                        }
                    }
                    break;
                case 3:
                    if (gm.step == step)
                    {
                        if (Takt == 3 || deadLine <= DateTime.Now)
                        {
                            tmOtvet.Stop();
                            nextTakt();
                        }
                        else
                        {
                            bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                            udp.Send(bytes, bytes.Length, point);
                        }
                    }
                    break;
                default:
                    break;
                    #endregion
            }
            //bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
            //udp.Send(bytes, bytes.Length, point);
        }
        public string startGM()
        {
            return JsonConvert.SerializeObject(gm);
        }
        public bool verify(string kluch, int numberTable)   //проверка коректности ключа
        {
            if (data.team[numberTable].kod == kluch) return true;
            return false;
        }
        public string ReadString(string txtQuery)
        {

            using (MySqlCommand cmd = new MySqlCommand(txtQuery, mycon))
            {
                object result = cmd.ExecuteScalar();
                return (result == null ? "" : result.ToString());
            }
        }
        void log()                                          //сохранение данных - логирование
        {
            SendLog sLog = new SendLog();
            //string vopr = gm.quest;
            //gm.quest = ""; //Если не хотим писать текст вопроса в лог
            sLog.gmLog = gm;
            sLog.dataLog = data;
            sLog.idTheme = themeID;
            sLog.Themes = theme;
            sLog.usersid = usersid;
            //sLog.gmLog.iCon--; //Записываем в базу данные не на начало следующего айкона, а на конец текущего.

            string sql = " insert into logs (gameid, zone, iqon_num, command) value (" + data.idGame + ", " + data.GameZone + ", " +
                sLog.gmLog.iCon + ",'" + JsonConvert.SerializeObject(sLog) + "')";

            sql = sql.Replace("\\", "\\\\");
            //mycon.Open();
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            cmd.ExecuteNonQuery(); 
            try
            {
                //определение есать ли у вопроса владелец, если есть владелец то внести в таблицу Payment////////////////////////////////////
                double WiQash = 0;
                int ownerId = Convert.ToInt32(ReadString("SELECT user_id from questOwner WHERE quest_id = " + gm.idQuest + ""));
                for (int i = 0; i < 3; i++)
                {
                    if (gm.team[i].correct)
                    {
                        switch (gm.team[i].answerOrder)
                        {
                            case 0:
                                WiQash = 0; break;
                            case 1:
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (gm.team[j].answerOrder == 0)
                                        {
                                            WiQash = Math.Round(gm.team[j].stavka * 0.03);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (gm.team[j].answerOrder == 0 || gm.team[j].answerOrder == 1)
                                        {
                                            WiQash += gm.team[j].stavka;
                                        }
                                    }
                                    WiQash = Math.Round(WiQash * 0.04);
                                }
                                break;
                        }
                    }
                }
                if ((gm.team[0].correct == gm.team[1].correct == gm.team[2].correct == false) && (gm.Cell != 0)) WiQash = Math.Round((gm.team[0].stavka + gm.team[1].stavka + gm.team[2].stavka) * 0.05);
                cmd = new MySqlCommand("insert into questPayment (gameid, zone, userid, questid, iqash) value(" + data.idGame + ", " + data.GameZone + "," + ownerId + "," + gm.idQuest + "," + WiQash + ")", mycon);
                cmd.ExecuteNonQuery();
                //конец записи в Payment //////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch{ };


            var gmMembers = data.team.SelectMany(x => x.member.Where(c => c != null));  //отберем всех участников игровой тройки
            foreach (var tUser in gmMembers)                                            //и занесем в базу "засвеченный" вопрос
            {
                sql = String.Format("INSERT INTO i_see (user_id, quest_id) values ({0}, {1})", tUser.id, gm.idQuest);
                SQLiteCommand cml = new SQLiteCommand(sql, conn);
                cml.ExecuteNonQuery();
            }

            this.gs.iCon.Text = (sLog.gmLog.iCon + 1).ToString();
            sLog = null;


        }
        public bool inGameZone(string id)
        {
            for (int i = 0; i < 3; i++)
            {
                if (data.team[i].kod == id)
                    return true;
            }
            return false;
        }
        public bool getOtvet(int table, string otv, byte step, IPEndPoint point)         //получение ответа команды
        {
            if (gm.step < 3 || gm.step > 6)  //???????????????????????????????????????????????????????????????????????????????
                return false;
            switch (step)
            {
                case 4:
                    if (Takt == 3) break;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 5:
                    if (Takt == 4) break;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 6:
                    if (Takt == 5) break;
                    gm.step = 7;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                default:
                    return false;
            }
            if (gm.activeTable != table + 1) return false;//если ответ не от активного стола -игнор
            if (gm.team[table].answer != "") return false;//если ответ уже есть, то - игнор

            tmOtvet.Stop();//останавливаем счетчик ожидания ответа.
            if (otv == "") //проверяем пришедший ответ, если ответ - пусто
            {
                correct = false;
                gm.team[table].answer = "Нет ответа."; // то записываем ответ - нет ответа
                gm.team[table].correct = false; // то записываем ответ - нет ответа
                nextTakt();
                return false;//сообщаем, что не нужно отправлять ответ модератору
            }

            gm.team[table].answer = otv.Trim(); //запоминаем ответ
            return true;//сообщаем, что надо проверить ответ
        }
        public void checkOtvet(bool right)
        {
            tmOtvet.Stop();
            correct = right;
            nextTakt();
        }
        public void stopGame()
        {
            stopGm = true;
            if (tm.Enabled)
            {
                tm.Stop();
                tm.Enabled = false;
                tmAktiv = true;
            }
            if (tmOtvet.Enabled)
            {
                tmOtvet.Stop();
                tmOtvet.Enabled = false;
                tmOtvetAktiv = true;
            }
        }
        public void ContinueGame()
        {
            stopGm = false;
            if (tmAktiv)
            {
                tm.Start();
                tmAktiv = false;
            }
            if (tmOtvetAktiv)
            {
                tmOtvet.Start();
                tmOtvetAktiv = false;
            }
        }

        public string bin2Hex(string strBin)

        {
            byte[] ba = Encoding.Default.GetBytes(strBin);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        [ComVisibleAttribute(true)]
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        [PermissionSetAttribute(SecurityAction.InheritanceDemand, Name = "FullTrust")]
        void nextTakt()
        {
            Game.Teames[] currTeam;
            //deadLine = DateTime.Now.AddMinutes(3);
            if (gm.iCon > 12)
            {
                gm.step = 10;
                tm.Stop();
                //for (int s = 0; s < 3; s++) udp.Send("ii" + JsonConvert.SerializeObject(gm), IP[s]);
            }
            else
            {
                //deadLine = DateTime.Now.AddMinutes(4);
                switch (Takt)
                {
                    #region 0 такт - определение темы вопроса. Ожидание ставок от команд
                    case 0:
                        //Array.Clear(ok, 0, ok.Length);
                        deadLine = DateTime.Now.AddSeconds(70);
                        Takt = 1;
                        string strZapros;
                        SQLiteCommand cmdl;
                        int questsCount = 0;
                        gm.step = 2;
                        //deadLine = null;
                        do
                        {
                            gm.Cell = rn.rnd();
                            gm.theme = (byte)((gm.Cell + 5) / 6);
                            if (gm.theme != 0)
                            {
                                //Проверим, сколько в выбранной теме осталось "незасвеченных" вопросов для данной игровой тройки
                                //и при отсутствии таковых сменим тему на ту, в которой такие вопросы остались.
                                strZapros = "select count(quests.id) as qCount " +
                                             "from quests left join i_see on (quests.id = i_see.quest_id and i_see.user_id in (" + usersid + ")) " +
                                             "where i_see.id is null and quests.themeId = " + themeID[gm.theme].ToString();
                                cmdl = new SQLiteCommand(strZapros, conn);
                                try
                                {
                                    questsCount = Convert.ToInt32(cmdl.ExecuteScalar());
                                    if (questsCount < 3) MessageBox.Show("Количество 'незасвеченных' вопросов в теме \"" + theme[gm.theme] + "\" - " + questsCount +
                                        " шт. " + Environment.NewLine + (questsCount == 0 ? "Будет произведена замена темы!" : "Добавьте в тему несколько новых вопросов!"), 
                                        "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                catch (SQLiteException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        } while (questsCount == 0 && gm.theme != 0);
                        // txb.Text += "ogg" + gm.step;
                        //tm.Start();
                        //Установим размер начальных ставок в минимальное значение
                        gm.team.ToList().ForEach(x => x.stavka = 25);
                        break;
                    #endregion
                    #region  1 такт - обработка полученных ставок, определение очереди, получение вопроса
                    case 1:
                        Array.Clear(ok, 0, ok.Length);
                        //deadLine = DateTime.Now.AddSeconds(25);
                        gm.step = 3;
                        gm.Cell = rn.rnd();
                        //if (gm.Cell == 0) gm.Cell = 1;
                        /* if (gm.Cell == 0)
                         {
                             gm.step = 2;
                             //gm.iCon++;                  
                             //Takt = 0;
                             endOfIqon = true;
                             txb.Text += "ogg-zerro" + gm.step;
                             //tmSync.Start();
                             for (int i = 0; i < 3; i++) ok[i] = false;
                             break;
                         }*/
                        //stavka = stavka.Select(value => (value == 0) ? 25 : value).ToArray();

                        gm.team.ToList().ForEach(x =>
                        {
                            x.stavka = x.stavka == 0 ? 25 : x.stavka;
                            x.iQash -= x.stavka;
                            x.answer = "";
                            x.correct = false;
                        });
                        
                        /*for (int i = 0; i < 3; i++)
                        {
                            //stavka[i] = (stavka[i] == 0) ? 25 : stavka[i];
                            // if (stavka[i] > gm.team[i].iQash) stavka[i] = gm.team[i].iQash;   ВАЖНО!!!
                            if(gm.team[i].stavka == 0)
                                gm.team[i].stavka = 25;
                            gm.team[i].iQash -= gm.team[i].stavka;
                            gm.team[i].answer = "";
                            gm.team[i].correct = false;
                        }*/

                        int[] st = gm.team.Select(x => x.stavka).ToArray();  //{ gm.team[0].stavka, gm.team[1].stavka, gm.team[2].stavka };
                        byte[] bAnsOrder = rul.ResponsePriority(gm.Cell, st);

                        //if (gm.Cell != 0)
                        //{
                            for (byte i = 0; i < gm.team.Count(); i++)
                                gm.team[bAnsOrder[i] - 1].answerOrder = i;
                        //}
                        
                        gm.activeTable = bAnsOrder[0];

                        #region получение вопроса
                        string temaID;
                        if (gm.theme != 0) temaID = themeID[gm.theme].ToString();
                        else temaID = tema; //если тема кот в мешке, то выбираем все темы

                        string zaprosSpiskaVoprosov = "select quests.id " +
                                     "from quests left join i_see on (quests.id = i_see.quest_id and i_see.user_id in (" + usersid + ")) " +
                                     "where i_see.id is null and quests.themeId in (" + temaID + ") " +
                                     "GROUP BY quests.id ORDER BY quests.id LIMIT 10";

                        SQLiteCommand cml = new SQLiteCommand(zaprosSpiskaVoprosov, conn);
                        DataTable dtVopros = new DataTable();


                        using (SQLiteDataReader sqr = cml.ExecuteReader())
                        {
                            dtVopros.Load(sqr);
                        }
                        //questID = (dtVopros.Rows.Count * rn.rnd()) / 37;//определяем случайно id вопроса из списка в таблице dtVopros
                        //questID = Convert.ToInt32(dtVopros.Rows[questID][0]);//questID - id случайного вопроса
                        questID = Convert.ToInt32(dtVopros.Rows[0][0]); //Id первого незасвеченного вопроса
                                                                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                                                        /*    string listOfQuestions = "select quests.text from quests";
                                                                            SQLiteCommand cm = new SQLiteCommand(listOfQuestions, conn);
                                                                            SQLiteDataReader rd = cm.ExecuteReader();
                                                                            DataTable dat = new DataTable();
                                                                            using (rd)  //если есть данные, то записываем в таблицу dat
                                                                            {
                                                                                if (rd.HasRows) dat.Load(rd);
                                                                            }
                                                                            var stringArr = dat.AsEnumerable().Select(r => r.Field<string>("text")).ToArray();

                                                                            string key = "Qade123asdasdasdqwewqeqw423412354232343253***????///";
                                                                            string cryptMessage;
                                                                            for (int i = 0; i < stringArr.Length; i++)
                                                                            {
                                                                                 cryptMessage = Crypt.Encrypt(stringArr[i], key);
                                                                                 stringArr[i] = cryptMessage;
                                                                            }*/
                                                                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            

                        string zaprocVoprosa = "select quests.text, answer, IFNULL(media,'') " +
                                               "from quests where id = "+ questID;

                        cml = new SQLiteCommand(zaprocVoprosa, conn);
                        SQLiteDataReader reader = cml.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            gm.quest = Crypt.Decrypt(reader.GetString(0), key);
                            gm.rightAnswer = Crypt.Decrypt(reader.GetString(1), key);
                            if (!reader.IsDBNull(1)) answerQ = Crypt.Decrypt(reader.GetString(1), key);
                            gm.idQuest = questID;//надо ли его посылать??
                            gm.media = reader.GetString(2);
                        }
                        #endregion
                        Takt = 3;
                        //log();
                       // txb.Text += "ogg" + gm.step;
                        tmOtvet.Interval = 90000; //запускаем таймер с ожиданием ответа 1 команды
                        deadLine = DateTime.Now.AddSeconds(90);
                        tmOtvet.Start();
                        correct = false;
                        break;
                    #endregion
                    #region 2 такт - обработка ответа первой команды
                
                    case 3://ответ первой команды


                        if (gm.Cell == 0)
                        {
                            tmOtvet.Stop();
                            gm.step = 0;
                            endOfIqon = true;
                           // Array.Clear(ok, 0, ok.Length);
                        }
                        else
                        {
                            gm.step = 5;
                            currTeam = gm.team.OrderBy(x => x.answerOrder).ToArray();
                            currTeam[0].correct = correct;
                            //gm.team[gm.o1 - 1].correct = correct;
                            if (correct)
                            {
                                ////Takt = -1;
                                //gm.team[gm.o1 - 1].iQash += 4 * gm.team[gm.o1 - 1].stavka;
                                currTeam[0].iQash += 3 * currTeam[0].stavka;
                                //gm.iCon++;
                                endOfIqon = true;
                                //for (int i = 0; i < 3; i++) ok[i] = false;
                                Array.Clear(ok, 0, ok.Length);
                            }
                            else//если ответ не верен запускаем таймер для приема ответа 2-ой команды
                            {
                                tmOtvet.Stop();
                                tmOtvet.Interval = 40000;
                                deadLine = DateTime.Now.AddSeconds(50);
                                tmOtvet.Start();
                                gm.activeTable = currTeam[1].table; //gm.o2;
                            }
                            Takt = 4;
                            //log();
                          //  txb.Text += "ogg" + gm.step;

                        }
                        

                        break;
                    #endregion
                    #region 3 такт - обработка ответ второй команды (если дойдет дело)
                    case 4://ответ второй команды 

                        gm.step = 6;
                        currTeam = gm.team.OrderBy(x => x.answerOrder).ToArray();
                        currTeam[1].correct = correct;
                        //gm.team[gm.o2 - 1].correct = correct;

                        if (correct)
                        {
                            //Takt = -1;
                            //gm.iCon++;
                            currTeam[1].iQash += 2 * currTeam[1].stavka;
                            //gm.team[gm.o2 - 1].iQash += 2 * gm.team[gm.o2 - 1].stavka;
                            endOfIqon = true;
                            //for (int i = 0; i < 3; i++) ok[i] = false;
                            Array.Clear(ok, 0, ok.Length);
                        }
                        else //если ответ не верен запускаем таймер для приема ответа 3-ой команды
                        {
                            gm.activeTable = currTeam[2].table;
                            tmOtvet.Stop();
                            tmOtvet.Interval = 40000;
                            deadLine = DateTime.Now.AddSeconds(50);
                            tmOtvet.Start();
                        }
                        Takt = 5;
                        //log();
                     //   txb.Text += "ogg" + gm.step;
                        break;
                    #endregion
                    #region 4 такт - обработка ответ третьей команды (если дойдет дело)
                    case 5://ответ третьей команды
                        gm.step = 7;
                        currTeam = gm.team.OrderBy(x => x.answerOrder).ToArray();
                        currTeam[2].correct = correct;
                        //gm.team[gm.o3 - 1].correct = correct;
                        if (correct)
                            currTeam[2].iQash += currTeam[2].stavka;
                            //gm.team[gm.o3 - 1].iQash += gm.team[gm.o3 - 1].stavka;

                        endOfIqon = true;
                        Array.Clear(ok, 0, ok.Length);
                        //log();
                        //txb.Text += "ogg" + gm.step;
                        break;
                        #endregion
                }
            }

            //Отправим сообщение всем столам данной игровой тройки

            if (gm.step != 0)
            {
                //if (gm.Cell != 0)
                Send2All("ogg");
            }
            if (endOfIqon)
            {
                endOfIqon = false;
                //gm.step = 7;
                log();
                stavka = stavka.Select(x => x = 25).ToArray();
                //Array.Clear(stavka, 0, stavka.Length);
                Array.Clear(ok, 0, ok.Length);
                
                deadLine = DateTime.Now.AddSeconds(150);
                gs.stopButton.PerformClick();
                gm.iCon++;
                gm.step = 1;
                Takt = 0;

                Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(7000);
                    Send2All("ogg");
                });
                //System.Threading.Thread.Sleep(7000);
                //stopGame();
                if (gm.iCon > 12)
                {
                    gm.Cell = rn.rnd();
                    gm.quest = "";
                    gm.theme = 0;
                   
                    //////////////////////////////////////Перерасчет рейтинга на конец игры////////////////////////////////////////////////////////////
                    var mesta = ResponsePriority(gm.Cell, gm.team.Select(x => x.iQash << 2).ToArray());
                    for(int i = 0; i < 3; i++) {
                        gm.team[mesta[i]-1].answerOrder = (byte)i;
                        gm.team[mesta[i]-1].answer = "";
                        gm.team[mesta[i]-1].correct = false;
                        gm.team[mesta[i]-1].stavka = 0;
                    }

                    Ratings rating = new Ratings(this, mycon, mesta);
                    var rat = rating.getRatings();
                    Array.Sort(rat);
                    Array.Reverse(rat);
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (teams.members member in this.data.team[mesta[i] - 1].member)
                        {
                            if (member != null)
                            {
                                member.rait += rat[i];
                                string sql = "UPDATE users SET rating=" + member.rait + " WHERE id=\"" + member.id + "\"";
                                MySqlCommand cm = new MySqlCommand(sql, mycon);
                                cm.ExecuteNonQuery();

                            }
                        }

                    }
                    
                    log();

                    WebBrowser webBrowser = new WebBrowser();
                    webBrowser.ScriptErrorsSuppressed = true;
                    String requestString = String.Format("700iq.by/{0}/{1}", bin2Hex("calc_game_stat"), bin2Hex(String.Format("game_id={0}&zone_id={1}", data.idGame, data.GameZone)));
                    webBrowser.Navigate(requestString);
                    string logs = "select * from logs where gameid = " + data.idGame + " AND zone = " + data.GameZone;
                    MySqlCommand cm1 = new MySqlCommand(logs, mycon);
                    MySqlDataReader rd = cm1.ExecuteReader();
                    DataTable dat = new DataTable();
                    using (rd)  //если есть данные, то записываем в таблицу dat
                    {
                        if (rd.HasRows)
                            dat.Load(rd);
                    }

                    var loggs = dat.AsEnumerable().Select(r => r.Field<string>("command")).ToArray();
                    SendLog [] jsonobj = new SendLog[loggs.Length];
                    for (int i = 0; i < loggs.Length; i++)
                    {
                        jsonobj[i] = JsonConvert.DeserializeObject<SendLog>(loggs[i]);
                     
                    }
                    Questions qt = new Questions();
                    
                    for (int i = 0; i < loggs.Length-1; i++)
                    {
                        qt.quest[i] = jsonobj[i].gmLog.quest;
                        qt.answer[i] = jsonobj[i].gmLog.rightAnswer;
                    }
                    string str = JsonConvert.SerializeObject(qt);
                    bytes = Encoding.UTF8.GetBytes("lst" + JsonConvert.SerializeObject(qt));
                    for (int i = 0; i < 3; i++)
                    {
                        if (endpoint[i] is IPEndPoint)
                            udp.Send(bytes, bytes.Length, endpoint[i]);
                    }
                }
            }

            //if (gm.Cell == 0)
            //Send2All("ogg");

        }
        public void RestartIqon()
        {
            tmOtvet.Stop();
            stavka = stavka.Select(x => x = 25).ToArray();
            //Array.Clear(stavka, 0, stavka.Length);
            Array.Clear(ok, 0, ok.Length);
            deadLine = DateTime.Now.AddSeconds(40);
            if (Takt >= 3 && Takt <= 5)
            {
                gm.team.ToList().ForEach(x => x.iQash += x.stavka);   //x.stavka = 25;
                //for (int i=0;i<3;i++){
                //    gm.team[i].iQash += gm.team[i].stavka;
                //    gm.team[i].stavka = 25;
                //}
            }
            gm.step = 1;
            Send2All("rgg");
            //gm.step = 0;
            Takt = 0;
            //Send2All("ogg");
        }

        internal void TurboIt()
        {
            this.deadLine = DateTime.Now;
        }

        private void Send2All(string command)
        {
            bytes = Encoding.UTF8.GetBytes(command + JsonConvert.SerializeObject(gm));
            for (int i = 0; i < 3; i++)
            {
                if (endpoint[i] is IPEndPoint) udp.Send(bytes, bytes.Length, endpoint[i]);
            }
        }
        public int[] ResponsePriority(int indexCell, int[] Rates)
        {
            int[] Roulet = { 0, 1, 3, 2, 1, 3, 2, 1, 2, 3, 1, 2, 1, 3, 2, 3, 1, 3, 2, 1, 2, 3, 1, 2, 1, 3, 2, 3, 1, 2, 3, 1, 3, 2, 1, 2, 3, 1, 3 };
            string Sector = " | " + Roulet[indexCell] + " | " + Roulet[indexCell + 1] + " | " + Roulet[indexCell + 2] + " |";
            try
            {
                Rates[Roulet[indexCell++] - 1] += 2;
                Rates[Roulet[indexCell] - 1]++;
                var dictRates = Enumerable.Range(0, Rates.Length).ToDictionary(x => ++x, x => Rates[x]).OrderByDescending(pair => pair.Value);
                return dictRates.Select(pair => pair.Key).ToArray();
            }
            catch
            {
                Console.WriteLine("Где-то косячок-с...");
            }
            return null;
        }
    }

}
