using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JumpGame
{
    // 발판 종류
    public enum PlatformType
    {   
        // 일반 발판
        Normal,
        // 움직이는 발판
        Moving,
        // 일정 시간 경과 후 사라지는 발판
        Disappear,
        // 밟으면 사라지는 발판
        StepDisappear,
        // 골인 발판
        Goal
    }

    public class Platform
    {
        // 발판 위치와 크기
        public Rectangle Area;

        // 발판 종류
        public PlatformType Type;

        // 발판 활성화 여부
        public bool IsActive = true;

        // 움직이는 발판 시작 위치
        private int _startX;

        // 좌우 이동 범위
        private int _moveRange = 150;

        // 이동 속도
        private int _moveSpeed = 2;

        // 이동 방향 (오른쪽)
        private int _direction = 1;

        // 발판을 밟은 이후 사용 할 타이머
        private int _timer = 0;

        // 발판이 사라지는 데 걸리는 시간
        private int _disappearDelay = 50;

        // 밟은 발판이 사라지는 데 걸리는 시간
        private int _stepDisappearDelay = 5;

        // 발판이 사라진 후 다시 나타나는 시간
        private int _reappearDelay = 100;

        // 발판을 밟았는지 여부
        private bool _stepped = false;

        public Platform(Rectangle area, PlatformType type)
        {
            Area = area;
            Type = type;

            if (Type == PlatformType.Moving)
            {
                _startX = area.X;
            }
        }

        // 캐릭터가 발판을 밟았을 때
        public void OnStepped()
        {
            if (Type == PlatformType.StepDisappear)
            {
                _stepped = true;
            }
        }

        // 발판 상태 업데이트
        public void PlatformUpdate()
        {
            // 움직이는 발판
            if (Type == PlatformType.Moving)
            {
                Area = new Rectangle(Area.X + _direction * _moveSpeed, Area.Y, Area.Width, Area.Height);

                // 발판이 왼쪽으로 이동
                if (Area.X > _startX + _moveRange)
                {
                    _direction = -1;
                }
                // 발판이 오른쪽으로 이동
                else if (Area.X < _startX - _moveRange)
                {
                    _direction = 1;
                }
            }
            // 일정 시간 경과 후 사라지는 발판
            if (Type == PlatformType.Disappear)
            {
                    _timer++;

                    // 발판이 사라지도록 설정 된 시간이 같아졌을 때
                    if (_timer == _disappearDelay)
                    {
                        // 발판 비활성화
                        IsActive = false;
                    }

                    // 발판이 사라진 이후 지난 시간이 다시 나타나도록 설정 된 시간과 같거나 커졌을 때
                    if (_timer >= _reappearDelay)
                    {
                        // 발판 활성화
                        IsActive = true;
                        _timer = 0;
                    }
           
            }
            // 밟으면 사라지는 발판
            if (Type == PlatformType.StepDisappear)
            {
                // 발판을 밟았을 때
                if (_stepped == true)
                {
                    _timer++;

                    // 밟은 이후 시간과 발판이 사라지도록 설정 된 시간이 같아졌을 때
                    if (_timer == _stepDisappearDelay)
                    {
                        // 발판 비활성화
                        IsActive = false;
                    }

                    // 발판이 사라진 이후 지난 시간이 다시 나타나도록 설정 된 시간과 같거나 커졌을 때
                    if (_timer >=  _reappearDelay)
                    {
                        // 발판 활성화
                        IsActive = true;
                        _stepped = false;
                        _timer = 0;
                    }
                }
            }
        }

        // 발판 그리기
        public void PlatformDraw(Graphics g, int offsetX, int offsetY)
        {
            // 발판 활성화 여부 체크
            if (IsActive == false)
            {
                return;
            }

            //기본 발판 갈색
            Brush brush = Brushes.Brown;

            //움직이는 발판 주황색
            if (Type == PlatformType.Moving)
            {
                brush = Brushes.Orange;
            }
            // 일정 시간 경과 후 사라지는 발판 파란색
            if (Type == PlatformType.Disappear)
            {
                brush = Brushes.Blue;
            }
            // 밟으면 사라지는 발판 회색
            else if (Type == PlatformType.StepDisappear)
            {
                brush = Brushes.Gray;
            }
            // 골인 발판 노란색
            else if (Type == PlatformType.Goal)
            {
                brush = Brushes.Yellow;
            }

            g.FillRectangle(brush, Area.X - offsetX, Area.Y - offsetY, Area.Width, Area.Height);
        }
    }
}
