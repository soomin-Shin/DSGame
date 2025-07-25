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
        private List<Projectile> _projectileLines; // 불꽃 추가 리스트
        private Image _fireBallImage; // 이미지

        private const int FIRE_START_X_RIGHT = 95; //X좌표 오른쪽 오ㅟ치
        private const int FIRE_START_X_LEFT = 739; // X좌표 왼쪽 위치
        private const int _FIRE_SPEED = 5; // 스피드

        public Obstruction()
        {
            _projectileLines = new List<Projectile>();

            int[] yCoordinates = { 459, 634, 783, 924, 1254, 1392 };
            int[] startXCoordinates = { FIRE_START_X_RIGHT, FIRE_START_X_LEFT, FIRE_START_X_RIGHT, FIRE_START_X_LEFT, FIRE_START_X_RIGHT, FIRE_START_X_LEFT };
            Image[] projectileImages = { Resources.FireBallRight, Resources.FireBallLeft, Resources.FireBallRight, Resources.FireBallLeft, Resources.FireBallRight, Resources.FireBallLeft };
            int[] directions = { 1, -1, 1, -1, 1, -1 };

            for (int i = 0; i < yCoordinates.Length; i++)
            {
                _projectileLines.Add(new Projectile(startXCoordinates[i], yCoordinates[i], _FIRE_SPEED, projectileImages[i], ProjectileType.FireBall, directions[i]));
            }
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