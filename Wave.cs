using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TowerDefense.GUI;

namespace TowerDefense
{
    class Wave
    {
        private int enemyCount = 0;
        private int origEnemyCount = 0;
        private int waveLevel = 0;
        private float spawnTimer = 0;
        private float spawnDelay = 0.7f;
        private bool enemiesSpawning = false;
        private int enemiesSpawned = 0;
        private bool enemyEnd = false;
        private Map map = null;
        private Texture2D enemy = null;
        private float speed = 0;
        private int coins = 0;
        private int hp = 0;
        private int AnimationFrameCount = 0;
        private float AnimationDelay = 0.0f;
        private int AnimationWidth = 0;
        private int AnimationHeight = 0;
        private string description = string.Empty;
        private Controller player = null;
        private Texture2D HealthBarTexture = null;
        private Texture2D HealthBarBackground = null;
        private List<Enemy> enemies = new List<Enemy>();

        public Wave(Texture2D EnemyTexture, Controller player, int WaveLevel, int EnemyCount, float Speed, int Coins, int HP, Map map)
        {
            this.enemy = EnemyTexture;
            this.player = player;
            this.waveLevel = WaveLevel;
            this.enemyCount = EnemyCount;
            this.origEnemyCount = EnemyCount;
            this.map = map;
            this.speed = Speed;
            this.coins = Coins;
            this.hp = HP;
        }

        public Wave(Texture2D EnemyTexture, Controller player, int WaveLevel, int EnemyCount, float Speed, int Coins, int HP, int AnimationFrameCount, float AnimationDelay, int AnimationWidth, int AnimationHeight, Map map)
            : this(EnemyTexture, player, WaveLevel, EnemyCount, Speed, Coins, HP, map)
        {
            this.AnimationHeight = AnimationHeight;
            this.AnimationWidth = AnimationWidth;
            this.AnimationDelay = AnimationDelay;
            this.AnimationFrameCount = AnimationFrameCount;
        }

        public Wave(string Name, Texture2D EnemyTexture, Controller player, int WaveLevel, int EnemyCount, float Speed, int Coins, int HP, int AnimationFrameCount, float AnimationDelay, int AnimationWidth, int AnimationHeight, Map map)
            : this(EnemyTexture, player, WaveLevel, EnemyCount, Speed, Coins, HP, AnimationFrameCount, AnimationDelay, AnimationWidth, AnimationHeight, map)
        {
            this.description = Name + " " + HP + "hp, worth " + Coins + "g each.";
        }

        public Wave(string Name, Texture2D healthbartexture, Texture2D healthbarbackground, Texture2D EnemyTexture, Controller player, int WaveLevel, int EnemyCount, float Speed, int Coins, int HP, int AnimationFrameCount, float AnimationDelay, int AnimationWidth, int AnimationHeight, Map map)
            : this(Name, EnemyTexture, player, WaveLevel, EnemyCount, Speed, Coins, HP, AnimationFrameCount, AnimationDelay, AnimationWidth, AnimationHeight, map)
        {
            this.HealthBarTexture = healthbartexture;
            this.HealthBarBackground = healthbarbackground;
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public bool WaveOver
        {
            get { return (this.enemiesSpawned == this.enemyCount || this.enemyCount > this.origEnemyCount) && this.enemies.Count == 0; }
        }

        public int WaveLevel
        {
            get { return this.waveLevel; }
        }

        public bool EnemySuccess
        {
            get { return this.enemyEnd; }
            set { this.enemyEnd = value; }
        }

        public List<Enemy> Enemies
        {
            get { return this.enemies; }
        }

        public void Start()
        {
            this.enemiesSpawning = true;
        }

        public void Reset()
        {
            this.enemies.Clear();
            this.enemiesSpawning = false;
            this.enemiesSpawned = 0;
            this.enemyCount = this.origEnemyCount;
        }

        private void AddEnemy(int Health, float Speed, int Coins)
        {
            Enemy enemy = this.AnimationFrameCount > 0 ? new AnimatedEnemy(this.HealthBarTexture, this.HealthBarBackground, this.enemy, this.map.WayPoints.Peek(), Health, Speed, Coins) : new Enemy(this.HealthBarTexture, this.HealthBarBackground, this.enemy, this.map.WayPoints.Peek(), Health, Speed, Coins);

            if (AnimationFrameCount > 0)
            {
                ((AnimatedEnemy)enemy).SetFrameInfo(this.AnimationFrameCount, this.AnimationWidth, this.AnimationHeight, this.AnimationDelay);
            }

            enemy.SetWayPoints(this.map.WayPoints);
            this.enemies.Add(enemy);
            this.spawnTimer = 0;
            ++this.enemiesSpawned;
        }

        public void SetSpawnDelay(float Delay)
        {
            this.spawnDelay = Delay;
        }

        public void Update(GameTime gameTime)
        {
            if (this.enemiesSpawned == this.enemyCount)
            {
                this.enemiesSpawning = false;
            }

            if (this.enemiesSpawning)
            {
                this.spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.spawnTimer > this.spawnDelay)
                    this.AddEnemy(this.hp, this.speed, this.coins);
            }

            for (int i = 0; i < this.enemies.Count; ++i)
            {
                Enemy enemy = this.enemies[i];
                enemy.Update(gameTime);
                if (enemy.isDead)
                {
                    if (enemy.Health > 0)
                    {
                        enemy.SetWayPoints(this.map.WayPoints);
                        this.EnemySuccess = true;
                        this.player.Lives -= 1;
                        this.player.Coins -= this.player.Coins > 0 ? 1 : 0;
                        this.enemyCount += 1;

                        if (this.player.Lives == 0)
                        {
                            this.enemies.Clear();
                            break;
                        }
                    }
                    else
                    {
                        this.player.Coins += enemy.Coins;
                        this.enemies.Remove(enemy);
                    }

                    if (this.enemiesSpawned >= this.enemyCount)
                    {
                        this.enemiesSpawning = false;
                        this.enemiesSpawned = this.enemyCount;
                        break;
                    }
                    --i;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Enemy enemy in this.enemies)
            {
                enemy.Draw(batch);
            }
        }
    }
}
