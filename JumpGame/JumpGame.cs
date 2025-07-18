﻿using JumpGame.Model;
using Raccoon.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpGame
{

    public partial class JumpGame : Form       
    {   
        // 카메라
        private Camera _camera;
        // 캐릭터
        private Character _character;
        // 발판 리스트
        private List<Platform> _platforms;
        // 게임 타이머
        private Timer _gameTimer;
        // 게임 시작 시간
        private DateTime _startTime;
        // 왼쪽 버튼 눌렀는지
        private bool _leftPressed = false;
        // 오른쪽 버튼 눌렀는지
        private bool _rightPressed = false;
        // 점프(스페이스) 버튼 눌렀는지
        private bool _jumpPressed = false;
        // ESC키 체크
        private bool _isGamePaused = false;
        // 점프 입력 버퍼 프레임 수
        private int _jumpBufferFrames = 5;
        // 점프 입력 버퍼 카운터
        private int _jumpBuffer = 0;
        // 폰트
        PrivateFontCollection _fonts = new PrivateFontCollection();
        // 게임 효과음 관리 객체
        private Effect _gameEffects;
        // ESC PauseMenu
        private PauseMenuControl _pauseMenu;
        // Game 점수, 목숨, 시간
        private GameStats _gameStats;

        public JumpGame()                      
        {
            // 게임 화면 너비
            this.Width = 800;
            // 게임 화면 높이
            this.Height = 600;
            // 화면 깜빡임 방지
            this.DoubleBuffered = true;        
            // 카메라 초기화
            _camera = new Camera(this.ClientSize.Width, this.ClientSize.Height);
            // 캐릭터 시작 위치
            _character = new Character(400, 1200);
            // 발판 리스트 생성
            _platforms = new List<Platform>();
            // 폰트 적용
            LoadCustomFont();
            this.Font = new Font(_fonts.Families[0], 14, FontStyle.Regular);
            // 효과음 추가
            _gameEffects = new Effect();
            // esc 추가
            PauseMenuSetting();
            // 게임 통계 초기화
            _gameStats = new GameStats();

            // 발판
            _platforms.Add(new Platform(new Rectangle(400, 1200, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(330, 1150, 40, 15), PlatformType.StepDisappear));
            _platforms.Add(new Platform(new Rectangle(260, 1100, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(190, 1050, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(120, 1000, 40, 15), PlatformType.Disappear));
            _platforms.Add(new Platform(new Rectangle(50, 950, 40, 15), PlatformType.Moving));
            _platforms.Add(new Platform(new Rectangle(120, 900, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(200, 850, 40, 15), PlatformType.StepDisappear));
            _platforms.Add(new Platform(new Rectangle(280, 800, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(360, 750, 40, 15), PlatformType.Disappear));
            _platforms.Add(new Platform(new Rectangle(430, 700, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(510, 650, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(580, 600, 40, 15), PlatformType.Moving));
            _platforms.Add(new Platform(new Rectangle(500, 550, 40, 15), PlatformType.StepDisappear));
            _platforms.Add(new Platform(new Rectangle(420, 500, 40, 15), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(300, 400, 40, 40), PlatformType.Goal));

            // 타이머 생성
            _gameTimer = new Timer();                      
            _gameTimer.Interval = 16;
            // 타이머 Tick 이벤트 핸들러 등록
            _gameTimer.Tick += GameTimer_Tick;
            // 게임 시작
            _gameTimer.Start();                            

            this.KeyDown += GameForm_KeyDown;              
            this.KeyUp += GameForm_KeyUp;                  
            this.Paint += GameForm_Paint;

            // 시작 시간 저장
            _startTime = DateTime.Now;
        }

        // 게임 상태 업데이트
        private void GameTimer_Tick(object sender, EventArgs e)   
        {
            // 점프 입력 버퍼 관리
            if (_jumpBuffer > 0)
            {
                _jumpBuffer--;
            }

            // 캐릭터가 현재 땅 위에 있고, 점프 버퍼가 유효하면 점프 실행
            if (_character.IsOnGround() == true && _jumpBuffer > 0)
            {
                _character.Jump();
                _gameEffects.PlayJumpSound();
                _jumpBuffer = 0;
            }

            // 왼쪽으로 이동
            if (_leftPressed == true)
            {
                _character.MoveLeft();
            }
            // 오른쪽으로 이동
            if (_rightPressed == true)
            {
                _character.MoveRight();
            }

            // 모든 발판 리스트 순서대로 업데이트
            for (int i = 0; i < _platforms.Count; i++)      
            {
                // i번째 발판 위치, 상태 업데이트
                _platforms[i].PlatformUpdate();
            }

            // 캐릭터 상태 업데이트
            _character.CharacterUpdate(_platforms, _camera.Y);
            // 카메라 상태 업데이트
            _camera.CameraUpdate(_character.GetX(), _character.GetY());

            // 골인 발판 판정
            for (int i = 0; i < _platforms.Count; i = i + 1)
            {
                // i번째 발판이 Goal 타입이고 활성 되어 있는 경우
                if (_platforms[i].Type == PlatformType.Goal && _platforms[i].IsActive == true)
                {
                    // 골인 발판의 영역을 goalRect에 저장
                    Rectangle goalRect = _platforms[i].Area;
                    // 캐릭터 히트박스를 charRect에 저장
                    Rectangle charRect = _character.GetHitBox();

                    // 캐릭터 히트박스가 골인 발판의 영역과 겹쳤을 때
                    if (charRect.IntersectsWith(goalRect) == true)
                    {
                        // 게임 타이머 멈춤
                        _gameTimer.Stop();
                        // 메세지 출력
                        string message = "골인 지점에 도달했습니다!\n걸린 시간: " + _gameStats.ElapsedTime.TotalSeconds.ToString("F2") + "초";
                        MessageBox.Show(message, "게임 종료");
                        this.Close();
                        break; 
                    }
                }
            }
            // 게임 시간 업데이트
            _gameStats.ElapsedTime = DateTime.Now - _startTime;
            // 화면 다시 그리기
            this.Invalidate();   
        }

        // 키를 누를 때
        private void GameForm_KeyDown(object sender, KeyEventArgs e)    
        {
            if (e.KeyCode == Keys.Left)
            {
                _leftPressed = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                _rightPressed = true;
            }
            // 점프키는 착지 상태일 때만 한 번 눌림으로 인식
            if (e.KeyCode == Keys.Space)
            {
                _jumpBuffer = _jumpBufferFrames;
            }
            if (e.KeyCode == Keys.Escape)
            {
                 if (_isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        // 키를 뗄 때
        private void GameForm_KeyUp(object sender, KeyEventArgs e)      
        {
            if (e.KeyCode == Keys.Left)
            {
                _leftPressed = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                _rightPressed = false;
            }
        }

        // 화면 그리기
        private void GameForm_Paint(object sender, PaintEventArgs e)    
        {
            Graphics g = e.Graphics;
            // 배경 색 채우기
            g.Clear(Color.CornflowerBlue);

            // 모든 발판 그리기
            for (int i = 0; i < _platforms.Count; i++)  
            {
                _platforms[i].PlatformDraw(g, _camera.X, _camera.Y);
            }

            // 캐릭터 그리기
            _character.Draw(g, _camera.X, _camera.Y);

            //Score 그리기
            DrawScoreUI(g);
        }

        // 폰트 추가
        private void LoadCustomFont()
        {
            string fontPath = $"{Application.StartupPath}//Assets//Font//Dongle-Regular.ttf";
            _fonts.AddFontFile(fontPath);
        }

        /// <summary>
        /// UI 요소 그리기 메서드
        /// </summary>
        private void DrawScoreUI(Graphics g)
        {
            // UI 배경 (선택 사항)
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, this.ClientSize.Height - 60, this.ClientSize.Width, 60);

            // 폰트 설정
            Font uiFont = new Font(_fonts.Families[0], 20, FontStyle.Bold); // 더 큰 폰트 크기
            SolidBrush fontBrush = new SolidBrush(Color.White);

            // 점수 표시
            string scoreText = $"Score: {_gameStats.Score}";
            g.DrawString(scoreText, uiFont, fontBrush, 10, this.ClientSize.Height - 50);

            // 목숨 표시
            string livesText = $"Lives: {_gameStats.Lives}";
            SizeF livesTextSize = g.MeasureString(livesText, uiFont);
            g.DrawString(livesText, uiFont, fontBrush, this.ClientSize.Width / 2 - livesTextSize.Width / 2, this.ClientSize.Height - 50);

            // 시간 표시
            string timeText = $"Time: {_gameStats.ElapsedTime.TotalSeconds:F2}초";
            SizeF timeTextSize = g.MeasureString(timeText, uiFont);
            g.DrawString(timeText, uiFont, fontBrush, this.ClientSize.Width - timeTextSize.Width - 10, this.ClientSize.Height - 50);

            uiFont.Dispose();
            fontBrush.Dispose();
        }

        /// <summary>
        /// 게임 다시 시작
        /// </summary>
        public void ResumeGame()
        {
            _isGamePaused = false;
            _gameTimer.Start(); // 게임 재개
            _pauseMenu.Visible = false; // 메뉴 숨김
            this.Focus();
        }
        /// <summary>
        /// 게임 멈춤
        /// </summary>
        public void PauseGame()
        {
            _isGamePaused = true;
            _gameTimer.Stop(); // 게임 일시정지
            _pauseMenu.Visible = true; // 메뉴 표시
            this.Focus();
        }

        /// <summary>
        /// esc 메뉴 세팅 메서드
        /// </summary>
        private void PauseMenuSetting()
        {
            _pauseMenu = new PauseMenuControl();

            // 중앙 위치 계산
            int centerX = (this.ClientSize.Width - _pauseMenu.Width) / 2;
            int centerY = (this.ClientSize.Height - _pauseMenu.Height) / 2;

            _pauseMenu.Location = new Point(centerX, centerY); // 계산된 위치 적용
            _pauseMenu.Visible = false; // 처음에 보이지 않게 설정

            this.Controls.Add(_pauseMenu); // 폼의 컨트롤 컬렉션에 추가
            _pauseMenu.BringToFront(); // 다른 컨트롤 위에 표시되도록 가장 앞으로 가져옴
        }
    }
}
