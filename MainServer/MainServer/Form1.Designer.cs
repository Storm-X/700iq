﻿namespace MainServer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lot = new System.Windows.Forms.Button();
            this.DBLink = new System.Windows.Forms.Button();
            this.ButtonReg = new System.Windows.Forms.Button();
            this.butEndReg = new System.Windows.Forms.Button();
            this.GameButton = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Anons = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gameStopBut = new System.Windows.Forms.Button();
            this.infoGame = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Home = new System.Windows.Forms.TabPage();
            this.ListGamesView = new DevExpress.XtraGrid.GridControl();
            this.ListGamesGrid = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.idColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cityColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.nameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gameidColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tour_idColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.game_nameColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.placeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.startTimeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ListKomand = new System.Windows.Forms.DataGridView();
            this.Control = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.questEditor = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tbMediaFile = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.questEditorGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qeFileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gameStatistics = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button7 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.Home.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListGamesView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListGamesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListKomand)).BeginInit();
            this.Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.questEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.questEditorGrid)).BeginInit();
            this.gameStatistics.SuspendLayout();
            this.SuspendLayout();
            // 
            // lot
            // 
            this.lot.Enabled = false;
            this.lot.Location = new System.Drawing.Point(20, 229);
            this.lot.Margin = new System.Windows.Forms.Padding(2);
            this.lot.Name = "lot";
            this.lot.Size = new System.Drawing.Size(135, 24);
            this.lot.TabIndex = 5;
            this.lot.Text = "Жеребьевка";
            this.lot.UseVisualStyleBackColor = true;
            this.lot.Click += new System.EventHandler(this.lot_Click);
            // 
            // DBLink
            // 
            this.DBLink.Location = new System.Drawing.Point(20, 72);
            this.DBLink.Margin = new System.Windows.Forms.Padding(2);
            this.DBLink.Name = "DBLink";
            this.DBLink.Size = new System.Drawing.Size(135, 24);
            this.DBLink.TabIndex = 1;
            this.DBLink.Text = "Подключение к БД";
            this.DBLink.UseVisualStyleBackColor = true;
            this.DBLink.Click += new System.EventHandler(this.DBLink_Click);
            // 
            // ButtonReg
            // 
            this.ButtonReg.Enabled = false;
            this.ButtonReg.Location = new System.Drawing.Point(20, 150);
            this.ButtonReg.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonReg.Name = "ButtonReg";
            this.ButtonReg.Size = new System.Drawing.Size(135, 24);
            this.ButtonReg.TabIndex = 3;
            this.ButtonReg.Text = "Начать регистрацию";
            this.ButtonReg.UseVisualStyleBackColor = true;
            this.ButtonReg.Click += new System.EventHandler(this.Registration_Click);
            // 
            // butEndReg
            // 
            this.butEndReg.Enabled = false;
            this.butEndReg.Location = new System.Drawing.Point(20, 191);
            this.butEndReg.Margin = new System.Windows.Forms.Padding(2);
            this.butEndReg.Name = "butEndReg";
            this.butEndReg.Size = new System.Drawing.Size(135, 24);
            this.butEndReg.TabIndex = 4;
            this.butEndReg.Text = "Закончить регистрацию";
            this.butEndReg.UseVisualStyleBackColor = true;
            this.butEndReg.Click += new System.EventHandler(this.butEndReg_Click);
            // 
            // GameButton
            // 
            this.GameButton.Enabled = false;
            this.GameButton.Location = new System.Drawing.Point(20, 112);
            this.GameButton.Margin = new System.Windows.Forms.Padding(2);
            this.GameButton.Name = "GameButton";
            this.GameButton.Size = new System.Drawing.Size(135, 24);
            this.GameButton.TabIndex = 2;
            this.GameButton.Text = "Выбор игры";
            this.GameButton.UseVisualStyleBackColor = true;
            this.GameButton.Click += new System.EventHandler(this.Game_Click);
            // 
            // start
            // 
            this.start.Enabled = false;
            this.start.Location = new System.Drawing.Point(20, 310);
            this.start.Margin = new System.Windows.Forms.Padding(2);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(135, 24);
            this.start.TabIndex = 7;
            this.start.Text = "Начать игру";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.Start);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1508, 34);
            this.label1.TabIndex = 11;
            this.label1.Text = "Интеллект-казино 700  IQ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Anons
            // 
            this.Anons.Enabled = false;
            this.Anons.Location = new System.Drawing.Point(20, 269);
            this.Anons.Margin = new System.Windows.Forms.Padding(2);
            this.Anons.Name = "Anons";
            this.Anons.Size = new System.Drawing.Size(135, 24);
            this.Anons.TabIndex = 6;
            this.Anons.Text = "Оповещение команд";
            this.Anons.UseVisualStyleBackColor = true;
            this.Anons.Click += new System.EventHandler(this.Anons_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(20, 519);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "Монитор с командами";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gameStopBut
            // 
            this.gameStopBut.Enabled = false;
            this.gameStopBut.Location = new System.Drawing.Point(20, 350);
            this.gameStopBut.Margin = new System.Windows.Forms.Padding(2);
            this.gameStopBut.Name = "gameStopBut";
            this.gameStopBut.Size = new System.Drawing.Size(135, 24);
            this.gameStopBut.TabIndex = 18;
            this.gameStopBut.Text = "Приостановка игры";
            this.gameStopBut.UseVisualStyleBackColor = true;
            this.gameStopBut.Click += new System.EventHandler(this.gameStopBut_Click);
            // 
            // infoGame
            // 
            this.infoGame.AutoSize = true;
            this.infoGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoGame.Location = new System.Drawing.Point(360, 34);
            this.infoGame.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.infoGame.Name = "infoGame";
            this.infoGame.Size = new System.Drawing.Size(153, 18);
            this.infoGame.TabIndex = 12;
            this.infoGame.Text = "информация об игре";
            this.infoGame.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.infoGame.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Home);
            this.tabControl1.Controls.Add(this.Control);
            this.tabControl1.Controls.Add(this.questEditor);
            this.tabControl1.Controls.Add(this.gameStatistics);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.ItemSize = new System.Drawing.Size(256, 28);
            this.tabControl1.Location = new System.Drawing.Point(167, 59);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1035, 533);
            this.tabControl1.TabIndex = 19;
            this.tabControl1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tabControl1_KeyUp);
            // 
            // Home
            // 
            this.Home.Controls.Add(this.ListGamesView);
            this.Home.Controls.Add(this.ListKomand);
            this.Home.Location = new System.Drawing.Point(4, 32);
            this.Home.Name = "Home";
            this.Home.Padding = new System.Windows.Forms.Padding(3);
            this.Home.Size = new System.Drawing.Size(1027, 497);
            this.Home.TabIndex = 0;
            this.Home.Text = "Зарегистрировавшиеся команды";
            this.Home.UseVisualStyleBackColor = true;
            // 
            // ListGamesView
            // 
            this.ListGamesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListGamesView.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.ListGamesView.Location = new System.Drawing.Point(3, 3);
            this.ListGamesView.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.ListGamesView.MainView = this.ListGamesGrid;
            this.ListGamesView.Name = "ListGamesView";
            this.ListGamesView.Size = new System.Drawing.Size(1021, 491);
            this.ListGamesView.TabIndex = 12;
            this.ListGamesView.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ListGamesGrid});
            // 
            // ListGamesGrid
            // 
            this.ListGamesGrid.ActiveFilterString = "[city] = \'Орша\'";
            this.ListGamesGrid.Appearance.EvenRow.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.EvenRow.Options.UseFont = true;
            this.ListGamesGrid.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ListGamesGrid.Appearance.FocusedRow.Options.UseBackColor = true;
            this.ListGamesGrid.Appearance.GroupButton.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.ListGamesGrid.Appearance.GroupButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.ListGamesGrid.Appearance.GroupButton.Options.UseFont = true;
            this.ListGamesGrid.Appearance.GroupPanel.Image = ((System.Drawing.Image)(resources.GetObject("ListGamesGrid.Appearance.GroupPanel.Image")));
            this.ListGamesGrid.Appearance.GroupPanel.Options.UseImage = true;
            this.ListGamesGrid.Appearance.GroupRow.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ListGamesGrid.Appearance.GroupRow.BackColor2 = System.Drawing.Color.LightGreen;
            this.ListGamesGrid.Appearance.GroupRow.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ListGamesGrid.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.ListGamesGrid.Appearance.GroupRow.Options.UseBackColor = true;
            this.ListGamesGrid.Appearance.GroupRow.Options.UseFont = true;
            this.ListGamesGrid.Appearance.GroupRow.Options.UseForeColor = true;
            this.ListGamesGrid.Appearance.HeaderPanel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.ListGamesGrid.Appearance.HeaderPanel.Options.UseFont = true;
            this.ListGamesGrid.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.ListGamesGrid.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ListGamesGrid.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ListGamesGrid.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.ListGamesGrid.Appearance.HorzLine.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.HorzLine.Options.UseFont = true;
            this.ListGamesGrid.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ListGamesGrid.Appearance.OddRow.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.OddRow.Options.UseBackColor = true;
            this.ListGamesGrid.Appearance.OddRow.Options.UseFont = true;
            this.ListGamesGrid.Appearance.Row.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.Row.Options.UseFont = true;
            this.ListGamesGrid.Appearance.RowSeparator.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.RowSeparator.Options.UseFont = true;
            this.ListGamesGrid.Appearance.VertLine.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Italic);
            this.ListGamesGrid.Appearance.VertLine.Options.UseFont = true;
            this.ListGamesGrid.Appearance.ViewCaption.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.ListGamesGrid.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Navy;
            this.ListGamesGrid.Appearance.ViewCaption.Options.UseFont = true;
            this.ListGamesGrid.Appearance.ViewCaption.Options.UseForeColor = true;
            this.ListGamesGrid.Appearance.ViewCaption.Options.UseImage = true;
            this.ListGamesGrid.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.idColumn,
            this.cityColumn,
            this.nameColumn,
            this.gameidColumn,
            this.tour_idColumn,
            this.game_nameColumn,
            this.placeColumn,
            this.dateColumn,
            this.startTimeColumn});
            this.ListGamesGrid.DetailTabHeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.ListGamesGrid.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.ListGamesGrid.GridControl = this.ListGamesView;
            this.ListGamesGrid.GroupCount = 2;
            this.ListGamesGrid.GroupFormat = "[#image]{1} {2}";
            this.ListGamesGrid.Name = "ListGamesGrid";
            this.ListGamesGrid.OptionsBehavior.Editable = false;
            this.ListGamesGrid.OptionsLayout.Columns.StoreAllOptions = true;
            this.ListGamesGrid.OptionsLayout.Columns.StoreAppearance = true;
            this.ListGamesGrid.OptionsLayout.StoreAllOptions = true;
            this.ListGamesGrid.OptionsLayout.StoreAppearance = true;
            this.ListGamesGrid.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.ListGamesGrid.OptionsView.EnableAppearanceEvenRow = true;
            this.ListGamesGrid.OptionsView.EnableAppearanceOddRow = true;
            this.ListGamesGrid.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office;
            this.ListGamesGrid.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            this.ListGamesGrid.OptionsView.RowAutoHeight = true;
            this.ListGamesGrid.OptionsView.ShowChildrenInGroupPanel = true;
            this.ListGamesGrid.OptionsView.ShowGroupPanel = false;
            this.ListGamesGrid.OptionsView.ShowIndicator = false;
            this.ListGamesGrid.OptionsView.ShowViewCaption = true;
            this.ListGamesGrid.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Indicator;
            this.ListGamesGrid.PaintStyleName = "Skin";
            this.ListGamesGrid.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.cityColumn, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.nameColumn, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.ListGamesGrid.ViewCaption = "Список турниров 700IQ";
            this.ListGamesGrid.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.ListGamesGrid_PopupMenuShowing);
            this.ListGamesGrid.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.ListGamesGrid.DoubleClick += new System.EventHandler(this.ListGamesGrid_DoubleClick);
            // 
            // idColumn
            // 
            this.idColumn.Caption = "ID турнира";
            this.idColumn.FieldName = "id";
            this.idColumn.Name = "idColumn";
            // 
            // cityColumn
            // 
            this.cityColumn.Caption = "Город";
            this.cityColumn.FieldName = "city";
            this.cityColumn.Name = "cityColumn";
            // 
            // nameColumn
            // 
            this.nameColumn.Caption = "Название турнира";
            this.nameColumn.FieldName = "name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.Visible = true;
            this.nameColumn.VisibleIndex = 0;
            // 
            // gameidColumn
            // 
            this.gameidColumn.Caption = "ID игры";
            this.gameidColumn.FieldName = "gameid";
            this.gameidColumn.Name = "gameidColumn";
            // 
            // tour_idColumn
            // 
            this.tour_idColumn.Caption = "ID тура";
            this.tour_idColumn.FieldName = "tour_id";
            this.tour_idColumn.Name = "tour_idColumn";
            // 
            // game_nameColumn
            // 
            this.game_nameColumn.Caption = "Название тура";
            this.game_nameColumn.FieldName = "game_name";
            this.game_nameColumn.Name = "game_nameColumn";
            this.game_nameColumn.Visible = true;
            this.game_nameColumn.VisibleIndex = 0;
            this.game_nameColumn.Width = 385;
            // 
            // placeColumn
            // 
            this.placeColumn.AppearanceCell.Options.UseTextOptions = true;
            this.placeColumn.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.None;
            this.placeColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.placeColumn.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.placeColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.placeColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.placeColumn.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.placeColumn.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.placeColumn.Caption = "Место проведения игр";
            this.placeColumn.FieldName = "place";
            this.placeColumn.Name = "placeColumn";
            this.placeColumn.Visible = true;
            this.placeColumn.VisibleIndex = 1;
            this.placeColumn.Width = 426;
            // 
            // dateColumn
            // 
            this.dateColumn.Caption = "Дата проведения";
            this.dateColumn.DisplayFormat.FormatString = "D";
            this.dateColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateColumn.FieldName = "date";
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.Visible = true;
            this.dateColumn.VisibleIndex = 2;
            this.dateColumn.Width = 126;
            // 
            // startTimeColumn
            // 
            this.startTimeColumn.Caption = "Время начала";
            this.startTimeColumn.FieldName = "startTime";
            this.startTimeColumn.Name = "startTimeColumn";
            this.startTimeColumn.Visible = true;
            this.startTimeColumn.VisibleIndex = 3;
            this.startTimeColumn.Width = 82;
            // 
            // ListKomand
            // 
            this.ListKomand.AllowDrop = true;
            this.ListKomand.AllowUserToAddRows = false;
            this.ListKomand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ListKomand.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.ListKomand.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.ListKomand.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.ListKomand.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ListKomand.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ListKomand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Orange;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ListKomand.DefaultCellStyle = dataGridViewCellStyle2;
            this.ListKomand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListKomand.Location = new System.Drawing.Point(3, 3);
            this.ListKomand.Margin = new System.Windows.Forms.Padding(2);
            this.ListKomand.MultiSelect = false;
            this.ListKomand.Name = "ListKomand";
            this.ListKomand.RowHeadersVisible = false;
            this.ListKomand.RowTemplate.Height = 24;
            this.ListKomand.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ListKomand.Size = new System.Drawing.Size(1021, 491);
            this.ListKomand.TabIndex = 4;
            this.ListKomand.VirtualMode = true;
            this.ListKomand.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ListKomand_CellContentClick);
            this.ListKomand.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.ListKomand.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.ListKomand_RowPrePaint);
            this.ListKomand.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.ListKomand_UserDeletingRow);
            this.ListKomand.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListKomand_DragDrop);
            this.ListKomand.DragOver += new System.Windows.Forms.DragEventHandler(this.ListKomand_DragOver);
            this.ListKomand.Leave += new System.EventHandler(this.ListKomand_Leave);
            this.ListKomand.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListKomand_MouseDown);
            this.ListKomand.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListKomand_MouseMove);
            // 
            // Control
            // 
            this.Control.Controls.Add(this.dataGridView2);
            this.Control.Location = new System.Drawing.Point(4, 32);
            this.Control.Name = "Control";
            this.Control.Padding = new System.Windows.Forms.Padding(3);
            this.Control.Size = new System.Drawing.Size(1027, 497);
            this.Control.TabIndex = 1;
            this.Control.Text = "Контроль ответов";
            this.Control.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView2.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1021, 491);
            this.dataGridView2.TabIndex = 10;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            this.dataGridView2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellValueChanged);
            this.dataGridView2.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView2_CurrentCellDirtyStateChanged);
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Zona";
            this.Column2.FillWeight = 34.46503F;
            this.Column2.HeaderText = "Зона";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Theme";
            this.Column5.FillWeight = 50.16106F;
            this.Column5.HeaderText = "Тема";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Vopros";
            this.Column6.FillWeight = 240.606F;
            this.Column6.HeaderText = "Вопрос";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "Otvet";
            this.Column7.FillWeight = 143.7915F;
            this.Column7.HeaderText = "Ответ";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "KomOtvet";
            this.Column8.FillWeight = 124.3689F;
            this.Column8.HeaderText = "Ответ команда";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // questEditor
            // 
            this.questEditor.Controls.Add(this.textBox2);
            this.questEditor.Controls.Add(this.tbMediaFile);
            this.questEditor.Controls.Add(this.button5);
            this.questEditor.Controls.Add(this.label2);
            this.questEditor.Controls.Add(this.button4);
            this.questEditor.Controls.Add(this.button3);
            this.questEditor.Controls.Add(this.textBox6);
            this.questEditor.Controls.Add(this.textBox5);
            this.questEditor.Controls.Add(this.textBox4);
            this.questEditor.Controls.Add(this.button2);
            this.questEditor.Controls.Add(this.comboBox1);
            this.questEditor.Controls.Add(this.questEditorGrid);
            this.questEditor.Location = new System.Drawing.Point(4, 32);
            this.questEditor.Name = "questEditor";
            this.questEditor.Padding = new System.Windows.Forms.Padding(3);
            this.questEditor.Size = new System.Drawing.Size(1027, 497);
            this.questEditor.TabIndex = 2;
            this.questEditor.Text = "Редактор вопросов";
            this.questEditor.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(774, 464);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 23);
            this.textBox2.TabIndex = 11;
            // 
            // tbMediaFile
            // 
            this.tbMediaFile.Location = new System.Drawing.Point(774, 428);
            this.tbMediaFile.Name = "tbMediaFile";
            this.tbMediaFile.Size = new System.Drawing.Size(145, 23);
            this.tbMediaFile.TabIndex = 10;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1039, 429);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(82, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Удалить";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 429);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 8;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(926, 428);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 30);
            this.button4.TabIndex = 7;
            this.button4.Text = "Сохранить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(926, 457);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 30);
            this.button3.TabIndex = 6;
            this.button3.Text = "Очистить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(623, 429);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(145, 58);
            this.textBox6.TabIndex = 5;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(45, 429);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(572, 58);
            this.textBox5.TabIndex = 4;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(259, 1);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(284, 23);
            this.textBox4.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(549, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(152, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Добавить тему";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(253, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // questEditorGrid
            // 
            this.questEditorGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.questEditorGrid.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.questEditorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.questEditorGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.qeFileColumn});
            this.questEditorGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.questEditorGrid.Location = new System.Drawing.Point(0, 30);
            this.questEditorGrid.MultiSelect = false;
            this.questEditorGrid.Name = "questEditorGrid";
            this.questEditorGrid.RowHeadersVisible = false;
            this.questEditorGrid.RowHeadersWidth = 25;
            this.questEditorGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.questEditorGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.questEditorGrid.Size = new System.Drawing.Size(1021, 393);
            this.questEditorGrid.TabIndex = 0;
            this.questEditorGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.questEditorGrid_CellMouseDoubleClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.DataPropertyName = "ID";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            this.Column1.Width = 35;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.DataPropertyName = "text";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column3.HeaderText = "Текст вопроса";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 700;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.DataPropertyName = "answer";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column4.HeaderText = "Ответ";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 170;
            // 
            // qeFileColumn
            // 
            this.qeFileColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.qeFileColumn.DataPropertyName = "media";
            this.qeFileColumn.HeaderText = "Файл";
            this.qeFileColumn.Name = "qeFileColumn";
            this.qeFileColumn.ReadOnly = true;
            // 
            // gameStatistics
            // 
            this.gameStatistics.Controls.Add(this.label9);
            this.gameStatistics.Controls.Add(this.richTextBox4);
            this.gameStatistics.Controls.Add(this.richTextBox3);
            this.gameStatistics.Controls.Add(this.richTextBox2);
            this.gameStatistics.Controls.Add(this.richTextBox1);
            this.gameStatistics.Controls.Add(this.label6);
            this.gameStatistics.Controls.Add(this.label5);
            this.gameStatistics.Controls.Add(this.label4);
            this.gameStatistics.Controls.Add(this.comboBox4);
            this.gameStatistics.Controls.Add(this.comboBox3);
            this.gameStatistics.Controls.Add(this.comboBox2);
            this.gameStatistics.Location = new System.Drawing.Point(4, 32);
            this.gameStatistics.Name = "gameStatistics";
            this.gameStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.gameStatistics.Size = new System.Drawing.Size(1027, 497);
            this.gameStatistics.TabIndex = 3;
            this.gameStatistics.Text = "Игровая статистика";
            this.gameStatistics.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(195, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 14;
            // 
            // richTextBox4
            // 
            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox4.Location = new System.Drawing.Point(23, 49);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(939, 75);
            this.richTextBox4.TabIndex = 13;
            this.richTextBox4.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox3.Location = new System.Drawing.Point(782, 183);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(342, 272);
            this.richTextBox3.TabIndex = 12;
            this.richTextBox3.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Location = new System.Drawing.Point(394, 187);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(342, 262);
            this.richTextBox2.TabIndex = 11;
            this.richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(3, 187);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(342, 262);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(804, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Команда 3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(450, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Команда 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Команда 1";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(760, 6);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(60, 24);
            this.comboBox4.TabIndex = 5;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(223, 6);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(472, 24);
            this.comboBox3.TabIndex = 4;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(701, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(53, 24);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1208, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(262, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Игровая зона    Номер iCon    Действия с тройкой";
            this.label3.Visible = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(198, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(157, 23);
            this.button7.TabIndex = 22;
            this.button7.Text = "Сменить пользователя";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1508, 615);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gameStopBut);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Anons);
            this.Controls.Add(this.infoGame);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.start);
            this.Controls.Add(this.GameButton);
            this.Controls.Add(this.butEndReg);
            this.Controls.Add(this.ButtonReg);
            this.Controls.Add(this.DBLink);
            this.Controls.Add(this.lot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Интеллект-казино 700 IQ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.Home.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ListGamesView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListGamesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListKomand)).EndInit();
            this.Control.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.questEditor.ResumeLayout(false);
            this.questEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.questEditorGrid)).EndInit();
            this.gameStatistics.ResumeLayout(false);
            this.gameStatistics.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button lot;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Button DBLink;
        private System.Windows.Forms.Button ButtonReg;
        private System.Windows.Forms.Button butEndReg;
        private System.Windows.Forms.Button GameButton;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Anons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button gameStopBut;
        private System.Windows.Forms.Label infoGame;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Home;
        private System.Windows.Forms.TabPage Control;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView ListKomand;
        private System.Windows.Forms.TabPage questEditor;
        private System.Windows.Forms.DataGridView questEditorGrid;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TabPage gameStatistics;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox tbMediaFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn qeFileColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraGrid.GridControl ListGamesView;
        private DevExpress.XtraGrid.Views.Grid.GridView ListGamesGrid;
        private DevExpress.XtraGrid.Columns.GridColumn idColumn;
        private DevExpress.XtraGrid.Columns.GridColumn nameColumn;
        private DevExpress.XtraGrid.Columns.GridColumn gameidColumn;
        private DevExpress.XtraGrid.Columns.GridColumn tour_idColumn;
        private DevExpress.XtraGrid.Columns.GridColumn game_nameColumn;
        private DevExpress.XtraGrid.Columns.GridColumn placeColumn;
        private DevExpress.XtraGrid.Columns.GridColumn dateColumn;
        private DevExpress.XtraGrid.Columns.GridColumn startTimeColumn;
        private DevExpress.XtraGrid.Columns.GridColumn cityColumn;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label9;
    }
}

