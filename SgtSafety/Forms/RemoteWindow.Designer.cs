﻿namespace SgtSafety.Forms
{
    partial class RemoteWindow
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(12, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "AVANCER";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(88, 61);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 45);
            this.button2.TabIndex = 1;
            this.button2.Text = "DROITE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(12, 61);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(70, 45);
            this.button3.TabIndex = 2;
            this.button3.Text = "GAUCHE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(12, 112);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(146, 45);
            this.button4.TabIndex = 3;
            this.button4.Text = "DEMI TOUR";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(164, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 191);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Buffer";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(121, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(227, 173);
            this.listBox1.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(109, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "LIFO (sinon FIFO)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(12, 163);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(70, 30);
            this.button5.TabIndex = 6;
            this.button5.Text = "TAKE";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(88, 163);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(70, 30);
            this.button6.TabIndex = 5;
            this.button6.Text = "DROP";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(6, 155);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(109, 30);
            this.button7.TabIndex = 7;
            this.button7.Text = "Supprimer action";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // button10
            // 
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Location = new System.Drawing.Point(6, 37);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(109, 30);
            this.button10.TabIndex = 10;
            this.button10.Text = "Lancer";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(6, 119);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(109, 30);
            this.button8.TabIndex = 11;
            this.button8.Text = "Supprimer tout";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // RemoteWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 199);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RemoteWindow";
            this.ShowIcon = false;
            this.Text = "Télécommande";
            this.Load += new System.EventHandler(this.RemoteWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button8;
    }
}