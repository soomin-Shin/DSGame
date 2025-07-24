using System;
using System.Collections.Generic;
using System.Drawing;

namespace JumpGame
{
    public class EnemyController
    {
        // 보스 객체
        private Boss _boss;
        // 화염구 이미지
        private Image _fireBallImage;
        // 보스의 발사체 리스트
        private List<Projectile> _projectiles = new List<Projectile>();
        // 게임 클리어 여부 ( 보스 사망 )
        private bool _gameCleared = false;

        private ProjectileType _type = ProjectileType.FireBall;
        // 발사체 타입
        public ProjectileType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        // 게임 클리어 외부 접근
        public bool GameCleared
        {
            get
            {
                return _gameCleared;
            }
            set
            {
                _gameCleared = value;
            }
        }
        // 적 생성자
        public EnemyController(Image boss, Image bossAttack, Image fireballImage, Image bossDeadImage, Point bossSpawn, CharacterStatus player)
        {
            _fireBallImage = fireballImage;
            _boss = new Boss(boss, bossAttack, fireballImage, bossDeadImage, bossSpawn.X, bossSpawn.Y, player);
        }

        // 보스 애니메이션 및 발사체, 피격 판정
        public void EnemyUpdate(List<Projectile> playerProjectiles)
        {
            // 보스가 살아있으면
            if (_boss != null && !_boss.IsDead)
            {
                _boss.BossUpdate(_projectiles, _fireBallImage);

                // 플레이어 검기 불러와서 충돌 판정
                foreach (var sword in playerProjectiles)
                {
                    if (sword.IsActive && sword.Type == ProjectileType.SwordEnergy && _boss.GetHitBox().IntersectsWith(sword.GetHitBox()))
                    {
                        sword.IsActive = false;
                        // 맞을 때마다 20 감소
                        _boss.BossTakeDamage(20); 
                        if (_boss.IsDead)
                        {
                            _gameCleared = true;
                        }
                    }
                }
            }

            // 보스 발사체 리스트 갱신
            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (!_projectiles[i].Update(Type))
                {
                    _projectiles.RemoveAt(i);
                }
            }
        }

        // 보스 및 발사체 그리기
        public void EnemyDraw(Graphics g, int cameraX, int cameraY)
        {
            if (_boss != null)
            {
                _boss.BossDraw(g, cameraX, cameraY);
            }

            foreach (var proj in _projectiles)
            {
                proj.FireBallDraw(g, cameraX, cameraY);
            }
        }

        // 보스 클래스
        public class Boss
        {
            // 보스 이미지
            private Image _baseImage;
            // 보스 공격 이미지
            private Image _attackImage;
            // 보스 사망 이미지
            private Image _deadImage;
            // 보스 공격 방향
            private Point _position;
            // 보스 키
            private int _width = 481;
            // 보스 몸
            private int _height = 442;
            // 보스 체력
            private int _health = 100;
            // 보스 공격 쿨타임
            private int _cooldown = 0;
            private int _cooldownMax = 60;
            // 타겟 캐릭터
            private CharacterStatus _target;
            // 보스 사망 여부
            private bool _isDead = false;

            public bool IsDead
            {
                get
                {
                    return _isDead;
                }
                set
                {
                    _isDead = value;
                }
            }
            // 보스 생성자
            public Boss(Image boss, Image attack, Image fireBall, Image deadImage, int x, int y, CharacterStatus target)
            {
                _baseImage = boss;
                _attackImage = attack;
                _deadImage = deadImage;
                _position = new Point(x, y);
                _target = target;
            }
            // 쿨타임에 맞춰 보스 공격
            public void BossUpdate(List<Projectile> projectiles, Image fireBallImage)
            {
                if (_isDead)
                {
                    return;
                }

                _cooldown++;
                if (_cooldown >= _cooldownMax)
                {
                    BossFire(projectiles, fireBallImage);
                    _cooldown = 0;
                }
            }

            //보스 공격
            private void BossFire(List<Projectile> projectiles, Image fireBallImage)
            {
                if (_target == null)
                {
                    return;
                }
                int speed = 5;
                int startX = _position.X + 30;
                int startY = _position.Y + 130;

                projectiles.Add(new Projectile(startX, startY, speed, fireBallImage, ProjectileType.FireBall, -1));
            }

            // 보스 움직임 및 체력바 
            public void BossDraw(Graphics g, int cameraX, int cameraY)
            {
                // 카메라 위치에 맞게 동기화
                int drawX = _position.X - cameraX;
                int drawY = _position.Y - cameraY;

                if (_isDead)
                {
                    g.DrawImage(_deadImage, drawX, drawY, _width, _height);
                    return;
                }

                // 쿨다운이 절반 미만이면 기본 이미지. 쿨다운 절반 이상이면 입 벌리는 공격자세
                Image current = _cooldown < _cooldownMax / 2 ? _baseImage : _attackImage;
                g.DrawImage(current, drawX, drawY, _width, _height);

                // 체력바
                g.FillRectangle(Brushes.Red, drawX, drawY - 10, _width * _health / 100, 5);
                g.DrawRectangle(Pens.Black, drawX, drawY - 10, _width, 5);
            }

            // 데미지 처리
            public void BossTakeDamage(int dmg)
            {
                _health -= dmg;
                if (_health <= 0)
                {
                    _health = 0;
                    _isDead = true;
                }
            }

            public Rectangle GetHitBox()
            {
                return new Rectangle(_position.X, _position.Y, _width, _height);
            }
        }
    }
}
