using System;
using System.Drawing;

namespace game
{
    class Sprite
    {
        private Image Image;
        private Point Position;
        private Point DrawPosition;
        private Size Size;
        private int Speed;
        private int FramesCount;
        private bool IsOnce;
        private double Index;

        public Sprite(Point DrawPosition, Image Image, Point Position, Size Size, int Speed, int FramesCount, bool IsOnce) {
            this.DrawPosition = DrawPosition;
            this.Image = Image;
            this.Position = Position;
            this.Size = Size;
            this.Speed = Speed;
            this.FramesCount = FramesCount;
            this.IsOnce = IsOnce;
        }
        
        public void Update() {
            this.Index += this.Speed;
        }

        public void Draw(Graphics g) {
            int frame;

            if (this.Speed > 0) {
                frame = Convert.ToInt32(Math.Floor(this.Index)) % this.FramesCount;

                if (this.IsDone()) {
                    return;
                }
            } else {
                frame = 0;
            }

            Point Pos = this.Position;
            Pos.X += frame * this.Size.Width;
            g.DrawImage(this.Image, new Rectangle(DrawPosition, this.Size), new Rectangle(Pos, this.Size), GraphicsUnit.Pixel);
        }

        public bool IsDone() {
            return (this.IsOnce && Convert.ToInt32(Math.Floor(this.Index)) >= this.FramesCount);
        }
    }
}
