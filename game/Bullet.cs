using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    enum BulletType
    {
        Player,
        Enemy,
    }

    class Bullet
    {
        private Point Position;
        private Image Image;
        private Size Size;
        private int Speed = -10;
        private BulletType Type;
        
        public Bullet() {

        }

        public Bullet(Image image) {
            this.SetImage(image);
            this.SetSize(image.Size);
        }

        public void SetSpeed(int speed) {
            this.Speed = speed;
        }

        public int GetSpeed() {
            return this.Speed;
        }

        public void SetImage(Image image) {
            this.Image = image;
        }

        public Image GetImage() {
            return this.Image;
        }

        public void SetProportionalSize(int width) {
            Double proportion = Convert.ToDouble(this.Image.Width) / Convert.ToDouble(this.Image.Height);
            int height = Convert.ToInt32(Convert.ToDouble(width) / proportion);
            this.SetSize(new Size(width, height));
        }

        public void SetSize(Size size) {
            this.Size = size;
        }

        public Size GetSize() {
            return this.Size;
        }

        public void SetPosition(Point point) {
            this.Position = point;
        }

        public Point GetPosition() {
            return this.Position;
        }

        public void SetType(BulletType type) {
            this.Type = type;
        }

        public bool CheckType(BulletType type) {
            if (this.Type == type) {
                return true;
            }
            return false;
        }

        public int Move(Scene GameScene) {
            Point NewPosition = new Point(this.Position.X, this.Position.Y + this.GetSpeed());

            if (NewPosition.Y <= -this.Size.Height) {
                return 0;
            } else if (NewPosition.Y >= GameScene.Size.Height) {
                return 0;
            }

            this.Position = NewPosition;
            return 1;
        }

        public void Draw(Graphics g) {
            g.DrawImage(this.GetImage(), new Rectangle(this.GetPosition(), this.GetSize()));
        }
    }
}
