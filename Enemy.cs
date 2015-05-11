using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TowerDefense.GUI;

namespace TowerDefense
{
    class Enemy : Sprite
    {
        protected float health = 0.0f;
        protected float InitialHealth = 0.0f;
        protected bool dead = false;
        protected float speed = 1.0f;
        protected int coins = 0;
        private float speedmod = 0.0f;
        private float speedmodtime = 0.0f;
        private float speedmodduration = 0.0f;
        protected HealthBar hpbar = null;
        private SpriteEffects effects = SpriteEffects.None;
        private Queue<Vector2> Waypoints = new Queue<Vector2>();

        public Enemy(Texture2D texture, Vector2 position, float health, float speed, int coins) : base(texture, position)
        {
            this.health = health;
            this.InitialHealth = health;
            this.speed = speed;
            this.coins = coins;
        }

        public Enemy(Texture2D healthbartexture, Texture2D healthbarbackground, Texture2D texture, Vector2 position, float health, float speed, int coins)
            : this(texture, position, health, speed, coins)
        {
            this.hpbar = new HealthBar(healthbartexture, healthbarbackground, health);
        }

        public SpriteEffects Effects
        {
            get { return this.effects; }
            set { this.effects = value; }
        }

        public float Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public bool isDead
        {
            get { return this.dead; }
        }

        public int Coins
        {
            get { return this.coins; }
        }

        public float SpeedModifier
        {
            get { return this.speedmod; }
            set { this.speedmod = value; }
        }

        public float SpeedModDuration
        {
            get { return this.speedmodduration; }
            set { this.speedmodduration = value; this.speedmodtime = 0; }
        }

        public void SetWayPoints(Queue<Vector2> wps)
        {
            foreach(Vector2 wp in wps)
            {
                this.Waypoints.Enqueue(wp);
            }
            this.pos = this.Waypoints.Dequeue();
        }

        public float Distance
        {
            get { return Vector2.Distance(this.pos, this.Waypoints.Peek()); }
        }

        protected void FaceDirection(Vector2 Direction)
        {
            Vector2 d = new Vector2(Direction.X, Direction.Y);
            d.Normalize();
            base.angle = (float)Math.Atan2(d.X, -d.Y);
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (this.Waypoints.Count > 0)
            {
                if (Distance < this.speed)
                {
                    this.pos = this.Waypoints.Peek();
                    this.Waypoints.Dequeue();
                }
                else
                {
                    Vector2 d = this.Waypoints.Peek() - this.pos;
                    d.Normalize();
                    this.FaceDirection(d);
                    float oldspeed = this.speed;
                    
                    if (this.speedmodtime > this.speedmodduration)
                    {
                        this.speedmod = 0;
                        this.speedmodtime = 0;
                    }

                    if (this.speedmod != 0 && this.speedmodtime <= this.speedmodduration)
                    {
                        oldspeed *= this.speedmod;
                        this.speedmodtime += (float)gametime.ElapsedGameTime.TotalSeconds;
                    }
                    base.velocity = Vector2.Multiply(d, oldspeed);
                    this.pos += base.velocity;
                }
            }

            if (this.Health > 0 && this.hpbar != null)
            {
                this.hpbar.Health = this.health;
                this.hpbar.Bounds = new Rectangle((int)this.pos.X, (int)this.pos.Y - 5, this.texture.Width, 5);
                this.hpbar.Update(gametime);
            }
            this.dead = this.Health <= 0 || this.Waypoints.Count <= 0;
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!this.dead)
            {
                if (this.hpbar == null)
                {
                    float hp = (float)health / (float)InitialHealth;
                    Color color = new Color(new Vector3(1 - hp, hp, 0));
                    base.Draw(batch, color);
                }
                else
                {
                    base.Draw(batch);
                    this.hpbar.Draw(batch);
                }
            }
        }
    }
}
