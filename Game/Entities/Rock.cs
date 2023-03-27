/*
Массив скал. Простая неориентированная сущность.
Имеет светлый и темный варианты.

2023-01-13
*/

using PlatformConsole;
using Scge;


namespace TestGame
{
    /// <summary>
    /// Массив скал. Простая неориентированная сущность.
    /// </summary>
    internal class Rock : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Rock(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Тёмный.
        /// </summary>
        internal bool Dark { get; set; }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return Dark? 1 : 0;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Rock(List<Cell>? cells = null) : base(cells)
        {
            Dark = false;
            MapColor = GameSeed.MapColorObstacle;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[0]);
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[7]);
        }
    }

}
