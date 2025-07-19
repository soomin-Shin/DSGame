using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; // WMPLib.WindowsMediaPlayer를 위해 필요
using System.Media; // SoundPlayer를 위해 추가

namespace JumpGame.Model
{
    public class Effect
    {
        private WMPLib.WindowsMediaPlayer _bgmPlayer = new WMPLib.WindowsMediaPlayer(); // 배경음악 전용 플레이어
        private SoundPlayer _jumpSoundPlayer; // 점프 효과음 전용 플레이어

        public Effect() // Effect 클래스 생성자 추가 (여기서 초기화)
        {
            BGMSetting(); // 배경음악 초기화 및 재생
            JumpSoundSetting(); // 점프 효과음 초기화
        }

        /// <summary>
        /// 배경음악 설정 및 재생
        /// </summary>
        private void BGMSetting()
        {
                string bgmFilePath = $"{Application.StartupPath}//Assets//BGM//bgm.mp3";
            if (System.IO.File.Exists(bgmFilePath))
                {
                    _bgmPlayer.URL = bgmFilePath;
                    _bgmPlayer.settings.setMode("loop", true);
                    _bgmPlayer.settings.volume = 30; // 배경음악 볼륨은 보통 낮게 설정
                    _bgmPlayer.controls.play();
            }
        }

        /// <summary>
        /// 점프 효과음 설정 (초기 로드)
        /// </summary>
        private void JumpSoundSetting()
        {
            // WAV 파일 경로 설정 (BGM 폴더에 같이 넣었다고 가정)
            string jumpSoundFilePath = $"{Application.StartupPath}//Assets//BGM//jumpSound.mp3";
            if (System.IO.File.Exists(jumpSoundFilePath))
            {
                    _jumpSoundPlayer = new SoundPlayer(jumpSoundFilePath);
                    _jumpSoundPlayer.Load(); // 미리 로드하여 재생 시 지연을 줄임
            }
        }

        /// <summary>
        /// 점프 효과음 재생 메서드
        /// </summary>
        public void PlayJumpSound()
        {
            if (_jumpSoundPlayer != null)
            {
                _jumpSoundPlayer.Play();
            }
        }
    }
}