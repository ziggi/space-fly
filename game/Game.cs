using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace game
{
    enum GameStatus
    {
        Menu,
        Game,
        End
    }

    struct PlayerStatus
    {
        public bool MouseHold;
    }

    struct Scene
    {
        public Bitmap BackgroundImg;
        public Bitmap OverlayImg;
        public Size Size;
    }

    class Game
    {
        private GameStatus GameStatus;
        private PlayerStatus PlayerStatus;
        private Player Player;
        public Scene Scene;
        private List<Bullet> Bullets;
        private Collision Collision;
        private List<Enemy> Enemies;
        private List<Explosion> Explosions;
        private List<Heal> Heals; 

        private int PlayerLevel;
        private int PlayerHealth;
        private int GameLevel;
        private int Score;
        private int MaxEnemyImages;
        private int LastTime;
        private int EnemyCreateTime;
        private int LastEnemyShootTime;
        private int LastIncreaseLevelTime;
        private int EnemyCreateDelay = Game.BaseEnemyCreateDelay;
        private int EnemyShootDelay = Game.BaseEnemyShootDelay;
        private int EnemySpeed = Game.BaseEnemySpeed;
        private int EnemyShootSpeed = Game.BaseEnemyShootSpeed;
        private bool LevelUpdated;

        private const int MaxPlayerLevel = 4;

        private const int BasePlayerShootSpeed = -50;
        private const int StepPlayerShootSpeed = -0;

        private const int BasePlayerReloadTime = 300;
        private const int StepPlayerReloadTime = -25;

        private const int MaxLevel = 10;
        private const int IncreaseLevelDelay = 3000;

        private const int BaseEnemyCreateDelay = 600;
        private const int StepEnemyCreateDelay = 40;

        private const int BaseEnemyShootDelay = 1000;
        private const int StepEnemyShootDelay = 70;

        private const int BaseEnemySpeed = 5;
        private const int StepEnemySpeed = 1;

        private const int BaseEnemyShootSpeed = 10;
        private const int StepEnemyShootSpeed = 1;

        private const int FramesPerSecond = 60;

        public void Init() {
            this.GameStatus = GameStatus.Menu;

            this.MaxEnemyImages = 0;
            ResourceManager resman = game.Properties.Resources.ResourceManager;
            while (resman.GetObject("enemy" + this.MaxEnemyImages) != null) {
                this.MaxEnemyImages++;
            }

            this.Scene.Size = new Size(400, 700);
            this.Scene.BackgroundImg = game.Properties.Resources.background;
            this.Scene.OverlayImg = game.Properties.Resources.gradient;

            this.Player = new Player(game.Properties.Resources.alien1);
            this.Player.SetProportionalSize(60);
            this.Player.CharacterShoot.SetReloadTime(Game.BasePlayerReloadTime);
            this.Player.CharacterShoot.SetSpeed(Game.BasePlayerShootSpeed);
            this.Player.CharacterShoot.SetWidth(15);
            this.Player.CharacterShoot.SetImage(game.Properties.Resources.laser);
            this.Player.CharacterShoot.SetOffset(new Point(this.Player.GetSize().Width / 2,
                -this.Player.GetSize().Height / 2));
            PlayerHealth = 3;

            this.Enemies = new List<Enemy>();
            this.Bullets = new List<Bullet>();
            this.Explosions = new List<Explosion>();
            this.Heals = new List<Heal>();

            this.Collision = new Collision(ref this.Bullets, ref this.Enemies, ref this.Player, ref this.Heals);
        }

        public void Draw(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            Point position = new Point(0, 0);
            Size size = this.Scene.BackgroundImg.Size;
            g.DrawImage(this.Scene.BackgroundImg, new Rectangle(position, size));

            if (this.GameStatus == GameStatus.Menu) {
                this.DrawMessage(g, "Click for start", 24);
            }

            if (this.GameStatus == GameStatus.Game) {
                this.Player.Draw(g);
            }

            if (this.GameStatus == GameStatus.Game || this.GameStatus == GameStatus.End) {
                foreach (Explosion explosion in this.Explosions) {
                    explosion.Draw(g);
                }

                foreach (Heal heal in this.Heals)
                {
                    heal.Draw(g);
                }

                foreach (Character enemy in this.Enemies) {
                    enemy.Draw(g);
                }

                foreach (Bullet bullet in this.Bullets) {
                    bullet.Draw(g);
                }


                if (this.GameStatus == GameStatus.End)
                {
                    this.PlayerHealth = 3;
                    this.DrawMessage(g, "Game Over\n\nClick, click, click", 24);
                }
                
                this.DrawHealth(g);
                this.DrawScore(g);
                this.DrawGameLevel(g);
                this.DrawPlayerLevel(g);
            }
        }

        public void Timer(Object sender, EventArgs e) {
            int ticks = Environment.TickCount;
            if (ticks > this.LastTime + 1000 / Game.FramesPerSecond) {
                Collide collide = this.Collision.Check();
                
                if (this.GameStatus == GameStatus.Game || this.GameStatus == GameStatus.End) {
                    for (int i = this.Bullets.Count - 1; i != -1; i--) {
                        int result = this.Bullets[i].Move(this.Scene);

                        if (result == 0) {
                            this.Bullets.RemoveAt(i);
                        }
                    }

                    foreach (Explosion explosion in this.Explosions) {
                        explosion.Update();
                    }

                    foreach (Heal heal in this.Heals)
                    {
                        heal.Update();
                    }

                    if (collide.Type == CollideType.TakeHeal)
                    {
                        this.PlayerHealth++;
                        this.Heals.Remove(collide.Heal);
                    }

                    if (collide.Type == CollideType.HitInEnemy) {
                        this.IncreaseScore();
                        this.AddExplosion(collide.Enemy);
                        this.AddHeal(collide.Enemy);

                        this.Enemies.Remove(collide.Enemy);
                        this.Bullets.Remove(collide.Bullet);
                    }

                    this.LastTime = ticks;
                }
                
                if (this.GameStatus == GameStatus.Game) {
                    foreach (Enemy enemy in this.Enemies.Reverse<Enemy>()) {
                        int result = enemy.Move(this.Scene);

                        if (ticks > this.LastEnemyShootTime + this.EnemyShootDelay) {
                            enemy.Shoot(ref this.Bullets);

                            this.LastEnemyShootTime = ticks;
                        }

                        if (result == 0) {
                            this.Enemies.Remove(enemy);
                        }
                    }
                    
                    if (ticks > this.LastIncreaseLevelTime + Game.IncreaseLevelDelay) {
                        this.EnemyCreateDelay = Game.BaseEnemyCreateDelay - Game.StepEnemyCreateDelay * this.GameLevel;
                        this.EnemyShootDelay = Game.BaseEnemyShootDelay - Game.StepEnemyShootDelay * this.GameLevel;
                        this.EnemySpeed = Game.BaseEnemySpeed + Game.StepEnemySpeed * this.GameLevel / 2;
                        this.EnemyShootSpeed = Game.BaseEnemyShootSpeed + Game.StepEnemyShootSpeed * this.GameLevel;

                        if (this.GameLevel < Game.MaxLevel) {
                            this.GameLevel++;
                        }

                        this.LastIncreaseLevelTime = ticks;
                    }

                    if (ticks > this.EnemyCreateTime + this.EnemyCreateDelay) {
                        Random rand = new Random();

                        Enemy enemy = new Enemy((Bitmap)game.Properties.Resources.ResourceManager.GetObject("enemy" + rand.Next(this.MaxEnemyImages)));
                        enemy.SetProportionalSize(60);
                        enemy.SetSpeed(this.EnemySpeed);
                        enemy.CharacterShoot.SetReloadTime(500);
                        enemy.CharacterShoot.SetSpeed(this.EnemyShootSpeed);
                        enemy.CharacterShoot.SetWidth(15);
                        enemy.CharacterShoot.SetImage(game.Properties.Resources.laser_red);
                        enemy.CharacterShoot.SetOffset(new Point(
                                enemy.GetSize().Width / 2,
                                enemy.GetSize().Height / 2
                            ));
                        enemy.SetPosition(new Point(
                                rand.Next(this.Scene.Size.Width - enemy.GetSize().Width),
                                -enemy.GetSize().Height
                            ));

                        this.Enemies.Add(enemy);

                        this.EnemyCreateTime = ticks;
                    }

                    if (this.PlayerStatus.MouseHold) {
                        this.Player.Shoot(ref this.Bullets);
                    }


                    if (this.Score > 0 && this.Score % 10 == 0 && !this.LevelUpdated) {
                        if (this.PlayerLevel < Game.MaxPlayerLevel) {
                            this.PlayerLevel++;
                            this.LevelUpdated = true;

                            this.Player.CharacterShoot.SetSpeed(Game.BasePlayerShootSpeed + Game.StepPlayerShootSpeed * this.PlayerLevel);
                            this.Player.CharacterShoot.SetReloadTime(Game.BasePlayerReloadTime + Game.StepPlayerReloadTime * this.PlayerLevel);
                        }
                    } else if (this.Score % 10 == 1)  {
                        this.LevelUpdated = false;
                    }

                    if (collide.Type == CollideType.HitInPlayer)
                    {
                        this.Bullets.Remove(collide.Bullet);
                        this.AddExplosion(this.Player);

                        this.PlayerHealth--;
                        this.Player.SetPosition(new Point(this.Scene.Size.Width / 2, this.Scene.Size.Height / 2));
                        
                        if (PlayerHealth < 1)
                        {
                            this.GameStatus = GameStatus.End;        
                        }
                    } else if (collide.Type == CollideType.Collide) {

                        this.AddExplosion(collide.Enemy);
                        this.AddExplosion(this.Player);

                        this.Enemies.Remove(collide.Enemy);

                        PlayerHealth--;
                        this.Player.SetPosition(new Point(this.Scene.Size.Width / 2, this.Scene.Size.Height / 2));

                        if (PlayerHealth < 1)
                        {
                            this.GameStatus = GameStatus.End;  
                        }
                    }

                    this.LastTime = ticks;
                }
            }
        }

        public void MouseMove(MouseEventArgs e) {
            if (this.GameStatus == GameStatus.End) {
                return;
            }
            this.Player.Move(this.Scene, e.X, e.Y);
        }

        public void MouseDown(MouseEventArgs e) {
            if (this.GameStatus == GameStatus.Game) {
                this.PlayerStatus.MouseHold = true;

                this.Player.Shoot(ref this.Bullets);
            }
        }

        public void MouseUp(MouseEventArgs e) {
            if (this.GameStatus == GameStatus.Menu || this.GameStatus == GameStatus.End) {
                if (!this.PlayerStatus.MouseHold) {
                    this.GameStart();
                }
            }

            this.PlayerStatus.MouseHold = false;
        }

        public void GameStart() {
            this.GameStatus = GameStatus.Game;
            this.Score = 0;
            this.GameLevel = 1;
            this.PlayerLevel = 1;
            this.EnemyCreateDelay = Game.BaseEnemyCreateDelay;
            this.EnemyShootDelay = Game.BaseEnemyShootDelay;
            this.EnemySpeed = Game.BaseEnemySpeed;
            this.EnemyShootSpeed = Game.BaseEnemyShootSpeed;

            this.Player.CharacterShoot.SetReloadTime(Game.BasePlayerReloadTime);
            this.Player.CharacterShoot.SetSpeed(Game.BasePlayerShootSpeed);

            this.Bullets.Clear();
            this.Enemies.Clear();
            this.Explosions.Clear();
            this.Heals.Clear();

            int ticks = Environment.TickCount;
            this.LastTime = ticks;
            this.LastIncreaseLevelTime = ticks;
            this.LastEnemyShootTime = ticks;
        }

        public void DrawMessage(Graphics g, String text, int fontSize = 16) {
            Point position = new Point(0, 0);
            Size size = this.Scene.BackgroundImg.Size;
            g.DrawImage(this.Scene.OverlayImg, new Rectangle(position, size));

            String drawString = text;
            Font drawFont = new Font(FontFamily.GenericSansSerif, fontSize);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            PointF drawPoint = new PointF(this.Scene.Size.Width / 2, this.Scene.Size.Height / 4);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;

            g.DrawString(drawString, drawFont, drawBrush, drawPoint, drawFormat);
        }

        private void AddHeal(Character character)
        {
            Point HealPos = character.GetCenterPosition();
            Size HealSize = new Size(32, 32);

            Heal heal = new Heal(new Sprite(HealPos,
                game.Properties.Resources.heal, new Point(0, 0), HealSize,
                0, 40, true));
            heal.Position = HealPos;
            heal.Size = HealSize;
            this.Heals.Add(heal);
        }

        private void AddExplosion(Character Character) {
            Size ExploseSize = new Size(128, 128);
            Point ExplosePos = Character.GetCenterPosition();
            ExplosePos.X -= ExploseSize.Width / 2;
            ExplosePos.Y -= ExploseSize.Height / 2;

            Explosion explosion = new Explosion(new Sprite(ExplosePos,
                game.Properties.Resources.Exp_type_A, new Point(0, 0), ExploseSize,
                16, 40, true));
            this.Explosions.Add(explosion);
        }

        public int GetScore() {
            return this.Score;
        }

        public int GetHealth()
        {
            return this.PlayerHealth;
        }

        public void SetScore(int score) {
            this.Score = score;
        }

        public void DrawHealth(Graphics g)
        {
            String drawString = "Health: " + this.GetHealth();
            Font drawFont = new Font(FontFamily.GenericSansSerif, 16);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            PointF drawPoint = new PointF(this.Scene.Size.Width - 130, 0);

            g.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public void DrawScore(Graphics g) {
            String drawString = "Score: " + this.GetScore();
            Font drawFont = new Font(FontFamily.GenericSansSerif, 16);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            PointF drawPoint = new PointF(0, 0);

            g.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public void DrawGameLevel(Graphics g) {
            String drawString = "Difficulty: " + this.GameLevel;
            Font drawFont = new Font(FontFamily.GenericSansSerif, 10);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            PointF drawPoint = new PointF(this.Scene.Size.Width / 4 + 20, 0);

            g.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public void DrawPlayerLevel(Graphics g) {
            String drawString = "Level: " + this.PlayerLevel;
            Font drawFont = new Font(FontFamily.GenericSansSerif, 10);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            PointF drawPoint = new PointF(this.Scene.Size.Width / 4 + 100, 0);

            g.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public void IncreaseScore(int value = 1) {
            this.Score += value;
        }

        public void SetStatus(GameStatus Status) {
            this.GameStatus = Status;
        }

        public GameStatus GetStatus() {
            return this.GameStatus;
        }
    }
}
