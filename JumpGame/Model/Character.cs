using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JumpGame
{
    public class Character
    {
        // 캐릭터 X 좌표
        private int _x;

        // 캐릭터 Y 좌표
        private int _y;

        // 캐릭터 초기 Y 좌표
        private int _initialY;

        // 캐릭터의 너비
        private int _width = 30;

        // 캐릭터의 높이
        private int _height = 50;

        // 수직 속도
        private float _velocityY = 0;

        // 중력 가속도
        private float _gravity = 0.4f;

        // 캐릭터 착지 여부
        private bool _onGround = false;

        // 좌우 이동 속도
        private int _moveSpeed = 5;

        // 캐릭터 초기 좌표 지정
        public Character(int startX, int startY)
        {
            _x = startX;
            _y = startY;
            // 초기 Y 좌표 저장
            _initialY = startY;      
        }

        // 캐릭터 X 좌표
        public int GetX()
        {
            return _x;
        }

        // 캐릭터 Y 좌표
        public int GetY()
        {
            return _y;
        }

        // 캐릭터 히트 박스
        public Rectangle GetHitBox()
        {
            return new Rectangle(_x, _y, _width, _height);
        }

        // 왼쪽으로 이동
        public void MoveLeft()
        {
            _x = _x - _moveSpeed;
        }

        // 오른쪽으로 이동
        public void MoveRight()
        {
            _x = _x + _moveSpeed;
        }

        // 점프
        public void Jump()
        {
            if (_onGround == true)
            {
                // 위쪽으로 속도 부여
                _velocityY = -7.5f;
                _onGround = false;
            }
        }

        // 현재 착지 여부 반환
        public bool IsOnGround()
        {
            return _onGround;
        }       

        // 캐릭터 상태 업데이트
        public void CharacterUpdate(List<Platform> platforms, int cameraY)
        {
            // 중력 가속도에 따라 수직 속도 증가
            _velocityY = _velocityY + _gravity;

            // Y 좌표 속도만큼 위치 변경
            _y = _y + (int)_velocityY;

            // 착지 여부 초기화
            _onGround = false;

            // 모든 발판과 충돌 검사
            for (int i = 0; i < platforms.Count; i++)
            {
                Platform platform = platforms[i];

                // 비활성화된 발판은 무시
                if (platform.IsActive == false)
                {
                    continue;
                }

                // 현재 발판의 위치와 크기 정보
                Rectangle platformRect = platform.Area;
                // futureHitBox는 캐릭터의 현재 위치 기준 히트 박스
                Rectangle futureHitBox = new Rectangle(_x, _y, _width, _height);

                // 캐릭터 히트 박스와 발판이 겹치는지 검사
                // intersectsWirh : 두 개의 사각형이 서로 겹치는지 확인해주는 함수
                if (futureHitBox.IntersectsWith(platformRect) == true)
                {
                    // 캐릭터가 위에서 아래로 떨어지면서 발판에 착지하는 경우에만 처리
                    if (_velocityY > 0 && _y + _height <= platformRect.Y + _velocityY)
                    {
                        // 캐릭터를 발판 바로 위에 위치시킴
                        _y = platformRect.Y - _height;

                        // 착지 후 멈춤
                        _velocityY = 0;

                        // 착지 상태로 변경
                        _onGround = true;

                        // 발판 밟음 처리
                        platform.OnStepped();
                    }
                }
            }

            // 캐릭터 최초 Y좌표보다 더 아래로 내려가지 않도록 제한
            if (_y > _initialY)
            {
                // Y 위치를 초기 위치로 고정
                _y = _initialY;
                // 수직 속도 제거
                _velocityY = 0;
                // 착지 상태로 설정
                _onGround = true;  
            }
        }

        // 캐릭터 그림
        public void Draw(Graphics g, int offsetX, int offsetY)
        {
            g.FillRectangle(Brushes.Green, _x - offsetX, _y - offsetY, _width, _height);
        }
    }
}
