/*
Полоса шоссе. Простая ориентированная сущность. 
Не двигается, не получает урон, не содержит анимаций, имеет шесть текстур - для комбинаций горизонтальной/вертикальной ориентаций и левой/центральной/правой полос .

2022-12-02
*/

using PlatformConsole;
using Scge;

namespace TestGame
{

    /// <summary>
    /// Позиция полосы шоссе.
    /// </summary>
    internal enum HighwayLinePosition
    {
        Left = 0,
        Center = 1,
        Right = 2,
    }

    /// <summary>
    /// Полоса шоссе. Простая ориентированная сущность. Спрайт зависит от позиции полосы и ориентации.
    /// </summary>
    internal class HighwayLine : Simple, ILand
    {
        /// <summary>
        /// Вид полосы.
        /// </summary>
        internal HighwayLinePosition Position { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal HighwayLine(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal HighwayLine(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            // картинка зависит от ориентации
            switch (Direction)
            {
                case Direction.Left or Direction.Right:
                    // =
                    return (int)Position * 2;
                default:
                    // ||
                    return (int)Position * 2 + 1; 
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[2]);
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[3]);
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[4]);
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[5]);
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[6]);
            sprites.Add(Game.SpriteLibrary.RoadSpriteSet[7]);
        }
    }
}
