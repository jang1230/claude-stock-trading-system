
namespace StockAutoTrade2
{
    partial class MainForm
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.KiwoomAPI = new AxKHOpenAPILib.AxKHOpenAPI();
            this.panel1 = new System.Windows.Forms.Panel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.holdingItemActivateButton = new System.Windows.Forms.Button();
            this.holdingitemsettingButton = new System.Windows.Forms.Button();
            this.profitLossButton = new System.Windows.Forms.Button();
            this.conditionSettingButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.myAccountComboBox = new System.Windows.Forms.ComboBox();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tradingConditionDataGridView = new System.Windows.Forms.DataGridView();
            this.매매조건식_조건식 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_종목투자금 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_매수종목수 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_실매수종목수 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_매수타입 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_익절 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_손절 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_추매 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매조건식_상태 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.매매조건식_삭제 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tradingItemDataGridView = new System.Windows.Forms.DataGridView();
            this.매매진행_구분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_종목명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_종목코드 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_조건식 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_총투자금 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_편입가격 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_편입대비수익률 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_매입금 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_보유수량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_주문가능수량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_매입가 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_현재가 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_평가손익 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_수익률 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_등락율 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_추매 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_진행상황 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매매진행_수동매수 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.매매진행_즉시매도 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.orderDataGridView = new System.Windows.Forms.DataGridView();
            this.주문_종목명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_조건식 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_주문번호 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_주문시간 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_주문량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_주문가격 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_매매구분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.주문_가격구분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.conclusionDataGridView = new System.Windows.Forms.DataGridView();
            this.체결_종목명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_주문번호 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_주문시간 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_주문량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_체결량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_단위체결량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_체결가 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체결_매매구분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.soldDataGridView = new System.Windows.Forms.DataGridView();
            this.매도_종목명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도_매도시간 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도_매도량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도_매도가격 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도_평가손익 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.매도_수익률 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.curOrderAmountLabel = new System.Windows.Forms.Label();
            this.totalPurchaseAmountLabel = new System.Windows.Forms.Label();
            this.totalEvalutionAmountLabel = new System.Windows.Forms.Label();
            this.totalProfitLossLabel = new System.Windows.Forms.Label();
            this.totalRateOfReturnLabel = new System.Windows.Forms.Label();
            this.indicatorPictureBox = new System.Windows.Forms.PictureBox();
            this.indicatorLabel = new System.Windows.Forms.Label();
            this.bunBongDataGridView = new System.Windows.Forms.DataGridView();
            this.분봉_상태 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.분봉_매수 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.분봉_추매 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.분봉_익절 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.분봉_손절 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indicatorViewButton = new System.Windows.Forms.Button();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.medoHiddenButton = new System.Windows.Forms.Button();
            this.showEntireLogButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.KiwoomAPI)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradingConditionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradingItemDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conclusionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.soldDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.indicatorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunBongDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            this.SuspendLayout();
            // 
            // KiwoomAPI
            // 
            this.KiwoomAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.KiwoomAPI.Enabled = true;
            this.KiwoomAPI.Location = new System.Drawing.Point(1275, 932);
            this.KiwoomAPI.Name = "KiwoomAPI";
            this.KiwoomAPI.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("KiwoomAPI.OcxState")));
            this.KiwoomAPI.Size = new System.Drawing.Size(47, 25);
            this.KiwoomAPI.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(105)))), ((int)(((byte)(140)))));
            this.panel1.Controls.Add(this.passwordTextBox);
            this.panel1.Controls.Add(this.holdingItemActivateButton);
            this.panel1.Controls.Add(this.holdingitemsettingButton);
            this.panel1.Controls.Add(this.profitLossButton);
            this.panel1.Controls.Add(this.conditionSettingButton);
            this.panel1.Location = new System.Drawing.Point(-2, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1277, 49);
            this.panel1.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(287, 16);
            this.passwordTextBox.MaxLength = 4;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(31, 21);
            this.passwordTextBox.TabIndex = 115;
            this.passwordTextBox.Text = "****";
            // 
            // holdingItemActivateButton
            // 
            this.holdingItemActivateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.holdingItemActivateButton.BackColor = System.Drawing.Color.White;
            this.holdingItemActivateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.holdingItemActivateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.holdingItemActivateButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.holdingItemActivateButton.Location = new System.Drawing.Point(840, 10);
            this.holdingItemActivateButton.Name = "holdingItemActivateButton";
            this.holdingItemActivateButton.Size = new System.Drawing.Size(98, 30);
            this.holdingItemActivateButton.TabIndex = 113;
            this.holdingItemActivateButton.Text = "전체보유매도";
            this.holdingItemActivateButton.UseVisualStyleBackColor = false;
            // 
            // holdingitemsettingButton
            // 
            this.holdingitemsettingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.holdingitemsettingButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.holdingitemsettingButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.holdingitemsettingButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(121)))), ((int)(((byte)(157)))));
            this.holdingitemsettingButton.FlatAppearance.BorderSize = 0;
            this.holdingitemsettingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.holdingitemsettingButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.holdingitemsettingButton.ForeColor = System.Drawing.Color.White;
            this.holdingitemsettingButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.holdingitemsettingButton.Location = new System.Drawing.Point(1163, 9);
            this.holdingitemsettingButton.Name = "holdingitemsettingButton";
            this.holdingitemsettingButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.holdingitemsettingButton.Size = new System.Drawing.Size(101, 34);
            this.holdingitemsettingButton.TabIndex = 112;
            this.holdingitemsettingButton.Text = "보유종목 설정";
            this.holdingitemsettingButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.holdingitemsettingButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.holdingitemsettingButton.UseVisualStyleBackColor = false;
            // 
            // profitLossButton
            // 
            this.profitLossButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.profitLossButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.profitLossButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.profitLossButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.profitLossButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.profitLossButton.ForeColor = System.Drawing.Color.White;
            this.profitLossButton.Location = new System.Drawing.Point(968, 9);
            this.profitLossButton.Name = "profitLossButton";
            this.profitLossButton.Size = new System.Drawing.Size(93, 34);
            this.profitLossButton.TabIndex = 108;
            this.profitLossButton.Text = "총실현손익";
            this.profitLossButton.UseVisualStyleBackColor = false;
            // 
            // conditionSettingButton
            // 
            this.conditionSettingButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.conditionSettingButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.conditionSettingButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.conditionSettingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.conditionSettingButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.conditionSettingButton.ForeColor = System.Drawing.Color.White;
            this.conditionSettingButton.Location = new System.Drawing.Point(1066, 9);
            this.conditionSettingButton.Name = "conditionSettingButton";
            this.conditionSettingButton.Size = new System.Drawing.Size(93, 34);
            this.conditionSettingButton.TabIndex = 3;
            this.conditionSettingButton.Text = "조건식설정";
            this.conditionSettingButton.UseVisualStyleBackColor = false;
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.LoginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LoginButton.ForeColor = System.Drawing.Color.White;
            this.LoginButton.Location = new System.Drawing.Point(9, 8);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(93, 34);
            this.LoginButton.TabIndex = 2;
            this.LoginButton.Text = "로그인";
            this.LoginButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(105)))), ((int)(((byte)(140)))));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(114, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "나의 계좌";
            // 
            // myAccountComboBox
            // 
            this.myAccountComboBox.DropDownWidth = 102;
            this.myAccountComboBox.FormattingEnabled = true;
            this.myAccountComboBox.Location = new System.Drawing.Point(177, 15);
            this.myAccountComboBox.Name = "myAccountComboBox";
            this.myAccountComboBox.Size = new System.Drawing.Size(102, 20);
            this.myAccountComboBox.TabIndex = 2;
            // 
            // logListBox
            // 
            this.logListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.logListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.logListBox.FormattingEnabled = true;
            this.logListBox.HorizontalExtent = 1000;
            this.logListBox.HorizontalScrollbar = true;
            this.logListBox.ItemHeight = 12;
            this.logListBox.Location = new System.Drawing.Point(639, 781);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(636, 172);
            this.logListBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.label2.Location = new System.Drawing.Point(10, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 18);
            this.label2.TabIndex = 56;
            this.label2.Text = "조건식 매매 방식";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox1.Location = new System.Drawing.Point(10, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 3);
            this.pictureBox1.TabIndex = 57;
            this.pictureBox1.TabStop = false;
            // 
            // tradingConditionDataGridView
            // 
            this.tradingConditionDataGridView.AllowUserToAddRows = false;
            this.tradingConditionDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.tradingConditionDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tradingConditionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tradingConditionDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.tradingConditionDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tradingConditionDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tradingConditionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tradingConditionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.매매조건식_조건식,
            this.매매조건식_종목투자금,
            this.매매조건식_매수종목수,
            this.매매조건식_실매수종목수,
            this.매매조건식_매수타입,
            this.매매조건식_익절,
            this.매매조건식_손절,
            this.매매조건식_추매,
            this.매매조건식_상태,
            this.매매조건식_삭제});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tradingConditionDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.tradingConditionDataGridView.EnableHeadersVisualStyles = false;
            this.tradingConditionDataGridView.GridColor = System.Drawing.Color.Silver;
            this.tradingConditionDataGridView.Location = new System.Drawing.Point(10, 90);
            this.tradingConditionDataGridView.Name = "tradingConditionDataGridView";
            this.tradingConditionDataGridView.RowHeadersVisible = false;
            this.tradingConditionDataGridView.RowTemplate.Height = 23;
            this.tradingConditionDataGridView.Size = new System.Drawing.Size(868, 130);
            this.tradingConditionDataGridView.TabIndex = 3;
            // 
            // 매매조건식_조건식
            // 
            this.매매조건식_조건식.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.매매조건식_조건식.FillWeight = 130F;
            this.매매조건식_조건식.Frozen = true;
            this.매매조건식_조건식.HeaderText = "조건식";
            this.매매조건식_조건식.Name = "매매조건식_조건식";
            this.매매조건식_조건식.ReadOnly = true;
            this.매매조건식_조건식.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.매매조건식_조건식.Width = 76;
            // 
            // 매매조건식_종목투자금
            // 
            this.매매조건식_종목투자금.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.매매조건식_종목투자금.FillWeight = 38.80204F;
            this.매매조건식_종목투자금.HeaderText = "종목투자금";
            this.매매조건식_종목투자금.Name = "매매조건식_종목투자금";
            this.매매조건식_종목투자금.ReadOnly = true;
            this.매매조건식_종목투자금.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매매조건식_매수종목수
            // 
            this.매매조건식_매수종목수.FillWeight = 34.92184F;
            this.매매조건식_매수종목수.HeaderText = "매수종목수";
            this.매매조건식_매수종목수.Name = "매매조건식_매수종목수";
            this.매매조건식_매수종목수.ReadOnly = true;
            this.매매조건식_매수종목수.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매매조건식_실매수종목수
            // 
            this.매매조건식_실매수종목수.FillWeight = 40.5775F;
            this.매매조건식_실매수종목수.HeaderText = "실매수종목수";
            this.매매조건식_실매수종목수.Name = "매매조건식_실매수종목수";
            this.매매조건식_실매수종목수.ReadOnly = true;
            // 
            // 매매조건식_매수타입
            // 
            this.매매조건식_매수타입.FillWeight = 38.80204F;
            this.매매조건식_매수타입.HeaderText = "매수타입";
            this.매매조건식_매수타입.Name = "매매조건식_매수타입";
            this.매매조건식_매수타입.ReadOnly = true;
            this.매매조건식_매수타입.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매매조건식_익절
            // 
            this.매매조건식_익절.FillWeight = 27.16143F;
            this.매매조건식_익절.HeaderText = "익절%";
            this.매매조건식_익절.Name = "매매조건식_익절";
            this.매매조건식_익절.ReadOnly = true;
            this.매매조건식_익절.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매매조건식_손절
            // 
            this.매매조건식_손절.FillWeight = 27.16143F;
            this.매매조건식_손절.HeaderText = "손절%";
            this.매매조건식_손절.Name = "매매조건식_손절";
            this.매매조건식_손절.ReadOnly = true;
            this.매매조건식_손절.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 매매조건식_추매
            // 
            this.매매조건식_추매.FillWeight = 29.12453F;
            this.매매조건식_추매.HeaderText = "추매";
            this.매매조건식_추매.Name = "매매조건식_추매";
            this.매매조건식_추매.ReadOnly = true;
            // 
            // 매매조건식_상태
            // 
            this.매매조건식_상태.FillWeight = 34.92184F;
            this.매매조건식_상태.HeaderText = "상태";
            this.매매조건식_상태.Name = "매매조건식_상태";
            this.매매조건식_상태.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 매매조건식_삭제
            // 
            this.매매조건식_삭제.FillWeight = 34.92184F;
            this.매매조건식_삭제.HeaderText = "삭제하기";
            this.매매조건식_삭제.Name = "매매조건식_삭제";
            this.매매조건식_삭제.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.label4.Location = new System.Drawing.Point(12, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 18);
            this.label4.TabIndex = 62;
            this.label4.Text = "매매 진행 종목";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox3.Location = new System.Drawing.Point(12, 260);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(160, 3);
            this.pictureBox3.TabIndex = 64;
            this.pictureBox3.TabStop = false;
            // 
            // tradingItemDataGridView
            // 
            this.tradingItemDataGridView.AllowUserToAddRows = false;
            this.tradingItemDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.tradingItemDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tradingItemDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tradingItemDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.tradingItemDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tradingItemDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.tradingItemDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tradingItemDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.매매진행_구분,
            this.매매진행_종목명,
            this.매매진행_종목코드,
            this.매매진행_조건식,
            this.매매진행_총투자금,
            this.매매진행_편입가격,
            this.매매진행_편입대비수익률,
            this.매매진행_매입금,
            this.매매진행_보유수량,
            this.매매진행_주문가능수량,
            this.매매진행_매입가,
            this.매매진행_현재가,
            this.매매진행_평가손익,
            this.매매진행_수익률,
            this.매매진행_등락율,
            this.매매진행_추매,
            this.매매진행_진행상황,
            this.매매진행_수동매수,
            this.매매진행_즉시매도});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tradingItemDataGridView.DefaultCellStyle = dataGridViewCellStyle9;
            this.tradingItemDataGridView.EnableHeadersVisualStyles = false;
            this.tradingItemDataGridView.GridColor = System.Drawing.Color.Silver;
            this.tradingItemDataGridView.Location = new System.Drawing.Point(10, 264);
            this.tradingItemDataGridView.Name = "tradingItemDataGridView";
            this.tradingItemDataGridView.RowHeadersVisible = false;
            this.tradingItemDataGridView.RowTemplate.Height = 23;
            this.tradingItemDataGridView.Size = new System.Drawing.Size(1167, 175);
            this.tradingItemDataGridView.TabIndex = 61;
            // 
            // 매매진행_구분
            // 
            this.매매진행_구분.FillWeight = 23.36003F;
            this.매매진행_구분.HeaderText = "구분";
            this.매매진행_구분.Name = "매매진행_구분";
            this.매매진행_구분.ReadOnly = true;
            // 
            // 매매진행_종목명
            // 
            this.매매진행_종목명.FillWeight = 23.36003F;
            this.매매진행_종목명.HeaderText = "종목명";
            this.매매진행_종목명.Name = "매매진행_종목명";
            this.매매진행_종목명.ReadOnly = true;
            // 
            // 매매진행_종목코드
            // 
            this.매매진행_종목코드.FillWeight = 23.36003F;
            this.매매진행_종목코드.HeaderText = "종목코드";
            this.매매진행_종목코드.Name = "매매진행_종목코드";
            this.매매진행_종목코드.ReadOnly = true;
            // 
            // 매매진행_조건식
            // 
            this.매매진행_조건식.FillWeight = 23.36003F;
            this.매매진행_조건식.HeaderText = "조건식";
            this.매매진행_조건식.Name = "매매진행_조건식";
            this.매매진행_조건식.ReadOnly = true;
            // 
            // 매매진행_총투자금
            // 
            this.매매진행_총투자금.FillWeight = 23.36003F;
            this.매매진행_총투자금.HeaderText = "총투자금";
            this.매매진행_총투자금.Name = "매매진행_총투자금";
            this.매매진행_총투자금.ReadOnly = true;
            // 
            // 매매진행_편입가격
            // 
            this.매매진행_편입가격.FillWeight = 23.36003F;
            this.매매진행_편입가격.HeaderText = "편입가격";
            this.매매진행_편입가격.Name = "매매진행_편입가격";
            this.매매진행_편입가격.ReadOnly = true;
            // 
            // 매매진행_편입대비수익률
            // 
            this.매매진행_편입대비수익률.FillWeight = 130F;
            this.매매진행_편입대비수익률.HeaderText = "편입대비수익률";
            this.매매진행_편입대비수익률.Name = "매매진행_편입대비수익률";
            this.매매진행_편입대비수익률.ReadOnly = true;
            this.매매진행_편입대비수익률.Visible = false;
            // 
            // 매매진행_매입금
            // 
            dataGridViewCellStyle6.NullValue = null;
            this.매매진행_매입금.DefaultCellStyle = dataGridViewCellStyle6;
            this.매매진행_매입금.FillWeight = 23.36003F;
            this.매매진행_매입금.HeaderText = "매입금";
            this.매매진행_매입금.Name = "매매진행_매입금";
            this.매매진행_매입금.ReadOnly = true;
            // 
            // 매매진행_보유수량
            // 
            this.매매진행_보유수량.FillWeight = 23.36003F;
            this.매매진행_보유수량.HeaderText = "보유수량";
            this.매매진행_보유수량.Name = "매매진행_보유수량";
            this.매매진행_보유수량.ReadOnly = true;
            // 
            // 매매진행_주문가능수량
            // 
            this.매매진행_주문가능수량.FillWeight = 30F;
            this.매매진행_주문가능수량.HeaderText = "주문가능수량";
            this.매매진행_주문가능수량.Name = "매매진행_주문가능수량";
            this.매매진행_주문가능수량.ReadOnly = true;
            // 
            // 매매진행_매입가
            // 
            this.매매진행_매입가.FillWeight = 23.36003F;
            this.매매진행_매입가.HeaderText = "매입가";
            this.매매진행_매입가.Name = "매매진행_매입가";
            this.매매진행_매입가.ReadOnly = true;
            // 
            // 매매진행_현재가
            // 
            this.매매진행_현재가.FillWeight = 23.36003F;
            this.매매진행_현재가.HeaderText = "현재가";
            this.매매진행_현재가.Name = "매매진행_현재가";
            this.매매진행_현재가.ReadOnly = true;
            // 
            // 매매진행_평가손익
            // 
            this.매매진행_평가손익.FillWeight = 23.36003F;
            this.매매진행_평가손익.HeaderText = "평가손익";
            this.매매진행_평가손익.Name = "매매진행_평가손익";
            this.매매진행_평가손익.ReadOnly = true;
            // 
            // 매매진행_수익률
            // 
            this.매매진행_수익률.FillWeight = 23.36003F;
            this.매매진행_수익률.HeaderText = "수익률";
            this.매매진행_수익률.Name = "매매진행_수익률";
            this.매매진행_수익률.ReadOnly = true;
            // 
            // 매매진행_등락율
            // 
            this.매매진행_등락율.FillWeight = 23.36003F;
            this.매매진행_등락율.HeaderText = "등락율";
            this.매매진행_등락율.Name = "매매진행_등락율";
            this.매매진행_등락율.ReadOnly = true;
            // 
            // 매매진행_추매
            // 
            this.매매진행_추매.FillWeight = 17.82636F;
            this.매매진행_추매.HeaderText = "추매";
            this.매매진행_추매.Name = "매매진행_추매";
            this.매매진행_추매.ReadOnly = true;
            // 
            // 매매진행_진행상황
            // 
            this.매매진행_진행상황.FillWeight = 23.36003F;
            this.매매진행_진행상황.HeaderText = "진행상황";
            this.매매진행_진행상황.Name = "매매진행_진행상황";
            this.매매진행_진행상황.ReadOnly = true;
            // 
            // 매매진행_수동매수
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.NullValue = "수동매수";
            this.매매진행_수동매수.DefaultCellStyle = dataGridViewCellStyle7;
            this.매매진행_수동매수.FillWeight = 23.73285F;
            this.매매진행_수동매수.HeaderText = "수동매수";
            this.매매진행_수동매수.Name = "매매진행_수동매수";
            // 
            // 매매진행_즉시매도
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.NullValue = "즉시매도";
            this.매매진행_즉시매도.DefaultCellStyle = dataGridViewCellStyle8;
            this.매매진행_즉시매도.FillWeight = 23.36003F;
            this.매매진행_즉시매도.HeaderText = "즉시매도";
            this.매매진행_즉시매도.Name = "매매진행_즉시매도";
            this.매매진행_즉시매도.ReadOnly = true;
            this.매매진행_즉시매도.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox5.Location = new System.Drawing.Point(642, 621);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(160, 3);
            this.pictureBox5.TabIndex = 73;
            this.pictureBox5.TabStop = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.label6.Location = new System.Drawing.Point(642, 602);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 18);
            this.label6.TabIndex = 72;
            this.label6.Text = "체결상태";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox4.Location = new System.Drawing.Point(10, 621);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(160, 3);
            this.pictureBox4.TabIndex = 71;
            this.pictureBox4.TabStop = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.label5.Location = new System.Drawing.Point(10, 602);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 18);
            this.label5.TabIndex = 70;
            this.label5.Text = "주문상태";
            // 
            // orderDataGridView
            // 
            this.orderDataGridView.AllowUserToAddRows = false;
            this.orderDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            this.orderDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.orderDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.orderDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.orderDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.orderDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.orderDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.orderDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.주문_종목명,
            this.주문_조건식,
            this.주문_주문번호,
            this.주문_주문시간,
            this.주문_주문량,
            this.주문_주문가격,
            this.주문_매매구분,
            this.주문_가격구분});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.orderDataGridView.DefaultCellStyle = dataGridViewCellStyle12;
            this.orderDataGridView.EnableHeadersVisualStyles = false;
            this.orderDataGridView.GridColor = System.Drawing.Color.Silver;
            this.orderDataGridView.Location = new System.Drawing.Point(7, 630);
            this.orderDataGridView.Name = "orderDataGridView";
            this.orderDataGridView.RowHeadersVisible = false;
            this.orderDataGridView.RowTemplate.Height = 23;
            this.orderDataGridView.Size = new System.Drawing.Size(628, 145);
            this.orderDataGridView.TabIndex = 74;
            this.orderDataGridView.SelectionChanged += new System.EventHandler(this.orderDataGridView_SelectionChanged);
            // 
            // 주문_종목명
            // 
            this.주문_종목명.HeaderText = "종목명";
            this.주문_종목명.Name = "주문_종목명";
            this.주문_종목명.ReadOnly = true;
            // 
            // 주문_조건식
            // 
            this.주문_조건식.HeaderText = "조건식";
            this.주문_조건식.Name = "주문_조건식";
            this.주문_조건식.ReadOnly = true;
            // 
            // 주문_주문번호
            // 
            this.주문_주문번호.HeaderText = "주문번호";
            this.주문_주문번호.Name = "주문_주문번호";
            this.주문_주문번호.ReadOnly = true;
            // 
            // 주문_주문시간
            // 
            this.주문_주문시간.HeaderText = "주문시간";
            this.주문_주문시간.Name = "주문_주문시간";
            this.주문_주문시간.ReadOnly = true;
            // 
            // 주문_주문량
            // 
            this.주문_주문량.HeaderText = "주문량";
            this.주문_주문량.Name = "주문_주문량";
            this.주문_주문량.ReadOnly = true;
            // 
            // 주문_주문가격
            // 
            this.주문_주문가격.HeaderText = "주문가격";
            this.주문_주문가격.Name = "주문_주문가격";
            this.주문_주문가격.ReadOnly = true;
            // 
            // 주문_매매구분
            // 
            this.주문_매매구분.HeaderText = "매매구분";
            this.주문_매매구분.Name = "주문_매매구분";
            this.주문_매매구분.ReadOnly = true;
            // 
            // 주문_가격구분
            // 
            this.주문_가격구분.HeaderText = "가격구분";
            this.주문_가격구분.Name = "주문_가격구분";
            this.주문_가격구분.ReadOnly = true;
            // 
            // conclusionDataGridView
            // 
            this.conclusionDataGridView.AllowUserToAddRows = false;
            this.conclusionDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            this.conclusionDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.conclusionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.conclusionDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.conclusionDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.conclusionDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.conclusionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.conclusionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.체결_종목명,
            this.체결_주문번호,
            this.체결_주문시간,
            this.체결_주문량,
            this.체결_체결량,
            this.체결_단위체결량,
            this.체결_체결가,
            this.체결_매매구분});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle15.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.conclusionDataGridView.DefaultCellStyle = dataGridViewCellStyle15;
            this.conclusionDataGridView.EnableHeadersVisualStyles = false;
            this.conclusionDataGridView.GridColor = System.Drawing.Color.Silver;
            this.conclusionDataGridView.Location = new System.Drawing.Point(639, 630);
            this.conclusionDataGridView.Name = "conclusionDataGridView";
            this.conclusionDataGridView.RowHeadersVisible = false;
            this.conclusionDataGridView.RowTemplate.Height = 23;
            this.conclusionDataGridView.Size = new System.Drawing.Size(633, 145);
            this.conclusionDataGridView.TabIndex = 75;
            this.conclusionDataGridView.SelectionChanged += new System.EventHandler(this.conclusionDataGridView_SelectionChanged);
            // 
            // 체결_종목명
            // 
            this.체결_종목명.HeaderText = "종목명";
            this.체결_종목명.Name = "체결_종목명";
            // 
            // 체결_주문번호
            // 
            this.체결_주문번호.HeaderText = "주문번호";
            this.체결_주문번호.Name = "체결_주문번호";
            // 
            // 체결_주문시간
            // 
            this.체결_주문시간.HeaderText = "주문시간";
            this.체결_주문시간.Name = "체결_주문시간";
            // 
            // 체결_주문량
            // 
            this.체결_주문량.HeaderText = "주문량";
            this.체결_주문량.Name = "체결_주문량";
            // 
            // 체결_체결량
            // 
            this.체결_체결량.HeaderText = "체결량";
            this.체결_체결량.Name = "체결_체결량";
            // 
            // 체결_단위체결량
            // 
            this.체결_단위체결량.HeaderText = "단위체결량";
            this.체결_단위체결량.Name = "체결_단위체결량";
            // 
            // 체결_체결가
            // 
            this.체결_체결가.HeaderText = "체결가";
            this.체결_체결가.Name = "체결_체결가";
            // 
            // 체결_매매구분
            // 
            this.체결_매매구분.HeaderText = "매매구분";
            this.체결_매매구분.Name = "체결_매매구분";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.label7.Location = new System.Drawing.Point(10, 781);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 18);
            this.label7.TabIndex = 76;
            this.label7.Text = "종목 수익률";
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox6.Location = new System.Drawing.Point(10, 800);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(160, 3);
            this.pictureBox6.TabIndex = 77;
            this.pictureBox6.TabStop = false;
            // 
            // soldDataGridView
            // 
            this.soldDataGridView.AllowUserToAddRows = false;
            this.soldDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            this.soldDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.soldDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.soldDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.soldDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.soldDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.soldDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.soldDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.매도_종목명,
            this.매도_매도시간,
            this.매도_매도량,
            this.매도_매도가격,
            this.매도_평가손익,
            this.매도_수익률});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.soldDataGridView.DefaultCellStyle = dataGridViewCellStyle18;
            this.soldDataGridView.EnableHeadersVisualStyles = false;
            this.soldDataGridView.GridColor = System.Drawing.Color.Silver;
            this.soldDataGridView.Location = new System.Drawing.Point(7, 809);
            this.soldDataGridView.Name = "soldDataGridView";
            this.soldDataGridView.RowHeadersVisible = false;
            this.soldDataGridView.RowTemplate.Height = 23;
            this.soldDataGridView.Size = new System.Drawing.Size(628, 145);
            this.soldDataGridView.TabIndex = 78;
            this.soldDataGridView.SelectionChanged += new System.EventHandler(this.soldDataGridView_SelectionChanged);
            // 
            // 매도_종목명
            // 
            this.매도_종목명.HeaderText = "종목명";
            this.매도_종목명.Name = "매도_종목명";
            this.매도_종목명.ReadOnly = true;
            // 
            // 매도_매도시간
            // 
            this.매도_매도시간.HeaderText = "매도시간";
            this.매도_매도시간.Name = "매도_매도시간";
            this.매도_매도시간.ReadOnly = true;
            // 
            // 매도_매도량
            // 
            this.매도_매도량.HeaderText = "매도량";
            this.매도_매도량.Name = "매도_매도량";
            this.매도_매도량.ReadOnly = true;
            // 
            // 매도_매도가격
            // 
            this.매도_매도가격.HeaderText = "매도가격";
            this.매도_매도가격.Name = "매도_매도가격";
            this.매도_매도가격.ReadOnly = true;
            // 
            // 매도_평가손익
            // 
            this.매도_평가손익.HeaderText = "평가손익";
            this.매도_평가손익.Name = "매도_평가손익";
            this.매도_평가손익.ReadOnly = true;
            // 
            // 매도_수익률
            // 
            this.매도_수익률.HeaderText = "수익률";
            this.매도_수익률.Name = "매도_수익률";
            this.매도_수익률.ReadOnly = true;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("굴림", 9.5F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(909, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(140, 18);
            this.label12.TabIndex = 97;
            this.label12.Text = "총 수익률";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("굴림", 9.5F);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(908, 160);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 18);
            this.label11.TabIndex = 96;
            this.label11.Text = "총 손익";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("굴림", 9.5F);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(908, 129);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 18);
            this.label10.TabIndex = 95;
            this.label10.Text = "총 평가금(세금포함)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("굴림", 9.5F);
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(908, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 18);
            this.label9.TabIndex = 94;
            this.label9.Text = "총 매입금";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("굴림", 9.5F);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(908, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 18);
            this.label8.TabIndex = 93;
            this.label8.Text = "매수가능금액";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox7.Location = new System.Drawing.Point(908, 89);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(274, 1);
            this.pictureBox7.TabIndex = 98;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox2.Location = new System.Drawing.Point(908, 121);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(274, 1);
            this.pictureBox2.TabIndex = 99;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox8.Location = new System.Drawing.Point(908, 152);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(274, 1);
            this.pictureBox8.TabIndex = 100;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox9.Location = new System.Drawing.Point(908, 183);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(274, 1);
            this.pictureBox9.TabIndex = 101;
            this.pictureBox9.TabStop = false;
            // 
            // curOrderAmountLabel
            // 
            this.curOrderAmountLabel.BackColor = System.Drawing.Color.Transparent;
            this.curOrderAmountLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.curOrderAmountLabel.ForeColor = System.Drawing.Color.Black;
            this.curOrderAmountLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.curOrderAmountLabel.Location = new System.Drawing.Point(1054, 67);
            this.curOrderAmountLabel.Name = "curOrderAmountLabel";
            this.curOrderAmountLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.curOrderAmountLabel.Size = new System.Drawing.Size(122, 19);
            this.curOrderAmountLabel.TabIndex = 103;
            this.curOrderAmountLabel.Text = "0";
            this.curOrderAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalPurchaseAmountLabel
            // 
            this.totalPurchaseAmountLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalPurchaseAmountLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalPurchaseAmountLabel.ForeColor = System.Drawing.Color.Black;
            this.totalPurchaseAmountLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalPurchaseAmountLabel.Location = new System.Drawing.Point(1054, 99);
            this.totalPurchaseAmountLabel.Name = "totalPurchaseAmountLabel";
            this.totalPurchaseAmountLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalPurchaseAmountLabel.Size = new System.Drawing.Size(122, 19);
            this.totalPurchaseAmountLabel.TabIndex = 104;
            this.totalPurchaseAmountLabel.Text = "0";
            this.totalPurchaseAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalEvalutionAmountLabel
            // 
            this.totalEvalutionAmountLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalEvalutionAmountLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalEvalutionAmountLabel.ForeColor = System.Drawing.Color.Black;
            this.totalEvalutionAmountLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalEvalutionAmountLabel.Location = new System.Drawing.Point(1059, 130);
            this.totalEvalutionAmountLabel.Name = "totalEvalutionAmountLabel";
            this.totalEvalutionAmountLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalEvalutionAmountLabel.Size = new System.Drawing.Size(117, 19);
            this.totalEvalutionAmountLabel.TabIndex = 105;
            this.totalEvalutionAmountLabel.Text = "0";
            this.totalEvalutionAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalProfitLossLabel
            // 
            this.totalProfitLossLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalProfitLossLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalProfitLossLabel.ForeColor = System.Drawing.Color.Black;
            this.totalProfitLossLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalProfitLossLabel.Location = new System.Drawing.Point(1057, 161);
            this.totalProfitLossLabel.Name = "totalProfitLossLabel";
            this.totalProfitLossLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalProfitLossLabel.Size = new System.Drawing.Size(119, 19);
            this.totalProfitLossLabel.TabIndex = 106;
            this.totalProfitLossLabel.Text = "0";
            this.totalProfitLossLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // totalRateOfReturnLabel
            // 
            this.totalRateOfReturnLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalRateOfReturnLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalRateOfReturnLabel.ForeColor = System.Drawing.Color.Black;
            this.totalRateOfReturnLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalRateOfReturnLabel.Location = new System.Drawing.Point(1063, 191);
            this.totalRateOfReturnLabel.Name = "totalRateOfReturnLabel";
            this.totalRateOfReturnLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalRateOfReturnLabel.Size = new System.Drawing.Size(114, 19);
            this.totalRateOfReturnLabel.TabIndex = 107;
            this.totalRateOfReturnLabel.Text = "0.00%";
            this.totalRateOfReturnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // indicatorPictureBox
            // 
            this.indicatorPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.indicatorPictureBox.Location = new System.Drawing.Point(11, 461);
            this.indicatorPictureBox.Name = "indicatorPictureBox";
            this.indicatorPictureBox.Size = new System.Drawing.Size(160, 3);
            this.indicatorPictureBox.TabIndex = 110;
            this.indicatorPictureBox.TabStop = false;
            // 
            // indicatorLabel
            // 
            this.indicatorLabel.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.indicatorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.indicatorLabel.Location = new System.Drawing.Point(11, 442);
            this.indicatorLabel.Name = "indicatorLabel";
            this.indicatorLabel.Size = new System.Drawing.Size(140, 27);
            this.indicatorLabel.TabIndex = 109;
            this.indicatorLabel.Text = "지표가격계산";
            // 
            // bunBongDataGridView
            // 
            this.bunBongDataGridView.AllowUserToAddRows = false;
            this.bunBongDataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.Black;
            this.bunBongDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            this.bunBongDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bunBongDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.bunBongDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bunBongDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.bunBongDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.bunBongDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.분봉_상태,
            this.분봉_매수,
            this.분봉_추매,
            this.분봉_익절,
            this.분봉_손절});
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle21.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bunBongDataGridView.DefaultCellStyle = dataGridViewCellStyle21;
            this.bunBongDataGridView.EnableHeadersVisualStyles = false;
            this.bunBongDataGridView.GridColor = System.Drawing.Color.Silver;
            this.bunBongDataGridView.Location = new System.Drawing.Point(10, 471);
            this.bunBongDataGridView.Name = "bunBongDataGridView";
            this.bunBongDataGridView.RowHeadersVisible = false;
            this.bunBongDataGridView.RowTemplate.Height = 23;
            this.bunBongDataGridView.Size = new System.Drawing.Size(1166, 128);
            this.bunBongDataGridView.TabIndex = 108;
            this.bunBongDataGridView.SelectionChanged += new System.EventHandler(this.bunBongDataGridView_SelectionChanged);
            // 
            // 분봉_상태
            // 
            this.분봉_상태.FillWeight = 40F;
            this.분봉_상태.HeaderText = "종목명";
            this.분봉_상태.Name = "분봉_상태";
            this.분봉_상태.ReadOnly = true;
            // 
            // 분봉_매수
            // 
            this.분봉_매수.HeaderText = "매수";
            this.분봉_매수.Name = "분봉_매수";
            this.분봉_매수.ReadOnly = true;
            // 
            // 분봉_추매
            // 
            this.분봉_추매.HeaderText = "추매";
            this.분봉_추매.Name = "분봉_추매";
            this.분봉_추매.ReadOnly = true;
            // 
            // 분봉_익절
            // 
            this.분봉_익절.HeaderText = "익절";
            this.분봉_익절.Name = "분봉_익절";
            this.분봉_익절.ReadOnly = true;
            // 
            // 분봉_손절
            // 
            this.분봉_손절.HeaderText = "손절";
            this.분봉_손절.Name = "분봉_손절";
            this.분봉_손절.ReadOnly = true;
            // 
            // indicatorViewButton
            // 
            this.indicatorViewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.indicatorViewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.indicatorViewButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.indicatorViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.indicatorViewButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.indicatorViewButton.ForeColor = System.Drawing.Color.White;
            this.indicatorViewButton.Location = new System.Drawing.Point(1064, 228);
            this.indicatorViewButton.Name = "indicatorViewButton";
            this.indicatorViewButton.Size = new System.Drawing.Size(112, 30);
            this.indicatorViewButton.TabIndex = 111;
            this.indicatorViewButton.Text = "지표계산보기 ▼";
            this.indicatorViewButton.UseVisualStyleBackColor = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            this.pictureBox10.Location = new System.Drawing.Point(908, 213);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(274, 1);
            this.pictureBox10.TabIndex = 102;
            this.pictureBox10.TabStop = false;
            // 
            // medoHiddenButton
            // 
            this.medoHiddenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.medoHiddenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.medoHiddenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.medoHiddenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.medoHiddenButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.medoHiddenButton.ForeColor = System.Drawing.Color.White;
            this.medoHiddenButton.Location = new System.Drawing.Point(937, 228);
            this.medoHiddenButton.Name = "medoHiddenButton";
            this.medoHiddenButton.Size = new System.Drawing.Size(112, 30);
            this.medoHiddenButton.TabIndex = 112;
            this.medoHiddenButton.Text = "매도종목 숨기기";
            this.medoHiddenButton.UseVisualStyleBackColor = false;
            // 
            // showEntireLogButton
            // 
            this.showEntireLogButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.showEntireLogButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.showEntireLogButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(139)))), ((int)(((byte)(181)))));
            this.showEntireLogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showEntireLogButton.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.showEntireLogButton.ForeColor = System.Drawing.Color.White;
            this.showEntireLogButton.Location = new System.Drawing.Point(810, 228);
            this.showEntireLogButton.Name = "showEntireLogButton";
            this.showEntireLogButton.Size = new System.Drawing.Size(112, 30);
            this.showEntireLogButton.TabIndex = 113;
            this.showEntireLogButton.Text = "전체 로그 보기";
            this.showEntireLogButton.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(217)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(1274, 961);
            this.Controls.Add(this.showEntireLogButton);
            this.Controls.Add(this.medoHiddenButton);
            this.Controls.Add(this.indicatorViewButton);
            this.Controls.Add(this.indicatorPictureBox);
            this.Controls.Add(this.indicatorLabel);
            this.Controls.Add(this.bunBongDataGridView);
            this.Controls.Add(this.totalRateOfReturnLabel);
            this.Controls.Add(this.totalProfitLossLabel);
            this.Controls.Add(this.totalEvalutionAmountLabel);
            this.Controls.Add(this.totalPurchaseAmountLabel);
            this.Controls.Add(this.curOrderAmountLabel);
            this.Controls.Add(this.pictureBox10);
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.soldDataGridView);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.conclusionDataGridView);
            this.Controls.Add(this.orderDataGridView);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tradingItemDataGridView);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tradingConditionDataGridView);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logListBox);
            this.Controls.Add(this.myAccountComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.KiwoomAPI);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "조건식 자동매매 프로그램";
            ((System.ComponentModel.ISupportInitialize)(this.KiwoomAPI)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradingConditionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradingItemDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conclusionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.soldDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.indicatorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bunBongDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Label label1;
        public AxKHOpenAPILib.AxKHOpenAPI KiwoomAPI;
        public System.Windows.Forms.ComboBox myAccountComboBox;
        private System.Windows.Forms.Button conditionSettingButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.DataGridView tradingConditionDataGridView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox3;
        public System.Windows.Forms.DataGridView tradingItemDataGridView;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.DataGridView orderDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_종목명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_조건식;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_주문번호;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_주문시간;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_주문량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_주문가격;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_매매구분;
        private System.Windows.Forms.DataGridViewTextBoxColumn 주문_가격구분;
        public System.Windows.Forms.DataGridView conclusionDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_종목명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_주문번호;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_주문시간;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_주문량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_체결량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_단위체결량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_체결가;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체결_매매구분;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox6;
        public System.Windows.Forms.DataGridView soldDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_종목명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_매도시간;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_매도량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_매도가격;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_평가손익;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매도_수익률;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox9;
        public System.Windows.Forms.Label curOrderAmountLabel;
        public System.Windows.Forms.Label totalPurchaseAmountLabel;
        public System.Windows.Forms.Label totalEvalutionAmountLabel;
        public System.Windows.Forms.Label totalProfitLossLabel;
        public System.Windows.Forms.Label totalRateOfReturnLabel;
        private System.Windows.Forms.Button profitLossButton;
        public System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_조건식;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_종목투자금;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_매수종목수;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_실매수종목수;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_매수타입;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_익절;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_손절;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매조건식_추매;
        private System.Windows.Forms.DataGridViewButtonColumn 매매조건식_상태;
        private System.Windows.Forms.DataGridViewButtonColumn 매매조건식_삭제;
        private System.Windows.Forms.PictureBox indicatorPictureBox;
        private System.Windows.Forms.Label indicatorLabel;
        public System.Windows.Forms.DataGridView bunBongDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 분봉_상태;
        private System.Windows.Forms.DataGridViewTextBoxColumn 분봉_매수;
        private System.Windows.Forms.DataGridViewTextBoxColumn 분봉_추매;
        private System.Windows.Forms.DataGridViewTextBoxColumn 분봉_익절;
        private System.Windows.Forms.DataGridViewTextBoxColumn 분봉_손절;
        public System.Windows.Forms.Button indicatorViewButton;
        private System.Windows.Forms.PictureBox pictureBox10;
        public System.Windows.Forms.Button medoHiddenButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_구분;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_종목명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_종목코드;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_조건식;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_총투자금;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_편입가격;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_편입대비수익률;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_매입금;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_보유수량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_주문가능수량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_매입가;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_현재가;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_평가손익;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_수익률;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_등락율;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_추매;
        private System.Windows.Forms.DataGridViewTextBoxColumn 매매진행_진행상황;
        private System.Windows.Forms.DataGridViewButtonColumn 매매진행_수동매수;
        private System.Windows.Forms.DataGridViewButtonColumn 매매진행_즉시매도;
        private System.Windows.Forms.Button holdingItemActivateButton;
        private System.Windows.Forms.Button holdingitemsettingButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        public System.Windows.Forms.Button showEntireLogButton;
    }
}

