/*
Консольная сущность.
Добавляет цвета фона и символа.
 
2022-11-10
*/

using Scge;

namespace PlatformConsole
{
    /// <summary>
    /// Консольная сущность.
    /// </summary>
    //[Obsolete]
    internal class ConsoleEntity: Entity
    {
        /// <summary>
        /// Селектор спрайта.
        /// </summary>
        internal ConsoleSpriteSelector SpriteSelector { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleEntity(Cell cell) : this(new List<Cell>() { cell })  { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleEntity(List<Cell>? cells = null) : base(cells)
        {
            // Создаём образ.
            SpriteSelector = new ConsoleSpriteSelector(FillProcedure, 0);
            SpriteSelector.SpriteSelection += SpriteSelection;
        }

        /// <summary>
        /// Очистить.
        /// (Отвязываем обработчик выбора спрайта).
        /// </summary>
        internal override void Clear()
        {
            base.Clear();
            SpriteSelector.SpriteSelection -= SpriteSelection;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected virtual void FillProcedure(List<ConsoleSprite> sprites) {}

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal virtual int SpriteSelection(int currentSelectedIndex)
        {
            return currentSelectedIndex;
        }

        /// <summary>
        /// Цвет на карте.
        /// </summary>
        internal ConsoleColor MapColor { get; set; }

        /// <summary>
        /// Цвет символа.
        /// </summary>
        [Obsolete]
        internal ConsoleColor ForegroundColor { get; set; } 
    }
}
