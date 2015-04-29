using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private bool IsDone;

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
            this.Index += this.Speed * 0.1;
        }

        public void Draw(Graphics g) {
            int frame;

            if (this.Speed > 0) {
                int max = this.FramesCount;
                int idx = Convert.ToInt32(Math.Floor(this.Index));

                frame = idx % max;

                if (this.IsOnce && idx >= max) {
                    this.IsDone = true;
                    return;
                }
            } else {
                frame = 0;
            }

            Point Pos = this.Position;
            Pos.X += frame * this.Size.Width;
            g.DrawImage(this.Image, new Rectangle(DrawPosition, this.Size), new Rectangle(Pos, this.Size), GraphicsUnit.Pixel);
        }
    }
}
