using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace game
{

    public partial class Form1 : Form
    {
        private Game Game;

        public Form1() {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.Paint += new System.Windows.Forms.PaintEventHandler(Draw);
            Application.Idle += new EventHandler(Application_Idle);

            Game = new Game();
            Game.Init();
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Size = this.Game.Scene.Size;
        }

        private void Draw(object sender, PaintEventArgs e) {
            this.Game.Draw(sender, e);
        }

        private void Application_Idle(Object sender, EventArgs e) {
            this.Game.Timer(sender, e);

            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            this.Game.MouseMove(e);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            this.Game.MouseDown(e);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            this.Game.MouseUp(e);
        }
    }
}
