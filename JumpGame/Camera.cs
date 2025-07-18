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
        public Camera(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        // 카메라를 캐릭터 위치 기준으로 조정
        public void CameraUpdate(int targetX, int targetY)
        {
            // 카메라 X 좌표는 캐릭터 X 좌표 중심에서 화면의 절반
            X = targetX - _screenWidth / 2;

            // 카메라 Y 좌표는 캐릭터 Y 좌표 중심에서 화면의 절반
            Y = targetY - _screenHeight / 2;

            // 화면의 최상단보다 위로 이동하지 않도록 제한
            if (Y < 0)
            {
                Y = 0;
            }
        }
    }
}
