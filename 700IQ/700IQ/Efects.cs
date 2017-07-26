﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Runtime.InteropServices;

namespace _700IQ
{
    public class IkonShow : resize // вывод анимации номера айкона
    {
        #region //переменные
        public delegate void stopIkon();
        public event stopIkon onStop;
        private PictureBox pictureBox1 = new PictureBox();
        private int ikon;
        //Size resolution; // System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;
        //System.Windows.Forms.Timer tmi = null;
        System.Timers.Timer tmi = null;
        byte tikStep = 0;
        Graphics g;
        //Form fsv;
        #endregion

        public void Ikon(GeneralForm ff, int ikon) //анимация номера айкона и начало 
        {
            #region//описание полй вывода
            workForm = ff;
            this.ikon = ikon;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            workForm.Controls.Add(pictureBox1);
            #endregion

            tmi = new System.Timers.Timer(300);
            tikStep = 0;
            //tmi.Interval = 300;
            //tmi.Tick += new EventHandler(tmi_Tick);
            tmi.Elapsed += tmi_Tick;
            tmi.AutoReset = true;
            tmi.Enabled = true;
            //tmi.Start();
        }
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle Rect;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;//выравнивание по горизонтали
            sf.LineAlignment = StringAlignment.Center;//выравнивание по вертикали
            int height = (workForm.ClientSize.Height - 350) / 2;// отступ от верхнего края
            Rect = new Rectangle(6, height + 3, workForm.ClientSize.Width, 300);

            g.DrawString((ikon).ToString(), new Font("Times New Roman", 250), Brushes.Black, Rect, sf);
            Rect = new Rectangle(0, height, workForm.ClientSize.Width, 300);
            g.DrawString((ikon).ToString(), new Font("Times New Roman", 250), Brushes.Yellow, Rect, sf);
            //g.DrawRectangle(Pens.Blue, Rect);
            Rect = new Rectangle(6, height + 3 + 250, workForm.ClientSize.Width, 100);
            g.DrawString("айкон", new Font("Times New Roman", 52), Brushes.Black, Rect, sf);
            Rect = new Rectangle(0, height + 250, workForm.ClientSize.Width, 100);
            g.DrawString("айкон", new Font("Times New Roman", 52), Brushes.Yellow, Rect, sf);
            //g.DrawRectangle(Pens.Blue, Rect);
           
            
            /*
            int zdvig = ikon < 10 ? 100 : 0;
            //if (ikon < 10) zdvig = 100;
            g.DrawString((ikon).ToString(), new Font("Times New Roman", 250), Brushes.Black, NewPoint(843 + zdvig, 203));
            g.DrawString("айкон", new Font("Times New Roman", 52), Brushes.Black, NewPoint(1003, 853));
            g.DrawString((ikon).ToString(), new Font("Times New Roman", 250), Brushes.Yellow, NewPoint(840 + zdvig, 200));
            g.DrawString("айкон", new Font("Times New Roman", 52), Brushes.Yellow, NewPoint(1000, 850));
            */
        }

        private void HideIkon()
        {
            if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    HideIkon();
                });
            }
            else
            {
                pictureBox1.Dispose();
                workForm.Invalidate();
            }
        }
        //private void tmi_Tick(object sender, EventArgs e) //таймер 
        private void tmi_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            tikStep++;
            if (tikStep == 15)
            {
                /*Point pn = NewPoint(900, 250);
                Size sz = NewSize(600, 750);              
                Bitmap bmp = new Bitmap(Properties.Resources.GreenTable, resolution);
                Bitmap bmpClear = new Bitmap(sz.Width, sz.Height);
                bmpClear = bmp.Clone(new RectangleF(pn, sz), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = workForm.CreateGraphics();
                g.DrawImage(bmpClear, pn);
                g.Dispose();*/
                HideIkon();
            }
            if (tikStep == 20)
            {
                tmi.Dispose();
                onStop();
            }
        }
    }
    public class StavkiPlus
    {
        Bitmap bmp = new Bitmap(100, 100);
        public void StavkaGoPlus(GeneralForm ff)
        {
            //Graphics g = ff.CreateGraphics();
            //g.DrawString("<<< 600", new Font("times new roman", 30), Brushes.Yellow, NewPoint(500, 500));
            //Bitmap ekran = new Bitmap(Properties.Resources.GreenTable, resolution);
            //Bitmap bmpPolosa = new Bitmap(200, 50);
            //Point pn = new Point(500, 400);
            //Size sz = new Size(200, 50);
            //bmpPolosa = ekran.Clone(new RectangleF(pn, sz), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //Graphics g1 = Graphics.FromImage(bmpPolosa);

        }
    }
    public class StavkiShow:resize
    {
        class stakan:resize
        {
            public  delegate void StopST();
            public event StopST onStop;
            private FrameDimension dimension;
            private int frameCount;
            private Image img;
            private int indexToPaint;
            private Timer timer = new Timer();
      
            PictureBox pc = new PictureBox();
            Label lb, lbSt;
            int stavka;
            int komanda;
            GeneralForm fsv;
            ~stakan()
            {
                
            }
            public void stak(int st1, Point point, GeneralForm fsv, int komanda)
            {
                this.komanda = komanda;
                this.fsv = fsv;
                workForm = fsv;
                if (workForm.InvokeRequired)
                {
                    workForm.BeginInvoke((MethodInvoker)delegate
                    {
                        stak(st1, point, fsv, komanda);
                    });
                }
                else
                {
                    pc.Size = NewSize(150, 500);
                    pc.Location = point;
                    pc.BackColor = Color.Transparent;
                    pc.SizeMode = PictureBoxSizeMode.StretchImage;
                    pc.Parent = workForm;
                    lb = new Label()
                    {
                        Parent = workForm,
                        Location = new Point(point.X - 20, point.Y + pc.Size.Height + 20),
                        Size = NewSize(150, 50) + new Size(40,0),
                        Name = "oneuse",
                        ForeColor = Color.White,
                        BackColor = Color.Transparent,
                        Font = new Font("arial", 12),
                        TextAlign = ContentAlignment.TopCenter,
                    };
                    pc.BringToFront();
                    lbSt = new Label() //метка размера ставки
                    {
                        Parent = workForm,
                        Location = new Point(point.X - 20, point.Y - 50),
                        Size = NewSize(150, 50) + new Size(40, 0),
                        Name = "oneuse",
                        ForeColor = Color.Gold,
                        BackColor = Color.Transparent,
                        Font = new Font("arial", 18),
                        TextAlign = ContentAlignment.TopCenter,
                    };
                    if (komanda != 0 && komanda < 4) lb.Text = komanda + " команда";
                    if (komanda > 5)
                    {
                        lbSt.Visible = false;
                        lb.Text = "Выигрыш  составил "+ lb.Text  + " - " + komanda + " айкэш";
                        lb.Size = NewSize(600, 70);
                        lb.Font = new Font("arial", 15);
                    }
                    if (komanda == 0) lbSt.Visible = false;
                    img = Properties.Resources._12_50int2;
                    dimension = new FrameDimension(img.FrameDimensionsList[0]);
                    frameCount = img.GetFrameCount(dimension);
                    //arr = new Bitmap[frameCount];
                    int[] frame = new int[] { 12, 24, 36, 47, 58, 69, 80, 90, 100, 110, 120, 130 };

                    frameCount = frame[st1 - 1];
                    /*
                    for (int i = 0; i < frameCount; i++)
                    {
                        img.SelectActiveFrame(dimension, i);
                        var bit = new Bitmap(img);
                        arr[i] = bit;
                    }
                    */
                    stavka = st1;
                    timer.Interval = 15;
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();
                }
            }
            void timer_Tick(object sender, EventArgs e)
            {
                indexToPaint++;
                if (indexToPaint >= frameCount)
                {
                    timer.Stop();
                    onStop();
                }
                else
                {
                    lbSt.Text = (Convert.ToInt16(stavka * 25 * (indexToPaint + 1) / frameCount / 25) * 25).ToString();
                    img.SelectActiveFrame(dimension, indexToPaint);
                    pc.Image = new Bitmap(img);//arr[indexToPaint];

                    if (this.komanda-1 == this.fsv.iQash1.number) this.fsv.iQash1.Text = (this.fsv.steck.team[this.fsv.iQash1.number].iQash + 25 * stavka  - Convert.ToInt16(stavka * 25 * (indexToPaint + 1) / frameCount / 25) * 25).ToString() + " IQ";//(Convert.ToInt32(this.fsv.iQash1.Text.Substring(0, this.fsv.iQash1.Text.Length - 3)) - 25).ToString() + " IQ";
                    if (this.komanda-1 == this.fsv.iQash2.number) this.fsv.iQash2.Text = (this.fsv.steck.team[this.fsv.iQash2.number].iQash + 25 * stavka - Convert.ToInt16(stavka * 25 * (indexToPaint + 1) / frameCount / 25) * 25).ToString() + " IQ";
                    if (this.komanda-1 == this.fsv.iQash3.number) this.fsv.iQash3.Text = (this.fsv.steck.team[this.fsv.iQash3.number].iQash + 25 * stavka - Convert.ToInt16(stavka * 25 * (indexToPaint + 1) / frameCount / 25) * 25).ToString() + " IQ";
                }
            }
            public void del()
            {
                pc.Dispose();
                timer.Dispose();
                lb.Dispose();
                lbSt.Dispose();
                workForm.Invalidate();
            }
        }
        public delegate void stShowFinish();
        public stShowFinish onStShow;
        PictureBox pc2 = new PictureBox();
        PictureBox pc3 = new PictureBox();
        PictureBox pc4 = new PictureBox();
        //Form ff = new Form();
        int stav1, stav2, stav3, stav4;
        Point pn = new Point();
        private Timer timer = new Timer();
        bool itsStavka;
        stakan st, st2, st3, st4;
        int distance;
        int size_stack;
        ~StavkiShow()
        {
          
        }
        public void inputStavki(int st1, int st2, int st3, int st4, GeneralForm fsv)
        {
            //workForm = fsv;
            stav1 = st1;
            stav2 = st2;
            stav3 = st3;
            stav4 = st4;
            //ff = v;

            if (stav1 == 0)
            {
                onStShow();
                return;
            }

            size_stack = NewSize(150, 0).Width;
            distance = NewSize(400, 0).Width / 2;

            if (st4 == 0 && st3 != 0)
            {
                pn = NewPoint(825, 400);
                itsStavka = true;
            }
            else  pn = NewPoint(1300, 850);

            st = new stakan();
            st.onStop += stavka2;
            int anons = stav1 + stav2 + stav3 + stav4;
            if (itsStavka) anons = 1;
            st.stak(stav1/25,pn, workForm, anons);
        }
        void stavka2()
        {
            if (stav2 == 0)
            {
                endofStavka();
                return;
            }
            st2 = new stakan();
            st2.onStop += stavka3;
            pn.X += size_stack;//150
            int anons = 0;
            if (itsStavka)
            {

                anons = 2;
                pn.X += distance;//50
            }
            else pn.X += distance / 2;

            st2.stak(stav2 / 25, pn, workForm, anons);
        }
        void stavka3()
        {
            if (stav3 == 0)
            {
                endofStavka();
                return;
            }
            st3 = new stakan();
            st3.onStop += stavka4;
            pn.X += size_stack;
            int anons = 0;
            if (itsStavka)
            {
                anons = 3;
                pn.X += distance;
            }
            else pn.X += distance / 2;
            st3.stak(stav3 / 25, pn, workForm, anons);
        }
        void stavka4()
        {
            if (stav4 == 0)
            {
                endofStavka();
                return;
            }
            st4 = new stakan();
            st4.onStop += endofStavka;
            pn.X += size_stack;
            pn.X += distance / 2;

            st4.stak(stav4 / 25, pn, workForm, 0);
        }
        void endofStavka()
        {
            timer.Interval = 7000;
            timer.Tick += Timer_Tick;
            timer.Start();
         
        }

        private void Timer_Tick(object sender, EventArgs e) 
        {
            timer.Stop();
            st.del();
            if (st2!=null) st2.del();
            if (st3!=null) st3.del();
            if (st4!=null) st4.del();
            onStShow();          
        }
    }
    public class Polosa:resize//Полоса ожидания
    {
        #region//переменные
       
        public delegate void PolosaEnd();
        public PolosaEnd onPolosaEnd;
        //Size resolution; // = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size;
        public PictureBoxWithInterpolationMode pcBox;
        public CircularProgressBar prBar;
        //private Form fsv;

        public Label ff;
        System.Timers.Timer tmBar = new System.Timers.Timer();
        #endregion
        public void polosa(int t, Point pn, GeneralForm fsv, string txt = "")
        {
            workForm = fsv;
            //resolution = Screen.FromControl(fsv).WorkingArea.Size;
            //resolution = fsv.ClientSize;
           
            InitBar(t, pn, txt);
       
        }
        public int Value
        {
            get { return prBar.Value; }
            set { prBar.Value = value; }
        }
        public void AnyEventHarakiri()
        {
            if (this.onPolosaEnd == null) return;
            foreach (Delegate d in this.onPolosaEnd.GetInvocationList())
            {
                this.onPolosaEnd -= (PolosaEnd)d;
            }
        }

        private void InitBar(int t, Point pn, string txt)
        {
            //fsv.Invoke(new MethodInvoker(() =>
            //{
            if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    InitBar(t ,pn, txt);
                });
            }
            else
            {
                tmBar = new System.Timers.Timer();

                if (ff == null)
                {
                    #region //описание области вывода полосы

                    ff = new Label()
                    {
                        BackColor = Color.Transparent,
                        Location = pn,
                        Size = NewSize(800, 250),
                        Font = new Font("Arial ", NewFontSize(1)),
                        Text = txt,
                        Parent = workForm,
                        ForeColor = Color.Green,
                    };
                    #endregion
                    prBar = new CircularProgressBar()
                    {
                        Location = NewRelPoint(350, 0),
                        AutoResetColor = Color.FromArgb(200, 12, 163, 218),
                        Parent = ff,
                        Size = NewSizeKv(200),
                        Value = 0,
                        Visible = true,
                        Maximum = 360,
                        Gradient = false,
                        ProgressSize = NewWidth(22),
                        interval = t,
                };
                    pcBox = new PictureBoxWithInterpolationMode
                    {
                        Parent = prBar,
                        Visible = true,
                        Location = new Point(prBar.ProgressSize, prBar.ProgressSize),
                        Size = new Size(prBar.Size.Width - (prBar.ProgressSize * 2), prBar.Size.Width - (prBar.ProgressSize * 2)),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        SmoothingMode = SmoothingMode.AntiAlias,
                        InterpolationMode = InterpolationMode.HighQualityBicubic,
                        Image = Properties.Resources.ok,
                    };
                    pcBox.MouseDown += PcBox_MouseDown;
                    pcBox.MouseUp += PcBox_MouseUp;
                }
                else
                {
                    ff.Visible = true;
                    ff.Text = txt;
                    if ((txt != "Step4 - Zero") && (txt != "Step7 - NoAnswer")&& (txt != "синхр")) pcBox.Visible = true;
                }
                //prBar.BackgroundImage = ((Bitmap)workForm.BackgroundImage).Clone(new Rectangle(new Point(pn.X + prBar.Location.X, pn.Y + prBar.Location.Y), prBar.Size), PixelFormat.Format32bppArgb);
                //pcBox.Visible = (txt == "синхр") ? false : true;
                prBar.SetInterval(t);
                prBar.AutoReset = (t == 1) ? true : false;
                tmBar.Interval = t;
                tmBar.Elapsed += TmBar_Tick;
                tmBar.AutoReset = true;
                tmBar.Start();
            }
        }
        //private void TmBar_Tick(object sender, EventArgs e)//изменение временной полосы
        private void TmBar_Tick(object state, System.Timers.ElapsedEventArgs e)//изменение временной полосы
        {
            if (ff.InvokeRequired)
            {
                ff.BeginInvoke((MethodInvoker)delegate
                {
                    TmBar_Tick(state, e);
                });
            }
            else
            {
                if (prBar.Value < prBar.Maximum)
                {
                    //ff.Text = prBar.ProgressColor1.R + "  " + prBar.ProgressColor1.G;
                    prBar.Value++; // Value++;
                }

                else
                {
                    ((System.Timers.Timer)state).Stop();
                    ((System.Timers.Timer)state).Dispose();
                    prBar.Value = 0;
                    pcBox.Visible = false;
                    ff.Visible = false;
                    workForm.Invalidate();
                    onPolosaEnd?.Invoke();
                }
            }
            //var reportProgress = new Action(() =>
            //{
            //    if (prBar.Value < 100) prBar.PerformStep(); // Value++;
            //    else
            //    {
            //        tmBar.Enabled = false;
            //        //tmBar.Stop();
            //        tmBar.Dispose();
            //        ff.Visible = false;
            //        pcBox.Visible = false;
            //        workForm.Invalidate();
            //        onPolosaEnd();
            //    }
            //});
            //workForm.BeginInvoke(reportProgress);
        }

        public void Finish()
        {
            ff.Visible = false;
            prBar.Value = prBar.Maximum;
        }
        public void PcBox_MouseUp(object sender, MouseEventArgs e)//нажатие кнопки ОК
        {
            var reportProgress = new Action(() =>
            {
                pcBox.Location = new Point(pcBox.Location.X - 2, pcBox.Location.Y - 2);
                //pcBox.Image = Properties.Resources.Неактивная;
                Finish();
            });
            workForm.BeginInvoke(reportProgress);
        }
        public void PcBox_MouseDown(object sender, MouseEventArgs e)//отображение нажатия кнопки
        {
            //pcBox.Image = Properties.Resources.Активная;
            pcBox.Location = new Point(pcBox.Location.X + 2, pcBox.Location.Y + 2);
        }
        public void WorkForm_KeyUp(object sender, KeyEventArgs e)
        {
            pcBox.Focus();
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("aasdsadas");
               /* var reportProgress = new Action(() =>
                {
                    pcBox.Image = Properties.Resources.Неактивная;
                    tmBar.Stop();
                    tmBar.Dispose();
                    ff.Dispose();
                    pcBox.Dispose();
                    workForm.Invalidate();
                    onPolosaEnd();
                });
                workForm.BeginInvoke(reportProgress);*/
            }

        }

        public void WorkForm_KeyDown(object sender, KeyEventArgs e)
        {
            pcBox.Focus();
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("aasdsadas");// pcBox.Image = Properties.Resources.Активная;
            }

        }

    }

    public class Rul : PictureBox  //класс рулетка
    {
        #region //описание переменных
        public delegate void stopRul();
        public event stopRul onStop;
        Rectangle rc;
        public bool Enabled
        {
            get { return enabled; }
        }
        private bool enabled = false;
        System.Windows.Forms.Timer tm = null;
        private GeneralForm fsv;
        //Label ff = new Label();
        Bitmap bmp;
        Image RouletteBall; // = new Bitmap(15, 15);
        Graphics g;
        Point point;
        //Point point1, point2;
        Rectangle z1;
        Rectangle z2;
        float i = 0f, vr = 0f;
        double centrx, centry, radius;
        float stepi = 0.00002f;// ускорение
        float vi = 2 * 0.00002f;// начальная скорость = 2 * ускорение
        float ifr = 0.03f;// при какой скороски начинает уменьшаться радиус
        float stepr = 0.0001f;//шаг изменения радиуса
        //float rotation_count = 5f;//количество полных оборотов рулетки (30сек.)

        float koef;
        bool flag, flagStop;
        int tickNumber = 0, nStop = 100;
        int offset=5;

        Video video;

        #endregion
        public Size size;
        public Video LoadVideo(int cell)
        {
            Video temp_video;
            string path = System.IO.Path.GetDirectoryName(Application.StartupPath);//получение текущей папки
            try
            {
                temp_video = new Video(path + String.Format("\\Video\\{0}.avi",cell + 1), false);
            }
            catch
            {
                try
                {
                    temp_video = new Video(path + String.Format("\\Debug\\Video\\{0}.avi", cell + 1), false);
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки видео \n" + Marshal.GetLastWin32Error());
                    temp_video = null;
                }
            }
            return temp_video;
        }
        public void StartRul(int cel, Rectangle rc, GeneralForm fsv, int rotation_count=5)
        {
            this.Visible = false;
            this.rc = rc;
            flagStop =false;
            tickNumber = 0; 
            this.fsv = fsv;
            //bmp = Properties.Resources.Ruletka;
            //g = Graphics.FromImage(bmp);
            //RouletteBall = Properties.Resources.ShadowBall;
            //RouletteBall = Image.FromFile(@"d:\Картинки\700IQ\ShadowBall.png");

            flag = false;
            //vi = 0.04f;
            //vi += 0.00004f + 0.00008f;
            //i = 0.157080f+0.07854f;      //6.28319 количество радиан в 360 градусах
            //40 ячеек 9 градусов на ячейку, 1 градус - 0.0174533 радиана
            //1 ячейка - 0.15708 радиана 1/2 ячейки = 0.07854
            //стартовая позиция 11,5 ячеек = 1.80642 радиана
            // удленнение пути на 1 ячейку равно корень из 2а(S+n*0.15708)

            //2*pi/37 - количество радиан в 1 ячейке
            // vi = (float)Math.Sqrt(0.00004f * (37 + (14 + cel) * 0.15708f));
            this.Location = rc.Location;
            this.Parent = fsv;
            //this.Paint += OnPaint;
            //SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            //DoubleBuffered = true;
            this.enabled = true;

            video = LoadVideo(cel);//new Video(@"d:\01.1.avi", false);

            //size = video.Size;
            //this.Size = size;
            video.Owner = this;
            this.Size = rc.Size;
            double koeff = 1.78; // (double)Width / (double)Height;
            size = new Size((int)(this.Height * koeff), this.Height);
            //this.Size = rc.Size;
            video.Size = size;
            video.Ending += Video_Ending;
            this.Visible = true;
            this.BringToFront();
            this.Refresh();
            //this.Invalidate();
            if (video != null) video.Play();
            //tmrVideo.Enabled = true;
            //btnPlayPause.Text = "Pause";

            #region//описание свойств формы
            //this.Visible = true;
            //this.Location = rc.Location;
            //this.BackgroundImage = bmp;
            //this.BackgroundImageLayout = ImageLayout.Zoom;
            //this.BackColor = Color.Transparent;
            //this.Size = rc.Size;
            //this.Parent = fsv;
            //SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            //DoubleBuffered = true;
            //this.enabled = true;
            //this.BringToFront();
            #endregion
            //g.Dispose();
            //this.Invalidate();
        }

        private void Video_Ending(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                //System.Threading.Thread.Sleep(2000);

                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        enabled = false;
                        onStop?.Invoke(); 
                    }));
                }
            });
        }
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_LBUTTONDOWN = 0x201;
        const int WM_LBUTTONUP = 0x202;
        const int WM_LBUTTONDBLCLICK = 0x203;
        const int VK_ESCAPE = 0x1b;
        protected override void WndProc(ref Message m)
        {
            //We have to change the clientsize to make room for borders
            //if not, the border is limited in how thick it is.
            switch(m.Msg)
            {
                case WM_LBUTTONDBLCLICK:  //WM_MOUSE_DOUBLE_CLICK
                    if(!video.Fullscreen)
                        video.Fullscreen = true;  //!video.Fullscreen;
                    break;
                case WM_KEYUP:
                    if (video.Fullscreen && (int)m.WParam == VK_ESCAPE)
                        video.Fullscreen = false;
                    break;
                case WM_MOUSEMOVE:
                    //int lParam = m.LParam.ToInt32();
                    //mouseLoc.X = lParam & 0xFFFF;
                    //mouseLoc.Y = (int)(lParam & 0xFFFF0000 >> 16);
                    Point mouseLoc = new Point();
                    mouseLoc.X = (Int16)m.LParam;
                    mouseLoc.Y = (Int16)((int)m.LParam >> 16);
                    if ((int)m.WParam == 0x001)
                        this.Location.Offset(10, 10);
                    break;

                    //if (m.WParam == IntPtr.Zero)   {  }   else         {      }
            }
            /*if (m.Msg == 0x85) //WM_NCPAINT
            {
            }*/
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            /*pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //pe.Graphics.FillEllipse(Brushes.White, z2);
            base.OnPaint(pe);
            pe.Graphics.DrawImage(RouletteBall, z2);*/

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            //System.Drawing.Rectangle r = new Rectangle((int)((rc.X - (fsv.delta < 0 ? fsv.delta : 0)) * koef1), (int)(rc.Y * koef2), (int)(size.Width * 0.53), (int)(size.Width * 0.53));
            System.Drawing.Rectangle r = new Rectangle((int)((size.Width*519/1920)), (int)(size.Width * 48/1920), (int)(size.Width * 1020/1920), (int)(size.Width * 1020/1920));   
            //System.Drawing.Rectangle r = new Rectangle(292, 28, 574, 571);
            e.Graphics.DrawEllipse(Pens.Black, r);//после нужного вам результата замените - Pens.Transparent
            gp.AddEllipse(r);
            Region = new System.Drawing.Region(gp);

        }
        public void close()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    close();
                });
            }
            else
            {
                tm?.Stop();
                this.Visible = false;
                video?.Dispose();
                this.enabled = false;
            }
        }
        public void AnyEventHarakiri()
        {
            if (this.onStop == null) return;
            foreach (Delegate d in this.onStop.GetInvocationList())
            {
                this.onStop -= (stopRul)d;
            }
        }

        private void ruletka()
        {

            z1.Location = z2.Location;
            z1.Offset(-offset, -offset);
            point.X = (int)(centrx + radius * Math.Cos(i));
            point.Y = (int)(centry + radius * Math.Sin(i));
            //point2.X = point.X - 30;
            //point2.Y = point.Y - 30;
            //z1.Location = point2;
            z2.Location = point;


            ////if (flag) g.DrawImage(zona, point1);
            ////else flag = true;

            ////zona = bmp.Clone(z1, bmp.PixelFormat);
            ////g.FillEllipse(Brushes.White, z2);
            //point1 = point2;
            //z1.X = (int)(z1.X * koef);
            //z1.Y = (int)(z1.Y * koef);
            this.Invalidate(); //z1
         
        }
        private void tm_Tick(object sender, EventArgs e)
        {
            /*tickNumber++;
            this.Location = new Point(655 + tickNumber*10, 107);
            this.Refresh();
            this.Invalidate();*/

            /*{
                tickNumber++;
                if (!flagStop)
                {
                    ruletka();
                    if (tickNumber > 100)
                    {
                        i += vi;
                        vi -= stepi;
                        if (vi <= 0)
                        {
                            flagStop = true;
                            tickNumber = 0;
                            ruletka();
                        }
                        else
                        {
                            if (vi < ifr)
                            {
                                //if (radius > 118 && vr < 0) { vr -= stepr * 8; }
                                //if (radius > 118 && vr >= 0) { vr -= stepr * 30; }//0.0001f
                                //if (radius <= 118 && radius >= 110) { vr += stepr * 3; }
                                //if (radius < 110) { vr = Math.Abs(vr); }
                                if (radius > 118 * koef && vr < 0) { vr -= 0.0008f; }
                                if (radius > 118 * koef && vr >= 0) { vr -= 0.003f; }
                                if (radius <= 118 * koef && radius >= 110 * koef) { vr += 0.0003f; }
                                if (radius < 110 * koef) { vr = Math.Abs(vr); }
                                radius += vr;
                            }
                        }

                    }
                }
                else
                {
                    if (tickNumber > nStop)
                    {
                        tm.Stop();
                        this.enabled = false;
                        onStop?.Invoke();
                    }
                }
            }*/
        }
    }
}
