using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fastChessEngine
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.PerformClick();
        }
        ThinkerPro th;
        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sp = new Stopwatch();
           // ThinkerPro th = new ThinkerPro();
            sp.Start();
            //th.assign();
             th = new ThinkerPro();
            sp.Stop();
           // MessageBox.Show(sp.ElapsedMilliseconds + "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            th = new ThinkerPro();
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            th.Board_init_board(0);
         //   th.board_tostring(0,richTextBox1);
            Stopwatch sp = new Stopwatch();
            sp.Start();
            for (int i = 0; i < 100000;i++) {
                // th.Board_init_board(0);
                // th.board_resetallsquares(0);
                // th.board_putpiecesinsquares(0);
                 th.board_getallchecks(0, 1);
                 th.board_getallmoves(0,0);
                // th.move_aplymove(0, 0);
                th.board_copyboard(0, 1);
              //  th.Board_init_board(1);
                //  th.board_getallchecks(1, 1);
                //th.board_getallmoves(1, 0);
                th.board_setboardfeature(0, 5, 0);
            }
            sp.Stop();
            th.board_tostring(0, richTextBox1);
            Text = sp.ElapsedMilliseconds + "";
            richTextBox2.Text=(th.board_listallmoves(0));
            //richTextBox2.Text = "";
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int i2 = 0; i2 < 8; i2++)
            //    {
            //        richTextBox2.Text += "square_reset_square(board," + i + ", " + i2 + ");";
            //        if ((i + i2) % 5 == 0)
            //        {
            //            richTextBox2.Text += "\r\n";
            //        }
            //    }
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            int p = (int)numericUpDown1.Value-1;
            th.move_aplymove(0, p);
            th.board_tostring(0, richTextBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            th.board_resetallsquares(0);
            th.check_resetme(0);
            th.board_putpiecesinsquares(0);
            th.board_getallchecks(0, -1);
            th.board_getallmoves(0, -1);
            richTextBox2.Text = (th.board_listallmoves(0));
            listBox1.Items.Clear();
            listBox1.Items.AddRange(th.board_listallmoves(0).Split('\n'));
            th.board_setboardfeature(0, 5, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(th.square_getsquare_feature(0, (int)(x.Value), (int)y.Value, (int)fet.Value)+"");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = listBox1.SelectedIndex + 1;
            button1.PerformClick();
            button3.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Stopwatch sp = new Stopwatch();
            th.depthM = 6;
            sp.Start();
            th.search(0, 0, th.depthM, 0);
            sp.Stop();
            MessageBox.Show(th.branchesfound + " time : "+sp.ElapsedMilliseconds+" ms");
        }
    }

}
