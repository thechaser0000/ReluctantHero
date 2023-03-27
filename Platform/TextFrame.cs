/*
Текстовый фрейм.
Окрашенный прямоугольник с несколькими строками текста с различным выравниванием.

2022-12-08
 
*/

using Scge;

namespace PlatformConsole
{
    /// <summary>
    /// Отступы.
    /// </summary>
    internal struct Margins
    {
        /// <summary>
        /// Левый отступ.
        /// </summary>
        internal int Left { get; init; }
        /// <summary>
        /// Верхний отступ.
        /// </summary>
        internal int Top { get; init; }
        /// <summary>
        /// Правый отступ.
        /// </summary>
        internal int Right { get; init; }
        /// <summary>
        /// Нижний отступ.
        /// </summary>
        internal int Bottom { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Margins(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    /// <summary>
    /// Выравнивание по горизонтали.
    /// </summary>
    internal enum HorizontalAlignment
    {
        /// <summary>
        /// По левому краю.
        /// </summary>
        Left = 0,
        /// <summary>
        /// По центру.
        /// </summary>
        Center = 1,
        /// <summary>
        /// По правому краю.
        /// </summary>
        Right = 2,
    }

    /// <summary>
    /// Выравнивание по вертикали.
    /// </summary>
    internal enum VerticalAlignment
    {
        /// <summary>
        /// По верхнему краю.
        /// </summary>
        Top = 0,
        /// <summary>
        /// По центру.
        /// </summary>
        Center = 1,
        /// <summary>
        ///  По нижнему краю.
        /// </summary>
        Bottom = 2,
    }

    /// <summary>
    /// Текстовый фрейм.
    /// </summary>
    internal class TextFrame
    {
        /// <summary>
        /// Отступы для текста.
        /// </summary>
        internal Margins Margins { get; set; }

        /// <summary>
        /// Видим.
        /// </summary>
        internal bool IsVisible { get; set; }

        /// <summary>
        /// Строки.
        /// </summary>
        internal List<string> Strings { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal TextFrame(string? text = null)
        {
            IsVisible = true;
            Strings = new();

            if (text is not null)
            {
                Strings.Add(text);
            }
        }

        /// <summary>
        /// Все строки как тест.
        /// </summary>
        internal string AsText
        {
            get => String.Join("", Strings.ToArray());
            set
            {
                Strings.Clear();
                Strings.Add(value);
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal TextFrame(ConsoleColor backgroundColor, ConsoleColor foregroundColor, string? text = null) : this(text)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Выравнивание по горизонтали.
        /// </summary>
        internal HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Выравнивание по вертикали.
        /// </summary>
        internal VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        internal Rectangle Rectangle { get; set; }

        /// <summary>
        /// Цвет фона.
        /// </summary>
        internal ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Цвет текста.
        /// </summary>
        internal ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Цвет панели без текста.
        /// </summary>
        internal ConsoleColor Color
        {
            set
            {
                BackgroundColor = value;
                ForegroundColor = value;
            }
        }
    }
}
