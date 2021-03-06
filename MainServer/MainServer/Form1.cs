﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Linq;
using System.IO;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Columns;


namespace MainServer
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form1 : Form
    {
        #region переменные
        public MySqlConnection mycon;
        public MySqlCommand mycom;
        public SQLiteConnection conn;
       // private FlashContent Content;
        GameinZone gz;
        SQLiteCommand cm;
        SQLiteDataReader rd;
        List<SendLog> logsOfteams = new List<SendLog>();
        List<SendLog> logsOftournaments = new List<SendLog>();
        List<SendLog> logsOfzones = new List<SendLog>();
        public DataTable dt, dtVoprosCheck;
        public RegData rgData = new RegData();
       // ResiveData dataZapros = new ResiveData();
        Registration reg;
        int indexOfThemes;
        string[] text1;
        logList[] tour;
        public TextBox comment;
        bool flag = false;
        public Data data = new Data();
        private string key = "Qade123asdasdasdqwewqeqw423412354232343253***????///";
        RND Rn;
        public List<GameinZone> MassGameZone = new List<GameinZone>();
        Dictionary<int, IPEndPoint> dic = new Dictionary<int, IPEndPoint>();
        public string tur = "Благотворительная игра";
        public string adressGame;
        int troika = 0;                     //номер тройки
        int NumRegKomand = 0;
        bool RassadkaFlag = false;          //флаг начала рассадки
        bool Vozobnovlenie = false;         //флаг о необходимости возобновить игру
        bool stopGame = false;              //флаг о преостановке игры
        private MediaServer mServer;
        private System.Timers.Timer tmr;
        private Object losker = new Object();

        string layoutFileName = "Tournaments.xml";
        string fileName = "out.txt";
        FileStream aFile;
        StreamWriter sw;
        public string roleName;
        Form f = new Form();
        Form2 authForm;
        WebBrowser wb;
        IPEndPoint endpoint;
        UdpClient Udp = new UdpClient(2049);

        Color oldColor;
        #endregion
        public Form1()
        {
           // this.Role = Role;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Rn = new RND();
            aFile = new FileStream(fileName, FileMode.OpenOrCreate);
            sw = new StreamWriter(aFile);
        }
        public void setForm(Form2 form2)
        {
            authForm = form2;
        }
        public bool IsVisible(TabPage tabPage)
        {
            if (tabPage.Parent == null)
                return false;
            else if (tabPage.Parent.Contains(tabPage))
                return true;
            else
                return false;
        }
        public void setRole(string roleName)
        {
            this.roleName = roleName;
            label7.Text = "Вы вошли как " + roleName;
            if (String.Compare(roleName, "Moderator",true) == 0)
            {
                if(IsVisible(questEditor)) tabControl1.TabPages.Remove(questEditor);
            }else
            {
                if (!IsVisible(questEditor)) tabControl1.TabPages.Insert(2,questEditor);
            }
        }

        #region//кнопки
        private void DBLink_Click(object sender, EventArgs e)               //1 кнопка - связь с БД
        {
            //if (DBLink.BackColor != Color.GreenYellow)
            if ((conn?.State != ConnectionState.Open) || (mycon?.State != ConnectionState.Open))
            {
                string myConnectionString = "Data Source=178.172.150.251; Port=27999; Database=iqseven_test; UserId=700iqby; Password=uLCUrohCLoPUcedI; Character Set=utf8;";
                //string myConnectionString = "Data Source=10.253.254.249;Port=3306; Database=iqseven_test; UserId=700iqby; Password=uLCUrohCLoPUcedI;Character Set=utf8;";
                mycon = new MySqlConnection(myConnectionString);
                conn = new SQLiteConnection("Data Source=casinoDB.db3; Version=3;");
                try
                {
                    conn.Open();
                    string themes = "select theme from themes";
                    SQLiteCommand cm = new SQLiteCommand(themes, conn);
                    SQLiteDataReader rd = cm.ExecuteReader();
                    DataTable tableofThemes = new DataTable();
                    using (rd)
                    {
                        if (rd.HasRows) tableofThemes.Load(rd);
                    }
                    var stringThemes = tableofThemes.AsEnumerable().Select(r => r.Field<string>("theme")).ToArray();
                    comboBox1.DataSource = stringThemes;
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    mycon.Open();
                    MySqlCommand cm = new MySqlCommand("SELECT CONCAT(tournaments.name, ' - ', games.game_name) AS Name, games.id as gameid  FROM (tournaments INNER JOIN city ON tournaments.city=city.id) INNER JOIN games ON tournaments.id=games.tournament_id", mycon);
                    MySqlDataReader rd = cm.ExecuteReader();
                    DataTable tournaments = new DataTable();
                    using (rd)  //если есть данные, то записываем в таблицу dat
                    {
                        if (rd.HasRows) tournaments.Load(rd);
                    }
                    tour = new logList[tournaments.Rows.Count];
                    for (int i=0; i<tournaments.Rows.Count; i++)
                    {
                        tour[i] = new logList();
                        tour[i].name = tournaments.Rows[i][0].ToString();
                        tour[i].gameid = tournaments.Rows[i][1].ToString();
                    }

                    //  tour.
                    //   MultiColumnComboBox comdo = new MultiColumnComboBox();
                    // comdo.Parent = this;
                    // comdo.Table = tournaments;

                    List<string> names = new List<string>();
                    foreach(logList l in tour)
                    {
                        names.Add(l.name);

                    }

                    comboBox3.DataSource = names;
                    //List<string> tour = new List<string>(tournaments.AsEnumerable().Select(r => r.Field<string>("Name")).ToArray());
                    //comboBox3.DataSource = tour;
                    mServer = new MediaServer();
                    mServer.Start();
                    if ((conn.State == ConnectionState.Open) && (mycon.State == ConnectionState.Open))
                    {
                        DBLink.BackColor = Color.GreenYellow;
                        DBLink.Text = "Связь установлена";
                        GameButton.Enabled = true;
                        GameButton.PerformClick();
                    }
                }
                catch (InvalidCastException j)
                {
                    MessageBox.Show("Нет подключения к Базе данных" + j.Message);
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    if (mycon.State == ConnectionState.Open)
                        mycon.Close();
                    return;
                }
            }
            else MessageBox.Show("Связь с базами данных уже установлена!");
        }
        private void Game_Click(object sender, EventArgs e)                 //2 кнопка - выбор игры
        {
            if (GameButton.BackColor != Color.GreenYellow)
            {
                //CONCAT(city.name, ' - ', place) AS 
                MySqlCommand cm = new MySqlCommand(@"SELECT tournaments.id, tournaments.name, games.id as gameid, games.tour_id, games.game_name, city.name as city,
                                                    REPLACE(REPLACE(REPLACE(place,'<b>',''),'</b>',''),'<br>','') AS place, date, TIME_FORMAT(TIME, '%H:%i') AS startTime  
                                                    FROM (tournaments INNER JOIN city ON tournaments.city=city.id) INNER JOIN games ON tournaments.id=games.tournament_id", mycon);
                DataTable dat = new DataTable();

                using (MySqlDataReader tur = cm.ExecuteReader())
                {
                    dat.Load(tur);
                }
                ListGamesView.Visible = true;
                ListGamesView.DataSource = dat;
                ListGamesView.BringToFront();
                //ListGames1.DataSource = dat;
                //ListGames1.AllowUserToAddRows = false;
                //ListGames.Size = new Size(1090, 200);
                //ListGames1.BringToFront();
                //ListGames.Location = new Point(250, 150); 
            }
            else
            {
                DialogResult result = MessageBox.Show("Игра выбрана, выхотите изменить выбор?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    GameButton.BackColor = Button.DefaultBackColor;
                    GameButton.FlatStyle = FlatStyle.Standard;
                    GameButton.Text = "Выбор игры";
                }
            }
            //ListKomand.Visible = false;
        }

        private void ListGamesGrid_DoubleClick(object sender, EventArgs e)
        {
            GridView ListGames = sender as GridView;
            GridHitInfo hi = ListGames.CalcHitInfo(ListGamesView.PointToClient(MousePosition));
            if (hi.InRowCell && ((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                data.city = ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["city"]).ToString(); //город
                //if (ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["number_game"]) != null)
                //    data.NumberGame = Convert.ToInt32(ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["number_game"]));//номер игры
                data.idGame = Convert.ToInt32(ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["gameid"]));       //  id игры
                data.NameGame = ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["name"]).ToString();         //название игры
                data.Tur = ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["game_name"]).ToString(); //tur;       //тур
                DateTime date_begin = new DateTime();
                date_begin = Convert.ToDateTime(ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["date"]));
                DateTime time_begin = new DateTime();
                time_begin = Convert.ToDateTime(ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["startTime"]));
                date_begin = date_begin.AddHours(time_begin.Hour);
                date_begin = date_begin.AddMinutes(time_begin.Minute);
                data.startTime = date_begin;//дата и время игры

                infoGame.Text = data.NameGame + " - г." + data.city + ", " + Regex.Replace(ListGames.GetRowCellValue(ListGames.FocusedRowHandle, ListGames.Columns["place"]).ToString(), "<.*?>", String.Empty) + " - " + data.Tur;
                MessageBox.Show(infoGame.Text);
                rgData.Set();           //создание таблицы для регистрации команд
                dt = rgData.ddt();
                //проверка на прерываение игры
                string sql = "SELECT id, zone, gameid, iqon_num, command FROM logs " +
                            "WHERE gameid=" + data.idGame + " AND iqon_num=(SELECT iqon_num FROM logs t1 WHERE t1.zone=logs.zone AND t1.gameid=logs.gameid " +
                            "ORDER BY iqon_num DESC LIMIT 1) ORDER BY zone";

                MySqlCommand cm = new MySqlCommand(sql, mycon);
                DataTable dat = new DataTable();

                using (MySqlDataReader tour = cm.ExecuteReader())
                {
                    dat.Load(tour);
                }
                if (dat.Rows.Count > 0)
                {
                    if (Convert.ToInt16(dat.Rows[0][3]) > 11)
                    {
                        Vozobnovlenie = false;
                        MessageBox.Show("Эта игра уже сыграна!");
                        //GameButton.PerformClick();
                        return;
                    }
                    DialogResult result = MessageBox.Show("Эта игра была прервана! Возобновить ее?", "", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes)
                        return;
                    Vozobnovlenie = true;
                    lot.Text = "Рассадка";
                    if (dat.Rows.Count != Convert.ToInt32(dat.Rows[dat.Rows.Count - 1][1])) //несовпадение кол-ва троек и кол-ва записей log
                    {
                        MessageBox.Show("Данных для восстановления игры не хватает");
                        return;
                    }

                    troika = dat.Rows.Count;
                    for (int i = 0; i < dat.Rows.Count; i++)    //перебираем все  тройки    //Нюхом чую, что здесь косяк вылезет, и сязан будет скорее всего с сортировкой или случайным удалением зоны
                    {
                        if (Convert.ToInt32(dat.Rows[i][1]) == i + 1)//проверка сопадает ли игровая зона
                        {
                            SendLog log = new SendLog();    //структура для получения log данных
                            string json = dat.Rows[i][4].ToString();
                            log = JsonConvert.DeserializeObject<SendLog>(json);
                            gz = new GameinZone(Rn, conn, mycon, Udp); //создаем экземпляр тройки
                            gz.usersid = log.usersid;               //список пользователей тройки
                            gz.setThemes(log.idTheme, log.Themes);  //список id тем и названий
                            gz.data = log.dataLog;                  //класс data
                            gz.gm = log.gmLog;                      //класс game
                            gz.gm.iCon++;                           //Начинаем игру со следующего айкона
                            for (int k = 0; k < 3; k++)      //для каждой команды тройки находим новый ключ сессии в таблице зарегистрировавшихся команд
                            {
                                dt.Rows.Add(new Object[] { log.dataLog.GameZone, log.dataLog.team[k].uid, log.dataLog.team[k].name, "", log.dataLog.team[k].rating, log.dataLog.team[k].iQash, log.dataLog.team[k].table, false });
                            }
                            //                        ListKomand.DataSource = dt;
                            //                        ListKomand.Columns[3].Visible = false;
                            MassGameZone.Add(gz);
                        }
                        else
                        {
                            gz = new GameinZone(Rn, conn, mycon, Udp); //создаем экземпляр тройки
                            MassGameZone.Add(gz);
                        }
                    }
                    lot.Text = "Рассадка закончена";
                }
                ListGamesView.Visible = false;
                GameButton.Text = "Игра выбрана";
                GameButton.BackColor = Color.GreenYellow;
                ButtonReg.Enabled = true;
                ButtonReg.PerformClick();
                infoGame.Visible = true;
            }
        }

        //private void ListGames_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)//получение данных выбранной игры
        //{
        //    //заниесение данных в класс DATA
        //    data.city = ListGames1.CurrentRow.Cells["place"].Value.ToString();             //город
        //    if (ListGames1.CurrentRow.Cells["number_game"].Value != DBNull.Value)
        //        data.NumberGame = Convert.ToInt32(ListGames1.CurrentRow.Cells["number_game"].Value);//номер игры
        //    data.idGame = Convert.ToInt32(ListGames1.CurrentRow.Cells["gameId"].Value);       //  id игры
        //    data.NameGame = ListGames1.CurrentRow.Cells["name"].Value.ToString();         //название игры
        //    data.Tur = ListGames1.CurrentRow.Cells["gameName"].Value.ToString(); //tur;       //тур
        //    DateTime date_begin = new DateTime();
        //    date_begin = Convert.ToDateTime(ListGames1.CurrentRow.Cells["date"].Value.ToString());
        //    DateTime time_begin = new DateTime();
        //    time_begin = Convert.ToDateTime(ListGames1.CurrentRow.Cells["startTime"].Value.ToString());
        //    date_begin = date_begin.AddHours(time_begin.Hour);
        //    date_begin = date_begin.AddMinutes(time_begin.Minute);
        //    data.startTime = date_begin;//дата и время игры

        //    infoGame.Text = data.NameGame + " г." + ListGames1.CurrentRow.Cells["place"].Value + " - " + data.Tur;
        //    //adressGame = ListGames.CurrentRow.Cells[5].Value.ToString();
        //    rgData.Set();           //создание таблицы для регистрации команд
        //    dt = rgData.ddt();
        //    //проверка на прерываение игры
        //    string sql = "SELECT id, zone, gameid, iqon_num, command FROM logs " +
        //                "WHERE gameid=" + data.idGame + " AND iqon_num=(SELECT iqon_num FROM logs t1 WHERE t1.zone=logs.zone AND t1.gameid=logs.gameid " +
        //                "ORDER BY iqon_num DESC LIMIT 1) ORDER BY zone";

        //    MySqlCommand cm = new MySqlCommand(sql, mycon);
        //    DataTable dat = new DataTable();

        //    using (MySqlDataReader tour = cm.ExecuteReader())
        //    {
        //        dat.Load(tour);
        //    }
        //    ListGames1.Visible = false;
        //    if (dat.Rows.Count > 0)
        //    {
        //        if (Convert.ToInt16(dat.Rows[0][3]) > 11)
        //        {
        //            Vozobnovlenie = false;
        //            MessageBox.Show("Эта игра уже сыграна!");
        //            return;
        //        }
        //        DialogResult result = MessageBox.Show("Эта игра была прервана! Возобновить ее?", "", MessageBoxButtons.YesNo);
        //        if (result != DialogResult.Yes)
        //            return;
        //        Vozobnovlenie = true;
        //        lot.Text = "Рассадка";
        //        if (dat.Rows.Count != Convert.ToInt32(dat.Rows[dat.Rows.Count - 1][1])) //несовпадение кол-ва троек и кол-ва записей log
        //        {
        //            MessageBox.Show("Данных для восстановления игры не хватает");
        //            return;
        //        }

        //        troika = dat.Rows.Count;
        //        for (int i = 0; i < dat.Rows.Count; i++)    //перебираем все  тройки    //Нюхом чую, что здесь косяк вылезет, и сязан будет скорее всего с сортировкой или случайным удалением зоны
        //        {
        //            if (Convert.ToInt32(dat.Rows[i][1]) == i + 1)//проверка сопадает ли игровая зона
        //            {
        //                SendLog log = new SendLog();    //структура для получения log данных
        //                string json = dat.Rows[i][4].ToString();
        //                log = JsonConvert.DeserializeObject<SendLog>(json);
        //                gz = new GameinZone(Rn, conn, mycon, Udp); //создаем экземпляр тройки
        //                gz.usersid = log.usersid;               //список пользователей тройки
        //                gz.setThemes(log.idTheme, log.Themes);  //список id тем и названий
        //                gz.data = log.dataLog;                  //класс data
        //                gz.gm = log.gmLog;                      //класс game
        //                gz.gm.iCon++;                           //Начинаем игру со следующего айкона
        //                for (int k = 0; k < 3; k++)      //для каждой команды тройки находим новый ключ сессии в таблице зарегистрировавшихся команд
        //                {
        //                    dt.Rows.Add(new Object[] { log.dataLog.GameZone, log.dataLog.team[k].uid, log.dataLog.team[k].name, "", log.dataLog.team[k].rating, log.dataLog.team[k].iQash, log.dataLog.team[k].table, false });
        //                }
        //                //                        ListKomand.DataSource = dt;
        //                //                        ListKomand.Columns[3].Visible = false;
        //                MassGameZone.Add(gz);
        //            }
        //            else
        //            {
        //                gz = new GameinZone(Rn, conn, mycon, Udp); //создаем экземпляр тройки
        //                MassGameZone.Add(gz);
        //            }
        //        }
        //        lot.Text = "Рассадка закончена";
        //    }
        //    GameButton.Text = "Игра выбрана";
        //    GameButton.BackColor = Color.GreenYellow;
        //    ButtonReg.Enabled = true;
        //    ButtonReg.PerformClick();
        //    infoGame.Visible = true;
        //}

        private void Registration_Click(object sender, EventArgs e)         //4 кнопка - начать регистрацию команд
        {
            if (ButtonReg.BackColor != Color.GreenYellow)
            {
                ListKomand.Visible = true;
                ListKomand.Focus();

                rgData.canReg = true;
                Zapros();
                ListKomand.DataSource = dt;
                ListKomand.Columns[1].Visible = false;
                ListKomand.Columns[3].Visible = false;
                ListKomand.Columns[2].FillWeight = 800;

                ButtonReg.BackColor = Color.GreenYellow;
                ButtonReg.Text = "Идет Регистрация";
                reg = new Registration();                //подключение к классу registration                             
                reg.mycon = mycon;//подключение к базе данных команд

                foreach (DataGridViewColumn lkCell in ListKomand.Columns)
                {
                    lkCell.HeaderText = dt.Columns[lkCell.Name].Caption;
                    lkCell.ReadOnly = lkCell.Name == "I-cash" ? false : true;
                }

                #region создание экземпляра КОМАНДА и ТЕМА
                for (int i = 0; i < 3; i++)
                {
                    data.team[i] = new teams();
                    for (int j = 0; j < 5; j++)
                    {
                        data.team[i].member[j] = new teams.members();
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    data.tema[i] = new Data.Temy();
                }
                #endregion

                reg.Server(dt, data, rgData, MassGameZone); //включить прослушку порта
                reg.onAddNewReg += refreshTable;            //обновить таблицу зарегистрировавшихся команд после добваления новой 
                butEndReg.Enabled = true;                   //активировать кнопку Конец регистрации
            }
            else
            #region повтороная регистрация
            {
                var result = MessageBox.Show("Идет регистрация!!! Начать регистрацию заново?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    rgData.canReg = true;
                    ButtonReg.BackColor = Button.DefaultBackColor;
                    ButtonReg.Text = "Начать регистрацию";
                    dt.Clear();

                }
            }
            #endregion
        }

        private void ListKomand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((ListKomand.CurrentCell.ColumnIndex == 7) && (start.Text == "Поехали"))
            {
                if (!(bool)ListKomand.CurrentCell.EditedFormattedValue)
                {
                    foreach (DataGridViewRow r in ListKomand.Rows)
                    {
                        if ((int)r.Cells[0].Value == (int)ListKomand[0, ListKomand.CurrentRow.Index].Value)
                        {
                            r.DefaultCellStyle.BackColor = oldColor;
                            r.Cells[7].Value = false;
                        }
                    }
                    MassGameZone[Convert.ToInt32(ListKomand[0, ListKomand.CurrentRow.Index].FormattedValue) - 1].ContinueGame();
                }

                else
                {
                    if ((bool)ListKomand.CurrentCell.EditedFormattedValue)
                    {
                        oldColor = ListKomand.CurrentRow.DefaultCellStyle.BackColor;
                        foreach (DataGridViewRow r in ListKomand.Rows)
                        {
                            if ((int)r.Cells[0].Value == (int)ListKomand[0, ListKomand.CurrentRow.Index].Value)
                            {
                                r.DefaultCellStyle.BackColor = Color.Gray;
                                r.Cells[7].Value = true;
                            }
                        }
                        MassGameZone[Convert.ToInt32(ListKomand[0, ListKomand.CurrentRow.Index].FormattedValue) - 1].stopGame();
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)//удаление команды из списка
        {
            /*if (butEndReg.Text != "Регистрация закончена")
            {
                DialogResult result = MessageBox.Show("Удалить из списка команду " + ListKomand.CurrentRow.Cells[2].Value + "?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dic.Remove(Convert.ToInt32(ListKomand.CurrentRow.Cells[1].Value));
                    ListKomand.Rows.Remove(ListKomand.CurrentRow);
                    NumRegKomand--;
                }
            }*/
        }

        private void ListKomand_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Удалить из списка команду " + e.Row.Cells[2].Value + "?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
            else
            {
                //dic.Remove(Convert.ToInt32(e.Row.Cells[1].Value));
                //ListKomand.Rows.Remove(e.Row);
                NumRegKomand--;
            }
        }

        private void ListKomand_Leave(object sender, EventArgs e)
        {
            //ListKomand.ClearSelection();
            ((DataGridView)sender).CurrentCell = null;
        }

        private void butEndReg_Click(object sender, EventArgs e)            //5 кнопка окончания регистрации
        {
            if (butEndReg.BackColor != Color.GreenYellow)
            {
                butEndReg.Text = "Регистрация закончена";
                butEndReg.BackColor = Color.GreenYellow;
                ButtonReg.Enabled = false;
                lot.Enabled = true;
                rgData.canReg = false;
                //ListKomand.ReadOnly = true;
                /*      //////////////////////////copy/////////////////////////////
                      ListKomand.ReadOnly = false;
                      foreach (DataGridViewRow r in ListKomand.Rows)
                      {
                          if ((Vozobnovlenie) && ((int)ListKomand[0, 0].Value == 0))
                          {
                              r.Cells[0].Value = (int)r.Cells[0].Value + 1;
                          }
                          r.Cells[7].Value = false;
                      }
                      int numKom = ListKomand.RowCount;   // количество команд               
                      int numZon = numKom / 3;            //количество троек
                      int komNextTur = numKom % 3;        //количество команд проходящих в следующий тур без игры
                      for (int i = 0; i < numKom / 3; i += 2)
                          for (int j = 0; j < 3; j++)
                              ListKomand.Rows[i * 3 + j + komNextTur].DefaultCellStyle.BackColor = Color.GreenYellow;
                      for (int i = 0; i < komNextTur; i++)
                          ListKomand.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
                      ListKomand.ReadOnly = true;

                      //////////////////////////copy/////////////////////////////
                      ListKomand.Columns[0].ReadOnly = true;
                      ListKomand.Columns[1].ReadOnly = true;
                      ListKomand.Columns[2].ReadOnly = true;
                      ListKomand.Columns[3].ReadOnly = true;
                      ListKomand.Columns[4].ReadOnly = true;
                      ListKomand.Columns[5].ReadOnly = true;
                      ListKomand.Columns[6].ReadOnly = true;
                      //////////////////////////copy/////////////////////////////
                      //////////////////////////copy/////////////////////////////*/


            }
            else
            #region продолить регистрацию
            {
                var result = MessageBox.Show("Продолжить регистрацию?", "", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    butEndReg.Text = "Закончить регистрацию";
                    lot.Enabled = false;
                    lot.BackColor = Button.DefaultBackColor;
                    lot.Text = "Жеребьевка";
                    butEndReg.BackColor = Button.DefaultBackColor;
                    // reg.start();
                    //reg.Server(dt, data, rgData);
                    lot.Enabled = false;
                    rgData.canReg = true;
                    //////////////////////////copy/////////////////////////////
                    ListKomand.ReadOnly = false;
                }
            }
            #endregion
        }

        private void lotClick()                  //6 кнопка - жеребьевка
        {
            #region возобновление игры по LOG
            if (Vozobnovlenie) //если игра была прервана и требуется возобновление игры
            {
                string sql = "SELECT id, zone, gameid, iqon_num, command FROM logs " +
                    "WHERE gameid=" + data.idGame + " AND iqon_num=(SELECT iqon_num FROM logs t1 WHERE t1.zone=logs.zone AND t1.gameid=logs.gameid " +
                    "ORDER BY iqon_num DESC LIMIT 1) ORDER BY zone";

                MySqlCommand cm = new MySqlCommand(sql, mycon);
                DataTable dat = new DataTable();

                using (MySqlDataReader tour = cm.ExecuteReader())
                {
                    dat.Load(tour); //заполняем таблицу последних сыгранных айконов выбранной игры
                }
                if (dat.Rows.Count == 0) //если данных нет , то выходим
                {
                    MessageBox.Show("Данных для восстановления игры нет");
                    return;
                }
                if (dat.Rows.Count != Convert.ToInt32(dat.Rows[dat.Rows.Count - 1][1])) //несовпадение кол-ва троек и кол-ва записей log
                {
                    MessageBox.Show("Данных для восстановления игры не хватает");
                    return;
                }
                troika = dat.Rows.Count;
                for (int i = 0; i < dat.Rows.Count; i++)    //перебираем все  тройки
                {
                    if (Convert.ToInt32(dat.Rows[i][1]) == i + 1)//проверка сопадает ли игровая зона
                    {
                        SendLog log = new SendLog();    //структура для получения log данных
                        string json = dat.Rows[i][4].ToString();
                        log = JsonConvert.DeserializeObject<SendLog>(json);
                        //GameinZone gz = new GameinZone(Rn, conn, mycon, Udp, textBox3); //создаем экземпляр тройки
                        gz.usersid = log.usersid;               //список пользователей тройки
                        gz.setThemes(log.idTheme, log.Themes);  //список id тем и названий
                        gz.data = log.dataLog;                  //класс data
                        gz.gm = log.gmLog;                      //класс game
                        gz.gm.iCon++;                           //Начинаем игру со следующего айкона

                        for (int k = 0; k < 3; k++)      //для каждой команды тройки находим новый ключ сессии в таблице зарегистрировавшихся команд
                            for (int j = 0; j < ListKomand.RowCount; j++)   //перебор зарег. команд
                            {
                                //если id команды совпадает, то запоминаем новый ключ
                                if (log.dataLog.team[k].uid == Convert.ToInt32(ListKomand.Rows[j].Cells[1].Value))
                                {
                                    gz.data.team[k].kod = ListKomand.Rows[j].Cells[3].Value.ToString();
                                    break;
                                }
                            }
                        /*
                        for (int counter_users = 0; counter_users < 3; counter_users++)
                        {
                            gz.users.Add(5);
                        }*/
                        MassGameZone.Add(gz);
                    }
                    else
                    {
                 //       GameinZone gz = new GameinZone(Rn, conn, mycon, Udp, textBox3); //создаем экземпляр тройки
                        MassGameZone.Add(gz);
                    }

                }
                //lot.BackColor = Color.GreenYellow;
                lot.Text = "Рассадка закончена";
                Anons.Enabled = true;
                return;
            }
            #endregion
            lot.BackColor = Color.GreenYellow;
            lot.Text = "Жеребьевка закончена";

            //сортируем зарегистрировавшиеся команды по рейтингу
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                object min;
                for (int j = 0; j < dt.Rows.Count - 1; j++)
                {
                    if (Convert.ToInt32(dt.Rows[j][4]) < Convert.ToInt32(dt.Rows[j + 1][4]))
                    {
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            min = dt.Rows[j + 1][x];
                            dt.Rows[j + 1][x] = dt.Rows[j][x];
                            dt.Rows[j][x] = min;
                        }
                    }
                }
            }
            int iBotsCount = (3 - dt.Rows.Count % 3) % 3;
            for (int i = 0; i < iBotsCount; i++)
            {
                DataRow dtrow = dt.NewRow();
                dtrow[0] = 0;
                dtrow[1] = i + 1;
                dtrow[2] = "Тупой бот №" + i;
                dtrow[4] = i * 9 + 1000;
                dtrow[5] = 700;
                dt.Rows.Add(dtrow);
            }

            dt.DefaultView.Sort = "zona ASC";
            //ListKomand.DataSource = dt;

            int numKom = dt.Rows.Count; //количество команд
            int komNextTur = numKom % 3; //команды перешедшие в следующий тур без игры    
            int numZon = numKom / 3;//количество игровых зон

            for (int i = 0; i < 3; i++)  //проставляем командам их игровые зоны и столы
                for (int j = 0; j < numZon; j++)
                {

                    dt.Rows[j + numZon * i + komNextTur][0] = j + 1;  //присваиваем номер игровой зоны команде
                                                                      //  dt.Rows[j + numZon * i + komNextTur][6] = i+1;  //присваиваем номер стола команде
                }

            ListKomand.DataSource = dt;//раскрашиваем в таблице тройки разным цветом
            for (int i = 0; i < numKom / 3; i += 2)
                for (int j = 0; j < 3; j++)
                    ListKomand.Rows[i * 3 + j + komNextTur].DefaultCellStyle.BackColor = Color.GreenYellow;
            for (int i = 0; i < komNextTur; i++)
                ListKomand.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
            Anons.Enabled = true;
        }

        private void lot_Click(object sender, EventArgs e)                  //6 кнопка - жеребьевка
        {
            if (lot.BackColor != Color.GreenYellow)
            {
                #region возобновление игры по LOG
                if (Vozobnovlenie) //если игра была прервана и требуется возобновление игры
                {
                    /*string sql = "SELECT id, zone, gameid, iqon_num, command FROM logs " +
                        "WHERE gameid=" + data.idGame + " AND iqon_num=(SELECT iqon_num FROM logs t1 WHERE t1.zone=logs.zone AND t1.gameid=logs.gameid " +
                        "ORDER BY iqon_num DESC LIMIT 1) ORDER BY zone";

                    MySqlCommand cm = new MySqlCommand(sql, mycon);    
                    DataTable dat = new DataTable();
                  
                    using (MySqlDataReader tour = cm.ExecuteReader())
                    {
                        dat.Load(tour); //заполняем таблицу последних сыгранных айконов выбранной игры
                    }
                    if (dat.Rows.Count == 0) //если данных нет , то выходим
                    {
                        MessageBox.Show("Данных для восстановления игры нет");
                        return;
                    }
                    if (dat.Rows.Count != Convert.ToInt32(dat.Rows[dat.Rows.Count - 1][1])) //несовпадение кол-ва троек и кол-ва записей log
                    {
                        MessageBox.Show("Данных для восстановления игры не хватает");
                        return;
                    }
                    troika = dat.Rows.Count;
                    for (int i = 0; i < dat.Rows.Count; i++)    //перебираем все  тройки
                    {
                        if (Convert.ToInt32(dat.Rows[i][1]) == i + 1)//проверка сопадает ли игровая зона
                        {
                            SendLog log = new SendLog();    //структура для получения log данных
                            string json = dat.Rows[i][4].ToString();
                            log = JsonConvert.DeserializeObject<SendLog>(json);
                            GameinZone gz = new GameinZone(Rn, conn, mycon, Udp, textBox3); //создаем экземпляр тройки
                            gz.usersid = log.usersid;               //список пользователей тройки
                            gz.setThemes(log.idTheme, log.Themes);  //список id тем и названий
                            gz.data = log.dataLog;                  //класс data
                            gz.gm = log.gmLog;                      //класс game
                            gz.gm.iCon++;                           //Начинаем игру со следующего айкона
                            for (int k = 0; k < 3; k++)      //для каждой команды тройки находим новый ключ сессии в таблице зарегистрировавшихся команд
                                for (int j = 0; j < ListKomand.RowCount; j++)   //перебор зарег. команд
                                {
                                    //если id команды совпадает, то запоминаем новый ключ
                                    if (log.dataLog.team[k].uid == Convert.ToInt32(ListKomand.Rows[j].Cells[1].Value))
                                    {
                                        gz.data.team[k].kod = ListKomand.Rows[j].Cells[3].Value.ToString();
                                        break;
                                    }
                                }
                          //  gz.gs = new GameStatistic(log.dataLog.GameZone.ToString(), log.gmLog.iCon.ToString(), 1400, 50 + (i * 35));

                            MassGameZone.Add(gz);
                        }
                        else
                        {
                            GameinZone gz = new GameinZone(Rn, conn, mycon, Udp, textBox3); //создаем экземпляр тройки
                         //   gz.gs = new GameStatistic(gz, 1, 1400, 50 + (i * 35));

                            MassGameZone.Add(gz);
                        }

                    }*/
                    lot.BackColor = Color.GreenYellow;
                    lot.Text = "Рассадка закончена";
                    Anons.Enabled = true;
                    return;
                }
                #endregion
                lot.BackColor = Color.GreenYellow;
                lot.Text = "Жеребьевка закончена";

                int iBotsCount = (3 - dt.Rows.Count % 3) % 3;
                for (int i = 0; i < iBotsCount; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow[0] = 0;
                    dtrow[1] = i + 1;
                    dtrow[2] = "Тупой бот №" + i;
                    dtrow[4] = i * 9 + 1000;
                    dtrow[5] = 700;
                    dt.Rows.Add(dtrow);
                }

                int numKom = dt.Rows.Count; //количество команд
                int komNextTur = numKom % 3; //команды перешедшие в следующий тур без игры    
                int numZon = numKom / 3;//количество игровых зон

                if (MessageBox.Show("Выполнить автоматическую жеребьевку по рейтингу команд?", "Способ жеребьевки", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //сортируем зарегистрировавшиеся команды по рейтингу
                    //for (int i = 0; i < dt.Rows.Count - 1; i++)
                    //{
                    //    object min;
                    //    for (int j = 0; j < dt.Rows.Count - 1; j++)
                    //    {
                    //        if (Convert.ToInt32(dt.Rows[j][4]) < Convert.ToInt32(dt.Rows[j + 1][4]))
                    //        {
                    //            for (int x = 0; x < dt.Columns.Count; x++)
                    //            {
                    //                min = dt.Rows[j + 1][x];
                    //                dt.Rows[j + 1][x] = dt.Rows[j][x];
                    //                dt.Rows[j][x] = min;
                    //            }
                    //        }
                    //    }
                    //}
                    //Есть способ намного проще ;)
                    //dt.DefaultView.Sort = "Rating DESC";
                    //dt = dt.DefaultView.ToTable();
                    //Ну или вот так:
                    dt = dt.Select("", "Rating DESC").CopyToDataTable(); // OrderByDescending(x => x["Rating"]).CopyToDataTable();
                    //проставляем командам их игровые зоны и столы
                    dt.Select().ToList<DataRow>().ForEach(dtRows =>
                    {
                        int index = dt.Rows.IndexOf(dtRows);
                        dtRows["Zone"] = index % numZon + 1;
                        dtRows["Table"] = index / numZon + 1;
                    });
                    /*for (int i = 0; i < 3; i++)  //проставляем командам их игровые зоны и столы
                        for (int j = 0; j < numZon; j++)
                        {
                            dt.Rows[j + numZon * i + komNextTur][0] = j + 1;  //присваиваем номер игровой зоны команде
                                                                              //  dt.Rows[j + numZon * i + komNextTur][6] = i+1;  //присваиваем номер стола команде
                        }*/
                }
                else
                    dt.Select().ToList<DataRow>().ForEach(dtRows =>
                    {
                        int index = dt.Rows.IndexOf(dtRows);
                        dtRows["Zone"] = index / 3 + 1;
                        dtRows["Table"] = index % 3 + 1;
                    });

                dt.DefaultView.Sort = "Zone ASC, Table ASC";

                //ListKomand.DataSource = dt;//раскрашиваем в таблице тройки разным цветом
                //for (int i = 0; i < numKom / 3; i += 2)
                //    for (int j = 0; j < 3; j++)
                //        ListKomand.Rows[i * 3 + j + komNextTur].DefaultCellStyle.BackColor = Color.GreenYellow;
                //for (int i = 0; i < komNextTur; i++)
                //    ListKomand.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
                Anons.Enabled = true;
            }
            else
            {
                DialogResult result = MessageBox.Show("Повторить жеребьевку?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    lot.BackColor = DefaultBackColor;
                    lot.Text = "Жеребьевка";
                    Anons.Enabled = false;
                    //foreach (DataRow dtRows in dt.Rows)
                    dt.Select().ToList<DataRow>().ForEach(dtRows => { dtRows["Zone"] = DBNull.Value; dtRows["Table"] = DBNull.Value; });
                    dt.DefaultView.Sort = "";
                }
            }
            ListKomand.DataSource = dt;
        }
        private void Anons_Click(object sender, EventArgs e)                //7 кнопка - оповещение команд (рассылка установочных данных)             
        {
            RassadkaFlag = true;

            start.Enabled = true;
            if (Vozobnovlenie)
            {
                Anons.BackColor = Color.GreenYellow;
                return;
            }
            if (Anons.BackColor != Color.GreenYellow)
            {

                Anons.BackColor = Color.GreenYellow;
                int numKom = ListKomand.RowCount;   // количество команд               
                int numZon = numKom / 3;            //количество троек
                int komNextTur = numKom % 3;        //количество команд проходящих в следующий тур без игры
                troika = 0;
               /* for (int i = 0; i < 5; i++)
                {
                    data.team[0].member[i].N = "";
                    data.team[0].member[i].F = "";
                    data.team[0].member[i].rait = 0;
                    data.team[0].member[i].dr = 0;
                }*/
                MassGameZone.Clear();

                #region создание троек
                for (int i = 0; i < numZon; i++)
                {

                    string userid = "";
                    troika++;
                    GameinZone gz = new GameinZone(Rn, conn, mycon, Udp);

                    for (int j = 0; j < 3; j++)
                    {
                        if (Convert.ToInt32(ListKomand.Rows[i * 3 + j + komNextTur].Cells[0].Value) != troika)
                        {
                            MessageBox.Show("Ошибка " + troika);
                            break;
                        }
                        #region заполнение данных о 3-ке команд и их членах
                        int index = i * 3 + j + komNextTur;
                        gz.data.GameZone = troika;
                        gz.data.city = data.city;
                        gz.data.NumberGame = data.NumberGame;
                        gz.data.idGame = data.idGame;
                        gz.data.NameGame = data.NameGame;
                        gz.data.Tur = data.Tur;
                        gz.data.startTime = data.startTime;
                        gz.gm.team[j] = new Game.Teames();
                        // gm.team[j].answer = ListKomand.Rows[index].Cells[3].Value.ToString();//IP
                        gz.gm.team[j].table = (byte)(j + 1); //Convert.ToByte( ListKomand.Rows[index].Cells[6].Value);  //номер стола
                        gz.gm.zoneUID = (byte)i;   //номер игровой зоны    

                        gz.data.team[j] = new teams();
                        gz.data.team[j].table = (byte)(j + 1); //Convert.ToByte(ListKomand.Rows[index].Cells[6].Value);
                        //gz.data.NumberTable = (byte)(j + 1);
                        if (ListKomand.Rows[index].Cells[5].Value != DBNull.Value)       //АйКЭШ
                            gz.gm.team[j].iQash = (Convert.ToInt32(ListKomand.Rows[index].Cells[5].Value));
                        //gz.gm.team[j].iQash = (ListKomand.Rows[index].Cells[5].Value != DBNull.Value) ? (Convert.ToInt32(ListKomand.Rows[index].Cells[5].Value)) : 700;
                        gz.data.team[j].iQash = gz.gm.team[j].iQash;
                        gz.gm.team[j].stavka = 25;  //Минимальная ставка по-умолчанию
                        gz.data.team[j].name = ListKomand.Rows[index].Cells[2].Value.ToString();//Name
                        gz.data.team[j].kod = ListKomand.Rows[index].Cells[3].Value.ToString();
                        if (DBNull.Value != ListKomand.Rows[index].Cells[1].Value)
                            gz.data.team[j].uid = Convert.ToInt32(ListKomand.Rows[index].Cells[1].Value);//id
                        gz.data.team[j].rating = Convert.ToInt32(ListKomand.Rows[index].Cells[4].Value);//rating
                        gz.gm.team[j].uid = gz.data.team[j].uid;
                        DataTable dtbl = rgData.get(gz.data.team[j].uid);

                        for (int k = 0; k < dtbl.Rows.Count; k++)
                        {
                            gz.data.team[j].member[k] = new teams.members();
                            gz.data.team[j].member[k].id = Convert.ToInt32(dtbl.Rows[k][1]);
                            gz.data.team[j].member[k].N = dtbl.Rows[k][2].ToString();
                            gz.data.team[j].member[k].F = dtbl.Rows[k][3].ToString();
                            gz.data.team[j].member[k].rait = Convert.ToInt32(dtbl.Rows[k][4]);
                            gz.data.team[j].member[k].dr = Convert.ToInt32(dtbl.Rows[k][5]);
                            userid += gz.data.team[j].member[k].id.ToString() + ", ";
                        }
                        if (userid == "") MessageBox.Show(gz.data.team[j].name + " нет игроков");
                        #endregion
                    }

                    userid = (userid == "") ? userid = "0" : userid = userid.Substring(0, userid.Length - 2);
                    //GameinZone gz = new GameinZone(gm, Rn, conn, userid);
                    gz.usersid = userid;


                    #region задаем темы вопросов для тройки
                    string zaprosTemy = "select themes.id as id, theme, description, required " +
                                  "from quests left join i_see on (quests.id = i_see.quest_id and i_see.user_id in (" + userid + ")) inner join themes on quests.themeid = themes.id " +
                                  "where i_see.id is null " +
                                  "group by quests.themeid " +
                                  "having count(quests.themeid) > 2";

                    SQLiteCommand cml = new SQLiteCommand(zaprosTemy, conn);
                    DataTable dtVopros = new DataTable();

                    using (SQLiteDataReader sqr = cml.ExecuteReader())
                    {
                        dtVopros.Load(sqr);
                    }

                    DataTable themesfortroika = dtVopros.Clone();
                    foreach (DataRow row in dtVopros.Select("required=1"))
                    {
                        themesfortroika.ImportRow(row);
                    }

                    int rowin;
                    while (themesfortroika.Rows.Count < 6)
                    {
                        rowin = (dtVopros.Rows.Count * Rn.rnd()) / 37;
                        ////////////////////////////////////////////////////////////////////////
                        try
                        {
                            int test = Convert.ToInt32(dtVopros.Rows[rowin][3]);
                            if (!Convert.ToBoolean(dtVopros.Rows[rowin][3])) themesfortroika.ImportRow(dtVopros.Rows[rowin]);
                        }
                        catch (Exception ex)
                        {
                            //                           MessageBox.Show("Повтор строки!");
                        }
                    }


                    string[] theme = new string[7];
                    int[] themeId = new int[7];
                    string[] description = new string[7];

                    theme[0] = "Кот в мешке";
                    themeId[0] = 0;
                    description[0] = "Случайная тема";
                    gz.data.tema[0] = new Data.Temy();
                    gz.data.tema[0].theme = theme[0];
                    gz.data.tema[0].themeId = (byte)themeId[0];
                    gz.data.tema[0].description = description[0];
                    for (int t = 1; t < 7; t++)
                    {

                        theme[t] = themesfortroika.Rows[t - 1][1].ToString();
                        themeId[t] = Convert.ToInt16(themesfortroika.Rows[t - 1][0]);
                        description[t] = themesfortroika.Rows[t - 1][2].ToString();
                        gz.data.tema[t] = new Data.Temy();
                        gz.data.tema[t].theme = theme[t];
                        gz.data.tema[t].themeId = (byte)themeId[t];
                        gz.data.tema[t].description = description[t];
                    }
                    //int[] themeID = themesfortroika.AsEnumerable().Select(r => r.Field<Int32>("id")).ToArray();
                    gz.setThemes(themeId, theme);
                    //ListKomand.DataSource = themesfortroika;
                    MassGameZone.Add(gz);//добавляем тройку в очередь    
                    #endregion                 
                }
                RassadkaFlag = true;
                #endregion
            }
            else
            #region если повторить оповещение
            {
                DialogResult result = MessageBox.Show("Повторить оповещение?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Anons.BackColor = Button.DefaultBackColor;
                    start.Enabled = false;
                }
            }
            #endregion
        }
        private void Start(object sender, EventArgs e)
        {
            //string sql = "CREATE TABLE gamePartitioners ('user_id' INT(5) NOT NULL AUTO_INCREMENT, 'username' VARCHAR(50), PRIMARY KEY('user_id'), INDEX('username'));";
            // MySqlCommand  cm = new MySqlCommand(sql, mycon);
            // cm.ExecuteReader();

            if (start.BackColor != Color.GreenYellow)
            {
                rgData.canReg = true;
                start.BackColor = Color.GreenYellow;
                start.Text = "Поехали";

                //for (int i = 0; i < troika - 1; i++)
                for (int i = 0; i < troika; i++)
                {
                    MassGameZone[i].startGame(Vozobnovlenie);
                }
                rgData.gameStart = true;
                CheckOtvet check = new CheckOtvet();
                check.set();
                dtVoprosCheck = check.get();
                dataGridView2.DataSource = dtVoprosCheck;
                dataGridView2.Columns["Id"].Visible = false;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                dataGridView2.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView2.AllowUserToResizeRows = true;
                dataGridView2.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView2.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                gameStopBut.Enabled = true;
                gameStopBut.Text = "Игра идет";
                ListKomand.ReadOnly = false;

                label3.Visible = true;
                for (int i = 0; i < MassGameZone.Count; i++)
                {
                    MassGameZone[i].gs = new GameStatistic(MassGameZone[i].data.GameZone, (MassGameZone[i].gm.iCon).ToString(), label3.Location.X,150+ (i * 35));
                    MassGameZone[i].gs.stopButton.Click += stGame;
                    MassGameZone[i].gs.restartIcon.Click += RestartIcon_Click;
                    MassGameZone[i].gs.turboButton.Click += TurboButton_Click;
                }
                ListKomand.ReadOnly = true;
                button1.Enabled = true;
                ToJS();
            }

        }

        private void TurboButton_Click(object sender, EventArgs e)
        {
            Button btnPressed = (Button)sender;
            int i = Convert.ToInt32(btnPressed.Tag) - 1;
            MassGameZone[i].TurboIt();
        }

        private void RestartIcon_Click(object sender, EventArgs e)
        {
            Button btnPressed = (Button)sender;
            int i = Convert.ToInt32(btnPressed.Tag) - 1;
            MassGameZone[i].RestartIqon();
        }

        private void stGame(object sender, EventArgs e)
        {
            Button btnPressed = (Button)sender;
            int i = Convert.ToInt32(btnPressed.Tag) - 1;
            if (!MassGameZone[i].stopGm)
            {
                MassGameZone[i].stopGame();
                btnPressed.Text = "Старт";
            }
            else
            {
                MassGameZone[i].ContinueGame();
                btnPressed.Text = "Пауза";
            }
        }


        private void gameStopBut_Click(object sender, EventArgs e)  //приостановка игры
        {

            if (gameStopBut.BackColor != Color.GreenYellow)
            {
                gameStopBut.BackColor = Color.GreenYellow;
                stopGame = true;
                for (int i = 0; i < MassGameZone.Count; i++)
                {
                    MassGameZone[i].stopGame();
                }
                gameStopBut.Text = "Игра приостановлена";
            }
            else
            {
                gameStopBut.BackColor = Button.DefaultBackColor;
                stopGame = false;
                for (int i = 0; i < MassGameZone.Count; i++)
                {
                    MassGameZone[i].ContinueGame();
                }
                gameStopBut.Text = "Игра идет";
            }
        }
        #endregion
        void refreshTable()//обновление таблицы зарегистрированных команд
        {
            NumRegKomand++;
            ListKomand.DataSource = dt;
            ListKomand.CurrentCell = null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Count() > 1)
            {
                f.Refresh();
                f.FormBorderStyle = FormBorderStyle.None;
                f.Location = Screen.AllScreens[0].Bounds.Location;
                f.Location.Offset(10, 10);
                f.WindowState = FormWindowState.Maximized;
                this.Location = Screen.AllScreens[1].Bounds.Location;
                button1.Text = "Монитор подключен";
                ToJS();
                f.Show();
            }
            else
            {
                f.WindowState = FormWindowState.Normal;
                f.Hide();
            }
        }
        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;

            if (grid.IsCurrentCellDirty && grid.CurrentCell.ColumnIndex == 8)
            {
                if (grid[grid.CurrentCell.ColumnIndex - 1, grid.CurrentCell.RowIndex].Value == DBNull.Value) grid.CancelEdit();
                else grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            else grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            var grid = sender as DataGridView;
            if (e.ColumnIndex == 7)
            {
                // DataGridViewImageCell cell = (DataGridViewImageCell)
                //  dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];


                // grid.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                //    (bool)grid[e.ColumnIndex, e.RowIndex].Value ? Color.Green : Color.Red;
            }
            if (e.ColumnIndex == 8)
            {
                // int stol = Convert.ToInt32( grid[1, e.RowIndex].Value);
                // string otvet = grid[6, e.RowIndex].Value.ToString();
                bool correct = (bool)grid[7, e.RowIndex].Value;
                int zona = Convert.ToInt32(grid[0, e.RowIndex].Value);
                MassGameZone[zona].checkOtvet(correct);
                dataGridView2.Rows.Remove(((DataGridView)sender).CurrentRow);
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var grid = sender as DataGridView;
                if (e.ColumnIndex == 7 && e.RowIndex > -1)
                {
                    if ((grid[7, e.RowIndex].Value == System.DBNull.Value) || (grid[7, e.RowIndex].Value == "Неправильно"))
                    {
                        grid[7, e.RowIndex].Value = "Правильно";
                        grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                        return;
                    }
                    if (grid[7, e.RowIndex].Value == "Правильно")
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        grid[7, e.RowIndex].Value = "Неправильно";
                        return;
                    }
                }
                if (e.ColumnIndex == 8 && e.RowIndex > -1)
                {
                    bool flag;

                    if (grid[7, e.RowIndex].Value == "Правильно")
                    {
                        flag = true;
                        bool cor = flag;
                        int zona = Convert.ToInt32(grid[0, e.RowIndex].Value);
                        MassGameZone[zona].checkOtvet(cor);
                        dataGridView2.Rows.Remove(((DataGridView)sender).CurrentRow);
                        return;
                    }

                    if (grid[7, e.RowIndex].Value == "Неправильно")
                    {
                        flag = false;
                        bool cor = flag;
                        int zona = Convert.ToInt32(grid[0, e.RowIndex].Value);
                        MassGameZone[zona].checkOtvet(cor);
                        dataGridView2.Rows.Remove(((DataGridView)sender).CurrentRow);
                        return;
                    }
                }
            }
            catch (IOException eX)
            {

                if (eX.Source != null)
                    Console.WriteLine("IOException source: {0}", eX.Source);
                throw;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            reloadDB();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            String text = textBox4.Text;
            string addTheme = "insert into themes(theme,required) values('" + text + "',0)";
            cm = new SQLiteCommand(addTheme, conn);
            cm.ExecuteNonQuery();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox6.Text = "";
            textBox2.Text = "";
            label2.Text = "";
            tbMediaFile.Text = "";
            questEditorGrid.Enabled = true;
        }
        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape && tabControl1.SelectedTab.Name.Equals("questEditor"))
                button3.PerformClick();
        }
        public string ReadString(string txtQuery)
        {
            
            using (SQLiteCommand cmd = new SQLiteCommand(txtQuery, conn))
            {
                object result = cmd.ExecuteScalar();
                return (result == null ? "" : result.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string saveRequest;
            string saveMySQLRequest;
            int lastId;
            if (label2.Text == "")
            {
                saveRequest = "insert or replace into quests (themeID,text,answer,media) values(" + indexOfThemes + ", '" + Crypt.Encrypt(textBox5.Text, key) + "', '" + Crypt.Encrypt(textBox6.Text, key) + "', '" + tbMediaFile.Text + "')";
                cm = new SQLiteCommand(saveRequest, conn);
                cm.ExecuteNonQuery();
                reloadDB();
                lastId = Convert.ToInt32(ReadString("SELECT ID from quests ORDER BY ID DESC LIMIT 1"));
                saveMySQLRequest = "SELECT user_id FROM compositions Where team_id = (SELECT team_id FROM compositions WHERE user_id = "+ textBox2.Text  +")";
                MySqlCommand mycm = new MySqlCommand(saveMySQLRequest, mycon);
                MySqlDataReader  read = mycm.ExecuteReader();
                DataTable mem = new DataTable();
                using (read)
                {
                    if (read.HasRows) mem.Load(read);
                }

                var members = mem.AsEnumerable().Select(r => r.Field<int>("user_id")).ToArray();
                foreach(int member in members)
                {
                 
                    SQLiteCommand cml = new SQLiteCommand(String.Format("INSERT INTO i_see (user_id, quest_id) values ({0}, {1})", member, lastId), conn);
                    cml.ExecuteNonQuery();
                }
            }
            else
            {
                saveRequest = "insert or replace into quests (ID,themeID,text,answer,media) values(" + label2.Text + "," + indexOfThemes + ", '" + Crypt.Encrypt(textBox5.Text, key) + "', '" + Crypt.Encrypt(textBox6.Text, key) + "', '" + tbMediaFile.Text + "')";
                cm = new SQLiteCommand(saveRequest, conn);
                cm.ExecuteNonQuery();
                reloadDB();
            }
            button3.PerformClick();
        }
        private void questEditorGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBox5.Text = questEditorGrid.Rows[questEditorGrid.CurrentRow.Index].Cells[1].Value.ToString();
            textBox6.Text = questEditorGrid.Rows[questEditorGrid.CurrentRow.Index].Cells[2].Value.ToString();
            label2.Text = questEditorGrid.Rows[questEditorGrid.CurrentRow.Index].Cells[0].Value.ToString();
            tbMediaFile.Text = questEditorGrid.Rows[questEditorGrid.CurrentRow.Index].Cells["qeFileColumn"].Value.ToString();
            textBox5.Focus();
            questEditorGrid.Enabled = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string saveRequest = "delete from quests where ID = " + questEditorGrid.Rows[questEditorGrid.CurrentRow.Index].Cells[0].Value.ToString();
            cm = new SQLiteCommand(saveRequest, conn);
            cm.ExecuteNonQuery();
            reloadDB();

            /* string sdf = "select id,text,answer from quests";
              cm = new SQLiteCommand(sdf, conn);
              rd = cm.ExecuteReader();
              DataTable tableofQuestion = new DataTable();
              using (rd)  //если есть данные, то записываем в таблицу dat
              {
                  if (rd.HasRows) tableofQuestion.Load(rd);
              }
              var text = tableofQuestion.AsEnumerable().Select(r => r.Field<string>("text")).ToArray();
              for(int i = 0; i < text.Length; i++)
              {
                  text[i] = Crypt.Encrypt(text[i], key);
              }
              var answer = tableofQuestion.AsEnumerable().Select(r => r.Field<string>("answer")).ToArray();
              for (int i = 0; i < answer.Length; i++)
              {
                 answer[i] = Crypt.Encrypt(answer[i], key);
              }
              string saveRequest;
              var id = tableofQuestion.AsEnumerable().Select(r => r.Field<Int64>("ID")).ToArray();

              for (int i = 0; i < text.Length; i++)
              {
                  //saveRequest = "insert or replace into quests (ID,text,answer) values(" + id[i] + ",'" + text[i] + "', '" + answer[i] + "')";
                  saveRequest = "update quests set text='"+text[i]+"' where id='"+id[i]+"'";
                  cm = new SQLiteCommand(saveRequest, conn);
                  cm.ExecuteNonQuery();
                  saveRequest = "update quests set answer='" + answer[i] + "' where id='" + id[i] + "'";
                  cm = new SQLiteCommand(saveRequest, conn);
                  cm.ExecuteNonQuery();
              }*/
        }
        private void reloadDB()
        {
            string question = "select quests.id,quests.text,quests.answer,quests.media from themes inner join quests on themes.id=quests.themeID WHERE quests.themeID = ' " + (comboBox1.SelectedIndex + 1) + "'";
            indexOfThemes = comboBox1.SelectedIndex + 1;
            cm = new SQLiteCommand(question, conn);
            rd = cm.ExecuteReader();
            DataTable tableofQuestion = new DataTable();
            using (rd)  //если есть данные, то записываем в таблицу dat
            {
                if (rd.HasRows) tableofQuestion.Load(rd);
            }
            var text = tableofQuestion.AsEnumerable().Select(r => r.Field<string>("text")).ToArray();
            var answer = tableofQuestion.AsEnumerable().Select(r => r.Field<string>("answer")).ToArray();
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = Crypt.Decrypt(text[i], key);
                answer[i] = Crypt.Decrypt(answer[i], key);
                tableofQuestion.Rows[i][1] = text[i];
                tableofQuestion.Rows[i][2] = answer[i];
            }

            questEditorGrid.DataSource = tableofQuestion;
            
            /*questEditorGrid.Enabled = true;
            textBox5.Text = "";
            textBox6.Text = "";
            label2.Text = "";*/
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            logsOfzones.Clear();
            List<string> icon = new List<string>();
            for (int i = 0; i < logsOftournaments.Count; i++)
            {
                if (String.Compare(comboBox2.SelectedItem.ToString(), logsOftournaments[i].dataLog.GameZone.ToString()) == 0)
                {
                    icon.Add(logsOftournaments[i].gmLog.iCon.ToString());
                    logsOfzones.Add(logsOftournaments[i]);
                }
            }
            comboBox4.DataSource = icon;
            comboBox4_SelectedIndexChanged(sender, e);
            //comboBox3_SelectedIndexChanged(sender, e);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (logList l in tour)
            {
                if (String.Compare(comboBox3.SelectedItem.ToString(), l.name) == 0)
                {
                    label9.Text = l.gameid;
                }

            }
            logsOftournaments.Clear();
            logsOfzones.Clear();

            string logs = "select * from logs where gameid = '"+ Int32.Parse(label9.Text) + "'";
            MySqlCommand cm = new MySqlCommand(logs, mycon);
            MySqlDataReader rd = cm.ExecuteReader();
            DataTable dat = new DataTable();
            using (rd)  //если есть данные, то записываем в таблицу dat
            {
                if (rd.HasRows) dat.Load(rd);
            }

            var log = dat.AsEnumerable().Select(r => r.Field<string>("command")).ToArray();
            List<string> gamezone = new List<string>();
            for (int i = 0; i < log.Length; i++)
            {
                SendLog l = JsonConvert.DeserializeObject<SendLog>(log[i]);
                logsOftournaments.Add(l);

            }
            for (int i = 0; i < logsOftournaments.Count; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    gamezone.Add(logsOftournaments[i].dataLog.GameZone.ToString());
                }

            }

            gamezone = new List<string>(gamezone.Distinct());

            comboBox2.DataSource = gamezone;
            comboBox2_SelectedIndexChanged(sender, e);
            comboBox4_SelectedIndexChanged(sender, e);



        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string questions = "";
            List<string> answer = new List<string>();
            List<string> icon = new List<string>();
            string themes = "";
            List<string> stavka = new List<string>();
            List<string> info = new List<string>();
            List<string> test = new List<string>();
            string ocher = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";

            for (int i = 0; i < logsOfzones.Count; i++)
            {

               // var answTeam = gz.gm.team.OrderBy(x => x.answerOrder);
                if (String.Compare(comboBox4.SelectedItem.ToString(), logsOfzones[i].gmLog.iCon.ToString()) == 0)
                {
                    themes = logsOfzones[i].gmLog.theme.ToString();
                    questions = logsOfzones[i].gmLog.quest.ToString();
                    //ocher = logsOfzones[i].gmLog.team[0].answerOrder+1.ToString() + " "+ logsOfzones[i].gmLog.team[1].answerOrder+1.ToString() + " " + logsOfzones[i].gmLog.team[2].answerOrder+1.ToString();
                    // answTeam.ElementAt(0).table.ToString() + " " + answTeam.ElementAt(1).table.ToString() + " " + answTeam.ElementAt(2).table.ToString();
                    
                    for (int j = 0; j < 3; j++)
                    {
                        int ansOrder = logsOfzones[i].gmLog.team[j].answerOrder + 1;
                        info.Add("Название команды - " + logsOfzones[i].dataLog.team[j].name + Environment.NewLine + "Игровой стол - " + logsOfzones[i].dataLog.team[j].table + Environment.NewLine + "Очередность ответа - " + ansOrder + Environment.NewLine + "Ответ на вопрос - " + logsOfzones[i].gmLog.team[j].answer + Environment.NewLine + "Ставка команды - " + logsOfzones[i].gmLog.team[j].stavka + Environment.NewLine + "Баланс IQash - " + logsOfzones[i].gmLog.team[j].iQash);
                    }

                }

            }
            info = new List<string>(info.Distinct());
            if (info.Count == 3)
            {
                richTextBox4.Text = String.Format("Тема вопроса: {1}{0}Текст вопроса: {2}{0}", Environment.NewLine, themes, questions);
                richTextBox1.Text = info[0];
                richTextBox2.Text = info[1];
                richTextBox3.Text = info[2];
            }

        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private enum Message : int
        {
            WM_SETREDRAW = 0x000B, // int 11
            SW_SHOWMAXIMIZED = 0x0003,
            SW_SHOW = 0x0005,
        }

        private void F_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из игры?", "Предупреждение!!!", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) e.Cancel = true;
        }

        private void ToJS()
        {
            List<teams> team = new List<teams>();
            for (int i = 0; i < MassGameZone.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    teams tm = new teams();
                    tm.name = MassGameZone[i].data.team[j].name.ToString();
                    tm.iQash = MassGameZone[i].gm.team[j].iQash;
                    team.Add(tm);
                }
            }
            string json = JsonConvert.SerializeObject(team);
            if (wb != null) wb.Document.InvokeScript("CreateListJson", new String[] { json });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListGamesView.ForceInitialize();
            // Restore the previously saved layout
            ListGamesView.MainView.RestoreLayoutFromXml(layoutFileName);

            wb = new WebBrowser
                {
                    Parent = f,
                    //Size = new Size(f.Width, f.Height),
                    AllowWebBrowserDrop = false,
                    IsWebBrowserContextMenuEnabled = false,
                    WebBrowserShortcutsEnabled = false,
                    ObjectForScripting = this,

                    Dock = DockStyle.Fill,
                };
                wb.Navigate(Application.StartupPath + @"\maxup\index.html");
        }

        //========================================================================================
        //Ручная жеребьевка игровых троек
        //========================================================================================
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void ListKomand_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = ListKomand.DoDragDrop(
                    ListKomand.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void ListKomand_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = ListKomand.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void ListKomand_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ListKomand_DragDrop(object sender, DragEventArgs e)
        {
            //Расположение мыши мы получаем относительно экрана, 
            //поэтому преобразуем их в координаты клиента.
            Point clientPoint = ListKomand.PointToClient(new Point(e.X, e.Y));

            //Получим индекс строки, над которой находится курсор мыши
            rowIndexOfItemUnderMouseToDrop =
                ListKomand.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            //Если текущая операция - перетаскивание, вставляем перетягиваемую строку 
            //в указанную пользователем позицию и удаляем исходную
            if (e.Effect == DragDropEffects.Move && rowIndexOfItemUnderMouseToDrop > -1)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                DataRow oldrow = ((DataRowView)rowToMove.DataBoundItem).Row;
                //Клонируем перетаскиваемую строку
                DataRow newrow = dt.NewRow();
                newrow.ItemArray = oldrow.ItemArray;
                dt.Rows.Remove(oldrow);
                dt.Rows.InsertAt(newrow, rowIndexOfItemUnderMouseToDrop);
                ListKomand.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
            }
        }

        private void ListKomand_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < ListKomand.RowCount)
            {
                //if (ListKomand.Rows[e.RowIndex].Cells[10].Value.ToString() == "1")
                    ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = ((e.RowIndex / 3) % 2) == 0 ? Color.GreenYellow : Color.SkyBlue;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            authForm.flush();
            authForm.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the layout to an XML file
            ListGamesView.MainView.SaveLayoutToXml(layoutFileName);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null)
                return;
            if (view.IsGroupRow(e.FocusedRowHandle))
            {
                bool expanded = view.GetRowExpanded(e.FocusedRowHandle);
                view.SetRowExpanded(e.FocusedRowHandle, !expanded);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            DBLink.PerformClick();
        }

        private void ListGamesGrid_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == GridMenuType.Row && e.HitInfo.InDataRow)
            {
                e.Menu.Items.Add(new DXMenuItem("Добавить новый тур", customItemClick, DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/addfile_32x32.png"))); // Properties.Resources.addfile_32x32));
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        private void customItemClick(object sender, EventArgs e)
        {
            String commentText = "";

            if (InputBox("Новый игровой тур", "Название нового тура:", ref commentText) == DialogResult.OK)
            {
                string sql = "Select Max(tour_id) FROM games WHERE tournament_id = '" + ListGamesGrid.GetRowCellValue(ListGamesGrid.FocusedRowHandle, ListGamesGrid.Columns["id"]) + "'";
                MySqlCommand cmd = new MySqlCommand(sql, mycon);
                cmd.ExecuteNonQuery();
                DataTable dat = new DataTable();

                using (MySqlDataReader tur = cmd.ExecuteReader())
                {
                    dat.Load(tur);
                }

                int game_num = Convert.ToInt32(dat.Rows[0][0]) + 1;
                sql = " insert into games (tournament_id, tour_id, game_name) value (" + ListGamesGrid.GetRowCellValue(ListGamesGrid.FocusedRowHandle, ListGamesGrid.Columns["id"]) + ", " + game_num + ",'" + commentText + "')";
                cmd = new MySqlCommand(sql, mycon);
                cmd.ExecuteNonQuery();

                GameButton.PerformClick();
                //string text = "" +
                //    ListGamesGrid.GetRowCellValue(ListGamesGrid.FocusedRowHandle, ListGamesGrid.FocusedColumn).ToString();
                //menuInfo.View.GetRowCellValue(menuInfo.RowHandle, menuInfo.Column).ToString();
                //Clipboard.SetText(text);
                //MessageBox.Show(text);

            }
        }

        //////////////////////////copy/////////////////////////////
        private async void Zapros()//получениеи обработка  запросов от команд
        {
            //IPEndPoint endpoint;
            while (true)
            {
                //var buffer = new ArraySegment<byte>(new byte[4096]);

                // Ожидаем данные от него
                try
                {
                        var result = await Udp.ReceiveAsync();
                        Application.DoEvents();
                        byte[] receiveBytes = result.Buffer;
                        byte[] bytes;
                        string txt = Encoding.UTF8.GetString(receiveBytes);
          
                         sw.WriteLine(txt);
                    if (!stopGame) //если игра не приостановлена, то ...
                        {
                            if (txt != null && txt.Length > 2)
                            {
                                endpoint = result.RemoteEndPoint;
                                string gm;

                                switch (txt.Substring(0, 3))
                                {
                                    #region zsp- запрос списка команд, если известна рассадка троек, то отправляется рассадка 
                                    case "zsp":
                                        if (RassadkaFlag)
                                        {
                                            string kluch = txt.Substring(3);  //ключ сессии в полученном сообщении                             
                                            for (int i = 0; i < MassGameZone.Count; i++)//перебором игровых зон находим кому принадлежит этот ключ
                                            {
                                                if (MassGameZone[i].inGameZone(kluch))//метод определения принадлежногсти ключа игровой зоне
                                                {
                                                    if (!MassGameZone[i].stopGm)//если игровая зона не приостановлена
                                                    {
                                                        // отправляем data клинету                                
                                                        bytes = Encoding.UTF8.GetBytes("osp" + JsonConvert.SerializeObject(MassGameZone[i].data));
                                                        Udp.Send(bytes, bytes.Length, endpoint);
                                                     //   textBox3.Text += "osp";
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            bytes = Encoding.UTF8.GetBytes("+sp" + JsonConvert.SerializeObject(dt));
                                            Udp.Send(bytes, bytes.Length, endpoint);
                                           // textBox3.Text += "+sp";
                                        }
                                        break;
                                    #endregion
                                    #region zst- запрос на старт игры
                                    case "zst":
                                        if (rgData.gameStart)
                                        {
                                            ResiveData dataZapros1 = new ResiveData();
                                            dataZapros1 = JsonConvert.DeserializeObject<ResiveData>(txt.Substring(3));
                                            if (MassGameZone.Count >= dataZapros1.uid && MassGameZone[dataZapros1.uid - 1].verify(dataZapros1.kluch, dataZapros1.table))
                                            {
                                                if (!MassGameZone[dataZapros1.uid - 1].stopGm)//если игровая зона не приостановлена
                                                {
                                                    gm = MassGameZone[dataZapros1.uid - 1].startGM();
                                                    bytes = Encoding.UTF8.GetBytes("ost" + gm);
                                                    Udp.Send(bytes, bytes.Length, endpoint);
                                                   /// textBox3.Text += "ost";
                                                }
                                            }

                                        }
                                        break;
                                    #endregion
                                    #region zgg- обработка шагов игры команды
                                    case "zgg":
                                        try
                                        {
                                        ResiveData dataZapros2 = new ResiveData();
                                        dataZapros2 = JsonConvert.DeserializeObject<ResiveData>(txt.Substring(3));
                                            if (MassGameZone.Count >= dataZapros2.uid && MassGameZone[dataZapros2.uid - 1].verify(dataZapros2.kluch, dataZapros2.table))
                                            {
                                                if (!MassGameZone[dataZapros2.uid - 1].stopGm)//если игровая зона не приостановлена
                                                {
                                                    MassGameZone[dataZapros2.uid - 1].Update(dataZapros2.step, dataZapros2.table, dataZapros2.otvet ?? "", dataZapros2.stavka, endpoint);
                                                }
                                            }
                                            ToJS();
                                        }
                                        catch (Exception e)
                                        {
                                            MessageBox.Show(e.Message, "Предупреждение!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                        break;
                                #endregion
                                #region zww- обработка ожидания
                                case "zww":
                                    ResiveData dataZapros3 = new ResiveData();
                                    dataZapros3 = JsonConvert.DeserializeObject<ResiveData>(txt.Substring(3));
                                        if (MassGameZone.Count >= dataZapros3.uid && MassGameZone[dataZapros3.uid - 1].verify(dataZapros3.kluch, dataZapros3.table))
                                        {
                                            if (!MassGameZone[dataZapros3.uid - 1].stopGm)//если игровая зона не приостановлена
                                            {
                                                MassGameZone[dataZapros3.uid - 1].wait(dataZapros3.step, dataZapros3.table, endpoint);
                                            }
                                        }
                                        break;
                                    #endregion
                                    #region ogg- обработка ответа команды
                                    case "ogg":
                                       Application.DoEvents();//////////////////////////////////////////////////////////////////////////////////????????????????????????????????
                                       ResiveData dataZapros = new ResiveData();
                                       dataZapros = JsonConvert.DeserializeObject<ResiveData>(txt.Substring(3));
                                        if (MassGameZone.Count >= dataZapros.uid && MassGameZone[dataZapros.uid - 1].verify(dataZapros.kluch, dataZapros.table)) //проверяем ключ сессии
                                        {
                                            if (!MassGameZone[dataZapros.uid - 1].stopGm)//если игровая зона не приостановлена
                                            {
                                                if (MassGameZone[dataZapros.uid - 1].getOtvet(dataZapros.table, dataZapros.otvet, dataZapros.step, endpoint)) //если ответа не было, то принимаем ответ
                                                {
                                                    string[] info = MassGameZone[dataZapros.uid - 1].otvetInfo();

                                                    DataRow row = dtVoprosCheck.NewRow();
                                                    row[0] = Convert.ToByte(info[0]);
                                                    row[1] = dataZapros.table;
                                                    row[2] = Convert.ToInt32(info[2]);
                                                    row[3] = info[3];
                                                    row[4] = info[4];
                                                    row[5] = info[5];
                                                    row[6] = dataZapros.otvet.Trim();
                                                    string[] arrayOfAnswer = info[5].Split(';');
                                                    bool check_answer = false;
                                                    foreach (string answer in arrayOfAnswer)
                                                    {
                                                        if (String.Compare(answer.Trim(), row[6].ToString(),true) == 0)
                                                        {
                                                            check_answer = true;
                                                            break; 
                                                        }         
                                                    }
                                                    if (check_answer)//if (String.Compare(row[5].ToString(), row[6].ToString(), true) == 0)
                                                    {
                                                        MassGameZone[dataZapros.uid - 1].checkOtvet(true);
                                                    }
                                                    else
                                                    {
                                                        dtVoprosCheck.Rows.Add(row);
                                                        dataGridView2.DataSource = dtVoprosCheck;
                                                        tabControl1.SelectedTab = tabControl1.TabPages["Control"];
                                                        this.Show();
                                                        this.Activate();
                                                    }
                                                }
                                            }
                                        }
                                        //bytes = Encoding.UTF8.GetBytes("wgg" + resDoo.uid); //ответ ожидание
                                        //Udp.Send(bytes, bytes.Length, endpoint);
                                        break;
                                    #endregion
                                    case "wgg":
                                    //    textBox3.Text += txt.Substring(3);
                                        break;
                                }
                            }
                        }
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}