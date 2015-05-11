using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    class Bullet : Sprite
    {
        private int dmg = 0;
        private int life = 0;
        private int speed = 0;

        public Bullet(Texture2D texture, Vector2 position, float rotation, int speed, int damage) : base(texture, position)
        {
            this.speed = speed;
            this.dmg = damage;
            this.angle = rotation;
        }

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, int speed, int damage) : base(texture, position)
        {
            this.dmg = damage;
            this.speed = speed;
            this.velocity = velocity * speed;
        }


        public int Damage
        {
            get { return this.dmg; }
        }

        public bool isDead()
        {
            return this.life > 100;
        }

        public void Destroy()
        {
            this.life = 101;
        }

        public void SetRotation(float angle)
        {
            this.angle = angle;
            this.velocity = Vector2.Transform(new Vector2(0, -this.speed), Matrix.CreateRotationZ(this.angle));
        }

        public override void Update(GameTime gametime)
        {
            ++this.life;
            this.pos += this.velocity;
            base.Update(gametime);
        }
    }
}
