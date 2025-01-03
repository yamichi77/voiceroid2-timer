using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceroidTimer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            voiceroid_load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.iniImport();
        }

        private void Form1_shown(object sender, EventArgs e)
        {
            voiceroid_load();
        }

        private void voiceroid_load()
        {
            if (main.voiceroidLoad())
            {
                label_true.Visible = true;
                label_false.Visible = false;
                string[] charas = main.getCharacter();
                foreach(string chara in charas)
                {
                    comboBox1.Items.Add(chara);
                }
            }
            else
            {
                label_true.Visible = false;
                label_false.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            main.setCharacter(comboBox1.SelectedIndex);
        }
    }
}
