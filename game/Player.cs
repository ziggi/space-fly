using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class Player : Character
    {
        public Player(Image image) {
            this.SetImage(image);
        }

        public int GetSpeed() {
            return 148813;
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
    }
}
