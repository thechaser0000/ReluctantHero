/*
Прямоугольник на плоскости. 
Вспомогательный невизуальный класс для манипуляций с дискретными прямоугольниками на плоскости.
Формируется из двух отрезков: вертикального Y и горизонтального X.

Не зависит от платформы и движка.

2022-10-25
*/

namespace Scge
{
    /// <summary>
    /// Прямоугольник
    /// </summary>
    internal struct Rectangle
    {
        /// <summary>
        /// Крайняя левая колонка.
        /// </summary>
        internal int Left => Horizontal.Start;
        
        /// <summary>
        /// Крайняя верхняя линия.
        /// </summary>
        internal int Top => Vertical.Start;
        
        /// <summary>
        /// Крайняя правая колонка.
        /// </summary>
        internal int Right => Horizontal.Stop;
        
        /// <summary>
        /// Крайняя нижняя линия.
        /// </summary>
        internal int Bottom => Vertical.Stop;

        /// <summary>
        /// Горизонтальный сегмент X.
        /// </summary>
        internal Segment Horizontal { get; init; }
        
        /// <summary>
        /// Вертикальный сегмент Y.
        /// </summary>
        internal Segment Vertical { get; init; }

        /// <summary>
        /// Ширина.
        /// </summary>
        internal int Width => Horizontal.Length;

        /// <summary>
        /// Высота.
        /// </summary>
        internal int Height => Vertical.Length;
        
        /// <summary>
        /// Ёмкость.
        /// </summary>
        internal int Capacity => Height * Width;

        /// <summary>
        /// Начальная точка (левая верхняя).
        /// </summary>
        internal Scge.Point Start
        {
            get => new(Horizontal.Start, Vertical.Start);
            init 
            {
                if (Horizontal.StopIsNull)
                {
                    Horizontal = new() { Start = value.X };
                }
                else
                {
                    Horizontal = new() { Start = value.X, Stop = Horizontal.Stop };
                }

                if (Vertical.StopIsNull)
                {
                    Vertical = new() { Start = value.Y };
                }
                else
                {
                    Vertical = new() { Start = value.Y, Stop = Vertical.Stop };
                }
            }
        }

        /// <summary>
        /// Конечная точка (правая нижняя).
        /// </summary>
        internal Scge.Point Stop
        {
            get => new(Horizontal.Stop, Vertical.Stop);
            init
            {
                if (Horizontal.StartIsNull)
                {
                    Horizontal = new() { Stop = value.X };
                }
                else
                {
                    Horizontal = new() { Stop = value.X, Start = Horizontal.Start };
                }

                if (Vertical.StartIsNull)
                {
                    Vertical = new() { Stop = value.Y };
                }
                else
                {
                    Vertical = new() { Stop = value.Y, Start = Vertical.Start };
                }
            }
        }

        /// <summary>
        /// Конструктор с указанием левой верхней и правой нижней точек прямоугольника. 
        /// </summary>
        internal Rectangle(int left, int top, int right, int bottom) : this()
        {
            Horizontal = new(left, right);
            Vertical = new(top, bottom);
        }

        /// <summary>
        /// Конструктор с указанием горизонтального и вертикального отрезков. 
        /// </summary>
        internal Rectangle(Segment horizontal, Segment vertical) : this()
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>
        /// Конструктор с указанием левой верхней и правой нижней точек прямоугольника. 
        /// </summary>
        internal Rectangle(Scge.Point start, Scge.Point stop) : this()
        {
            Horizontal = new(start.X, stop.X);
            Vertical = new(start.Y, stop.Y);
        }

        /// <summary>
        /// Конструктор с указанием левой верхней точки и ширины и высоты 
        /// </summary>
        internal Rectangle(Scge.Point start, int width = 1, int height = 1) : this()
        {
            Horizontal = new() { Start = start.X, Length = width };
            Vertical = new() { Start = start.Y, Length = height };
        }

        /// <summary>
        /// Конструктор с указанием ширины и высоты (начальная точка (0,0)). 
        /// </summary>
        internal Rectangle(int width = 1, int height = 1) : this()
        {
            Horizontal = new() { Start = 0, Length = width };
            Vertical = new() { Start = 0, Length = height };
        }

        /// <summary>
        /// Переместить прямоугольник в указанном направлении
        /// </summary>
        internal static Rectangle MoveTo(Rectangle rectangle, Direction direction, int distance = 1)
        {
            return direction switch
            {
                Direction.Up => new(rectangle.Horizontal, new(rectangle.Top - distance, rectangle.Bottom - distance)),
                Direction.Down => new(rectangle.Horizontal, new(rectangle.Top + distance, rectangle.Bottom + distance)),
                Direction.Left => new(new(rectangle.Left - distance, rectangle.Right - distance), rectangle.Vertical),
                Direction.Right => new(new(rectangle.Left + distance, rectangle.Right + distance), rectangle.Vertical),
                _ => rectangle,
            };
        }
    }

}
