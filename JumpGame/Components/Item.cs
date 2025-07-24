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
        // 아이템 타입
        public ItemType Type;
        // 아이템이 위치하고 있는 영역
        public Rectangle Area;

        public Item(ItemType type, Rectangle area)
        {
            Type = type;
            Area = area;
        }

        // 아이템 리스트
        private List<Item> _items = new List<Item>();
        // 랜덤 객체
        private Random _random = new Random();

        // 아이템 이미지
        private Image _rubyImage;
        private Image _sapphireImage;
        private Image _emeraldImage;

        // 이미지 로드
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
            // 발판 개수
            int platformCount = platforms.Count;

            // 인덱스 배열
            int[] index = new int[platformCount];

            // 0부터 platformCount-1까지의 인덱스 배열 생성
            for (int i = 0; i < platformCount; i++)
            {
                index[i] = i;
            }

            // 인덱스 배열 무작위 섞기
            for (int i = platformCount - 1; i > 0; i--)
            {
                // 0 이상 i 이하 임의 수 생성
                int j = _random.Next(0, i + 1);
                int temp = index[i];
                index[i] = index[j];
                index[j] = temp;
            }

            // 인덱스 순서대로 최대 count 개수만큼 아이템 생성
            for (int i = 0; i < count && i < platformCount; i++)
            {
                Platform platform = platforms[index[i]];

                // 아이템 크기 지정
                int itemWidth = 30;
                int itemHeight = 30;
                // 아이템 발판 중앙 정렬
                int itemX = platform.Area.X + (platform.Area.Width - itemWidth) / 2;
                // 아이템 발판 바로 위에 배치
                int itemY = platform.Area.Y - itemHeight;

                // 아이템의 위치와 크기를 Rectangle로 생성
                Rectangle area = new Rectangle(itemX, itemY, itemWidth, itemHeight);
                // 아이템 타입을 랜덤으로 정함
                ItemType type = (ItemType)_random.Next(0, 3);
                // 새 아이템 객체를 만들어 리스트에 추가
                _items.Add(new Item(type, area));
            }
        }

        // 아이템 그리기
        public void DrawItems(Graphics g, int offsetX, int offsetY)
        {
            int count = _items.Count;

            for (int i = 0; i < count; i++)
            {
                Item item = _items[i];
                Image itemImage = null;

                // 아이템 타입이 루비 일 때
                if (item.Type == ItemType.Ruby)
                {
                    itemImage = _rubyImage;
                }
                // 아이템 타입이 사파이어 일 때
                else if (item.Type == ItemType.Sapphire)
                {
                    itemImage = _sapphireImage;
                }
                // 아이템 타입 에메랄드 일 때
                else if (item.Type == ItemType.Emerald)
                {
                    itemImage = _emeraldImage;
                }

                // 이미지가 정상적으로 로드된 경우
                if (itemImage != null)
                {
                    // 카메라 위치 계산 해서 그릴 위치 계산
                    Rectangle drawRect = new Rectangle(item.Area.X - offsetX, item.Area.Y - offsetY, item.Area.Width, item.Area.Height);
                    // 이미지 그리기
                    g.DrawImage(itemImage, drawRect);
                }
            }
        }
    }
}
