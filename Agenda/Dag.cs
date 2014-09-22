using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agenda
{
    class Dag : IComparable<Dag>
    {
        public DateTime Datum { get; private set; }
        public string[] Tekst { get; private set; }

        public Dag(DateTime datum, string[] tekst)
        {
            Datum = datum;
            Tekst = tekst;
        }

        public Dag(DateTime datum)
        {
            Datum = datum;
        }

        public int CompareTo(Dag other)
        {
            return Datum.CompareTo(other.Datum);
        }
    }
}
