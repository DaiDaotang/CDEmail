namespace CDEmail
{
    partial class Receive
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tPop3Server = new System.Windows.Forms.TextBox();
            this.tUsername = new System.Windows.Forms.TextBox();
            this.tPassword = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnReadMail = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.btnRcvClousure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "POP3服务器";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 187);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "密码";
            // 
            // tPop3Server
            // 
            this.tPop3Server.Location = new System.Drawing.Point(55, 71);
            this.tPop3Server.Name = "tPop3Server";
            this.tPop3Server.Size = new System.Drawing.Size(100, 25);
            this.tPop3Server.TabIndex = 3;
            this.tPop3Server.Text = "pop.126.com";
            // 
            // tUsername
            // 
            this.tUsername.Location = new System.Drawing.Point(55, 139);
            this.tUsername.Name = "tUsername";
            this.tUsername.Size = new System.Drawing.Size(100, 25);
            this.tUsername.TabIndex = 4;
            this.tUsername.Text = "whitecoffeesama@126.com";
            // 
            // tPassword
            // 
            this.tPassword.Location = new System.Drawing.Point(55, 225);
            this.tPassword.Name = "tPassword";
            this.tPassword.Size = new System.Drawing.Size(100, 25);
            this.tPassword.TabIndex = 5;
            this.tPassword.Text = "19990202hlx";
            this.tPassword.UseSystemPasswordChar = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(55, 321);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(55, 411);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(212, 258);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox4.Size = new System.Drawing.Size(579, 239);
            this.textBox4.TabIndex = 8;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(212, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(387, 23);
            this.comboBox1.TabIndex = 9;
            // 
            // btnReadMail
            // 
            this.btnReadMail.Location = new System.Drawing.Point(615, 40);
            this.btnReadMail.Name = "btnReadMail";
            this.btnReadMail.Size = new System.Drawing.Size(110, 32);
            this.btnReadMail.TabIndex = 10;
            this.btnReadMail.Text = "查看邮件";
            this.btnReadMail.UseVisualStyleBackColor = true;
            this.btnReadMail.Click += new System.EventHandler(this.btnReadMail_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "发信人";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "标题";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(284, 105);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 25);
            this.textBox5.TabIndex = 13;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(284, 150);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(100, 25);
            this.textBox6.TabIndex = 14;
            // 
            // btnRcvClousure
            // 
            this.btnRcvClousure.Location = new System.Drawing.Point(490, 108);
            this.btnRcvClousure.Name = "btnRcvClousure";
            this.btnRcvClousure.Size = new System.Drawing.Size(143, 60);
            this.btnRcvClousure.TabIndex = 15;
            this.btnRcvClousure.Text = "附件接收";
            this.btnRcvClousure.UseVisualStyleBackColor = true;
            this.btnRcvClousure.Click += new System.EventHandler(this.btnRcvClousure_Click);
            // 
            // Receive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(850, 625);
            this.Controls.Add(this.btnRcvClousure);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnReadMail);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tPassword);
            this.Controls.Add(this.tUsername);
            this.Controls.Add(this.tPop3Server);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Receive";
            this.Text = "Receive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tPop3Server;
        private System.Windows.Forms.TextBox tUsername;
        private System.Windows.Forms.TextBox tPassword;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnReadMail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button btnRcvClousure;
    }
}