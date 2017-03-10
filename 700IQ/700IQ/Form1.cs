using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using WMPLib;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace _700IQ
{
    public partial class GeneralForm : Form
    {
        #region //объявление переменных          
        Otvet otvetStatic;
        Conn cn;
        Padge pdg=new Padge();
        Rul ruletka;
        public Game steck = new Game();
        AutoCompleteStringCollection teamLst;
        Data predUs = new Data();
        Table tbl;
        IPAddress server=null;
       
        string kluch; //ключ игровой сессии
        int uidKomand;//идентификатор команды
        int tableOfKom;//стол команды
        private bool bIconFinalised = false;
        public Label errorLabel;
        public DataTable dt = new DataTable();
        public Size resolution; //System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;
        public int delta;
        private System.Timers.Timer MyTimer;
        private System.Windows.Forms.Timer gifTimer= new System.Windows.Forms.Timer();
        private static IniFile fIni = new IniFile(Application.StartupPath + "\\settings.ini");
        IPAddress IP = null;
        public static string infoOfserver;
        struct SendData //структурированные данные отправляемые серверу
        {
            public int uid;         //игровая зона
            public string kluch;    //ключ игровой сессии
            public byte table;      //номер стола
            public byte step;       //шаг игры
            public string otvet;    //ответ
            public int stavka;      //ставка
        }
        #endregion

        public GeneralForm()
        {
            
            InitializeComponent();
            //this.KeyDown += GeneralForm_KeyDown;
            //this.KeyPress += p.WorkForm_KeyDown;
        }

    
        #region//процедуры инициализации
        //Функция получения прозрачного изображения //////////////////////////////////////////////////////////////////
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private IPAddress ServerSearch()
        {
            IPAddress[] IPs = new IPAddress[5];
            /*IPs[0] = IPAddress.Parse(fIni.IniReadValue("Settings", "Server1", "127.0.0.1"));
            IPs[1] = IPAddress.Parse(fIni.IniReadValue("Settings", "Server2", "127.0.0.1"));
            IPs[2] = IPAddress.Parse(fIni.IniReadValue("Settings", "Server3", "127.0.0.1"));
            IPs[3] = IPAddress.Parse(fIni.IniReadValue("Settings", "Server4", "127.0.0.1"));
            IPs[4] = IPAddress.Parse(fIni.IniReadValue("Settings", "Server5", "127.0.0.1"));*/
            IP = null;
            string response = "";
            string datagram = "hi";
            int correct_server = 0;

                try
                {
                    for (int i = 0; i < IPs.Count(); i++)
                    {
                        using (var tcpClient = new TcpClient())
                        {
                            IPs[i] = IPAddress.Parse(fIni.IniReadValue("Settings", string.Format("Server{0}", i + 1), "127.0.0.1"));
                            if (tcpClient.ConnectAsync(IPs[i], 2050).Wait(200))
                            {
                                using (var networkStream = tcpClient.GetStream())
                                {
                                    byte[] result;
                                    result = Encoding.UTF8.GetBytes(datagram);
                                    networkStream.Write(result, 0, result.Length);

                                    var buffer = new byte[4096];
                                    var byteCount = networkStream.Read(buffer, 0, buffer.Length);
                                    response = Encoding.UTF8.GetString(buffer, 0, byteCount);

                                    if (response == "700iq")
                                    {
                                        correct_server++;
                                        if (IP == null) IP = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address;
                                        networkStream.Close();
                                        tcpClient.Close();
                                        break;
                                    }
                                    networkStream.Close();
                                }
                            }
                            tcpClient.Close();
                        }
                    }
                }
                catch (Exception ex)
                { }
    /*            finally
                {
                    tcpClient.Close();
                }*/
                if (IP == null) dataReceive("Error");
             //   if (IP != null) MessageBox.Show(IP.ToString());
                return IP;
        }
        void IniScreen()//Инициализация начальной картинки и определение размеров экрана 
        {
            #region //описание графики начальной заставки
            this.Controls.Clear();                                      //очистка экрана
            Image bmp = Properties.Resources.nz;                        //начальная заставка
            resolution = GetWorkingClientSize(this);                    //new Size(koorX, this.ClientSize.Height);
            //int koorX = (int)(this.ClientSize.Height * 1.5625f);        //приведенная ширина картинки на экран
            //int koorY = this.ClientSize.Height;                       // высота экран
            Size maxClientSize = GetMaximizedClientSize(this);
            Bitmap bmpNew = new Bitmap(Properties.Resources.fon, maxClientSize.Width, maxClientSize.Height);
            Graphics g = Graphics.FromImage(bmpNew);
            delta = (maxClientSize.Width - resolution.Width) / 2;               //координата х для рисования          
            g.DrawImage(bmp, delta, 0, resolution.Width, resolution.Height); // рисуем картинку в масштабе
            g.Dispose();
            this.BackgroundImage = bmpNew;
            bmp.Dispose();
            // bmpNew.Dispose();
            #endregion

            axWindowsMediaPlayer1.Visible = false;
            #region //описание кнопки входа
            Point pn = NewPoint(1060, 691);
            pn.X += delta < 0 ? delta : 0;
            bmp = Properties.Resources.rotor;
            PictureBox pcBox = new PictureBox()
            {
                Parent = this,
                Name = "oneuse",
                Visible = true,
                Location = pn,
                BackColor = Color.Transparent,
                Size = NewSizeKv(390),
                Image = bmp,
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            
            pcBox.Click += onClickMedal;
           
            #endregion
        }

        private void dataReceive(string response)
        {
            try
            {
                switch (response.Substring(0, 5))
                {
                    case "regOk":
                        ini3(response.Substring(5));
                        break;
                    case "Teams":
                        teamLst = JsonConvert.DeserializeObject<AutoCompleteStringCollection>(response.Substring(5));
                        Ini1();
                        break;
                    case "False":
                        MessageBox.Show("Неверный логин или пароль!");
                        break;
                    case "IPtwo":
                        MessageBox.Show("Повторный IP-адресс");
                        break;
                    case "Error":
                        if (errorLabel == null)
                        {
                            errorLabel = new Label()
                            {
                                Parent = this,
                                Name = "oneuse",
                                Text = "Сервер недоступен или процедура регистрации не начата",
                                AutoSize = true,
                                Font = new Font("Arial ", NewFontSize(10)),
                                ForeColor = Color.Gold,
                                BackColor = Color.Transparent,
                                Location = NewPoint(2000, 1560),
                            };

                            MyTimer = new System.Timers.Timer();
                            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
                            MyTimer.AutoReset = false;
                        }

                        MyTimer.Interval = 5000;
                        MyTimer.Start();
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
        }
        public void timer1_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            //this.Invoke(new MethodInvoker(() =>
            //{
            //    errorLabel.Dispose();
            //    errorLabel = null;
            //}));
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    timer1_Tick(sender, e);
                });
            }
            else
            {
                errorLabel?.Dispose();
                errorLabel = null;
            }
        }

        private void RemoveTempControls()
        {
            //var namedControls = this.Descendants().Where(ctrl => ctrl.Name = "NameToFind");
            /*this.Invoke(new MethodInvoker(() =>
            {
                foreach (Control t in this.Controls.Find("oneuse", true))
                    this.Controls.Remove(t);
            }));*/
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    RemoveTempControls();
                });
            }
            else
            {
                foreach (Control t in this.Controls.Find("oneuse", true))
                    this.Controls.Remove(t);
                this.Invalidate();
            }
        }
        void onClickMedal(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = ServerSearch();
            }
            
                if ((teamLst == null)&&(server!=null))
                {
                    Connection connect = new Connection(server);
                    connect.onDataReceive += dataReceive;
                    connect.Send("tm");
                }
                else
                {
                   // Ini1();
                }
            
        }
        void Ini1()//ввод логина и пароля
        {
            
            //int ctrCount = Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) this.Controls.RemoveByKey("oneuse");    //очистка экрана
            RemoveTempControls();

            #region //описание графики экрана для            
            Bitmap bmp =new Bitmap(Properties.Resources.shtorka1, resolution);          //начальная заставка                                                       
            #endregion
            #region//описание полей ввода логин и пароль
            Label lb = new Label()
            {
                Parent = this,
                Name = "oneuse",
                Size = resolution,
                Image = bmp,
                BackColor = Color.Transparent,
                Location = new Point(delta, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            TextBox logintb = new TextBox()
            {
                Parent = lb,
                Name = "login",
                AutoSize = false,
                Size = NewSize(300, 60),
                Location = NewRelPoint(1175, 780),
                BackColor = Color.LightGray,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.CustomSource,
                AutoCompleteCustomSource = teamLst,
                BorderStyle = BorderStyle.None,
                Font = new Font("Cambria", NewFontSize(20)),
                TextAlign = HorizontalAlignment.Left,
                MaxLength = 12, 
                AcceptsReturn=false,
            };
            TextBox paroltb = new TextBox
            {
                Parent = lb,
                AutoSize = false,
                Size = NewSize(300, 60),
                Location = NewRelPoint(1175, 928),            
                BackColor=Color.LightGray,
                BorderStyle=BorderStyle.None,
                Font = new Font("Cambria", NewFontSize(20)),             
                TextAlign = HorizontalAlignment.Left,
                PasswordChar = '*',
                MaxLength = 12,
                Name = "parol" ,
            };
            #endregion
            #region//описание кнопки ввода
            bmp = Properties.Resources.Активная;
            PictureBox pcBox = new PictureBox()
            {
                Parent = lb,
                Location = NewRelPoint(1150, 1100),
                BackColor = Color.Transparent,
                Size = NewSize(200, 200),
                Image = bmp,
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            pcBox.Click += Ini2;
            #endregion                     
            logintb.KeyDown += logintb_KeyDown;
            paroltb.KeyDown += logintb_KeyDown;
            logintb.Focus();
          
        }
        private void logintb_KeyDown(object sender, KeyEventArgs e) //Обработка нажатия клавиш в полях ввода
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (((Control)sender).Name == "login")
                    this.ProcessTabKey(true);
                else
                    Ini2(sender, e);
            }
        }
        void Ini2(object sender, EventArgs e)//проверка на корректность ввода логина и пароля
        {
            /*List<Control> alltextbox = new List<Control>();
            GetAllTypedControls(this, alltextbox, typeof(TextBox));
            bool loginOK = true;
            string parol="";
            string login="";
            foreach (TextBox tb in alltextbox)
            {
                if (tb.Name == "parol")
                    if ( tb.Text != "") parol = tb.Text;
                    else loginOK = false;
                if (tb.Name == "login" )
                    if ( tb.Text != "") login = tb.Text;
                    else loginOK = false;
            }*/
            Control login = this.Controls.Find("login", true).FirstOrDefault();
            Control parol = this.Controls.Find("parol", true).FirstOrDefault();
            if (login!= null && parol!=null)
            {
                if (!String.IsNullOrEmpty(parol.Text) && !String.IsNullOrEmpty(login.Text))
                {
                    Connection cnn = new Connection(server);
                    string[] info = { login.Text, getSHAHash(parol.Text) };
                    cnn.onDataReceive += dataReceive;
                    cnn.Send("rg" + JsonConvert.SerializeObject(info));
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                    login.Focus();
                }
            }
        }
        void ini3(string response)//информация о зарегистрированной команде и ее членах
        {
            //int ctrCount = Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) this.Controls.RemoveByKey("oneuse");//очистка экрана от временных элементов          
            RemoveTempControls();
            predUs = JsonConvert.DeserializeObject<Data>(response);
            kluch = predUs.team[0].kod;
            uidKomand = predUs.team[0].uid;
            Bitmap bmpNew = new Bitmap(Properties.Resources.GreenTable, resolution.Width + delta * 2, resolution.Height);
            Graphics g = Graphics.FromImage(bmpNew);
            g.DrawString("Добро пожаловать", new Font("Times New Roman", NewFontSize(40), FontStyle.Italic), Brushes.White, NewPoint(950, 50));
            g.DrawString("в интеллект-казино 700 IQ!", new Font("Times New Roman", NewFontSize(30), FontStyle.Regular), Brushes.White, NewPoint(950, 200));
            infoOfserver = "Ваш игровой сервер " + IP.ToString();
            // g.DrawString(infoOfserver, new Font("Times New Roman", NewFontSize(30), FontStyle.Regular), Brushes.White, NewPoint(950, 700));
            g.DrawImage(Properties.Resources.Печать_с_тенью, new Rectangle(NewPoint(100, 100),NewSizeKv(500)));
            g.DrawString(predUs.city+"  -  "+predUs.NumberGame+" -  "+predUs.Tur, new Font("Times New Roman", NewFontSize(15), FontStyle.Italic), Brushes.White, NewPoint(130, 600));
            this.BackgroundImage = bmpNew;

            #region//описание поля с информацией о команде
            string spis="Ваша команда  \"" + predUs.team[0].name+"\"  зарегистрирована!\n  " + predUs.NumberGame + " в городе "
                + predUs.city + "  " + predUs.Tur;
          
            Label lb = new Label()
            {
                Location = NewPoint(800, 400),
                Size = NewSize(1600, 250),
                Parent = this,
                Text = spis,
                Font = new Font("Monotype Corsiva", NewFontSize(35), FontStyle.Italic),
                BackColor = Color.Transparent,
                ForeColor = Color.WhiteSmoke,
                Name = "oneuse",
            };
            #endregion
            #region//описание поля с информацией о членах команды
            spis = "";
            int i = 0;
            foreach (var member in predUs.team[0].member)
            {
                spis += member == null ? "" : String.Format("{0}. {1} {2}{3}", ++i, member.F, member.N, Environment.NewLine);
            }
            /*for (int i = 0; i < 5; i++)
            {
                spis += (i+1)+". "+predUs.team[0].member[i].F + " " + predUs.team[0].member[i].N + "\n";
            }*/
            Label lb1 = new Label()//список команд
            {
                Location = NewPoint(1100, 700),
                Size = NewSize(1200, 550),
                Parent = this,
                Text = spis,
                Font = new Font("Monotype Corsiva", NewFontSize(20), FontStyle.Italic),
                BackColor = Color.Transparent,
                ForeColor = Color.WhiteSmoke,
                Name = "oneuse",
            };
            #endregion
            #region начало игры
            Label lbr = new Label()//--------------------игра начнется
            {
                Parent = this,
                Visible = true,
                Location = NewPoint(80, 800),
                TextAlign = ContentAlignment.TopCenter,
                Size = NewSize(500, 100),
                Name = "oneuse",
                Text = "Игра начнется",
                BackColor = Color.Transparent,
                Font = new Font("Cambria ", NewFontSize(20)),
                ForeColor = Color.White

            };
            Label lbrn = new Label()//время
            {
               
                Parent = this,
                Visible = true,
                Location = NewPoint(80, 890),
                Name = "oneuse",
                TextAlign = ContentAlignment.TopCenter,
                Size = NewSize(500, 300),
                Text = predUs.startTime.ToString("t"),
                BackColor = Color.Transparent,
                Font = new Font("Cambria ", NewFontSize(25)),
                ForeColor = Color.White
            };
            #endregion
         

            Polosa pol = new Polosa();
            pol.onPolosaEnd += ini4;
            pol.polosa(500, NewPoint(1600, 1350), this, "ini3");
           
        }      
        void ini4()//вывод спсок зарегистрированных команд
        {
            cn = new Conn(server);
            cn.onNewKom += DoIt;
            //cn.SendUDP("zsp"+predUs.team[0].kod); 
            cn.start("zsp" + predUs.team[0].kod, 3000);
            //pdg.spisokOut(this, dt, predUs);    //обновление списка зарегистрировавшихся команд
            //spisokOut(this, dt, predUs);    //обновление списка зарегистрировавшихся команд
        }



        public void spisokOut(Form fm, DataTable dt, Data predUs)//вывод спсок зарегистрированных команд
        {
            //    if (iniEnd) return;
            RemoveTempControls();
            //int ctrCount = fm.Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) fm.Controls.RemoveByKey("oneuse");//очистка экрана от временных элементов
            string spis = "";
            //= "Зарегистрировавшиеся команды:\n\n";          
            int ii = 1;
            string tab = "";
            DataRow[] dtRow = dt.Select();
            for (int i = 0; i < dtRow.Length; i++)
            {
                spis += ii.ToString() + ". " + dtRow[i][2];
                if ((ii.ToString() + ". " + dtRow[i][2]).Length < 9) tab = "\t\t\t\t\t\t";
                else if ((ii.ToString() + ". " + dtRow[i][2]).Length < 13) tab = "\t\t\t\t\t";
                else if ((ii.ToString() + ". " + dtRow[i][2]).Length < 17) tab = "\t\t\t\t";
                else if ((ii.ToString() + ". " + dtRow[i][2]).Length < 21) tab = "\t\t\t";
                else if ((ii.ToString() + ". " + dtRow[i][2]).Length < 25) tab = "\t\t";
                else if ((ii.ToString() + ". " + dtRow[i][2]).Length < 29) tab = "\t";
                spis += tab + dtRow[i][4] + "\n";
                ii += 1;
            };
            #region//описание полей вывода информации список команд
            //fm.Invoke(
            //    new MethodInvoker(() =>
            //    {
                    Label zagolovok = new Label
                    {
                        Parent = this,
                        Name = "oneuse",
                        BackColor = Color.Transparent,
                        ForeColor = Color.White,
                        Font = new Font("times new roman", NewFontSize(20), FontStyle.Italic),
                        Text = "Зарегистрировавшиеся команды:",
                        Location = NewPoint(800, 350),
                        Size = NewSize(700, 100),
                    };
                    RichTextBox lb = new RichTextBox()//список команд
                    {
                        Location = NewPoint(800, 520),
                        Size = NewSize(1200, 900),
                        Parent = this,
                        Text = spis,
                        Font = new Font("times new roman", NewFontSize(25), FontStyle.Italic),//Courier New
                        BackColor = Color.Green,
                        ForeColor = Color.WhiteSmoke,
                        Name = "oneuse",
                        Multiline = true,
                    };

                    #endregion
                    #region начало игры
                    Label lbr = new Label()//--------------------игра начнется
                    {
                        Parent = this,
                        Visible = true,
                        Location = NewPoint(80, 800),
                        TextAlign = ContentAlignment.TopCenter,
                        Size = NewSize(500, 100),
                        Name = "oneuse",
                        Text = "Игра начнется в",
                        BackColor = Color.Transparent,
                        Font = new Font("Cambria ", NewFontSize(20)),
                        ForeColor = Color.White

                    };
                    Label lbrn = new Label()//время
                    {
                        Parent = this,
                        Visible = true,
                        Location = NewPoint(80, 890),
                        Name = "oneuse",
                        TextAlign = ContentAlignment.TopCenter,
                        Size = NewSize(500, 300),
                        Text = predUs.startTime.ToString("t"),
                        BackColor = Color.Transparent,
                        Font = new Font("Cambria ", NewFontSize(25)),
                        ForeColor = Color.White
                    };
                //}
                //));
            #endregion
        }



    void ini5()//команды  игровой зоны 
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ini5();
                });
            }
            else
            {
                RemoveTempControls();
                #region//описание поля вывода информации список команд 
                string spis = "Ваша игровая зона  -  " + predUs.GameZone;
                Label lb = new Label()//список команд
                {
                    Location = NewPoint(800, 420),
                    Size = NewSize(800, 100),
                    Parent = this,
                    Text = spis,
                    Font = new Font("Times New Roman", NewFontSize(24), FontStyle.Bold),
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Name = "oneuse",
                };
                #endregion
                #region//описание участников команд 
                Label kom1 = new Label()//-------------------1 команда------------
                {
                    Location = NewPoint(870, 700),
                    Size = NewSize(700, 150),
                    Parent = this,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Cambria ", NewFontSize(20), FontStyle.Bold),
                    Text = predUs.team[0].table + " стол      " + predUs.team[0].name,
                    Name = "oneuse",
                };
                Label kom2 = new Label()//--------------- 2 команда -----------------
                {
                    Location = NewPoint(870, 850),
                    Size = NewSize(700, 150),
                    Parent = this,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Cambria ", NewFontSize(20), FontStyle.Bold),
                    Text = predUs.team[1].table + " стол       " + predUs.team[1].name,
                    Name = "oneuse",
                };
                Label kom3 = new Label()//-----------3 команда---------------------
                {
                    Location = NewPoint(870, 1000),
                    Size = NewSize(700, 150),
                    Parent = this,
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Cambria ", NewFontSize(20), FontStyle.Bold),
                    Text = predUs.team[2].table + " стол       " + predUs.team[2].name,
                    Name = "oneuse",
                };
                #endregion
                #region//остаток времени до начала игры
                Label lbr = new Label()//--------------------games time start
                {
                    Parent = this,
                    Visible = true,
                    Location = NewPoint(80, 800),
                    TextAlign = ContentAlignment.TopCenter,
                    Size = NewSize(500, 100),
                    Name = "oneuse",
                    Text = "Игра начнется в",
                    BackColor = Color.Transparent,
                    Font = new Font("Cambria ", NewFontSize(20)),
                    ForeColor = Color.White

                };
                Label lbrn = new Label()//время
                {
                    Parent = this,
                    Visible = true,
                    Location = NewPoint(80, 890),
                    Name = "oneuse",
                    TextAlign = ContentAlignment.TopCenter,
                    Size = NewSize(500, 300),

                    Text = predUs.startTime.ToString("t"),

                    BackColor = Color.Transparent,
                    Font = new Font("Cambria ", NewFontSize(25)),
                    ForeColor = Color.White
                };
                #endregion
                for (int i = 0; i < 3; i++)
                {
                    if (predUs.team[i].uid == uidKomand) tableOfKom = predUs.team[i].table - 1;
                }
            }
            //}));
        }
            void ini6()//старт игры (видеоролик)
        {
            RemoveTempControls();
            this.BackgroundImage = Properties.Resources.GreenTable;
            //axWindowsMediaPlayer1.Size = new Size(resolution.Width, resolution.Height);
            //axWindowsMediaPlayer1.Location = (new Point(0, 0));
            //axWindowsMediaPlayer1.Visible = true;
            //axWindowsMediaPlayer1.URL = "D:\\3D_mesh\\материалы\\video giraf.mp4";
         //   NextStep();
        }
        #endregion

        void DoIt(string komanda)
        {           
            switch (komanda.Substring(0, 3))
            {            
                #region case +sp - список зарегистрировавшихся команд
                case "+sp":
                    dt = JsonConvert.DeserializeObject<DataTable>(komanda.Substring(3));
                    pdg.spisokOut(this, dt, predUs);
                    break;
                    //case osp -тройка 
                case "osp": //окончание получения списка и получение данных по тройкам
                    //cn.stop();
                    predUs = JsonConvert.DeserializeObject<Data>(komanda.Substring(3));
                    ini5();
                    SendData sendD = new SendData();
                    sendD.table =(byte)tableOfKom;
                    sendD.kluch = predUs.team[tableOfKom].kod;
                    sendD.uid = predUs.GameZone;
                    cn.start("zst" + JsonConvert.SerializeObject(sendD),3000);//запрос на начало игры
                    break;
                #endregion
                #region case ost - команда старт игровой зоны
                case "ost":
                    cn.stop();
                    //сделать  запуск выидеоролика
                    //ini6();
                    steck = JsonConvert.DeserializeObject<Game>(komanda.Substring(3));
                    Step1();
                    break;
                #endregion
                case "ogg":              
                    steck = JsonConvert.DeserializeObject<Game>(komanda.Substring(3));
                    Debug.WriteLine(komanda);
                    if(!bIconFinalised)
                        CheckSteck();              
                    break;
                #region case owt - ожидание
                case "oww":
                    steck = JsonConvert.DeserializeObject<Game>(komanda.Substring(3));
                    Debug.WriteLine(komanda);
                    //CheckSteck();
                    break;
                #endregion
                case "err":
                    IniScreen();
                    break;
            }
        }
        private void CheckSteck() //потактовая обработка шага инстукции
        {
            switch (steck.step)
            {
                case 1:
                    Step1_3();
                    break;
                case 2:
                    Step2();
                    break;
                case 3:
                    Step3();
                    break;
                case 5:
                    Step5();
                    break;
                case 6:
                    Step6();
                    break;
                case 7:
                    Step7();
                    break;
                //default:
                //    NextStep();                                 
                //    break;
            }
        }

        #region  шаги игры
          #region шаг 1 - рассадка, список тем, игровой стол, запрос "готов" к началу айкона
        void Step1()    // шаг1 заставка с инфо о рассадке команд
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step1();
                });
            }
            else
            {
                RemoveTempControls();
                tbl = new Table(predUs, tableOfKom, this);
                if (steck.iCon > 12)
                {
                    Step10();
                    return;
                }
                tbl.Rassadka(steck);
                tbl.SetInfoTopStroka();

                Polosa pol = new Polosa();
                pol.onPolosaEnd += Temy;
                pol.polosa(500, NewPoint(1600, 1350), this, "Step1");
            }         
        }     
        void Temy()     //  заставка с инфо о темах вопросов на игру
        {   
            tbl.SetInfoTemy();
            Polosa pol = new Polosa();
            pol.onPolosaEnd += Step1_3;
            pol.polosa(500, NewPoint(1600, 1350),this, "Temy");
         
        }
        void Step1_3()  // заставка игрового стола 
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step1_3();
                });
            }
            else
            {
                BackgroundImage = tbl.SetIQ(steck, tableOfKom);
                Label lbStart = new Label()
                {
                    Name = "oneuse",
                    Location = NewPoint(1650, 1200),
                    Size = NewSize(450, 100),
                    Text = "К " + steck.iCon + " айкону готов!",
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Calibri", NewFontSize(20), FontStyle.Bold),
                    Parent = this,
                };
                Label iQon = new Label()
                {
                    Name = "Iqon",
                    Location = NewPoint(1160, 30),
                    Text = steck.iCon + " айкон",
                    AutoSize = true,
                    Font = new Font("Arial ", NewFontSize(22)),
                    ForeColor = Color.Gold,
                    Parent = this,
                    BackColor = Color.Transparent,
                };

                Polosa pol = new Polosa();
                pol.onPolosaEnd += Step1_4;
                pol.polosa(200, NewPoint(1600, 1350), this, "Step1_3");
                this.Invalidate();
            }
        }
        void Step1_4()  //сообщение серверу --- готов!!!
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step1_4();
                });
            }
            else
            {
                SendData gotov = new SendData();
                gotov.uid = predUs.GameZone;
                gotov.table = (byte)tableOfKom;
                gotov.step = 1;
                gotov.otvet = "gotov";
                gotov.kluch = kluch;
                cn.SendUDP("zgg" + JsonConvert.SerializeObject(gotov));
            }
        }
        #endregion
          #region шаг 2 - отбивка айкона, выбор темы, прием ставок, запрос "ставки"
        void Step2()    //отбивка номер айкона и розыгрыш темы вопроса 
        {
            //int ctrCount = Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) this.Controls.RemoveByKey("oneuse");//очистка экрана от временных элементов
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step2();
                });
            }
            else
            {
                RemoveTempControls();
                IkonShow ik = new IkonShow();
                ik.Ikon(this, steck.iCon);
                ik.onStop += Step2_2;
            }
        }
        void Step2_2()  //запуск рулетки для определения темы вопроса
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step2_2();
                });
            }
            else
            {
                tbl.TemaShow(steck, true);
                //this.Controls["Iqon"].Text = steck.iCon + " айкон";
                Rectangle kv = new Rectangle(NewPoint(800, 150), NewSizeKv(900));
                ruletka = new Rul();
                ruletka.StartRul(steck.Cell, kv, this, 2); //2 ячейка ??? надо ли??
                ruletka.onStop += Step2_3;
                this.Invalidate();
            }
        }
        void Step2_3()  //Выпавшая тема вопроса  и прием ставок
        {
            //int ctrCount = Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) this.Controls.RemoveByKey("oneuse");//очистка экрана от временных элементов
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step2_3();
                });
            }
            else
            {
                RemoveTempControls();
                tbl.TemaShow(steck, false);
                GetStavka st = new GetStavka();
                int MaxStavka = Math.Min(steck.team[tableOfKom].iQash-(12 - steck.iCon) * 25, 300);
                //if (MaxStavka > 300) MaxStavka = 300;
                st.stavka(25, MaxStavka, this);
                st.onStavka += doStavka;
            }     
        }
        public void doStavka(int st)//делегат сигнализирующий о сделанных ставках
        {
            SendData sd = new SendData();
            sd.uid = predUs.GameZone;
            sd.table = (byte)tableOfKom;
            sd.step = 2;
            sd.stavka = st;
            sd.otvet = "stavka";
            sd.kluch = kluch;
            cn.SendUDP("zgg" + JsonConvert.SerializeObject(sd));
        }
        #endregion
          #region шаг 3
        void Step3()    //показ ставок команд
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step3();
                });
            }
            else
            {
                if(ruletka != null)
                    ruletka.close();
                ruletka = null;
                int i = steck.Cell;
                BackgroundImage = tbl.SetIQ(steck, tableOfKom);
                StavkiShow stShow = new StavkiShow();
                stShow.onStShow += Step3_1;
                stShow.inputStavki(steck.team[0].stavka, steck.team[1].stavka, steck.team[2].stavka, 0, this);
            }
        }
        void Step3_1()  //показ вопроса, запуск рулетки
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step3_1();
                });
            }
            else
            {
                RemoveTempControls();
                Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, resolution);
                Graphics g = Graphics.FromImage(bmp);
                this.BackgroundImage = bmp;
                //this.Controls["Iqon"].Text = steck.iCon + " айкон";
                otvetStatic = new Otvet(cn, predUs, tableOfKom, this);
                otvetStatic.svitok(steck, predUs);
                ruletka = new Rul();
                ruletka.StartRul(steck.Cell, new Rectangle(NewPoint(1640, 100), NewSizeKv(1000)), this);
                ruletka.onStop += Step4; //остановка рулетки отрисовка очереди
            }          
        }
        void Step4()    //показ очереди и передача хода первой команде
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step4();
                });
            }
            else
            {
                if (steck.Cell != 0)
                {
                    otvetStatic.ochered(steck);
                    otvetStatic.semafor(1);
                    otvetStatic.focus();

                    if (steck.activeTable == tableOfKom + 1)//если ответ моей команды, то запускаем таймер
                    {
                        //Debug.WriteLine();
                        otvetStatic.polosaStart(this, 4);
                        //otvetStatic.onSendOtvet += NextStep;
                    }
                    else //если не мой ответ, то ждем следующей команды сервера
                    {
                        SendData sd = new SendData();
                        sd.kluch = kluch;
                        sd.table = (byte)tableOfKom;
                        sd.uid = predUs.GameZone;
                        sd.step = 4;
                        cn.SendUDP("zww" + JsonConvert.SerializeObject(sd));
                    }
                }
                else
                {
                    bIconFinalised = true;
                    Graphics g = this.CreateGraphics();
                    g.DrawString("ЗЕРРО, Господа!", new Font("Cambria ", NewFontSize(20)), Brushes.Black, NewPoint(1403, 955));
                    g.DrawString("ЗЕРРО, Господа!", new Font("Cambria ", NewFontSize(20)), Brushes.White, NewPoint(1400, 950));
                    g.DrawString("Ваши ставки сгорели!", new Font("Cambria ", NewFontSize(20)), Brushes.Black, NewPoint(1403, 1055));
                    g.DrawString("Ваши ставки сгорели!", new Font("Cambria ", NewFontSize(20)), Brushes.White, NewPoint(1400, 1050));
                    Polosa pol = new Polosa();
                    pol.onPolosaEnd += Step9;
                    pol.polosa(100, NewPoint(1600, 1350), this, "Step4 - Zero");
                }
            }
        }
        #endregion
        void Step5()    //получение ответа 1 команды 
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step5();
                });
            }
            else
            {
                if(ruletka != null)
                    ruletka.close();
                ruletka = null;
                otvetStatic.semafor(0);
                otvetStatic.answer(1, steck.team[steck.o1 - 1].answer, steck.correct);// вывод ответа первой команды

                if (!steck.correct)//если ответ не верный
                {
                    // otvetStatic.mistake(1, steck.team[steck.o1 - 1].answer);//не правильный ответ первой команды в очереди

                    otvetStatic.semafor(2);
                    otvetStatic.focus();

                    if (steck.activeTable == tableOfKom + 1)//если ответ моей команды, то запускаем таймер
                    {
                        otvetStatic.polosaStart(this, 5);
                        //otvetStatic.onSendOtvet += NextStep;
                    }
                    else
                    {
                        //если не мой ответ, то ждем следующей команды сервера
                        SendData sd = new SendData();
                        sd.kluch = kluch;
                        sd.table = (byte)tableOfKom;
                        sd.uid = predUs.GameZone;
                        sd.step = 5;
                        cn.SendUDP("zww" + JsonConvert.SerializeObject(sd));
                    }
                }
                else
                {
                    bIconFinalised = true;
                    StavkiShow stShow = new StavkiShow();
                    stShow.onStShow += Step9;//переход на окончание айкона
                    int stav = steck.team[steck.o1 - 1].stavka;
                    stShow.inputStavki(stav, stav, stav, stav, this);
                }
            }
        }
        void Step6()    //получение ответа от второй команды 
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step6();
                });
            }
            else
            {
                otvetStatic.semafor(0);
                otvetStatic.answer(2, steck.team[steck.o2 - 1].answer, steck.correct);// вывод ответа второй команды

                if (!steck.correct)//если ответ не верный
                {
                    // otvetStatic.mistake(1, steck.team[steck.o1 - 1].answer);//не правильный ответ первой команды в очереди

                    otvetStatic.semafor(3);
                    otvetStatic.focus();

                    if (steck.activeTable == tableOfKom + 1)//если ответ моей команды, то запускаем таймер
                    {
                        otvetStatic.polosaStart(this, 6);
                        // otvetStatic.onSendOtvet += NextStep;
                    }
                    else
                    {
                        //если не мой ответ, то ждем следующей команды сервера

                        SendData sd = new SendData();
                        sd.kluch = kluch;
                        sd.table = (byte)tableOfKom;
                        sd.uid = predUs.GameZone;
                        sd.step = 6;
                        cn.SendUDP("zww" + JsonConvert.SerializeObject(sd));
                    }
                }
                else
                {
                    bIconFinalised = true;
                    StavkiShow stShow = new StavkiShow();
                    stShow.onStShow += Step9;//переход на окончание айкона
                    int stav = steck.team[steck.o2 - 1].stavka;
                    stShow.inputStavki(stav, stav, 0, 0, this);
                }
            }
        }
        private void Step7()//получение ответа от 3 команды
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step7();
                });
            }
            else
            {
                otvetStatic?.semafor(0);
                otvetStatic?.answer(3, steck.team[steck.o3 - 1].answer, steck.correct);// вывод ответа третьей команды

                if (!steck.correct)//если ответ не верный
                {
                    bIconFinalised = true;
                    Graphics g = this.CreateGraphics();
                    g.DrawString("Все ответы неверны!", new Font("Cambria ", NewFontSize(25)), Brushes.Black, NewPoint(1405, 955));
                    g.DrawString("Все ответы неверны!", new Font("Cambria ", NewFontSize(25)), Brushes.White, NewPoint(1400, 950));
                    g.DrawString("Ваши ставки переходят казино!", new Font("Cambria ", NewFontSize(25)), Brushes.Black, NewPoint(1405, 1055));
                    g.DrawString("Ваши ставки переходят казино!", new Font("Cambria ", NewFontSize(25)), Brushes.White, NewPoint(1400, 1050));
                    Polosa pol = new Polosa();
                    pol.onPolosaEnd += Step9;
                    pol.polosa(50, NewPoint(1600, 1350), this, "Step7 - NoAnswer"); 

                }
                else
                {
                    bIconFinalised = true;
                    StavkiShow stShow = new StavkiShow();
                    stShow.onStShow += Step9;//переход на окончание айкона
                    int stav = steck.team[steck.o3 - 1].stavka;
                    stShow.inputStavki(stav, 0, 0, 0, this);
                }
            }
        }    
        private void Step9()//  окончание айкона
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step9();
                });
            }
            else
            {
                bIconFinalised = false;
                this.Controls["Iqon"].Dispose(); // Text = "";
                //if (otvetStatic != null)
                    otvetStatic?.close();
                //otvetStatic = null;
                //if (ruletka != null)
                    ruletka?.close();
                //ruletka = null;
                this.Invalidate();
                if (steck.iCon > 12)
                    Step10();
                else
                {
                    SendData sd = new SendData();
                    sd.kluch = kluch;
                    sd.table = (byte)tableOfKom;
                    sd.uid = predUs.GameZone;
                    sd.step = 1;
                    cn.SendUDP("zww" + JsonConvert.SerializeObject(sd));
                    //    Step1_3();
                }
            }
        }
        void Step10()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Step10();
                });
            }
            else
            {
                tbl.EndOfGame(steck);//описать финальную заставку 
            }
        }
       
        #endregion

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {            
                this.axWindowsMediaPlayer1.Dispose(); // закрываем сам плеер, чтобы все ресурсы освободились                                   
            }
        }

        #region // регион вспомогательных процедур и функций     
        public Point NewPoint(int x, int y)     //производит пересчет к новым координатам
        {
            //return new Point((x * resolution.Width / 2500) + delta, y * resolution.Height / 1600);
            return new Point((int)(x * resolution.Width / 2500) + (delta > 0 ? delta : 0), (int)(resolution.Height * y / 1600));
            //return new Point((int)(x * this.Width / 2500), (int)(this.Height * y / 1600));
        }
        public Point NewRelPoint(int x, int y)     //производит пересчет к новым координатам
        {
            //resolution = GetWorkingClientSize(myWorkForm);
            return new Point((int)(x * resolution.Width / 2500), (int)(resolution.Height * y / 1600));
        }
        public Size NewSize(int x, int y)  //производит пересчет к новым размерам
        {
            return new Size(x * resolution.Width / 2500, y * resolution.Height / 1600);
            //return new Size(x * this.Width / 2500, y * this.Height / 1600);
        }
        public Size NewSizeKv(int x)
        {
            return new Size(x * resolution.Height / 1600, x * resolution.Height / 1600);
            //return new Size(x * this.Height / 1600, x * this.Height / 1600);
        }
        public float NewFontSize(float x)     //производит пересчет к новым координатам
        {
            return x * resolution.Height / 1017;
            //return new Point((int)(x  this.Width / 2500), (int)(this.Height  y / 1600));
        }
        public void cleanTable(Point pn, Size sz)
        {
            //Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, resolution);
            //Bitmap bmpClear=new Bitmap(sz.Width,sz.Height);          
            //bmpClear = bmp.Clone(new RectangleF(pn,sz),System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //Graphics g = CreateGraphics();
            //g.DrawImage(bmpClear, pn);
            //g.Dispose();
        } 
        static string getSHAHash(string input)//хеш пароля
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
        void GetAllTypedControls(Control ctrl, List<Control> controls, Type type)//создаем список контролов по типу
        {
            // Работаем только с элементами искомого типа   
            if (ctrl.GetType() == type)
            {
                controls.Add(ctrl);
            }
            // Проходим через элементы рекурсивно,   
            // чтобы не пропустить элементы,   
            //которые находятся в контейнерах   
            foreach (Control ctrlChild in ctrl.Controls)
            {
                GetAllTypedControls(ctrlChild, controls, type);
            }
        }
        #endregion
        private static Size GetMaximizedClientSize(Form form)
        {
            var original = form.WindowState;
            try
            {
                BeginUpdate(form);
                form.WindowState = FormWindowState.Maximized;
                return form.ClientSize;
            }
            finally
            {
                form.WindowState = original;
                EndUpdate(form);
            }
        }

        private static Size GetWorkingClientSize(Form form)
        {
            var original = form.WindowState;
            try
            {
                BeginUpdate(form);

                form.WindowState = FormWindowState.Maximized;
                return new Size((int)(form.ClientSize.Height * 1.5625f), form.ClientSize.Height);
            }
            finally
            {
                form.WindowState = original;
                EndUpdate(form);
            }
        }
        [DllImport("User32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private enum Message : int
        {
            WM_SETREDRAW = 0x000B, // int 11
            SW_SHOWMAXIMIZED = 0x0003,
            SW_SHOW = 0x0005,
        }

        /// <summary>
        /// Calls user32.dll SendMessage(handle, WM_SETREDRAW, 0, null) native function to disable painting
        /// </summary>
        /// <param name="c"></param>
        public static void BeginUpdate(Control c)
        {
            SendMessage(c.Handle, (int)Message.WM_SETREDRAW, new IntPtr(0), IntPtr.Zero);
        }

        /// <summary>
        /// Calls user32.dll SendMessage(handle, WM_SETREDRAW, 1, null) native function to enable painting
        /// </summary>
        /// <param name="c"></param>
        public static void EndUpdate(Control c)
        {
            SendMessage(c.Handle, (int)Message.WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из игры?", "Предупреждение!!!", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) e.Cancel = true;
            else
                ruletka?.close();
        }

        private void GeneralForm_Load(object sender, EventArgs e)
        {
            //Location = new Point(0, 0);
            //this.WindowState = FormWindowState.Maximized;
            //this.TopMost = true;
            //this.Size = new Size(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height);
            //this.MaximizeBox = false;
            //this.MaximumSize = new Size(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height);
        }

        private void GeneralForm_Deactivate(object sender, EventArgs e)
        {
            //this.Size = new Size(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width, System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height);
            //this.Activate();
            //this.Show();
            //this.Focus();
            //this.TopMost = true;
        }

        private void GeneralForm_Shown(object sender, EventArgs e)
        {
            this.Text = "Интеллект-казино 700 IQ";
            
            ShowWindow(this.Handle, (int)Message.SW_SHOWMAXIMIZED);
            //SetForegroundWindow(this.Handle);
            IniScreen();
        }

        private void GeneralForm_Activated(object sender, EventArgs e)
        {
            
            this.Invalidate();
            this.Refresh();
        }
    }
}
