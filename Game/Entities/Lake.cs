/*
Озеро.
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
    /// Озеро.
    /// </summary>
    internal class Lake : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Lake(List<Cell>? cells = null) : base(cells)
        {
            MapColor = GameSeed.MapColorWater;
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            base.DoCheck();

            // Раз в 10 тиков меняем индекс спрайта.
            if (Engine.IsNumberedTick(10))
            {
                SpriteSelector.SelectedIndex = SpriteSelector.SelectedIndex == 1 ? 0 : 1;
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            sprites.AddRange(Game.SpriteLibrary.WaterSpriteSet);
        }
    }
}
