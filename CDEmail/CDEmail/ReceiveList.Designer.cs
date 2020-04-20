namespace CDEmail
{
    partial class ReceiveList
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
            this.label1 = new System.Windows.Forms.Label();
            this.tUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tPassword = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tPop3Server = new System.Windows.Forms.TextBox();
            this.tPop3Port = new System.Windows.Forms.TextBox();
            this.btnReadMail = new System.Windows.Forms.Button();
            this.btnDeleteMail = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.dgvMails = new System.Windows.Forms.DataGridView();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrePage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMails)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(404, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名";
            // 
            // tUsername
            // 
            this.tUsername.Font = new System.Drawing.Font("宋体", 12F);
            this.tUsername.Location = new System.Drawing.Point(507, 12);
            this.tUsername.Name = "tUsername";
            this.tUsername.Size = new System.Drawing.Size(202, 30);
            this.tUsername.TabIndex = 1;
            this.tUsername.Text = "whitecoffeesama@126.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(404, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码";
            // 
            // tPassword
            // 
            this.tPassword.Font = new System.Drawing.Font("宋体", 12F);
            this.tPassword.Location = new System.Drawing.Point(507, 68);
            this.tPassword.Name = "tPassword";
            this.tPassword.Size = new System.Drawing.Size(202, 30);
            this.tPassword.TabIndex = 6;
            this.tPassword.Text = "19990202hlx";
            this.tPassword.UseSystemPasswordChar = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F);
            this.button1.Location = new System.Drawing.Point(741, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 34);
            this.button1.TabIndex = 7;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(21, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "服务器";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(21, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "端口";
            // 
            // tPop3Server
            // 
            this.tPop3Server.Font = new System.Drawing.Font("宋体", 12F);
            this.tPop3Server.Location = new System.Drawing.Point(124, 12);
            this.tPop3Server.Name = "tPop3Server";
            this.tPop3Server.Size = new System.Drawing.Size(202, 30);
            this.tPop3Server.TabIndex = 10;
            this.tPop3Server.Text = "pop.126.com";
            // 
            // tPop3Port
            // 
            this.tPop3Port.Font = new System.Drawing.Font("宋体", 12F);
            this.tPop3Port.Location = new System.Drawing.Point(124, 68);
            this.tPop3Port.Name = "tPop3Port";
            this.tPop3Port.Size = new System.Drawing.Size(202, 30);
            this.tPop3Port.TabIndex = 11;
            this.tPop3Port.Text = "110";
            // 
            // btnReadMail
            // 
            this.btnReadMail.Font = new System.Drawing.Font("宋体", 12F);
            this.btnReadMail.Location = new System.Drawing.Point(741, 131);
            this.btnReadMail.Name = "btnReadMail";
            this.btnReadMail.Size = new System.Drawing.Size(97, 34);
            this.btnReadMail.TabIndex = 13;
            this.btnReadMail.Text = "查看";
            this.btnReadMail.UseVisualStyleBackColor = true;
            this.btnReadMail.Click += new System.EventHandler(this.btnReadMail_Click);
            // 
            // btnDeleteMail
            // 
            this.btnDeleteMail.Font = new System.Drawing.Font("宋体", 12F);
            this.btnDeleteMail.Location = new System.Drawing.Point(741, 171);
            this.btnDeleteMail.Name = "btnDeleteMail";
            this.btnDeleteMail.Size = new System.Drawing.Size(97, 34);
            this.btnDeleteMail.TabIndex = 14;
            this.btnDeleteMail.Text = "删除";
            this.btnDeleteMail.UseVisualStyleBackColor = true;
            this.btnDeleteMail.Click += new System.EventHandler(this.btnDeleteMail_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(640, 290);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(8, 4);
            this.checkedListBox1.TabIndex = 15;
            // 
            // dgvMails
            // 
            this.dgvMails.AllowUserToAddRows = false;
            this.dgvMails.AllowUserToDeleteRows = false;
            this.dgvMails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.Subject,
            this.From,
            this.Date,
            this.To,
            this.Uid});
            this.dgvMails.Location = new System.Drawing.Point(25, 131);
            this.dgvMails.Name = "dgvMails";
            this.dgvMails.ReadOnly = true;
            this.dgvMails.RowHeadersWidth = 51;
            this.dgvMails.RowTemplate.Height = 27;
            this.dgvMails.Size = new System.Drawing.Size(684, 463);
            this.dgvMails.TabIndex = 17;
            // 
            // Num
            // 
            this.Num.DataPropertyName = "Num";
            this.Num.HeaderText = "Num";
            this.Num.MinimumWidth = 6;
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.Visible = false;
            this.Num.Width = 125;
            // 
            // Subject
            // 
            this.Subject.DataPropertyName = "Subject";
            this.Subject.HeaderText = "主题";
            this.Subject.MinimumWidth = 6;
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            this.Subject.Width = 200;
            // 
            // From
            // 
            this.From.DataPropertyName = "From";
            this.From.HeaderText = "发送人";
            this.From.MinimumWidth = 6;
            this.From.Name = "From";
            this.From.ReadOnly = true;
            this.From.Width = 175;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "时间";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 150;
            // 
            // To
            // 
            this.To.DataPropertyName = "To";
            this.To.HeaderText = "To";
            this.To.MinimumWidth = 6;
            this.To.Name = "To";
            this.To.ReadOnly = true;
            this.To.Visible = false;
            this.To.Width = 125;
            // 
            // Uid
            // 
            this.Uid.DataPropertyName = "Uid";
            this.Uid.HeaderText = "Uid";
            this.Uid.MinimumWidth = 6;
            this.Uid.Name = "Uid";
            this.Uid.ReadOnly = true;
            this.Uid.Visible = false;
            this.Uid.Width = 125;
            // 
            // btnPrePage
            // 
            this.btnPrePage.Font = new System.Drawing.Font("宋体", 12F);
            this.btnPrePage.Location = new System.Drawing.Point(741, 518);
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(97, 35);
            this.btnPrePage.TabIndex = 18;
            this.btnPrePage.Text = "上一页";
            this.btnPrePage.UseVisualStyleBackColor = true;
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("宋体", 12F);
            this.btnNextPage.Location = new System.Drawing.Point(741, 559);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(97, 35);
            this.btnNextPage.TabIndex = 19;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // ReceiveList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 625);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrePage);
            this.Controls.Add(this.dgvMails);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btnDeleteMail);
            this.Controls.Add(this.btnReadMail);
            this.Controls.Add(this.tPop3Port);
            this.Controls.Add(this.tPop3Server);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tUsername);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReceiveList";
            this.Text = "ReceiveList";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tPassword;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tPop3Server;
        private System.Windows.Forms.TextBox tPop3Port;
        private System.Windows.Forms.Button btnReadMail;
        private System.Windows.Forms.Button btnDeleteMail;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.DataGridView dgvMails;
        private System.Windows.Forms.Button btnPrePage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uid;
    }
}