using System.Drawing;

namespace game
{
    class Explosion
    {
        private Sprite Sprite;

        public Explosion()
        {

        }

        public Explosion(Sprite Sprite)
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

        public bool IsDone()
        {
            return this.Sprite.IsDone();
        }
    }
}
