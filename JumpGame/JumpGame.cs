using JumpGame.Model;
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
        //폰트
        PrivateFontCollection _fonts = new PrivateFontCollection();
        // 게임 효과음 관리 객체
        private Effect _gameEffects;
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

            // 발판
            _platforms.Add(new Platform(new Rectangle(400, 1150, 130, 20), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(350, 1040, 100, 20), PlatformType.Moving));
            _platforms.Add(new Platform(new Rectangle(450, 960, 120, 20), PlatformType.StepDisappear));
            _platforms.Add(new Platform(new Rectangle(320, 880, 110, 20), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(510, 800, 100, 20), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(400, 720, 100, 20), PlatformType.Moving));
            _platforms.Add(new Platform(new Rectangle(460, 640, 120, 20), PlatformType.StepDisappear));
            _platforms.Add(new Platform(new Rectangle(380, 560, 100, 20), PlatformType.Normal));
            _platforms.Add(new Platform(new Rectangle(420, 450, 40, 40), PlatformType.Goal));

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
                        // 게임 시작 경과 시간을 계산
                        TimeSpan elapsed = DateTime.Now - _startTime;
                        // 메세지 출력
                        string message = "골인 지점에 도달했습니다!\n걸린 시간: " + elapsed.TotalSeconds.ToString("F2") + "초";
                        MessageBox.Show(message, "게임 종료");
                        this.Close();
                        break; 
                    }
                }
            }

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
                _gameEffects.PlayJumpSound();
            }
            if (e.KeyCode == Keys.Escape)
            {
                 if (_isGamePaused)
                {
                    _isGamePaused = false;
                    _gameTimer.Start(); // 게임 재개
                }
                else
                {
                    _isGamePaused = true;
                    _gameTimer.Stop(); // 게임 일시정지
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
        }

        // 폰트 추가
        private void LoadCustomFont()
        {
            string fontPath = $"{Application.StartupPath}//Assets//Font//Dongle-Regular.ttf";
            _fonts.AddFontFile(fontPath);
        }
    }
}
