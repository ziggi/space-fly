using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    class Heal
    {
        public Point Position { get; set; }
        public Size Size { get; set; }
        private Sprite Sprite;

        public Heal()
        {
            
        }

        public Heal(Sprite Sprite)
        {
            this.Sprite = Sprite;
        }

        public void Update()
        {
            this.Sprite.Update();
        }

        public void Draw(Graphics g)
        {
            this.Sprite.Draw(g);
        }
    }
}
