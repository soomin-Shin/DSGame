using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpGame
{

    public class Camera
    {
        // 카메라의 X 좌표
        private int _x;

        // 카메라의 Y 좌표
        private int _y;

        // 화면 가로 크기
        private int _screenWidth;

        // 화면 세로 크기
        private int _screenHeight;

        // 배경 화면 높이
        private int _worldHeight;

        public int X
        {
            get
            {
                return _x;
            }
            private set
            {
                _x = value;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
            private set
            {
                _y = value;
            }
        }

        // 화면 크기 초기화
        public Camera(int screenWidth, int screenHeight, int worldHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _worldHeight = worldHeight;
        }

        // 카메라를 캐릭터 위치 기준으로 조정
        public void CameraUpdate(int targetX, int targetY)
        {
            // 좌우 스크롤 필요시 아래 주석 해제, 현재 0 고정
            _x = 0;

            // 카메라 Y 좌표는 캐릭터 Y 좌표 중심에서 화면의 절반
            _y = targetY - _screenHeight / 2;

            // 화면의 최상단보다 위로 이동하지 않도록 제한
            if (_y < 0)
            {
                _y = 0;
            }

            // 배경 화면 하단 넘어가지 않도록 제한
            int maxY = _worldHeight - _screenHeight;
            if (_y > maxY)
            {
                _y = maxY;
            }
        }


        // 카메라 위치를 초기화
        public void CameraReset()
        {
            // 카메라의 X, Y 좌표를 0으로 초기화
            _x = 0;
            _y = 0;
        }
    }
}
