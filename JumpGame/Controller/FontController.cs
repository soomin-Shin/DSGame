using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumpGame.Controller
{
    public class FontController
    {
        private PrivateFontCollection _fonts = new PrivateFontCollection();
        public PrivateFontCollection Fonts
        {
            get
            {
                return _fonts;
            }
            set
            {
                _fonts = value;
            }
        }

        public FontController(AdventureOfKnight parent){
            LoadCustomFont();
            parent.Font = new Font(_fonts.Families[0], 12, FontStyle.Regular);
        }

        // 폰트 추가
        private void LoadCustomFont()
        {
            string fontPath = $"{Application.StartupPath}//Assets//Font//Cinzel-VariableFont_wght.ttf";
            _fonts.AddFontFile(fontPath);
        }
    }
}
