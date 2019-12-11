using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace X0
{
    public partial class Form1 : Form
    {
        int CrossCount = 0, ZeroCount = 0;
        public Form1()
        {
            InitializeComponent();
            pole1.OnFinish += new FinishEventHandler(Finish);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Button2_Click(sender, e);

            labelComp.Text = labelMan.Text = 0.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            pole1.Enabled = true;
            pole1.Clear();
        }

        void Finish(char Winner)
        {
            switch (Winner)
            {
                case 'X':
                    CrossCount++;
                    MessageBox.Show("Перемогли хрестики!");
                    break;
                case 'O':
                    ZeroCount++;
                    MessageBox.Show("Перемогли нулики!");
                    break;
                case 'N':
                    MessageBox.Show("Нічия!");
                    break;

            }
            labelMan.Text = Convert.ToString(CrossCount);
            labelComp.Text = Convert.ToString(ZeroCount);
        }

    }
}
