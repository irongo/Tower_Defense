using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    class Tower : Sprite
    {
        protected int cost = 0;
        protected int dmg = 0;
        protected float radius = 0.0f;
        protected Enemy target = null;
        protected Texture2D bulletTexture;
        protected float bulletTime = 0.0f;
        protected List<Bullet> bullets = new List<Bullet>();

        public Tower(Texture2D texture, Texture2D bulletTexture, Vector2 position) : base(texture, position)
        {
            this.bulletTexture = bulletTexture;
        }

        public virtual bool HasTarget
        {
            get { return this.target != null; }
        }

        public Enemy Target
        {
            get { return this.target; }
        }

        public int Cost
        {
            get { return this.cost; }
        }

        public int Damage
        {
            get { return this.dmg; }
        }

        public float Radius
        {
            get { return this.radius; }
        }

        public bool CanReach(Vector2 position)
        {
            return Vector2.Distance(base.center, position) <= this.radius;
        }

        protected void FaceTarget()
        {
            Vector2 d = base.center - this.target.Center;
            d.Normalize();
            base.angle = (float)Math.Atan2(-d.X, d.Y);
        }

        public virtual void TargetClosestEnemy(List<Enemy> enemies)
        {
            this.target = null;
            float smallest = radius;

            foreach (Enemy enemy in enemies)
            {
                if (Vector2.Distance(base.center, enemy.Center) < smallest)
                {
                    smallest = Vector2.Distance(base.center, enemy.Center);
                    this.target = enemy;
                }
            }
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            this.bulletTime += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (this.target != null)
            {
                this.FaceTarget();
                if (!this.CanReach(this.target.Center) || target.isDead)
                {
                    this.target = null;
                    this.bulletTime = 0;
                }
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(batch);
            }

            base.Draw(batch);
        }
    }
}
