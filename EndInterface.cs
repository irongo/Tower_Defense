using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense.GUI
{
    class EndInterface
    {
        private Button RetryButton = null;
        private Interface iface = null;
        private Controller player = null;
        private WaveMgr waveManager = null;
        private SpriteFont[] fonts = null;

        public EndInterface(Interface iface, Controller player, WaveMgr waveManager, SpriteFont[] fonts, Texture2D[] RetryButtonTextures)
        {
            this.iface = iface;
            this.player = player;
            this.waveManager = waveManager;
            this.fonts = fonts;
            this.RetryButton = new Button(RetryButtonTextures[0], RetryButtonTextures[1], RetryButtonTextures[2], Vector2.Zero);
            this.RetryButton.OnMouseClick += new EventHandler(RetryButton_OnMouseClick);
            this.RetryButton.Enabled = false;
        }

        private void Reset()
        {
            waveManager.Reset();
            this.iface.Reset();
            this.player.Reset();
        }

        private void RetryButton_OnMouseClick(object sender, EventArgs e)
        {
            this.iface.Enabled = true;
            this.RetryButton.Enabled = false;
            this.Reset();
        }

        public void Update(GameTime gameTime)
        {
            this.RetryButton.Update(gameTime);
        }

        public void Draw(SpriteBatch batch, bool PlayerWon)
        {
            this.iface.Enabled = false;
            this.RetryButton.Enabled = true;
            Rectangle GameArea = new Rectangle(0, 0, Map.LevelWidth * Map.TileWidth, Map.LevelHeight * Map.TileHeight);
            Interface.DrawRectangle(GameArea, Interface.DummyTexture, Color.Black * 0.85f, batch, true, 1);

            int Score = player.Coins + player.Lives;
            Vector2 TitlePosition = new Vector2(PlayerWon ? 85 : 65, 40);

            batch.DrawString(fonts[0], PlayerWon ? "You Won!" : "Game Over", TitlePosition, PlayerWon ? Color.White : Color.Red);
            batch.DrawString(fonts[1], "Your Score Is: " + Score, TitlePosition + new Vector2(Score < 10000 ? -25 : Score < 100000 ? -30 : -40, 50), Color.Lime);
            batch.DrawString(fonts[2], "Level: " + waveManager.LastLevel, TitlePosition + new Vector2(15, 100), Color.Lime);

            this.RetryButton.SetScale(new Vector2(100, 250), 1.0f);
            this.RetryButton.Draw(batch);
            batch.DrawString(fonts[3], "Try Again", new Vector2(this.RetryButton.Position.X + 3, this.RetryButton.Position.Y + 12), Color.Yellow);
        }
    }
}
