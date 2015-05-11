using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.Towers
{
    class SpikeTower : Tower
    {
        private int bulletspeed = 6;
        private float firingspeed = 1.0f;
        private Vector2[] dirs = null;
        private List<Enemy> targets = new List<Enemy>();

        public SpikeTower(Texture2D texture, Texture2D bulletTexture, Vector2 position) : base(texture, bulletTexture, position)
        {
            this.dmg = 50;
            this.cost = 40;
            this.radius = 100;
            dirs = new Vector2[] {           
               new Vector2( 0, -1), // North
               new Vector2( 0,  1), // South
               new Vector2( 1,  0), // East
               new Vector2(-1,  0), // West

               new Vector2(-1, -1), // North West
               new Vector2( 1, -1), // North East
               new Vector2(-1,  1), // South West        
               new Vector2( 1,  1), // South East
            };
        }

        public override bool HasTarget
        {
            get { return false; }
        }

        public override void TargetClosestEnemy(List<Enemy> enemies)
        {
            this.targets.Clear();
            foreach (Enemy enemy in enemies)
            {
                if (this.CanReach(enemy.Center))
                {
                    this.targets.Add(enemy);
                }
            }
        }

        public void RotateBullet(Bullet bullet, Vector2 Direction)
        {
            Vector2 d = Direction;
            d.Normalize();
            bullet.SetRotation((float)Math.Atan2(-d.X, d.Y));
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (this.bulletTime >= this.firingspeed && this.targets.Count != 0)
            {
                for (int i = 0; i < this.dirs.Length; ++i)
                {
                    Bullet bullet = new Bullet(this.bulletTexture, Vector2.Subtract(this.center, new Vector2(this.bulletTexture.Width / 2)), this.dirs[i], this.bulletspeed, this.dmg);
                    RotateBullet(bullet, this.dirs[i]);                    
                    this.bullets.Add(bullet);
                }

                this.bulletTime = 0;
            }

            for (int i = 0; i < this.bullets.Count; ++i)
            {
                Bullet bullet = this.bullets[i];
                bullet.Update(gametime);

                if (!this.CanReach(bullet.Center))
                {
                    bullet.Destroy();
                }

                for (int j = 0; j < this.targets.Count; ++j)
                {
                    //(bullet.width / 2) + (enemy.width / 2)
                    int hitDist = (int)(bullet.Center.X - bullet.Position.X) + (this.targets[j] != null ? (int)(this.targets[j].Center.X - this.targets[j].Position.X) : 0);

                    if (this.targets[j] != null && Vector2.Distance(bullet.Center, this.targets[j].Center) < hitDist)
                    {
                        this.targets[j].Health -= bullet.Damage;
                        bullet.Destroy();
                        break;
                    }
                }

                if (bullet.isDead())
                {
                    this.bullets.Remove(bullet);
                    --i;
                }
            }
        }
    }
}
