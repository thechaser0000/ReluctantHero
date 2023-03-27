/*
Простое текстовое меню. 


2022-12-11 
*/

using PlatformConsole;
using Scge;
using TestGame;

namespace PlatformConsole
{
    /// <summary>
    /// Простое текстовое многострочное меню.
    /// </summary>
    internal class Menu
    {
        /// <summary>
        /// Верхняя позиция.
        /// </summary>
        internal int Top { get; init; }

        /// <summary>
        /// Левая позиция.
        /// </summary>
        internal int Left { get; init; }

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        internal Rectangle Rectangle => Area.Rectangle;

        /// <summary>
        /// Высота меню.
        /// </summary>
        internal int Height => Items.Count + 4;

        private bool _isVisible;

        /// <summary>
        /// Видимо.
        /// </summary>
        internal bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    foreach(var frame in GetFrames())
                    {
                        frame.IsVisible = IsVisible;
                    }
                }
            }
        }

        /// <summary>
        /// Цвет заголовка.
        /// </summary>
        internal ConsoleColor HeaderColor { get; init; }

        /// <summary>
        /// Цвет элемента.
        /// </summary>
        internal ConsoleColor ItemColor { get; init; }

        /// <summary>
        /// Цвет выделенного элемента.
        /// </summary>
        internal ConsoleColor ItemSelectedColor { get; init; }

        /// <summary>
        /// Цвет справки.
        /// </summary>
        internal ConsoleColor HelpColor { get; init; }

        /// <summary>
        /// Цвет фона.
        /// </summary>
        internal ConsoleColor BackgroundColor { get; init; }

        private int _selectedIndex;

        /// <summary>
        /// Индекс выбранного элемента.
        /// </summary>
        internal int SelectedIndex
        {
            get => _selectedIndex;

            set
            {
                int index = Math.Min(Items.Count - 1, Math.Max(0, value));

                if (index != SelectedIndex)
                // Меняем текущий вбранный пункт и перекрашиваем старый выбранный пункт и новый выбранный пункт.
                {
                    if (SelectedIndex != -1)
                    {
                        Items[SelectedIndex].ForegroundColor = ItemColor;
                    }
                    _selectedIndex = index;
                    Items[SelectedIndex].ForegroundColor = ItemSelectedColor;
                }
            }
        }

        /// <summary>
        /// Цвет границы.
        /// </summary>
        internal ConsoleColor BorderColor { get; init; }

        /// <summary>
        /// Толщина границы.
        /// </summary>
        internal int BorderWidth { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Menu(int top, int left, string header, string help)
        {
            BorderWidth = 1;
            _isVisible = true;
            Top = top;
            Left = left;

            BorderColor = GameSeed.TransparentColor;
            BackgroundColor = GameSeed.TransparentColor;
            HeaderColor = ConsoleColor.Blue;
            ItemColor = ConsoleColor.White;
            ItemSelectedColor = ConsoleColor.DarkYellow;
            HelpColor = ConsoleColor.DarkGray;

            _selectedIndex = -1;

            InnerArea = new("") { BackgroundColor = BackgroundColor, Rectangle = new(0, Top + BorderWidth, GameSeed.WindowWidth - 1, Top + 3 + BorderWidth)};
            Area = new() { BackgroundColor = BorderColor, Rectangle = new(new Segment(), new(Top, Top + 3 + BorderWidth * 2))};

            Items = new();
            Header = new(BackgroundColor, HeaderColor, header)
            { Rectangle = new(new Segment(), new(InnerArea.Rectangle.Top, InnerArea.Rectangle.Top)), HorizontalAlignment = HorizontalAlignment.Center };

            Help = new(BackgroundColor, HelpColor, help)
            { Rectangle = new(new Segment(), new(InnerArea.Rectangle.Top + 3, InnerArea.Rectangle.Top + 3)), HorizontalAlignment = HorizontalAlignment.Center };
        }

        /// <summary>
        /// Добавить пункт.
        /// </summary>
        internal void AddItem(string item)
        {
            TextFrame frame = new(BackgroundColor, ItemColor, item)
            { Rectangle = new(new Segment(), new(InnerArea.Rectangle.Top + Height - 2, InnerArea.Rectangle.Top + Height - 2)), HorizontalAlignment = HorizontalAlignment.Left, IsVisible = IsVisible };

            Items.Add(frame);

            Help.Rectangle = Rectangle.MoveTo(Help.Rectangle, Direction.Down);
            InnerArea.Rectangle = new(new Segment(), new(InnerArea.Rectangle.Top, InnerArea.Rectangle.Bottom + 1));

            SelectedIndex = SelectedIndex == -1 ? 0 : SelectedIndex;
            RealignHorizontal();
        }

        /// <summary>
        /// Выровнять горизонтальное положение.
        /// (исходя из длины текста)
        /// </summary>
        private void RealignHorizontal()
        {
            int maxItemLength = 0;
            foreach(var item in Items)
            {
                maxItemLength = Math.Max(maxItemLength, item.AsText.Length);
            }
            int maxTextLength = Math.Max(maxItemLength, Header.AsText.Length);
            maxTextLength = Math.Max(maxTextLength, Help.AsText.Length);
            int left = Left + (GameSeed.WindowWidth - maxTextLength) / 2;
            int itemLeft = Left + (GameSeed.WindowWidth - maxItemLength) / 2;

            // внутренний прямоугольник
            InnerArea.Rectangle = new(new(left, left + maxTextLength - 1), InnerArea.Rectangle.Vertical);
            // внешний прямоугольник
            Area.Rectangle = new(new Segment(InnerArea.Rectangle.Left - 2 * BorderWidth, InnerArea.Rectangle.Right + 2 * BorderWidth), new(Top, InnerArea.Rectangle.Bottom + BorderWidth));

            // заголовок
            Header.Rectangle = new(InnerArea.Rectangle.Horizontal, Header.Rectangle.Vertical);
            // элементы
            for (int index = 0; index < Items.Count; index++)
            {
                Items[index].Rectangle = new(new(itemLeft, InnerArea.Rectangle.Right), Items[index].Rectangle.Vertical);
            }
            // справка
            Help.Rectangle = new(InnerArea.Rectangle.Horizontal, Help.Rectangle.Vertical);
        }

        /// <summary>
        /// Внутренняя область.
        /// </summary>
        internal TextFrame InnerArea { get; init; }

        /// <summary>
        /// Внешняя область.
        /// </summary>
        internal TextFrame Area { get; init; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        internal TextFrame Header { get; init; }

        /// <summary>
        /// Справка.
        /// </summary>
        internal TextFrame Help { get; init; }

        /// <summary>
        /// Пункты.
        /// </summary>
        internal List<TextFrame> Items { get; init; }

        /// <summary>
        /// Возвразщает все используемые текстовые фреймы.
        /// </summary>
        internal List<TextFrame> GetFrames()
        {
            List<TextFrame> result = new()
            {
                Area,
                InnerArea,
                Header,
                Help
            };
            result.AddRange(Items);

            return result;
        }
    }
}
