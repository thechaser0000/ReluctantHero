/*
Простая ориентированная сущность. 
И её прямые наследники.
Не двигается, не получает урон, не содержит анимаций, имеет две текстуры - горизонтальную и вертикальную.
Не обязана размещаться на уровне взаимодействия.

2022-11-18 
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Простая ориентированная сущность.
    /// </summary>
    internal abstract class Oriented : ConsoleEntity
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Oriented(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Oriented(List<Cell>? cells = null) : base(cells) { }

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
                    return 0;
                default:
                    // ||
                    return 1;
            }
        }
    }

    /// <summary>
    /// Тропа на песчаной поверхности. Простая ориентированная сущность. 
    /// </summary>
    internal class SandPath : Oriented, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal SandPath(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal SandPath(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.SandSpriteSet[1]);
            sprites.Add(Game.SpriteLibrary.SandSpriteSet[2]);
        }
    }

    /// <summary>
    /// Cкальный мост. Простая ориентированная сущность. 
    /// </summary> 
    internal class RockBridge : Oriented
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockBridge(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockBridge(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.UnknownColor;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[5]);
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[6]);
        }
    }

    /// <summary>
    /// Cкальная тропа. Простая ориентированная сущность. 
    /// </summary>
    internal class RockPath : Oriented, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockPath(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockPath(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[3]);
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[4]);
        }
    }

    /// <summary>
    /// Деревянный мост. Простая ориентированная сущность.
    /// </summary>
    internal class WoodBridge : Oriented, ILand
    {
        /// <summary>
        /// Умер.
        /// (И подлежит удалению)
        /// </summary>
        public override bool IsDead
        {
            get => base.IsDead;
            set
            {
                base.IsDead = value;
                NeedDelete = value;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal WoodBridge(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal WoodBridge(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[0]);
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[1]);
        }
    }
}
