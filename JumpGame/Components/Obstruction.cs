using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JumpGame.Properties; // 네임스페이스를 JumpGame.Properties로 변경
using JumpGame.Model; // FireBall 클래스를 사용하기 위해 추가 (Obstruction과 같은 네임스페이스라면 필요 없을 수 있음)

namespace JumpGame.Model // 네임스페이스를 JumpGame.Model로 변경
{
    public class Obstruction
    {
        // ... (기존 Obstruction 클래스의 다른 멤버 변수들: 설치류 장애물 등) ...

        // ===== 불꽃 라인 관리용 변수 =====
        private List<FireBall> fireLines; // 여러 불꽃 라인을 담을 리스트
        private Image defaultFireBallImage; // 모든 불꽃이 사용할 기본 이미지 (이름 변경)

        // 불꽃의 고정 시작 X 좌표 및 속도
        private const int FIRE_START_X = 753;
        private const int FIRE_SPEED = 5;

        // 불꽃 생성 간격 (각 라인마다 독립적으로 적용)
        private const int FIRE_SPAWN_INTERVAL = 150;


        public Obstruction()
        {
            // ... (기존 Obstruction 초기화 로직 및 설치류 장애물 초기화 로직) ...

            // 불꽃 이미지 로드 (한 번만 로드하여 모든 불꽃 라인에 공유)
            defaultFireBallImage = Resources.FireBallLeft; // 실제 리소스 이름으로 변경!

            // ===== 불꽃 라인 목록 초기화 =====
            fireLines = new List<FireBall>();

            // 수정된 초기화: (startX, startY, fireSpeed, image)
            // startX는 FIRE_START_X (753)여야 하고, startY는 원하는 Y 좌표여야 합니다.
            fireLines.Add(new FireBall(FIRE_START_X, 417, FIRE_SPEED, defaultFireBallImage));
            fireLines.Add(new FireBall(FIRE_START_X, 300, FIRE_SPEED, defaultFireBallImage));
            fireLines.Add(new FireBall(FIRE_START_X, 200, FIRE_SPEED, defaultFireBallImage));
            // 필요한 만큼 다른 고정 Y 좌표에 불꽃 라인 추가 가능
        }


        /// <summary>
        /// 모든 불꽃 객체들과 설치류 장애물 객체들의 상태를 업데이트합니다.
        /// </summary>
        public void UpdateAllObstacles()
        {
            // --- 불꽃 라인 업데이트 로직 ---
            foreach (FireBall line in fireLines)
            {
                line.Update(); // 각 불꽃 라인의 업데이트 메서드 호출
            }

            // ... (설치류 장애물 업데이트 로직 - 기존 코드 유지) ...
        }

        /// <summary>
        /// 모든 불꽃 객체들과 설치류 장애물 객체들을 그립니다.
        /// </summary>
        /// <param name="g">그리기를 위한 Graphics 객체입니다.</param>
        public void DrawAllObstacles(Graphics g)
        {
            // --- 불꽃 라인 그리기 ---
            foreach (FireBall line in fireLines)
            {
                line.FireBallDraw(g); // 각 불꽃 라인의 그리기 메서드 호출
            }

            // ... (설치류 장애물 그리기 - 기존 코드 유지) ...
        }

        // 기존 draw(PaintEventArgs e) 메서드를 변경하여 DrawAllObstacles를 호출하도록
        public void draw(Graphics g)
        {
            // ... (Obstruction의 다른 그리기 로직: 예를 들어 배경의 정적 장애물) ...

            DrawAllObstacles(g); // 모든 불꽃과 설치류 장애물 그리기 호출
        }

        public void reSet() // 게임 재시작 시 모든 장애물 초기화
        {
            // ... (기존 Obstruction 리셋 로직 및 설치류 장애물 리셋 로직) ...

            // ===== 불꽃 라인 초기화 =====
            foreach (FireBall line in fireLines)
            {
                line.Reset(); // 각 불꽃 라인 리셋
            }
        }
    }
}