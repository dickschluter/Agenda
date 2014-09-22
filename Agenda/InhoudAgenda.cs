using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Agenda
{
    class InhoudAgenda
    {
        static List<Dag> dagenLijst = new List<Dag>();
        static bool inhoudGewijzigd;

        static readonly string padAgendaMap =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Agenda";
        static readonly string padBackupDataMap =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BackupAgenda";

        public static void LeesInhoud()
        {
            if (Directory.Exists(padAgendaMap) == false)
                Directory.CreateDirectory(padAgendaMap);
            if (Directory.Exists(padBackupDataMap) == false)
                Directory.CreateDirectory(padBackupDataMap);

            bool bestandGelezen = LeesBestand(padAgendaMap + @"\Inhoud.xml");
            if (bestandGelezen == false)
            {
                inhoudGewijzigd = true;
                bestandGelezen = LeesBestand(padBackupDataMap + @"\Inhoud.xml"); // Lees backup
            }
            if (bestandGelezen == false)
                System.Windows.Forms.MessageBox.Show
                    ("Fout in bestand. Inhoud agenda wordt niet goed gelezen.");
        }

        static bool LeesBestand(string bestand)
        {
            if (File.Exists(bestand) == false)
                return false;

            dagenLijst.Clear();
            XmlTextReader reader = new XmlTextReader(bestand);
            DateTime huidigeDag = DateTime.MinValue;
            string[] dagTekst = null;
            int regel = 0;

            bool resultaat = true;
            try
            {
                while (reader.Read())
                {
                    if (reader.Name == "Dag" && reader.NodeType == XmlNodeType.Element)
                    {
                        huidigeDag = DateTime.Parse(reader.GetAttribute("datum"));
                        dagTekst = new string[7];
                    }
                    else if (reader.Name == "Regel" && reader.NodeType == XmlNodeType.Element)
                        regel = int.Parse(reader.GetAttribute("nr"));
                    else if (reader.NodeType == XmlNodeType.Text)
                        dagTekst[regel] = reader.Value;
                    else if (reader.Name == "Dag" && reader.NodeType == XmlNodeType.EndElement)
                        dagenLijst.Add(new Dag(huidigeDag, dagTekst));
                }
            }
            catch (Exception)
            {
                resultaat = false;
            }
            finally
            {
                reader.Close();
            }

            return resultaat;
        }

        public static IEnumerable<Dag> DagReeks(DateTime startDatum, int aantalDagen)
        {
            int index = vindIndex(startDatum);

            DateTime datum = startDatum;
            while (datum < startDatum.AddDays(aantalDagen))
            {
                if (index < dagenLijst.Count - 1 && dagenLijst[index].Datum < datum) // naar eerste lijstelement >= datum
                    index++;
                if (dagenLijst.Count > 0 && dagenLijst[index].Datum == datum)
                    yield return dagenLijst[index];
                else
                    yield return new Dag(datum);
                datum = datum.AddDays(1);
            }
        }

        public static void VervangDag(DateTime datum, string[] dagTekst)
        {
            inhoudGewijzigd = true;
            int index = vindIndex(datum);

            // datum verwijderen indien aanwezig
            if (dagenLijst.Count > 0 && datum == dagenLijst[index].Datum)
                dagenLijst.RemoveAt(index);
            // nieuwe dag invoegen als deze inhoud heeft
            if (dagTekst != null)
            {
                if (index == dagenLijst.Count)
                    dagenLijst.Add(new Dag(datum, dagTekst));
                else if (datum < dagenLijst[index].Datum)
                    dagenLijst.Insert(index, new Dag(datum, dagTekst));
                else
                    dagenLijst.Insert(index + 1, new Dag(datum, dagTekst));
            }
        }

        private static int vindIndex(DateTime datum)
        {
            int index = 0, laag = 0, hoog = dagenLijst.Count - 1;
            while (laag <= hoog)
            {
                index = (laag + hoog) / 2;
                if (datum < dagenLijst[index].Datum)
                    hoog = index - 1;
                else if (datum > dagenLijst[index].Datum)
                    laag = index + 1;
                else
                    break;
            }
            return index;
        }

        public static void Opslaan()
        {
            if (inhoudGewijzigd)
            {
                XmlDocument document = new XmlDocument();
                XmlNode hoofdNode = document.CreateElement("Inhoud");
                document.AppendChild(hoofdNode);

                DateTime startDatum = DateTime.Today.AddDays(-420); // meer dan 60 weken geleden niet opslaan
                foreach (Dag dag in dagenLijst)
                {
                    if (dag.Datum < startDatum)
                        continue;

                    XmlNode dagNode = document.CreateElement("Dag");
                    XmlAttribute datum = document.CreateAttribute("datum");
                    datum.Value = dag.Datum.ToShortDateString();
                    dagNode.Attributes.Append(datum);
                    for (int regel = 0; regel < 7; regel++)
                        if (dag.Tekst[regel] != null)
                        {
                            XmlNode regelNode = document.CreateElement("Regel");
                            XmlAttribute nummer = document.CreateAttribute("nr");
                            nummer.Value = regel.ToString();
                            regelNode.Attributes.Append(nummer);
                            regelNode.InnerText = dag.Tekst[regel];
                            dagNode.AppendChild(regelNode);
                        }
                    hoofdNode.AppendChild(dagNode);
                }

                document.Save(padAgendaMap + @"\Inhoud.xml");
                document.Save(padBackupDataMap + @"\Inhoud.xml");
            }

            if (DateTime.Today.Month == 1) maakJaarBestand(DateTime.Today.Year - 1);
        }

        private static void maakJaarBestand(int jaartal)
        {
            if (Directory.Exists(padAgendaMap + @"\Geschiedenis") == false)
                Directory.CreateDirectory(padAgendaMap + @"\Geschiedenis");

            if (File.Exists(padAgendaMap + @"\Geschiedenis\Inhoud" + jaartal + ".xml") == false)
            {
                XmlDocument document = new XmlDocument();
                XmlNode hoofdNode = document.CreateElement("Inhoud" + jaartal);
                document.AppendChild(hoofdNode);

                foreach (Dag dag in dagenLijst)
                {
                    if (dag.Datum.Year != jaartal)
                        continue;

                    XmlNode dagNode = document.CreateElement("Dag");
                    XmlAttribute datum = document.CreateAttribute("datum");
                    datum.Value = dag.Datum.ToShortDateString();
                    dagNode.Attributes.Append(datum);
                    for (int regel = 0; regel < 7; regel++)
                        if (dag.Tekst[regel] != null)
                        {
                            XmlNode regelNode = document.CreateElement("Regel");
                            XmlAttribute nummer = document.CreateAttribute("nr");
                            nummer.Value = regel.ToString();
                            regelNode.Attributes.Append(nummer);
                            regelNode.InnerText = dag.Tekst[regel];
                            dagNode.AppendChild(regelNode);
                        }
                    hoofdNode.AppendChild(dagNode);
                }

                document.Save(padAgendaMap + @"\Geschiedenis\Inhoud" + jaartal + ".xml");
            }
        }
    }
}
