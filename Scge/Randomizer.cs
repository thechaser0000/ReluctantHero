/*
Статичный класс для удобной герерации случайных данных.
 
2022-10
*/

using TestGame;

namespace Scge
{
    /// <summary>
    /// Рандомизатор
    /// </summary>
    internal static class Randomizer
    {
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        internal static Random Random { get; } = new();

        /// <summary>
        /// Возвращает целое число из диапазона (включительно)
        /// (Резултат может быть отрицательным)
        /// </summary>
        internal static int RandomBetwen(int from, int to) => Random.Next(from - from, - from + ++to  ) + from;

        /// <summary>
        /// Возвращает целое число до указанного предела (не включительно)
        /// </summary>
        internal static int RandomTo(int to) => Random.Next(to);

        /// <summary>
        /// Возвращает случайное направление.
        /// (Может вернуть None)
        /// </summary>
        internal static Direction RandomDirection => (Direction)Randomizer.Random.Next(5);

        /// <summary>
        /// Возвращает случайнвй простой цвет.
        /// </summary>
        internal static SimpleColor RandomSimpleColor => (SimpleColor)Randomizer.Random.Next(6);

        /// <summary>
        /// Возвращает случайное гарантированное направление.
        /// (Не может вернуть None)
        /// </summary>
        internal static Direction RandomAssuredDirection => (Direction)Randomizer.Random.Next(4) + 1;

        /// <summary>
        /// Возвращает истину с указанной вероятностью (1-base)
        /// </summary>
        internal static bool ItIsTrue(double probability)
        {
            return 1 - probability <= Random.NextDouble();
        }

        /// <summary>
        /// Возвращает истину с вероятностью 50%.
        /// </summary>
        internal static bool ItIsTrue()
        {
            return 0 == RandomBetwen(0, 1);
        }

        /// <summary>
        /// Возвращает +1 или -1 с ероятностью 50%.
        /// </summary>
        internal static int RandomSign()
        {
            return ItIsTrue() ? -1 : 1;
        }

    }
}
