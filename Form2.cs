using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace scanner
{
    public partial class Form2 : Form
    {
        public Form2(string tex)
        {
            InitializeComponent();
            textBox1.Text = tex;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}