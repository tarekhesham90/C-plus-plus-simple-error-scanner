using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace scanner
{
    public partial class Form1 : Form
    {
        //public string error;
        string[] lines;
        string dec1 = @"(public |private |protected |internal )?(int |double |decimal |float |long |short )\s*(_*[a-zA-Z]+[0-9]*)+\s*(=\s*[0-9]+)?\s*;";
        string dec2 = "(public |private |protected |internal )?(string )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\"(\\s*[a-zA-Z0-9]+\\s*)*\")?;";
        string dec3 = "(public |private |protected |internal )?(char )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\'[a-zA-Z0-9]\')?\\s*;";
        string dec4 = "(public |private |protected |internal )?(bool )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*(true|false))?\\s*;";
        string exp1 = @"[a-zA-Z]+=[a-zA-Z0-9]+((\*|/|\+|\-)[a-zA-Z0-9]+)*;";
        string cond = @"[a-zA-Z0-9]+\s*((==|\|\||&&|>|<|<=|>=|=>|=<)\s*[a-zA-Z0-9]+)*";
        public int counter = 1;
        
        public Form1()
        {
            InitializeComponent();
            lines = new string[100000];
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            int rowCount = dataGridView1.RowCount ;
            for (int i = 0; i < rowCount; i++)
            {

                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);

            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file=null;
            openFileDialog1.Filter = "C++ files (*.cpp)|*.cpp|Header files (*.h)|*.h";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
            }
            try
            {
                TextReader tr = new StreamReader(file);
                textBox1.Text = tr.ReadToEnd();
                tr.Close();
            }
            catch { }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = null;
            saveFileDialog1.Filter = "C++ files (*.cpp)|*.cpp|Header files (*.h)|*.h";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file = saveFileDialog1.FileName;
            }
            try
            {
                TextWriter tr = new StreamWriter(file);
                tr.WriteLine(textBox1.Text);
                tr.Close();
            }
            catch { }

        }

        private void checkErrorsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int count =disceret(textBox1.Text);
            int rowCount = dataGridView1.RowCount ;
            counter = 0;
            for (int i = 0; i < rowCount; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            for ( counter = 0; counter < count; counter++) 
            {
                if (!Regex.Match(lines[counter],@"(\w)+").Success) 
                    continue;
                if (isif() ||iswh()|| isfor() || isdec() ||isexp() )
                      continue;
                  else
                      dataGridView1.Rows.Add( "sytax", counter + 1);
                
            }
                
        }

        private int  disceret(string text)
        {
            int count=0;
            lines = textBox1.Text.Split('\n');
            count = lines.Length;
            return count;
        }
    
        public bool isdec() 
        {
            try
            {
                if (Regex.Match(lines[counter], @"\W+\s*(public |private |protected |internal )?(int |double |decimal |float |long |short )\s*(_*[a-zA-Z]+[0-9]*)+\s*(=\s*[0-9]+)?\s*;").Success || Regex.Match(lines[counter], "\\W+\\s*(public |private |protected |internal )?(string )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\"(\\s*[a-zA-Z0-9]+\\s*)*\")?\\s*;").Success || Regex.Match(lines[counter], "\\W+\\s*(public |private |protected |internal )?(char )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\'[a-zA-Z0-9]\')?\\s*;").Success || Regex.Match(lines[counter], "\\W+\\s*(public |private |protected |internal )?(bool )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*(true|false))?\\s*;").Success)
                    return false;
                else if (Regex.Match(lines[counter], @"(public |private |protected |internal )?(int |double |decimal |float |long |short )\s*(_*[a-zA-Z]+[0-9]*)+\s*(=\s*[0-9]+)?\s*;").Success || Regex.Match(lines[counter], "(public |private |protected |internal )?(string )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\"(\\s*[a-zA-Z0-9]+\\s*)*\")?\\s*;").Success || Regex.Match(lines[counter], "(public |private |protected |internal )?(char )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*\'[a-zA-Z0-9]\')?\\s*;").Success || Regex.Match(lines[counter], "(public |private |protected |internal )?(bool )\\s*(_*[a-zA-Z]+[0-9]*)+\\s*(=\\s*(true|false))?\\s*;").Success)
                    return true;
                else
                {
                    return false;
                }
            }
            catch { return true; }
        }
        public bool isexp()
        {
            try
            {
                if (Regex.Match(lines[counter], @"\W+\s*[a-zA-Z0-9]+\s*=\s*[a-zA-Z0-9]+((\*|/|\+|\-)[a-zA-Z0-9]+)*\s*;").Success)
                    return false;
                else if (Regex.Match(lines[counter], @"\s*[a-zA-Z0-9]+\s*=\s*[a-zA-Z0-9]+((\*|/|\+|\-)[a-zA-Z0-9]+)*\s*;").Success)
                    return true;
                else
                {
                    return false;
                }
            }
            catch { return true; }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1-Tarek Hesham Abd el Aziz \n\n2-Eslam Mohamed Ramadan \n3-Motasem Bellah Hesham \n4-Mostafa Gamal \n5-Ahmed Abod Saad\n6-Islam Mohamed Abd allah \n7-Abo Bakr Abd el montaser ");
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string str;

                TextReader tr = new StreamReader("read me.txt");
                str = tr.ReadToEnd();
                tr.Close();

            Form2 frm = new Form2(str);
            frm.ShowDialog();
        }
        public bool isif()
        {
            
            try
            {
                if (Regex.Match(lines[counter], @"\s*if\s*\(\s*" + cond + @"\s*\)\s*{\s*(((" + dec1 + @")|(" + dec2 + ")|(" + dec3 + ")|(" + dec4 + ")|(" + exp1 + @"))\s*)*\s*}\s*").Success)
                    return true;
                else if (Regex.Match(lines[counter], @"(\s*if\s*\(\s*" + cond + @"\s*\)\s*)").Success)
                {
                    counter++;
                    while (counter < lines.Length)
                    {
                        if (!Regex.Match(lines[counter], @"(\w)+").Success)
                        {
                            counter++;
                            continue;
                        }
                        else if (Regex.Match(lines[counter], @"{").Success)
                        {
                            while (counter < lines.Length)
                            {
                                if (!Regex.Match(lines[counter], @"(\w)+").Success)
                                {
                                    counter++;
                                }
                                else if (Regex.Match(lines[counter], @"}").Success)
                                {
                                    counter++;
                                    return true;
                                }
                                else if (isif() || iswh() || isfor() || isdec() || isexp())
                                {
                                    counter++;
                                }
                                else
                                {
                                    counter++;
                                    dataGridView1.Rows.Add( "sytax", counter + 1);
                                }
                            }
                        }
                        return false;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch { return true; }
        }
        public bool isfor()
        {
            try
            {
                if (Regex.Match(lines[counter], @"\s*for\s*\(\s*" + dec1 + @"\s*"+cond+@"\s*;\s*[a-zA-Z]+(\+\+|\-\-)\s*\)\s*{\s*(((" + dec1 + @")|(" + dec2 + ")|(" + dec3 + ")|(" + dec4 + ")|(" + exp1 + @"))\s*)*\s*}\s*").Success)
                    return true;
                else if (Regex.Match(lines[counter], @"(\s*for\s*\(\s*" + dec1 + @"\s*" + cond + @"\s*;\s*[a-zA-Z]+(\+\+|\-\-)\s*\)\s*)").Success)
                {
                    counter++;
                    while (counter < lines.Length)
                    {
                        if (!Regex.Match(lines[counter], @"(\w)+").Success)
                        {
                            counter++;
                            continue;
                        }
                        else if (Regex.Match(lines[counter], @"{").Success)
                        {
                            while (counter < lines.Length)
                            {
                                if (!Regex.Match(lines[counter], @"(\w)+").Success)
                                {
                                    counter++;
                                }
                                else if (Regex.Match(lines[counter], @"}").Success)
                                {
                                    counter++;
                                    return true;
                                }
                                else if (isif() || iswh() || isfor() || isdec() || isexp())
                                {
                                    counter++;
                                }
                                else
                                {
                                    counter++;
                                    dataGridView1.Rows.Add("sytax", counter + 1);
                                }
                            }
                        }
                        return false;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch { return true; }
        }
        public bool iswh()
        {

            try
            {
                if (Regex.Match(lines[counter], @"\s*while\s*\(\s*" + cond + @"\s*\)\s*{\s*(((" + dec1 + @")|(" + dec2 + ")|(" + dec3 + ")|(" + dec4 + ")|(" + exp1 + @"))\s*)*\s*}\s*").Success)
                    return true;
                else if (Regex.Match(lines[counter], @"(\s*while\s*\(\s*" + cond + @"\s*\)\s*)").Success)
                {
                    counter++;
                    while (counter < lines.Length)
                    {
                        if (!Regex.Match(lines[counter], @"(\w)+").Success)
                        {
                            counter++;
                            continue;
                        }
                        else if (Regex.Match(lines[counter], @"{").Success)
                        {
                            while (counter < lines.Length)
                            {
                                if (!Regex.Match(lines[counter], @"(\w)+").Success)
                                {
                                    counter++;
                                }
                                else if (Regex.Match(lines[counter], @"}").Success)
                                {
                                    counter++;
                                    return true;
                                }
                                else if (isif() || iswh() || isfor() || isdec() || isexp())
                                {
                                    counter++;
                                }
                                else
                                {
                                    counter++;
                                    dataGridView1.Rows.Add("sytax", counter + 1);
                                }
                            }
                        }
                        return false;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch { return true; }
        }
    } 
}