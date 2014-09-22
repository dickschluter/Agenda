using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Agenda
{
    public partial class FormTaken : Form
    {
        string padAgendaMap;
        string[,] taken = new string[4, 24];
        int huidigeLijst; // 0 t/m 3
        bool takenGewijzigd;

        Panel[] panelLijst = new Panel[4];
        Label[] labelLijst = new Label[4];
        TextBox[] textBoxLijst = new TextBox[4];
        PictureBox[] pictureBoxLijst = new PictureBox[4];
        PictureBox[] pictureBoxRegel = new PictureBox[24];
        TextBox[] textBoxRegel = new TextBox[24];

        private static FormTaken formTaken;

        public static FormTaken GeefInstantie(Form1 form1)
        {
            if (formTaken == null)
                formTaken = new FormTaken(form1);
            return formTaken;
        }

        private FormTaken(Form1 form1)
        {
            InitializeComponent();

            this.BackColor = form1.BackColor;
            pictureBox1.BackColor = form1.LijnKleur;
            pictureBox2.BackColor = form1.LijnKleur;
            pictureBox3.BackColor = form1.LijnKleur;

            MaakLayout(form1);

            padAgendaMap = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Agenda";
            if (File.Exists(padAgendaMap + @"\Taken.xml"))
            {
                int lijst = -1, regel = 0;
                XmlTextReader reader = new XmlTextReader(padAgendaMap + @"\Taken.xml");
                try
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "Lijst" && reader.NodeType == XmlNodeType.Element)
                        {
                            lijst++;
                            labelLijst[lijst].Text = reader.GetAttribute("naam");
                        }
                        if (reader.Name == "Regel" && reader.NodeType == XmlNodeType.Element)
                            regel = int.Parse(reader.GetAttribute("nr"));
                        if (reader.NodeType == XmlNodeType.Text)
                            taken[lijst, regel] = reader.Value;
                    }
                }
                catch { MessageBox.Show("Fout in bestand. Takenlijst wordt niet goed gelezen."); }
                finally { reader.Close(); }
            }

            for (int regel = 0; regel < 24; regel++)
                if (taken[0, regel] != null)
                    textBoxRegel[regel].Text = taken[0, regel];
        }

        void MaakLayout(Form1 form1)
        {
            Bitmap bitmapKruis = Properties.Resources.Kruis;
            bitmapKruis.MakeTransparent(Color.White);

            for (int lijst = 0; lijst < 4; lijst++)
            {
                panelLijst[lijst] = new Panel();
                panelLijst[lijst].Size = new Size(100, 34);
                panelLijst[lijst].Location = new Point(lijst * 102, 0);
                panelLijst[lijst].Tag = lijst;
                panelLijst[lijst].MouseClick += new MouseEventHandler(labelLijst_MouseClick);
                this.Controls.Add(panelLijst[lijst]);

                textBoxLijst[lijst] = new TextBox();
                textBoxLijst[lijst].Size = new Size(96, 20);
                textBoxLijst[lijst].Location = new Point(2, 10);
                textBoxLijst[lijst].BackColor = this.BackColor;
                textBoxLijst[lijst].BorderStyle = BorderStyle.None;
                textBoxLijst[lijst].Cursor = Cursors.Hand;
                textBoxLijst[lijst].MaxLength = 12;
                textBoxLijst[lijst].Tag = lijst;
                textBoxLijst[lijst].TextAlign = HorizontalAlignment.Center;
                textBoxLijst[lijst].Visible = false;
                textBoxLijst[lijst].TextChanged += (sender, e) => takenGewijzigd = true;
                textBoxLijst[lijst].Leave += new EventHandler(textBoxLijst_Leave);
                panelLijst[lijst].Controls.Add(textBoxLijst[lijst]);

                labelLijst[lijst] = new Label();
                labelLijst[lijst].Size = new Size(96, 13);
                labelLijst[lijst].Location = new Point(2, 10);
                labelLijst[lijst].Cursor = Cursors.Hand;
                labelLijst[lijst].Tag = lijst;
                labelLijst[lijst].Text = (lijst + 1).ToString();
                labelLijst[lijst].TextAlign = ContentAlignment.MiddleCenter;
                labelLijst[lijst].MouseClick += new MouseEventHandler(labelLijst_MouseClick);
                panelLijst[lijst].Controls.Add(labelLijst[lijst]);

                pictureBoxLijst[lijst] = new PictureBox();
                pictureBoxLijst[lijst].Size = new Size(100, 2);
                pictureBoxLijst[lijst].Location = new Point(0, 32);
                pictureBoxLijst[lijst].BackColor = form1.LijnKleur;
                pictureBoxLijst[lijst].Visible = lijst > 0 ? true : false;
                panelLijst[lijst].Controls.Add(pictureBoxLijst[lijst]);
            }

            for (int regel = 0; regel < 24; regel++)
            {
                pictureBoxRegel[regel] = new PictureBox();
                pictureBoxRegel[regel].Size = new Size(15, 15);
                pictureBoxRegel[regel].Location = new Point(8, 48 + regel * 22);
                pictureBoxRegel[regel].Image = bitmapKruis;
                pictureBoxRegel[regel].Tag = regel;
                pictureBoxRegel[regel].Click += new EventHandler(pictureBoxRegel_Click);
                this.Controls.Add(pictureBoxRegel[regel]);

                textBoxRegel[regel] = new TextBox();
                textBoxRegel[regel].Size = new Size(350, 22);
                textBoxRegel[regel].Location = new Point(30, 46 + regel * 22);
                textBoxRegel[regel].BackColor = this.BackColor;
                textBoxRegel[regel].BorderStyle = BorderStyle.None;
                textBoxRegel[regel].Multiline = true;
                textBoxRegel[regel].WordWrap = false;
                textBoxRegel[regel].Cursor = Cursors.Hand;
                textBoxRegel[regel].Font = new Font("Microsoft Sans Serif", 10F);
                textBoxRegel[regel].Tag = regel;
                textBoxRegel[regel].KeyDown += new KeyEventHandler(textBoxRegel_KeyDown);
                textBoxRegel[regel].KeyUp += new KeyEventHandler(textBoxRegel_KeyUp);
                textBoxRegel[regel].TextChanged += (sender, e) => takenGewijzigd = true;
                this.Controls.Add(textBoxRegel[regel]);
            }
        }

        void labelLijst_MouseClick(object sender, MouseEventArgs e)
        {
            int index = (int)(sender as Control).Tag;

            if (index != huidigeLijst)
            {
                for (int regel = 0; regel < 24; regel++)
                    taken[huidigeLijst, regel] =
                        textBoxRegel[regel].Text.Trim().Length > 0 ? textBoxRegel[regel].Text.Trim() : null;
                pictureBoxLijst[huidigeLijst].Visible = true;

                huidigeLijst = index;

                pictureBoxLijst[huidigeLijst].Visible = false;
                for (int regel = 0; regel < 24; regel++)
                    textBoxRegel[regel].Text = taken[huidigeLijst, regel];
            }

            if (e.Button == MouseButtons.Right)
            {
                textBoxLijst[index].Visible = true;
                textBoxLijst[index].Text = labelLijst[index].Text;
                textBoxLijst[index].Focus();
                textBoxLijst[index].DeselectAll();
            }
            else
                buttonFocus.Focus();
        }

        void textBoxLijst_Leave(object sender, EventArgs e)
        {
            int index = (int)(sender as Control).Tag;

            textBoxLijst[index].Visible = false;
            labelLijst[index].Text = textBoxLijst[index].Text;
        }

        void pictureBoxRegel_Click(object sender, EventArgs e)
        {
            int index = (int)(sender as Control).Tag;

            textBoxRegel[index].Text = "";
            this.Update();
            System.Threading.Thread.Sleep(500);
            for (int regel = index; regel < 23; regel++)
                textBoxRegel[regel].Text = textBoxRegel[regel + 1].Text;
            textBoxRegel[23].Text = "";

            buttonFocus.Focus();
        }

        void textBoxRegel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                int index = (int)(((Control)sender).Tag);
                if (index < 23)
                {
                    index++;
                    textBoxRegel[index].Focus();
                    textBoxRegel[index].DeselectAll();
                }
            }
        }

        void textBoxRegel_KeyUp(object sender, KeyEventArgs e)
        {
            int index = (int)(((Control)sender).Tag);
            if (e.KeyCode == Keys.Down && index < 23)
            {
                index++;
                textBoxRegel[index].Focus();
                textBoxRegel[index].DeselectAll();
            }
            else if (e.KeyCode == Keys.Up && index > 0)
            {
                index--;
                textBoxRegel[index].Focus();
                textBoxRegel[index].DeselectAll();
            }
        }

        private void FormTaken_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (takenGewijzigd)
            {
                for (int regel = 0; regel < 24; regel++)
                    taken[huidigeLijst, regel] =
                        textBoxRegel[regel].Text.Trim().Length > 0 ? textBoxRegel[regel].Text.Trim() : null;

                XmlDocument document = new XmlDocument();
                XmlNode hoofdNode = document.CreateElement("Taken");
                document.AppendChild(hoofdNode);
                for (int lijst = 0; lijst < 4; lijst++)
                {
                    XmlNode lijstNode = document.CreateElement("Lijst");
                    XmlAttribute naam = document.CreateAttribute("naam");
                    naam.Value = labelLijst[lijst].Text;
                    lijstNode.Attributes.Append(naam);
                    for (int regel = 0; regel < 24; regel++)
                    {
                        if (taken[lijst, regel] == null) continue;
                        XmlNode regelNode = document.CreateElement("Regel");
                        XmlAttribute nummer = document.CreateAttribute("nr");
                        nummer.Value = regel.ToString();
                        regelNode.Attributes.Append(nummer);
                        regelNode.InnerText = taken[lijst, regel];
                        lijstNode.AppendChild(regelNode);
                    }
                    hoofdNode.AppendChild(lijstNode);
                }
                document.Save(padAgendaMap + @"\Taken.xml");
            }

            formTaken = null;
        }
    }
}
