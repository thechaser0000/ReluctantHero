/*
Картинка.
Отображает спрайт в указанной позиции.

2022-12-26
 */

using Scge;

namespace PlatformConsole
{
    /// <summary>
    /// Картинка.
    /// </summary>
    internal class Picture
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Picture(ConsoleSprite sprite)
        {
            IsVisible = true;
            ConsoleSpriteSelector = new();
            ConsoleSpriteSelector.Items.Add(sprite);
            ConsoleSpriteSelector.SelectedIndex = 0;
        }

        /// <summary>
        /// Тэг для специального использования. 
        /// </summary>
        internal object? Tag;

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Picture(List<ConsoleSprite> sprites)
        {
            IsVisible = true;
            ConsoleSpriteSelector = new();
            ConsoleSpriteSelector.Items.AddRange(sprites);
            SpriteIndex = 0;
        }

        /// <summary>
        /// Селектор спрайтов.
        /// </summary>
        internal ConsoleSpriteSelector ConsoleSpriteSelector { get; private set; }

        /// <summary>
        /// Индекс спрайта.
        /// </summary>
        internal int SpriteIndex
        {
            get => ConsoleSpriteSelector.SelectedIndex;
            set => ConsoleSpriteSelector.SelectedIndex = value;
        }

        /// <summary>
        /// Циклически перебирает спрайты.
        /// </summary>
        internal void NextSprite()
        {
            if (SpriteIndex < SpriteCount - 1)
            {
                SpriteIndex++;
            }
            else
            {
                SpriteIndex = 0;
            }
        }

        /// <summary>
        /// Количество спрайтов.
        /// </summary>
        internal int SpriteCount => ConsoleSpriteSelector.Items.Count;

        // REF Перейти на Point вместо/вместе с Left/Top 

        /// <summary>
        /// Спрайт.
        /// </summary>
        internal ConsoleSprite Sprite => ConsoleSpriteSelector.Selected;

        /// <summary>
        /// Крайняя верхняя линия.
        /// </summary>
        internal int Top { get; set; }

        /// <summary>
        /// Крайний левый столбец.
        /// </summary>
        internal int Left { get; set; }

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        internal Rectangle Rectangle => new(Left, Top, Left + Width - 1, Top + Height - 1);

        /// <summary>
        /// Высота в строках.
        /// </summary>
        internal int Height => Sprite.Height;

        /// <summary>
        /// Ширина.
        /// </summary>
        internal int Width => Sprite.Width;

        /// <summary>
        /// Видим.
        /// </summary>
        internal bool IsVisible { get; set; }
    }
}
