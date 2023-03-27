/*
Спрайт.
Проста сущность без поведения, но с отрисовкой.
 
2023
*/


using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Спрайт.
    /// </summary>
    internal class Sprite : Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Sprite(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Sprite(List<Cell>? cells = null) : base(cells) { }

        /// <summary>
        /// Индекс спрайта.
        /// </summary>
        internal int SpriteIndex
        {
            get => SpriteSelector.SelectedIndex;
            set
            {
                SpriteSelector.SelectedIndex = value;
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.HeroSpriteSet);
        }
    }

    /// <summary>
    /// Логотип.
    /// </summary>
    internal class Logo : Sprite
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Logo(Cell cell) : base(cell)
        {
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.LogoSpriteSet);
        }
    }

    /// <summary>
    /// Квадрат.
    /// </summary>
    internal class Square : Sprite
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Square(Cell cell) : this(new List<Cell>() { cell }) { }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Square(List<Cell>? cells = null) : base(cells) { }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.DefaultSpriteSet);
        }
    }

    /// <summary>
    /// Тайтл.
    /// </summary>
    internal class Title : Sprite
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Title(Cell cell) : base(cell)
        {
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.TitleSpriteSet);
        }
    }

}
