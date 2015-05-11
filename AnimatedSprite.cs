using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.Renders
{
    class AnimatedSprite : Sprite
    {
        private int frame = 0;
        private float delay = 0.0f;
        private float elapsed = 0.0f;
        private int totalFrames = 0;
        private Rectangle destArea = Rectangle.Empty;
        private Rectangle sourceArea = Rectangle.Empty;
        private int Width = 0, Height = 0;
        private SpriteEffects effects = SpriteEffects.None;

        public AnimatedSprite(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        private SpriteEffects Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }

        public float Angle
        {
            get { return this.angle; }
            set { this.angle = value; }
        }

        public int GetWidth
        {
            get { return this.Width; }
        }

        public int GetHeight
        {
            get { return this.Height; }
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
        }

        public override void Draw(SpriteBatch batch)
        {
            this.destArea.Location = new Point((int)this.pos.X + Width / 2, (int)this.pos.Y + Height / 2);
            Rectangle area = sourceArea.Width != 0 && sourceArea.Height != 0 ? sourceArea : this.texture.Bounds;
            batch.Draw(this.texture, this.destArea, area, Color.White, this.angle, this.origin, this.effects, 0);
        }
    }
}
