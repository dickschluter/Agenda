namespace Agenda
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelVandaag = new System.Windows.Forms.Label();
            this.labelTaken = new System.Windows.Forms.Label();
            this.buttonFocus = new System.Windows.Forms.Button();
            this.labelWeergave = new System.Windows.Forms.Label();
            this.labelWekker = new System.Windows.Forms.Label();
            this.pictureBoxVolgende = new System.Windows.Forms.PictureBox();
            this.pictureBoxVorige = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVolgende)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVorige)).BeginInit();
            this.SuspendLayout();
            // 
            // labelVandaag
            // 
            this.labelVandaag.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelVandaag.Location = new System.Drawing.Point(274, 18);
            this.labelVandaag.Name = "labelVandaag";
            this.labelVandaag.Size = new System.Drawing.Size(70, 22);
            this.labelVandaag.TabIndex = 29;
            this.labelVandaag.Text = "Vandaag";
            this.labelVandaag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelVandaag.Click += new System.EventHandler(this.labelVandaag_Click);
            // 
            // labelTaken
            // 
            this.labelTaken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelTaken.Location = new System.Drawing.Point(634, 18);
            this.labelTaken.Name = "labelTaken";
            this.labelTaken.Size = new System.Drawing.Size(60, 22);
            this.labelTaken.TabIndex = 30;
            this.labelTaken.Text = "Taken";
            this.labelTaken.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTaken.Click += new System.EventHandler(this.labelTaken_Click);
            // 
            // buttonFocus
            // 
            this.buttonFocus.Location = new System.Drawing.Point(0, 0);
            this.buttonFocus.Name = "buttonFocus";
            this.buttonFocus.Size = new System.Drawing.Size(1, 1);
            this.buttonFocus.TabIndex = 31;
            this.buttonFocus.UseVisualStyleBackColor = true;
            // 
            // labelWeergave
            // 
            this.labelWeergave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelWeergave.Location = new System.Drawing.Point(531, 18);
            this.labelWeergave.Name = "labelWeergave";
            this.labelWeergave.Size = new System.Drawing.Size(60, 22);
            this.labelWeergave.TabIndex = 35;
            this.labelWeergave.Text = "Maand";
            this.labelWeergave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelWeergave.Click += new System.EventHandler(this.labelWeergave_Click);
            // 
            // labelWekker
            // 
            this.labelWekker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelWekker.Location = new System.Drawing.Point(740, 18);
            this.labelWekker.Name = "labelWekker";
            this.labelWekker.Size = new System.Drawing.Size(60, 22);
            this.labelWekker.TabIndex = 36;
            this.labelWekker.Text = "Wekker";
            this.labelWekker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelWekker.Click += new System.EventHandler(this.labelWekker_Click);
            // 
            // pictureBoxVolgende
            // 
            this.pictureBoxVolgende.BackColor = System.Drawing.Color.White;
            this.pictureBoxVolgende.Image = global::Agenda.Properties.Resources.NaarRechts;
            this.pictureBoxVolgende.Location = new System.Drawing.Point(434, 18);
            this.pictureBoxVolgende.Name = "pictureBoxVolgende";
            this.pictureBoxVolgende.Size = new System.Drawing.Size(40, 25);
            this.pictureBoxVolgende.TabIndex = 33;
            this.pictureBoxVolgende.TabStop = false;
            this.pictureBoxVolgende.Click += new System.EventHandler(this.pictureBoxVolgende_Click);
            this.pictureBoxVolgende.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxVolgende_Paint);
            this.pictureBoxVolgende.DoubleClick += new System.EventHandler(this.pictureBoxVolgende_DoubleClick);
            this.pictureBoxVolgende.MouseEnter += new System.EventHandler(this.pictureBoxVolgende_MouseEnter);
            this.pictureBoxVolgende.MouseLeave += new System.EventHandler(this.pictureBoxVolgende_MouseLeave);
            // 
            // pictureBoxVorige
            // 
            this.pictureBoxVorige.BackColor = System.Drawing.Color.White;
            this.pictureBoxVorige.Image = global::Agenda.Properties.Resources.NaarLinks;
            this.pictureBoxVorige.Location = new System.Drawing.Point(394, 18);
            this.pictureBoxVorige.Name = "pictureBoxVorige";
            this.pictureBoxVorige.Size = new System.Drawing.Size(40, 25);
            this.pictureBoxVorige.TabIndex = 32;
            this.pictureBoxVorige.TabStop = false;
            this.pictureBoxVorige.Click += new System.EventHandler(this.pictureBoxVorige_Click);
            this.pictureBoxVorige.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxVorige_Paint);
            this.pictureBoxVorige.DoubleClick += new System.EventHandler(this.pictureBoxVorige_DoubleClick);
            this.pictureBoxVorige.MouseEnter += new System.EventHandler(this.pictureBoxVorige_MouseEnter);
            this.pictureBoxVorige.MouseLeave += new System.EventHandler(this.pictureBoxVorige_MouseLeave);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 672);
            this.Controls.Add(this.labelWekker);
            this.Controls.Add(this.labelWeergave);
            this.Controls.Add(this.pictureBoxVolgende);
            this.Controls.Add(this.pictureBoxVorige);
            this.Controls.Add(this.buttonFocus);
            this.Controls.Add(this.labelTaken);
            this.Controls.Add(this.labelVandaag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agenda";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVolgende)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVorige)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelVandaag;
        private System.Windows.Forms.Label labelTaken;
        private System.Windows.Forms.Button buttonFocus;
        private System.Windows.Forms.PictureBox pictureBoxVorige;
        private System.Windows.Forms.PictureBox pictureBoxVolgende;
        private System.Windows.Forms.Label labelWekker;
        private System.Windows.Forms.Label labelWeergave;
    }
}

