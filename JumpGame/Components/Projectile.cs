using System;
using System.Drawing;

namespace JumpGame
{
    // 발사체의 종류를 구분합니다.
    public enum ProjectileType
    {
        FireBall,
        SwordEnergy
    }

    // 개별 불꽃 이미지를 관리하는 클래스
    public class Projectile
    {
        private Image _projectileImage;
        private Point _position;
        private int _speed;
        private bool _isActive;
        private int _initialX; // 초기 X 위치를 저장할 변수 추가
        private int _direction; // 왼쪽: -1, 오른쪽: 1
        private ProjectileType _type;
        
        // 검기 사거리
        private int _distanceTraveled = 0;
        private int _maxDistance = 300;

        // 불꽃의 현재 위치를 가져오거나 설정
        public Point Position
        {
            get { return _position; }
            set { _position = value; }
        }

        // 불꽃의 활성화 상태를 가져오거나 설정
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        // 발사체 타입
        public ProjectileType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        // 히트박스 영역 반환
        public Rectangle GetHitBox()
        {
            return new Rectangle(_position.X, _position.Y, 30, 30);
        }

        /// <summary>
        /// Projectile 클래스의 새 인스턴스를 초기화
        /// </summary>
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
                {
                    this._projectileImage = Image.FromFile("Assets/Image/SwordEnergyLeft.png");
                }
                else
                {
                    this._projectileImage = Image.FromFile("Assets/Image/SwordEnergy.png");
                }
            }
            else // FireBall 타입
            {
                this._projectileImage = image; // FireBall 그대로 사용
            }
        }

        /// <summary>
        /// 위치를 업데이트
        /// </summary>
        /// <returns>불꽃이 화면을 벗어나 비활성화되었는지 여부를 반환</returns>
        public bool Update(ProjectileType type)
        {
            if(type == ProjectileType.FireBall)
            {
                _position.X += _speed * _direction;
                return _isActive;
            }
            else if(type == ProjectileType.SwordEnergy)
            {
                _position.X += _speed * _direction;
                _distanceTraveled += Math.Abs(_speed);

                if (_distanceTraveled >= _maxDistance)
                {
                    _isActive = false;
                }

                return _isActive;
            }
            else
            {
                _position.X += _speed * _direction;
                return _isActive;
            }
        }

        /// <summary>
        /// 불꽃의 위치를 업데이트
        /// </summary>
        /// <param name="screenWidth">현재 게임 화면의 너비(JumpStage 833, BossStage 1536)</param>
        public bool Update(int screenWidth)
        {
            if (_isActive)
            {
                _position.X += _direction * _speed;

                // 불꽃이 화면 밖으로 나가면 초기 X좌표에서 다시 시작
                if (_direction == -1) // 왼쪽으로 이동하는 불꽃
                {
                    // 불꽃이 화면 왼쪽 끝을 완전히 벗어나면
                    if (_position.X < -_projectileImage.Width)
                    {
                        // 초기 X좌표에서 다시 시작
                        _position.X = _initialX;
                    }
                }
                else // _direction == 1 (오른쪽으로 이동하는 불꽃)
                {
                    // 불꽃이 화면 오른쪽 끝을 완전히 벗어나면
                    if (_position.X > screenWidth)
                    {
                        // 초기 X좌표에서 다시 시작
                        _position.X = _initialX;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 객체에 불꽃 이미지를 그림
        /// </summary>
        /// <param name="g"></param>
        public void FireBallDraw(Graphics g, int cameraX, int cameraY)
        {
            if (_isActive && _type == ProjectileType.FireBall && _projectileImage != null)
            {
                g.DrawImage(_projectileImage, _position.X - cameraX, _position.Y - cameraY);
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