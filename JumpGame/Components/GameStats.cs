using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

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

        private int _elapsedTime; // Elapsed game time
        public int ElapsedTime
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

        private Timer _gameTimer;         // 초 단위 타이머
        public Timer GameTimer
        {
            get
            {
                return _gameTimer;
            }
            set
            {
                _gameTimer = value;
            }
        }
        public GameStats()
        {
            GameTimer = new Timer();
            GameTimer.Interval = 1000; // 1000ms = 1초
            GameTimer.Start(); // 타이머 시작
        }

        public void StatsUpdateTimer()
        {
            ElapsedTime++;
        }
        /// <summary>
        /// 정보 리셋
        /// </summary>
        public void StatsReset()
        {
            this._score = 0;
            this._lives = 3;
            this._elapsedTime = 0;
        }
    }
}
