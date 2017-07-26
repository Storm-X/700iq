using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _700IQ
{
    public class CircularProgressBar : Control
    {

        #region Enums

        public enum _ProgressShape
        {
            Round,
            Flat
        }

        #endregion
        #region Variables

        private int _Value;
        private int _Maximum = 100;
        private Color _ProgressColor1 = Color.Green;
        private Color _ProgressColor2 = Color.Red;
        private Color _AutoResetColor = Color.Blue;
        private _ProgressShape ProgressShapeVal;
        private int progress_size = 14;
        private bool gradient = false;
        private bool autoReset = false;
        private bool defaul_color = true;
        public int interval = 100;
        int begin;
        int interval_cels;
        int beeps_count;

        #endregion
        #region Custom Properties

        public int Value
        {
            get { return _Value; }
            set
            {
                if (value >= _Maximum)
                    value = (autoReset) ? 0 : _Maximum;
                _Value = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                if (value < 1)
                    value = 1;
                _Maximum = value;
                Invalidate();
            }
        }

        public Color ProgressColor1
        {
            get { return _ProgressColor1; }
            set
            {
                _ProgressColor1 = value;
                defaul_color = ((_ProgressColor1 == Color.Green) && (_ProgressColor2 == Color.Red)) ? true : false;
                Invalidate();
            }
        }

        public Color ProgressColor2
        {
            get { return _ProgressColor2; }
            set
            {
                _ProgressColor2 = value;
                defaul_color = ((_ProgressColor1 == Color.Green) && (_ProgressColor2 == Color.Red)) ? true : false;
                Invalidate();
            }
        }

        public Color AutoResetColor
        {
            get { return _AutoResetColor; }
            set
            {
                _AutoResetColor = value;
                Invalidate();
            }
        }

        public _ProgressShape ProgressShape
        {
            get { return ProgressShapeVal; }
            set
            {
                ProgressShapeVal = value;
                Invalidate();
            }
        }

        public int ProgressSize
        {
            get { return progress_size; }
            set
            {
                progress_size = value;
                Invalidate();
            }
        }
        public bool Gradient
        {
            get { return gradient; }
            set
            {
                gradient = value;
                Invalidate();
            }
        }
        public bool AutoReset
        {
            get { return autoReset; }
            set
            {
                autoReset = value;
                Invalidate();
            }
        }

        #endregion
        #region EventArgs

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetStandardSize();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
        }

        protected override void OnPaintBackground(PaintEventArgs p)
        {
            base.OnPaintBackground(p);
        }

        #endregion

        public CircularProgressBar()
        {
            Size = new Size(130, 130);
            Font = new Font("Segoe UI", 15);
            MinimumSize = new Size(100, 100);
            DoubleBuffered = true;
            this.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.Color.Transparent;

        }

        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }
        public void SetInterval(int t)
        {
            this.interval = t;
            this.begin = _Maximum - 5000 / interval;
            this.interval_cels = 1000 / interval;
            this.beeps_count = 0;
        }

        public void Increment(int Val)
        {
            this._Value += Val;
            Invalidate();
        }

        public void Decrement(int Val)
        {
            this._Value -= Val;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            /*
            this.begin = _Maximum - 5000 / interval;
            this.interval_cels = 1000 / interval;
            this.beeps_count = 0;
            */
            if (defaul_color) 
            {
                //if (_Value <= _Maximum / 2) _ProgressColor1 = Color.FromArgb(150, _Value * 2 * 255 / _Maximum, 255, 0);
                //else _ProgressColor1 = Color.FromArgb(150, 255, 255 - (_Value * 2 - _Maximum) * 255 / _Maximum, 0);
                Double curVal = (2 * (double)_Value / _Maximum) - 1;
                //Double Red = 255 * (1 + Math.Min(curVal, 0));
                //Double Green = 255 * (1 - Math.Max(curVal, 0));
                _ProgressColor1 = Color.FromArgb(200, (byte)(255 * (1 + Math.Min(curVal, 0))), (byte)(255 * (1 - Math.Max(curVal, 0))), 0);
            }
            using (Bitmap bitmap = new Bitmap(this.Width, this.Height)) // BackgroundImage))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(this.BackColor);
                    LinearGradientBrush brush;
                    if (!autoReset) brush = new LinearGradientBrush(this.ClientRectangle, this._ProgressColor1, (gradient) ? this._ProgressColor2 : this._ProgressColor1, LinearGradientMode.ForwardDiagonal);
                    else brush = new LinearGradientBrush(this.ClientRectangle, AutoResetColor, AutoResetColor, LinearGradientMode.ForwardDiagonal);
                    using (brush)
                    {
                        using (Pen pen = new Pen(brush, progress_size))
                        {
                            switch (this.ProgressShapeVal)
                            {
                                case _ProgressShape.Round:
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    break;

                                case _ProgressShape.Flat:
                                    pen.StartCap = LineCap.Flat;
                                    pen.EndCap = LineCap.Flat;
                                    break;
                            }
                            if (autoReset) graphics.DrawArc(pen, progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))), 120);
                           // else graphics.DrawArc(pen, progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))));
                            else
                            {
                                if (_Value >  begin)
                                {
                                    if ((_Value - begin) % interval_cels == 1)
                                    {
                                        Console.Beep(800, (beeps_count < 4) ? 250 : 1000 );
                                        beeps_count++;
                                        //if (_Value - begin % interval_cels > (interval_cels / 2))
                                           // graphics.DrawArc(pen, progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))));
                                    }
                                    //if (_Value % (_Maximum / 15) > (_Maximum / 30) ) graphics.DrawArc(pen, progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))));
                                    //if ((_Value % 30 == 1) && _Value < 300 ) Console.Beep(800,250);
                                    //else if (_Value % 300 == 1) Console.Beep(800, 1000);

                                    //else graphics.DrawArc(new Pen(Color.Transparent, progress_size), progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))));
                                }
                                 graphics.DrawArc(pen, progress_size / 2 + 1, progress_size / 2 + 1, (this.Width - progress_size) - 2, (this.Height - progress_size) - 2, -90, (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * (this._Maximum - this._Value))));
                            }

                        }
                    }
                    /*
                    using (LinearGradientBrush brush2 = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(0x34, 0x34, 0x34), Color.FromArgb(0x34, 0x34, 0x34), LinearGradientMode.Vertical))
                    {
                        graphics.FillEllipse(brush2, progress_size + 2, progress_size + 2, (this.Width - (progress_size + 2) * 2), (this.Height - (progress_size + 2) * 2));
                    }*/
                    //SizeF MS = graphics.MeasureString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font);
                    //graphics.DrawString(Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value)), Font, Brushes.White, Convert.ToInt32(Width / 2 - MS.Width / 2), Convert.ToInt32(Height / 2 - MS.Height / 2));
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    graphics.Dispose();
                    bitmap.Dispose();
                }
            }
        }
    }
}
