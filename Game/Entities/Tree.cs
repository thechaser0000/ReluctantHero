/*
Дерево.
Наследуется от простой неориентированной сущности.
Не двигается, не получает урон.
Есть периодическая анимация.

2022-12-01 
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Озеро.
    /// </summary>
    internal class Tree : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Tree(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Tree(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorThickets;
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            base.DoCheck();

            // Раз в 5 тиков меняем индекс спрайта.
            if (Engine.IsNumberedTick(5))
            {
                SpriteSelector.SelectedIndex = SpriteSelector.SelectedIndex == 1 ? 0 : 1;
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.AddRange(Game.SpriteLibrary.TreeSpriteSet);
        }
    }
}
