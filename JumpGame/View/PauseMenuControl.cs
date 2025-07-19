using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raccoon.View
{
    public partial class PauseMenuControl : UserControl
    {
        private string _parentRaccoonForm;
        public PauseMenuControl( )
        {
            InitializeComponent();
        }
        /// <summary>
        /// 게임 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 게임 재시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, EventArgs e)
        {
            //_parentRaccoonForm._Character.reSet();
            //_parentRaccoonForm._Obstruction.reSet();
            //_parentRaccoonForm._Enemy.reSet();
            //_parentRaccoonForm.ResumeGame();
        }
    }
}
