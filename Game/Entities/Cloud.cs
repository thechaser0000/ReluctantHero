/*
Облако.
Наследуется от простой неориентированной сущности.
Двигается, не получает урон.

2022-11-20 
*/


using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Облако.
    /// </summary>
    internal class Cloud : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Cloud(List<Cell>? cells) : base(cells)
        {
            MapColor = GameSeed.UnknownColor;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.Add(Game.SpriteLibrary.CloudSpriteSet[0]);
        }

        /// <summary>
        /// Выполнить перемещение.
        /// </summary>
        internal override void DoMove()
        {
            // двигаем облака 1 раз в N тактов.
            if (Engine.IsNumberedTick(3))
            {
                Move(Direction.Right, true);
            };
            if (Engine.IsNumberedTick(12))
            {
                Move(Direction.Down, true);
            };

            base.DoMove();
        }
    }
}
