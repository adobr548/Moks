using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U2_6_Mokslininkas
{
    /// <summary>
    ///  Klase vieno mokslininko duomenims saugoti
    /// </summary>
    class Mokslininkas
    {
        public string PavVrd    { get; set; }                              //Savybes:Straipsnio autoriaus vardas ir pavarde
        public string StraipPav { get; set; }                              //Straipsnio pavadinimas
        public string ZurnalPav { get; set; }                              //Zurnalo pavadinimas
        public int ZurnalNr     { get; set; }                              //Zurnalo Numeris
        public int StraipPsl    { get; set; }                              //Straipsnio puslapiu skaicius
        public int Metai        { get; set; }                              //Publikavimo metai

        /// <summary>
        /// Klases konstruktorius: savybems suteikiam reiksmes
        /// </summary>
        /// <param name="pavv">pavarde ir vardas</param>
        /// <param name="straippav">straipsnio pavadinimas</param>
        /// <param name="zurnalpav">zurnalo pavadinimas</param>
        /// <param name="zurnalnr">zurnalo numeris</param>
        /// <param name="straipsl">straipsnio puslapiu skaicius</param>
        /// <param name="metai">publikavimo metai</param>
        public Mokslininkas(string pavv, string straippav, string zurnalpav, int zurnalnr,int straipsl,int metai)
        {
            PavVrd = pavv;
            StraipPav = straippav;
            ZurnalPav = zurnalpav;
            ZurnalNr = zurnalnr;
            StraipPsl = straipsl;
            Metai = metai;
        }

        /// <summary>
        /// Uzklotas operatorius ToString()
        /// </summary>
        /// <returns>grazina suformuota eilute</returns>
        public override string ToString()
        {
            string eilute;
            eilute = String.Format("{0} {1} {2} {3} {4,3} {5,6}",
                                    PavVrd, StraipPav, ZurnalPav, ZurnalNr, StraipPsl, Metai);
            return eilute;
        }
      
        /// <summary>
        /// Uzklotas palyginimo operatorius 
        /// </summary>
        /// <param name="m1">1 Zurnalo pavadinimas</param>
        /// <param name="m2">2 Zurnalo pavadinimas</param>
        /// <returns>grazina pozicija</returns>
        public static bool operator >(Mokslininkas m1, Mokslininkas m2)
        {
            int poz = String.Compare(m1.ZurnalPav, m2.ZurnalPav, StringComparison.CurrentCulture);
            return (poz > 0);
        }

        /// <summary>
        ///  Uzklotas palyginimo operatorius 
        /// </summary>
        /// <param name="m1">1 Zurnalo pavadinimas</param>
        /// <param name="m2">2 Zurnalo pavadinimas</param>
        /// <returns>grazina pozicija</returns>
        public static bool operator <(Mokslininkas m1, Mokslininkas m2)
        {
            int poz = String.Compare(m1.ZurnalPav, m2.ZurnalPav, StringComparison.CurrentCulture);
            return (poz < 0);
        }
                    
    }
}
