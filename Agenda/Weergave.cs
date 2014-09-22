using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Agenda
{
    abstract class Weergave : UserControl
    {
        public static string KopTekst { get; set; }
        public static bool IsWeekWeergave { get; private set; }
        public static event EventHandler Gewisseld;

        private static WeekWeergave weekWeergave;
        private static MaandWeergave maandWeergave;

        protected static DateTime datumGeselecteerd; // eerste maandag van de geselecteerde week/maand
        protected static Dictionary<int, int> paasZondagen = new Dictionary<int, int>(); // memoizatie paaszondagen als DayOfYear

        public static void Initialiseren(Form1 form1)
        {
            IsWeekWeergave = true;
            weekWeergave = new WeekWeergave(form1);
            maandWeergave = new MaandWeergave(form1);
            form1.Controls.Add(weekWeergave);
            form1.Controls.Add(maandWeergave);
            
            datumGeselecteerd = DateTime.Today;
            while (datumGeselecteerd.DayOfWeek != DayOfWeek.Monday)
                datumGeselecteerd = datumGeselecteerd.AddDays(-1);
            weekWeergave.UpdateTekst();
        }

        public static void VooruitSpringen(int maand)
        {
            BewaarWijzigingen();

            if (maand > datumGeselecteerd.Month)
                datumGeselecteerd = new DateTime(datumGeselecteerd.Year, maand, 1);
            else
                datumGeselecteerd = new DateTime(datumGeselecteerd.Year + 1, maand, 1);
            while (datumGeselecteerd.DayOfWeek != DayOfWeek.Monday)
                datumGeselecteerd = datumGeselecteerd.AddDays(1);
            updateTekst();
        }

        public static void TerugSpringen(int maand)
        {
            BewaarWijzigingen();

            if (maand < datumGeselecteerd.Month)
                datumGeselecteerd = new DateTime(datumGeselecteerd.Year, maand, 1);
            else
                datumGeselecteerd = new DateTime(datumGeselecteerd.Year - 1, maand, 1);
            while (datumGeselecteerd.DayOfWeek != DayOfWeek.Monday)
                datumGeselecteerd = datumGeselecteerd.AddDays(1);
            updateTekst();
        }

        public static void Vandaag()
        {
            if (IsWeekWeergave)
            {
                weekWeergave.BewaarTekst();
                datumGeselecteerd = DateTime.Today;
                while (datumGeselecteerd.DayOfWeek != DayOfWeek.Monday)
                    datumGeselecteerd = datumGeselecteerd.AddDays(-1);
            }
            else
            {
                datumGeselecteerd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                while (datumGeselecteerd.DayOfWeek != DayOfWeek.Monday)
                    datumGeselecteerd = datumGeselecteerd.AddDays(1);
            }

            updateTekst();
        }

        public static void Vorige()
        {
            if (IsWeekWeergave)
            {
                weekWeergave.BewaarTekst();
                datumGeselecteerd = datumGeselecteerd.AddDays(-7);
            }
            else
            {
                datumGeselecteerd = datumGeselecteerd.AddDays(-28);
                if (datumGeselecteerd.Day > 7)
                    datumGeselecteerd = datumGeselecteerd.AddDays(-7);
            }

            updateTekst();
        }

        public static void Volgende()
        {
            if (IsWeekWeergave)
            {
                weekWeergave.BewaarTekst();
                datumGeselecteerd = datumGeselecteerd.AddDays(7);
            }
            else
            {
                datumGeselecteerd = datumGeselecteerd.AddDays(28);
                if (datumGeselecteerd.Day > 7)
                    datumGeselecteerd = datumGeselecteerd.AddDays(7);
            }

            updateTekst();
        }

        public static void WisselWeergave()
        {
            if (IsWeekWeergave)
            {
                IsWeekWeergave = false;
                weekWeergave.BewaarTekst();
                while (datumGeselecteerd.Day > 7)
                    datumGeselecteerd = datumGeselecteerd.AddDays(-7); // zet datum op eerste maandag v/d maand
                maandWeergave.BringToFront();
            }
            else
            {
                IsWeekWeergave = true;
                weekWeergave.BringToFront();
            }

            updateTekst();
            if (Gewisseld != null)
                Gewisseld(null, null);
        }

        public static void BewaarWijzigingen()
        {
            if (IsWeekWeergave)
                weekWeergave.BewaarTekst();
        }

        private static void updateTekst()
        {
            if (IsWeekWeergave)
                weekWeergave.UpdateTekst();
            else
                maandWeergave.UpdateTekst();
        }

        public static int GeefPaasZondag(int jaar) // geeft het dagnummer vanaf 1 januari
        {
            int resultaat;
            if (paasZondagen.TryGetValue(jaar, out resultaat))
                return resultaat;

            // resultaat moet berekend worden
            int dagenJanFeb = jaar % 4 > 0 ? 59 : 60;
            if (jaar % 100 == 0 && jaar % 400 > 0)
                dagenJanFeb = 59;

            int G = (jaar % 19) + 1;
            int C = (jaar / 100) + 1;
            int X = (C * 3) / 4 - 12;
            int Y = ((8 * C) + 5) / 25 - 5;
            int Z = (5 * jaar) / 4 - X - 10;
            int E = (11 * G + 20 + Y - X) % 30;
            if (E == 24 || (E == 25 && G > 11))
                E += 1;
            int N = 44 - E;
            if (N < 21)
                N += 30;
            int P = N + 7 - ((Z + N) % 7); // dagnummer in maart; of april als P > 31

            resultaat = dagenJanFeb + P;
            paasZondagen[jaar] = resultaat;
            return resultaat;
        }

        public static string GeefFeestdag(DateTime datum, int paasZondag)
        {
            string tekst = string.Empty;

            if (datum.DayOfYear == 1)
                tekst = "Nieuwjaarsdag";
            else if (datum.Month == 4 && datum.Day == 27 && datum.Year >= 2014)
                tekst = "Koningsdag";
            else if (datum.Year <= 2013 && datum.Month == 4 && datum.Day == 30)
                tekst = "Koninginnedag";
            else if (datum.Month == 12 && datum.Day == 25)
                tekst = "Eerste Kerstdag";
            else if (datum.Month == 12 && datum.Day == 26)
                tekst = "Tweede Kerstdag";
            else if (datum.DayOfYear == paasZondag - 2)
                tekst = "Goede Vrijdag";
            else if (datum.DayOfYear == paasZondag)
                tekst = "Eerste Paasdag";
            else if (datum.DayOfYear == paasZondag + 1)
                tekst = "Tweede Paasdag";
            else if (datum.DayOfYear == paasZondag + 39)
                tekst = "Hemelvaartsdag";
            else if (datum.DayOfYear == paasZondag + 49)
                tekst = "Eerste Pinksterdag";
            else if (datum.DayOfYear == paasZondag + 50)
                tekst = "Tweede Pinksterdag";

            return tekst;
        }
    }
}
