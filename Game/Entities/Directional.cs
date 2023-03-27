/*
Простая направленная сущность. 
И её прямые наследники.
Не двигается, не получает урон, не содержит анимаций, имеет 5 текстур - по количеству направлений:
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
Нулевая текстура, как правило, фиктивная и не используется.
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
    internal abstract class Directional : ConsoleEntity
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Directional(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Directional(List<Cell>? cells = null) : base(cells)
        {
            // Задаем случайное направление.
            Direction = Randomizer.RandomAssuredDirection;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction;
        }
    }

    /// <summary>
    /// Вход в пещеру. Простая направленная сущность. 
    /// </summary>
    internal class CaveEntrance : Directional
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CaveEntrance(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CaveEntrance(List<Cell>? cells = null) : base(cells) { }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.AddRange(Game.SpriteLibrary.MountineSpriteSet);
        }
    }

    /// <summary>
    /// Девушка. Простая направленная сущность. 
    /// </summary>
    internal class Lady : Directional
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Lady(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Lady(List<Cell>? cells = null) : base(cells) { }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.AddRange(Game.SpriteLibrary.LadySpriteSet);
        }
    }

    /// <summary>
    /// Бандит. Простая направленная сущность. 
    /// </summary>
    internal class Bandit : Directional
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Bandit(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Bandit(List<Cell>? cells = null) : base(cells) { }

        // Хардкожим удар.
        internal bool IsAttack { get; set; }
        //{
        //    set => SpriteSelector.SelectedIndex = 5 + (int)Direction;
        //}
        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction + (IsAttack ? 5 : 0);
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.AddRange(Game.SpriteLibrary.ChaserSpriteSet);
        }
    }

    /// <summary>
    /// Бандит. Простая направленная сущность. 
    /// </summary>
    internal class Door : Directional, IDamageableOfShot, IDamageableOfExplosion
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
        internal Door(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Door(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
            Health.Maximum = GameSeed.DoorHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[11]);
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[12]);
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[13]);
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[14]);
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[15]);
        }
    }

}
