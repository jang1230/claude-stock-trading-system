
namespace StockAutoTrade2
{
    partial class ConditionLoadDialog
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
            this.jogunDeleteButton = new System.Windows.Forms.Button();
            this.jogunLoadingButton = new System.Windows.Forms.Button();
            this.jogunSaveLoadDataGridView = new System.Windows.Forms.DataGridView();
            this.조건식로딩_조건식명 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.조건식로딩_조건식번호 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.jogunSaveLoadDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // jogunDeleteButton
            // 
            this.jogunDeleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(121)))), ((int)(((byte)(157)))));
            this.jogunDeleteButton.ForeColor = System.Drawing.Color.White;
            this.jogunDeleteButton.Location = new System.Drawing.Point(10, 265);
            this.jogunDeleteButton.Name = "jogunDeleteButton";
            this.jogunDeleteButton.Size = new System.Drawing.Size(144, 36);
            this.jogunDeleteButton.TabIndex = 0;
            this.jogunDeleteButton.Text = "삭제하기";
            this.jogunDeleteButton.UseVisualStyleBackColor = false;
            // 
            // jogunLoadingButton
            // 
            this.jogunLoadingButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(121)))), ((int)(((byte)(157)))));
            this.jogunLoadingButton.ForeColor = System.Drawing.Color.White;
            this.jogunLoadingButton.Location = new System.Drawing.Point(160, 265);
            this.jogunLoadingButton.Name = "jogunLoadingButton";
            this.jogunLoadingButton.Size = new System.Drawing.Size(144, 36);
            this.jogunLoadingButton.TabIndex = 1;
            this.jogunLoadingButton.Text = "불러오기";
            this.jogunLoadingButton.UseVisualStyleBackColor = false;
            // 
            // jogunSaveLoadDataGridView
            // 
            this.jogunSaveLoadDataGridView.AllowUserToAddRows = false;
            this.jogunSaveLoadDataGridView.AllowUserToDeleteRows = false;
            this.jogunSaveLoadDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.jogunSaveLoadDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.jogunSaveLoadDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(112)))), ((int)(((byte)(149)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.jogunSaveLoadDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.jogunSaveLoadDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.jogunSaveLoadDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.조건식로딩_조건식명,
            this.조건식로딩_조건식번호});
            this.jogunSaveLoadDataGridView.EnableHeadersVisualStyles = false;
            this.jogunSaveLoadDataGridView.GridColor = System.Drawing.Color.Silver;
            this.jogunSaveLoadDataGridView.Location = new System.Drawing.Point(12, 12);
            this.jogunSaveLoadDataGridView.Name = "jogunSaveLoadDataGridView";
            this.jogunSaveLoadDataGridView.RowHeadersVisible = false;
            this.jogunSaveLoadDataGridView.RowTemplate.Height = 23;
            this.jogunSaveLoadDataGridView.Size = new System.Drawing.Size(292, 247);
            this.jogunSaveLoadDataGridView.TabIndex = 2;
            this.jogunSaveLoadDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jogunSaveLoadDataGridView_CellClick);
            // 
            // 조건식로딩_조건식명
            // 
            this.조건식로딩_조건식명.FillWeight = 121.8274F;
            this.조건식로딩_조건식명.HeaderText = "조건식명";
            this.조건식로딩_조건식명.Name = "조건식로딩_조건식명";
            this.조건식로딩_조건식명.ReadOnly = true;
            this.조건식로딩_조건식명.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 조건식로딩_조건식번호
            // 
            this.조건식로딩_조건식번호.FillWeight = 78.17259F;
            this.조건식로딩_조건식번호.HeaderText = "조건식번호";
            this.조건식로딩_조건식번호.Name = "조건식로딩_조건식번호";
            this.조건식로딩_조건식번호.ReadOnly = true;
            this.조건식로딩_조건식번호.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ConditionLoadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 308);
            this.Controls.Add(this.jogunSaveLoadDataGridView);
            this.Controls.Add(this.jogunLoadingButton);
            this.Controls.Add(this.jogunDeleteButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConditionLoadDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "조건식 매매 방식 불러오기";
            ((System.ComponentModel.ISupportInitialize)(this.jogunSaveLoadDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button jogunDeleteButton;
        private System.Windows.Forms.Button jogunLoadingButton;
        public System.Windows.Forms.DataGridView jogunSaveLoadDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn 조건식로딩_조건식명;
        private System.Windows.Forms.DataGridViewTextBoxColumn 조건식로딩_조건식번호;
    }
}