using System.Drawing;

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
