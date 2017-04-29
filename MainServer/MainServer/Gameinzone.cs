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
        int[] stavka = new int[3] { 0, 0, 0 };          //массив ставок команд      
        string[] otvet = new string[3];     //массив ответов команд
        bool[] ok = new bool[3];            //массив логических триггеров
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();   //таймер ожидания хода команды
        Timer tmOtvet = new Timer();                                        //таймер ожидания ответа команды
        bool tmAktiv = false;
        bool tmOtvetAktiv = false;
        public bool stopGm = false;
        public GameStatistic gs;
        private DateTime deadLine;
        //public List<int> users = new List<int>();

        #endregion

        TextBox txb = new TextBox();

        #endregion

        public GameinZone(RND Rn, SQLiteConnection Conn, MySqlConnection mycn, UdpClient udpserver, TextBox tx)
        {
            rn = Rn;
            conn = Conn;
            mycon = mycn;
            udp = udpserver;
            txb = tx;
        }
        public void startGame(bool vozobnov)
        {
            #region задание начальных значений 
            endOfIqon = false;
            Array.Clear(stavka, 0, stavka.Length);
            Array.Clear(ok, 0, ok.Length);
            //for (int i = 1; i < 3; i++) { stavka[i] = 0; ok[i] = false; }

            if (!vozobnov) gm.iCon = 1;
            //else gm.iCon++;
            gm.step = 0;
            gm.Cell = 0;
            gm.theme = 0;
            gm.activeTable = 1;
            gm.quest = "";
            gm.o1 = 1;
            gm.o2 = 2;
            gm.o3 = 3;
            #endregion
            deadLine = DateTime.Now.AddMinutes(3);
            tm.Tick += Tm_Tick;
            tmOtvet.Tick += TmOtvet_Tick;

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
            nextTakt();
        }
        private void TmOtvet_Tick(object sender, EventArgs e)
        {
            tmOtvet.Stop();
            gm.team[gm.activeTable - 1].answer = "Нет ответа.";
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
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 4:
                    if (Takt == 2) return;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);

                    break;
                case 5:
                    if (Takt == 3) return;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 6:
                    if (Takt == 4) return;
                    // gm.step = 7;
                    bytes = Encoding.UTF8.GetBytes("oww" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    //gm.step = 1;
                    break;
            }
        }
        public void Update(int step, int table, string otv, int stav, IPEndPoint point) //обнавление ставки и команды готов
        {
            endpoint[table] = point;
            switch (step)
            {
                #region обработка команд готов
                case 1:
                    if (ok[table])
                    {
                        if (ok[0] & ok[1] & ok[2] || Takt != 0)
                        {
                            bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                            udp.Send(bytes, bytes.Length, point);
                        }
                        //else
                        //{
                        //    if (Takt != 0)
                        //    {
                        //        bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                        //        udp.Send(bytes, bytes.Length, point);
                        //    }
                        //}
                    }
                    else
                    {
                        if (Takt != 0) return;
                        ok[table] = true;
                        tm.Interval = 40000;
                        tm.Start();
                        if (ok[0] & ok[1] & ok[2])
                        {
                            tm.Stop();
                            nextTakt();
                        }
                    }
                    break;
                #endregion
                #region обработка ставок
                case 2:
                    if (stavka[table] != 0)
                    {
                        //if (stavka[0] != 0 && stavka[1] != 0 && stavka[2] != 0)
                        //{
                            bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                            udp.Send(bytes, bytes.Length, point);
                        //}
                    }
                    else
                    {
                        stavka[table] = stav;
                        tm.Interval = 25000;
                        tm.Start();
                        if (stavka[0] != 0 && stavka[1] != 0 && stavka[2] != 0)
                        {
                            tm.Stop();
                            nextTakt();
                        }
                    }
                    break;
                    #endregion
            }
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
            //gm.quest = vopr;

            var gmMembers = data.team.SelectMany(x => x.member.Where(c => c != null));  //отберем всех участников игровой тройки
            foreach (var tUser in gmMembers)                                            //и занесем в базу "засвеченный" вопрос
            {
                sql = String.Format("INSERT INTO i_see (user_id, quest_id) values ({0}, {1})", tUser.id, gm.idQuest);
                SQLiteCommand cml = new SQLiteCommand(sql, conn);
                cml.ExecuteNonQuery();
            }

            this.gs.iCon.Text = (sLog.gmLog.iCon + 1).ToString();


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
            switch (step)
            {
                case 4:
                    if (Takt == 2) break;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 5:
                    if (Takt == 3) break;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
                case 6:
                    if (Takt == 4) break;
                    gm.step = 7;
                    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                    udp.Send(bytes, bytes.Length, point);
                    break;
            }
            if (gm.activeTable != table + 1) return false;//если ответ не от активного стола -игнор
            if (gm.team[table].answer != "") return false;//если ответ уже есть, то - игнор

            tmOtvet.Stop();//останавливаем счетчик ожидания ответа.
            if (otv == "") //проверяем пришедший ответ, если ответ - пусто
            {
                correct = false;
                gm.team[table].answer = "Нет ответа."; // то записываем ответ - нет ответа
                nextTakt();
                return false;//сообщаем, что не нужно отправлять ответ модератору
            }

            gm.team[table].answer = otv; //запоминаем ответ
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
        void nextTakt()
        {
            if (gm.iCon > 12)
            {
                gm.step = 10;
                tm.Stop();
                //for (int s = 0; s < 3; s++) udp.Send("ii" + JsonConvert.SerializeObject(gm), IP[s]);
            }
            else
            {
                switch (Takt)
                {
                    #region 0 такт - определение темы вопроса. Ожидание ставок от команд
                    case 0:

                        gm.step = 2;
                        gm.Cell = rn.rnd();
                        gm.theme = (byte)((gm.Cell + 5) / 6);

                        Takt++;
                        txb.Text += "ogg" + gm.step;
                        //tm.Start();
                        break;
                    #endregion
                    #region  1 такт - обработка полученных ставок, определение очереди, получение вопроса
                    case 1:

                        gm.step = 3;
                        gm.Cell = rn.rnd();
                        if (gm.Cell == 0)
                        {
                            gm.step = 2;
                            //gm.iCon++;                  
                            //Takt = 0;
                            endOfIqon = true;
                            txb.Text += "ogg-zerro" + gm.step;
                            //tmSync.Start();
                            for (int i = 0; i < 3; i++) ok[i] = false;
                            break;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            stavka[i] = (stavka[i] == 0) ? 25 : stavka[i];
                            // if (stavka[i] > gm.team[i].iQash) stavka[i] = gm.team[i].iQash;   ВАЖНО!!!
                            gm.team[i].stavka = stavka[i];
                            gm.team[i].iQash -= gm.team[i].stavka;
                            gm.team[i].answer = "";
                        }

                        int[] st = { gm.team[0].stavka, gm.team[1].stavka, gm.team[2].stavka };
                        int[] o = rul.ResponsePriority(gm.Cell, st);
                        gm.o1 = (byte)o[0];
                        gm.o2 = (byte)o[1];
                        gm.o3 = (byte)o[2];

                        gm.activeTable = gm.o1;

                        #region получение вопроса
                        string temaID;
                        if (gm.theme != 0) temaID = themeID[gm.theme].ToString();
                        else temaID = tema; //если тема кот в мешке, то выбираем все темы

                        string zaprosSpiskaVoprosov = " select quests.id " +
                                     "from quests left join i_see on quests.id = i_see.quest_id " +
                                     "where(i_see.id is null or user_id not in (" + usersid + ")) and quests.themeId in (" + temaID + ")";

                        SQLiteCommand cml = new SQLiteCommand(zaprosSpiskaVoprosov, conn);
                        DataTable dtVopros = new DataTable();


                        using (SQLiteDataReader sqr = cml.ExecuteReader())
                        {
                            dtVopros.Load(sqr);
                        }
                        questID = (dtVopros.Rows.Count * rn.rnd()) / 37;//определяем случайно id вопроса из списка в таблице dtVopros
                        questID = Convert.ToInt32(dtVopros.Rows[questID][0]);//questID - id случайного вопроса
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

                        string zaprocVoprosa = "select quests.text, answer, media " +
                                                "from quests where id =" + questID;


                        cml = new SQLiteCommand(zaprocVoprosa, conn);

                        SQLiteDataReader reader = cml.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            gm.quest = Crypt.Decrypt(reader.GetString(0), key);
                            if (!reader.IsDBNull(1)) answerQ = Crypt.Decrypt(reader.GetString(1), key);
                            gm.idQuest = questID;//надо ли его посылать??
                        }
                        #endregion
                        Takt++;
                        //log();
                        txb.Text += "ogg" + gm.step;
                        tmOtvet.Interval = 65000; //запускае таймер с ожиданием ответа 1 команды
                        tmOtvet.Start();
                        correct = false;
                        break;
                    #endregion
                    #region 2 такт - обработка ответа первой команды
                    case 2://ответ первой команды
                        gm.step = 5;
                        gm.correct = correct;
                        if (correct)
                        {
                            //Takt = -1;
                            gm.team[gm.o1 - 1].iQash += 4 * gm.team[gm.o1 - 1].stavka;
                            //gm.iCon++;
                            endOfIqon = true;
                            for (int i = 0; i < 3; i++) ok[i] = false;
                        }
                        else//если ответ не верен запускаем таймер для приема ответа 2-ой команды
                        {
                            tmOtvet.Interval = 20000;
                            tmOtvet.Start();
                            gm.activeTable = gm.o2;
                        }
                        Takt++;
                        //log();
                        txb.Text += "ogg" + gm.step;

                        break;
                    #endregion
                    #region 3 такт - обработка ответ второй команды (если дойдет дело)
                    case 3://ответ второй команды 

                        gm.step = 6;
                        gm.correct = correct;

                        if (correct)
                        {
                            //Takt = -1;
                            //gm.iCon++;
                            endOfIqon = true;
                            gm.team[gm.o2 - 1].iQash += 2 * gm.team[gm.o2 - 1].stavka;
                            for (int i = 0; i < 3; i++) ok[i] = false;
                        }
                        else //если ответ не верен запускаем таймер для приема ответа 3-ой команды
                        {
                            gm.activeTable = gm.o3;
                            tmOtvet.Interval = 20000;
                            tmOtvet.Start();
                        }
                        Takt++;
                        //log();
                        txb.Text += "ogg" + gm.step;
                        break;
                    #endregion
                    #region 4 такт - обработка ответ третьей команды (если дойдет дело)
                    case 4://ответ третьей команды
                        gm.step = 7;
                        gm.correct = correct;
                        if (correct)
                        {
                            gm.team[gm.o3 - 1].iQash += gm.team[gm.o3 - 1].stavka;
                        }
                        endOfIqon = true;
                        //log();
                        txb.Text += "ogg" + gm.step;
                        break;
                        #endregion
                }
            }

            //Отправим сообщение всем столам данной игровой тройки
            Send2All("ogg");

            if (endOfIqon)
            {
                endOfIqon = false;
                gm.step = 1;
                Takt = 0;
                if (gm.iCon >= 12)
                {
                    gm.Cell = rn.rnd();
                    gm.quest = "";
                    gm.theme = 0;
                }
                log();
                gm.iCon++;
                Array.Clear(stavka, 0, stavka.Length);
                Array.Clear(ok, 0, ok.Length);
                Send2All("ogg");
                //for (int i = 0; i < 3; i++)
                //{
                //    //stavka[i] = 0;
                //    //ok[i] = false;
                //    bytes = Encoding.UTF8.GetBytes("ogg" + JsonConvert.SerializeObject(gm));
                //    if (endpoint[i] is IPEndPoint) udp.Send(bytes, bytes.Length, endpoint[i]);
                //}
            }
        }
        private void Send2All(string command)
        {
            bytes = Encoding.UTF8.GetBytes(command + JsonConvert.SerializeObject(gm));
            for (int i = 0; i < 3; i++)
            {
                if (endpoint[i] is IPEndPoint) udp.Send(bytes, bytes.Length, endpoint[i]);
            }
        }
    }
}
