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
            this.lbl_rev_port = new System.Windows.Forms.Label();
            this.lbl_sed_port = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btn_cnt = new System.Windows.Forms.Button();
            this.btn_dcnt = new System.Windows.Forms.Button();
            this.btn_spk = new System.Windows.Forms.Button();
            this.lbl_test = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
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
            // lbl_rev_port
            // 
            this.lbl_rev_port.AutoSize = true;
            this.lbl_rev_port.Location = new System.Drawing.Point(12, 45);
            this.lbl_rev_port.Name = "lbl_rev_port";
            this.lbl_rev_port.Size = new System.Drawing.Size(57, 12);
            this.lbl_rev_port.TabIndex = 1;
            this.lbl_rev_port.Text = "수신 포트";
            // 
            // lbl_sed_port
            // 
            this.lbl_sed_port.AutoSize = true;
            this.lbl_sed_port.Location = new System.Drawing.Point(12, 83);
            this.lbl_sed_port.Name = "lbl_sed_port";
            this.lbl_sed_port.Size = new System.Drawing.Size(57, 12);
            this.lbl_sed_port.TabIndex = 2;
            this.lbl_sed_port.Text = "송신 포트";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 21);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(162, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(162, 80);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 21);
            this.textBox3.TabIndex = 5;
            // 
            // btn_cnt
            // 
            this.btn_cnt.Location = new System.Drawing.Point(24, 132);
            this.btn_cnt.Name = "btn_cnt";
            this.btn_cnt.Size = new System.Drawing.Size(75, 23);
            this.btn_cnt.TabIndex = 6;
            this.btn_cnt.Text = "Connect";
            this.btn_cnt.UseVisualStyleBackColor = true;
            this.btn_cnt.Click += new System.EventHandler(this.btn_cnt_Click);
            // 
            // btn_dcnt
            // 
            this.btn_dcnt.Location = new System.Drawing.Point(105, 132);
            this.btn_dcnt.Name = "btn_dcnt";
            this.btn_dcnt.Size = new System.Drawing.Size(99, 23);
            this.btn_dcnt.TabIndex = 7;
            this.btn_dcnt.Text = "Disconnect";
            this.btn_dcnt.UseVisualStyleBackColor = true;
            this.btn_dcnt.Click += new System.EventHandler(this.btn_dcnt_Click);
            // 
            // btn_spk
            // 
            this.btn_spk.Location = new System.Drawing.Point(24, 162);
            this.btn_spk.Name = "btn_spk";
            this.btn_spk.Size = new System.Drawing.Size(238, 88);
            this.btn_spk.TabIndex = 8;
            this.btn_spk.Text = "Speaking";
            this.btn_spk.UseVisualStyleBackColor = true;
            // 
            // lbl_test
            // 
            this.lbl_test.AutoSize = true;
            this.lbl_test.Location = new System.Drawing.Point(12, 842);
            this.lbl_test.Name = "lbl_test";
            this.lbl_test.Size = new System.Drawing.Size(81, 12);
            this.lbl_test.TabIndex = 9;
            this.lbl_test.Text = "disconnected";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(14, 256);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(251, 583);
            this.textBox4.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 863);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.lbl_test);
            this.Controls.Add(this.btn_spk);
            this.Controls.Add(this.btn_dcnt);
            this.Controls.Add(this.btn_cnt);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbl_sed_port);
            this.Controls.Add(this.lbl_rev_port);
            this.Controls.Add(this.lpl_ip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lpl_ip;
        private System.Windows.Forms.Label lbl_rev_port;
        private System.Windows.Forms.Label lbl_sed_port;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btn_cnt;
        private System.Windows.Forms.Button btn_dcnt;
        private System.Windows.Forms.Button btn_spk;
        private System.Windows.Forms.Label lbl_test;
        private System.Windows.Forms.TextBox textBox4;
    }
}

