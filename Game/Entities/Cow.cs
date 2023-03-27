/*
Корова.
Мирный NPC

2022-12-20
*/

using PlatformConsole;
using Scge;
using System;


namespace TestGame
{
    /// <summary>
    /// Корова.
    /// </summary>
    internal class Cow : Simple, IDamageableOfShot, IDamageableOfKnife, IMoveable, IDamageableOfExplosion
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
        internal Cow(Cell cell) : base(cell)
        {
            MapColor = GameSeed.MapColorNpc;
            Health.Maximum = GameSeed.ChaserHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.CowSpriteSet);
        }

        /// <summary>
        /// Выполнить тик игрового цикла.
        /// </summary>
        internal override void DoMove()
        {
            // враг перемещается в случайном направлении или стоит на месте
            if (Randomizer.ItIsTrue(GameSeed.CowStrayProbability))
            // Корова двигается в ту сторону, куда повернута
            {
                this.Move(Direction);
            }
            else
            {
                if (Randomizer.ItIsTrue(GameSeed.CowTurnProbability))
                // Корова куда-то поворачивает
                {
                    this.Direction = Randomizer.RandomAssuredDirection;
                }
            }

            base.DoMove();
        }
    }
}
