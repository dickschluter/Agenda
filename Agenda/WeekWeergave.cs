using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Agenda
{
    class WeekWeergave : Weergave
    {
        Panel[] panelDag = new Panel[7];
        Label[] labelDag = new Label[7];
        Label[] labelFeestdag = new Label[7];
        TextBox[] textBoxRegel = new TextBox[45];

        int tekstGewijzigd; // bitset v/d dagen, maandag is LSB, etc

        public WeekWeergave(Form1 form1)
        {
            this.Location = new Point(0, 60);
            this.Size = new Size(868, 612);
            this.BackColor = form1.LijnKleur;

            Font font10 = new Font("Microsoft Sans Serif", 10F);
            for (int dag = 0; dag < 7; dag++)
            {
                panelDag[dag] = new Panel();
                panelDag[dag].Size = new Size(433, (dag < 5 ? 202 : 100));
                panelDag[dag].Location = new Point((dag < 3 ? 0 : 435), (dag < 6 ? 2 + (dag % 3) * 204 : 512));
                panelDag[dag].Tag = dag;
                panelDag[dag].BackColor = form1.BackColor;
                panelDag[dag].MouseClick += new MouseEventHandler(panelDag_MouseClick);
                this.Controls.Add(panelDag[dag]);

                labelDag[dag] = new Label();
                labelDag[dag].Size = new Size(210, 18);
                labelDag[dag].Location = new Point(27, 4);
                labelDag[dag].Font = font10;
                panelDag[dag].Controls.Add(labelDag[dag]);

                labelFeestdag[dag] = new Label();
                labelFeestdag[dag].Size = new Size(140, 18);
                labelFeestdag[dag].Location = new Point(237, 4);
                labelFeestdag[dag].Font = font10;
                panelDag[dag].Controls.Add(labelFeestdag[dag]);

                for (int regel = 0; regel < 7; regel++)
                {
                    if (dag > 4 && regel > 2) continue;
                    int index = 7 * dag + regel;
                    textBoxRegel[index] = new TextBox();
                    textBoxRegel[index].Size = new Size(400, 22);
                    textBoxRegel[index].Location = new Point(30, 34 + regel * 22);
                    textBoxRegel[index].BackColor = form1.BackColor;
                    textBoxRegel[index].BorderStyle = BorderStyle.None;
                    textBoxRegel[index].Multiline = true;
                    textBoxRegel[index].MaxLength = 55;
                    textBoxRegel[index].WordWrap = false;
                    textBoxRegel[index].Cursor = Cursors.Hand;
                    textBoxRegel[index].Font = font10;
                    textBoxRegel[index].Tag = index;
                    textBoxRegel[index].KeyDown += new KeyEventHandler(textBoxRegel_KeyDown);
                    textBoxRegel[index].KeyUp += new KeyEventHandler(textBoxRegel_KeyUp);
                    textBoxRegel[index].TextChanged += new EventHandler(textBoxRegel_TextChanged);
                    panelDag[dag].Controls.Add(textBoxRegel[index]);
                }
            }
        }

        public void UpdateTekst()
        {
            KopTekst = "Agenda - week " + geefWeekNummer(datumGeselecteerd);
            int paasZondag = GeefPaasZondag(datumGeselecteerd.Year);

            int dag = 0;
            foreach (Dag huidigeDag in InhoudAgenda.DagReeks(datumGeselecteerd, 7))
            {
                DateTime datum = huidigeDag.Datum;
                labelDag[dag].Text = datum.ToLongDateString();
                labelDag[dag].ForeColor = datum == DateTime.Today ? this.BackColor : Color.Black;
                labelFeestdag[dag].Text = GeefFeestdag(datum, paasZondag);
                labelFeestdag[dag].ForeColor = labelDag[dag].ForeColor;

                for (int regel = 0; regel < 7; regel++)
                {
                    if (dag > 4 && regel > 2)
                        continue;
                    textBoxRegel[7 * dag + regel].Text =
                        huidigeDag.Tekst != null ? huidigeDag.Tekst[regel] : "";
                    textBoxRegel[7 * dag + regel].ForeColor =
                            textBoxRegel[7 * dag + regel].Text.Contains("!") ? Color.Red : Color.Black;
                }
                
                dag++;
            }

            tekstGewijzigd = 0;
        }

        private int geefWeekNummer(DateTime maandag)
        {
            if (maandag.DayOfWeek != DayOfWeek.Monday)
                throw new ArgumentException("Datum is geen maandag");

            if (maandag.Month == 12 && maandag.Day >= 29)
                return 1;
            int resultaat = maandag.DayOfYear / 7 + 1;
            if (maandag.DayOfYear % 7 > 4) // eerste maandag valt op 5/6/7 januari
                resultaat++;
            return resultaat;
        }

        private void textBoxRegel_TextChanged(object sender, EventArgs e)
        {
            int index = (int)(sender as Control).Tag;
            textBoxRegel[index].ForeColor = textBoxRegel[index].Text.Contains("!") ? Color.Red : Color.Black;
            tekstGewijzigd |= (1 << (index / 7));
        }

        public void BewaarTekst()
        {
            for (int dag = 0; dag < 7; dag++)
                if ((tekstGewijzigd & (1 << dag)) != 0)
                {
                    bool nuttigeInhoud = false;
                    string[] dagTekst = new string[7];
                    for (int regel = 0; regel < 7; regel++)
                    {
                        if (dag > 4 && regel > 2)
                            continue;
                        string tekst = textBoxRegel[7 * dag + regel].Text.Trim();
                        if (tekst.Length > 0)
                        {
                            dagTekst[regel] = tekst;
                            nuttigeInhoud = true;
                        }
                    }

                    if (nuttigeInhoud)
                        InhoudAgenda.VervangDag(datumGeselecteerd.AddDays(dag), dagTekst);
                    else
                        InhoudAgenda.VervangDag(datumGeselecteerd.AddDays(dag), null);
                }
        }

        private void panelDag_MouseClick(object sender, MouseEventArgs e)
        {
            int dag = (int)((sender as Control).Tag);
            int regel = (e.Y - 32) / 22;
            if (regel < 0)
                regel = 0;
            if (regel > 6)
                regel = 6;
            if (dag > 4 && regel > 2)
                regel = 2;
            focusOpRegel(7 * dag + regel);
        }

        private void textBoxRegel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void textBoxRegel_KeyUp(object sender, KeyEventArgs e)
        {
            int index = (int)((sender as Control).Tag);
            if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter) && index < 44)
            {
                index++;
                if (index == 38) index = 42;
                focusOpRegel(index);
            }
            else if (e.KeyCode == Keys.Up && index > 0)
            {
                index--;
                if (index == 41) index = 37;
                focusOpRegel(index);
            }
        }

        private void focusOpRegel(int index)
        {
            textBoxRegel[index].Focus();
            textBoxRegel[index].Select(textBoxRegel[index].TextLength, 0);
        }
    }
}
