/*
Прогрессбар.
Окрашенный прямоугольник из двух частей разных цветов.
Высота всегда составляет 1 строку, но отрисовка может осуществляться либо в режиме полной высоты, либо в режиме верхнего/нижнего полусимвола

Если прогресс больше минимума - цветом ForeColor закрашена хотя бы одна позиция.
Если прогресс меньше максимума - цветом BackColor закрашена хотя бы одна позиция.

2022-12-08
 
*/

using Scge;

namespace PlatformConsole
{
    /// <summary>
    /// Высота прогрессбара.
    /// </summary>
    internal enum ProgressBarHeight
    {
        /// <summary>
        /// Полная высота.
        /// </summary>
        Full = 0,
        /// <summary>
        /// Верхний полусимвол.
        /// </summary>
        Top = 1,
        /// <summary>
        /// Нижний полусимвол.
        /// </summary>
        Bottom = 2
    }

    /// <summary>
    /// Прогрессбар.
    /// </summary>
    internal class ProgressBar
    {

        /// <summary>
        /// Видим.
        /// </summary>
        internal bool IsVisible { get; set; }

        internal ProgressBar()
        {
            IsVisible = true;
            Characteristic = new();
        }

        /// <summary>
        /// Цвет закрашенной области.
        /// </summary>
        internal ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Цвет незакрашенной области.
        /// </summary>
        internal ConsoleColor BackgroundColor { get; set; }   

        /// <summary>
        /// Цвет прозрачной области.
        /// (Используется при неполной высоте)
        /// </summary>
        internal ConsoleColor TransparentColor { get; set; }

        /// <summary>
        /// Крайняя верхняя линия.
        /// </summary>
        internal int Top { get; set; }
        
        /// <summary>
        /// Горизонтальный сегмент.
        /// </summary>
        internal Segment Horizontal { get; set; }

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        internal Rectangle Rectangle => new(Horizontal, new(Top, Top));

        /// <summary>
        /// Ширина.
        /// </summary>
        internal int Width => Horizontal.Length;

        /// <summary>
        /// Высота.
        /// (полная или верхняя/нижняя половина)
        /// </summary>
        internal ProgressBarHeight Height { get; set; }

        /// <summary>
        /// Характеристика. 
        /// (Максимум, минимум, текущее значение)
        /// </summary>
        internal Characteristic Characteristic { get; set; }
    }
}
