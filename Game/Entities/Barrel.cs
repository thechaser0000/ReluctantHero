/*
Ящик. Простая неориентированная сущность. 
Взрывается при получении огнестрельного ущерба.

2023-01-05
*/

using PlatformConsole;
using Scge;


namespace TestGame
{
    /// <summary>
    /// Простой цвет.
    /// </summary>
    internal enum SimpleColor
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Gray = 3,
        Yellow = 4,
        Cyan = 5,
    }

    /// <summary>
    /// Бочка. Простая неориентированная сущность. 
    /// (При получении огнестрельного ущерба взрывается с нанесением урона. Имеет цвет.)
    /// </summary>
    internal class Barrel : Simple, IDamageableOfShot, IDamageableOfExplosion, IExploding
    {
        /// <summary>
        /// Слой взрыва.
        /// </summary>
        internal Layer ExplosionsLayer { get; private set; }

        /// <summary>
        /// Цвет.
        /// </summary>
        internal SimpleColor Color { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Barrel(Layer explosionsLayer, Cell cell) : this(explosionsLayer, new List<Cell>() { cell }) { }

        private bool _allowBlowUp;

        /// <summary>
        /// Разрешить взрыв.
        /// </summary>
        internal bool AllowBlowUp {
            get => _allowBlowUp; 
            
            set
            {
                Health.IsInfinity = !value;
                _allowBlowUp = value;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Barrel(Layer explosionsLayer, List<Cell>? cells = null) : base(cells)
        {
            AllowBlowUp = true;
            ExplosionsLayer = explosionsLayer;
            MapColor = GameSeed.MapColorBuilding;
            Health.Maximum = 1;
            Health.Restore();
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Color;
        }

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
                if (IsDead)
                {
                    if (AllowBlowUp)
                    {
                        BlowUp();
                    }
                    NeedDelete = value;
                }
            }
        }

        /// <summary>
        /// Взорвать.
        /// </summary>
        internal void BlowUp()
        {
            // Находим соседей и наносим им урон
            List<Cell> neighbors = Cell.GetNeighbors();
            foreach (Cell cell in neighbors)
            {
                if (!cell.IsEmpty && cell.Entity is IDamageableOfExplosion entity)
                {
                    entity.Damage(GameSeed.BarrelExplosiveDamage);
                }
            }

            // Создаём взрыв в той же точке, но в другом слое.
            Explosion explosion = new(ExplosionsLayer.Cells[Cell.Index]);
            explosion.BlowUp();
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.BarrelSpriteSet);
        }
    }

}
