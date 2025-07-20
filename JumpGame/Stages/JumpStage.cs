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
                // Rectangle(x, y, width, height), PlatformType
                new Platform(new Rectangle(400, 1200, 40, 15), PlatformType.Normal),           // 일반 발판
                new Platform(new Rectangle(330, 1150, 40, 15), PlatformType.StepDisappear),    // 밟으면 사라지는 발판
                new Platform(new Rectangle(260, 1100, 40, 15), PlatformType.Normal),           // 일반 발판
                new Platform(new Rectangle(190, 1050, 40, 15), PlatformType.Normal),           // 일반 발판
                new Platform(new Rectangle(120, 1000, 40, 15), PlatformType.StepDisappear),    // 사라지는 발판
                new Platform(new Rectangle(50, 950, 40, 15), PlatformType.Moving),             // 움직이는 발판
                new Platform(new Rectangle(120, 900, 40, 15), PlatformType.Normal),            // 일반 발판
                new Platform(new Rectangle(200, 850, 40, 15), PlatformType.StepDisappear),     // 사라지는 발판
                new Platform(new Rectangle(280, 800, 40, 15), PlatformType.Normal),            // 일반 발판
                new Platform(new Rectangle(360, 750, 40, 15), PlatformType.StepDisappear),     // 사라지는 발판
                new Platform(new Rectangle(430, 700, 40, 15), PlatformType.Normal),            // 일반 발판
                new Platform(new Rectangle(510, 650, 40, 15), PlatformType.Normal),            // 일반 발판
                new Platform(new Rectangle(580, 600, 40, 15), PlatformType.Moving),            // 움직이는 발판
                new Platform(new Rectangle(500, 550, 40, 15), PlatformType.StepDisappear),     // 사라지는 발판
                new Platform(new Rectangle(420, 500, 40, 15), PlatformType.Normal),            // 일반 발판
                new Platform(new Rectangle(300, 400, 40, 40), PlatformType.Goal)               // 골인(노란색 정사각형)
            };

            //  배경 화면 로드
            Image _backgroundImage = Image.FromFile("Assets/Image/Stage1Map.png");

            // 플레이어 시작 위치
            Point _startPosition = new Point(400, 1500);

            return new JumpStage(_platforms, _backgroundImage, _startPosition);
        }
    }
}
