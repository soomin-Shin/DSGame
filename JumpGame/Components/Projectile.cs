using System;
using System.Drawing;

namespace JumpGame
{
    /// <summary>
    /// 개별 불꽃 이미지를 관리하는 클래스
    /// </summary>
    public class FireBall
    {
        private Image fireImage;
        private Point position;
        private int speed;
        private bool isActive;
        private int initialX; // 초기 X 위치를 저장할 변수 추가
        /// <summary>
        /// 불꽃의 현재 위치를 가져오거나 설정
        /// </summary>
        public Point _Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// 불꽃의 활성화 상태를 가져오거나 설정
        /// </summary>
        public bool _IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// FireBall 클래스의 새 인스턴스를 초기화
        /// </summary>
        /// <param name="startX">불꽃의 시작 X 좌표입니다.</param>
        /// <param name="startY">불꽃의 시작 Y 좌표입니다.</param>
        /// <param name="fireSpeed">불꽃의 이동 속도입니다.</param>
        /// <param name="image">사용할 불꽃 이미지입니다.</param>
        public FireBall(int startX, int startY, int fireSpeed, Image image)
        {
            this.position = new Point(startX, startY);
            this.initialX = startX; // 초기 X 저장
            this.speed = fireSpeed;
            this.fireImage = image;
            this.isActive = true;
        }

        /// <summary>
        /// 불꽃의 위치를 업데이트합니다.
        /// </summary>
        /// <returns>불꽃이 화면을 벗어나 비활성화되었는지 여부를 반환</returns>
        public bool Update()
        {
            if (isActive)
            {
                position.X -= speed; // 왼쪽으로 이동

                // 불꽃이 화면 왼쪽 끝(X=0)을 벗어나면 비활성화
                if (position.X + fireImage.Width < 0)
                {
                    isActive = false;
                }
            }
            return isActive;
        }

        /// <summary>
        /// 객체에 불꽃 이미지를 그림
        /// </summary>
        /// <param name="g">.</param>
        public void FireBallDraw(Graphics g)
        {
            if (isActive && fireImage != null)
            {
                g.DrawImage(fireImage, position);
            }
        }

        internal void Reset()
        {
            this.position = new Point(initialX, this.position.Y); // 저장된 초기 X로 리셋
            this.isActive = true;
        }
    }
}