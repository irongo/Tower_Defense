using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TowerDefense
{
    class WaveMgr
    {
        private int waveCount = 0;
        private Queue<Wave> waves = new Queue<Wave>();
        private bool nextWaveReady = true;
        private bool waveFinished = false;
        private Map map = null;
        private Wave[] waveArray = null;
        private int lastLevel = 1;

        public WaveMgr(Map map, Wave[] waves)
        {
            this.map = map;
            this.waveArray = waves;
            this.waveCount = waves.Count();
            
            foreach (Wave wave in waves)
            {
                this.waves.Enqueue(wave);
            }
        }

        public int LastLevel
        {
            get { return this.lastLevel; }
        }

        public bool NextWaveReady
        {
            get { return this.nextWaveReady; }
        }

        public bool Finished
        {
            get { return this.waves.Count == 0; }
        }

        public Wave CurrentWave
        {
            get { return this.waves.Peek(); }
        }

        public Wave GetWave(int Index)
        {
            return this.waves.ElementAt(Index);
        }

        public List<Enemy> Enemies
        {
            get { return this.CurrentWave.Enemies; }
        }

        public int Level
        {
            get { return this.CurrentWave.WaveLevel; }
        }

        public void Reset()
        {
            this.waves.Clear();
            this.waveFinished = true;
            this.nextWaveReady = true;

            foreach (Wave wave in waveArray)
            {
                wave.Reset();
                this.waves.Enqueue(wave);
            }
        }

        public void StartNextWave()
        {
            if (this.waves.Count > 0 && this.nextWaveReady)
            {
                this.waves.Peek().Start();
                this.waveFinished = false;
                this.nextWaveReady = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.CurrentWave.Update(gameTime);
            this.lastLevel = this.CurrentWave.WaveLevel;

            if (this.CurrentWave.WaveOver)
            {
                this.waveFinished = true;
            }

            if (this.waveFinished && !this.nextWaveReady)
            {
                this.waves.Dequeue();
                this.nextWaveReady = true;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            this.CurrentWave.Draw(batch);
        }
    }
}
