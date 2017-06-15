using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Drawing.Text;

namespace _700IQ
{
    [ToolboxBitmapAttribute(typeof(CustomLabel), "EffectsLabel.bmp"),
    Description("Label with shadow and animation (transparency, rotation and zoom)")]

    public partial class CustomLabel : Control, ICustomTypeDescriptor
    {
        public SmoothingMode SmoothingMode { get; set; }
        public InterpolationMode InterpolationMode { get; set; }
        public TextRenderingHint TextRenderingHint { get; set; }
        
        public CustomLabel()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            InitializeComponent();
            DoubleBuffered = true;//Flickers a lot without this

            _CurrentAlpha = 255;
            _MinAlpha = 255;
            _MaxAlpha = 255;
            _CurrentZoom = 100;
            _MinZoom = 100;
            base.AutoSize = true;
        }

        #region Overriden inherited properties
        [Description("Text to display")]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                RecalculateSize(_Letterwise == false);

                numberOfLines = 1;//there is allways at least one line
                foreach (Char c in Text)
                    if (c == '\n')
                        numberOfLines++;
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                RecalculateSize(_Letterwise == false);
            }
        }

        public override Color ForeColor
        {
            get { return Color.FromArgb(255, base.ForeColor); }
            set { base.ForeColor = value; }
        }
        [Browsable(true), DefaultValue(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                RecalculateSize(_Letterwise == false);
            }
        }

        [Description("Enables/disables animation")]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (value)//value==true
                    RecalculateTimer();
                else
                    aTimer.Stop();
            }
        }

        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                //brushove dispozat
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region ShadowProperties
        private Point _ShadowOffset;
        [Category("Effects"), DefaultValue(typeof(Point), "0, 0"),
        Description("If differs from (0,0), creates shadow with specified offset from Left and Top")]
        public Point ShadowOffset
        {
            get { return _ShadowOffset; }
            set
            {
                _ShadowOffset = value;
                RecalculateSize(_Letterwise == false);
            }
        }

        [Category("Effects"), DefaultValue(typeof(Color), "LightGray"),
        Description("If ShadowOffset is set, this is color of the shadow")]
        public Color ShadowColor
        {
            get { return Color.FromArgb(255, BackBrush.Color); }
            set
            {
                BackBrush.Color = value;
                Refresh();
            }
        }
        #endregion

        #region Zoom
        private byte _MinZoom;
        [Category("Effects"), DefaultValue((byte)100),
        Description("Animate text zoom, in range MinZoom%-100%")]
        public byte MinZoom
        {
            get { return _MinZoom; }
            set
            {
                _MinZoom = value <= 100 ? value : (byte)100;
                if (_MinZoom == 0)
                    _MinZoom = 1;
                if (_CurrentZoom < _MinZoom)
                    _CurrentZoom = _MinZoom;
                if (_MinZoom < 100)
                {
                    aTimer.Start();
                }
                else
                    RecalculateTimer();
                Refresh();
            }
        }
        #endregion

        #region AlphaProperties
        private byte _MaxAlpha;
        [Category("Effects"), DefaultValue((byte)255),
        Description("Animate text transparency, in range MinAlpha-MaxAlpha")]
        public byte MaxAlpha
        {
            get { return _MaxAlpha; }
            set
            {
                _MaxAlpha = value;
                if (_CurrentAlpha > _MaxAlpha || _MaxAlpha <= _MinAlpha)
                    _CurrentAlpha = _MaxAlpha;
                if (_MinAlpha < _MaxAlpha)
                {
                    aTimer.Start();
                }
                else
                    RecalculateTimer();
                ForeColor = Color.FromArgb(_CurrentAlpha, ForeColor);
                BackBrush.Color = Color.FromArgb(_CurrentAlpha, BackBrush.Color);
                Refresh();
            }
        }

        private byte _MinAlpha;
        [Category("Effects"), DefaultValue((byte)255),
        Description("Animate text transparency, in range MinAlpha-MaxAlpha")]
        public byte MinAlpha
        {
            get { return _MinAlpha; }
            set
            {
                _MinAlpha = value;
                if (_CurrentAlpha < _MinAlpha || _MaxAlpha <= _MinAlpha)
                    _CurrentAlpha = _MinAlpha;
                if (_MinAlpha < _MaxAlpha)
                {
                    aTimer.Start();
                }
                else
                    RecalculateTimer();
                ForeColor = Color.FromArgb(_CurrentAlpha, ForeColor);
                BackBrush.Color = Color.FromArgb(_CurrentAlpha, BackBrush.Color);
                Refresh();
            }
        }
        #endregion

        #region RotateProperties
        private sbyte _MaxRotate;
        [Category("Effects"), DefaultValue(typeof(sbyte), "0"),
        //DefaultValue does not have overload for sbyte, so we use general type specification (otherwise, it is allways bold in VisualStudio's properties panel)
        Description("Animate text rotation, in range MinRotate°-MaxRotate°")]
        public sbyte MaxRotate
        {
            get { return _MaxRotate; }
            set
            {
                _MaxRotate = value;
                if (_CurrentRotate > _MaxRotate || _MaxRotate <= _MinRotate)
                    _CurrentRotate = _MaxRotate;
                if (_MinRotate < _MaxRotate)
                {
                    aTimer.Start();
                }
                else
                    RecalculateTimer();
                RecalculateSize(_Letterwise == false);
                Refresh();
            }
        }

        private sbyte _MinRotate;
        [Category("Effects"), DefaultValue(typeof(sbyte), "0"),
        //DefaultValue does not have overload for sbyte, so we use general type specification (otherwise, it is allways bold in VisualStudio's properties panel)
        Description("Animate text rotation, in range MinRotate°-MaxRotate°")]
        public sbyte MinRotate
        {
            get { return _MinRotate; }
            set
            {
                _MinRotate = value;
                if (_CurrentRotate < _MinRotate || _MaxRotate <= _MinRotate)
                    _CurrentRotate = _MinRotate;
                if (_MinRotate < _MaxRotate)
                {
                    aTimer.Start();
                }
                else
                    RecalculateTimer();
                RecalculateSize(_Letterwise == false);
                Refresh();
            }
        }
        #endregion

        #region Letterwise
        private bool _Letterwise = true;
        [Category("Effects"), DefaultValue(true),
        Description("Determines wheather animation should be letter by letter, or apply to whole string at once")]
        public bool Letterwise
        {
            get { return _Letterwise; }
            set
            {
                _Letterwise = value;
                RecalculateSize(true);
                Refresh();
            }
        }
        #endregion

        #region Inner workings
        private byte _CurrentAlpha;
        private sbyte _AlphaStep = 2;//change alpha by this value after each animation timer tick
        private byte _CurrentZoom;
        private sbyte _ZoomStep = 1;
        private sbyte _CurrentRotate;
        private sbyte _RotateStep = 2;
        private SolidBrush BackBrush = new SolidBrush(Color.LightGray);

        private RectangleF[] charSizes = null;
        SizeF spaceSize;
        uint numberOfLines;
        bool InPaint = false;
        public int number;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode;
            e.Graphics.InterpolationMode = InterpolationMode;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;      

            base.OnPaint(e);
            lock (this)
            {
                if (InPaint)
                    return;//control is allready in paint event
                InPaint = true;
            }
            //e.Graphics.PageUnit = GraphicsUnit.Pixel;
            if (!_Letterwise || (_CurrentRotate == 0 && !aTimer.Enabled))
            //this case executes faster, so use it when rotation is disabled
            {
                //e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; //System.Drawing.Text.TextRenderingHint.AntiAlias;

                SizeF tsize = e.Graphics.MeasureString(Text, Font);
                tsize = new SizeF(tsize.Width + ShadowOffset.X, tsize.Height + ShadowOffset.Y);

                if (_ShadowOffset != new Point(0, 0))
                {
                    e.Graphics.TranslateTransform((Size.Width + ShadowOffset.X) / 2, (Size.Height + ShadowOffset.Y) / 2);
                    if (_CurrentRotate != 0)
                        e.Graphics.RotateTransform(_CurrentRotate);
                    if (_CurrentZoom < 100)
                        e.Graphics.ScaleTransform(_CurrentZoom / 100.0f, _CurrentZoom / 100.0f);
                    e.Graphics.DrawString(Text, Font, BackBrush, -tsize.Width / 2, -tsize.Height / 2);
                    e.Graphics.Transform = new Matrix();//clear tranformation matrix
                }

                e.Graphics.TranslateTransform(Size.Width / 2, Size.Height / 2);
                if (_CurrentRotate != 0)
                    e.Graphics.RotateTransform(_CurrentRotate);
                if (_CurrentZoom < 100)
                    e.Graphics.ScaleTransform(_CurrentZoom / 100.0f, _CurrentZoom / 100.0f);
                e.Graphics.DrawString(Text, Font, new SolidBrush(base.ForeColor), -tsize.Width / 2, -tsize.Height / 2);
                //Our ForeColor does not return proper alpha (alpha=255), so use (correct) parent color 
            }
            else//_Letterwise=true
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;  //System.Drawing.Text.TextRenderingHint.AntiAlias;
                //animation is not smooth without anti-aliasing
                const float _ZeroRotationAngle = 0.1f;
                //if this is 0, text shakes if zoom is applied, but no rotation is applied

                bool shadow_exists = (_ShadowOffset != new Point(0, 0));
                Matrix empty = new Matrix();

                RectangleF posRect = new RectangleF();
                posRect.Size = e.Graphics.MeasureString(Text, Font);
                posRect.Location = new PointF((Size.Width - posRect.Width - _ShadowOffset.X) / 2, (Size.Height - posRect.Height - _ShadowOffset.Y) / 2);

                Single lineHeight = posRect.Height / numberOfLines;

                RectangleF lastChar = new RectangleF(posRect.Location, new SizeF(0, 0));
                if (shadow_exists)//first draw all shadows, and later "main" text over them
                    //this ensures that shadow never draws over normal text
                    for (int i = 0; i < Text.Length; i++)
                    {
                        e.Graphics.Transform = empty;//e.Graphics.Transform.Reset(); did not work! (i don't know why)

                        if (Text[i] == '\n')//when newline character encountered, manually compute rectangle
                            lastChar = new RectangleF(posRect.X, lastChar.Y + lineHeight, 0, lineHeight);
                        else
                        {
                            if (charSizes[i].Height == 0 || charSizes[i].Width == 0)
                                //that was a non-printable character (like space, tab, and so on)
                                //replace it with a space the size of spaceSize
                                lastChar = new RectangleF(new PointF(lastChar.X + lastChar.Width, lastChar.Y), spaceSize);
                            else
                                lastChar = new RectangleF(new PointF(lastChar.X + lastChar.Width, lastChar.Y), charSizes[i].Size);
                        }

                        e.Graphics.TranslateTransform(lastChar.X + lastChar.Width / 2 + _ShadowOffset.X, lastChar.Y + lastChar.Height / 2 + _ShadowOffset.Y);
                        if (_CurrentRotate == 0)
                            e.Graphics.RotateTransform(_ZeroRotationAngle);
                        else
                            e.Graphics.RotateTransform(_CurrentRotate);
                        e.Graphics.ScaleTransform(_CurrentZoom / 100.0f, _CurrentZoom / 100.0f);
                        e.Graphics.DrawString(Text[i].ToString(), Font, BackBrush, -lastChar.Width / 2.0f, -lastChar.Height / 2.0f, StringFormat.GenericTypographic);
                    }
                lastChar = new RectangleF(posRect.Location, new SizeF(0, 0));//reinitialize lastChar
                for (int i = 0; i < Text.Length; i++)
                {
                    e.Graphics.Transform = empty;//e.Graphics.Transform.Reset(); did not work! (i don't know why)

                    if (Text[i] == '\n')//when newline character encountered, manually compute rectangle
                        lastChar = new RectangleF(posRect.X, lastChar.Y + lineHeight, 0, lineHeight);
                    else
                    {
                        if (charSizes[i].Height == 0 || charSizes[i].Width == 0)
                            //that was a non-printable character (like space, tab, and so on)
                            //replace it with a space the size of spaceSize
                            lastChar = new RectangleF(new PointF(lastChar.X + lastChar.Width, lastChar.Y), spaceSize);
                        else
                            lastChar = new RectangleF(new PointF(lastChar.X + lastChar.Width, lastChar.Y), charSizes[i].Size);
                    }

                    e.Graphics.TranslateTransform(lastChar.X + lastChar.Width / 2, lastChar.Y + lastChar.Height / 2);
                    if (_CurrentRotate == 0)
                        e.Graphics.RotateTransform(_ZeroRotationAngle);
                    else
                        e.Graphics.RotateTransform(_CurrentRotate);
                    e.Graphics.ScaleTransform(_CurrentZoom / 100.0f, _CurrentZoom / 100.0f);
                    e.Graphics.DrawString(Text[i].ToString(), Font, new SolidBrush(base.ForeColor), -lastChar.Width / 2.0f, -lastChar.Height / 2.0f, StringFormat.GenericTypographic);
                }
            }
            lock (this)
            {
                InPaint = false;
            }
        }

        /// <summary>Recalculates size of the control</summary>
        /// <param name="MaintainCenter">Wheather position of the center should be maintained, otherwise maintains UpperLeft</param>
        private void RecalculateSize(bool MaintainCenter)
        {
            Size oldSize = Size;
            Size newSize;
            Graphics g = this.CreateGraphics();
            if (!_Letterwise)//whole string is rotated
            {
                if (!base.AutoSize)
                    return;//if autosizing is not set, no work needed
                SizeF measured_size = g.MeasureString(Text, Font);
                newSize = new Size(Convert.ToInt32(Math.Ceiling(measured_size.Width + Math.Abs(ShadowOffset.X))),//Math.Abs(ShadowOffset
                    Convert.ToInt32(Math.Ceiling(measured_size.Height + Math.Abs(ShadowOffset.Y))));

                int d = Convert.ToInt32(Math.Ceiling(Math.Sqrt(newSize.Width * newSize.Width + newSize.Height * newSize.Height)));//d=√(w²+h²)
                //size (calculated below) can be vastly improved (currently, it allmost allways oversizes control)
                if (newSize.Width == 0)
                    newSize = new Size(newSize.Height, 1);//must be non-null in order to calcualte arcus tangent
                double d_angle = Math.Atan((double)newSize.Height / newSize.Width);//angle between diagonal and x-axis
                double r_angle = Math.Max(Math.Abs(-d_angle + _MinRotate * Math.PI / 180.0), Math.Abs(d_angle + _MaxRotate * Math.PI / 180.0));//maximum rotational angle
                newSize = new Size(d, Convert.ToInt32(Math.Ceiling(d * Math.Sin(Math.Min(Math.PI / 2, r_angle + d_angle)))));
            }
            else//each character is rotated independently
            {
                SizeF W = g.MeasureString("W", Font);//single character dimensions
                SizeF measured_size = g.MeasureString(Text, Font);
                newSize = new Size(Convert.ToInt32(Math.Ceiling(measured_size.Width + Math.Abs(ShadowOffset.X) + W.Height * 0.5)),
                    Convert.ToInt32(Math.Ceiling(measured_size.Height + Math.Abs(ShadowOffset.Y) + W.Height * 0.5)));

                //calculate size of each character (which is used in OnPaint)
                //do it here once, and use it many times in OnPaint
                SizeF A_A = g.MeasureString("A A", Font, new SizeF(newSize.Width, newSize.Height), StringFormat.GenericTypographic);
                SizeF AA = g.MeasureString("AA", Font, new SizeF(newSize.Width, newSize.Height), StringFormat.GenericTypographic);
                spaceSize = new SizeF(A_A.Width - AA.Width, A_A.Height);

                charSizes = new RectangleF[Text.Length];//allocate it
                CharacterRange[] cr = new CharacterRange[1];
                StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
                Region[] r;
                cr[0] = new CharacterRange(0, 1);

                for (int i = 0; i < Text.Length; i++)
                {
                    stringFormat.SetMeasurableCharacterRanges(cr);
                    r = g.MeasureCharacterRanges(Text[i].ToString(), Font, new RectangleF(0, 0, newSize.Width, newSize.Height), stringFormat);
                    charSizes[i] = r[0].GetBounds(g);
                    r[0].Dispose();
                }

                stringFormat.Dispose();
            }
            if (base.AutoSize)
            {
                Size = newSize;
                if (MaintainCenter)
                {
                    Left += (oldSize.Width - newSize.Width) / 2;
                    Top += (oldSize.Height - newSize.Height) / 2;
                }
            }
            g.Dispose();
            Refresh();
        }

        /// <summary>Determines whether any animation is active, and starts/stops animation timer accordingly</summary>
        private void RecalculateTimer()
        {
            if (_MinAlpha < _MaxAlpha || _MinRotate < _MaxRotate || _MinZoom < 100)
                aTimer.Start();
            else
                aTimer.Stop();
        }

        private void aTimer_Tick(object sender, EventArgs e)//animation timer tick
        {
            if (_MinAlpha < _MaxAlpha)//transparency animation enabled?
            {
                if (_CurrentAlpha == _MaxAlpha)
                    _AlphaStep = (sbyte)-Math.Abs(_AlphaStep);
                if (_CurrentAlpha == _MinAlpha)
                    _AlphaStep = (sbyte)Math.Abs(_AlphaStep);
                int sum = (_CurrentAlpha + _AlphaStep);
                sum = sum < _MinAlpha ? _MinAlpha : sum;
                sum = sum > _MaxAlpha ? _MaxAlpha : sum;
                _CurrentAlpha = (byte)sum;
                ForeColor = Color.FromArgb(_CurrentAlpha, ForeColor);
                BackBrush.Color = Color.FromArgb(_CurrentAlpha, BackBrush.Color);
            }
            if (_MinRotate < _MaxRotate)//roatation enabled?
            {
                if (_CurrentRotate == _MaxRotate)
                    _RotateStep = (sbyte)-Math.Abs(_RotateStep);
                if (_CurrentRotate == _MinRotate)
                    _RotateStep = (sbyte)Math.Abs(_RotateStep);
                int sum = (_CurrentRotate + _RotateStep);
                sum = sum < _MinRotate ? _MinRotate : sum;
                sum = sum > _MaxRotate ? _MaxRotate : sum;
                _CurrentRotate = (sbyte)sum;
            }
            if (_MinZoom < 100)//zoom animation enabled?
            {
                if (_CurrentZoom == 100)
                    _ZoomStep = (sbyte)-1;
                if (_CurrentZoom == _MinZoom)
                    _ZoomStep = (sbyte)1;
                _CurrentZoom = (byte)(_CurrentZoom + _ZoomStep);
            }
            Refresh();
        }
        #endregion

        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code
        private System.Windows.Forms.Timer aTimer;

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.aTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // aTimer
            // 
            this.aTimer.Interval = 20;
            this.aTimer.Tick += new System.EventHandler(this.aTimer_Tick);
            this.ResumeLayout(false);
        }
        #endregion

        #region Getting rid of senseless properties (bobpowell.net)
        private string[] NamesToRemove =
        {
            "AccesibilityObject",
            "AccessibleDescription",
            "AccessibleName",
            "AllowDrop",
            "Capture",
            "DisplayRectangle",
            "RightToLeft",
            "Region",
            "Tag",
            "UseWaitCursor",
            "ImeMode",
            "Padding"
        };

        //Does the property filtering...
        private PropertyDescriptorCollection
        FilterProperties(PropertyDescriptorCollection pdc)
        {
            ArrayList toRemove = new ArrayList();
            foreach (string s in NamesToRemove)
                toRemove.Add(s);

            PropertyDescriptorCollection adjustedProps = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
            foreach (PropertyDescriptor pd in pdc)
                if (!toRemove.Contains(pd.Name))
                    adjustedProps.Add(pd);

            return adjustedProps;
        }

        #region ICustomTypeDescriptor Members
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }


        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this, attributes, true);
            return FilterProperties(pdc);
        }

        PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this, true);
            return FilterProperties(pdc);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        #endregion

        #endregion
    }
}
