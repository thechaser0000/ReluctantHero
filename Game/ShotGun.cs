/*
Огнестрельное оружие. 

2022-11
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Огнестрельное оружие.
    /// </summary>
    internal class ShotGun
    {
        /// <summary>
        /// Осуществляется атака.
        /// </summary>
        internal bool IsAttack { get; set; }

        /// <summary>
        /// Патроны.
        /// </summary>
        internal Characteristic Ammo { get; init; }

        /// <summary>
        /// Задержка перед повторным выстрелом.
        /// (в тиках)
        /// </summary>
        internal Characteristic Delay { get; init; }

        /// <summary>
        /// Текущий счётчик задержек.
        /// (Выстрел не произойдёт пока счётчик не обнулится.
        /// Каждый тик счётчик декрементируется.
        /// После выстрела счётчик устанавливается в Delay).
        /// </summary>
        internal int DelayCounter { get; private set; }

        /// <summary>
        /// Урон.
        /// </summary>
        internal Characteristic Damage { get; init; }

        /// <summary>
        /// Бесконечные боеприпасы.
        /// (Не расходуются при стрельбе, стрельба возможна при любом количестве боеприпаов).
        /// </summary>
        internal bool IsInfinityAmmo { get; set; }

        /// <summary>
        /// Включено.
        /// Разрешает или запрещает попытку стрелять (не исключает остальных проверок).
        /// </summary>
        internal bool IsEnabled { get; set; }
        
        /// <summary>
        /// В данный момент выстрел разрешен.
        /// (Оружие включено, есть патроны, нет задержки)
        /// </summary>
        internal bool AllowAttack => IsEnabled && (!Ammo.IsMinimum || IsInfinityAmmo) && (0 == DelayCounter);

        /// <summary>
        /// Огонь.
        /// </summary>
        internal bool Attack()
        {
            if (AllowAttack)
            {
                Cell neighbor = Owner.Cell.GetNeighbor(Owner.Direction);

                if (neighbor is not null && neighbor.IsEmpty)
                // Создаем выстрел, если возможно: есть патроны, есть свободная клетка впереди, нажата кнопка выстрела.
                {
                    IsAttack = true;

                    Shot shot = new(neighbor, Owner.Direction, Damage);
                    Owner.InvokeEntityCreated(Owner, shot);
                    
                    if (!IsInfinityAmmo)
                    // Тратим боеприпасы, если они не бесконечные.
                    {
                        Ammo.Decrease();
                    }

                    // Устанавливаем задержку выстрела.
                    DelayCounter = Delay.Value;

                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Владелец.
        /// </summary>
        private Entity Owner { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ShotGun(Entity owner, int ammo, int maxAmmo, int shotDamage)
        {
            Owner = owner;
            Ammo = new(ammo, maxAmmo);
            Damage = new(shotDamage, shotDamage);
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal virtual void DoCheck()
        {
            // уменьшаем счётчик задержки каждый такт.
            DelayCounter -= 0 < DelayCounter ? 1 : 0;
        }
    }
}
