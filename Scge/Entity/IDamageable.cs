/*
Интерфейс "Повреждаемый".
Обладатель этого интерфейса получает ущерб и может погибнуть.

2022-11-16
*/

namespace Scge
{
    /// <summary>
    /// Интерфейс "Повреждаемый".
    /// </summary>
    internal interface IDamageable
    {
        /// <summary>
        /// Здоровье.
        /// </summary>
        public Characteristic Health { get; init; }

        /// <summary>
        /// Нанести повреждение.
        /// </summary>
        internal void Damage(int value)
        {
            Health.Decrease(value);
        }

        /// <summary>
        /// Умер.
        /// </summary>
        internal bool IsDead { get; set; }

        /// <summary>
        /// Проверить повреждение.
        /// </summary>
        internal void CheckDamage()
        {
            IDamageable damaged = this as IDamageable;

            if (damaged.Health.IsMinimum)
            {
                IsDead = true;
//                (damaged as Entity).NeedDelete = true;
            }
        }
    }
}
