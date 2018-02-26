using System;
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
                onStop?.Invoke();
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
            public Audio audio;
            public  delegate void StopST();
            public event StopST onStop;
            private FrameDimension dimension;
            private double frameCount;
            private Image img;
            private double indexToPaint;
            private Timer timer = new Timer();
      
            PictureBox pc = new PictureBox();
            Video video;
            Label lb, lbSt;
            int stavka;
            int komanda;
            GeneralForm fsv;
            PictureBox border;
            int number;
            ~stakan()
            {
                //video.Dispose();
              //pc.Dispose();   попытка доступа из другого потока
              //lb.Dispose();
              //lbSt.Dispose();
               // Console.WriteLine("Class destructor...");
            }
            public Audio LoadAudio()
            {
                //Audio temp_audio;
                try
                {
                    return new Audio(Application.StartupPath + "\\Audio\\chips.mp3", false);
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки звука \n" + Marshal.GetLastWin32Error());
                }
                return null;
            }
            public void stak(int st1, Point point, GeneralForm fsv,PictureBox border, int komanda,int number)
            {
                //audio = LoadAudio();
                this.komanda = komanda;
                this.fsv = fsv;
                this.border = border;
                workForm = fsv;
                if (workForm.InvokeRequired)
                {
                    workForm.BeginInvoke((MethodInvoker)delegate
                    {
                        stak(st1, point, fsv,border, komanda,number);
                    });
                }
                else
                {
                   // pc.Size = (number == 0) ? NewSize(160, 500) : NewSize(160, 400);//было 500
                    pc.Location = point;
                    pc.BackColor = Color.Transparent;
                    pc.SizeMode = PictureBoxSizeMode.StretchImage;
                   // pc.BorderStyle = BorderStyle.Fixed3D;
                    pc.Parent = border;
                   // pc.Visible = false;
                    lb = new Label()
                    {
                        Parent = border,
                        Location = new Point(point.X + 20, point.Y-60),
                        Size = NewSize(150, 50) + new Size(40,0),
                        Name = "oneuse",
                        ForeColor = Color.Gold,
                        BackColor = Color.Transparent,
                        Font = new Font("arial", 12),
                        TextAlign = ContentAlignment.TopCenter,
                    };
                    pc.BringToFront();
                    lb.BringToFront();
                    lbSt = new Label() //метка размера ставки
                    {
                        Parent = border,
                        Location = new Point(point.X +20, point.Y-30),
                        Size = NewSize(150, 50) + new Size(40, 0),
                        Name = "oneuse",
                        ForeColor = Color.Gold,
                        BackColor = Color.Transparent,
                        Font = new Font("arial", 18),
                        TextAlign = ContentAlignment.TopCenter,
                    };
                    if (komanda != 0 && komanda < 4)
                    {
                        lb.Text = komanda + " стол";
                    }
                    if (komanda > 5)
                    {
                        lbSt.Visible = false;

                        lb.Text = "Выигрыш команды:" + fsv.predUs.team[number - 1].name + " составил " + komanda + " айкэш";
                        this.number = number;
                        lb.Location = NewRelPoint(0, 410);
                        lb.Size = new Size(700, 70);
                        lb.Font = new Font("arial", 11);
                        if (fsv.predUs.team[this.number - 1].name == fsv.myTeam.name) this.fsv.iQash1.Text = this.fsv.steck.team[this.fsv.iQash1.number].iQash.ToString() + " IQ";
                    }
                    //lb.BringToFront();
                    if (komanda == 0) lbSt.Visible = false;
                    try
                    {
                        frameCount = 0;
                        indexToPaint = 0;
                        //if (video != null)
                        {
                            video?.Stop();
                            video?.Dispose();
                        }
                        video = new Video(Application.StartupPath + String.Format("\\Video\\Chips\\st{0}.mp4", st1), false); //вместо st25 поставить st1
                                                                                                                             //size = video.Size;
                                                                                                                             //this.Size = size;
                        video.Owner = pc;
                        pc.Size = (number == 0) ? NewSize(252, 580) : NewSize(223, 322);
                        video.Size = pc.Size;
                        //video.Ending += Video_Ending;
                        pc.Visible = true;
                        pc.BringToFront();
                        pc.Refresh();
                        try
                        {
                            frameCount = video.Duration;
                        }
                        catch
                        {  }
                        //if (video != null)
                        video.Play();
                    }
                    catch(Exception ex)
                    {
                        //MessageBox.Show("Ошибка загрузки видео:" + Environment.NewLine + "st1 = " + st1 + Environment.NewLine + ex.StackTrace + Environment.NewLine + Marshal.GetLastWin32Error());
                    }

                    /*img = Properties.Resources._12_50int2;
                    dimension = new FrameDimension(img.FrameDimensionsList[0]);
                    frameCount = img.GetFrameCount(dimension);
                    //arr = new Bitmap[frameCount];
                    int[] frame = new int[] { 12, 24, 36, 47, 58, 69, 80, 90, 100, 110, 120, 130 };

                    frameCount = frame[st1 - 1]; */
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
                    //audio.Play();
                    timer.Start();
                }
            }
            void timer_Tick(object sender, EventArgs e)
            {
                double frmCount = 1;
                if (frameCount != 0)
                {
                    indexToPaint = video.CurrentPosition;
                    frmCount = frameCount;
                }
                lbSt.Text = (Convert.ToInt16(stavka * indexToPaint / frmCount) * 25).ToString();
                if (this.komanda - 1 == this.fsv.iQash1.number)
                    this.fsv.iQash1.Text = (this.fsv.steck.team[this.fsv.iQash1.number].iQash + 25 * (stavka - Convert.ToInt16(stavka * indexToPaint / frmCount))).ToString() + " IQ";//(Convert.ToInt32(this.fsv.iQash1.Text.Substring(0, this.fsv.iQash1.Text.Length - 3)) - 25).ToString() + " IQ";
                if (this.komanda - 1 == this.fsv.iQash2.number)
                    this.fsv.iQash2.Text = (this.fsv.steck.team[this.fsv.iQash2.number].iQash + 25 * (stavka - Convert.ToInt16(stavka * indexToPaint / frmCount))).ToString() + " IQ";
                if (this.komanda - 1 == this.fsv.iQash3.number)
                    this.fsv.iQash3.Text = (this.fsv.steck.team[this.fsv.iQash3.number].iQash + 25 * (stavka - Convert.ToInt16(stavka * indexToPaint / frmCount))).ToString() + " IQ";
                //if (indexToPaint >= frameCount)
                if (indexToPaint >= frmCount)
                {
                    timer.Stop();
                    if (video != null && frameCount > 0)
                        video.CurrentPosition = video.Duration;
                    onStop?.Invoke();
                }
                indexToPaint++;
                //else
                //{
                //if (audio.CurrentPosition >= audio.Duration)
                //{
                //    audio.Stop();    
                //}

                //audio.Play();
                //lbSt.Text = (Convert.ToInt16(stavka * 25 * (indexToPaint) / frameCount / 25) * 25).ToString();
                //img.SelectActiveFrame(dimension, indexToPaint);
                //pc.Image = new Bitmap(img);//arr[indexToPaint];


                //if ((komanda > 5) && (fsv.predUs.team[this.number - 1].name == fsv.predUs.team[0].name)) this.fsv.iQash1.Text = (this.fsv.steck.team[this.fsv.iQash1.number].iQash - 25 * stavka + Convert.ToInt16(stavka * 25 * (indexToPaint + 1) / frameCount / 25) * 25).ToString() + " IQ";

                //}
            }
            public void del()
            {
                if (workForm.InvokeRequired)
                {
                    workForm.BeginInvoke((MethodInvoker)delegate
                    {
                        del();
                    });
                }
                else
                {
                    timer.Dispose();
                    pc.Visible = lb.Visible = lbSt.Visible =false;
                    //video.Stop();
                    //video.Dispose();
                    workForm.Invalidate();
                    Console.WriteLine("Deleting video...");
                }
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
        PictureBox  border;
        stakan st, st2, st3, st4;
        int distance;
        int size_stack;
        int number;
        ~StavkiShow()
        {
          
        }
        public void inputStavki(int st1, int st2, int st3, int st4, GeneralForm fsv,int number)
        {
            //workForm = fsv;
            stav1 = st1;
            stav2 = st2;
            stav3 = st3;
            stav4 = st4;
            this.number = number;
            //ff = v;

            if (stav1 == 0)
            {
                onStShow?.Invoke();
                return;
            }

           // size_stack =  NewSize(240, 0).Width;//41
           // distance = NewSize(400, 0).Width /2;
           // pn = NewRelPoint(55, 43);//46,90
            if (st4 == 0 && st3 != 0)
            {
                
                itsStavka = true;
                size_stack = NewSize(252, 0).Width;
                distance = NewSize(400, 0).Width / 2;
                pn = NewRelPoint(17, 123);
                border = new PictureBox
                {
                    Location = NewPoint(860, 310),
                    Size = NewSize(800,800),
                    Name = "oneuse",
                    BackColor = Color.Transparent,
                    Image = Properties.Resources.RAMKA,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Parent = workForm,
                };
            }
            else
            {
                size_stack = NewSize(223, 0).Width;
                distance = NewSize(400, 0).Width / 2;
                pn = NewRelPoint(20, 70);
                border = new PictureBox
                {
                    Location = NewPoint(1500, 790),
                    Name = "oneuse",
                    Size = NewSize(945, 450),
                    BackColor = Color.Transparent,
                    Image = Properties.Resources.RAMKA,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Parent = workForm,
                };
            }

            st = new stakan();
            st.onStop += stavka2;
            int anons = stav1 + stav2 + stav3; // + stav4;
            if (itsStavka) anons = 1;
            st.stak(stav1/25,pn, workForm, border, anons,number);
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
                //pn.X += distance;//50
            }
           // else pn.X += distance / 2;

            st2.stak(stav2 / 25, pn, workForm, border, anons,number);
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
                //pn.X += distance;
            }
            //else pn.X += distance / 2;
            st3.stak(stav3 / 25, pn, workForm, border, anons,number);
        }
        void stavka4()
        {
            if (stav4 < 2) // == 0
            {
                endofStavka();
                return;
            }
            st4 = new stakan();
            st4.onStop += endofStavka;
            pn.X += size_stack;
           // pn.X += distance / 2;

            st4.stak(stav4 / 25, pn, workForm, border, 0,number);
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
            onStShow?.Invoke();          
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
        System.Timers.Timer tmBar; // = new System.Timers.Timer();
        #endregion
        public void polosa(int t, Point pn, GeneralForm fsv, string txt = "")
        {
            workForm = fsv;
            //resolution = Screen.FromControl(fsv).WorkingArea.Size;
            //resolution = fsv.ClientSize;
           
            InitBar(t, pn, txt);
       
        }
        public void polosaUpdate(PictureBoxWithInterpolationMode pic)
        {
           
            prBar = new CircularProgressBar()
            {
                Location = NewRelPoint(350, 0),
                AutoResetColor = Color.FromArgb(200, 12, 163, 218),
                Parent = pic,
                Size = NewSizeKv(800),
                Value = 0,
                Visible = true,
                Gradient = false,
                ProgressColor1 = Color.Gold,
               // ProgressSize = new Size(50,50),
               
            };

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
            if (tmBar != null)
                tmBar.Enabled = false;
            else
            {
                tmBar = new System.Timers.Timer();
                tmBar.Elapsed += TmBar_Tick;
            }

            if (workForm.InvokeRequired)
            {
                workForm.BeginInvoke((MethodInvoker)delegate
                {
                    InitBar(t ,pn, txt);
                });
            }
            else
            {
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
                        //interval = t,
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
                prBar.BeepTime = 5;
                tmBar.Interval = t;
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
                    //((System.Timers.Timer)state).Stop();
                    //((System.Timers.Timer)state).Dispose();
                    Finish();
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
            if (ff.InvokeRequired)
            {
                ff.BeginInvoke((MethodInvoker)delegate
                {
                    Finish();
                });
            }
            else
            {
                tmBar.Enabled = false;
                //prBar.Value = prBar.Maximum;

                prBar.Value = 0;
                pcBox.Visible = false;
                ff.Visible = false;
                workForm.Invalidate();
                onPolosaEnd?.Invoke();
            }
        }
        public void PcBox_MouseUp(object sender, MouseEventArgs e)//нажатие кнопки ОК
        {
            /*tmBar.Enabled = false;
            //var reportProgress = new Action(() =>
            //{
                pcBox.Location = new Point(pcBox.Location.X - 2, pcBox.Location.Y - 2);
                //pcBox.Image = Properties.Resources.Неактивная;
                Finish();
            //});
            //workForm.BeginInvoke(reportProgress);*/
        }
        public void PcBox_MouseDown(object sender, MouseEventArgs e)//отображение нажатия кнопки
        {
            //pcBox.Image = Properties.Resources.Активная;
            //pcBox.Location = new Point(pcBox.Location.X + 2, pcBox.Location.Y + 2);

            tmBar.Enabled = false;
            Finish();
        }
        /*public void WorkForm_KeyUp(object sender, KeyEventArgs e)
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
                workForm.BeginInvoke(reportProgress); //
            }

        }

        public void WorkForm_KeyDown(object sender, KeyEventArgs e)
        {
            pcBox.Focus();
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("aasdsadas");// pcBox.Image = Properties.Resources.Активная;
            }

        }*/

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
        int waitSecondOnEnd;
        Video video;
        #endregion
        public Size size;


        public void StartRul(int cel, Rectangle rc, GeneralForm fsv, int waitSecondOnEnd=0)
        {
            this.Visible = false;
            this.rc = rc;
            flagStop =false;
            tickNumber = 0; 
            this.fsv = fsv;
            this.waitSecondOnEnd = waitSecondOnEnd;

            flag = false;

            #region//описание свойств формы
            DoubleBuffered = true;
            #endregion
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
            this.enabled = true;

            try
            {
                video = new Video(Application.StartupPath + String.Format("\\Video\\{0}.mp4", cel + 1), false);
                //size = video.Size;
                //this.Size = size;
                video.Owner = this;
                this.Size = rc.Size;
                double koeff = 1.33; // (double)Width / (double)Height;
                size = new Size((int)(this.Height * koeff), this.Height);
                //this.Size = rc.Size;
                video.Size = size;
                video.Ending += Video_Ending;
                this.Visible = true;
                this.BringToFront();
                this.Refresh();
                video.Play();
            }
            catch
            {
                WaitBeforeStopRul();
                //MessageBox.Show("Ошибка загрузки видео!\n" + Marshal.GetLastWin32Error());
            }
        }

        //точка перемещения
        Point DownPoint;
        //нажата ли кнопка мыши
        bool IsDragMode;
        protected override void OnMouseDown(MouseEventArgs mEvent)
        {
            if (!video?.Fullscreen ?? false)
            {
                DownPoint = mEvent.Location;
                IsDragMode = true;
            }
            base.OnMouseDown(mEvent);
        }

        protected override void OnMouseUp(MouseEventArgs mEvent)
        {
            IsDragMode = false;
            base.OnMouseUp(mEvent);
        }

        protected override void OnMouseMove(MouseEventArgs mEvent)
        {
            //если кнопка мыши нажата
            if (IsDragMode)
            {
                Point p = mEvent.Location;
                //вычисляем разницу в координатах между положением курсора и "нулевой" точкой рулетки
                Point dp = new Point(p.X - DownPoint.X, p.Y - DownPoint.Y);
                Location = new Point(Location.X + dp.X, Location.Y + dp.Y);
                this.Invalidate();
            }
            base.OnMouseMove(mEvent);
        }

        private void WaitBeforeStopRul()
        {
            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(waitSecondOnEnd * 1000);
                enabled = false;
                onStop?.Invoke();
            });
        }

        private void Video_Ending(object sender, EventArgs e)
        {
            WaitBeforeStopRul();
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
            //bool IsFullscreen = video?.Fullscreen ?? false;
            switch (m.Msg)
            {
                case WM_LBUTTONDBLCLICK:  //WM_MOUSE_DOUBLE_CLICK
                    if(!video?.Fullscreen ?? false)
                        video.Fullscreen = true;
                    break;
                case WM_KEYUP:
                    if (video?.Fullscreen ?? false && (int)m.WParam == VK_ESCAPE)
                        video.Fullscreen = false;
                    break;
                case WM_MOUSEMOVE:
                    //int lParam = m.LParam.ToInt32();
                    //mouseLoc.X = lParam & 0xFFFF;
                    //mouseLoc.Y = (int)(lParam & 0xFFFF0000 >> 16);
                    //Point mouseLoc = new Point();
                    //mouseLoc.X = (Int16)m.LParam;
                    //mouseLoc.Y = (Int16)((int)m.LParam >> 16);
                    //if ((int)m.WParam == 0x0001)
                        //Location = new Point((mouseLoc.X - this.Location.X), (mouseLoc.Y - this.Location.Y));
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
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Rectangle r = new Rectangle((int)((size.Width*101/768)), (int)(size.Height * 3/576), (int)(size.Width * 568/768), (int)(size.Width * 568/768));
            e.Graphics.DrawEllipse(Pens.Transparent, r);//после нужного вам результата замените - Pens.Transparent
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
                video?.Stop();
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
