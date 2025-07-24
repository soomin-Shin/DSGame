using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JumpGame.Components
{
    // 아이템 종류
    public enum ItemType
    {
        Ruby,
        Sapphire,
        Emerald
    }

    public class Item
    {
        public ItemType Type;
        public Rectangle Area;

        public Item(ItemType type, Rectangle area)
        {
            Type = type;
            Area = area;
        }

        // 내부 아이템 리스트
        private List<Item> _items = new List<Item>();
        private Random _random = new Random();

        // 아이템 이미지
        private Image _rubyImage;
        private Image _sapphireImage;
        private Image _emeraldImage;

        // 생성자에서 이미지 로드
        public Item()
        {

            _rubyImage = Image.FromFile("Assets/Image/Ruby.png");
            _sapphireImage = Image.FromFile("Assets/Image/Sapphire.png");
            _emeraldImage = Image.FromFile("Assets/Image/Emerald.png");

        }

        // 아이템 리스트 반환
        public List<Item> GetItems()
        {
            return _items;
        }

        // 아이템 생성
        public void GenerateItems(List<Platform> platforms, int count)
        {
            int platformCount = platforms.Count;
            int[] indices = new int[platformCount];

            for (int i = 0; i < platformCount; i++)
            {
                indices[i] = i;
            }

            // Fisher-Yates 셔플
            for (int i = platformCount - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }

            for (int i = 0; i < count && i < platformCount; i++)
            {
                Platform platform = platforms[indices[i]];

                int itemWidth = 15;
                int itemHeight = 15;
                int itemX = platform.Area.X + (platform.Area.Width - itemWidth) / 2;
                int itemY = platform.Area.Y - itemHeight;

                Rectangle area = new Rectangle(itemX, itemY, itemWidth, itemHeight);
                ItemType type = (ItemType)_random.Next(0, 3);

                _items.Add(new Item(type, area));
            }
        }

        // 아이템 그리기
        public void DrawItems(Graphics g)
        {
            int count = _items.Count;

            for (int i = 0; i < count; i++)
            {
                Item item = _items[i];
                Image itemImage = null;

                if (item.Type == ItemType.Ruby)
                {
                    itemImage = _rubyImage;
                }
                else if (item.Type == ItemType.Sapphire)
                {
                    itemImage = _sapphireImage;
                }
                else if (item.Type == ItemType.Emerald)
                {
                    itemImage = _emeraldImage;
                }

                if (itemImage != null)
                {
                    g.DrawImage(itemImage, item.Area);
                }
            }
        }
    }
}
