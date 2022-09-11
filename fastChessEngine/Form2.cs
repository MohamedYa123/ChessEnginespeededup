using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fastChessEngine
{
   
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
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
            for (int i = 0; i < 1;i++) {
                // th.Board_init_board(0);
                // th.board_resetallsquares(0);
                // th.board_putpiecesinsquares(0);
                 th.board_getallchecks(0, 1);
                 th.board_getallmoves(0,0);
                // th.move_aplymove(0, 0);
              //  th.board_copyboard(0, 1);
              //  th.Board_init_board(1);
                //  th.board_getallchecks(1, 1);
                //th.board_getallmoves(1, 0);
                th.board_setboardfeature(0, 5, 0);
            }
            sp.Stop();
            th.board_tostring(0, richTextBox1);
            //Text = sp.ElapsedMilliseconds + "";
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
            th.board_setboardfeature(0, 5, 0);
            th.board_resetallsquares(0);
            th.check_resetme(0);
            th.board_putpiecesinsquares(0);
            th.board_getallchecks(0, -1);
            th.board_getallmoves(0, -1);
            richTextBox2.Text = (th.board_listallmoves(0));
            listBox1.Items.Clear();
            listBox1.Items.AddRange(th.board_listallmoves(0).Split('\n'));
            
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
            th.depthM =(int)(numericUpDown2.Value)+1;
            th.board_setboardfeature(0, 5, 0);
            sp.Start();
            pathway pth = new pathway();
            var eval= th.search(0, 0, th.depthM, 0,checkBox1.Checked);
            sp.Stop();
            th.board_setboardfeature(0, 5, 0);
            MessageBox.Show(th.branchesfound + " branches \n time : "+sp.ElapsedMilliseconds+" ms \n average speed : "+ th.branchesfound / sp.ElapsedMilliseconds + " knode/sec\neval : "+eval+" Bestmove : "+(th.bestmove+1));
        }
        int cc = 0;
        List<int> boards = new List<int>();
        Form1 f;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (true)
            {
                //white
                Stopwatch sp = new Stopwatch();
                th.depthM = (int)(numericUpDown2.Value) + 1;
                th.board_setboardfeature(0, 5, 0);
                th.board_resetallsquares(0);
                th.check_resetme(0);
                th.board_putpiecesinsquares(0);
                
                pathway pth = new pathway();
                sp.Start();
                var eval = th.search(cc %2, cc %2, th.depthM, 0, checkBox1.Checked);
                sp.Stop();
                var keyg = th.board_getkey(0, cc % 2, cc % 2);
                th.putrepeatedposition(keyg);
                th.board_setboardfeature(0, 5, 0);
                th.move_aplymove(0, th.bestmove);
                th.board_copyboard(0, boards.Count + 1000);
                boards.Add(boards.Count + 1000);
                index = boards.Count+1000;
                th.board_setboardfeature(0, 5, 0);
                richTextBox1.Text = "";
                th.board_tostring(0, richTextBox1);
                f.button1.PerformClick();
                try
                {
                    textBox1.Text = (th.branchesfound + " branches \r\n time : " + sp.ElapsedMilliseconds + " ms \r\n average speed : " + th.branchesfound / sp.ElapsedMilliseconds + " knode/sec\r\neval : " + eval + "\r\nBestmove : " + th.bestmove);
                }
                catch { }
                cc++;
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(() => aplyside(1));
            //th.Start();
            aplyside(1);
            //Task ts = new Task(() => aplyside(1));
            //ts.Start();
            //aplyside( 1);
        }
        void aplyside(int csc)
        {
            Stopwatch sp = new Stopwatch();
            th.depthM = (int)(numericUpDown2.Value) + 1;
            th.board_setboardfeature(0, 5, 0);
            th.board_resetallsquares(0);
            th.board_putpiecesinsquares(0);
            th.check_resetme(0);
            th.board_setboardfeature(0, 5, 0);
            th.board_resetallsquares(0);
            th.check_resetme(0);
            th.board_putpiecesinsquares(0);
            sp.Start();
            pathway pth = new pathway();
            var eval = th.search(csc % 2, (csc) % 2, th.depthM, 0, checkBox1.Checked);
            sp.Stop();
            th.board_setboardfeature(0, 5, 0);
            th.move_aplymove(0, th.bestmove);
            th.board_copyboard(0, boards.Count + 1000);
            boards.Add(boards.Count + 1000);
            index = boards.Count + 1000;
            th.board_setboardfeature(0, 5, 0);
            richTextBox1.Text = "";
            th.board_tostring(0, richTextBox1);
            try
            {
                f.button1.PerformClick();
            }
            catch { }

            th.board_setboardfeature(0, 5, 0);
            th.board_resetallsquares(0);
            th.check_resetme(0);
            th.board_putpiecesinsquares(0);
            th.board_getallchecks(0, 1-csc);
            th.board_getallmoves(0, 1-csc);
            richTextBox2.Text = (th.board_listallmoves(0));
            listBox1.Items.Clear();
            listBox1.Items.AddRange(th.board_listallmoves(0).Split('\n'));
            try
            {
                textBox1.Text = (th.branchesfound + " branches \r\n time : " + sp.ElapsedMilliseconds + " ms \r\n average speed : " + th.branchesfound / sp.ElapsedMilliseconds + " knode/sec\r\neval : " + eval + "\r\nBestmove : " + th.bestmove);
                var c = pth;
                while (true)
                {
                    textBox1.Text += "\r\n" + c.move;
                    c = c.child;
                }
            }
            catch { }
            cc++;
        }
        int index = 0;
        private void button9_Click(object sender, EventArgs e)
        {
            index--;
            richTextBox1.Text = "";
            th.board_tostring(index, richTextBox1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            index++;
            richTextBox1.Text = "";
            th.board_tostring(index, richTextBox1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            th.board_copyboard(index, 0);
            richTextBox1.Text = "";
            th.board_tostring(index, richTextBox1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(() => aplyside(0));
            //th.Start();
            aplyside(0);
        }

        private void button12_Click(object sender, EventArgs e)
        {
             f = new Form1();
            f.th = th;
            f.f2 = this;
            f.blackside = comboBox2.SelectedIndex;
            f.whiteside = comboBox1.SelectedIndex;
            button3.PerformClick();
            f.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
          //  th.board_getallchecks(0,-1);
            MessageBox.Show(th.board_eval(0, 0, 1, th.board_arethermoves(0,0), 0) + " ");// +th.positionslist[th.board_getkey(0,1,0)][0]);
        }
    }

}
 