/*
Точка с координатами X и Y. 
Да, есть встроенный тип с большим функционалом, но почему-то я слепил свой. Наверное, на будущее, которое никогда не наступит.

2022-10-26 
*/

namespace Scge
{
    /// <summary>
    /// Точка на плоскости
    /// </summary>
    internal struct Point
    {
        /// <summary>
        /// Координата по оси X (горизонталь).
        /// </summary>
        internal int X { get; init; }

        /// <summary>
        /// Координата по оси Y (вертикаль).
        /// </summary>
        internal int Y { get; init; }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
