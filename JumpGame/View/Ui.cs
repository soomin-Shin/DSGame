using JumpGame.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpGame.View
{
    public class Ui
    {
        private GameStats _gameStats;
        private FontFamily _uiFontFamily;

        /// <summary>
        /// Ui 클래스 생성자
        /// </summary>
        /// <param name="gameStats">게임 통계 객체</param>
        /// <param name="uiFontFamily">UI 텍스트에 사용할 폰트 패밀리</param>
        public Ui(GameStats gameStats, FontFamily uiFontFamily)
        {
            _gameStats = gameStats;
            _uiFontFamily = uiFontFamily;
        }

        /// <summary>
        /// 게임 내 UI 요소(점수, 목숨, 시간)를 그리는 메서드
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clientWidth"></param>
        /// <param name="clientHeight"></param>
        public void DrawScoreUI(Graphics g, int clientWidth, int clientHeight)
        {
            // UI 배경
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, clientHeight - 60, clientWidth, 60);

            // 폰트 설정
            Font uiFont = new Font(_uiFontFamily, 20, FontStyle.Bold); // 폰트 크기
            SolidBrush fontBrush = new SolidBrush(Color.White);

            // 점수 표시
            string scoreText = $"Score: {_gameStats.Score}";
            g.DrawString(scoreText, uiFont, fontBrush, 10, clientHeight - 50);

            // 목숨 표시
            string livesText = $"Lives: {_gameStats.Lives}";
            SizeF livesTextSize = g.MeasureString(livesText, uiFont);
            g.DrawString(livesText, uiFont, fontBrush, clientWidth / 2 - livesTextSize.Width / 2, clientHeight - 50);

            // 시간 표시
            string timeText = $"Time: {_gameStats.ElapsedTime.TotalSeconds:F2}초";
            SizeF timeTextSize = g.MeasureString(timeText, uiFont);
            g.DrawString(timeText, uiFont, fontBrush, clientWidth - timeTextSize.Width - 10, clientHeight - 50);
            
            //조작키 설명
            Font controlsFont = new Font(_uiFontFamily, 12, FontStyle.Regular); // 조작키는 좀 더 작은 폰트
            SolidBrush controlsBrush = new SolidBrush(Color.LightGray); // 색상 변경

            string controlsText = "이동: ← → ↑ ↓ | 점프: Space | 일시정지: ESC";
            SizeF controlsTextSize = g.MeasureString(controlsText, controlsFont);

            g.DrawString(controlsText, controlsFont, controlsBrush, (clientWidth / 2) - (controlsTextSize.Width / 2),10); // Y 좌표를 10으로 설정하여 상단에 배치

            uiFont.Dispose();
            fontBrush.Dispose();
            controlsFont.Dispose();
            controlsBrush.Dispose();
        }
    }
}
