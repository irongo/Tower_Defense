using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TowerDefense.Renders;

namespace TowerDefense.GUI
{
    class Interface
    {
        private Texture2D background = null;
        private Texture2D toolbar_background = null;
        private SpriteFont font = null;
        private SpriteFont startbtnfont = null;
        private SpriteFont welcomefont = null;
        private SpriteFont textboxfont = null;
        private Vector2 pos = Vector2.Zero;
        private Vector2 textpos = Vector2.Zero;
        private static Texture2D dummy = null;
        private AnimatedSprite coins = null;
        private Button AddArrowTower = null;
        private Button AddCannonTower = null;
        private Button AddSpeedTower = null;
        private Button AddSpikeTower = null;
        private Button StartButton = null;
        private Texture2D AddArrowTexture = null;
        private Texture2D AddCannonTexture = null;
        private Texture2D AddSpeedTexture = null;
        private Texture2D AddSpikeTexture = null;
        private Controller player = null;
        private WaveMgr waveManager = null;
        private bool Started = false, WaveReady = false;
        private const String WelcomeMessage = "Welcome To Critter Defence!";

        public Interface(Controller player, Texture2D background, Texture2D toolbar_background, SpriteFont font, Vector2 position, Vector2 textpos)
        {
            this.player = player;
            this.background = background;
            this.toolbar_background = toolbar_background;
            this.font = font;
            this.pos = position;
            this.textpos = textpos;
        }

        public bool Enabled
        {
            set
            {
                this.AddArrowTower.Enabled = value;
                this.AddCannonTower.Enabled = value;
                this.AddSpeedTower.Enabled = value;
                this.AddSpikeTower.Enabled = value;
                this.Started = !value;
                this.StartButton.Enabled = value;
            }
        }

        public static Texture2D DummyTexture
        {
            get { return dummy; }
        }

        public void Reset()
        {
            this.Started = false;
            this.StartButton.Enabled = true;
        }

        public void SetStartButton(SpriteFont font, Texture2D[] ButtonBackgrounds, WaveMgr waveManager)
        {
            this.startbtnfont = font;
            this.waveManager = waveManager;
            this.StartButton = new Button(ButtonBackgrounds[0], ButtonBackgrounds[1], ButtonBackgrounds[2], Vector2.Zero);
            this.StartButton.OnMouseClick += new EventHandler(StartButton_OnMouseClick);
        }

        public void SetTowerButtonBackgrounds(Texture2D[] ButtonBackgrounds, Texture2D[] ButtonIcons)
        {
            this.AddArrowTexture = ButtonIcons[0];
            this.AddCannonTexture = ButtonIcons[1];
            this.AddSpeedTexture = ButtonIcons[2];
            this.AddSpikeTexture = ButtonIcons[3];

            this.AddArrowTower = new Button(ButtonBackgrounds[0], ButtonBackgrounds[1], ButtonBackgrounds[2], Vector2.Zero);
            this.AddCannonTower = new Button(ButtonBackgrounds[0], ButtonBackgrounds[1], ButtonBackgrounds[2], Vector2.Zero);
            this.AddSpeedTower = new Button(ButtonBackgrounds[0], ButtonBackgrounds[1], ButtonBackgrounds[2], Vector2.Zero);
            this.AddSpikeTower = new Button(ButtonBackgrounds[0], ButtonBackgrounds[1], ButtonBackgrounds[2], Vector2.Zero);

            this.AddArrowTower.OnMouseDown += new EventHandler(AddArrowTower_OnMouseDown);
            this.AddCannonTower.OnMouseDown += new EventHandler(AddCannonTower_OnMouseDown);
            this.AddSpeedTower.OnMouseDown += new EventHandler(AddSpeedTower_OnMouseDown);
            this.AddSpikeTower.OnMouseDown += new EventHandler(AddSpikeTower_OnMouseDown);
        }

        private void StartButton_OnMouseClick(object sender, EventArgs e)
        {
            this.waveManager.StartNextWave();
            this.StartButton.Enabled = false;
            this.Started = true;
            this.WaveReady = true;
        }

        private void AddArrowTower_OnMouseDown(object sender, EventArgs e)
        {
            player.CreateNewTower(TowerTypes.ARROW_TOWER);            
        }

        private void AddCannonTower_OnMouseDown(object sender, EventArgs e)
        {
            player.CreateNewTower(TowerTypes.CANNON_TOWER);  
        }

        private void AddSpeedTower_OnMouseDown(object sender, EventArgs e)
        {
            player.CreateNewTower(TowerTypes.SPEED_TOWER);  
        }

        private void AddSpikeTower_OnMouseDown(object sender, EventArgs e)
        {
            player.CreateNewTower(TowerTypes.SPIKE_TOWER);  
        }

        public void SetCoinInfo(Texture2D texture, Vector2 position, int TotalFrames, int Width, int Height, float Delay)
        {
            this.coins = new AnimatedSprite(texture, position);
            this.coins.SetFrameInfo(TotalFrames, Width, Height, Delay);
            this.textpos.X += Width;
            this.textpos.Y += Height / 4;
        }

        public void SetTextboxFonts(SpriteFont welcomefont, SpriteFont textboxfont)
        {
            this.welcomefont = welcomefont;
            this.textboxfont = textboxfont;
        }

        public void SetDummyTexture(Texture2D texture)
        {
            dummy = texture;
            dummy.SetData(new Color[] {Color.White});
        }

        public static void DrawRectangle(Rectangle rec, Texture2D tex, Color col, SpriteBatch spriteBatch, bool solid, int thickness)
        {
            if (!solid)
            {

                Vector2 Position = new Vector2(rec.X, rec.Y);
                int border = thickness;

                int borderWidth = (int)(rec.Width) + (border * 2);
                int borderHeight = (int)(rec.Height) + (border);

                drawLine(new Vector2((int)rec.X, (int)rec.Y), new Vector2((int)rec.X + rec.Width, (int)rec.Y), tex, col, spriteBatch, thickness); //top bar 
                drawLine(new Vector2((int)rec.X, (int)rec.Y + rec.Height), new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height), tex, col, spriteBatch, thickness); //bottom bar 
                drawLine(new Vector2((int)rec.X, (int)rec.Y), new Vector2((int)rec.X, (int)rec.Y + rec.Height), tex, col, spriteBatch, thickness); //left bar 
                drawLine(new Vector2((int)rec.X + rec.Width, (int)rec.Y), new Vector2((int)rec.X + rec.Width, (int)rec.Y + rec.Height), tex, col, spriteBatch, thickness); //right bar 
            }
            else
            {
                spriteBatch.Draw(tex, new Vector2((float)rec.X, (float)rec.Y), rec, col, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            }

        }

        public static void drawLine(Vector2 A, Vector2 B, Texture2D tex, Color col, SpriteBatch spriteBatch, int thickness)
        {
            Rectangle rec;
            if (A.X < B.X)
            {
                rec = new Rectangle((int)A.X, (int)A.Y, (int)(B.X - A.X), thickness);
            }
            else
            {
                rec = new Rectangle((int)A.X, (int)A.Y, thickness, (int)(B.Y - A.Y));
            }

            spriteBatch.Draw(tex, rec, col);
        }

        public void Update(GameTime gameTime)
        {
            this.coins.Update(gameTime);
            this.AddArrowTower.Update(gameTime);
            this.AddCannonTower.Update(gameTime);
            this.AddSpeedTower.Update(gameTime);
            this.AddSpikeTower.Update(gameTime);
            this.StartButton.Update(gameTime);
            if (this.waveManager.NextWaveReady && player.Lives > 0)
            {
                this.StartButton.Enabled = true;
                this.WaveReady = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.background, new Rectangle((int)this.pos.X, (int)this.pos.Y, this.background.Width, (int)Map.ToMapSpace(new Vector2(0, Map.LevelHeight + 2)).Y), null, Color.White);
            batch.Draw(this.toolbar_background, this.pos, Color.White);
            this.coins.Draw(batch);
            batch.DrawString(font, player.Coins.ToString(), this.textpos, player.Coins < 20 ? Color.Red : Color.Lime);
            batch.DrawString(font, "Lives: " + player.Lives.ToString(), this.textpos + new Vector2(this.coins.GetWidth * 3, 0), player.Lives < 10 ? Color.Red : Color.Lime);

            Rectangle bg = new Rectangle((int)this.pos.X + 10, (int)this.pos.Y + 50, 200, 100);
            DrawRectangle(bg, dummy, Color.Black, batch, false, 1);
            DrawRectangle(new Rectangle((int)this.pos.X + 10, (int)this.pos.Y + bg.Bottom + 20, 200, 200), dummy, Color.Black, batch, false, 1);

            Vector2 ButtonPosition = new Vector2(50.0f, 0.0f);
            this.AddArrowTower.SetScale(new Vector2(this.pos.X + 15, this.pos.Y + bg.Bottom + 25), 0.8f);
            this.AddCannonTower.SetScale(this.AddArrowTower.Position + ButtonPosition, 0.8f);
            this.AddSpeedTower.SetScale(this.AddCannonTower.Position + ButtonPosition, 0.8f);
            this.AddSpikeTower.SetScale(this.AddSpeedTower.Position + ButtonPosition, 0.8f);

            this.AddArrowTower.Draw(batch);
            this.AddCannonTower.Draw(batch);
            this.AddSpeedTower.Draw(batch);
            this.AddSpikeTower.Draw(batch);

            ButtonPosition.X = 3.0f;
            ButtonPosition.Y = 6.0f;
            batch.Draw(this.AddArrowTexture, this.AddArrowTower.Position + ButtonPosition, this.AddArrowTower.Enabled ? Color.White : Color.Gray);
            batch.Draw(this.AddCannonTexture, this.AddCannonTower.Position + ButtonPosition, this.AddCannonTower.Enabled ? Color.White : Color.Gray);

            ButtonPosition.Y = 3.0f;
            batch.Draw(this.AddSpeedTexture, this.AddSpeedTower.Position + ButtonPosition, this.AddSpeedTower.Enabled ? Color.White : Color.Gray);
            batch.Draw(this.AddSpikeTexture, this.AddSpikeTower.Position + ButtonPosition, this.AddSpikeTower.Enabled ? Color.White : Color.Gray);

            Vector2 TextBoxPosition = Map.ToMapSpace(new Vector2(0, Map.LevelHeight - 2));
            DrawRectangle(new Rectangle((int)TextBoxPosition.X, (int)TextBoxPosition.Y, (Map.LevelWidth - 7) * Map.TileWidth, Map.TileHeight * 2), dummy, Color.White, batch, true, 1);

            Vector2 StartButtonPosition = new Vector2((int)this.pos.X + 10, (int)this.pos.Y + bg.Bottom + 227);
            this.StartButton.Bounds = new Rectangle((int)StartButtonPosition.X, (int)StartButtonPosition.Y, StartButton.Bounds.Width, Map.TileHeight);
            this.StartButton.Draw(batch, Color.White, Color.Turquoise);

            if (!this.waveManager.Finished)
            {
                if (this.player.Lives <= 0)
                {
                    this.Started = false;
                    this.StartButton.Enabled = false;
                }

                batch.DrawString(this.startbtnfont, this.StartButton.Enabled ? !Started ? "Start" : "Next Wave" : "Wave: " + this.waveManager.LastLevel, this.StartButton.Position + new Vector2(this.StartButton.Enabled ? Started ? 6 : 25 : 15, 7), this.StartButton.Enabled ? Color.Yellow : Color.Gray);
                batch.DrawString(this.welcomefont, this.StartButton.Enabled ? !Started ? WelcomeMessage : "Get Ready for the next wave!" : "Level: " + this.waveManager.LastLevel, Map.ToMapSpace(new Vector2(0.05f, Map.LevelHeight - 2)), Color.Black);

                if (this.Started && this.WaveReady)
                {
                    batch.DrawString(this.textboxfont, this.waveManager.CurrentWave.Description, Map.ToMapSpace(new Vector2(2.25f, (Map.LevelHeight - 2) + 0.02f)), Color.Black);
                }
            }
        }
    }
}
