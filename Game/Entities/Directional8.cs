/*
Деревянный забор. Простая 8-направленная сущность.
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        LeftUp = 5,
        LeftDown = 6,
        RightUp = 7,
        RightDown = 8
Не двигается, не получает урон, не содержит анимаций, имеет несколько текстур, зависящих от направления.

2022-12-02  
*/

using PlatformConsole;
using Scge;

namespace TestGame
{

    /// <summary>
    /// Простая 8-направленная сущность.
    /// </summary>
    internal class Directional8 : Simple
    {
        /// <summary>
        /// Направление 8-позиционное.
        /// </summary>
        internal Direction8 Direction8 { get; set; }

        /// <summary>
        /// Направление 4-позиционное.
        /// </summary>
        internal override Direction Direction
        {
            get
            {
                // "Изящно" преобразуем 8-позиционное направление в 4-позиционное.
                return Direction8 switch
                {
                    Direction8.Left or Direction8.LeftUp => Direction.Left,
                    Direction8.Up or Direction8.RightUp => Direction.Up,
                    Direction8.Right or Direction8.RightDown => Direction.Right,
                    Direction8.Down or Direction8.LeftDown => Direction.Down,
                    _ => Direction.None,
                };
            }

            // Преобразуем 4-позиционное направление в 8-позиционное.
            set => Direction8 = (Direction8)value;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Directional8(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Directional8(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction8;
        }
    }

    /// <summary>
    /// Деревянный забор. Простая сложноориентированная сущность.
    /// </summary>
    internal class WoodFence : Directional8
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal WoodFence(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal WoodFence(List<Cell>? cells = null) : base(cells) { }
        
        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            for (int index = 2; index <= 10; index++)
            {
                sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[index]);
            }
        }
    }
}
