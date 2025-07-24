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
        private bool _gameCleared = false;

        private ProjectileType _type = ProjectileType.FireBall;
        // 발사체 타입
        public ProjectileType Type
        {
            get { return _type; }
            set { _type = value; }
        }

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
        public EnemyController(Image bossIdle, Image bossAttack, Image fireballImage, Image bossDeadImage, Point bossSpawn, CharacterStatus player)
        {
            _fireBallImage = fireballImage;
            _boss = new Boss(bossIdle, bossAttack, fireballImage, bossDeadImage, bossSpawn.X, bossSpawn.Y, player);
        }

        public void Update(List<Projectile> playerProjectiles)
        {
            if (_boss != null && !_boss.IsDead)
            {
                _boss.Update(_projectiles, _fireBallImage);

                foreach (var sword in playerProjectiles)
                {
                    if (sword.IsActive && sword.Type == ProjectileType.SwordEnergy && _boss.GetHitBox().IntersectsWith(sword.GetHitBox()))
                    {
                        sword.IsActive = false;
                        _boss.TakeDamage(20); // 맞을 때마다 20 감소
                        if (_boss.IsDead)
                        {
                            _gameCleared = true;
                        }
                    }
                }
            }

            for (int i = _projectiles.Count - 1; i >= 0; i--)
            {
                if (!_projectiles[i].Update(Type))
                    _projectiles.RemoveAt(i);
            }
        }

        public void Draw(Graphics g, int cameraX, int cameraY)
        {
            _boss?.Draw(g, cameraX, cameraY);

            foreach (var proj in _projectiles)
            {
                proj.FireBallDraw(g, cameraX, cameraY);
            }
        }

        public class Boss
        {
            private Image _idleImage;
            private Image _attackImage;
            private Image _deadImage;
            private Point _position;
            private int _width = 481;
            private int _height = 442;
            private int _health = 100;
            private int _cooldown = 0;
            private int _cooldownMax = 60;
            private CharacterStatus _target;
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
            public Boss(Image idle, Image attack, Image fireBall, Image deadImage, int x, int y, CharacterStatus target)
            {
                _idleImage = idle;
                _attackImage = attack;
                _deadImage = deadImage;
                _position = new Point(x, y);
                _target = target;
            }

            public void Update(List<Projectile> projectiles, Image fireBallImage)
            {
                if (_isDead) return;

                _cooldown++;
                if (_cooldown >= _cooldownMax)
                {
                    Fire(projectiles, fireBallImage);
                    _cooldown = 0;
                }
            }

            private void Fire(List<Projectile> projectiles, Image fireBallImage)
            {
                if (_target == null) return;

                float tx = _target.GetX() - _position.X;
                float ty = _target.GetY() - _position.Y;
                float len = (float)Math.Sqrt(tx * tx + ty * ty);
                float dx = tx / len;
                int direction = dx >= 0 ? 1 : -1;

                int speed = 5;
                int startX = _position.X + 30;
                int startY = _position.Y + 130;

                projectiles.Add(new Projectile(startX, startY, speed, fireBallImage, ProjectileType.FireBall, direction));
            }

            public void Draw(Graphics g, int cameraX, int cameraY)
            {
                int drawX = _position.X - cameraX;
                int drawY = _position.Y - cameraY;

                if (_isDead)
                {
                    g.DrawImage(_deadImage, drawX, drawY, _width, _height);
                    return;
                }

                Image current = _cooldown < _cooldownMax / 2 ? _idleImage : _attackImage;
                g.DrawImage(current, drawX, drawY, _width, _height);

                // 체력바
                g.FillRectangle(Brushes.Red, drawX, drawY - 10, _width * _health / 100, 5);
                g.DrawRectangle(Pens.Black, drawX, drawY - 10, _width, 5);
            }

            public void TakeDamage(int dmg)
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
