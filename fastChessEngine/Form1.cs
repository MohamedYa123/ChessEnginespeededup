using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
        public int numofrowsandcolumns = 8;
        public Color darkcolor = Color.Orange;
        public Color lightcolor=Color.Wheat;
        public Panel[,] allpieces;
        public int[,,] orders;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            setcolors();
            button1.PerformClick();
            doit = false;
            if ((sidetoplay == 1 && blackside == 0) || (sidetoplay == 0 && whiteside == 0))
            {
                

            }
            else
            {

                if (blackside == 1)
                {
                    f2.button6.PerformClick();
                }
                else if (whiteside == 1)
                {
                    f2.button7.PerformClick();
                }
                f2.button3.PerformClick();
            }
            foreach (var d in allpieces)
            {
                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        ll.Hide();
                        ll.Text = "";
                    }
                    catch { }
                }
            }
            button1.PerformClick();
        }
        public void setcolors()
        {
            int cc = 1;
            allpieces = new Panel[numofrowsandcolumns, numofrowsandcolumns];
            for (int i = 0; i < numofrowsandcolumns; i++)
            {
                cc++;
                for (int i2 = 0; i2 < numofrowsandcolumns; i2++)
                {
                    Panel p = new Panel();
                    if (cc % 2 != 0)//(i % 2 != 0 || i2%2!=0)
                    {
                        //color with dark ##

                        p.Location = new Point(iboard.Top + i * iboard.Height / numofrowsandcolumns, iboard.Top + i2 * iboard.Width / numofrowsandcolumns);
                        p.Size = new Size(iboard.Width / numofrowsandcolumns, iboard.Height / numofrowsandcolumns);
                        p.BackColor = darkcolor;
                        p.SendToBack();

                    }
                    else
                    {
                        p.Location = new Point(iboard.Top + i * iboard.Height / numofrowsandcolumns, iboard.Top + i2 * iboard.Width / numofrowsandcolumns);
                        p.Size = new Size(iboard.Width / numofrowsandcolumns, iboard.Height / numofrowsandcolumns);
                        p.BackColor = lightcolor;
                        p.SendToBack();

                    }
                    //     p.Size = new Size(90, 90);
                    Button l = new Button();
                    //l.AutoSize = true;
                    l.Hide();
                    l.Location = new Point(p.Width / 10, p.Width / 10);// p.Location;
                                                                       //  l.Text = "kk";
                    Button b = new Button();
                    b.Location = new Point(p.Top + 10, p.Left + 10);
                    b.Text = "ck";
                    b.Size = new Size(30, 40);
                    //  p.Controls.Add(b);
                    //  this.Text = l.Location.X + " " + l.Location.Y;
                    //  this.Text = p.Location.X + " " + p.Location.Y;
                    p.Controls.Add(l);
                    p.Click += delegate
                    {
                        button1.PerformClick();
                    };
                    allpieces[i, i2] = p;
                    cc++;
                    iboard.Controls.Add(p);
                }
            }
            iboard.Width *= 10 / 4;
            iboard.Height *= 10 / 4;
        }
        bool flip = false;
        public ThinkerPro th;
        public void viewposition(int board)
        {
            
            foreach (var d in allpieces)
            {
                int postx = d.Location.X / d.Width;
                int posty = d.Location.Y / d.Width;
                if (!flip)
                {
                    postx = 8 - postx;
                    posty = 8 - posty;
                }
                else
                {
                    postx += 1;
                    posty += 1;
                }

                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        bool notdf = false;
                        for (int piece = 0; piece < 32; piece++)
                        {
                            var exists = th.piece_getpiece_feature(board, piece, 3);
                            if (exists == -1)
                            {
                                continue;
                            }
                            notdf = true;
                            ll.Text = th.piece_todraw(board, piece);
                            ll.Size = new Size(d.Width * 8 / 9, d.Width * 6 / 9);
                            //this.Text = ll.Location.X + " " + ll.Location.Y;
                            ll.BringToFront();
                            ll.Show();
                            ll.Name = "piece";
                            //  ll.BackColor = Color.Black;
                            ll.ForeColor = Color.Black;
                            Font f = new Font("tahoma", 16, FontStyle.Bold);
                            ll.Font = f;
                            if (th.piece_getpiece_feature(board, piece, 4) == 0)
                            {
                                //    ll.BackColor = Color.Wheat;
                                ll.ForeColor = Color.White;
                            }
                            FieldInfo f1 = typeof(Control).GetField("EventClick",
                                BindingFlags.Static | BindingFlags.NonPublic);

                            /*object obj = f1.GetValue(ll);
                            PropertyInfo pi = ll.GetType().GetProperty("Events",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            EventHandlerList list = (EventHandlerList)pi.GetValue(ll, null);
                            list.RemoveHandler(obj, list[obj]);*/
                            ll.FlatStyle = FlatStyle.Flat;
                            ll.Click += delegate
                            {
                                //  viewposition(b);
                                //  getclicks(p, postx, posty, ll, b);
                            };
                            if (!notdf)
                            {
                                ll.Hide();
                            }
                            /* Label l = new Label();
                             l.Location = d.Location;
                             l.AutoSize = true;
                             l.Text= b.pieces[count].ToString();
                             l.BringToFront();
                             l.Show();
                             l.ForeColor = Color.Yellow;
                             iboard.Controls.Add(l); */
                        }
                    }
                    catch
                    {

                    }
                }

            }
        }
        int b=0;
        int sidetoplay = 0;
        public int whiteside;
        public int blackside;
        public Form2 f2;
        void aplymove(int move)
        {
            th.move_aplymove(b, move);
            th.board_setboardfeature(b, 5, 0);
            if ((sidetoplay == 0 && blackside == 0) || (sidetoplay == 1 && whiteside == 0))
            {
                sidetoplay = 1 - sidetoplay;
                th.board_setboardfeature(0, 5, 0);
                th.board_resetallsquares(0);
                th.check_resetme(0);
                th.board_putpiecesinsquares(0);
                th.board_getallchecks(0, -1);
                th.board_getallmoves(0, -1);
                
            }
            else
            {
               
                if (blackside == 1)
                {
                    f2.button6.PerformClick();
                }
                else if (whiteside == 1)
                {
                    f2.button7.PerformClick();
                }
                f2.button3.PerformClick();
            }
            foreach (var d in allpieces)
            {
                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        ll.Hide();
                        ll.Text = "";
                    }
                    catch { }
                }
            }
            button1.PerformClick();
            
        }
        bool doit = true;
        void aplyorder(int x,int y)
        {
            if (orders[x, y, 0] != 0)
            {
                var piec2 = orders[x, y, 0];
                var xc = th.board_getboardfeature(b, 5);
                for (int i = 0; i <xc; i++)
                {
                    var s = th.move_to_string(b, i);
                    var piece = th.move_getmove_feature(b, i, 8);
                    if (piece== piec2 - 1)
                    {
                        show_button(th.move_getmove_feature(b,i,4),th.move_getmove_feature(b,i,5));
                        var xx =  th.move_getmove_feature(b, i, 4);
                        var yy = th.move_getmove_feature(b, i, 5);
                        orders[xx, yy, 1] = piec2;// orders[x, y, 0];
                        orders[xx, yy, 0] = 0;
                    }
                }
            }
            else
            {
              //  var f = orders[x, y, 1];
                for (int i = 0; i < th.board_getboardfeature(b, 5); i++)
                {
                    if (th.move_getmove_feature(b, i, 4) == x && th.move_getmove_feature(b, i, 5) == y && th.move_getmove_feature(b, i, 8) == orders[x, y, 1] - 1)
                    {
                        orders[x, y, 0] = orders[x, y, 1];
                        for (int ii = 0; ii < 8; ii++)
                        {
                            for (int i2 = 0; i2 < 8; i2++)
                            {
                                // orders[ii, i2, 0] = 0;
                                orders[ii, i2, 1] = 0;
                            }
                        }
                       // var s = th.move_to_string(b, i);
                        aplymove(i);
                       
                    }
                }
            }
        }
        void show_button(int x,int y)
        {
            foreach (var d in allpieces)
            {
                int postx = d.Location.X / d.Width;
                int posty = d.Location.Y / d.Width;
                if (!flip)
                {
                    postx = 8 - postx;
                    posty = 8 - posty;
                }
                else
                {
                    postx += 1;
                    posty += 1;
                }
                if (postx==7-x+1 && posty == y + 1)
                {

                }
                else
                {
                    continue;
                }
                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        ll.Show();
                        ll.BackColor = Color.Green;
                        orders[x, y, 0] = 0;
                        ll.Size = new Size(d.Width * 8 / 9, d.Width * 6 / 9);
                        
                    }
                    catch { }
                }
            }
        }
        void hidegreen()
        {
            foreach (var d in allpieces)
            {

                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        if (ll.BackColor == Color.Green)
                        {
                            ll.Hide();
                        }
                    }
                    catch { }
                }
           }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            int board = b;
            orders = new int[8, 8, 2];
            foreach (var d in allpieces)
            {
                int postx = d.Location.X / d.Width;
                int posty = d.Location.Y / d.Width;
                if (!flip)
                {
                    postx = 8 - postx;
                    posty = 8 - posty;
                }
                else
                {
                    postx += 1;
                    posty += 1;
                }
                
             //   d.Click += delegate { viewposition(b); };
                count++;
                foreach (var c in d.Controls)
                {
                    try
                    {
                        Button ll = (Button)c;
                        ll.Font = new Font("tahoma", 14f, FontStyle.Regular);
                        bool notdf = false;
                        if (doit)
                        {
                            ll.Click += delegate
                            {
                                hidegreen();
                                aplyorder(7 - postx + 1, posty - 1);
                                doit = false;
                            };
                        }
                        Font f = new Font("tahoma", 25, FontStyle.Bold);
                        ll.Font = f;
                        ll.BackColor = Color.Transparent;
                        ll.FlatStyle = FlatStyle.Popup;
                        for (int piece = 0; piece < 32; piece++)
                        {
                            var exists = th.piece_getpiece_feature(board, piece, 3);
                            if (exists == -1)
                            {
                                continue;
                            }
                            var x = th.piece_getpiece_feature(board, piece, 1)+1;
                            var y = th.piece_getpiece_feature(board, piece, 2)+1;
                            if (7-x+2 != postx || y != posty)
                            {
                                continue;
                            }
                            orders[x - 1, y - 1, 0] = piece+1;
                            if (piece == 4)
                            {

                            }
                            notdf = true;
                            ll.Text = th.piece_todraw(board, piece);
                            ll.Size = new Size(d.Width * 8 / 9, d.Width * 6 / 9);
                            //this.Text = ll.Location.X + " " + ll.Location.Y;
                            ll.BringToFront();
                            ll.Show();
                            ll.Name = "piece";
                            ll.BackColor = Color.Transparent;
                          //  ll.BackColor = Color.Black;
                            ll.ForeColor = Color.Black;
                           
                            if (th.piece_getpiece_feature(board, piece, 4) == 0)
                            {
                            //    ll.BackColor = Color.Wheat;
                                ll.ForeColor = Color.White;
                            }
                            FieldInfo f1 = typeof(Control).GetField("EventClick",
                                BindingFlags.Static | BindingFlags.NonPublic);
                            
                            /*object obj = f1.GetValue(ll);
                            PropertyInfo pi = ll.GetType().GetProperty("Events",
                                BindingFlags.NonPublic | BindingFlags.Instance);
                            EventHandlerList list = (EventHandlerList)pi.GetValue(ll, null);
                            list.RemoveHandler(obj, list[obj]);*/
                            
                            //ll.Click += delegate
                            //{
                            //    aplyorder(x - 1, y - 1);
                            //    //  viewposition(b);
                            //  //  getclicks(p, postx, posty, ll, b);
                            //};
                            //  ll.ForeColor = Color.White;
                        }
                        if (!notdf)
                        {
                            ll.Hide();
                        }
                        /* Label l = new Label();
                         l.Location = d.Location;
                         l.AutoSize = true;
                         l.Text= b.pieces[count].ToString();
                         l.BringToFront();
                         l.Show();
                         l.ForeColor = Color.Yellow;
                         iboard.Controls.Add(l); */
                    }
                    catch
                    {

                    }
                }

            }
        }
    }
}
