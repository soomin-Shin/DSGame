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

        private EnemyController _enemyController;

        // 적 클래스
        private EnemyController Enemy
        {
            get
            {
                return _enemyController;
            }
            set
            {
                _enemyController = value;
            }
        }

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
        public static void CreateStage(AdventureOfKnight game)
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

            game.Width = 1536; // 각 스테이지
            // 게임 화면 높이
            game.Height = 833; // 각 스테이지
            game.Platforms = _platforms;  // 각 스테이지
            game.BackgroundImage = Image.FromFile("Assets/Image/BossStage.png");  // 각 스테이지
            game.CharacterStatus = new CharacterStatus(300, 500);  // 각 스테이지
            game.CameraDisplay = new CameraDisplay(game.ClientSize.Width, game.ClientSize.Height, game.BackgroundImage.Height);  // 각 스테이지

            // 보스 목록 정의
            Image boss = Image.FromFile("Assets/Image/Boss.png");
            Image bossAttack = Image.FromFile("Assets/Image/BossAttack.png");
            Image bossDead = Image.FromFile("Assets/Image/BossDead.png");
            Image fireball = Image.FromFile("Assets/Image/BossFireBall.png");

            Point bossSpawn = new Point(864, 158); // 원하는 위치로 설정


            game.EnemyController = new EnemyController(boss, bossAttack, fireball, bossDead, bossSpawn, game.CharacterStatus);


            //  배경 화면 로드

        }

        // 보스 스테이지 캐릭터 초기화
        public void BossStageReset(CharacterStatus character)
        {
            character.CharacterReset(_x, _y);
        }
    }
}
