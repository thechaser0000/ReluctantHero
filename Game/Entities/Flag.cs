/*
Контрольная точка.
Наследуется от простой неориентированной сущности.
Не двигается, не получает урон.
Есть периодическая анимация.

2022-11-18 
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Контрольная точка.
    /// </summary>
    internal class Flag : Simple, IDamageableOfShot, IDamageableOfKnife, IDamageableOfExplosion
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
        internal Flag(Cell cell) : base(cell)
        {
            MapColor = GameSeed.MapColorBuilding;
            Health.Maximum = GameSeed.DoorHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            // флаг периодически колышется
            int remainder = Engine.TickCount % 20;
            return remainder <= 3 ? remainder : 4;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.CheckpointSpriteSet);
        }
    }
}
