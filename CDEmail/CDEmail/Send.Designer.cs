namespace CDEmail
{
    partial class Send
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
            this.btn_send = new System.Windows.Forms.Button();
            this.tb_server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_to = new System.Windows.Forms.TextBox();
            this.tb_subject = new System.Windows.Forms.TextBox();
            this.tb_content = new System.Windows.Forms.TextBox();
            this.Filebutton = new System.Windows.Forms.Button();
            this.Deletebutton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lsb_status = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_conn = new System.Windows.Forms.Button();
            this.tb_from = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Enabled = false;
            this.btn_send.Location = new System.Drawing.Point(500, 274);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(295, 40);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "发送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.Sendbutton_Click);
            // 
            // tb_server
            // 
            this.tb_server.Location = new System.Drawing.Point(12, 125);
            this.tb_server.Name = "tb_server";
            this.tb_server.Size = new System.Drawing.Size(100, 25);
            this.tb_server.TabIndex = 1;
            this.tb_server.Text = "smtp.126.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "SMTP服务器";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "用户名";
            // 
            // tb_username
            // 
            this.tb_username.Location = new System.Drawing.Point(12, 214);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(100, 25);
            this.tb_username.TabIndex = 6;
            this.tb_username.Text = "whuddt@126.com";
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(12, 302);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(100, 25);
            this.tb_password.TabIndex = 7;
            this.tb_password.Text = "GWBMFFULQFZQCALI";
            this.tb_password.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "密码";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(274, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "收信人邮箱";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(548, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "标题";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "附件";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(216, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 12;
            this.label8.Text = "正文";
            // 
            // tb_to
            // 
            this.tb_to.Location = new System.Drawing.Point(379, 24);
            this.tb_to.Name = "tb_to";
            this.tb_to.Size = new System.Drawing.Size(155, 25);
            this.tb_to.TabIndex = 13;
            this.tb_to.Text = "1025563447@qq.com";
            // 
            // tb_subject
            // 
            this.tb_subject.Location = new System.Drawing.Point(600, 24);
            this.tb_subject.Name = "tb_subject";
            this.tb_subject.Size = new System.Drawing.Size(147, 25);
            this.tb_subject.TabIndex = 14;
            this.tb_subject.Text = "测试邮件";
            // 
            // tb_content
            // 
            this.tb_content.Location = new System.Drawing.Point(219, 156);
            this.tb_content.Multiline = true;
            this.tb_content.Name = "tb_content";
            this.tb_content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_content.Size = new System.Drawing.Size(576, 93);
            this.tb_content.TabIndex = 16;
            this.tb_content.Text = "whitecoffeesama@126.com\\r\\n看一看能不能成功发送邮件奥利给";
            // 
            // Filebutton
            // 
            this.Filebutton.Location = new System.Drawing.Point(569, 67);
            this.Filebutton.Name = "Filebutton";
            this.Filebutton.Size = new System.Drawing.Size(75, 23);
            this.Filebutton.TabIndex = 19;
            this.Filebutton.Text = "浏览";
            this.Filebutton.UseVisualStyleBackColor = true;
            this.Filebutton.Click += new System.EventHandler(this.Filebutton_Click);
            // 
            // Deletebutton
            // 
            this.Deletebutton.Location = new System.Drawing.Point(672, 66);
            this.Deletebutton.Name = "Deletebutton";
            this.Deletebutton.Size = new System.Drawing.Size(75, 23);
            this.Deletebutton.TabIndex = 20;
            this.Deletebutton.Text = "删除";
            this.Deletebutton.UseVisualStyleBackColor = true;
            this.Deletebutton.Click += new System.EventHandler(this.Deletebutton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(259, 67);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(275, 23);
            this.comboBox1.TabIndex = 21;
            // 
            // lsb_status
            // 
            this.lsb_status.FormattingEnabled = true;
            this.lsb_status.ItemHeight = 15;
            this.lsb_status.Location = new System.Drawing.Point(219, 367);
            this.lsb_status.Name = "lsb_status";
            this.lsb_status.Size = new System.Drawing.Size(576, 124);
            this.lsb_status.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "状态";
            // 
            // btn_conn
            // 
            this.btn_conn.Location = new System.Drawing.Point(12, 385);
            this.btn_conn.Name = "btn_conn";
            this.btn_conn.Size = new System.Drawing.Size(75, 23);
            this.btn_conn.TabIndex = 24;
            this.btn_conn.Text = "连接";
            this.btn_conn.UseVisualStyleBackColor = true;
            this.btn_conn.Click += new System.EventHandler(this.btn_conn_Click);
            // 
            // tb_from
            // 
            this.tb_from.Location = new System.Drawing.Point(93, 24);
            this.tb_from.Name = "tb_from";
            this.tb_from.Size = new System.Drawing.Size(160, 25);
            this.tb_from.TabIndex = 25;
            this.tb_from.Text = "whuddt@126.com";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 26;
            this.label9.Text = "发件人邮箱";
            // 
            // Send
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(850, 625);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_from);
            this.Controls.Add(this.btn_conn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lsb_status);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Deletebutton);
            this.Controls.Add(this.Filebutton);
            this.Controls.Add(this.tb_content);
            this.Controls.Add(this.tb_subject);
            this.Controls.Add(this.tb_to);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_server);
            this.Controls.Add(this.btn_send);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Send";
            this.Text = "Send";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox tb_server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_to;
        private System.Windows.Forms.TextBox tb_subject;
        private System.Windows.Forms.TextBox tb_content;
        private System.Windows.Forms.Button Filebutton;
        private System.Windows.Forms.Button Deletebutton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox lsb_status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_conn;
        private System.Windows.Forms.TextBox tb_from;
        private System.Windows.Forms.Label label9;
    }
}