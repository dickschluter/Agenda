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
    public partial class FormWekker : Form
    {
        Wekker wekker;

        public FormWekker(Wekker wekker, Form1 form1)
        {
            InitializeComponent();

            this.wekker = wekker;
            this.BackColor = form1.BackColor;
            this.DesktopLocation = form1.DesktopLocation + new Size(686, 100);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') // 'Enter'
            {
                pictureBoxAan_Click(sender, e);
                return;
            }
            if (e.KeyChar == '\b') // 'Backspace'
                return;

            string toegestaneTekens = "1234567890.,;:";
            if (textBox1.TextLength < 5 && toegestaneTekens.Contains(e.KeyChar.ToString()))
                return;
            else
                e.Handled = true;
        }

        private void pictureBoxAan_Click(object sender, EventArgs e)
        {
            string invoer = textBox1.Text;
            bool geldigeTijd = true;
            int uur = 0, minuut = 0;

            if (invoer.Length < 4)
                geldigeTijd = false;

            for (int i = 0; i < invoer.Length; i++)
            {
                if (i != (invoer.Length - 3) && !char.IsDigit(invoer, i))
                    geldigeTijd = false;
                if (i == (invoer.Length - 3) && char.IsDigit(invoer, i))
                    geldigeTijd = false;
            }

            if (geldigeTijd)
            {
                int.TryParse(invoer.Substring(0, invoer.Length - 3), out uur);
                int.TryParse(invoer.Substring(invoer.Length - 2, 2), out minuut);
                if (uur >= 24 || minuut >= 60)
                    geldigeTijd = false;
            }

            if (geldigeTijd)
            {
                DateTime vandaag = DateTime.Today;
                DateTime alarmTijd = new DateTime(vandaag.Year, vandaag.Month, vandaag.Day, uur, minuut, 0);

                if (alarmTijd > DateTime.Now)
                {
                    wekker.Zetten(alarmTijd);
                    this.Dispose();
                }
                else
                    MessageBox.Show("Ingestelde tijd is al verstreken");
            }
            else
                MessageBox.Show("Geen geldige tijd ingevoerd");
        }

        private void pictureBoxUit_Click(object sender, EventArgs e)
        {
            wekker.UitZetten();
            this.Dispose();
        }
    }
}
