using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    class SpeedTower : Tower
    {
        private int bulletSpeed = 6;
        private float firingspeed = 1.0f;
        private float speedmodifier = 0.6f;
        private float speedmodduration = 2.0f;

        public SpeedTower(Texture2D texture, Texture2D bulletTexture, Vector2 position) : base(texture, bulletTexture, position)
        {
            this.dmg = 5;
            this.cost = 6;
            this.radius = 100;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            if (this.bulletTime >= this.firingspeed && target != null)
            {
                Bullet bullet = new Bullet(this.bulletTexture, Vector2.Subtract(this.center, new Vector2(this.bulletTexture.Width / 2)), this.angle, this.bulletSpeed, this.dmg);
                this.bullets.Add(bullet);
                this.bulletTime = 0;
            }

            for (int i = 0; i < this.bullets.Count; ++i)
            {
                Bullet bullet = this.bullets[i];

                bullet.SetRotation(this.angle);
                bullet.Update(gametime);

                if (!this.CanReach(bullet.Center))
                {
                    bullet.Destroy();
                }

                //(bullet.width / 2) + (enemy.width / 2)
                int hitDist = (int)(bullet.Center.X - bullet.Position.X) + (this.target != null ? (int)(this.target.Center.X - this.target.Position.X) : 0);

                if (this.target != null && Vector2.Distance(bullet.Center, this.target.Center) < hitDist)
                {
                    if (this.target.SpeedModifier <= this.speedmodifier)
                    {
                        this.target.SpeedModifier = this.speedmodifier;
                        this.target.SpeedModDuration = this.speedmodduration;
                    }

                    this.target.Health -= bullet.Damage;
                    bullet.Destroy();
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
