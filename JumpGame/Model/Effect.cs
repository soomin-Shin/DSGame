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
        private WMPLib.WindowsMediaPlayer _jumpSoundPlayer= new WMPLib.WindowsMediaPlayer(); // 점프 효과음 전용 플레이어
    
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
                _jumpSoundPlayer.URL = jumpSoundFilePath;
                _jumpSoundPlayer.settings.volume = 30;
                _jumpSoundPlayer.controls.stop(); // URL 설정 후 즉시 정지
                _jumpSoundPlayer.settings.setMode("loop", false); // 효과음은 반복 재생하지 않도록 설정
            }
        }

        /// <summary>
        /// 점프 효과음 재생 메서드
        /// </summary>
        public void PlayJumpSound()
        {
            if (_jumpSoundPlayer != null)
            {
                // 이미 재생 중이라면 처음부터 다시 재생
                _jumpSoundPlayer.controls.stop();
                _jumpSoundPlayer.controls.play();
            }
        }
    }
}