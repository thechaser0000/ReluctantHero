/*
Нож. 
Наносит ущерб оказавшемуся перед владельцем носителю IKnifeDamaged.
  
2022-11-21
 */

using Scge;

namespace TestGame
{
    /// <summary>
    /// Нож.
    /// </summary>
    internal class Knife
    {
        /// <summary>
        /// Включено.
        /// Разрешает или запрещает попытку ударить ножом (не исключает остальных проверок).
        /// </summary>
        ///  REF это и подобные свойства нужно бы вынести в общий клэсс
        internal bool IsEnabled { get; set; }

        /// <summary>
        /// Задержка перед повторным ударом.
        /// (в тиках)
        /// </summary>
        internal Characteristic Delay { get; init; }

        /// <summary>
        /// Текущий счётчик задержек.
        /// (Удар не произойдёт пока счётчик не обнулится.
        /// Каждый тик счётчик декрементируется.
        /// После удара счётчик устанавливается в Delay).
        /// </summary>
        internal int DelayCounter { get; private set; }

        /// <summary>
        /// Осуществляется атака.
        /// </summary>
        internal bool IsAttack { get; set; }

        /// <summary>
        /// Владелец.
        /// </summary>
        private Entity Owner { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Knife(Entity owner, int knifeDamage)
        {
            Owner = owner;
            Damage = new(knifeDamage, knifeDamage);
            Delay = new();
        }

        /// <summary>
        /// В данный момент атака разрешена.
        /// (Оружие включено, есть патроны, нет задержки)
        /// </summary>
        internal bool AllowAttack => IsEnabled && (0 == DelayCounter);

        /// <summary>
        /// Атака.
        /// </summary>
        internal bool Attack()
        {
            if (AllowAttack)
            {
                IsAttack = true;

                Cell neighbor = Owner.Cell.GetNeighbor(Owner.Direction);

                if (neighbor is not null && !neighbor.IsEmpty && neighbor.Entity is IDamageableOfKnife opponent)
                // Ударяем, если возможно, впереди принимающая удар сущность, нажата кнопка выстрела.
                {
                    // Наносим урон.
                    opponent.Damage(Damage.Value);

                    // Устанавливаем задержку выстрела.
                    DelayCounter = Delay.Value;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal virtual void DoCheck()
        {
            // уменьшаем счётчик задержки каждый такт.
            DelayCounter -= 0 < DelayCounter ? 1 : 0;
        }

        /// <summary>
        /// Урон.
        /// </summary>
        internal Characteristic Damage { get; init; }
    }
}
