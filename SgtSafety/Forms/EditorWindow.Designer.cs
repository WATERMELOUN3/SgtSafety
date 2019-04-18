namespace SgtSafety.Forms
{
    partial class EditorWindow
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
            this.drawEditor1 = new SgtSafety.Forms.DrawEditor();
            this.SuspendLayout();
            // 
            // drawEditor1
            // 
            this.drawEditor1.Location = new System.Drawing.Point(82, 12);
            this.drawEditor1.Name = "drawEditor1";
            this.drawEditor1.Size = new System.Drawing.Size(706, 426);
            this.drawEditor1.TabIndex = 0;
            this.drawEditor1.Text = "drawEditor1";
            // 
            // EditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.drawEditor1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditorWindow";
            this.ShowIcon = false;
            this.Text = "Éditeur de circuit";
            this.ResumeLayout(false);

        }

        #endregion

        private DrawEditor drawEditor1;
    }
}