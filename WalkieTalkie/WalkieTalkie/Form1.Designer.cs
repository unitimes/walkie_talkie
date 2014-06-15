namespace WalkieTalkie
{
    partial class Form1
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
            this.lpl_ip = new System.Windows.Forms.Label();
            this.tBoxIP = new System.Windows.Forms.TextBox();
            this.btn_cnt = new System.Windows.Forms.Button();
            this.btn_dcnt = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lbl_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lpl_ip
            // 
            this.lpl_ip.AutoSize = true;
            this.lpl_ip.Location = new System.Drawing.Point(12, 9);
            this.lpl_ip.Name = "lpl_ip";
            this.lpl_ip.Size = new System.Drawing.Size(56, 12);
            this.lpl_ip.TabIndex = 0;
            this.lpl_ip.Text = "상대방 IP";
            // 
            // tBoxIP
            // 
            this.tBoxIP.Location = new System.Drawing.Point(84, 12);
            this.tBoxIP.Name = "tBoxIP";
            this.tBoxIP.Size = new System.Drawing.Size(178, 21);
            this.tBoxIP.TabIndex = 3;
            this.tBoxIP.Text = "127.0.0.1";
            // 
            // btn_cnt
            // 
            this.btn_cnt.Location = new System.Drawing.Point(14, 107);
            this.btn_cnt.Name = "btn_cnt";
            this.btn_cnt.Size = new System.Drawing.Size(119, 23);
            this.btn_cnt.TabIndex = 6;
            this.btn_cnt.Text = "Connect";
            this.btn_cnt.UseVisualStyleBackColor = true;
            this.btn_cnt.Click += new System.EventHandler(this.btn_cnt_Click);
            // 
            // btn_dcnt
            // 
            this.btn_dcnt.Location = new System.Drawing.Point(142, 107);
            this.btn_dcnt.Name = "btn_dcnt";
            this.btn_dcnt.Size = new System.Drawing.Size(123, 23);
            this.btn_dcnt.TabIndex = 7;
            this.btn_dcnt.Text = "Disconnect";
            this.btn_dcnt.UseVisualStyleBackColor = true;
            this.btn_dcnt.Click += new System.EventHandler(this.btn_dcnt_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Location = new System.Drawing.Point(12, 136);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(253, 76);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Speak";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(157, 215);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(108, 12);
            this.lbl_status.TabIndex = 15;
            this.lbl_status.Text = "Connection Status";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 236);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btn_dcnt);
            this.Controls.Add(this.btn_cnt);
            this.Controls.Add(this.tBoxIP);
            this.Controls.Add(this.lpl_ip);
            this.Name = "Form1";
            this.Text = "WalkieTalkie";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lpl_ip;
        private System.Windows.Forms.TextBox tBoxIP;
        private System.Windows.Forms.Button btn_cnt;
        private System.Windows.Forms.Button btn_dcnt;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lbl_status;
    }
}

