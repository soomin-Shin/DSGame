using System;
using System.Drawing;

namespace JumpGame
{
    /// <summary>
    /// 발사체의 종류를 구분합니다.
    /// </summary>
    public enum ProjectileType
    {
        FireBall,
        SwordEnergy
    }
    /// <summary>
    /// 개별 불꽃 이미지를 관리하는 클래스
    /// </summary>
    public class Projectile
    {
        private Image _projectileImage;
        private Point _position;
        private int _speed;
        private bool _isActive;
        private int _initialX; // 초기 X 위치를 저장할 변수 추가
        private int _direction; // 왼쪽: -1, 오른쪽: 1
        private ProjectileType _type;

        /// <summary>
        /// 불꽃의 현재 위치를 가져오거나 설정
        /// </summary>
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// 불꽃의 활성화 상태를 가져오거나 설정
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        /// <summary>
        /// 발사체 타입
        /// </summary>
        public ProjectileType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 히트박스 영역 반환
        /// </summary>
        public Rectangle GetHitBox()
        {
            return new Rectangle(_position.X, _position.Y, 30, 30);
        }

        /// <summary>
        /// FireBall 클래스의 새 인스턴스를 초기화
        /// </summary>
        /// <param name="startX">불꽃의 시작 X 좌표입니다.</param>
        /// <param name="startY">불꽃의 시작 Y 좌표입니다.</param>
        /// <param name="fireSpeed">불꽃의 이동 속도입니다.</param>
        /// <param name="image">사용할 불꽃 이미지입니다.</param>
        public Projectile(int startX, int startY, int fireSpeed, Image image, ProjectileType type, int direction)
        {
            this._position = new Point(startX, startY);
            this._initialX = startX; // 초기 X 저장
            this._speed = fireSpeed;
            this._projectileImage = image;
            this._isActive = true;
            this._type = type;
            this._direction = direction;

            if (type == ProjectileType.SwordEnergy)
            {
                // 방향에 따라 이미지 다르게 설정
                if (direction == -1)
                    this._projectileImage = Image.FromFile("Assets/Image/SwordEnergyLeft.png");
                else
                    this._projectileImage = Image.FromFile("Assets/Image/SwordEnergy.png");
            }
            else
            {
                this._projectileImage = image; // FireBall 그대로 사용
            }
        }

        /// <summary>
        /// 불꽃의 위치를 업데이트합니다.
        /// </summary>
        /// <returns>불꽃이 화면을 벗어나 비활성화되었는지 여부를 반환</returns>
        public bool Update()
        {
            if (_isActive)
            {
                _position.X += _direction * _speed; //양쪽 방향

                // 불꽃이 화면 왼쪽 끝(X=0)을 벗어나면 비활성화
                if (_position.X + _projectileImage.Width < 0 || _position.X > 1000)
                {
                    _isActive = false;
                }
            }
            return _isActive;
        }

        /// <summary>
        /// 객체에 불꽃 이미지를 그림
        /// </summary>
        /// <param name="g"></param>
        public void FireBallDraw(Graphics g)
        {
            if (_isActive && _projectileImage != null)
            {
                g.DrawImage(_projectileImage, _position);
            }
        }

        /// <summary>
        /// SwordEnergy용 렌더링 (카메라 적용)
        /// </summary>
        public void SwordEnergyDraw(Graphics g, int cameraX, int cameraY)
        {
            if (_isActive && _type == ProjectileType.SwordEnergy && _projectileImage != null)
            {
                g.DrawImage(_projectileImage, _position.X - cameraX, _position.Y - cameraY, 30, 30);
            }
        }

        internal void Reset()
        {
            this._position = new Point(_initialX, this._position.Y); // 저장된 초기 X로 리셋
            this._isActive = true;
        }
    }
}