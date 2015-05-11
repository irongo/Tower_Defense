using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TowerDefense.GUI;

namespace TowerDefense
{
    class AnimatedEnemy : Enemy
    {
        private int frame = 0;
        private float delay = 0.0f;
        private float elapsed = 0.0f;
        private int totalFrames = 0;
        private Rectangle destArea = Rectangle.Empty;
        private Rectangle sourceArea = Rectangle.Empty;
        private int Width = 0, Height = 0;

        public AnimatedEnemy(Texture2D texture, Vector2 position, float health, float speed, int coins)
            : base(texture, position, health, speed, coins)
        {
        }

        public AnimatedEnemy(Texture2D healthbartexture, Texture2D healthbarbackground, Texture2D texture, Vector2 position, float health, float speed, int coins)
            : base(healthbartexture, healthbarbackground, texture, position, health, speed, coins)
        {
        }

        public float Angle
        {
            get { return this.angle; }
            set { this.angle = value; }
        }

        public float Delay
        {
            get { return this.delay; }
            set { this.delay = value; }
        }

        public int FrameCount
        {
            get { return this.totalFrames; }
            set { this.totalFrames = value; }
        }

        public void SetFrameInfo(int TotalFrames, int Width, int Height, float Delay)
        {
            this.delay = Delay;
            this.totalFrames = TotalFrames;
            this.Width = Width;
            this.Height = Height;
            this.sourceArea = new Rectangle(0, 0, Width, Height);
            this.destArea = new Rectangle((int)this.pos.X, (int)this.pos.Y, Width, Height);
            this.origin = new Vector2(Width / 2, Height / 2);
            this.center = new Vector2(this.pos.X + Width / 2, this.pos.Y + Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (this.frame >= this.totalFrames - 1)
                {
                    this.frame = 0;
                }
                else
                {
                    ++this.frame;
                }
                elapsed = 0;
            }

            this.sourceArea = (new Rectangle(Width * frame, 0, Width, Height));
            base.Update(gameTime);
            this.center = new Vector2(this.pos.X + Width / 2, this.pos.Y + Height / 2);
            if (this.hpbar != null)
            {
                this.hpbar.Health = this.health;
                this.hpbar.Bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y, Width, 5);
                this.hpbar.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            this.destArea.Location = new Point((int)this.center.X, (int)this.center.Y);
            Rectangle area = sourceArea.Width != 0 && sourceArea.Height != 0 ? sourceArea : this.texture.Bounds;
            batch.Draw(this.texture, this.destArea, area, Color.White, this.angle, this.origin, base.Effects, 0);
            if (this.hpbar != null)
            {
                this.hpbar.Draw(batch);
            }
        }
    }
}
