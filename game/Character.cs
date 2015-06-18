using System;
using System.Collections.Generic;
using System.Drawing;

namespace game
{
    class Character
    {
        protected Point Position;
        protected Image Image;
        protected Size Size;

        public struct CShoot
        {
            public Image Image;
            public int ReloadTime;
            public int LastTime;
            public int Speed;
            public int Width;
            public Point Offset;

            public void SetOffset(Point offset) {
                this.Offset = offset;
            }

            public Point GetOffset() {
                return this.Offset;
            }

            public void SetReloadTime(int time) {
                this.ReloadTime = time;
            }

            public int GetReloadTime() {
                return this.ReloadTime;
            }

            public void SetSpeed(int speed) {
                this.Speed = speed;
            }

            public int GetSpeed() {
                return this.Speed;
            }

            public void SetWidth(int width) {
                this.Width = width;
            }

            public int GetWidth() {
                return this.Width;
            }

            public void SetImage(Image image) {
                this.Image = image;
            }

            public Image GetImage() {
                return this.Image;
            }
        }
        public CShoot CharacterShoot;

        public Character() {

        }

        public Character(Image image) {
            this.SetImage(image);
            this.SetSize(image.Size);
            this.CharacterShoot = new CShoot();
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

        public Point GetCenterPosition() {
            return new Point(this.GetPosition().X + this.GetSize().Width / 2, this.GetPosition().Y + this.GetSize().Height / 2);
        }

        public int Shoot(ref List<Bullet> bullets, BulletType type) {
            int ticks = Environment.TickCount;
            if (ticks < this.CharacterShoot.LastTime + this.CharacterShoot.GetReloadTime()) {
                return 0;
            }

            this.CharacterShoot.LastTime = ticks;

            Bullet bullet = new Bullet(this.CharacterShoot.GetImage());
            bullet.SetProportionalSize(15);
            bullet.SetSpeed(this.CharacterShoot.GetSpeed());
            bullet.SetType(type);

            Point Position = this.GetPosition();
            Position.X += this.CharacterShoot.GetOffset().X - bullet.GetSize().Width / 2;
            Position.Y += this.CharacterShoot.GetOffset().Y + bullet.GetSize().Height / 4;
            
            bullet.SetPosition(Position);

            bullets.Add(bullet);
            return 1;
        }

        public void Draw(Graphics g) {
            g.DrawImage(this.GetImage(), new Rectangle(this.GetPosition(), this.GetSize()));
        }
    }
}
