/*
Снимок состояния области вывода на конслоь.
Используется в ConsoleAccelerator.

2022-11-23 (Вынесено из ConsoleAccelerator)
*/

namespace PlatformConsole
{
    /// <summary>
    /// Снимок состояния области вывода.
    /// </summary>
    internal class ConsoleSnapshot
    {
        /// <summary>
        /// Ускоритель вывода цветного текста в консоль.
        /// </summary>
        private readonly ConsoleAccelerator accelerator;

        /// <summary>
        /// Конструктор
        /// </summary>
        internal ConsoleSnapshot(ConsoleAccelerator accelerator)
        {
            this.accelerator = accelerator;

            Characters = new ColoredChar[this.accelerator.Width, this.accelerator.Height];
            Reset();
        }

        /// <summary>
        /// Список символов.
        /// </summary>
        internal ColoredChar[,] Characters { get; }

        /// <summary>
        /// Сброс снимка.
        /// </summary>
        internal void Reset()
        {
            for (int column = 0; column < Width; column++)
            {
                for (int line = 0; line < Height; line++)
                {
                    Characters[column, line] = new(accelerator.DefaultColor, accelerator.DefaultColor, ConsoleAccelerator.DefaultChar, false);
                }
            }
        }

        /// <summary>
        /// Ширина снимка. 
        /// </summary>
        private int Width => accelerator.Width;
        /// <summary>
        /// Высота снимка.
        /// </summary>
        private int Height => accelerator.Height;
    }
}
