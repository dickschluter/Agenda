namespace Agenda
{
    partial class FormWekker
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxUit = new System.Windows.Forms.PictureBox();
            this.pictureBoxAan = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAan)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(95, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(45, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Alarmtijd";
            // 
            // pictureBoxUit
            // 
            this.pictureBoxUit.Image = global::Agenda.Properties.Resources.Uit;
            this.pictureBoxUit.Location = new System.Drawing.Point(100, 90);
            this.pictureBoxUit.Name = "pictureBoxUit";
            this.pictureBoxUit.Size = new System.Drawing.Size(40, 25);
            this.pictureBoxUit.TabIndex = 5;
            this.pictureBoxUit.TabStop = false;
            this.pictureBoxUit.Click += new System.EventHandler(this.pictureBoxUit_Click);
            // 
            // pictureBoxAan
            // 
            this.pictureBoxAan.Image = global::Agenda.Properties.Resources.Aan;
            this.pictureBoxAan.Location = new System.Drawing.Point(30, 90);
            this.pictureBoxAan.Name = "pictureBoxAan";
            this.pictureBoxAan.Size = new System.Drawing.Size(40, 25);
            this.pictureBoxAan.TabIndex = 4;
            this.pictureBoxAan.TabStop = false;
            this.pictureBoxAan.Click += new System.EventHandler(this.pictureBoxAan_Click);
            // 
            // FormWekker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(170, 145);
            this.Controls.Add(this.pictureBoxUit);
            this.Controls.Add(this.pictureBoxAan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormWekker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Wekker";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxAan;
        private System.Windows.Forms.PictureBox pictureBoxUit;
    }
}