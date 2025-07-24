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

        // 스테이지 생성 함수
        public static void CreateStage(AdventureOfKnight now)
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


            // 폼 크기
            now.Width = 833;
            // 게임 화면 높이
            now.Height = 600; 
            now.Platforms = platforms;  
            now.BackgroundImage = Image.FromFile("Assets/Image/Stage1Map.png");  
            now.CharacterStatus = new CharacterStatus(300, 1360);  
            now.CameraDisplay = new CameraDisplay(now.ClientSize.Width, now.ClientSize.Height, now.BackgroundImage.Height);  
            // 좌표 전달 위해 JumpStage에도 보존
            now.JumpStage = new JumpStage();
           
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
