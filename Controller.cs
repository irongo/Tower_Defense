using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TowerDefense.Towers;

namespace TowerDefense
{
    enum TowerTypes
    {
        TOWER_NONE, ARROW_TOWER, CANNON_TOWER, SPEED_TOWER, SPIKE_TOWER
    }

    class Controller
    {
        private int lives = 20;
        private int coins = 40;
        private int startlives = 20;
        private int startcoins = 40;
        private Map map = null;
        private Texture2D[] towertextures = null;
        private Texture2D[] bullettextures = null;
        private Vector2 Cell = Vector2.Zero;
        private Vector2 Tile = Vector2.Zero;
        private List<Tower> towers = new List<Tower>();
        private MouseState mouseOldState, mouseNewState;
        private TowerTypes TowerType = TowerTypes.TOWER_NONE;

        public Controller(Map map, Texture2D[] towertextures, Texture2D[] bullettextures)
        {
            this.map = map;
            this.towertextures = towertextures;
            this.bullettextures = bullettextures;
        }

        public int Coins
        {
            get { return this.coins; }
            set { this.coins = value; }
        }

        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }

        private bool CanPlaceTower()
        {
            bool valid = this.Cell.X >= 0 && this.Cell.Y >= 0 && this.Cell.X < this.map.Width && this.Cell.Y < this.map.Height;

            foreach (Tower tower in this.towers)
            {
                if (tower.Position == this.Tile)
                {
                    valid = false;
                    break;
                }
            }
            return valid && (this.map.GetMapIndex(this.Cell) != 1);
        }

        public void CreateNewTower(TowerTypes type)
        {
            this.TowerType = type;
        }

        public void Reset()
        {
            this.towers.Clear();
            this.lives = this.startlives;
            this.coins = this.startcoins;
        }

        public void AddTower()
        {
            Tower nt = null;

            switch(this.TowerType)
            {
                case TowerTypes.ARROW_TOWER:
                    nt = new ArrowTower(this.towertextures[(int)TowerTypes.ARROW_TOWER - 1], this.bullettextures[(int)TowerTypes.ARROW_TOWER - 1], this.Tile);
                    break;

                case TowerTypes.SPIKE_TOWER:
                    nt = new SpikeTower(this.towertextures[(int)TowerTypes.SPIKE_TOWER - 1], this.bullettextures[(int)TowerTypes.SPIKE_TOWER - 1], this.Tile);
                    break;

                case TowerTypes.SPEED_TOWER:
                    nt = new SpeedTower(this.towertextures[(int)TowerTypes.SPEED_TOWER - 1], this.bullettextures[(int)TowerTypes.SPEED_TOWER - 1], this.Tile);
                    break;

                case TowerTypes.CANNON_TOWER:
                    nt = new CannonTower(this.towertextures[(int)TowerTypes.CANNON_TOWER - 1], this.bullettextures[(int)TowerTypes.CANNON_TOWER - 1], this.Tile);
                    break;
            }

            if (nt != null && this.CanPlaceTower() && nt.Cost <= this.coins)
            {
                this.towers.Add(nt);
                this.coins -= nt.Cost;
                this.TowerType = TowerTypes.TOWER_NONE;
            }
            this.TowerType = TowerTypes.TOWER_NONE;
        }

        public void PreviewTower(SpriteBatch batch)
        {
            if (this.TowerType != TowerTypes.TOWER_NONE)
            {
                this.Cell = new Vector2((int)(this.mouseOldState.X / Map.TileWidth), (int)(this.mouseOldState.Y / Map.TileHeight));
                this.Tile = new Vector2(this.Cell.X * Map.TileWidth, this.Cell.Y * Map.TileHeight);
                Texture2D texture = this.towertextures[(int)this.TowerType - 1];
                batch.Draw(texture, new Rectangle(this.mouseOldState.X - texture.Width / 2, this.mouseOldState.Y - texture.Height / 2, texture.Width, texture.Height), Color.White);
            }
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            this.mouseNewState = Mouse.GetState();
            this.Cell = new Vector2((int)(this.mouseNewState.X / Map.TileWidth), (int)(this.mouseNewState.Y / Map.TileHeight));
            this.Tile = new Vector2(this.Cell.X * Map.TileWidth, this.Cell.Y * Map.TileHeight);
            if (this.mouseNewState.LeftButton == ButtonState.Released && this.mouseOldState.LeftButton == ButtonState.Pressed)
            {
                this.AddTower();
            }

            foreach (Tower tower in towers)
            {
                if (!tower.HasTarget)
                {
                    tower.TargetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }

            this.mouseOldState = this.mouseNewState;
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(batch);
            }
        }
    }
}
