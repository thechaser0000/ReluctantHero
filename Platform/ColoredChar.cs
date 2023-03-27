/*
Символ для вывода на консоль с указанием цвета текста и цвета фона.
Используется в ConsoleAccelerator.

2022-11-23 (Вынесено из ConsoleAccelerator)
*/

namespace PlatformConsole
{
    /// <summary>
    /// Символ для вывода на консоль с указанием цвета текста и цвета фона.
    /// </summary>
    internal struct ColoredChar
    {
        /// <summary>
        /// Символ пустой (не содержит ни одного полусимвола).
        /// </summary>
        internal bool IsEmpty => BackgroundColor is null && ForegroundColor is null;
     
        /// <summary>
        /// Символ полный (содержит два полусимвола) .
        /// </summary>
        internal bool IsFull => BackgroundColor is not null && ForegroundColor is not null;
        internal ConsoleColor? BackgroundColor { get; set; }
        internal ConsoleColor? ForegroundColor { get; set; }
        internal Char Char { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ColoredChar(ConsoleColor? backgroundColor, ConsoleColor? foregroundColor, Char @char, bool isEmpty)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            Char = @char;
        }

        /// <summary>
        /// Цвет нижнего полусимвола.
        /// </summary>
        internal ConsoleColor? BottomColor => BackgroundColor;
        
        /// <summary>
        /// Цвет верхнего полусимвола.
        /// </summary>
        internal ConsoleColor? TopColor => ForegroundColor;

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ColoredChar(ConsoleColor? topColor, ConsoleColor? bottomColor)
        {
            BackgroundColor = bottomColor;
            ForegroundColor = topColor;
            Char = bottomColor != topColor ? ConsoleAccelerator.TopSquare : ConsoleAccelerator.Space;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ColoredChar(ConsoleColor сolor)
        {
            BackgroundColor = сolor;
            ForegroundColor = сolor;
            Char = ConsoleAccelerator.Space;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ColoredChar()
        {
            BackgroundColor = null;
            ForegroundColor = null;
            Char = ConsoleAccelerator.DefaultChar;
        }

        /// <summary>
        /// Сравнить на равенство.
        /// </summary>
        public bool Equals(ColoredChar other)
        {
            return BackgroundColor == other.BackgroundColor && ForegroundColor == other.ForegroundColor && Char == other.Char && IsEmpty == other.IsEmpty;
        }

        /// <summary>
        /// Преобразовать в строку.
        /// </summary>
        public override string ToString()
        {
            return Char.ToString();
        }
    }
}
