using System.Collections.Generic;
using System.Drawing;

namespace game
{
    class Player : Character
    {
        private int Health;

        public Player(Image image) {
            this.SetImage(image);
        }

        public int Shoot(ref List<Bullet> bullets) {
            this.Shoot(ref bullets, BulletType.Player);
            return 1;
        }

        public void Move(Scene GameScene, int NewX, int NewY) {
            Size size = this.GetSize();
            Point position = new Point(NewX - size.Width / 2, NewY - size.Height / 2);

            if (NewX + size.Width / 2 > GameScene.Size.Width) {
                position.X = GameScene.Size.Width - size.Width;
            }

            if (NewX - size.Width / 2 < 0) {
                position.X = 0;
            }

            if (NewY + size.Height / 2 > GameScene.Size.Height) {
                position.Y = GameScene.Size.Height - size.Height;
            }

            if (NewY - size.Height / 2 < 0) {
                position.Y = 0;
            }

            this.SetPosition(position);
        }

        public int GetHealth() {
            return this.Health;
        }

        public void SetHealth(int value) {
            this.Health = value;
        }

        public void AddHealth(int value) {
            this.Health += value;
        }
    }
}
