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
        private Image _fireBallImage;

        private const int FIRE_START_X_RIGHT = 95;
        private const int FIRE_START_X_LEFT = 739;
        private const int _FIRE_SPEED = 5;

        public Obstruction()
        {
            _fireBallImage = Resources.FireBallLeft; // 리소스 경로 확인

            _projectileLines = new List<Projectile>();

            // Projectile 생성자 호출 시 타입과 방향 인자 추가
            //_projectileLines.Add(new Projectile(FIRE_START_X_RIGHT, 417, _FIRE_SPEED, Resources.FireBallLeft, ProjectileType.FireBall, -1));
            _projectileLines.Add(new Projectile(FIRE_START_X_RIGHT, 459, _FIRE_SPEED, Resources.FireBallRight, ProjectileType.FireBall, 1));
            _projectileLines.Add(new Projectile(FIRE_START_X_LEFT, 634, _FIRE_SPEED, Resources.FireBallLeft, ProjectileType.FireBall, -1));
            _projectileLines.Add(new Projectile(FIRE_START_X_RIGHT, 783, _FIRE_SPEED, Resources.FireBallRight, ProjectileType.FireBall, 1));
            _projectileLines.Add(new Projectile(FIRE_START_X_LEFT, 924, _FIRE_SPEED, Resources.FireBallLeft, ProjectileType.FireBall, -1));
            _projectileLines.Add(new Projectile(FIRE_START_X_RIGHT, 1254, _FIRE_SPEED, Resources.FireBallRight, ProjectileType.FireBall, 1));
            _projectileLines.Add(new Projectile(FIRE_START_X_LEFT, 1392, _FIRE_SPEED, Resources.FireBallLeft, ProjectileType.FireBall, -1));
            //_projectileLines.Add(new Projectile(FIRE_START_X_LEFT, 100, _FIRE_SPEED, Resources.FireBallRight, ProjectileType.FireBall, 1));
        }


        /// <summary>
        /// 모든 객체들과 설치류 장애물 객체들의 상태를 업데이트
        /// </summary>
        /// <param name="screenWidth">현재 게임 화면의 너비</param>
        public void UpdateAllObstacles(int screenWidth) // screenWidth 인자 추가
        {
            foreach (Projectile line in _projectileLines)
            {
                line.Update(screenWidth); // 각 Projectile의 Update 메서드에 screenWidth 전달
            }
        }

        /// <summary>
        /// 모든 객체들과 설치류 장애물 객체들을 그립니다.
        /// </summary>
        /// <param name="g">.</param>
        public void DrawAllObstacles(Graphics g, int cameraX, int cameraY)
        {
            foreach (Projectile line in _projectileLines)
            {
                line.FireBallDraw(g, cameraX, cameraY);
            }
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