using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using JumpGame.Components;

namespace JumpGame.Stages
{
    // 아이템 타입
    public enum ItemType
    {
        Ruby,
        Sapphire,
        Emerald
    }


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

        // 아이템
        private Item _item; 

        // 폼 너비
        private int _formWidth;

        // 폼 높이
        private int _formHeight;


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

        // 캐릭터 X 좌표
        public int FormWidth()
        {
            return _formWidth;
        }

        // 캐릭터 Y 좌표
        public int FormHeight()
        {
            return _formHeight;
        }

        public JumpStage(List<Platform> platforms, Image bg, int startX, int startY, int formWidth, int formHeight)
        {
            // 발판 리스트
            _platforms = platforms;
            // 배경 화면
            _backgroundImage = bg;
            _x = startX;
            _y = startY;
            // 폼 크기
            _formWidth = formWidth;
            _formHeight = formHeight;

            _item = new Item();
            // 아이템 5개 생성
            _item.GenerateItems(_platforms, 5); 
        }
        // 생성자 함수
        public JumpStage()
        {
        }

        // 스테이지 생성 함수
        public JumpStage CreateStage()
        {
            // 발판 목록 정의
            var platforms = new List<Platform>();

            // 발판 x 좌표 배열
            int[] platformXs = { 220, 475, 55, 295, 650, 120, 475, 295, 550, 145, 395, 150, 320 };
            // 발판 y 좌표 배열
            int[] platformYs = { 1420, 1290, 1290, 1130, 1130, 970, 970, 810, 680, 615, 485, 360, 228 };
            // 발판 너비 배열
            int[] platformWidths = { 165, 160, 125, 165, 130, 160, 160, 160, 160, 160, 165, 155, 190 };
            // 발판 높이 배열
            int[] platformHeights = { 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45, 45 };
            // 발판 타입 배열
            PlatformType[] platformTypes = { PlatformType.Normal, PlatformType.StepDisappear, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.StepDisappear, PlatformType.StepDisappear, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.StepDisappear, PlatformType.Goal };

            // 발판 배열에 추가
            for (int i = 0; i < platformXs.Length; i++)
            {
                Rectangle area = new Rectangle(platformXs[i], platformYs[i], platformWidths[i], platformHeights[i]);
                platforms.Add(new Platform(area, platformTypes[i]));
            }

            //  배경 화면 로드
            Image bg = Image.FromFile("Assets/Image/Stage1Map.png");

            // 플레이어 시작 위치
            int startX = 300;
            int startY = 1360;

            // 폼 크기
            int formWidth = 833;
            int formHeight = 600;

            return new JumpStage(platforms, bg, startX, startY, formWidth, formHeight);
        }

        // 점프 스테이지 캐릭터 초기화 
        public void CheckResetCondition(CharacterStatus character)
        {
            // 캐릭터가 초기 위치 보다 100만큼 아래로 떨어졌는지 확인
            if (character.GetY() > _y + 100)
            {
                character.CharacterReset(_x, _y);
            }
        }
    }
}
