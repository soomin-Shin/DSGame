using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using JumpGame.Model;

namespace JumpGame.Stages
{
    public class JumpStage
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

        // Stage 생성자
        public JumpStage(AdventureOfKnight test)
        {
            test.StartTime = DateTime.Now;
        }

        public JumpStage(List<Platform> platforms, Image bg, int startX, int startY)
        {
            // 발판 리스트
            _platforms = platforms;
            // 배경 화면
            _backgroundImage = bg;
            _x = startX;
            _y = startY;
        }

        // 스테이지 생성 함수
        public static JumpStage CreateStage()
        {
            // 발판 목록 정의(아래에 각 발판 좌표/종류를 배열에 추가)
            var _platforms = new List<Platform>()
            {
                // (x, y, width, height), PlatformType
                new Platform(new Rectangle(220, 1420, 165, 45), PlatformType.Normal),           
                new Platform(new Rectangle(475, 1290, 160, 45), PlatformType.StepDisappear),    
                new Platform(new Rectangle(55, 1290, 125, 45), PlatformType.Normal),           
                new Platform(new Rectangle(295, 1130, 165, 45), PlatformType.Normal),          
                new Platform(new Rectangle(650, 1130, 130, 45), PlatformType.Normal),           
                new Platform(new Rectangle(120, 970, 160, 45), PlatformType.Normal),           
                new Platform(new Rectangle(475, 970, 160, 45), PlatformType.StepDisappear),    
                new Platform(new Rectangle(295, 810, 160, 45), PlatformType.StepDisappear),    
                new Platform(new Rectangle(550, 680, 160, 45), PlatformType.Normal),           
                new Platform(new Rectangle(145, 615, 160, 45), PlatformType.Normal),           
                new Platform(new Rectangle(395, 485, 165, 45), PlatformType.Normal),           
                new Platform(new Rectangle(150, 360, 155, 45), PlatformType.StepDisappear),    
                new Platform(new Rectangle(320, 228, 190, 45), PlatformType.Goal)               
            };

            //  배경 화면 로드
            Image _backgroundImage = Image.FromFile("Assets/Image/Stage1Map.png");

            // 플레이어 시작 위치
            int startX = 300;
            int startY = 1360;

            return new JumpStage(_platforms, _backgroundImage, startX, startY);
        }

        // 점프 스테이지 캐릭터 초기화
        public void JumpStageReset(CharacterStatus character)
        {
            character.CharacterReset(_x, _y);
        }
    }
}
