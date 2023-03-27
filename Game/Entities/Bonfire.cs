/*
Костер. 
Анимированная сущность.
Не содержит дополнительной логики.

2023-01-09 
*/


using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Костер.
    /// </summary>
    internal class Bonfire : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Bonfire(Cell cell) : this(new List<Cell>() { cell }) { }
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Bonfire(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorBuilding;
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            base.DoCheck();

            // Каждый тик меняем индекс спрайта.
            if (SpriteSelector.SelectedIndex < (SpriteSelector.Items.Count - 1))
            {
                SpriteSelector.SelectedIndex++;
            }
            else
            // начинаем сначала.
            {
                SpriteSelector.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов. Простая неориентированная сущность.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.BonfireSpriteSet);
        }

    }
}
