/*
Простая неориентированная сущность.
И её прямые наследники.
Не двигается, не получает урон, не содержит анимаций, имеет единственную текстуру.
Не обязана размещаться на уровне взаимодействия.

2022-11-18 
*/

using PlatformConsole;
using Scge;


namespace TestGame
{

    /// <summary>
    /// Простая неориентированная сущность.
    /// Этот нужен лишь для построения более строгой иерархии.
    /// </summary>
    internal abstract class Simple : ConsoleEntity
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Simple(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Simple(List<Cell>? cells = null) : base(cells) { }
    }

    /// <summary>
    /// Песчаная поверхность. Простая неориентированная сущность.
    /// </summary>
    internal class SandGround: Simple, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal SandGround(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор. 
        /// </summary>
        internal SandGround(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов. Простая неориентированная сущность.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.SandSpriteSet[0]);
        }
    }

    /// <summary>
    /// Горы. Простая неориентированная сущность.
    /// </summary>
    internal class Mountine : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Mountine(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Mountine(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorObstacle;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.MountineSpriteSet[0]);
        }
    }

    /// <summary>
    /// Скалистая поверхность. Простая неориентированная сущность.
    /// </summary>
    internal class RockGround : Simple, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockGround(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal RockGround(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[1]);
        }
    }

    /// <summary>
    /// Туман войны. Простая неориентированная сущность.
    /// </summary>
    internal class FogOfWar : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal FogOfWar(List<Cell>? cells) : base(cells)
        {
            MapColor = GameSeed.MapColorFogOfWar;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.FogOfWarSpriteSet[0]);
        }
    }

    /// <summary>
    /// Крепь пещеры. Простая неориентированная сущность.
    /// </summary>
    internal class CaveSupport : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CaveSupport(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CaveSupport(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[16]);
        }
    }

    /// <summary>
    /// Куча мусора. Простая неориентированная сущность.
    /// </summary>
    internal class Heap : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Heap(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Heap(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.WoodWorkSpriteSet[17]);
        }
    }

    /// <summary>
    /// Куча мусора. Простая неориентированная сущность.
    /// </summary>
    internal class Target : Simple, IDamageableOfShot, IDamageableOfKnife, IDamageableOfExplosion
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
        internal Target(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Target(List<Cell>? cells = null) : base(cells)
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
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.TargetSpriteSet[0]);
        }
    }

    /// <summary>
    /// Болото. Простая неориентированная сущность.
    /// </summary>
    internal class Marsh : Simple, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Marsh(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Marsh(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorWater;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.WaterSpriteSet[2]);
        }
    }

    /// <summary>
    /// Брод. Простая неориентированная сущность.
    /// </summary>
    internal class Ford : Simple, ILand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Ford(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Ford(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorGround;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.WaterSpriteSet[3]);
        }
    }

    /// <summary>
    /// Пропасть. Простая неориентированная сущность. 
    /// </summary>
    internal class Chasm : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Chasm(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Chasm(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorChasm;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.RockSpriteSet[2]);
        }
    }

    /// <summary>
    /// Ящик. Простая неориентированная сущность. 
    /// Имеет цвет.
    /// </summary>
    internal class Box : Simple, IDamageableOfExplosion
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
        /// Цвет.
        /// </summary>
        internal SimpleColor Color { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Box(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Box(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Color;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.BoxSpriteSet);
        }
    }

}
