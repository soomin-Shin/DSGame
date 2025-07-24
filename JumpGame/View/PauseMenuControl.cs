using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpGame.View
{
    public partial class PauseMenuControl : UserControl
    {
        private AdventureOfKnight _parentJumpGameForm;
        public PauseMenuControl(AdventureOfKnight parentForm)
        {
            InitializeComponent();
            _parentJumpGameForm = parentForm;
        }
        /// <summary>
        /// 게임 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 게임 재시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        /// <summary>
        /// 게임을 완전히 초기 상태로 재시작합니다.
        /// </summary>
        public void RestartGame()
        {
            // JumpStage에서 시작 좌표 가져오기
            int JumpX = _parentJumpGameForm.JumpStage.GetX();
            int JumpY = _parentJumpGameForm.JumpStage.GetY();

            // 게임 상태 초기화
            _parentJumpGameForm.CameraDisplay.CameraReset();
            _parentJumpGameForm.CharacterStatus.CharacterReset(JumpX, JumpY);
            _parentJumpGameForm.GameStats.StatsReset();
            _parentJumpGameForm.GameStats.ElapsedTime = 0; // 경과 시간 리셋

            // 게임 플레이 관련 변수 초기화
            _parentJumpGameForm.LeftPressed = false;
            _parentJumpGameForm.RightPressed = false;
            _parentJumpGameForm.JumpBuffer = 0;
            _parentJumpGameForm.ResumeGame();
        }
    }
}
