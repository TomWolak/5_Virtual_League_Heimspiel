using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _5_Virtual_League_Heimspiel
{
    public enum PlayerRolle
    {
        Torwart,
        Abwehrspieler,
        Mittelfeldspieler,
        Stuermer
    }

    public enum Nation
    {
        Belaruss,
        Deutschland,
        Finnland,
        Island,
        Norwegen,
        Polen,
        Slovakei,
        Schweden,
        Spanien,
        Tschechien,
        Ukraine
    }

    public static class RandomProvider   // eine Klasse, die Objekte vom Typ Random bereitstellt
    {
        public static Random Random => random;                //  property
        private static readonly Random random = new Random();  //  Field, privat, unveränderbar
    }

    public class Player
    {
        public string Name   // property, readonly
        {
            get
            {
                return name;
            }
        }

        public int SpielerStaerke => spielerStaerke;

        public PlayerRolle Rolle
        {
            get => rolle;
            set => rolle = value;
        }

        public DateTime Geburtsdatum => geburtsdatum;    //  property

        public Player(string name)   // Basiskonstruktor, enthält gemeinsame Funktionen Player verschiedener Typen
        {
            this.name = name;
            Random random = RandomProvider.Random;
            int tag = random.Next() % 28 + 1;                // Bereich von Monatstage
            int monat = random.Next() % 12 + 1;              // Bereich von Monaten
            int jahr = random.Next() % 21 + 1980;            // Jahr-Bereich; der älteste Spieler könnte 1980 geboren sein
            geburtsdatum = new DateTime(jahr, monat, tag);   // Field
            spielerStaerke = random.Next() % 9 + 1;          // Random-Spielerstaerke im Bereich 1-9
        }

        public string getFunction()            // Methode, diese Methode gibt Art des Spielers zurück
        {
            throw new NotImplementedException(nameof(getFunction));    // Ausnahme
        }

        public int PlayerAlter()      // Methode
        {
            int alter = DateTime.Now.Year - geburtsdatum.Year;
            return alter;
        }

        private string name;
        private PlayerRolle rolle;       // Fields
        private DateTime geburtsdatum;
        private int spielerStaerke;
    }

    public class Torwart : Player
    {
        public Torwart(string name) : base(name)
        {
            Rolle = PlayerRolle.Torwart;
        }

        public new string getFunction()     // diese Methode überschreibt die getFunction-Methode der Player-Klasse,
        {                                   // weil es vom gleichen Typ ist und die gleichen Argumente verwendet
            return Rolle.ToString();
        }
    }

    public class Abwehrspieler : Player
    {
        public Abwehrspieler(string name) : base(name)
        {
            Rolle = PlayerRolle.Abwehrspieler;
        }

        public new string getFunction()
        {
            return Rolle.ToString();
        }
    }

    public class Mittelfeldspieler : Player
    {
        public Mittelfeldspieler(string name) : base(name)
        {
            Rolle = PlayerRolle.Mittelfeldspieler;
        }

        public new string getFunction()
        {
            return Rolle.ToString();
        }
    }

    public class Stuermer : Player
    {
        public Stuermer(string name) : base(name)
        {
            Rolle = PlayerRolle.Stuermer;
        }

        public new string getFunction()
        {
            return Rolle.ToString();
        }
    }

    public class Team
    {
        public Nation Nation => nation;   // property, readonly

        public static Team Create(Nation nation)   // Die statische Methode wird verwendet, um den Konstruktor 'private Team(Nation ...)' aufzurufen
        {
            Team resultat = new Team(nation);
            return resultat;
        }

        public Team(Nation nation)    // Konstruktor
        {
            this.nation = nation;
            players = new List<Player>();

            players.Add(new Torwart("Torwart"));

            players.Add(new Abwehrspieler("Abwehspieler1"));
            players.Add(new Abwehrspieler("Abwehspieler2"));
            players.Add(new Abwehrspieler("Abwehspieler3"));
            players.Add(new Abwehrspieler("Abwehspieler4"));

            players.Add(new Mittelfeldspieler("Mittelfeldspieler1"));
            players.Add(new Mittelfeldspieler("Mittelfeldspieler1"));
            players.Add(new Mittelfeldspieler("Mittelfeldspieler1"));
            players.Add(new Mittelfeldspieler("Mittelfeldspieler1"));

            players.Add(new Stuermer("Stuermer1"));
            players.Add(new Stuermer("Stuermer2"));
        }

        public int getPower()
        {
            int summeDerSpielerStaerke = 0;
            for (int i = 0; i < players.Count; i++)
            {
                summeDerSpielerStaerke += players[i].SpielerStaerke;
            }
            return summeDerSpielerStaerke;
        }

        public string EntnehmenAlsAufschrift()
        {
            string resultat = "Unten sind die Players von Nation: " + nation + ". Gesamtstaerke der Mannshaft: " + getPower().ToString() + "\n";
            for (int i = 0; i < players.Count; i++)
            {
                resultat += ("Spielername: " + players[i].Name + ", " + "Spielerart: " + players[i].Rolle + ", " + "Spieler-alter: " + players[i].PlayerAlter() + ", " + "Spieler-Staerke: " + players[i].SpielerStaerke + " " + "\n");
            }
            return resultat;
        }

        private Nation nation;         // Field >>> enum
        private List<Player> players;
    }

    public class Turnier
    {
        public Turnier(List<Team> teilnehmer, string filePath)   // Konstruktor
        {
            if (teilnehmer.Count != 8)
            {
                throw new ArgumentException(nameof(teilnehmer));
            }

            List<int> nummern = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            mannschaften = new SortedDictionary<int, Team>();

            file = new FileInfo(filePath);     //  ein neues Obiekt Klasse FileInfo
            File.WriteAllText(file.FullName, string.Empty);

            foreach (Team team in teilnehmer)      // eine Schleife, die Teams iterieren
            {
                int index = RandomProvider.Random.Next() % nummern.Count;  // In diesem Schritt wählen wir den Startnummernindex für das betreffende Team aus der Liste „nummern“ aus

                mannschaften[nummern[index]] = team;
                nummern.RemoveAt(index);
            }
        }

        public void machtTurnier()    // Methode mit einem Algorithmus
        {
            foreach (var team in mannschaften)    // diese Schleife wird die Mannschaften dokumentiert, sowie deren Startnummern
            {
                string raport = team.Value.EntnehmenAlsAufschrift();
                raport += "Dieses Team wird mit der Startnummer herauskommen: " + team.Key.ToString() + "\n\n";
                Dokumentieren(raport);
            }

            int winnerNr = 0;

            List<int> mannschaftList = mannschaften.Select(f => f.Key).ToList();
            while (mannschaftList.Count > 1)
            {
                List<int> mannschaftList2 = new List<int>();
                for (int i = 0; i < mannschaftList.Count / 2; i++)
                {
                    int a = i * 2;
                    int b = (i * 2) + 1;
                    int aNr = mannschaftList[a];
                    int bNr = mannschaftList[b];
                    int w = GetSiegerDesDuells(aNr, bNr);

                    mannschaftList2.Add(w);
                }

                mannschaftList = mannschaftList2;
            }

            winnerNr = mannschaftList[0];

            string raport1 = string.Format("Der Gewinner ist das Team: {0} - {1}", winnerNr, mannschaften[winnerNr].Nation.ToString());
            Dokumentieren(raport1);
        }

        private void Dokumentieren(string text)    // (Methode) in Datei schreiben
        {
            using (FileStream sw = new FileStream(file.FullName, FileMode.Append))
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                sw.Write(data, 0, data.Length);
            }
        }

        private int GetSiegerDesDuells(int team1Nr, int team2Nr)   //  Methode, mit der wird den Sieger des Duells ermitteln
        {
            int team1PerformancePercent = RandomProvider.Random.Next() % 51 + 50;  // Hier weisen wir zufällig einen Leistungsprozentsatz zu
            int team2PerformancePercent = RandomProvider.Random.Next() % 51 + 50;

            Team team1 = mannschaften[team1Nr];
            Team team2 = mannschaften[team2Nr];

            double team1Performance = (double)team1.getPower() * (double)team1PerformancePercent / 100d;   // Berechnung der Endleistung des Teams
            double team2Performance = (double)team2.getPower() * (double)team2PerformancePercent / 100d;

            string raport = string.Format("Duell {0} - {1} \n", team1Nr, team2Nr);
            Dokumentieren(raport);

            int winnerNr;

            if (team1Performance > team2Performance)
            {
                winnerNr = team1Nr;
            }
            else
            {
                winnerNr = team2Nr;
            }

            raport = string.Format("Der Gewinner: {0} \n", winnerNr);
            Dokumentieren(raport);
            return winnerNr;
        }

        private FileInfo file;
        private SortedDictionary<int, Team> mannschaften;   // Dictionary
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Team> teilnehmer = new List<Team>();
            teilnehmer.Add(new Team(Nation.Belaruss));
            teilnehmer.Add(new Team(Nation.Deutschland));
            teilnehmer.Add(new Team(Nation.Finnland));
            teilnehmer.Add(new Team(Nation.Island));
            teilnehmer.Add(new Team(Nation.Norwegen));
            teilnehmer.Add(new Team(Nation.Polen));
            teilnehmer.Add(new Team(Nation.Tschechien));
            teilnehmer.Add(new Team(Nation.Schweden));

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Virtual-League.txt");
            Turnier turnier = new Turnier(teilnehmer, path);  // Objekt der Klasse Turnier, Teilnehmer + Dateipfad

            turnier.machtTurnier();

            Console.WriteLine("Hallo!");
            Console.WriteLine("Turnierergebnisse sind im Ordner „Dokumente“ als Textdatei 'Virtual-League' verfügbar  :) ");

            Console.ReadLine();
        }
    }
}