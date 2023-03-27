/*
Трофей.
Наследуется от простой неориентированной сущности.
Не двигается, не получает урон.
Нет анимации.

2022-11-18 
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Трофей.
    /// </summary>
    internal class Trophy : Simple
    {
        /// <summary>
        /// Количество содержимого.
        /// </summary>
        internal int Capacity { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Trophy(Cell cell) : base(cell)
        {
            MapColor = GameSeed.MapColorTrophy;
        }
    }

    /// <summary>
    /// Трофей - аптечка.
    /// </summary>
    internal class HealthTrophy : Trophy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal HealthTrophy(Cell cell) : base(cell)
        {
            Capacity = GameSeed.TrophyHealthCapacity;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.TrophySpriteSet[3]);
        }
    }

    /// <summary>
    /// Трофей - патроны.
    /// </summary>
    internal class AmmoTrophy : Trophy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal AmmoTrophy(Cell cell) : base(cell)
        {
            Capacity = GameSeed.TrophyAmmoCapaciy;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.TrophySpriteSet[4]);
        }
    }

    /// <summary>
    /// Трофей - огнестрельное оружие.
    /// </summary>
    internal class ShotGunTrophy : Trophy
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ShotGunTrophy(Cell cell) : base(cell)
        {
            Capacity = 1;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.TrophySpriteSet[2]);
        }
    }
}
