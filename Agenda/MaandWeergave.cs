using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Agenda
{
    class MaandWeergave : Weergave
    {
        Label labelHuidigeMaand;
        Panel[] panelDag = new Panel[42];
        Label[] labelDag = new Label[42];
        Label[] labelFeestdag = new Label[42];
        Label[] labelRegel = new Label[294];
        string[] maandNamen = { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december" };

        public MaandWeergave(Form1 form1)
        {
            this.Location = new Point(0, 60);
            this.Size = new Size(868, 612);
            this.BackColor = form1.LijnKleur;

            Panel panelMaand = new Panel();
            panelMaand.Size = new Size(this.Width, 39);
            panelMaand.Location = new Point(0, 2);
            panelMaand.BackColor = form1.BackColor;
            this.Controls.Add(panelMaand);

            labelHuidigeMaand = new Label();
            labelHuidigeMaand.Size = new Size(120, 17);
            labelHuidigeMaand.Location = new Point(374, 2);
            labelHuidigeMaand.Font = new Font("Microsoft Sans Serif", 10F);
            labelHuidigeMaand.TextAlign = ContentAlignment.MiddleCenter;
            panelMaand.Controls.Add(labelHuidigeMaand);

            string[] dagNamen = { "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag", "zondag" };
            for (int dag = 0; dag < 7; dag++)
            {
                Label labelDagNaam = new Label();
                labelDagNaam.Size = new Size(123, 13);
                labelDagNaam.Location = new Point(124 * dag, 24);
                labelDagNaam.Font = new Font("Microsoft Sans Serif", 8F);
                labelDagNaam.TextAlign = ContentAlignment.MiddleCenter;
                labelDagNaam.Text = dagNamen[dag];
                panelMaand.Controls.Add(labelDagNaam);
            }

            for (int dag = 0; dag < 42; dag++)
            {
                int dagVanWeek = dag % 7;
                int week = dag / 7;

                panelDag[dag] = new Panel();
                panelDag[dag].Size = new Size(123, 94);
                panelDag[dag].Location =
                    new Point((dagVanWeek < 5 ? 124 * dagVanWeek : 124 * dagVanWeek + 1), 43 + 95 * week);
                panelDag[dag].BackColor = form1.BackColor;
                panelDag[dag].Tag = week;
                panelDag[dag].Click += new EventHandler(panelDag_Click);
                this.Controls.Add(panelDag[dag]);

                labelDag[dag] = new Label();
                labelDag[dag].Size = new Size(20, 13);
                labelDag[dag].Location = new Point(0, 0);
                labelDag[dag].Font = new Font("Microsoft Sans Serif", 8F);
                labelDag[dag].Tag = week;
                labelDag[dag].Click += new EventHandler(panelDag_Click);
                panelDag[dag].Controls.Add(labelDag[dag]);

                labelFeestdag[dag] = new Label();
                labelFeestdag[dag].Size = new Size(105, 13);
                labelFeestdag[dag].Location = new Point(18, 0);
                labelFeestdag[dag].Font = new Font("Microsoft Sans Serif", 8F);
                labelFeestdag[dag].Tag = week;
                labelFeestdag[dag].Click += new EventHandler(panelDag_Click);
                panelDag[dag].Controls.Add(labelFeestdag[dag]);

                for (int regel = 0; regel < 7; regel++)
                {
                    int index = 7 * dag + regel;
                    labelRegel[index] = new Label();
                    labelRegel[index].Size = new Size(123, 13);
                    labelRegel[index].Location = new Point(0, 13 + 11 * regel);
                    labelRegel[index].Font = new Font("Microsoft Sans Serif", 7F);
                    labelRegel[index].Tag = week;
                    labelRegel[index].Click += new EventHandler(panelDag_Click);
                    panelDag[dag].Controls.Add(labelRegel[index]);
                }
            }
        }

        void panelDag_Click(object sender, EventArgs e)
        {
            int week = (int)((sender as Control).Tag);
            datumGeselecteerd = datumGeselecteerd.AddDays(7 * week - 7);
            WisselWeergave();
        }

        public void UpdateTekst()
        {
            string maand = maandNamen[datumGeselecteerd.Month - 1]; // DateTime.Month is niet zero-based
            KopTekst = String.Format("Agenda - {0} {1}", maand.Substring(0, 3), datumGeselecteerd.Year);
            labelHuidigeMaand.Text = String.Format("{0} {1}", maand, datumGeselecteerd.Year);
            int paasZondag = GeefPaasZondag(datumGeselecteerd.Year);

            int dag = 0;
            foreach (Dag huidigeDag in InhoudAgenda.DagReeks(datumGeselecteerd.AddDays(-7), 42))
            {
                DateTime datum = huidigeDag.Datum;
                labelDag[dag].Text = datum.Day.ToString();
                if (datum == DateTime.Today)
                    labelDag[dag].ForeColor = this.BackColor;
                else if (datum.Month == datumGeselecteerd.Month)
                    labelDag[dag].ForeColor = Color.Black;
                else
                    labelDag[dag].ForeColor = Color.Gray;

                labelFeestdag[dag].Text = GeefFeestdag(datum, paasZondag);
                labelFeestdag[dag].ForeColor = labelDag[dag].ForeColor;

                for (int regel = 0; regel < 7; regel++)
                {
                    if (dag % 7 > 4 && regel > 2)
                        continue;
                    labelRegel[7 * dag + regel].Text = huidigeDag.Tekst != null ? huidigeDag.Tekst[regel] : "";
                    labelRegel[7 * dag + regel].ForeColor = labelRegel[7 * dag + regel].Text.Contains("!") ? Color.Red : Color.Black;
                }

                dag++;
            }
        }
    }
}
