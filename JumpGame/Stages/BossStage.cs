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
        // 발판 리스트
        private List<Platform> _platforms;

        // 배경 화면
        private Image _backgroundImage;

        // 적 클래스
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

        // 발판 리스트
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

        // 배경이미지
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

        // 스테이지 생성 함수
        public static void CreateStage(AdventureOfKnight game)
        {
            // 발판 목록 정의
            var platforms = new List<Platform>();

            // 발판 x 좌표 배열
            int[] platformXs = { 200, 314, 500, 314, 500, 0, 590 };
            // 발판 y 좌표 배열
            int[] platformYs = { 525, 325, 325, 425, 425, 667, 601 };
            // 발판 너비 배열
            int[] platformWidths = { 160, 160, 160, 160, 160, 1536, 64 };
            // 발판 높이 배열
            int[] platformHeights = { 45, 45, 45, 45, 45, 166, 61 };
            // 발판 타입 배열
            PlatformType[] platformTypes = { PlatformType.Normal, PlatformType.Normal, PlatformType.StepDisappear, PlatformType.Normal, PlatformType.StepDisappear, PlatformType.Transparent, PlatformType.Transparent };

            // 발판 배열에 추가
            for (int i = 0; i < platformXs.Length; i++)
            {
                Rectangle area = new Rectangle(platformXs[i], platformYs[i], platformWidths[i], platformHeights[i]);
                platforms.Add(new Platform(area, platformTypes[i]));
            }

            // 게임 화면 너비
            game.Width = 1536; 
            // 게임 화면 높이
            game.Height = 833;
            // 발판 생성
            game.Platforms = platforms;
            // 게임 배경화면
            game.BackgroundImage = Image.FromFile("Assets/Image/BossStage.png");
            // 플레이어 시작지점
            game.CharacterStatus = new CharacterStatus(300, 500);
            // 카메라 초기화
            game.CameraDisplay = new CameraDisplay(game.ClientSize.Width, game.ClientSize.Height, game.BackgroundImage.Height);

            // 보스 목록 정의 ( 보스 기본그림, 보스 공격 그림, 보스 사망 그림, 보스 화염구 )
            Image boss = Image.FromFile("Assets/Image/Boss.png");
            Image bossAttack = Image.FromFile("Assets/Image/BossAttack.png");
            Image bossDead = Image.FromFile("Assets/Image/BossDead.png");
            Image fireball = Image.FromFile("Assets/Image/BossFireBall.png");

            // 보스 스폰 지점
            Point bossSpawn = new Point(864, 158);
            // 보스 동작 조작
            game.EnemyController = new EnemyController(boss, bossAttack, fireball, bossDead, bossSpawn, game.CharacterStatus);

        }

    }
}
