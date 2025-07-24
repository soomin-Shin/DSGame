using JumpGame.Model;
using JumpGame.Components;
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
        // 배경 화면
        private Image _backgroundImage;
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
        // 점프 스테이지
        private JumpStage _jumpStage;
        public JumpStage JumpStage
        {
            get
            {
                return _jumpStage;
            }
            set
            {
                _jumpStage = value;
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
        private string _currentStage = "JumpStage";
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

        // Enemy Controller (보스 + 검기)
        private EnemyController _enemyController;
        public EnemyController EnemyController
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
        private bool _bossStageInitialized = false;
        private bool _jumpStageClear = false; // jumpstage로 이동
        private bool _gameClearDisplayed = false;

        public AdventureOfKnight()
        {
            // 폼 사이즈 변경 불가
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // 화면 깜빡임 방지
            this.DoubleBuffered = true;
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
            // Obstruction 인스턴스 생성
            _obstruction = new Obstruction();
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

            // 만약 스테이지 처음 선언할때 바꿔주고 싶으면 여기에 넣으시오.
            if (_currentStage == "JumpStage")
            {
                JumpStage.CreateStage(this);
                //BossStage.CreateStage(this);
            }
            else
            {

            }
            if (_enemyController?.GameCleared == true && !_gameClearDisplayed)
            {
                _gameClearDisplayed = true;
                _gameTimer.Stop();
                Invalidate();
                return;
            }
        }

        // 게임 상태 업데이트
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (_enemyController?.GameCleared == true && !_gameClearDisplayed)
            {
                _gameClearDisplayed = true;
                _gameTimer.Stop();
                Invalidate();
                return;
            }

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

            // 점프 스테이지에서 캐릭터 낙하 시 초기 위치로 리셋
            _jumpStage.JumpStageReset(_characterStatus);

            // 모든 발판 리스트 순서대로 업데이트
            for (int i = 0; i < _platforms.Count; i++)
            {
                // i번째 발판 위치, 상태 업데이트
                _platforms[i].PlatformUpdate();
            }
            
            // 투사체 상태 확인
            _characterStatus.UpdateProjectiles();
            _characterStatus.CharacterUpdate(_platforms, _cameraDisplay.Y);

            // 카메라 위치 업데이트 - 캐릭터 Y 기준으로 따라가도록
            _cameraDisplay.CameraUpdate(_characterStatus.GetX(), _characterStatus.GetY());

            // 적 상태 확인
            _enemyController?.EnemyUpdate(CharacterStatus.SwordProjectiles);

            int currentScreenWidth = this.ClientSize.Width; // 현재 폼의 너비

            _obstruction.UpdateAllObstacles(currentScreenWidth); // 수정된 메서드 호출
            
            // 점프 스테이지의 아이템 리스트
            List<Item> items = _jumpStage.ItemManager.GetItems();
            // 캐릭터 히트박스
            Rectangle charRect = _characterStatus.GetHitBox();

            for (int i = 0; i < items.Count; i++)
            {
                // 아이템이 캐릭터 히트박스와 겹치는지 검사
                if (items[i].Area.IntersectsWith(charRect))
                {   // 아이템이 루비일 경우 점수 1000점 추가
                    if (items[i].Type == ItemType.Ruby)
                    {
                        GameStats.Score += 1000;
                    }
                    // 아이템이 사파이어일 경우 점수 500점 추가
                    else if (items[i].Type == ItemType.Sapphire)
                    {
                        GameStats.Score += 500;
                    }
                    // 아이템이 에메랄드일 경우 점수 100점 추가
                    else if (items[i].Type == ItemType.Emerald)
                    {
                        GameStats.Score += 100;
                    }
                    items.RemoveAt(i);
                    break;
                }
            }

            // 골인 발판 판정
            for (int i = 0; i < _platforms.Count; i = i + 1)  // 각 스테이지
            {
                // i번째 발판이 Goal 타입이고 활성 되어 있는 경우
                if (_platforms[i].Type == PlatformType.Goal && _platforms[i].IsActive == true)  // 각 스테이지
                {
                    // 골인 발판의 영역을 goalRect에 저장
                    Rectangle goalRect = _platforms[i].Area;  // 각 스테이지

                    // 골인 발판 영역의 위쪽과 캐릭터 히트 박스의 아래쪽이 같은지 판단
                    bool isOnGoal = charRect.Bottom == goalRect.Top; // 각 스테이지


                    if (isOnGoal == true) // 각 스테이지
                    {
                        _jumpStageClear = true;
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
            
            //검기 발사
            if (e.KeyCode == Keys.Q)
            {
                _characterStatus.ShootSword();
            }

            // 골인 지점으로 왔을 때 해당 좌표에서 위에 키를 누르면 보스 스테이지로 이동시켜주기.
            if (_jumpStageClear == true && _bossStageInitialized == false)
            {
                if (e.KeyCode == Keys.Up)
                {
                    // Boss 스테이지로 이동하는 메서드 실행.
                    _currentStage = "BossStage";
                    BossStage.CreateStage(this);
                    _bossStageInitialized = true;
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

            // 아이템 그리기
            _jumpStage.DrawItems(g, _cameraDisplay.X, _cameraDisplay.Y);

            // UI 요소 그리기
            _gameUI.DrawScoreUI(g, this.ClientSize.Width, this.ClientSize.Height);
           
            // 검기 발사
            _characterStatus.ShootProjectiles(g, _cameraDisplay.X, _cameraDisplay.Y);

            if (_currentStage == "BossStage")
            {
                _enemyController.EnemyDraw(g, _cameraDisplay.X, _cameraDisplay.Y);
                if (_gameClearDisplayed)
                {
                    Font clearFont = new Font("Arial", 40, FontStyle.Bold);
                    Brush clearBrush = Brushes.Yellow;
                    g.DrawString("용을 죽이고 보물을 얻었다!", clearFont, clearBrush, 500, 300);
                    g.DrawString($"SCORE: {GameStats.Score}", clearFont, clearBrush, 500, 360);
                }
            }
            else
            {
                // 장애물 그리기
                _obstruction.DrawAllObstacles(g, _cameraDisplay.X, _cameraDisplay.Y);
            }
        }

        /// <summary>
        /// 게임 다시 시작
        /// </summary>
        public void ResumeGame()
        {
            _isGamePaused = false;
            GameTimer.Start(); // 게임 재개
            GameStats.GameTimer.Start();
            _pauseMenu.Visible = false; // 메뉴 숨김
            this.Focus();
        }
        /// <summary>
        /// 게임 멈춤
        /// </summary>
        public void PauseGame()
        {
            _isGamePaused = true;
            GameTimer.Stop(); // 게임 일시정지
            GameStats.StopTimer();
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
    }
}
