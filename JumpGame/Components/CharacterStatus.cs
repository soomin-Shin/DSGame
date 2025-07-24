using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using JumpGame.Model;

namespace JumpGame
{
    // 캐릭터 상태
    public enum CharacterState
    {
        MoveLeft,
        MoveRight,
        Jump
    }

    public class CharacterStatus
    {
        // 캐릭터 X 좌표
        private int _x;

        // 캐릭터 Y 좌표
        private int _y;

        // 캐릭터의 너비
        private int _width = 53;

        // 캐릭터의 높이
        private int _height = 53;

        // 캐릭터 기본 상태
        private CharacterState _currentState = CharacterState.MoveRight;

        // 캐릭터 왼쪽 이미지
        private Image _characterLeftImage;
        
        // 캐릭터 오른쪽 이미지
        private Image _characterRightImage;

        // 캐릭터 점프 이미지
        private Image _characterJumpImage;

        // 수직 속도
        private float _velocityY = 0;

        // 중력 가속도
        private float _gravity = 0.4f;

        // 캐릭터 착지 여부
        private bool _onGround = false;

        // 좌우 이동 속도
        private int _moveSpeed = 5;

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

        private ProjectileType _type = ProjectileType.SwordEnergy;
        // 발사체 타입
        public ProjectileType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private List<Projectile> _swordProjectiles = new List<Projectile>();
        private int _swordCooldown = 0;
        private int _swordCooldownMax = 40;
        private Image _swordImage = Image.FromFile("Assets/Image/SwordEnergy.png");

        public List<Projectile> SwordProjectiles
        {
            get { return _swordProjectiles; }
        }

        public CharacterStatus(int startX, int startY)
        {
            _x = startX;
            _y = startY;
            _characterLeftImage = Image.FromFile("Assets/Image/PlayerLeftSword.png");
            _characterRightImage = Image.FromFile("Assets/Image/PlayerRightSword.png");
            _characterJumpImage = Image.FromFile("Assets/Image/PlayerJumpRightSword.png");

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

            // 벽 바깥으로 빠져나가지 못하게 제한
            if (_x < 55)
            {
                _x = 55;
            }

            // 상태 변경
            _currentState = CharacterState.MoveLeft;
        }

        // 오른쪽으로 이동
        public void MoveRight()
        {
            _x = _x + _moveSpeed;

            // 벽 바깥으로 빠져나가지 못하게 제한
            if (_x > 780 - _width)
            {
                _x = 780 - _width;
            }

            // 상태 변경
            _currentState = CharacterState.MoveRight;
        }

        // 점프
        public void Jump()
        {
            if (_onGround == true)
            {
                // 위쪽으로 속도 부여
                _velocityY = -12;
                _onGround = false;
            }

            // 상태 변경
            _currentState = CharacterState.Jump;
        }

        public void ShootSword()
        {
            if (_swordCooldown == 0)
            {
                int direction = _currentState == CharacterState.MoveLeft ? -1 : 1;
                int speed = 7;
                int startX = _x + (_width / 2);
                int startY = _y + 10;

                _swordProjectiles.Add(new Projectile(startX, startY, speed, _swordImage, ProjectileType.SwordEnergy, direction));
                _swordCooldown = _swordCooldownMax;
            }
        }

        public void UpdateProjectiles()
        {
            if (_swordCooldown > 0) _swordCooldown--;

            for (int i = _swordProjectiles.Count - 1; i >= 0; i--)
            {
                if (!_swordProjectiles[i].Update(Type))
                    _swordProjectiles.RemoveAt(i);
            }
        }

        public void ShootProjectiles(Graphics g, int cameraX, int cameraY)
        {
            foreach (var p in _swordProjectiles)
            {
                p.SwordEnergyDraw(g, cameraX, cameraY);
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
        }

        // 캐릭터 그림
        public void Draw(Graphics g, int offsetX, int offsetY)
        {
            // 기본은 오른쪽 이동 이미지
            Image characterImage = _characterRightImage;

            // 캐릭터가 왼쪽으로 이동하는 경우
            if (_currentState == CharacterState.MoveLeft)
            {
                characterImage = _characterLeftImage;
            }
            // 캐릭터가 오른쪽으로 이동하는 경우
            else if (_currentState == CharacterState.MoveRight)
            {
                characterImage = _characterRightImage;
            }
            // 캐릭터 초기 이미지
            if (characterImage != null)
            {
                g.DrawImage(characterImage, _x - offsetX, _y - offsetY, _width, _height);
            }
            // 캐릭터 이미지를 불러오지 못한 경우 사각형으로 대체
            else
            {
                g.FillRectangle(Brushes.Green, _x - offsetX, _y - offsetY, _width, _height);
            }
        }

        // 캐릭터 상태를 초기화
        public void CharacterReset(int x, int y)
        {
            _x = x;
            _y = y;
            _velocityY = 0;
            _onGround = true;
        }
    }
}
