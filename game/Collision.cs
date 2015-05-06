using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    enum CollideType
    {
        None,
        HitInPlayer,
        HitInEnemy,
        Collide,
        TakeHeal,
    }

    struct Collide
    {
        public CollideType Type;
        public Enemy Enemy;
        public Bullet Bullet;
        public Heal Heal;
    }

    class Collision
    {
        List<Bullet> Bullets;
        List<Enemy> Enemies;
        List<Heal> Heals;
        Player Player;

        public Collision(ref List<Bullet> Bullets, ref List<Enemy> Enemies, ref Player Player, ref List<Heal> Heals) {
            this.Bullets = Bullets;
            this.Player = Player;
            this.Enemies = Enemies;
            this.Heals = Heals;
        }

        public Collide Check() {
            Collide Result = new Collide();

            foreach (Heal Heal in this.Heals) {
                if (IsCollides(
                        Heal.Position, Heal.Size,
                        Player.GetPosition(), Player.GetSize(),
                        10, 10)
                    ) {
                        Result.Type = CollideType.TakeHeal;
                        Result.Heal = Heal;
                        Result.Bullet = null;
                        Result.Enemy = null;
                        return Result;
                }
            }

            foreach (Enemy Enemy in this.Enemies) {
                // collide
                if (IsCollides(
                        Enemy.GetPosition(), Enemy.GetSize(),
                        Player.GetPosition(), Player.GetSize(),
                        15, 20)
                    ) {
                        Result.Type = CollideType.Collide;
                        Result.Enemy = Enemy;
                        Result.Bullet = null;
                        return Result;
                }

                // hits by player
                foreach (Bullet Bullet in this.Bullets) {
                    if (Bullet.CheckType(BulletType.Player)) {
                        if (IsCollides(Enemy.GetPosition(), Enemy.GetSize(),
                                Bullet.GetPosition(), Bullet.GetSize(), 0, 10)) {
                            Result.Type = CollideType.HitInEnemy;
                            Result.Enemy = Enemy;
                            Result.Bullet = Bullet;
                            return Result;
                        }
                    }
                }
            }

            // hits by enemy
            foreach (Bullet Bullet in this.Bullets) {
                if (Bullet.CheckType(BulletType.Enemy)) {
                    if (IsCollides(Bullet.GetPosition(), Bullet.GetSize(),
                            Player.GetPosition(), Player.GetSize(), 5, 5)) {
                            Result.Type = CollideType.HitInPlayer;
                            Result.Enemy = null;
                            Result.Bullet = Bullet;
                            return Result;
                    }
                }
            }

            return Result;
        }

        
        public bool IsCollides(Point Pos1, Size Size1, Point Pos2, Size Size2, int MarginX = 0, int MarginY = 0) {
            return
                Pos1.X + Size1.Width >= Pos2.X + MarginX &&
                Pos1.X - Size2.Width <= Pos2.X - MarginX &&
                Pos1.Y + Size1.Height >= Pos2.Y + MarginY &&
                Pos1.Y - Size2.Height <= Pos2.Y - MarginY;
        }
    }
}
