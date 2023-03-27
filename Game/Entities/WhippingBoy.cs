/*
Мальчик для битья. Тестовый враг. 
Наследуется от простой неориентированной сущности.

2022-11-22 
*/

using PlatformConsole;
using Scge;
using TestGame;

namespace TestEntity
{
    /// <summary>
    /// Мальчик для битья
    /// </summary>
    internal class WhippingBoy : Simple, IDamageableOfShot, IDamageableOfKnife, IDamageableOfExplosion
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
        /// Конструктор
        /// </summary>
        internal WhippingBoy(Cell cell) : base(cell)
        {
            Health.Maximum = GameSeed.ChaserHealth;
            Health.Value = Health.Maximum;
            MapColor = GameSeed.MapColorEnemy;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.TargetSpriteSet);
        }

    }
}
