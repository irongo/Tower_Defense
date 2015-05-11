using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    class Map
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;
        public const int LevelWidth = 17;
        public const int LevelHeight = 13;
        private Queue<Vector2> Waypoints = new Queue<Vector2>();
        private List<Texture2D> Tiles = new List<Texture2D>();

        private int[,] map = new int[,]
        {
            {0,0,0,1,0,1,0,0,0,0},
            {0,1,1,1,0,1,1,1,1,0},
            {0,1,0,0,0,0,0,0,1,0},
            {0,1,0,1,1,1,1,0,1,0},
            {0,1,0,1,0,0,1,0,1,0},
            {0,1,1,1,0,0,1,0,1,0},
            {0,0,0,0,0,0,1,0,1,0},
            {0,1,1,1,1,1,1,0,1,0},
            {0,1,0,0,0,0,0,0,1,0},
            {0,1,1,1,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0}
        };

        public Map()
        {
            this.Waypoints.Enqueue(new Vector2(3 * TileWidth, 0 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(3 * TileWidth, 1 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(1 * TileWidth, 1 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(1 * TileWidth, 5 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(3 * TileWidth, 5 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(3 * TileWidth, 3 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(6 * TileWidth, 3 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(6 * TileWidth, 7 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(1 * TileWidth, 7 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(1 * TileWidth, 9 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(8 * TileWidth, 9 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(8 * TileWidth, 1 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(5 * TileWidth, 1 * TileHeight));
            this.Waypoints.Enqueue(new Vector2(5 * TileWidth, 0 * TileHeight));
        }

        public void Setup(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = LevelWidth * Map.TileWidth;
            graphics.PreferredBackBufferHeight = LevelHeight * Map.TileHeight;
            graphics.ApplyChanges();
        }

        public void LoadTextures(Game game)
        {
            Tiles.Add(game.Content.Load<Texture2D>("Scenary/Grass"));
            Tiles.Add(game.Content.Load<Texture2D>("Scenary/Dirt"));
        }

        public int GetMapIndex(Vector2 Cell)
        {
            return Cell.X < 0 || Cell.Y < 0 || Cell.X > Width - 1 || Cell.Y > Height - 1 ? -1 : this.map[(int)Cell.Y, (int)Cell.X];
        }

        public int Width
        {
            get { return this.map.GetLength(1); }
        }

        public int Height
        {
            get { return this.map.GetLength(0); }
        }

        public Queue<Vector2> WayPoints
        {
            get { return this.Waypoints; }
        }

        public static Vector2 ToMapSpace(Vector2 Position)
        {
            return new Vector2(Position.X * TileWidth, Position.Y * TileHeight);
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    int index = map[i, j];
                    if (index < 0) continue;
                    batch.Draw(Tiles[index], new Rectangle(j * TileWidth, i * TileHeight, TileWidth, TileHeight), Color.White);
                }
            }
        }
    }
}
