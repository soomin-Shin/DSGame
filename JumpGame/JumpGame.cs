using JumpGame.Model;
using JumpGame.View;
using JumpGame.Stages;
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
        public Camera Camera
        {
            get
            {
                return _camera;
            }
            set
            {
                _camera = value;
            }
        }
        // 캐릭터
        private Character _character;
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
            }
        }
        // 발판 리스트
        private List<Platform> _platforms;
        // 배경 화면
        private Image _backgroundImage;
        // 게임 스테이지
        private JumpStage _jumpstage;
        public JumpStage Jump
        {
            get
            {
                return _jumpstage;
            }
            set
            {
                _jumpstage = value;
            }
        }
        // 게임 타이머
        private Timer _gameTimer;
        public Timer GameTimer
        {
            get
            {
                return _gameTimer;
            }
            set
            {
                _gameTimer = value;
            }
        }
        // 게임 시작 시간
        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }
        // 왼쪽 버튼 눌렀는지
        private bool _leftPressed = false;
        public bool LeftPressed
        {
            get
            {
                return _leftPressed;
            }
            set
            {
                _leftPressed = value;
            }
        }
        // 오른쪽 버튼 눌렀는지
        private bool _rightPressed = false;
        public bool RightPressed
        {
            get
            {
                return _rightPressed;
            }
            set
            {
                _rightPressed = value;
            }
        }
        // 점프(스페이스) 버튼 눌렀는지
        private bool _jumpPressed = false;
        // ESC키 체크
        private bool _isGamePaused = false;
        public bool IsGamePaused
        {
            get
            {
                return _isGamePaused;
            }
            set
            {
                _isGamePaused = value;
            }
        }
        // 점프 입력 버퍼 프레임 수
        private int _jumpBufferFrames = 5;
        // 점프 입력 버퍼 카운터
        private int _jumpBuffer = 0;
        public int JumpBuffer
        {
            get
            {
                return _jumpBuffer;
            }
            set
            {
                _jumpBuffer = value;
            }
        }
        // 폰트
        PrivateFontCollection _fonts = new PrivateFontCollection();
        // 게임 효과음 관리 객체
        private Effect _gameEffects;
        // ESC PauseMenu
        private PauseMenuControl _pauseMenu;
        // Game 점수, 목숨, 시간
        private GameStats _gameStats;
        public GameStats GameStats
        {
            get
            {
                return _gameStats;
            }
            set
            {
                _gameStats = value;
            }
        }
        // UI 관리 객체 추가
        private Ui _gameUI;

        public JumpGame()                      
        {
            // 게임 화면 너비
            this.Width = 800;
            // 게임 화면 높이
            this.Height = 600;
            // 화면 깜빡임 방지
            this.DoubleBuffered = true;        
            // 스테이지 초기화
            _jumpstage = JumpStage.CreateStage();
            // 발판 리스트 
            _platforms = _jumpstage.Platforms;
            _backgroundImage = _jumpstage.BackgroundImage;
            Point startPos = _jumpstage.StartPosition;
            _character = new Character(startPos.X, startPos.Y);
            // 카메라 초기화
            _camera = new Camera(this.ClientSize.Width, this.ClientSize.Height, _backgroundImage.Height);
            // 폰트 적용
            LoadCustomFont();
            this.Font = new Font(_fonts.Families[0], 12, FontStyle.Regular);
            // 효과음 추가
            _gameEffects = new Effect();
            // esc 추가
            PauseMenuSetting(this);
            // 게임 통계 초기화
            _gameStats = new GameStats();
            // UI 객체 초기화 
            _gameUI = new Ui(_gameStats, _fonts.Families[0]); // <-- UI 객체 생성         

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
            
            // 배경 화면을 카메라 에 맞춰 그리기
            if (_backgroundImage != null)
            {
                Rectangle srcRect = new Rectangle(0, _camera.Y, this.ClientSize.Width, this.ClientSize.Height);
                Rectangle destRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);

                if (srcRect.Bottom > _backgroundImage.Height)
                {
                    srcRect.Y = _backgroundImage.Height - this.ClientSize.Height;
                    if (srcRect.Y < 0)
                    {
                        srcRect.Y = 0;
                    }
                }
                g.DrawImage(_backgroundImage, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                // 배경 없으면 디폴트 채우기
                g.Clear(Color.CornflowerBlue);
            }

            // 모든 발판 그리기
            for (int i = 0; i < _platforms.Count; i++)  
            {
                _platforms[i].PlatformDraw(g, _camera.X, _camera.Y);
            }

            // 캐릭터 그리기
            _character.Draw(g, _camera.X, _camera.Y);

            // UI 요소 그리기
            _gameUI.DrawScoreUI(g, this.ClientSize.Width, this.ClientSize.Height);
        }

        // 폰트 추가
        private void LoadCustomFont()
        {
            string fontPath = $"{Application.StartupPath}//Assets//Font//Cinzel-VariableFont_wght.ttf";
            _fonts.AddFontFile(fontPath);
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
        private void PauseMenuSetting(JumpGame jumpGame)
        {
            _pauseMenu = new PauseMenuControl(jumpGame);

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
