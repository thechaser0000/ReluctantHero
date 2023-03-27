/*
Библиотека консольных спрайтов.

Содержит несколько наборов спрайтов. Эти наборы могут использоваться как угодно через ConsoleSpriteSelector и собраны здесь лишь для удобства повторного использования.
Изначально содержит только набор спрайтов по умолчанию, в наследниках следует добавлять свои наборы.

2022-11-24
*/

namespace PlatformConsole
{
    /// <summary>
    /// Библиотека консольных спрайтов.
    /// </summary>
    internal class ConsoleSpriteLibrary
    {
        /// <summary>
        /// Ширина спрайта по умолчанию.
        /// </summary>
        internal int DefaultWidth { get; init; }
        
        /// <summary>
        /// Высота спрайта по умолчанию.
        /// </summary>
        internal int DefaultHeight { get; init; }

        /// <summary>
        /// Набор спрайтов по умолчанию.
        /// </summary>
        internal List<ConsoleSprite> DefaultSpriteSet { get; private set; }

        /// <summary>
        /// Конструктор.
        /// (При наследовании сначала следует вызывать свой конструктор, а потом базовый. Либо реализовывать свой с вызовом Generate()).
        /// </summary>
        internal ConsoleSpriteLibrary(int defaultWidth, int defaultHeight)
        {
            DefaultHeight = defaultHeight;
            DefaultWidth = defaultWidth;

            Generate();
        }

        /// <summary>
        /// Создать и сгенерировать библиотеку.
        /// </summary>
        internal virtual void Generate()
        {
            DefaultSpriteSet = new List<ConsoleSprite>();

            // Заполняем набор спрайтов по умолчанию простой заливкой 16 цветов.
            for (int color = 0; color < 16; color++)
            {
                ConsoleSprite sprite = new(DefaultWidth, DefaultHeight);
                sprite.FillImage((ConsoleColor)color, (ConsoleColor)color, ConsoleAccelerator.DefaultChar);

                DefaultSpriteSet.Add(sprite);
            }
        }

    }
}
