using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agenda
{
    public partial class Form1 : Form
    {
        public readonly Color AchtergrondKleur = Color.FromArgb(255, 255, 220);
        public readonly Color LijnKleur = Color.FromArgb(0, 78, 152);
        public readonly Brush HoverBrush = new SolidBrush(Color.FromArgb(245, 220, 0));

        Label[] labelMaand = new Label[12];
        bool pictureBoxVorigeHover, pictureBoxVolgendeHover;

        FormTaken formTaken;
        Wekker wekker;

        public Form1()
        {
            InitializeComponent();
            this.BackColor = AchtergrondKleur;

            string[] maandAfkortingen = { "Jan", "Feb", "Maa", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" };
            for (int maand = 0; maand < 12; maand++)
            {
                labelMaand[maand] = new Label();
                labelMaand[maand].Size = new Size(28, 13);
                labelMaand[maand].Location = new Point(62 + (maand % 6) * 28, 14 + (maand / 6) * 20);
                labelMaand[maand].Cursor = Cursors.Hand;
                labelMaand[maand].Tag = maand;
                labelMaand[maand].Text = maandAfkortingen[maand];
                labelMaand[maand].MouseClick += new MouseEventHandler(labelMaand_MouseClick);
                this.Controls.Add(labelMaand[maand]);
            }

            InhoudAgenda.LeesInhoud();

            Weergave.Initialiseren(this);
            this.Text = Weergave.KopTekst;
            Weergave.Gewisseld += new EventHandler(Weergave_Gewisseld);

            wekker = new Wekker(this);
            wekker.Location = new Point(740, 0);
            this.Controls.Add(wekker);
            wekker.BringToFront();
            wekker.LooptAf += new EventHandler(wekker_LooptAf);
        }

        private void Weergave_Gewisseld(object sender, EventArgs e)
        {
            this.Text = Weergave.KopTekst;
            labelWeergave.Text = Weergave.IsWeekWeergave ? "Maand" : "Week";
        }

        private void wekker_LooptAf(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        void labelMaand_MouseClick(object sender, MouseEventArgs e)
        {
            buttonFocus.Focus();
            int maand = (int)(sender as Control).Tag + 1; // DateTime.Month is niet zero-based

            if (e.Button == MouseButtons.Left)
                Weergave.VooruitSpringen(maand);
            else
                Weergave.TerugSpringen(maand);
            this.Text = Weergave.KopTekst;
        }

        private void labelVandaag_Click(object sender, EventArgs e)
        {
            buttonFocus.Focus();
            Weergave.Vandaag();
            this.Text = Weergave.KopTekst;
        }

        void pictureBoxVorige_Click(object sender, System.EventArgs e)
        {
            buttonFocus.Focus();
            Weergave.Vorige();
            this.Text = Weergave.KopTekst;
        }

        void pictureBoxVorige_DoubleClick(object sender, System.EventArgs e)
        {
            Weergave.Vorige();
            this.Text = Weergave.KopTekst;
        }

        void pictureBoxVorige_MouseEnter(object sender, System.EventArgs e)
        {
            pictureBoxVorigeHover = true;
            pictureBoxVorige.Refresh();
        }

        void pictureBoxVorige_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBoxVorigeHover = false;
            pictureBoxVorige.Refresh();
        }

        private void pictureBoxVorige_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBoxVorigeHover)
            {
                e.Graphics.FillRectangle(HoverBrush, 2, 2, 36, 2);
                e.Graphics.FillRectangle(HoverBrush, 2, 21, 36, 2);
                e.Graphics.FillRectangle(HoverBrush, 2, 2, 2, 21);
                e.Graphics.FillRectangle(HoverBrush, 36, 2, 2, 21);
            }
        }

        void pictureBoxVolgende_Click(object sender, System.EventArgs e)
        {
            buttonFocus.Focus();
            Weergave.Volgende();
            this.Text = Weergave.KopTekst;
        }

        void pictureBoxVolgende_DoubleClick(object sender, System.EventArgs e)
        {
            Weergave.Volgende();
            this.Text = Weergave.KopTekst;
        }

        void pictureBoxVolgende_MouseEnter(object sender, System.EventArgs e)
        {
            pictureBoxVolgendeHover = true;
            pictureBoxVolgende.Refresh();
        }

        void pictureBoxVolgende_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBoxVolgendeHover = false;
            pictureBoxVolgende.Refresh();
        }

        private void pictureBoxVolgende_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBoxVolgendeHover)
            {
                e.Graphics.FillRectangle(HoverBrush, 2, 2, 36, 2);
                e.Graphics.FillRectangle(HoverBrush, 2, 21, 36, 2);
                e.Graphics.FillRectangle(HoverBrush, 2, 2, 2, 21);
                e.Graphics.FillRectangle(HoverBrush, 36, 2, 2, 21);
            }
        }

        private void labelWeergave_Click(object sender, EventArgs e)
        {
            buttonFocus.Focus();
            Weergave.WisselWeergave();
        }

        private void labelTaken_Click(object sender, EventArgs e)
        {
            formTaken = FormTaken.GeefInstantie(this);
            formTaken.Show();
            formTaken.Focus();
        }

        private void labelWekker_Click(object sender, EventArgs e)
        {
            wekker.ToonFormulier();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && wekker.IsGezet
                && MessageBox.Show("Agenda toch afsluiten?", "De wekker is gezet", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    e.Cancel = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (formTaken != null)
                formTaken.Close();
            Weergave.BewaarWijzigingen();
            InhoudAgenda.Opslaan();
        }
    }
}
