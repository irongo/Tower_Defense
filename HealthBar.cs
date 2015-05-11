using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense.GUI
{
    class HealthBar
    {
        private float MaxHealth = 0;
        private float CurrentHealth = 0;
        private Texture2D health = null;
        private Texture2D background = null;
        private Rectangle bounds = Rectangle.Empty;
        private Rectangle healthBounds = Rectangle.Empty;

        public HealthBar(Texture2D healthTexture, Texture2D backgroundTexture, float MaxHealth)
        {
            this.health = healthTexture;
            this.background = backgroundTexture;
            this.MaxHealth = MaxHealth;
            this.CurrentHealth = MaxHealth;
            this.bounds = backgroundTexture.Bounds;
        }

        public float Health
        {
            get { return this.CurrentHealth; }
            set { this.CurrentHealth = value;}
        }

        public Rectangle Bounds
        {
            get { return this.bounds; }
            set { this.bounds = value; }
        }

        public void Update(GameTime gameTime)
        {
            this.bounds.X += 5;
            this.bounds.Width -= 10;
            this.bounds.Height = 4;
            
            int Width = (int)((this.CurrentHealth / this.MaxHealth) * this.bounds.Width);
            this.healthBounds = new Rectangle(this.bounds.X, this.bounds.Y, Width, this.bounds.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.background, this.bounds, Color.White);
            batch.Draw(this.health, this.healthBounds, Color.White);
        }
    }
}
