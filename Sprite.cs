using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense
{
    abstract class Sprite
    {
        protected Vector2 pos = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;
        protected Vector2 center = Vector2.Zero;
        protected Vector2 origin = Vector2.Zero;
        protected float angle = 0.0f;
        protected Texture2D texture = null;

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.pos = position;
            this.texture = texture;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }

        public Vector2 Position
        {
            get { return this.pos; }
        }

        public Vector2 Center
        {
            get { return center; }
        }

        public virtual void Update(GameTime gametime)
        {
            this.center = new Vector2(pos.X + texture.Width / 2, pos.Y + texture.Height / 2);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            this.Draw(batch, Color.White);
        }

        public virtual void Draw(SpriteBatch batch, Color tint)
        {
            batch.Draw(texture, center, null, tint, angle, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
