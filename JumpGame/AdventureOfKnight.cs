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
using System.Diagnostics;
using JumpGame.Controller;

namespace JumpGame
{

    public partial class AdventureOfKnight : Form       
    {   
        // 카메라
        private CameraDisplay _cameraDisplay;
        public CameraDisplay CameraDisplay
        {
            get
            {
                return _cameraDisplay;
            }
            set
            {
                _cameraDisplay = value;
            }
        }
        // 캐릭터
        private CharacterStatus _characterStatus;
        public CharacterStatus CharacterStatus
        {
            get
            {
                return _characterStatus;
            }
            set
            {
                _characterStatus = value;
            }
        }
        // 발판 리스트
        private List<Platform> _platforms;
        // 배경 화면
        private Image _backgroundImage;
        // 점프 스테이지
        private JumpStage _jumpstage;
        public JumpStage Jumpstage
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
        // 점프 입력 버퍼 프레임
        private int _jumpBufferFrames = 5;
        // 점프 입력 버퍼
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
        // 게임 효과음 관리 객체
        private SoundEffect _soundEffect;
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
        private FontController _fontController;

        // UI 관리 객체 추가
        private Ui _gameUI;
        private Obstruction _obstruction;
        private string _currentStage;
        public string CurrentStage
        {
            get
            {
                return _currentStage;
            }
            set
            {
                _currentStage = value;
            }
        }
        public AdventureOfKnight()
        {
            // 폼 사이즈 변경 불가
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // 화면 깜빡임 방지
            this.DoubleBuffered = true;
            // Jumpstage
            Jumpstage = new JumpStage();
            // 효과음 추가
            _soundEffect = new SoundEffect();
            // esc 추가
            PauseMenuSetting(this);
            // 게임 통계 초기화 및 타이머 시작
            GameStats = new GameStats();
            // 폰트 초기화
            _fontController = new FontController(this);
            // UI 객체 초기화 
            _gameUI = new Ui(GameStats, _fontController.Fonts.Families[0]);
            // 타이머 생성
            GameTimer = new Timer();
            GameTimer.Interval = 16;
            // 타이머 Tick 이벤트 핸들러 등록
            GameTimer.Tick += GameTimer_Tick;
            // 게임 시작
            GameTimer.Start();

            this.KeyDown += GameForm_KeyDown; 
            this.KeyUp += GameForm_KeyUp;                  
            this.Paint += GameForm_Paint;

            this.MouseDown += new MouseEventHandler(CoordinateCheckClick); // MouseClick 이벤트 핸들러 추가

            // 만약 스테이지 처음 선언할때 바꿔주고 싶으면 여기에 넣으시오.
            if (_currentStage == "JumpStage")
            {

            }
            else
            {

            }
        }

        // 게임 상태 업데이트
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // 시간 업데이트
            GameStats.StatsUpdateTimer();

            // 점프 입력 버퍼 관리
            // https://m.blog.naver.com/sorang226/223083889817
            if (_jumpBuffer > 0)
            {
                _jumpBuffer--;
            }
            // 캐릭터가 현재 땅 위에 있고, 점프 버퍼가 유효하면 점프 실행
            if (_characterStatus.IsOnGround() == true && _jumpBuffer > 0)
            {
                _characterStatus.Jump();
                _soundEffect.PlayJumpSound();
                _jumpBuffer = 0;
            }


            // 왼쪽으로 이동
            if (_leftPressed == true)
            {
                _characterStatus.MoveLeft();
            }
            // 오른쪽으로 이동
            if (_rightPressed == true)
            {
                _characterStatus.MoveRight();
            }


            // 모든 발판 리스트 순서대로 업데이트
            for (int i = 0; i < _platforms.Count; i++)
            {
                // i번째 발판 위치, 상태 업데이트
                _platforms[i].PlatformUpdate();
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
            if (e.KeyCode == Keys.Space)
            {
                _jumpBuffer = _jumpBufferFrames;
            }

            if (e.KeyCode == Keys.Escape)
            {
                 if (_isGamePaused == true)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

            // 골인 지점으로 왔을 때 해당 좌표에서 위에 키를 누르면 보스 스테이지로 이동시켜주기.
            if ( 1 < CharacterStatus.GetX() && CharacterStatus.GetX() < 3 && CharacterStatus.GetY() == 2) // x좌표는 200 ~ 240까지 y좌표는 고정
            {
                if (e.KeyCode == Keys.Up)
                {
                    // Boss 스테이지로 이동하는 메서드 실행.
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
                Rectangle srcRect = new Rectangle(0, _cameraDisplay.Y, this.ClientSize.Width, this.ClientSize.Height);
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
                _platforms[i].PlatformDraw(g, _cameraDisplay.X, _cameraDisplay.Y);
            }

            // 캐릭터 그리기
            _characterStatus.Draw(g, _cameraDisplay.X, _cameraDisplay.Y);

            // UI 요소 그리기
            _gameUI.DrawScoreUI(g, this.ClientSize.Width, this.ClientSize.Height);

            // 장애물 그리기
            _obstruction.draw(g);
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
        private void PauseMenuSetting(AdventureOfKnight jumpGame)
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

        /// <summary>
        /// x,y 좌표 찾기 나중에 지울거임.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoordinateCheckClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"{e.X},{e.Y}");
        }
    }
}
