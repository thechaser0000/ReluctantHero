/*
Часть автомобиля. Простая направленная сущность. 
Не двигается, получает урон от взрыва, не содержит анимаций, имеет 15 текстур - для комбинаций направления и передней/центральной/задней частей .

2022-12-02
*/

using PlatformConsole;
using Scge;

namespace TestGame
{

    /// <summary>
    /// Позиция части автомобиля.
    /// </summary>
    internal enum CarPartPosition
    {
        Front = 0,
        Center = 1,
        Rear = 2,
    }

    /// <summary>
    /// Тип кузова автомобиля.
    /// </summary>
    internal enum CarBodyStyle
    {
        Sedan = 0,
        Pickup = 1,
    }

    /// <summary>
    /// Часть машины. Простая ориентированная сущность. Спрайт зависит от позиции полосы и ориентации.
    /// Взрываемый и взрывающийся.
    /// </summary>
    internal class CarPart : Directional, IDamageableOfExplosion
    {
        /// <summary>
        /// Слой взрыва.
        /// </summary>
        internal Layer ExplosionsLayer { get; private set; }

        /// <summary>
        /// Тип кузова автомобиля.
        /// </summary>
        internal CarBodyStyle BodyStyle { get; set; }

        /// <summary>
        /// Вид полосы.
        /// </summary>
        internal CarPartPosition Position { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CarPart(Layer explosionsLayer, Cell cell) : this(explosionsLayer, new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CarPart(Layer explosionsLayer, List<Cell>? cells = null) : base(cells)
        {
            ExplosionsLayer = explosionsLayer;
            MapColor = GameSeed.MapColorBuilding;
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
                    BlowUp();
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
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction + (int)Position * 5 + (int)BodyStyle * 15;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.CarSedanSpriteSet);
            sprites.AddRange(Game.SpriteLibrary.CarPickupSpriteSet);
        }
    }
}
