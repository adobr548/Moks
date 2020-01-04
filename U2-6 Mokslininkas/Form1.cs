using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace U2_6_Mokslininkas
{
    public partial class Form1 : Form
    {
        /// Konstantos        
        const string CFr = "Rezultatai.txt";                                 // rezultatu failas
        const string CFu = "Uzduotis.txt";                                 // užduoties failo vardas 
        const string CFn = "Nurodymai.txt";                                 // nurodymų failo vardas 
        
        ///kintamieji(objektu masyvai)       
        private List<Mokslininkas> StraipsMas1;                              //1 mokslininko atrinkti straipsniai
        private List<Mokslininkas> StraipsMas2;                              //2 mokslininko atrinkti straipsniai 
        private List<Mokslininkas> NaujiStraipsniai;                         //Nauji straipsniai                     
        private List<Mokslininkas> NaujasRinkinys;                           //1 ir 2 mokslininko rinkinys
        
        string pavardvard1;                                                  // 1 mokslininko vardas pavarde                   
        string pavardvard2;                                                  // 2 mokslininko vardas pavarde
        string pavadinimas;                                                  // Nauju straipsniu saraso pavadinimas

        public Form1()
        {
            InitializeComponent();

            //Jei rezultatu failas egzistuoja,ji isvalo
            if (File.Exists(CFr))
                File.Delete(CFr);

            // Nurodyti meniu punktai padaromi pasyviais 
            spausdintiToolStripMenuItem.Enabled = false;
            tinkamuStraispniuKiekisToolStripMenuItem.Enabled = false;
            naujasRinkinysToolStripMenuItem.Enabled = false;
            aBCRikiavimasToolStripMenuItem.Enabled = false;
            pasalinimasToolStripMenuItem.Enabled = false;
            papildymasToolStripMenuItem.Enabled = false;
        }
        //-----------------------------------------------------
        // Grafines sasajos valdymo metodai
        //-----------------------------------------------------
        /// <summary>
        /// Meniu punkto "Baigti" atliekami veiksmai
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void baigtiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Meniu punkto "Užduotis" atliekami veiksmai: 
        /// parodomas užduoties failo turinys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uzduotisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rezultatai.LoadFile(CFu, RichTextBoxStreamType.PlainText);

            Pranesimasvartotojui.Text = " Uzduotis uzkrauta ";
        }
        /// <summary>
        /// Meniu punkto "Nurodymai vartotojui" atliekami veiksmai 
        /// parodomas nurodymų vartotojui failo turinys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nurodymaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rezultatai.LoadFile(CFn, RichTextBoxStreamType.PlainText);

            Pranesimasvartotojui.Text = "Nurodymai uzkrauti ";
        }
        /// <summary>
        /// Meniu punkto "Įvesti" atliekami veiksmai
        /// Duomenų failo vardas išrenkamas naudojant openFileDialog komponentą
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ivestiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // OpenFileDialog komponento savybių nustatymas 
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";             // filtras
            openFileDialog1.Title = "Pasirinkite duomenų failą";                               // pavadinimas
            DialogResult result = openFileDialog1.ShowDialog();
            
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";             
            openFileDialog2.Title = "Pasirinkite duomenų failą";                               
            DialogResult result2 = openFileDialog2.ShowDialog();

            OpenFileDialog openFileDialog3 = new OpenFileDialog();
            openFileDialog3.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";             
            openFileDialog3.Title = "Pasirinkite duomenų failą";                               
            DialogResult result3 = openFileDialog3.ShowDialog();

            // jeigu pasirinktas failas
            if (result == DialogResult.OK)
            {
                string fv = openFileDialog1.FileName;                             // fv - duomenu failo vardas

                //Duomenu parodymas richtextboxe                
                richTextBox1.LoadFile(fv, RichTextBoxStreamType.PlainText);
                //Nuskaitymas
                StraipsMas1 = Skaityti(fv,out pavardvard1);

            }

            if (result2 == DialogResult.OK)
            {
                string fv2 = openFileDialog2.FileName;                             
                            
                richTextBox2.LoadFile(fv2, RichTextBoxStreamType.PlainText);
                
                StraipsMas2 = Skaityti(fv2,out pavardvard2);
            }

            if (result3 == DialogResult.OK)
            {
                string fv3 = openFileDialog3.FileName;

                richTextBox3.LoadFile(fv3, RichTextBoxStreamType.PlainText);

                NaujiStraipsniai = Skaityti(fv3, out pavadinimas);

            }

            // Meniu punktų nustatymai
            ivestiToolStripMenuItem.Enabled = false;           
            tinkamuStraispniuKiekisToolStripMenuItem.Enabled = true;
            naujasRinkinysToolStripMenuItem.Enabled = true;

            Pranesimasvartotojui.Text = "Duomenys ivesti! Suformuokite nauja sarasa,"
                         +"\n kad galetumete atlikti kitus veiksmus";
        }
        /// <summary>
        /// Meniu punkto "Spausdinti" atliekami veiksmai
        /// Rezultatų failo vardas išrenkamas naudojant saveFileDialog komponentą
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spausdintiToolStripMenuItem_Click(object sender, EventArgs e)
        {


            // SaveFileDialog komponento savybių nustatymas 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";        //filtras
            saveFileDialog1.Title = "Pasirinkite txt rezultatų failą";                         //pavadinimas
            DialogResult result = saveFileDialog1.ShowDialog();

            // jeigu pasirinktas failas 
            if (result == DialogResult.OK)
            {
                string fv = saveFileDialog1.FileName;                          // fv - rezultatu failo vardas

                if (File.Exists(fv))
                    File.Delete(fv);

                if (StraipsMas1.Count > 0)
                {
                    Spausdinti(fv, StraipsMas1, " Pirmo mokslininko duomenys ",pavardvard1);
                }
                else
                    MessageBox.Show("1 duomenu failas tuscias!");

                if (StraipsMas2.Count > 0)
                    Spausdinti(fv, StraipsMas2, " Antro mokslininko duomenys ",pavardvard2);
                else
                    MessageBox.Show(" 2 duomenu failas tuscias!");

                if (NaujiStraipsniai.Count > 0)
                    Spausdinti(fv, NaujiStraipsniai, " ", pavadinimas);
                else
                    MessageBox.Show(" nauju duomenu failas tuscias!");
               

                   Spausdinti(fv, NaujasRinkinys, "naujas", "");

            }

            // SaveFileDialog komponento savybių nustatymas 
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*";        //filtras
            saveFileDialog2.Title = "Pasirinkite csv rezultatų failą";
            DialogResult result2 = saveFileDialog2.ShowDialog();

            //---------------------------------------------------------------------------
            // Komponento dataGridView1 užpildymas duomenimis 
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "Nr";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Name = "Pavarde ir vardas";
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Name = "Straipsnio pavadinimas";
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].Name = "Zurnalo pavadinimas";
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Name = "Zurnalo nr";
            dataGridView1.Columns[4].Width = 40;
            dataGridView1.Columns[5].Name = "Straipsnio psl";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[6].Name = "Publikavimo metai";
            dataGridView1.Columns[6].Width = 80;

            for (int i = 0; i < NaujasRinkinys.Count; i++)
            {
                Mokslininkas mokslininkas = NaujasRinkinys[i];
                dataGridView1.Rows.Add(i + 1, mokslininkas.PavVrd, mokslininkas.StraipPav
                                            , mokslininkas.ZurnalPav, mokslininkas.ZurnalNr
                                            , mokslininkas.StraipPsl, mokslininkas.Metai);
            }

            //---------------------------------------------------------------------------
            // jeigu pasirinktas failas 
            if (result2 == DialogResult.OK)
            {
                string fv = saveFileDialog2.FileName;                   // fv - csv rezultatu failo vardas

                //jei reikia isvalo faila
                if (File.Exists(fv))
                    File.Delete(fv);
               
                
                string csv = string.Empty;                             //tuscia stringas

                //suformuoja stulpelius pagal duomenu pavadinimus
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csv += column.HeaderText + ';';
                }

                //nauja linija
                csv += "\r\n";

                //Uzpildo langelius duomenimis
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        csv += (cell.Value) + ";";
                    }

                    
                    csv += "\r\n";
                }

                //Suraso i csv faila suformuota lentele
                File.WriteAllText(fv, csv);
            }

            Pranesimasvartotojui.Text = " Galutiniai duomenys atspausdinti failuose ";

        }
        /// <summary>
        /// Menu punkto "TinkamuStraipsniuKiekis" atliekami veiksmai
        /// Sis menu punktas atrenka tinkamus straipsnius pagal vartotojo ivestus metus ir puslapiu skaiciu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tinkamuStraispniuKiekisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Klaidos isvedimas jei textboxe yra tuscia vieta
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Klaida! Prasom ivesti metus");

            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Klaida! Prasom ivesti puslapiu skaiciu");

            }

          
            // kintamieji,kuriu reiksmes iveda vartotojas
            int metai = 0;
            int puslapiusk = 0;

            //Jei vartotojas ivede ne int tipo reiksme ismetamas pranesimas
            if (!Int32.TryParse(textBox1.Text, out metai))
                MessageBox.Show("Klaida! Blogai iraset metus");

            if (!Int32.TryParse(textBox2.Text, out puslapiusk))
                MessageBox.Show("Klaida! Blogai iraset puslapiu skaiciu");

            //Skaiciuoja kiek kiekvieno mokslininko sarase yra tinkamu straipsniu
            using (var fr = File.AppendText(CFr))
            {
                int kiekis1 = Kiekis(StraipsMas1, metai, puslapiusk);

                if (kiekis1 > 0)
                {
                    label1.Text = " 1 mokslininko tinkamiausiu straipsniu skaicius: " + kiekis1.ToString();
                    fr.WriteLine(" 1 mokslininko tinkamiausiu straipsniu skaicius: {0} ", kiekis1);
                }
                else
                {
                    label1.Text = " Nera tinkamu straipsniu ";
                    fr.WriteLine(" Nera tinkamu straipsniu ");
                }

                int kiekis2 = Kiekis(StraipsMas2, metai, puslapiusk);

                if (kiekis2 > 0)
                {
                    label2.Text = " 2 mokslininko tinkamiausiu straipsniu skaicius: " + kiekis2.ToString();
                    fr.WriteLine(" 2 mokslininko tinkamiausiu straipsniu skaicius: {0} ", kiekis2);
                }
                else
                {
                    label2.Text = " Nera tinkamu straipsniu ";
                    fr.WriteLine(" Nera tinkamu straipsniu ");
                }

            }

            Pranesimasvartotojui.Text = " Skaiciavimai atlikti ! ";
           

        }
        /// <summary>
        /// Menu punkto "Naujas rinkinys" atliekami veiksmai
        /// Suformuoja nauja sarasa pagal salyga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void naujasRinkinysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Formuoja nauja rinkini
            NaujasRinkinys = Formuoti(StraipsMas1, StraipsMas2);
            //Atspausdina txt faile
            Spausdinti(CFr, NaujasRinkinys, " naujas", "");
            //Atspausdina Ekrane                     
            Rezultatai.AppendText("\nNaujas sarasas\n");
            for (int i = 0; i < NaujasRinkinys.Count; i++)
            {
                Mokslininkas mok = NaujasRinkinys[i];
                Rezultatai.AppendText(i+1 + " " + mok.ToString() + "\n");
            }
            // Meniu punktų nustatymai
            spausdintiToolStripMenuItem.Enabled = true;
            aBCRikiavimasToolStripMenuItem.Enabled = true;
            pasalinimasToolStripMenuItem.Enabled = true;
            papildymasToolStripMenuItem.Enabled = true;

            Pranesimasvartotojui.Text = " Suformuotas naujas rinkinys !"
                         +"\n Dabar galite atlikti likusius veiksmus";
         
        }

        /// <summary>
        /// Menu punkto "ABC rikiavimas" aliekami veiksmai
        /// Rikiuoja sarasa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aBCRikiavimasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Rikiuoja burbuliuko metodu
            Rikiuoti(NaujasRinkinys);
            //Spausdina txt faile
            Spausdinti(CFr, NaujasRinkinys, "rikiuotas naujas", "");
            //spausdina ekrane
            Rezultatai.AppendText("\nNaujas sarasas po rikiavimo\n");
            for (int i = 0; i < NaujasRinkinys.Count; i++)
            {
                Mokslininkas mok = NaujasRinkinys[i];
                Rezultatai.AppendText(i + 1 + " " + mok.ToString() + "\n");
            }

            Pranesimasvartotojui.Text = "Sarasas surikiuotas ";
        }
        /// <summary>
        /// menu punto "Pasalinimas" atliekami veiksmai
        /// Salina pagal vartotojo ivesta raide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pasalinimasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Klaidos isvedimas jei textboxe yra tuscia vieta

            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Klaida! Prasom ivesti raide");
              
            }

            else
            {
                //vartotojo ivesta raide
                string raide = textBox3.Text;
                //salinimas
                Salinti(NaujasRinkinys, raide);
                // spausdinimas txt faile
                Spausdinti(CFr, NaujasRinkinys, " naujas po salinimo", "");
                //spausdinimas ekrane
                Rezultatai.AppendText("\nNaujas sarasas po pasalinimo\n");
                for (int i = 0; i < NaujasRinkinys.Count; i++)
                {
                    Mokslininkas mok = NaujasRinkinys[i];
                    Rezultatai.AppendText(i + 1 + " " + mok.ToString() + "\n");
                }

                Pranesimasvartotojui.Text = " Straipsniai pasalinti";
            }
        }
        /// <summary>
        /// Menu punkto "Papildymas" atliekami veiksmai
        /// papildo surikiuota sarasa naujais straipsniais pagal vartotojo ivesta raide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void papildymasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Klaidos isvedimas jei textboxe yra tuscia vieta
            if (textBox4.Text.Trim() == "")
            {
                MessageBox.Show("Klaida! Prasom ivesti raide");

            }
            else
            {

                string raide = textBox4.Text;
                //Iterpimas
                Iterpti(NaujasRinkinys, NaujiStraipsniai, raide);
                //Spausdina txt faile
                Spausdinti(CFr, NaujasRinkinys, " naujas po iterpimo", "");
               //Spausdina ekrane
                Rezultatai.AppendText("\nNaujas sarasas po papildymo\n");
                for (int i = 0; i < NaujasRinkinys.Count; i++)
                {
                    Mokslininkas mok = NaujasRinkinys[i];
                    Rezultatai.AppendText(i + 1 + " " + mok.ToString() + "\n");
                }

                Pranesimasvartotojui.Text = " Straipsniai papildyti ";
            }
        }
       
        //------------------------------------------------------------
        // Uzduoties metodai
        //------------------------------------------------------------
        /// <summary>
        ///  Skaito visus duomenis is failo i List
        /// </summary>
        /// <param name="fv">duomenu failo vardas</param>
        /// <param name="pav">mokslininko vardas pavarde</param>
        /// <returns>grazina suformuoto studento List nuoroda</returns>
        static List<Mokslininkas> Skaityti(string fv,out string pav)
        {

            List<Mokslininkas> Moks = new List<Mokslininkas>();
            using (StreamReader reader = new StreamReader(fv, Encoding.GetEncoding(1257)))
            {

                pav = reader.ReadLine();
                string eilute;                                                // visa duomenu failo eilute
                while ((eilute = reader.ReadLine()) != null)
                {
                    string[] eilDalis = eilute.Split(';');
                    string pavVrd = eilDalis[0];
                    string straipPav = eilDalis[1];
                    string zurnalPav = eilDalis[2];
                    int zurnalNr = int.Parse(eilDalis[3]);
                    int straipPsl = int.Parse(eilDalis[4]);
                    int metai = int.Parse(eilDalis[5]);
                    Mokslininkas mokslinink = new Mokslininkas(pavVrd, straipPav, zurnalPav, zurnalNr, straipPsl, metai);
                    Moks.Add(mokslinink);
                }
            }

            return Moks;
        }
        /// <summary>
        /// Spausdina List duomenis lentele faile.
        /// </summary>
        /// <param name="fv">rezultatu failo vardas</param>
        /// <param name="List">moksliniko List</param>
        /// <param name="antraste">uzrasas virs lenteles</param>
        /// <param name="pav">mokslininko vardas pavarde</param>
        static void Spausdinti(string fv, List<Mokslininkas> List, string antraste,string pav)
        {
            string eilute = new string('-', 65);
            using (var fr = new StreamWriter(File.Open(fv, FileMode.Append), Encoding.GetEncoding(1257)))
            {

                fr.WriteLine("\n" + antraste);               
                fr.WriteLine("{0}",pav);
                fr.WriteLine(eilute);
                fr.WriteLine("Nr Autoriaus        Straipsnio        Žurnalo pav. Nr Psl. Metai");
                fr.WriteLine(" vardas ir pavarde  pavadinimas                      Skaic.     ");
                fr.WriteLine(eilute);
                for (int i = 0; i < List.Count; i++)
                {
                    Mokslininkas mok = List[i];
                    fr.WriteLine("{0} {1}", i + 1, mok);
                }
                fr.WriteLine(eilute);
                
            }

        }

        

        /// <summary>
        /// Skaiciuoja kiek mokslininko sarase yra tinkamu straipsniu
        /// </summary>
        /// <param name="List">mokslininko List</param>
        /// <param name="met">metai</param>
        /// <param name="psl">puslapiai</param>
        /// <returns>grazina kieki</returns>
        static int Kiekis(List<Mokslininkas> List, int met, int psl)
        {
            int kiek = 0;
            for (int i = 0; i < List.Count; i++)
            {
                Mokslininkas mok = List[i];
                if (met >= mok.Metai && psl >= mok.StraipPsl)
                    kiek++;
            }
            return kiek;
        }

  
        /// <summary>
        /// Suraso duomenis i nauja List
        /// </summary>
        /// <param name="L1">1 List</param>
        /// <param name="L2">2 List</param>
        /// <param name="New">naujas List</param>
        static List<Mokslininkas> Formuoti(List<Mokslininkas> L1, List<Mokslininkas> L2)
        {
            List<Mokslininkas> New = new List<Mokslininkas>();

            int i = 0;
            int j = 0;

            int m1 = SeniausoStraipsioPaieska(L1).Metai;
            int m2 = SeniausoStraipsioPaieska(L2).Metai;

            int senpsl1 = SeniausoStraipsioPaieska(L1).StraipPsl;
            int senpsl2 = SeniausoStraipsioPaieska(L2).StraipPsl;

            while ((i < L1.Count) && (j < L2.Count))
            {
                if (L1[i] < L2[j])
                {
                    Mokslininkas mok1 = L1[i];


                    int psl1 = mok1.StraipPsl;

                    if (m1 < m2)
                    {
                        if (psl1 <= senpsl1)
                        {
                            New.Add(mok1);
                        }
                    }
                    else
                    {
                        if (psl1 <= senpsl2)
                        {
                            New.Add(mok1);
                        }
                    }
                    i = i + 1;
                }
                else
                {
                    Mokslininkas mok2 = L2[j];


                    int psl2 = mok2.StraipPsl;

                    if (m1 < m2)
                    {
                        if (psl2 <= senpsl1)
                        {
                            New.Add(mok2);
                        }
                    }
                    else
                    {
                        if (psl2 <= senpsl2)
                        {
                            New.Add(mok2);
                        }
                    }
                    j = j + 1;
                }
                while (i < L1.Count)
                {
                    Mokslininkas mok1 = L1[i];


                    int psl1 = mok1.StraipPsl;

                    if (m1 < m2)
                    {
                        if (psl1 <= senpsl1)
                        {
                            New.Add(mok1);
                        }
                    }
                    else
                    {
                        if (psl1 <= senpsl2)
                        {
                            New.Add(mok1);
                        }
                    }
                    i = i + 1;
                }
                while (j < L2.Count)
                {
                    Mokslininkas mok2 = L2[j];


                    int psl2 = mok2.StraipPsl;

                    if (m1 < m2)
                    {
                        if (psl2 <= senpsl1)
                        {
                            New.Add(mok2);
                        }
                    }
                    else
                    {
                        if (psl2 <= senpsl2)
                        {
                            New.Add(mok2);
                        }
                    }
                    j = j + 1;
                }


            }
            return New;
        }

        /// <summary>
        /// Seniausio straipsnio paieska
        /// </summary>
        /// <param name="mok">mokslininko List </param>
        /// <returns>grazina seiniausio indeksa</returns>
        static Mokslininkas SeniausoStraipsioPaieska(List<Mokslininkas> mok)
        {
            Mokslininkas seniausiasst = mok[0];

            for (int i = 0; i < mok.Count; i++)
            {
                Mokslininkas seniausiasstr = mok[i];

                if (seniausiasstr.Metai < seniausiasst.Metai)
                {
                    seniausiasst = seniausiasstr;
                }
            }

            return seniausiasst;
        }
        /// <summary>
        /// Rikiavimas burbuliuko metodu
        /// </summary>
        /// <param name="M">mokslininku List</param>
        static void Rikiuoti(List<Mokslininkas> M)
        {
            int i = 0;
            bool bk = true;
            while (bk)
            {
                bk = false;
                for (int j = M.Count - 1; j > i; j--)
                    if (M[j] < M[j - 1])
                    {
                        bk = true;
                        Mokslininkas moks = M[j];
                        M[j] = M[j - 1];
                        M[j - 1] = moks;
                    }
                i++;
            }
        }


    /// <summary>
    /// Salinimas pagal raide
    /// </summary>
    /// <param name="M">moksliniku List</param>
    /// <param name="x">raide</param>
        static void Salinti(List<Mokslininkas> M, string x)
        {

            for (int i = 0; i < M.Count; i++)
            {
                if (M[i].StraipPav.ToString()[0] == x.ToString()[0])
                {
                    //Salina
                    M.RemoveAt(i);

                }

            }
        }
        /// <summary>
        /// Iterpia pagal raide
        /// </summary>
        /// <param name="M">Sarasas i kuri iterpia</param>
        /// <param name="M2">Sarasas is kurio imami elementai</param>
        /// <param name="x">raide</param>
        static void Iterpti(List<Mokslininkas> M,List<Mokslininkas> M2, string x)
        {
           

            for (int i = 0; i < M2.Count; i++)
            {
                if (M2[i].StraipPav.ToString()[0] == x.ToString()[0])
                {
                    //Iterpia
                    M.Insert(i, M2[i]);

                }

            }
        }




        

    }
}

