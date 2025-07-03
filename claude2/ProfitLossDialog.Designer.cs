
namespace StockAutoTrade2
{
    partial class ProfitLossDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.totalRateOfReturnDlgLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.totalProfitLossDlgLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.profitLossDataGirdView = new System.Windows.Forms.DataGridView();
            this.실현손익_매매일 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_종목명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_종목코드 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_매수금 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_매도금 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_매도량 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_실현손익 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.실현손익_수익률 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.profitLossDataGirdView)).BeginInit();
            this.SuspendLayout();
            // 
            // totalRateOfReturnDlgLabel
            // 
            this.totalRateOfReturnDlgLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalRateOfReturnDlgLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalRateOfReturnDlgLabel.ForeColor = System.Drawing.Color.Black;
            this.totalRateOfReturnDlgLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalRateOfReturnDlgLabel.Location = new System.Drawing.Point(575, 6);
            this.totalRateOfReturnDlgLabel.Name = "totalRateOfReturnDlgLabel";
            this.totalRateOfReturnDlgLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalRateOfReturnDlgLabel.Size = new System.Drawing.Size(64, 19);
            this.totalRateOfReturnDlgLabel.TabIndex = 93;
            this.totalRateOfReturnDlgLabel.Text = "0.00%";
            this.totalRateOfReturnDlgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(525, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 92;
            this.label3.Text = "총수익률";
            // 
            // totalProfitLossDlgLabel
            // 
            this.totalProfitLossDlgLabel.BackColor = System.Drawing.Color.Transparent;
            this.totalProfitLossDlgLabel.Font = new System.Drawing.Font("굴림", 9.25F);
            this.totalProfitLossDlgLabel.ForeColor = System.Drawing.Color.Black;
            this.totalProfitLossDlgLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalProfitLossDlgLabel.Location = new System.Drawing.Point(415, 6);
            this.totalProfitLossDlgLabel.Name = "totalProfitLossDlgLabel";
            this.totalProfitLossDlgLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.totalProfitLossDlgLabel.Size = new System.Drawing.Size(86, 19);
            this.totalProfitLossDlgLabel.TabIndex = 91;
            this.totalProfitLossDlgLabel.Text = "0";
            this.totalProfitLossDlgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(353, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 90;
            this.label1.Text = "총실현손익";
            // 
            // profitLossDataGirdView
            // 
            this.profitLossDataGirdView.AllowUserToAddRows = false;
            this.profitLossDataGirdView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Cornsilk;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.profitLossDataGirdView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.profitLossDataGirdView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.profitLossDataGirdView.BackgroundColor = System.Drawing.Color.White;
            this.profitLossDataGirdView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.profitLossDataGirdView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.profitLossDataGirdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.profitLossDataGirdView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.실현손익_매매일,
            this.실현손익_종목명,
            this.실현손익_종목코드,
            this.실현손익_매수금,
            this.실현손익_매도금,
            this.실현손익_매도량,
            this.실현손익_실현손익,
            this.실현손익_수익률});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.profitLossDataGirdView.DefaultCellStyle = dataGridViewCellStyle3;
            this.profitLossDataGirdView.EnableHeadersVisualStyles = false;
            this.profitLossDataGirdView.GridColor = System.Drawing.Color.Silver;
            this.profitLossDataGirdView.Location = new System.Drawing.Point(7, 32);
            this.profitLossDataGirdView.Name = "profitLossDataGirdView";
            this.profitLossDataGirdView.RowHeadersVisible = false;
            this.profitLossDataGirdView.RowTemplate.Height = 23;
            this.profitLossDataGirdView.Size = new System.Drawing.Size(634, 413);
            this.profitLossDataGirdView.TabIndex = 94;
            // 
            // 실현손익_매매일
            // 
            this.실현손익_매매일.HeaderText = "매매일";
            this.실현손익_매매일.Name = "실현손익_매매일";
            this.실현손익_매매일.ReadOnly = true;
            // 
            // 실현손익_종목명
            // 
            this.실현손익_종목명.FillWeight = 130F;
            this.실현손익_종목명.HeaderText = "종목명";
            this.실현손익_종목명.Name = "실현손익_종목명";
            this.실현손익_종목명.ReadOnly = true;
            // 
            // 실현손익_종목코드
            // 
            this.실현손익_종목코드.HeaderText = "종목코드";
            this.실현손익_종목코드.Name = "실현손익_종목코드";
            this.실현손익_종목코드.ReadOnly = true;
            this.실현손익_종목코드.Visible = false;
            // 
            // 실현손익_매수금
            // 
            this.실현손익_매수금.HeaderText = "매수금";
            this.실현손익_매수금.Name = "실현손익_매수금";
            this.실현손익_매수금.ReadOnly = true;
            // 
            // 실현손익_매도금
            // 
            this.실현손익_매도금.HeaderText = "매도금";
            this.실현손익_매도금.Name = "실현손익_매도금";
            this.실현손익_매도금.ReadOnly = true;
            // 
            // 실현손익_매도량
            // 
            this.실현손익_매도량.FillWeight = 90F;
            this.실현손익_매도량.HeaderText = "매도량";
            this.실현손익_매도량.Name = "실현손익_매도량";
            this.실현손익_매도량.ReadOnly = true;
            // 
            // 실현손익_실현손익
            // 
            this.실현손익_실현손익.HeaderText = "실현손익";
            this.실현손익_실현손익.Name = "실현손익_실현손익";
            this.실현손익_실현손익.ReadOnly = true;
            // 
            // 실현손익_수익률
            // 
            this.실현손익_수익률.HeaderText = "수익률";
            this.실현손익_수익률.Name = "실현손익_수익률";
            this.실현손익_수익률.ReadOnly = true;
            // 
            // ProfitLossDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(217)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(648, 450);
            this.Controls.Add(this.profitLossDataGirdView);
            this.Controls.Add(this.totalRateOfReturnDlgLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.totalProfitLossDlgLabel);
            this.Controls.Add(this.label1);
            this.Name = "ProfitLossDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "총실현손익";
            ((System.ComponentModel.ISupportInitialize)(this.profitLossDataGirdView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label totalRateOfReturnDlgLabel;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label totalProfitLossDlgLabel;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView profitLossDataGirdView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_매매일;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_종목명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_종목코드;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_매수금;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_매도금;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_매도량;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_실현손익;
        private System.Windows.Forms.DataGridViewTextBoxColumn 실현손익_수익률;
    }
}