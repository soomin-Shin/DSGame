using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JumpGame.Stages
{
    public class JumpStage
    {
        // 발판 리스트
        private List<Platform> _platforms;

        // 배경 화면
        private Image _backgroundImage;

        // 캐릭터 시작 위치
        private Point _startPosition;

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

        public Point StartPosition
        {
            get
            {
                return _startPosition;
            }
            set
            {
                _startPosition = value;
            }
        }



        // Stage 생성자
        public JumpStage(List<Platform> platforms, Image bg, Point start)
        {
            // 발판 리스트
            _platforms = platforms;
            // 배경 화면
            _backgroundImage = bg;
            // 시작 위치 지정
            _startPosition = start;                 
        }

        // 스테이지 생성 함수
        public static JumpStage CreateStage()
        {
            // 발판 목록 정의(아래에 각 발판 좌표/종류를 배열에 추가)
            var _platforms = new List<Platform>()
            {
                // (x, y, width, height), PlatformType
                new Platform(new Rectangle(225, 1430, 155, 20), PlatformType.Normal),           
                new Platform(new Rectangle(475, 1300, 155, 20), PlatformType.StepDisappear),    
                new Platform(new Rectangle(55, 1300, 120, 20), PlatformType.Normal),           
                new Platform(new Rectangle(300, 1140, 155, 20), PlatformType.Normal),          
                new Platform(new Rectangle(655, 1140, 120, 20), PlatformType.Normal),           
                new Platform(new Rectangle(120, 980, 155, 20), PlatformType.Normal),           
                new Platform(new Rectangle(480, 980, 150, 20), PlatformType.StepDisappear),    
                new Platform(new Rectangle(300, 820, 150, 20), PlatformType.StepDisappear),    
                new Platform(new Rectangle(555, 690, 155, 20), PlatformType.Normal),           
                new Platform(new Rectangle(150, 625, 153, 20), PlatformType.Normal),           
                new Platform(new Rectangle(400, 500, 155, 20), PlatformType.Normal),           
                new Platform(new Rectangle(150, 370, 150, 20), PlatformType.StepDisappear),    
                new Platform(new Rectangle(330, 240, 175, 20), PlatformType.Goal)               
            };

            //  배경 화면 로드
            Image _backgroundImage = Image.FromFile("Assets/Image/Stage1Map.png");

            // 플레이어 시작 위치
            Point _startPosition = new Point(300, 1380);

            return new JumpStage(_platforms, _backgroundImage, _startPosition);
        }
    }
}
