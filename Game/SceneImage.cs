/*
Образ сцены.
Абстрактная схема уровня. Конкретное наполнение объектами не регламентировано.

2022-12-17 
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Образ сцены.
    /// </summary>
    internal class SceneImage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal SceneImage()
        {
            Items = new byte[,] {{0}};
        }

        /// <summary>
        /// Ширина.
        /// </summary>
        internal int Width => Items.GetUpperBound(1) + 1;

        /// <summary>
        /// Высота.
        /// </summary>
        internal int Height => Items.GetUpperBound(0) + 1;

        /// <summary>
        /// Прямоугольник.
        /// </summary>
        internal Rectangle Rectangle => new(Width, Height);

        /// <summary>
        /// Образ.
        /// </summary>
        internal byte[,] Items { get; private protected set; }
    }
}
