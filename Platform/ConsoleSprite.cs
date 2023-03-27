/*
Спрайт сущности для вывода на консоль.
Используется в ConsoleAccelerator.
Можно было бы добавить сюда паитру, но, пока мне кажется, это избыточная оптимизация. Спрайты миниатюрные, проще их копипастить.

2022-11-23
*/

namespace PlatformConsole
{
    /// <summary>
    /// Спрайт сущности для вывода на консоль.
    /// </summary>
    internal class ConsoleSprite
    {
        /// <summary>
        /// Список символов.
        /// </summary>
        internal ColoredChar[,] Image { get; }

        /// <summary>
        /// Конструктор (из другого спрайта).
        /// </summary>
        internal ConsoleSprite(ConsoleSprite source)
        {
            Image = source.Image;
        }

        /// <summary>
        /// Конструктор (из массива и палитры).
        /// </summary>
        internal ConsoleSprite(byte[,] bitMap, ConsoleColor?[] palette  )
        {
            int height = bitMap.GetUpperBound(0) + 1;
            int width = bitMap.GetUpperBound(1) + 1;
            int colorCount;

            Image = new ColoredChar[width, height / 2];

            for(int widthIndex = 0; widthIndex < width; widthIndex++)
            {
                for (int heightIndex = 0; heightIndex < height / 2; heightIndex++)
                {
                    ConsoleColor? top = palette[bitMap[heightIndex * 2, widthIndex]];
                    ConsoleColor? down = palette[bitMap[heightIndex * 2 + 1, widthIndex]];

                    if (top == down && top is not null)
                    {
                        Image[widthIndex, heightIndex] = new((ConsoleColor)top);
                    }
                    else
                    {
                        Image[widthIndex, heightIndex] = new(top, down);
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор (пустой спрайт).
        /// </summary>
        internal ConsoleSprite(int width, int height, ConsoleColor? backgroundColor = null, ConsoleColor? foregroundColor = null, Char fillChar = ConsoleAccelerator.Space)
        {
            Image = new ColoredChar[width, height];
            FillImage(backgroundColor, foregroundColor, fillChar);
        }

        /// <summary>
        /// Ширина.
        /// </summary>
        internal int Width => Image.GetUpperBound(0) + 1;

        /// <summary>
        /// Высота.
        /// </summary>
        internal int Height => Image.GetUpperBound(1) + 1;

        /// <summary>
        /// Заполнить образ цветом фона.
        /// </summary>
        internal void FillImage(ConsoleColor? backgroundColor, ConsoleColor? foregroundColor, Char @char = ConsoleAccelerator.DefaultChar)
        {
            for (int column = 0; column < Width; column++)
            {
                for (int line = 0; line < Height; line++)
                {
                    Image[column, line] = new(backgroundColor, foregroundColor, @char, false);
                }
            }
        }
    }
}
