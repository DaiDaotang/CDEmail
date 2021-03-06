﻿namespace CDEmail
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
            this.btnBack = new System.Windows.Forms.Button();
            this.tBody = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tFrom = new System.Windows.Forms.TextBox();
            this.tSubject = new System.Windows.Forms.TextBox();
            this.btnRcvClousure = new System.Windows.Forms.Button();
            this.btnNextMail = new System.Windows.Forms.Button();
            this.btnPrevMail = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(728, 6);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(110, 32);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "返回";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // tBody
            // 
            this.tBody.Location = new System.Drawing.Point(20, 80);
            this.tBody.Multiline = true;
            this.tBody.Name = "tBody";
            this.tBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tBody.Size = new System.Drawing.Size(818, 486);
            this.tBody.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "发信人";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "标题";
            // 
            // tFrom
            // 
            this.tFrom.Location = new System.Drawing.Point(86, 12);
            this.tFrom.Name = "tFrom";
            this.tFrom.ReadOnly = true;
            this.tFrom.Size = new System.Drawing.Size(440, 25);
            this.tFrom.TabIndex = 13;
            // 
            // tSubject
            // 
            this.tSubject.Location = new System.Drawing.Point(86, 48);
            this.tSubject.Name = "tSubject";
            this.tSubject.ReadOnly = true;
            this.tSubject.Size = new System.Drawing.Size(440, 25);
            this.tSubject.TabIndex = 14;
            // 
            // btnRcvClousure
            // 
            this.btnRcvClousure.Location = new System.Drawing.Point(571, 42);
            this.btnRcvClousure.Name = "btnRcvClousure";
            this.btnRcvClousure.Size = new System.Drawing.Size(110, 32);
            this.btnRcvClousure.TabIndex = 15;
            this.btnRcvClousure.Text = "附件接收";
            this.btnRcvClousure.UseVisualStyleBackColor = true;
            this.btnRcvClousure.Click += new System.EventHandler(this.btnRcvClousure_Click);
            // 
            // btnNextMail
            // 
            this.btnNextMail.Font = new System.Drawing.Font("宋体", 12F);
            this.btnNextMail.Location = new System.Drawing.Point(750, 584);
            this.btnNextMail.Name = "btnNextMail";
            this.btnNextMail.Size = new System.Drawing.Size(88, 33);
            this.btnNextMail.TabIndex = 16;
            this.btnNextMail.Text = "Next";
            this.btnNextMail.UseVisualStyleBackColor = true;
            this.btnNextMail.Click += new System.EventHandler(this.btnNextMail_Click);
            // 
            // btnPrevMail
            // 
            this.btnPrevMail.Font = new System.Drawing.Font("宋体", 12F);
            this.btnPrevMail.Location = new System.Drawing.Point(20, 584);
            this.btnPrevMail.Name = "btnPrevMail";
            this.btnPrevMail.Size = new System.Drawing.Size(91, 33);
            this.btnPrevMail.TabIndex = 17;
            this.btnPrevMail.Text = "Prev";
            this.btnPrevMail.UseVisualStyleBackColor = true;
            this.btnPrevMail.Click += new System.EventHandler(this.btnPrevMail_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(728, 42);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 32);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReply
            // 
            this.btnReply.Location = new System.Drawing.Point(571, 6);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(110, 32);
            this.btnReply.TabIndex = 19;
            this.btnReply.Text = "回复";
            this.btnReply.UseVisualStyleBackColor = true;
            this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // Receive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(850, 625);
            this.Controls.Add(this.btnReply);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnPrevMail);
            this.Controls.Add(this.btnNextMail);
            this.Controls.Add(this.btnRcvClousure);
            this.Controls.Add(this.tSubject);
            this.Controls.Add(this.tFrom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tBody);
            this.Controls.Add(this.btnBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Receive";
            this.Text = "Receive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TextBox tBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tFrom;
        private System.Windows.Forms.TextBox tSubject;
        private System.Windows.Forms.Button btnRcvClousure;
        private System.Windows.Forms.Button btnNextMail;
        private System.Windows.Forms.Button btnPrevMail;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReply;
    }
}