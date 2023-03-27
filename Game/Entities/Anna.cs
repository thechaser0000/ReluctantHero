/*
Жена героя.
Ведет себя как преследователь, но не атакует.

2022-12-13 
 */

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Анна.
    /// </summary>
    internal class Anna : Chaser
    {
        /// <summary>
        /// Умер.
        /// </summary>
        public override bool IsDead { get; set; }

        /// <summary>
        /// Жив.
        /// </summary>
        internal bool IsAlive => !IsDead;

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            if (IsAlive)
            {
                return (int)Direction;
            }
            else
            {
                return 5;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Anna(Cell cell) : base(cell)
        {
            Knife.IsEnabled = false;
            MapColor = GameSeed.MapColorAssociate;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.AnnaSpriteSet);
        }
    }
}
