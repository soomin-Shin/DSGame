using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpGame.Model
{
    public class GameStats
    {
        private int _score = 0; // Current score
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }

        private int _lives = 3; // Initial number of lives
        public int Lives
        {
            get
            {
                return _lives;
            }
            set
            {
                _lives = value;
            }
        }

        private TimeSpan _elapsedTime; // Elapsed game time
        public TimeSpan ElapsedTime
        {
            get
            {
                return _elapsedTime;
            }
            set
            {
                _elapsedTime = value;
            }
        }
        /// <summary>
        /// 정보 리셋
        /// </summary>
        public void StatsReset()
        {
            this._score = 0;
            this._lives = 3;
            this._elapsedTime = TimeSpan.Zero;
        }
    }
}
