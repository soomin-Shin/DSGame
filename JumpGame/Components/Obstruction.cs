using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JumpGame.Properties; 
using JumpGame.Model; 

namespace JumpGame.Model
{
    public class Obstruction
    {
        private List<Projectile> _projectileLines; 
        private Image _defaultFireBallImage; 

        // 고정 시작 X 좌표 및 속도
        private const int _FIRE_START_X = 753;
        private const int _FIRE_SPEED = 5;

        // 생성 간격 (각 라인마다 독립적으로 적용)
        private const int _FIRE_SPAWN_INTERVAL = 150;


        public Obstruction()
        {

            // 이미지 로드
            _defaultFireBallImage = Resources.FireBallLeft;

            _projectileLines = new List<Projectile>();

            _projectileLines.Add(new Projectile(_FIRE_START_X, 417, _FIRE_SPEED, _defaultFireBallImage, ProjectileType.FireBall, -1));
            _projectileLines.Add(new Projectile(_FIRE_START_X, 300, _FIRE_SPEED, _defaultFireBallImage, ProjectileType.FireBall, -1));
            _projectileLines.Add(new Projectile(_FIRE_START_X, 200, _FIRE_SPEED, _defaultFireBallImage, ProjectileType.FireBall, -1));
        }


        /// <summary>
        /// 모든 객체들과 설치류 장애물 객체들의 상태를 업데이트
        /// </summary>
        public void UpdateAllObstacles()
        {
            foreach (Projectile line in _projectileLines)
            {
                line.Update(); // 각 라인의 업데이트 메서드 호출
            }
        }

        /// <summary>
        /// 모든 객체들과 설치류 장애물 객체들을 그립니다.
        /// </summary>
        /// <param name="g">.</param>
        public void DrawAllObstacles(Graphics g)
        {
            foreach (Projectile line in _projectileLines)
            {
                line.FireBallDraw(g); // 각 라인의 그리기 메서드 호출
            }
        }

        public void draw(Graphics g)
        {
            DrawAllObstacles(g); // 모든 설치류 장애물 그리기 호출
        }

        public void ReSet()
        {
            foreach (Projectile line in _projectileLines)
            {
                line.Reset();
            }
        }
    }
}