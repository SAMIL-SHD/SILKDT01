namespace DUTY1000
{
    partial class duty8021
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(duty8021));
            this.btn_exit = new SilkRoad.UserControls.SRButton();
            this.srPanel1 = new SilkRoad.UserControls.SRPanel();
            this.srTitle1 = new SilkRoad.UserControls.SRTitle();
            this.srPanel3 = new SilkRoad.UserControls.SRPanel();
            this.btn_save = new SilkRoad.UserControls.SRButton();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gr_detail = new SilkRoad.UserControls.SRGroupBox();
            this.srLabel13 = new SilkRoad.UserControls.SRLabel();
            this.cmb_gubn = new SilkRoad.UserControls.SRCombo();
            this.srLabel9 = new SilkRoad.UserControls.SRLabel();
            this.sl_gnmu2 = new SilkRoad.UserControls.SRLookup2();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.srLabel5 = new SilkRoad.UserControls.SRLabel();
            this.dat_ycdt2 = new DevExpress.XtraEditors.DateEdit();
            this.dat_ycdt = new DevExpress.XtraEditors.DateEdit();
            this.sl_gnmu = new SilkRoad.UserControls.SRLookup2();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn64 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn65 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn66 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sl_embs = new SilkRoad.UserControls.SRLookup2();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.srLabel7 = new SilkRoad.UserControls.SRLabel();
            this.srLabel2 = new SilkRoad.UserControls.SRLabel();
            this.srLabel1 = new SilkRoad.UserControls.SRLabel();
            this.dat_year = new SilkRoad.UserControls.SRDate();
            this.srLabel3 = new SilkRoad.UserControls.SRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.srPanel1)).BeginInit();
            this.srPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.srPanel3)).BeginInit();
            this.srPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gr_detail)).BeginInit();
            this.gr_detail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_gubn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_gnmu2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_gnmu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_embs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_year.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_year.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exit.Authority = false;
            this.btn_exit.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btn_exit.Image = ((System.Drawing.Image)(resources.GetObject("btn_exit.Image")));
            this.btn_exit.Location = new System.Drawing.Point(73, 9);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(60, 24);
            this.btn_exit.SRAuthCrud = SilkRoad.UserControls.SRButton.AuthCrudType.NONE;
            this.btn_exit.SRKindOf = SilkRoad.UserControls.SRButton.ButtonKindOfType.종료;
            this.btn_exit.SRWidthType = SilkRoad.UserControls.SRButton.WidthType.WIDTH_60;
            this.btn_exit.TabIndex = 6;
            this.btn_exit.Text = "종료";
            this.btn_exit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_exit.UseVisualStyleBackColor = true;
            // 
            // srPanel1
            // 
            this.srPanel1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.srPanel1.Appearance.Options.UseBackColor = true;
            this.srPanel1.Controls.Add(this.srTitle1);
            this.srPanel1.Controls.Add(this.srPanel3);
            this.srPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.srPanel1.Location = new System.Drawing.Point(0, 0);
            this.srPanel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.srPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.srPanel1.Name = "srPanel1";
            this.srPanel1.Size = new System.Drawing.Size(398, 46);
            this.srPanel1.TabIndex = 0;
            // 
            // srTitle1
            // 
            this.srTitle1.BackColor = System.Drawing.SystemColors.Control;
            this.srTitle1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.srTitle1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srTitle1.Location = new System.Drawing.Point(2, 2);
            this.srTitle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.srTitle1.Name = "srTitle1";
            this.srTitle1.Size = new System.Drawing.Size(182, 39);
            this.srTitle1.SRTitleTxt = "연차기준년도수정";
            this.srTitle1.TabIndex = 26;
            this.srTitle1.TabStop = false;
            // 
            // srPanel3
            // 
            this.srPanel3.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.srPanel3.Appearance.Options.UseBackColor = true;
            this.srPanel3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.srPanel3.Controls.Add(this.btn_save);
            this.srPanel3.Controls.Add(this.btn_exit);
            this.srPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.srPanel3.Location = new System.Drawing.Point(255, 3);
            this.srPanel3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.srPanel3.LookAndFeel.UseDefaultLookAndFeel = false;
            this.srPanel3.Name = "srPanel3";
            this.srPanel3.Size = new System.Drawing.Size(140, 40);
            this.srPanel3.TabIndex = 0;
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_save.Authority = false;
            this.btn_save.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.Location = new System.Drawing.Point(12, 9);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(60, 24);
            this.btn_save.SRAuthCrud = SilkRoad.UserControls.SRButton.AuthCrudType.U;
            this.btn_save.SRKindOf = SilkRoad.UserControls.SRButton.ButtonKindOfType.수정;
            this.btn_save.SRWidthType = SilkRoad.UserControls.SRButton.WidthType.WIDTH_60;
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "수정";
            this.btn_save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "코드";
            this.gridColumn15.FieldName = "CODE";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 0;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "명칭";
            this.gridColumn20.FieldName = "NAME";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 1;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "코드";
            this.gridColumn21.FieldName = "CODE";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 0;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "명칭";
            this.gridColumn22.FieldName = "NAME";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 1;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "사번";
            this.gridColumn7.FieldName = "NAME";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 0;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "이름";
            this.gridColumn8.FieldName = "NAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            // 
            // gr_detail
            // 
            this.gr_detail.AppearanceCaption.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.gr_detail.AppearanceCaption.Options.UseFont = true;
            this.gr_detail.Controls.Add(this.srLabel3);
            this.gr_detail.Controls.Add(this.dat_year);
            this.gr_detail.Controls.Add(this.srLabel13);
            this.gr_detail.Controls.Add(this.cmb_gubn);
            this.gr_detail.Controls.Add(this.srLabel9);
            this.gr_detail.Controls.Add(this.sl_gnmu2);
            this.gr_detail.Controls.Add(this.srLabel5);
            this.gr_detail.Controls.Add(this.dat_ycdt2);
            this.gr_detail.Controls.Add(this.dat_ycdt);
            this.gr_detail.Controls.Add(this.sl_gnmu);
            this.gr_detail.Controls.Add(this.sl_embs);
            this.gr_detail.Controls.Add(this.srLabel7);
            this.gr_detail.Controls.Add(this.srLabel2);
            this.gr_detail.Controls.Add(this.srLabel1);
            this.gr_detail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gr_detail.Location = new System.Drawing.Point(0, 46);
            this.gr_detail.Name = "gr_detail";
            this.gr_detail.Size = new System.Drawing.Size(398, 225);
            this.gr_detail.TabIndex = 1;
            this.gr_detail.Text = "내역";
            // 
            // srLabel13
            // 
            this.srLabel13.BackColor = System.Drawing.Color.LightCyan;
            this.srLabel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srLabel13.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel13.ForeColor = System.Drawing.Color.Black;
            this.srLabel13.Location = new System.Drawing.Point(49, 107);
            this.srLabel13.Name = "srLabel13";
            this.srLabel13.Size = new System.Drawing.Size(65, 23);
            this.srLabel13.SRBulletColor = System.Drawing.Color.Empty;
            this.srLabel13.TabIndex = 552;
            this.srLabel13.Text = "신청구분";
            this.srLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmb_gubn
            // 
            this.cmb_gubn.EditValue = "결재신청";
            this.cmb_gubn.Enabled = false;
            this.cmb_gubn.EnterMoveNextControl = true;
            this.cmb_gubn.Location = new System.Drawing.Point(114, 107);
            this.cmb_gubn.Name = "cmb_gubn";
            this.cmb_gubn.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmb_gubn.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.cmb_gubn.Properties.Appearance.Options.UseFont = true;
            this.cmb_gubn.Properties.Appearance.Options.UseForeColor = true;
            this.cmb_gubn.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.cmb_gubn.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.cmb_gubn.Properties.AppearanceDropDown.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmb_gubn.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmb_gubn.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmb_gubn.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.cmb_gubn.Properties.AutoHeight = false;
            this.cmb_gubn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_gubn.Properties.DropDownRows = 10;
            this.cmb_gubn.Properties.Items.AddRange(new object[] {
            "결재신청",
            "취소신청"});
            this.cmb_gubn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_gubn.Size = new System.Drawing.Size(107, 23);
            this.cmb_gubn.TabIndex = 551;
            // 
            // srLabel9
            // 
            this.srLabel9.AutoSize = true;
            this.srLabel9.BackColor = System.Drawing.Color.Transparent;
            this.srLabel9.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel9.ForeColor = System.Drawing.Color.Black;
            this.srLabel9.Location = new System.Drawing.Point(221, 163);
            this.srLabel9.Name = "srLabel9";
            this.srLabel9.Size = new System.Drawing.Size(19, 15);
            this.srLabel9.TabIndex = 550;
            this.srLabel9.Text = "ㅡ";
            // 
            // sl_gnmu2
            // 
            this.sl_gnmu2.EditValue = "근무선택";
            this.sl_gnmu2.Enabled = false;
            this.sl_gnmu2.EnterMoveNextControl = true;
            this.sl_gnmu2.Location = new System.Drawing.Point(240, 159);
            this.sl_gnmu2.Name = "sl_gnmu2";
            this.sl_gnmu2.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_gnmu2.Properties.Appearance.Options.UseFont = true;
            this.sl_gnmu2.Properties.AppearanceDropDown.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_gnmu2.Properties.AppearanceDropDown.Options.UseFont = true;
            this.sl_gnmu2.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sl_gnmu2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.sl_gnmu2.Properties.AutoHeight = false;
            this.sl_gnmu2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sl_gnmu2.Properties.DisplayMember = "G_FNM";
            this.sl_gnmu2.Properties.LookAndFeel.SkinName = "Lilian";
            this.sl_gnmu2.Properties.MaxLength = 4;
            this.sl_gnmu2.Properties.NullText = "연차선택";
            this.sl_gnmu2.Properties.PopupFormSize = new System.Drawing.Size(300, 500);
            this.sl_gnmu2.Properties.ValueMember = "G_CODE";
            this.sl_gnmu2.Properties.View = this.gridView3;
            this.sl_gnmu2.Size = new System.Drawing.Size(107, 23);
            this.sl_gnmu2.TabIndex = 545;
            // 
            // gridView3
            // 
            this.gridView3.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.gridView3.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView3.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView3.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView3.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.gridView3.Appearance.Row.Options.UseFont = true;
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn11,
            this.gridColumn13});
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "코드";
            this.gridColumn4.FieldName = "G_CODE";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 205;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "근무명칭";
            this.gridColumn11.FieldName = "G_FNM";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 408;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "약칭";
            this.gridColumn13.FieldName = "G_SNM";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 2;
            this.gridColumn13.Width = 247;
            // 
            // srLabel5
            // 
            this.srLabel5.AutoSize = true;
            this.srLabel5.BackColor = System.Drawing.Color.Transparent;
            this.srLabel5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel5.ForeColor = System.Drawing.Color.Black;
            this.srLabel5.Location = new System.Drawing.Point(221, 136);
            this.srLabel5.Name = "srLabel5";
            this.srLabel5.Size = new System.Drawing.Size(19, 15);
            this.srLabel5.TabIndex = 549;
            this.srLabel5.Text = "ㅡ";
            // 
            // dat_ycdt2
            // 
            this.dat_ycdt2.EditValue = "GNMU_FR";
            this.dat_ycdt2.Enabled = false;
            this.dat_ycdt2.EnterMoveNextControl = true;
            this.dat_ycdt2.Location = new System.Drawing.Point(240, 133);
            this.dat_ycdt2.Name = "dat_ycdt2";
            this.dat_ycdt2.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dat_ycdt2.Properties.Appearance.Options.UseFont = true;
            this.dat_ycdt2.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.dat_ycdt2.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.dat_ycdt2.Properties.AutoHeight = false;
            this.dat_ycdt2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dat_ycdt2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dat_ycdt2.Properties.Mask.EditMask = "\\d\\d\\d\\d-\\d\\d-\\d\\d";
            this.dat_ycdt2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.dat_ycdt2.Properties.Mask.SaveLiteral = false;
            this.dat_ycdt2.Properties.Mask.ShowPlaceHolders = false;
            this.dat_ycdt2.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dat_ycdt2.Size = new System.Drawing.Size(107, 23);
            this.dat_ycdt2.TabIndex = 543;
            // 
            // dat_ycdt
            // 
            this.dat_ycdt.EditValue = "GNMU_FR";
            this.dat_ycdt.Enabled = false;
            this.dat_ycdt.EnterMoveNextControl = true;
            this.dat_ycdt.Location = new System.Drawing.Point(114, 133);
            this.dat_ycdt.Name = "dat_ycdt";
            this.dat_ycdt.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dat_ycdt.Properties.Appearance.Options.UseFont = true;
            this.dat_ycdt.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.dat_ycdt.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.dat_ycdt.Properties.AutoHeight = false;
            this.dat_ycdt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dat_ycdt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dat_ycdt.Properties.Mask.EditMask = "\\d\\d\\d\\d-\\d\\d-\\d\\d";
            this.dat_ycdt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.dat_ycdt.Properties.Mask.SaveLiteral = false;
            this.dat_ycdt.Properties.Mask.ShowPlaceHolders = false;
            this.dat_ycdt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dat_ycdt.Size = new System.Drawing.Size(107, 23);
            this.dat_ycdt.TabIndex = 542;
            // 
            // sl_gnmu
            // 
            this.sl_gnmu.EditValue = "근무선택";
            this.sl_gnmu.Enabled = false;
            this.sl_gnmu.EnterMoveNextControl = true;
            this.sl_gnmu.Location = new System.Drawing.Point(114, 159);
            this.sl_gnmu.Name = "sl_gnmu";
            this.sl_gnmu.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_gnmu.Properties.Appearance.Options.UseFont = true;
            this.sl_gnmu.Properties.AppearanceDropDown.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_gnmu.Properties.AppearanceDropDown.Options.UseFont = true;
            this.sl_gnmu.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sl_gnmu.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.sl_gnmu.Properties.AutoHeight = false;
            this.sl_gnmu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sl_gnmu.Properties.DisplayMember = "G_FNM";
            this.sl_gnmu.Properties.LookAndFeel.SkinName = "Lilian";
            this.sl_gnmu.Properties.MaxLength = 4;
            this.sl_gnmu.Properties.NullText = "연차선택";
            this.sl_gnmu.Properties.PopupFormSize = new System.Drawing.Size(300, 500);
            this.sl_gnmu.Properties.ValueMember = "G_CODE";
            this.sl_gnmu.Properties.View = this.gridView5;
            this.sl_gnmu.Size = new System.Drawing.Size(107, 23);
            this.sl_gnmu.TabIndex = 544;
            // 
            // gridView5
            // 
            this.gridView5.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.gridView5.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView5.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView5.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView5.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.gridView5.Appearance.Row.Options.UseFont = true;
            this.gridView5.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn64,
            this.gridColumn65,
            this.gridColumn66});
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn64
            // 
            this.gridColumn64.Caption = "코드";
            this.gridColumn64.FieldName = "G_CODE";
            this.gridColumn64.Name = "gridColumn64";
            this.gridColumn64.Visible = true;
            this.gridColumn64.VisibleIndex = 0;
            this.gridColumn64.Width = 205;
            // 
            // gridColumn65
            // 
            this.gridColumn65.Caption = "근무명칭";
            this.gridColumn65.FieldName = "G_FNM";
            this.gridColumn65.Name = "gridColumn65";
            this.gridColumn65.Visible = true;
            this.gridColumn65.VisibleIndex = 1;
            this.gridColumn65.Width = 408;
            // 
            // gridColumn66
            // 
            this.gridColumn66.Caption = "약칭";
            this.gridColumn66.FieldName = "G_SNM";
            this.gridColumn66.Name = "gridColumn66";
            this.gridColumn66.Visible = true;
            this.gridColumn66.VisibleIndex = 2;
            this.gridColumn66.Width = 247;
            // 
            // sl_embs
            // 
            this.sl_embs.EditValue = "사원선택";
            this.sl_embs.Enabled = false;
            this.sl_embs.EnterMoveNextControl = true;
            this.sl_embs.Location = new System.Drawing.Point(114, 81);
            this.sl_embs.Name = "sl_embs";
            this.sl_embs.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_embs.Properties.Appearance.Options.UseFont = true;
            this.sl_embs.Properties.AppearanceDropDown.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.sl_embs.Properties.AppearanceDropDown.Options.UseFont = true;
            this.sl_embs.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sl_embs.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.sl_embs.Properties.AutoHeight = false;
            this.sl_embs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sl_embs.Properties.DisplayMember = "NAME";
            this.sl_embs.Properties.LookAndFeel.SkinName = "Lilian";
            this.sl_embs.Properties.NullText = "사원선택";
            this.sl_embs.Properties.ValueMember = "CODE";
            this.sl_embs.Properties.View = this.gridView2;
            this.sl_embs.Size = new System.Drawing.Size(107, 23);
            this.sl_embs.TabIndex = 541;
            // 
            // gridView2
            // 
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView2.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.gridView2.Appearance.Row.Options.UseFont = true;
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18});
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "사번";
            this.gridColumn16.FieldName = "CODE";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 0;
            this.gridColumn16.Width = 80;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "이름";
            this.gridColumn17.FieldName = "NAME";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 1;
            this.gridColumn17.Width = 120;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "부서";
            this.gridColumn18.FieldName = "DEPT_NM";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 2;
            this.gridColumn18.Width = 184;
            // 
            // srLabel7
            // 
            this.srLabel7.BackColor = System.Drawing.Color.LightCyan;
            this.srLabel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srLabel7.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel7.ForeColor = System.Drawing.Color.Black;
            this.srLabel7.Location = new System.Drawing.Point(49, 81);
            this.srLabel7.Name = "srLabel7";
            this.srLabel7.Size = new System.Drawing.Size(65, 23);
            this.srLabel7.SRBulletColor = System.Drawing.Color.Empty;
            this.srLabel7.TabIndex = 546;
            this.srLabel7.Text = "사원";
            this.srLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // srLabel2
            // 
            this.srLabel2.BackColor = System.Drawing.Color.LightCyan;
            this.srLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srLabel2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel2.ForeColor = System.Drawing.Color.Black;
            this.srLabel2.Location = new System.Drawing.Point(49, 133);
            this.srLabel2.Name = "srLabel2";
            this.srLabel2.Size = new System.Drawing.Size(65, 23);
            this.srLabel2.SRBulletColor = System.Drawing.Color.Empty;
            this.srLabel2.TabIndex = 548;
            this.srLabel2.Text = "신청일자";
            this.srLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // srLabel1
            // 
            this.srLabel1.BackColor = System.Drawing.Color.LightCyan;
            this.srLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srLabel1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel1.ForeColor = System.Drawing.Color.Black;
            this.srLabel1.Location = new System.Drawing.Point(49, 159);
            this.srLabel1.Name = "srLabel1";
            this.srLabel1.Size = new System.Drawing.Size(65, 23);
            this.srLabel1.SRBulletColor = System.Drawing.Color.Empty;
            this.srLabel1.TabIndex = 547;
            this.srLabel1.Text = "연차구분";
            this.srLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dat_year
            // 
            this.dat_year.EditValue = "";
            this.dat_year.EnterMoveNextControl = true;
            this.dat_year.Location = new System.Drawing.Point(114, 43);
            this.dat_year.Name = "dat_year";
            this.dat_year.Properties.AllowMouseWheel = false;
            this.dat_year.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.dat_year.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dat_year.Properties.Appearance.Options.UseFont = true;
            this.dat_year.Properties.AutoHeight = false;
            this.dat_year.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dat_year.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dat_year.Properties.DisplayFormat.FormatString = "y";
            this.dat_year.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dat_year.Properties.EditFormat.FormatString = "y";
            this.dat_year.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dat_year.Properties.Mask.EditMask = "yyyy년";
            this.dat_year.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dat_year.Properties.NullDate = new System.DateTime(((long)(0)));
            this.dat_year.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            this.dat_year.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            this.dat_year.Size = new System.Drawing.Size(107, 23);
            this.dat_year.TabIndex = 554;
            // 
            // srLabel3
            // 
            this.srLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.srLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srLabel3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.srLabel3.ForeColor = System.Drawing.Color.Black;
            this.srLabel3.Location = new System.Drawing.Point(49, 43);
            this.srLabel3.Name = "srLabel3";
            this.srLabel3.Size = new System.Drawing.Size(65, 23);
            this.srLabel3.SRBulletColor = System.Drawing.Color.Empty;
            this.srLabel3.TabIndex = 555;
            this.srLabel3.Text = "기준년도";
            this.srLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // duty8021
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 271);
            this.Controls.Add(this.gr_detail);
            this.Controls.Add(this.srPanel1);
            this.Name = "duty8021";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "duty8021";
            this.Load += new System.EventHandler(this.duty8021_Load);
            this.Shown += new System.EventHandler(this.duty8021_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.duty8021_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.srPanel1)).EndInit();
            this.srPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.srPanel3)).EndInit();
            this.srPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gr_detail)).EndInit();
            this.gr_detail.ResumeLayout(false);
            this.gr_detail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_gubn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_gnmu2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_ycdt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_gnmu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sl_embs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_year.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dat_year.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SilkRoad.UserControls.SRButton btn_exit;
        private SilkRoad.UserControls.SRPanel srPanel1;
        private SilkRoad.UserControls.SRPanel srPanel3;
        private SilkRoad.UserControls.SRTitle srTitle1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private SilkRoad.UserControls.SRButton btn_save;
        private SilkRoad.UserControls.SRGroupBox gr_detail;
        private SilkRoad.UserControls.SRLabel srLabel13;
        private SilkRoad.UserControls.SRCombo cmb_gubn;
        private SilkRoad.UserControls.SRLabel srLabel9;
        private SilkRoad.UserControls.SRLookup2 sl_gnmu2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private SilkRoad.UserControls.SRLabel srLabel5;
        private DevExpress.XtraEditors.DateEdit dat_ycdt2;
        private DevExpress.XtraEditors.DateEdit dat_ycdt;
        private SilkRoad.UserControls.SRLookup2 sl_gnmu;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn64;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn65;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn66;
        private SilkRoad.UserControls.SRLookup2 sl_embs;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private SilkRoad.UserControls.SRLabel srLabel7;
        private SilkRoad.UserControls.SRLabel srLabel2;
        private SilkRoad.UserControls.SRLabel srLabel1;
        private SilkRoad.UserControls.SRLabel srLabel3;
        private SilkRoad.UserControls.SRDate dat_year;
    }
}

