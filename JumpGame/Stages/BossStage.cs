using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JumpGame.Stages
{
    class BossStage
    {
        // JumpStage 캐릭터 X 좌표
        private int _x;

        // JumpStage 캐릭터 Y 좌표
        private int _y;

        // 발판 리스트
        private List<Platform> _platforms;

        // 배경 화면
        private Image _backgroundImage;


        public List<Platform> Platforms
        {
            get
            {
                return _platforms;
            }
            set
            {
                _platforms = value;
            }
        }

        public Image BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }
            set
            {
                _backgroundImage = value;
            }
        }

        // 캐릭터 X 좌표
        public int GetX()
        {
            return _x;
        }

        // 캐릭터 Y 좌표
        public int GetY()
        {
            return _y;
        }
        
        // 스테이지 생성 함수
        public static void CreateStage(AdventureOfKnight now)
        {
            // 발판 목록 정의(아래에 각 발판 좌표/종류를 배열에 추가)
            var _platforms = new List<Platform>()
            {
                // (x, y, width, height), PlatformType
                new Platform(new Rectangle(200, 525, 160, 45), PlatformType.Normal),
                new Platform(new Rectangle(314, 325, 160, 45), PlatformType.Normal),
                new Platform(new Rectangle(500, 325, 160, 45), PlatformType.StepDisappear),
                new Platform(new Rectangle(314, 425, 160, 45), PlatformType.Normal),
                new Platform(new Rectangle(500, 425, 160, 45), PlatformType.StepDisappear),
                new Platform(new Rectangle(0, 667, 1536, 166), PlatformType.Transparent),
                new Platform(new Rectangle(590, 601, 64, 61), PlatformType.Transparent)
            };

            now.Width = 1536; // 각 스테이지
            // 게임 화면 높이
            now.Height = 833; // 각 스테이지
            now.Platforms = _platforms;  // 각 스테이지
            now.BackgroundImage = Image.FromFile("Assets/Image/BossStage.png");  // 각 스테이지
            now.CharacterStatus = new CharacterStatus(300, 500);  // 각 스테이지
            now.CameraDisplay = new CameraDisplay(now.ClientSize.Width, now.ClientSize.Height, now.BackgroundImage.Height);  // 각 스테이지

            //  배경 화면 로드
           
        }

        // 보스 스테이지 캐릭터 초기화
        public void BossStageReset(CharacterStatus character)
        {
            character.CharacterReset(_x, _y);
        }
    }
}
