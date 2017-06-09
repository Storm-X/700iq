﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _700IQ
{

    class Table : resize //отрисовка команд за столом
    {
        #region переменные

        //Image[] fish = new Image[] { Properties.Resources.kom1, Properties.Resources.kom2, Properties.Resources.kom3 };
        Data predUs;
        Bitmap bmpStol;
        Bitmap[] fish = new Bitmap[3];
      
        //private Form fsv;
        #endregion

        public Table(Data predus, int tableofkom, GeneralForm fsv)
        {
            workForm = fsv;
            bmpStol = new Bitmap(Properties.Resources.GreenTable, workResolution);
            Graphics g = Graphics.FromImage(bmpStol);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.predUs = predus;
            //this.fsv = fsv;
           
            fish[0] = new Bitmap(Properties.Resources.kom1, NewSizeKv(170));
            fish[1] = new Bitmap(Properties.Resources.kom2, NewSizeKv(170));
            fish[2] = new Bitmap(Properties.Resources.kom3, NewSizeKv(170));

            int mesto = tableofkom;

            g.DrawImage(fish[mesto], NewPoint(200, 1200));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.Black, NewPoint(405, 1155));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.White, NewPoint(400, 1150));

            mesto++;
            if (mesto > 2) mesto = 0;
            //лево
            g.DrawImage(fish[mesto], NewPoint(200, 150));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.Black, NewPoint(410, 105));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.White, NewPoint(410, 100));

            mesto++;
            if (mesto > 2) mesto = 0;
            //право
            g.DrawImage(fish[mesto], NewPoint(1900, 150));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.Black, NewPoint(2105, 105));
            g.DrawString(predUs.team[mesto].name, new Font("Calibri", NewFontSize(40)), Brushes.White, NewPoint(2100, 100));
        }
        
         public Table(GeneralForm fsv)//для тестирования
         {
             workForm = fsv;
            bmpStol = new Bitmap(Properties.Resources.GreenTable, workResolution);
            Graphics g = Graphics.FromImage(bmpStol);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Game steck = new Game();
            steck.team[0] = new Game.Teames();
            steck.team[0].iQash = 900;
            steck.team[1] = new Game.Teames();
            steck.team[1].iQash = 1000;
            steck.team[2] = new Game.Teames();
            steck.team[2].iQash = 1100;
             SetIQ(steck, 1);
         }
         

        ~Table()
        {

        }
        public Bitmap SetIQ(Game steck, int tableofkom) //рассадка тройки за столом
        {
            Bitmap bmp =(Bitmap) bmpStol.Clone();
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int mesto = tableofkom;
            CustomLabel iQash1 = new CustomLabel()
            {
                Name = "iQash",
                Location = NewPoint(400, 1330),
                Text = steck.team[mesto].iQash + " IQ",
                BackColor = Color.Transparent,
                Font = new Font("Calibri", NewFontSize(20), FontStyle.Bold),
                SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality,
                InterpolationMode = InterpolationMode.HighQualityBicubic,
                ForeColor = Color.Gold,
                Parent = this.workForm,
                ShadowColor = Color.Black,
                ShadowOffset = new Point(3, 3),
                number = mesto,
            };

            mesto++;
            if (mesto > 2) mesto = 0;
            //лево
            CustomLabel iQash2 = new CustomLabel()
            {
                Name = "iQash",
                Location = NewPoint(400, 280),
                Text = steck.team[mesto].iQash + " IQ",
                BackColor = Color.Transparent,
                Font = new Font("Calibri", NewFontSize(20), FontStyle.Bold),
                SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality,
                InterpolationMode = InterpolationMode.HighQualityBicubic,
                ForeColor = Color.Gold,
                Parent = this.workForm,
                ShadowColor = Color.Black,
                ShadowOffset = new Point(3, 3),
                number = mesto,
            };

            mesto++;
            if (mesto > 2) mesto = 0;
            //право
            CustomLabel iQash3 = new CustomLabel()
            {
                Name = "iQash",
                Location = NewPoint(2100, 280),
                Text = steck.team[mesto].iQash + " IQ",
                BackColor = Color.Transparent,
                Font = new Font("Calibri", NewFontSize(20), FontStyle.Bold),
                SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality,
                InterpolationMode = InterpolationMode.HighQualityBicubic,
                ForeColor = Color.Gold,
                Parent = this.workForm,
                ShadowColor = Color.Black,
                ShadowOffset = new Point(3, 3),
                number = mesto,
            };
            this.workForm.iQash1 = iQash1;
            this.workForm.iQash2 = iQash2;
            this.workForm.iQash3 = iQash3;

            mesto = tableofkom;
            g.Dispose();
            return bmp;
        }
        public void SetInfoTopStroka() //(Form ff) //установка верхней инфострочки
        {
            //ff.Invoke(new MethodInvoker(() =>
            //{
            if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    SetInfoTopStroka();
                });
            }
            else
            {
                Label serv = new Label()
                {
                    //Location = NewPoint(2200, 1550),
                    Text = GeneralForm.infoOfserver,
                    AutoSize = true,
                    Font = new Font("Cambria ", NewFontSize(12)),
                    ForeColor = Color.Gold,
                    Parent = workForm,
                    BackColor = Color.Transparent,
                    //TextAlign = ContentAlignment.MiddleRight,
                };
                //serv.Location = new Point(workForm.Width - serv.Width - 3, workForm.Height - 3 - serv.Height);
                serv.Location = new Point(3, workForm.Height - 3 - serv.Height);

                PictureBoxWithInterpolationMode IQ700 = new PictureBoxWithInterpolationMode()
                {
                    Size = NewSizeKv(100),
                    Location = new Point(workForm.Width - NewSizeKv(100).Width - 3 , 3),//NewPoi,nt(2410, 0),
                    Image = Properties.Resources.rotor700,

                    SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias,
                    InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic,
                    BackColor = Color.Transparent,
                    Parent = workForm,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                Label intel = new Label()
                {
                    AutoSize = true,
                    Text = "Интеллект-казино",
                    //Location = NewPoint(2150, 30),
                    Font = new Font("Cambria ", NewFontSize(12)),
                    ForeColor = Color.Gold,
                    Parent = workForm,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight,
                };
                //intel.BackgroundImage = bmp;
                intel.Location = new Point(workForm.Width - NewSizeKv(100).Width - intel.Width, 3 + (NewSizeKv(100).Height - intel.Height)/2);

                Label inform = new Label()
                {
                    Location = new Point(3,3), //NewPoint(5, 30),
                    Text = predUs.city + " - " + predUs.NumberGame + " игра - " + predUs.Tur,
                    AutoSize = true,
                    Font = new Font("Cambria ", NewFontSize(12)),
                    ForeColor = Color.Gold,
                    Parent = workForm,
                    BackColor = Color.Transparent,
                };
            }
            //}));
        }     
        public void Rassadka(Game steck) //,  Form ff)//  заставка с инфо о рассадке команд
        {
            Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, workResolution);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.DrawString(predUs.GameZone + " игровая зона", new Font("Times New Roman", NewFontSize(50)), Brushes.Black, NewPoint(855, 55));
            g.DrawString(predUs.GameZone + " игровая зона", new Font("Times New Roman", NewFontSize(50)), Brushes.Yellow, NewPoint(850, 50));
         
            for (int y = 0; y < 3; y++)
            {
                g.DrawImage(fish[y], NewPoint(100, 300 + 380 * y));
                g.DrawString("Команда  " + predUs.team[y].name, new Font("Cambria ", NewFontSize(25)), Brushes.Black, NewPoint(255, 300 + y * 380));
                g.DrawString("Команда  " + predUs.team[y].name, new Font("Cambria ", NewFontSize(25)), Brushes.Green, NewPoint(250, 295 + y * 380));
                g.DrawString("Рейтинг - " + predUs.team[y].rating + "     " + steck.team[y].iQash + " Iq", new Font("Cambria ", NewFontSize(15)), Brushes.YellowGreen, NewPoint(270, 390 + y * 380));
                g.DrawLine(Pens.Red, NewPoint(50, 230 + y * 380), NewPoint(2000, 230 + y * 380));
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        g.DrawString((i + 1) + ". " + predUs.team[y].member[i].F + " " + predUs.team[y].member[i].N,
                            new Font("Cambria ", NewFontSize(18)), Brushes.White, NewPoint(1200, i * 60 + 250 + y * 380));
                        g.DrawString(predUs.team[y].member[i].rait.ToString(),
                            new Font("Cambria ", NewFontSize(18)), Brushes.White, NewPoint(1900, i * 60 + 250 + y * 380));
                    }
                    catch { }
                }
            }
            g.Dispose();
            workForm.BackgroundImage = bmp;        
        }
        public void SetInfoTemy() //Form ff)//заставка с темами на игру
        {
            Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, workResolution);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.DrawString("Темы вопросов на игру", new Font("Times New Roman", NewFontSize(48)), Brushes.Black, NewPoint(500 + 4, 55));
            g.DrawString("Темы вопросов на игру", new Font("Times New Roman", NewFontSize(48)), Brushes.Yellow, NewPoint(500, 50));

            Rectangle rec = new Rectangle
            {
                Location = NewPoint(400, 450),
                Size = NewSizeKv(80),
            };
            Rectangle rect = new Rectangle
            {
                Location = NewPoint(403, 453),
                Size = NewSizeKv(80),
            };
            SolidBrush sBrash = new SolidBrush(Color.Orange);
            string[] tmcolor = { "Black", "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
            for (int i = 1; i < 7; i++)
            {
                sBrash.Color = Color.FromName(tmcolor[i]);
                rec.Location = NewPoint(400, 400 + i * 100);
                rect.Location = new Point(NewPoint(400, 400 + i * 100).X + 3, NewPoint(400, 400 + i * 100).Y + 3);//NewPoint(403, 403 + i * 100);
                g.FillRectangle(Brushes.Black, rect);
                g.FillRectangle(sBrash, rec);
                g.DrawString(predUs.tema[i].theme, new Font("Buxton Sketch", NewFontSize(30)), Brushes.Black, new Point(NewPoint(500, 390 + i * 100).X + 3, NewPoint(500, 390 + i * 100).Y + 3));//NewPoint(503, 393 + i * 100)
                g.DrawString(predUs.tema[i].theme, new Font("Buxton Sketch", NewFontSize(30)), sBrash, NewPoint(500, 390 + i * 100));
            }
            workForm.BackgroundImage = bmp;
            g.Dispose();
            sBrash.Dispose();
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
        private void stopRul()
        {
            throw new NotImplementedException();
        }

        public void EndOfGame(Game steck) //, Form ff)//  заставка с результатами игры
        {
            string[] txtPos = { "Победитель", "2 место", "3 место" };
            Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, workResolution);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            string lbText = "";

            //var teamPos = steck.team.OrderByDescending(c => c.iQash);                         //Список команд, отсортированный по результату
            int[] o = ResponsePriority(steck.Cell, steck.team.Select(x => x.iQash << 2).ToArray());
            var cntEqualTeamsGrp = steck.team.GroupBy(x => x.iQash);                            //Группировка по результату
            int cntEqualTeams = steck.team.Select(x => x.iQash).Distinct().Count();             //Подсчет количества различных результатов у команд
            workForm.BackgroundImage = bmp;
            g.DrawString(predUs.GameZone + " игровая зона", new Font("Times New Roman", NewFontSize(50)), Brushes.Black, NewPoint(855, 55));
            g.DrawString(predUs.GameZone + " игровая зона", new Font("Times New Roman", NewFontSize(50)), Brushes.Yellow, NewPoint(850, 50));
            g.DrawString("игра  завершена!", new Font("Times New Roman", NewFontSize(30)), Brushes.Black, NewPoint(985, 165));
            g.DrawString("игра  завершена!", new Font("Times New Roman", NewFontSize(30)), Brushes.Yellow, NewPoint(980, 160));

            if (cntEqualTeams != 3)
            {
                var teamWins = steck.team.Where(c => c.iQash == steck.team.Max(n => n.iQash));      //Команды с максимальным результатом
                if (teamWins.Count() > 1)                                                            //И количество этих команд
                {
                    lbText = teamWins.Count() + " команды закончили игру с максимальным результатом. " + Environment.NewLine 
                                              + "Победителя определит рулетка!";
                }
                else
                {
                    lbText = "Две команды закончили игру с одинаковыми результатами! " + Environment.NewLine 
                           + "2 и 3 место определит рулетка!";
                }
                Label zagolovok = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("times new roman", NewFontSize(20), FontStyle.Italic),
                    Text = lbText,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = NewPoint(750, 350),
                    Size = NewSize(900, 200),
                };

                //Rul Ruletka = new Rul();
                workForm.Ruletka.AnyEventHarakiri();
                workForm.Ruletka.StartRul(steck.Cell, new Rectangle(NewPoint(1700, 210), NewSizeKv(900)), workForm, 3);
                while (workForm.Ruletka.Enabled && !this.workForm.IsDisposed)
                    Application.DoEvents();
                zagolovok.Dispose();
                workForm.Invalidate();
            }
            int iPositions = 0;
            foreach (var table in o)
            {
                g.DrawString(string.Format("{0} (стол {1})", predUs.team[table - 1].name, predUs.team[table - 1].table), new Font("Cambria ", NewFontSize(25)), Brushes.Black, NewPoint(305, 350 + 380 * iPositions));
                g.DrawString(string.Format("{0} (стол {1})", predUs.team[table - 1].name, predUs.team[table - 1].table), new Font("Cambria ", NewFontSize(25)), Brushes.White, NewPoint(300, 345 + 380 * iPositions));
                g.DrawString("с результатом  -  " + steck.team[table - 1].iQash + " Iq", new Font("Cambria ", NewFontSize(20)), Brushes.YellowGreen, NewPoint(1250, 350 + 380 * iPositions));
                g.DrawLine(Pens.Red, NewPoint(50, 280 + 380 * iPositions), NewPoint(2000, 280 + 380 * iPositions));

                var place = (Image)Properties.Resources.ResourceManager.GetObject(String.Format("cup_{0}", iPositions + 1));
                PictureBox winnerCup = new PictureBox()
                {
                    Location = NewPoint(50, 330 + 380 * iPositions),
                    Size = NewSizeKv(280),
                    Image = place,
                    BackColor = Color.Transparent,
                    Parent = workForm,
                    SizeMode = PictureBoxSizeMode.Zoom,
                };
                iPositions++;
            }
            g.Dispose();
            //workForm.BackgroundImage = bmp;
            workForm.Invalidate();
        }

        public void TemaShow(Game steck, bool tema) //Form ff, 
        {      
            string[] tmcolor = { "Black", "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
            workForm.BeginInvoke(new MethodInvoker(() =>
            {
                Label rec = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    Location = NewPoint(825, 1060),
                    Size = NewSize(850, 100),
                    BackColor = Color.White,
                    Image = Properties.Resources.paper,
                    Text = tema ? "Выбор темы вопроса" : predUs.tema[steck.theme].theme,
                    Font = new Font("Buxton Sketch", NewFontSize(25)),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                Label rect = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    Location = new Point(NewPoint(825, 1060).X - 3, NewPoint(825, 1060).Y - 3),//NewPoint(847, 1057),
                    Size = new Size(NewSize(850, 100).Width + 6, NewSize(850, 100).Height + 6),//NewSize(806, 106),
                    BackColor = tema ? Color.Gray : Color.FromName(tmcolor[steck.theme]),
                };
            }));
        }
/*
        public void TemaShow(bool tema) //Form ff, //для тестирования
        {
            string[] tmcolor = { "Black", "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
            workForm.BeginInvoke(new MethodInvoker(() =>
            {
                Label rec = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    Location = NewPoint(825, 1060),
                    Size = NewSize(850, 100),
                    BackColor = Color.White,
                    Image = Properties.Resources.paper,
                    Text = tema ? "Выбор темы вопроса" : "какая-то тема",
                    Font = new Font("Buxton Sketch", NewFontSize(25)),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                Label rect = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    Location = new Point(NewPoint(825, 1060).X - 3, NewPoint(825, 1060).Y - 3),//NewPoint(847, 1057),
                    Size = new Size(NewSize(850, 100).Width + 6, NewSize(850, 100).Height + 6),//NewSize(806, 106),
                    BackColor = Color.Red,
                };
            }));
        }
*/
    }
    public class Padge : resize //заставка с зарегистрировавшимися командами
    {
        public CustomScrollbar cs;
        public ListViewWithoutScrollBar listView1;
        int visible_count = 0;
        public void spisokOut(GeneralForm fm, DataTable dt, Data predUs)//вывод спсок зарегистрированных команд
        {
            //    if (iniEnd) return;
            //int ctrCount = fm.Controls.Count;
            //for (int i = 0; i <= ctrCount; i++) fm.Controls.RemoveByKey("oneuse");//очистка экрана от временных элементов
            workForm = fm;
            //= "Зарегистрировавшиеся команды:\n\n";          
            DataRow[] dtRow = dt.Select();
            #region//описание полей вывода информации список команд
            workForm.BeginInvoke(new MethodInvoker(() =>
            {
                //resolution = Screen.FromControl(fm).WorkingArea.Size;
                foreach (Control t in workForm.Controls.Find("oneuse", true))
                    workForm.Controls.Remove(t);

                Point listView1_Location = NewPoint(800, 400);
                Size listView1_Size = NewSize(1700, 1160);
                Bitmap listView1_BackgroundImage = new Bitmap(workForm.BackgroundImage).Clone(new Rectangle(listView1_Location.X, listView1_Location.Y, listView1_Size.Width, listView1_Size.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Label zagolovok = new Label
                {
                    Parent = workForm,
                    Name = "oneuse",
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("times new roman", NewFontSize(20), FontStyle.Italic),
                    Text = "Зарегистрировавшиеся команды:",
                    Location = NewPoint(800, 350),
                    Size = NewSize(700, 50),
                };
                listView1 = new ListViewWithoutScrollBar()
                {
                    Name = "oneuse",
                    Location = listView1_Location,
                    Size = listView1_Size,
                    BackgroundImage = new Bitmap(workForm.BackgroundImage).Clone(new Rectangle(listView1_Location.X, listView1_Location.Y, listView1_Size.Width, listView1_Size.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb),
                    Font = new Font("times new roman", NewFontSize(25), FontStyle.Italic),
                    ForeColor = Color.Gold,
                    BorderStyle = BorderStyle.None,
                    BackgroundImageTiled = true,
                    View = View.Details,
                    LabelEdit = false,
                    AllowColumnReorder = true,
                    HeaderStyle = ColumnHeaderStyle.None,
                    Parent = workForm,
               };
                listView1.Items.Add(new ListViewItem(new string[] { "№", "Название", "Рейтинг" }));
                int count = 1;
                for (int j = 0; j < 20; j++)
                {
                    for (int i = 0; i < dtRow.Length; i++)
                    {
                        listView1.Items.Add(new ListViewItem(new string[] { (count++).ToString(), dtRow[i][2].ToString(), dtRow[i][4].ToString() }));
                    };
                }
 
                listView1.Columns.Add("", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("", -2, HorizontalAlignment.Left);
                listView1.Columns.Add("", -2, HorizontalAlignment.Left);
                listView1.Columns[0].Width = new Size((int)(listView1.Width * 0.05), 500).Width;
                listView1.Columns[1].Width = new Size((int)(listView1.Width * 0.80) - 20, 500).Width;
                listView1.Columns[2].Width = new Size((int)(listView1.Width * 0.15), 500).Width;
                visible_count = listView1.Height / (listView1.Font.Height+4);
                cs = new CustomScrollbar()
                {
                    Parent = listView1,
                    ChannelColor = Color.Transparent,
                    LargeChange = 0,
                    Location = new Point((int)(listView1.Width) - 20,0),
                    Maximum = listView1.Items.Count - visible_count - 1,
                    Minimum = 0,
                    MinimumSize = NewSize(15, 92),
                    Name = "cs",
                    Size = new Size(18, listView1.Height),
                    SmallChange = 0,
                    BackColor = Color.Transparent,
                    BorderStyle = BorderStyle.None,
                    TabIndex = 1,
                    Value = 0
                };
                cs.Scroll += Cs_Scroll;
                if (visible_count >= listView1.Items.Count)
                {
                    cs.Visible = false;
                }
                else
                {
                    cs.Visible = true;
                }
                #endregion
                #region начало игры
                Label lbr = new Label()//--------------------игра начнется
                {
                    Parent = workForm,
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
                    Parent = workForm,
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
            }));
            #endregion
        }

        private void Cs_Scroll(object sender, EventArgs e)
        {
                listView1.TopItem = listView1.Items[cs.Value];
        }
    }
    public class Otvet : resize //ответ на вопрос и показ очереди ответа
    {
        #region//переменные  
        PictureBox pc1;
        PictureBox pc2;
        PictureBox pc3;
        PictureBox picBox1, bgrdPic;
        Label lb1, lb, vpramka;
        Label lb2;
        Label lb3;
        Label lbst1;
        Label lbst2;
        Label lbst3;
        TextBox txBox;
        Label otv;
        private int frameCount;
        //Bitmap[] arr;
        //Bitmap img;
        private FrameDimension dimension;
        GifImage gifImage;
        PictureBox pc1rez;
        PictureBox pc2rez;
        PictureBox pc3rez;
        PictureBox pc4rez;
        PictureBox pc5rez;
        PictureBox pc6rez;
        PictureBox[] pcResult;
        PictureBox temp =new PictureBox();
        private System.Windows.Forms.Timer gifTimer1 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer gifTimer2 = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer gifTimer3 = new System.Windows.Forms.Timer();

        private int indexToPaint = 0;
        //private Form fsv;
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tmSem = new System.Windows.Forms.Timer();
        int tick = 0;
        int semaforN;
        Conn cn;
        Data predUs;
        int tableofkom;
        int step = 0;
        private bool IsDisposed = false;

        struct SendData //структурированные данные отправляемые серверу
        {
            public int uid;
            public string kluch;    //ключ игровой сессии
            public byte table;
            public byte step;       //шаг игры
            public string otvet;
            public int stavka;
        }
        // реализация интерфейса IDisposable.
        public void Dispose()
        {
            Dispose(true);
            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    //this.DisposeSequence(pcResult);
                    // Освобождаем управляемые ресурсы
                    foreach (object o in this.pcResult.OfType<IDisposable>())
                    {
                        if (o is IDisposable)
                            ((IDisposable)o).Dispose();
                    }
                    Array.Clear(pcResult, 0, pcResult.Length);

                    pc1.Dispose(); pc2.Dispose(); pc3.Dispose();
                    lb1.Dispose(); lb2.Dispose(); lb3.Dispose();
                    //pc1rez.Dispose(); pc2rez.Dispose(); pc3rez.Dispose();
                    lbst1.Dispose(); lbst2.Dispose(); lbst3.Dispose();
                    tmSem.Dispose();
                    bgrdPic.Dispose();
                }
                // освобождаем неуправляемые объекты
                IsDisposed = true;
            }
            // Обращение к методу Dispose базового класса
            base.Dispose(disposing);
        }

        //Деструктор класса
        ~Otvet()
        {
            Dispose(false);
        }
        #endregion
        public Otvet(Conn cn, Data predus, int tableofkom, GeneralForm fsv)
        {
            workForm = fsv;
            predUs = predus;
            this.cn = cn;
            this.tableofkom = tableofkom;
            //this.fsv = fsv;
            gifTimer1.Tick += gifTimer_Tick1;
            gifTimer2.Tick += gifTimer_Tick2;
            gifTimer3.Tick += gifTimer_Tick3;
        }
        private async Task<Image> ResultOfCycle(string fileName)
        {
            MediaReceiver mReceiver = new MediaReceiver(workForm.IP, 8080);
            byte[] filecontent = mReceiver.GetMedia(fileName);
            if (filecontent.Length == 0)
                return null;
            else
            {
                MemoryStream ms = new MemoryStream(filecontent);
                mReceiver = null; //Сделать Dispose() ОБЯЗАТЕЛЬНО!!!!
                return Image.FromStream(ms, true);
            }
        }
        public async Task svitok(Game steckIn, Data predUs)
        {
                String fileName = "";
                int picWidth = 0;
                #region //описание свитка с вопросом               
                Bitmap bmp = new Bitmap(Properties.Resources.Svitok, NewSize(900, 1190));
                bgrdPic = new PictureBox()
                {
                    Parent = workForm,
                    Size = NewSize(900, 1210),
                    Location = NewPoint(100, 150),
                    Image = bmp,
                    BackColor = Color.Transparent
                };

                picBox1 = new PictureBox()
                {
                    Parent = bgrdPic,
                    Size = NewSize(850, picWidth),
                    Location = NewRelPoint(25, 200), //new Point(25, NewPoint(25, 200).Y),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = (fileName == "") ? null : await ResultOfCycle(fileName)
                };
                lb = new Label()
                {
                    Parent = bgrdPic,
                    Size = NewSize(850, 850 - picWidth),
                    Location = NewRelPoint(25, 200 + picWidth), //new Point(25, NewPoint(25, 200 + picWidth).Y),
                    //Image = bmp,
                    BackColor = Color.Transparent,
                    Text = steckIn.quest,
                    Font = new Font("Arial Black Italic", NewFontSize(18), FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Padding = new Padding(20, 0, 20, 20),
                    //BorderStyle = BorderStyle.FixedSingle
                };
                //Parent = bgrdPic //////// Parent = lb
                vpramka = new Label()
                {
                    Parent = bgrdPic,
                    Size = NewSize(850, 130),
                    Location = NewRelPoint(25, 70), //NewPoint(25, 70),
                    BackColor = Color.Transparent,
                    Font = new Font("Cambria", NewFontSize(25), FontStyle.Bold),
                    TextAlign = ContentAlignment.TopCenter,
                    Text = predUs.tema[steckIn.theme].theme,
                    //BorderStyle = BorderStyle.FixedSingle
                };

                #endregion
                otv = new Label()
                {
                    Parent = bgrdPic,
                    Size = NewSize(900, 300),
                    Location = NewRelPoint(50, 1020), //NewPoint(50, 1020),
                    BackColor = Color.Transparent,
                    Text = "Ответ",
                    Font = new Font("Arial Black Italic", NewFontSize(20), FontStyle.Bold),
                    TextAlign = ContentAlignment.TopLeft,
                    Padding = new Padding(0, 20, 20, 20),
                };
                txBox = new TextBox()
                {
                    Parent = otv,
                    Size = NewSize(640, 190),
                    Multiline = false,
                    MaxLength = 33,
                    BackColor = Color.LightGreen,
                    Location = NewRelPoint(140, 30), //NewPoint(165, 30),
                    Font = new Font("Arial Black Italic", NewFontSize(18), FontStyle.Bold),
                    Cursor = workForm.SetCursor(workForm.path + "Text Select.ani"),//установка курсора из файла
                };
                //await ochered(steckIn);
                bgrdPic.BringToFront();
        }
        public void SetFocus()
        {
            txBox.Focus();
        }
        public async Task ochered(Game steckIn) //, Form fsv)//расчет очереди-----------------------------------
        {
           /* if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    ochered(steckIn);
                });
            }
            else
            {*/
                Image[] im = new Image[3];
                im[0] = Properties.Resources.kom1;
                im[1] = Properties.Resources.kom2;
                im[2] = Properties.Resources.kom3;
                int dy = -50;

                #region//описание фишек с номерами команд
                pc1 = new PictureBoxWithInterpolationMode()
                {
                    Parent = workForm,
                    Visible = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewPoint(1250, 250 + dy),
                    BackgroundImage = im[steckIn.o1 - 1],
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.AntiAlias,
                    InterpolationMode = InterpolationMode.NearestNeighbor
                };
                pc2 = new PictureBoxWithInterpolationMode()
                {
                    Parent = workForm,
                    Visible = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewPoint(1250, 450 + dy),
                    BackgroundImage = im[steckIn.o2 - 1],
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.AntiAlias,
                    InterpolationMode = InterpolationMode.NearestNeighbor
                };
                pc3 = new PictureBoxWithInterpolationMode()
                {
                    Parent = workForm,
                    Visible = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewPoint(1250, 650 + dy),
                    BackgroundImage = im[steckIn.o3 - 1],
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.AntiAlias,
                    InterpolationMode = InterpolationMode.NearestNeighbor
                };
            #endregion
            #region//описание полей выигрыш проигрыш
            pcResult = new PictureBoxWithInterpolationMode[] { 
                //pc1rez = 
                new PictureBoxWithInterpolationMode()
                {
                    Parent = pc1,
                    //Name = "oneuse",
                    Visible = true,
                    Enabled = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewRelPoint(0, 0),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.HighQuality,
                    InterpolationMode = InterpolationMode.HighQualityBicubic
                }, //;
                //pc2rez = 
                new PictureBoxWithInterpolationMode()
                {
                    Parent = pc2,
                    //Name = "oneuse",
                    Visible = true,
                    Enabled = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewRelPoint(0, 0),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.HighQuality,
                    InterpolationMode = InterpolationMode.HighQualityBicubic
                }, //;
                   //pc3rez = 
                new PictureBoxWithInterpolationMode()
                {
                    Parent = pc3,
                    //Name = "oneuse",
                    Visible = true,
                    Enabled = false,
                    BackColor = Color.Transparent,
                    Size = NewSizeKv(170),
                    Location = NewRelPoint(0, 0),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    SmoothingMode = SmoothingMode.HighQuality,
                    InterpolationMode = InterpolationMode.HighQualityBicubic
                }
                };

                #endregion
                #region//описание полей со ставками команд
                lbst1 = new Label()
                {
                    Parent = workForm,
                    Visible = true,
                    BackColor = Color.Transparent,
                    Size = NewSize(95, 120),
                    Location = NewPoint(1400, 275 + dy),
                    Font = new Font("times new roman", NewFontSize(17)),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Yellow,
                    Text = steckIn.team[steckIn.o1 - 1].stavka + "\n IQ",
                };
                lbst2 = new Label()
                {
                    Parent = workForm,
                    Visible = true,
                    BackColor = Color.Transparent,
                    Size = NewSize(95, 120),
                    Location = NewPoint(1400, 470 + dy),
                    Font = new Font("times new roman", NewFontSize(16)),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Yellow,
                    Text = steckIn.team[steckIn.o2 - 1].stavka.ToString() + "\n IQ",
                };
                lbst3 = new Label()
                {
                    Parent = workForm,
                    Visible = true,
                    BackColor = Color.Transparent,
                    Size = NewSize(95, 120),
                    Location = NewPoint(1400, 680 + dy),
                    Font = new Font("Times New Roman", NewFontSize(16)),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Yellow,
                    Text = steckIn.team[steckIn.o3 - 1].stavka.ToString() + "\n IQ",
                };
                #endregion
                #region//описание полей с ответами команд
                lb1 = new Label()
                {
                    Parent = workForm,
                    Visible = false,
                    //BackColor = Color.Transparent,
                    Image = Properties.Resources.paper,
                    Size = NewSize(600, 70),
                    Location = NewPoint(1560, 310 + dy),
                    Font = new Font("Arial Black Italic", NewFontSize(18), FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,

                };
                lb2 = new Label()
                {
                    Parent = workForm,
                    Visible = false,
                    //BackColor = Color.Green,
                    Image = Properties.Resources.paper,
                    Size = NewSize(600, 70),
                    Location = NewPoint(1560, 510 + dy),
                    Font = new Font("Arial Black Italic", NewFontSize(19), FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    //ForeColor = Color.White,
                };
                lb3 = new Label()
                {
                    Parent = workForm,
                    Visible = false,
                    Image = Properties.Resources.paper,
                    Size = NewSize(600, 70),
                    Location = NewPoint(1560, 710 + dy),
                    Font = new Font("Arial Black Italic", NewFontSize(20), FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                #endregion

                tm.Interval = 60;
                tm.Tick += Tm_Tick;
                tm.Start();
                tmSem.Interval = 300;
                tmSem.Tick += TmSem_Tick;
            Thread.Sleep(1000);
            //}
        }
        private void Tm_Tick(object sender, EventArgs e)
        {
            tick += 1;
            if (tick == 1) pc1.Visible = true;
            if (tick == 6) pc2.Visible = true;
            if (tick == 12) { pc3.Visible = true; tm.Dispose(); }
        }
        #region включение полей ответов
        public void Ot1Show() { pc1.Visible = true; pc1.BringToFront(); }
        public void Ot2Show() { pc2.Visible = true; pc2.BringToFront(); }
        public void Ot3Show() { pc3.Visible = true; pc3.BringToFront(); }
        #endregion    
        #region методы для мигания поля
        public void semafor(int number)//запуск и остановка таймера для мигания
        {
            //var reportProgress = new Action(() =>
            //{
            /*if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    semafor(number);
                });
            }
            else
            {*/
            //if (tmSem.Enabled || number == 0)
            //{
                tmSem.Stop();
                //pc1.Visible = true;
                //lbst1.Visible = true;
                //pc2.Visible = true;
                //lbst2.Visible = true;
                //pc3.Visible = true;
                //lbst3.Visible = true;
            //}
            //else
                tmSem.Start();
                semaforN = number;
            /*}*/
        }
        void semStart()//мигание отвечающей команды
        {
            switch (semaforN)
            {
                case 1:
                    pc1.Visible = !pc1.Visible;
                    pc2.Visible = true;
                    pc3.Visible = true;
                    break;
                case 2:
                    pc2.Visible = !pc2.Visible;
                    pc1.Visible = true;
                    pc3.Visible = true;
                    break;
                case 3:
                    pc3.Visible = !pc3.Visible;
                    pc2.Visible = true;
                    pc1.Visible = true;
                    break;
            }
        }
        private void TmSem_Tick(object sender, EventArgs e)
        {
            semStart();
        }
        public static Bitmap MakeImage(Size ImgSize, Bitmap foreImg, Bitmap backImg, byte s)
        {
            // ImgSize = размер картинки-результата, обе исходные картинки приводятся к указанному размеру
            // s прозрачность накладываемого изображения foreImg от 0 (100%) до 255 (0%)
            // результат наследует Альфа-канал фонового изображения
            // наложение использует Альфа-канал накладываемого изображения
            Bitmap fimg = new Bitmap(foreImg, ImgSize);
            Bitmap bimg = new Bitmap(backImg, ImgSize);
            Bitmap bmp = new Bitmap(ImgSize.Width, ImgSize.Height);
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color fm = fimg.GetPixel(i, j);
                    Color bm = bimg.GetPixel(i, j);
                    byte af = (byte)(fm.A * s / byte.MaxValue);
                    byte a = fm.A;                     //используем альфа-канал накладываемого изображения
                    byte r = (byte)((fm.R * af + bm.R * (byte.MaxValue - af)) / byte.MaxValue);
                    byte g = (byte)((fm.G * af + bm.G * (byte.MaxValue - af)) / byte.MaxValue);
                    byte b = (byte)((fm.B * af + bm.B * (byte.MaxValue - af)) / byte.MaxValue);
                    bmp.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                }
            return bmp;
        }
        
        #endregion
        public void getArrayOfFrames(Image im)
        {
            gifImage = new GifImage(im);
            gifImage.ReverseAtEnd = false; //dont reverse at end
            //dimension = new FrameDimension(im.FrameDimensionsList[0]);
            //frameCount = im.GetFrameCount(dimension);
            //arr = new Bitmap[frameCount];
            //for (int i = 0; i < frameCount; i++)
            //{
            //    im.SelectActiveFrame(dimension, i);
            //    arr[i] = new Bitmap(im);
            //}
        }

        public void answer(int o, string otv, bool correct) //неправильный ответ
        {
           /* if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    answer(o, otv, correct);
                });
            }
            else
            {*/
                Bitmap im2 = Properties.Resources.галочка;
                Bitmap im3 = Properties.Resources.крестик;
                gifTimer1.Interval = 25;
                gifTimer2.Interval = 25;
                gifTimer3.Interval = 25;

                semafor(0);
                //if (correct)
                //{
                    getArrayOfFrames(correct ? im2 : im3);
                //}
                //else
                //{
                //    getArrayOfFrames(im3);
                //}
                    
                if (o == 1)
                {
                    lb1.Text = otv;
                    lb1.Visible = true;
                    lb1.BringToFront();
                    gifTimer1.Start();
                    pcResult[0].Visible = true;
                    //pc1rez.Visible = true;
                }
                if (o == 2)
                {
                    lb2.Text = otv;
                    lb2.Visible = true;
                    lb2.BringToFront();
                    gifTimer2.Start();
                    pcResult[1].Visible = true;
                    //pc2rez.Visible = true;     
                }
                if (o == 3)
                {
                    lb3.Text = otv;
                    lb3.Visible = true;
                    lb3.BringToFront();
                    gifTimer3.Start();
                    pcResult[2].Visible = true;
                    //pc3rez.Visible = true;
                }
            //}
        }
        void gifTimer_Tick1(object sender, EventArgs e)
        {
            indexToPaint++;
            if (indexToPaint > gifImage.FrameCount)
            {
                indexToPaint = 0;
                gifTimer1.Stop();
                //ImageAnimator.StopAnimate(this.pc1rez.Image, null);
                //Bitmap bt = new Bitmap(pc1.BackgroundImage);
                //pc1rez.Image = MakeImage(NewSizeKv(170), arr[frameCount - 1], bt, 230); 
            }
            else
            {
                //pc1rez.Image = 
                pcResult[0].Image = gifImage.GetNextFrame(); //arr[indexToPaint];
            }
        }
        void gifTimer_Tick2(object sender, EventArgs e)
        {
            indexToPaint++;
            if (indexToPaint > gifImage.FrameCount)
            {
                indexToPaint = 0;
                gifTimer2.Stop();
                //Bitmap bt = new Bitmap(pc2.BackgroundImage);
                //pc2rez.Image = MakeImage(NewSizeKv(170), arr[frameCount - 1], bt, 230);
            }
            else
            {
                //pc2rez.Image = gifImage.GetNextFrame(); //arr[indexToPaint];
                pcResult[1].Image = gifImage.GetNextFrame(); //arr[indexToPaint];
            }
        }
        void gifTimer_Tick3(object sender, EventArgs e)
        {
            indexToPaint++;
            if (indexToPaint > gifImage.FrameCount)
            {
                indexToPaint = 0;
                gifTimer3.Stop();
                //Bitmap bt = new Bitmap(pc3.BackgroundImage);
                //pc3rez.Image = MakeImage(NewSizeKv(170), arr[frameCount - 1], bt, 230);
            }
            else
            {
                //pc3rez.Image = gifImage.GetNextFrame(); //arr[indexToPaint];
                pcResult[2].Image = gifImage.GetNextFrame(); //arr[indexToPaint];
            }
        }

        public void close()//удаление всех элементов с поля ответ
        {
            /*if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    close();
                });
            }
            else
            {*/
                workForm.Invalidate();
                workForm.Refresh();
                this.Dispose();
            //}
        }
        public void polosaStart(GeneralForm ff, int Curstep,Polosa pol)//старт временной полосы?походу больше не нужен
        {
            if (step != Curstep)
            {
                step = Curstep;
                pol.AnyEventHarakiri();
                pol.polosa(100, NewPoint(1700, 1350), ff, "CurStep = " + Curstep.ToString());
                pol.onPolosaEnd += sendOtvet;
            }
        }
        void sendOtvet()//отправка ответа на сервер
        {
            txBox.Enabled = false;
            SendData snd = new SendData();
            snd.otvet = txBox.Text;
            snd.uid = predUs.GameZone;
            snd.step =(byte) step;
            snd.table = (byte)tableofkom;
            snd.kluch = predUs.team[tableofkom].kod;
            
            cn.SendUDP("ogg" + JsonConvert.SerializeObject(snd));        
        }
    }
    public class GetStavka : resize //Вывод темы и прием ставок команд
    {
        #region //данные
        public delegate void temaFinish(int st);
        public temaFinish onStavka;
        Label lbin,lbin1; // = new Label();
        PictureBox lbPlus; // = new Panel();
        PictureBox lbMines; // = new Panel();
        public PictureBox stavkaRegion;
        Label lbText;
        Label ff = new Label();
        int stMin, stMax;
        int stDelta = 25;
        #endregion
        
        public void stavka(int minSt, int MaxSt, GeneralForm fsv,Polosa pol)
        {
            workForm = fsv;
            //Bitmap bit = new Bitmap(Properties.Resources.SpinEdit_color);

            if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    stavka(minSt, MaxSt, fsv,pol);
                });
            }
            else
            {
                if (stavkaRegion == null)
                {
                    stavkaRegion = new PictureBox
                    {
                        BackgroundImage = Properties.Resources.SpinEdit_color, // bit;
                        BackgroundImageLayout = ImageLayout.Zoom,
                        Parent = workForm,
                        Location = NewPoint(1800, 940),
                        Size = NewSize(585, 249),
                        BackColor = Color.Transparent
                    };


                    #region//описание кнопок увеличение и уменьшение ставок
                    stMin = minSt; stMax = MaxSt;
                    lbPlus = new PictureBox
                    {
                        BackgroundImage = Properties.Resources.Up,
                        Parent = stavkaRegion,
                        Location = new Point((int)(stavkaRegion.Width * 0.72), (int)(stavkaRegion.Height * 0.15)),
                        Size = new Size((int)(stavkaRegion.Width * 0.22), (int)(stavkaRegion.Height * 0.305)),
                        BackgroundImageLayout = ImageLayout.Zoom,
                        Name = "oneuse",
                    };
                    lbPlus.Click += LbPlus_Click;
                    lbPlus.MouseDown += lbSpinBtn_MouseDown;
                    lbPlus.MouseUp += lbSpinBtn_MouseUp;

                    lbMines = new PictureBox
                    {
                        Parent = stavkaRegion,
                        BackgroundImage = Properties.Resources.Down,
                        Size = new Size((int)(stavkaRegion.Width * 0.22), (int)(stavkaRegion.Height * 0.305)),
                        BackgroundImageLayout = ImageLayout.Zoom,
                        Visible = true,
                        Location = new Point((int)(stavkaRegion.Width * 0.72), (int)(stavkaRegion.Height * 0.55)),
                        Name = "oneuse"
                    };
                    lbMines.Click += LbMines_Click;
                    lbMines.MouseDown += lbSpinBtn_MouseDown;
                    lbMines.MouseUp += lbSpinBtn_MouseUp;
                    #endregion

                    #region //описание области ввода ставки
                    lbin = new Label
                    {
                        Text = Convert.ToString(minSt),
                        Parent = stavkaRegion,
                        Location = new Point((int)(stavkaRegion.Height * 0.06), (int)(stavkaRegion.Height * 0.1)),
                        Visible = true,
                        Font = new Font("Arial Black Italic", (int)(stavkaRegion.Height * 0.5)),
                        ForeColor = Color.Black,
                        BackColor = Color.Transparent,
                        Size = new Size((int)(stavkaRegion.Width * 0.68), (int)(stavkaRegion.Height * 0.8)),
                        TextAlign = ContentAlignment.TopCenter,
                        Name = "oneuse"
                    };

                    lbin1 = new Label
                    {
                        Text = Convert.ToString(minSt),
                        Parent = lbin,
                        Location = NewRelPoint(-5, -5),// new Point((int)(stavkaRegion.Height * 0.06), (int)(stavkaRegion.Height * 0.1));
                        Visible = true,
                        Font = new Font("Arial Black Italic", (int)(stavkaRegion.Height * 0.5)),
                        ForeColor = Color.Gold,
                        Size = new Size((int)(stavkaRegion.Width * 0.68), (int)(stavkaRegion.Height * 0.8)),
                        TextAlign = ContentAlignment.TopCenter,
                        BackColor = Color.Transparent,
                        Name = "oneuse"
                    };
                    #endregion

                }
                else
                {
                    stavkaRegion.Visible = true;
                    stMin = minSt; stMax = MaxSt;
                    stDelta = 25;
                    lbin.Text = Convert.ToString(minSt);
                    lbin1.Text = Convert.ToString(minSt);
            
                }

                pol.AnyEventHarakiri();
                pol.onPolosaEnd += StavkaEndTime;
                pol.polosa(200, NewPoint(1700, 1350), fsv, "Stavka");
            }
        }

        private void lbSpinBtn_MouseDown(object sender, EventArgs e)
        {
            ((PictureBox)sender).Location = new Point(((PictureBox)sender).Location.X + 2, ((PictureBox)sender).Location.Y + 2);
        }
        private void lbSpinBtn_MouseUp(object sender, EventArgs e)
        {
            ((PictureBox)sender).Location = new Point(((PictureBox)sender).Location.X - 2, ((PictureBox)sender).Location.Y - 2);
        }
        private void LbPlus_Click(object sender, EventArgs e)//увеличение ставки
        {
            if (Convert.ToInt16(lbin.Text) < stMax)
            {
                lbin.Text = Convert.ToString(Convert.ToInt32(lbin.Text) + stDelta);
                lbin1.Text = Convert.ToString(Convert.ToInt32(lbin1.Text) + stDelta);
            }
        }
        private void LbMines_Click(object sender, EventArgs e)//уменьшение ставки
        {
            if (Convert.ToInt16(lbin.Text) > stMin)
            {
                lbin.Text = Convert.ToString(Convert.ToInt32(lbin.Text) - stDelta);
                lbin1.Text = Convert.ToString(Convert.ToInt32(lbin1.Text) - stDelta);
            }
        }
        private void StavkaEndTime()
        {
            stDelta = 0;
            // lbMines.Dispose();
            // lbPlus.Dispose();
            // lbText.Dispose();         
            onStavka(Convert.ToInt32(lbin.Text));
            stavkaRegion.Visible=false;
            //  lbin.Dispose();
            workForm.Invalidate();
        }
    }
    public class resize : IDisposable
    {
        public static Size resolution; // = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;
        private static GeneralForm myWorkForm;
        private static int delta;
        public static Size workResolution;
        public GeneralForm workForm { get { return myWorkForm; }
                                      set { GetWorkingClientSize(value); } }
        private bool disposed = false;

        // реализация интерфейса IDisposable.
        public void Dispose()
        {
            Dispose(true);
            // подавляем финализацию
            GC.SuppressFinalize(this);
        }
        /*public void DisposeItems<T>(this IEnumerable<T> source) where T : IDisposable
        {
            foreach (var item in source)
            {
                item.Dispose();
            }
        }
        public void DisposeSequence<T>(this IEnumerable<T> source)
        {
            foreach (IDisposable disposableObject in source
                     .Where(source is System.IDisposable)
                     .Select(source as System.IDisposable)
            {
                disposableObject.Dispose();
            };
        }
        public void DisposeAll(this IEnumerable set)
        {
            foreach (Object obj in set)
            {
                IDisposable disp = obj as IDisposable;
                if (disp != null) { disp.Dispose(); }
            }
        }*/

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                }
                // освобождаем неуправляемые объекты
                disposed = true;
            }
        }

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

        private static Size GetWorkingClientSize(GeneralForm form)
        {
            /*var original = form.WindowState;
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
            }*/
            resolution = form.resolution;
            delta = form.delta;
            workResolution = new Size(resolution.Width + delta * 2, resolution.Height);
            myWorkForm = form;
            return resolution;
        }
        [DllImport("User32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private enum Message : int
        {
            WM_SETREDRAW = 0x000B, // int 11
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
        
        #region функции пересчета координат и размеров
        public Point NewPoint(int x, int y)     //производит пересчет к новым координатам
        {
            //resolution = GetWorkingClientSize(myWorkForm);
            return new Point((int)(x * resolution.Width / 2500) + (delta > 0 ? delta : 0), (int)(resolution.Height * y / 1600));
        }
        public Point NewRelPoint(int x, int y)     //производит пересчет к новым координатам
        {
            //resolution = GetWorkingClientSize(myWorkForm);
            return new Point((int)(x * workResolution.Width / 2500), (int)(workResolution.Height * y / 1600));
        }
        public Size NewSize(int x, int y)  //производит пересчет к новым размерам
        {
            //resolution = GetWorkingClientSize(myWorkForm);
            return new Size(x * resolution.Width / 2500, y * resolution.Height / 1600);
        }
        public Size NewSizeKv(int x)
        {
            //resolution = GetWorkingClientSize(myWorkForm);
            return new Size(x * resolution.Height / 1600, x * resolution.Height / 1600);
        }
        public float NewFontSize(float x)     //производит пересчет к новым координатам
        {
            return x * resolution.Height / 1017;
            //return new Point((int)(x  this.Width / 2500), (int)(this.Height  y / 1600));
        }
        #endregion
    }
}