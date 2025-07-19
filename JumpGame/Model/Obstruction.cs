using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpGame.Model
{
    public class Obstruction
    {
        private int _score = 0; // 현재 점수
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
    }
}
