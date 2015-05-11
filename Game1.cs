using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TowerDefense.GUI;

namespace TowerDefense
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map = new Map();
        WaveMgr waveManager = null;
        Controller player = null;
        Interface infoInterface = null;
        EndInterface endInterface = null;

        public Song music;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            map.Setup(graphics);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            music = Content.Load<Song>("Magic_Marker");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            map.LoadTextures(this);

            Texture2D[] towerTextures = new Texture2D[] {
                Content.Load<Texture2D>("Towers/ArrowTower"),
                Content.Load<Texture2D>("Towers/CannonTower"),
                Content.Load<Texture2D>("Towers/DualShot"),
                Content.Load<Texture2D>("Towers/SpikeTower")
            };

            Texture2D[] bulletTextures = new Texture2D[] {
                Content.Load<Texture2D>("Bullets/Arrow"),
                Content.Load<Texture2D>("Bullets/Cannonball"),
                Content.Load<Texture2D>("Bullets/WaterBullet"),
                Content.Load<Texture2D>("Bullets/Bullet")
            };

            player = new Controller(map, towerTextures, bulletTextures);

            Texture2D bluehealthbar = Content.Load<Texture2D>("HealthBar/BlueProgressbar");
            Texture2D greenhealthbar = Content.Load<Texture2D>("HealthBar/GreenProgressbar");
            Texture2D healthbarbackground = Content.Load<Texture2D>("HealthBar/ProgressbarBackground");

            waveManager = new WaveMgr(map, new Wave[]{
                new Wave("Rats", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Rat"), player, 1, 25, 1.0f, 1, 200, 4, 100.0f, 34, 32, map),
                new Wave("Scorpions", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Scorpion"), player, 2, 25, 1.0f, 1, 42, 8, 100.0f, 31, 31, map),
                new Wave("Beetles", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Beetle"), player, 3, 25, 1.0f, 1, 65, 3, 100.0f, 32, 32, map),
                new Wave("Chitiniacs", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Chitiniac"), player, 4, 25, 1.0f, 1, 75, 8, 100.0f, 31, 31, map),
                new Wave("Cockroaches", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Cockroach"), player, 5, 25, 1.0f, 1, 101, 8, 100.0f, 31, 31, map),

                new Wave("Bandits", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Bandit"), player, 6, 25, 1.5f, 1, 87, 8, 100.0f, 31, 31, map),
                new Wave("Crawlers", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Crawler"), player, 7, 25, 1.0f, 2, 136, 7, 100.0f, 36, 36, map),
                new Wave("Hornets", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Hornet"), player, 8, 25, 1.0f, 1, 158, 8, 100.0f, 31, 31, map),
                new Wave("Ogres", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Ogre"), player, 9, 25, 1.0f, 1, 189, 8, 100.0f, 31, 31, map),
                new Wave("PigKlivers", greenhealthbar, healthbarbackground,  Content.Load<Texture2D>("NPCs/PigKliver"), player, 10, 25, 1.0f, 2, 212, 8, 100.0f, 31, 31, map),

                new Wave("[BOSS] Mage", bluehealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Mage"), player, 10, 1, 1.0f, 45, 2000, 8, 100.0f, 31, 31, map),
                new Wave("Trolls", greenhealthbar, healthbarbackground, Content.Load<Texture2D>("NPCs/Troll"), player, 11, 25, 1.0f, 2, 245, 8, 100.0f, 31, 31, map)
            });

            infoInterface = new Interface(player, Content.Load<Texture2D>("GUI/Toolbar"), Content.Load<Texture2D>("GUI/Toolbar"), Content.Load<SpriteFont>("Fonts/ToolbarFont"), Map.ToMapSpace(new Vector2(map.Width, 0)), Map.ToMapSpace(new Vector2(map.Width, 0)));
            infoInterface.SetCoinInfo(Content.Load<Texture2D>("GUI/Coin"), Map.ToMapSpace(new Vector2(map.Width, 0.1f)), 7, 32, 32, 100.0f);
            infoInterface.SetDummyTexture(new Texture2D(this.graphics.GraphicsDevice, 1, 1));

            Texture2D ButtonBackground = Content.Load<Texture2D>("GUI/ButtonBackground");
            Texture2D ButtonHoverBackground = Content.Load<Texture2D>("GUI/ButtonHoverBackground");
            Texture2D ButtonClickBackground = Content.Load<Texture2D>("GUI/ButtonClickBackground");

            Texture2D StartButtonBackground = Content.Load<Texture2D>("GUI/StartButtonBackground");
            Texture2D StartButtonHoverBackground = Content.Load<Texture2D>("GUI/StartButtonHoverBackground");
            Texture2D StartButtonClickBackground = Content.Load<Texture2D>("GUI/StartButtonClickBackground");
            infoInterface.SetTowerButtonBackgrounds(new Texture2D[] { ButtonBackground, ButtonClickBackground, ButtonHoverBackground }, towerTextures);
            infoInterface.SetStartButton(Content.Load<SpriteFont>("Fonts/StartButtonFont"), new Texture2D[] { StartButtonBackground, StartButtonClickBackground, StartButtonHoverBackground }, waveManager);
            infoInterface.SetTextboxFonts(Content.Load<SpriteFont>("Fonts/TextboxWelcomeFont"), Content.Load<SpriteFont>("Fonts/TextboxFont"));

            endInterface = new EndInterface(infoInterface, player, waveManager, new SpriteFont[] { Content.Load<SpriteFont>("Fonts/EndInterfaceTitle"), Content.Load<SpriteFont>("Fonts/EndInterfaceScoreTitle"), Content.Load<SpriteFont>("Fonts/EndInterfaceTitle"), Content.Load<SpriteFont>("Fonts/StartButtonFont") }, new Texture2D[] { StartButtonBackground, StartButtonClickBackground, StartButtonHoverBackground });
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!waveManager.Finished && player.Lives > 0)
            {
                waveManager.Update(gameTime);
                if (!waveManager.Finished)
                {
                    player.Update(gameTime, waveManager.Enemies);
                }
            }

            infoInterface.Update(gameTime);
            endInterface.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(/*SpriteSortMode.Immediate, BlendState.AlphaBlend*/);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            if (!waveManager.Finished && player.Lives > 0)
            {
                waveManager.Draw(spriteBatch);
            }
            else if (!waveManager.Finished && player.Lives <= 0)
            {
                endInterface.Draw(spriteBatch, false);
            }
            else if (waveManager.Finished && player.Lives > 0)
            {
                endInterface.Draw(spriteBatch, true);
            }
            infoInterface.Draw(spriteBatch);
            player.PreviewTower(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
