
namespace HttpKeyboardMouse
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.HttpKeyboardMouse = new System.Windows.Forms.NotifyIcon(this.components);
            this.LabelComputerName = new System.Windows.Forms.Label();
            this.LinkLabelComputerName = new System.Windows.Forms.LinkLabel();
            this.TextLog = new System.Windows.Forms.TextBox();
            this.BtnClearLog = new System.Windows.Forms.Button();
            this.LinkLabelIP = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // HttpKeyboardMouse
            // 
            this.HttpKeyboardMouse.Icon = ((System.Drawing.Icon)(resources.GetObject("HttpKeyboardMouse.Icon")));
            this.HttpKeyboardMouse.Text = "Http Keyboard Mouse";
            this.HttpKeyboardMouse.Visible = true;
            this.HttpKeyboardMouse.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // label1
            // 
            this.LabelComputerName.AutoSize = true;
            this.LabelComputerName.Location = new System.Drawing.Point(12, 9);
            this.LabelComputerName.Name = "label1";
            this.LabelComputerName.Size = new System.Drawing.Size(97, 15);
            this.LabelComputerName.TabIndex = 0;
            this.LabelComputerName.Text = "Computer name:";
            // 
            // LinkLabelComputerName
            // 
            this.LinkLabelComputerName.AutoSize = true;
            this.LinkLabelComputerName.Location = new System.Drawing.Point(115, 9);
            this.LinkLabelComputerName.Name = "LinkLabelComputerName";
            this.LinkLabelComputerName.Size = new System.Drawing.Size(117, 15);
            this.LinkLabelComputerName.TabIndex = 1;
            this.LinkLabelComputerName.TabStop = true;
            this.LinkLabelComputerName.Text = "http://localhost:8080";
            this.LinkLabelComputerName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelComputerName_LinkClicked);
            // 
            // TextLog
            // 
            this.TextLog.BackColor = System.Drawing.Color.White;
            this.TextLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextLog.Location = new System.Drawing.Point(15, 48);
            this.TextLog.Multiline = true;
            this.TextLog.Name = "textBox1";
            this.TextLog.ReadOnly = true;
            this.TextLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextLog.Size = new System.Drawing.Size(681, 351);
            this.TextLog.TabIndex = 2;
            this.TextLog.WordWrap = false;
            // 
            // BtnClearLog
            // 
            this.BtnClearLog.Location = new System.Drawing.Point(621, 5);
            this.BtnClearLog.Name = "BtnClearLog";
            this.BtnClearLog.Size = new System.Drawing.Size(75, 23);
            this.BtnClearLog.TabIndex = 3;
            this.BtnClearLog.Text = "Clear log";
            this.BtnClearLog.UseVisualStyleBackColor = true;
            this.BtnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // LinkLabelIP
            // 
            this.LinkLabelIP.AutoSize = true;
            this.LinkLabelIP.Location = new System.Drawing.Point(115, 30);
            this.LinkLabelIP.Name = "LinkLabelIP";
            this.LinkLabelIP.Size = new System.Drawing.Size(119, 15);
            this.LinkLabelIP.TabIndex = 5;
            this.LinkLabelIP.TabStop = true;
            this.LinkLabelIP.Text = "http://127.0.0.1:8080/";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 411);
            this.Controls.Add(this.LinkLabelIP);
            this.Controls.Add(this.BtnClearLog);
            this.Controls.Add(this.TextLog);
            this.Controls.Add(this.LinkLabelComputerName);
            this.Controls.Add(this.LabelComputerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Http Keyboard Mouse";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon HttpKeyboardMouse;
        private Label LabelComputerName;
        private LinkLabel LinkLabelComputerName;
        private TextBox TextLog;
        private Button BtnClearLog;
        private LinkLabel LinkLabelIP;
    }
}