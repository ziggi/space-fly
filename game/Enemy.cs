using System.Collections.Generic;
using System.Drawing;

namespace game
{
    class Enemy : Character
    {
        private int Speed;

        public Enemy(Image image)
        {
            this.SetImage(image);
            this.SetSize(image.Size);
        }

        public void SetSpeed(int speed)
        {
            this.Speed = speed;
        }

        public int GetSpeed()
        {
            return this.Speed;
        }

        public int Move(Scene GameScene)
        {
            Point NewPosition = new Point(this.Position.X, this.Position.Y + this.GetSpeed());

            if (NewPosition.Y <= -this.Size.Height)
            {
                return 0;
            }
            else if (NewPosition.Y >= GameScene.Size.Height)
            {
                return 0;
            }

            this.SetPosition(NewPosition);
            return 1;
        }

        public int Shoot(ref List<Bullet> bullets)
        {
            this.Shoot(ref bullets, BulletType.Enemy);
            return 1;
        }
    }
}
