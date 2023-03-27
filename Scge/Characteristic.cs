/*
Какой-то показатель, характеристика. (изначально было здоровье, но какая разница?).
Содержит максимум, текущее значение и разные полезные методы.

2022-11-09  
*/

namespace Scge
{
    /// <summary>
    /// Показатель.
    /// </summary>
    internal class Characteristic
    {
        /// <summary>
        /// Отрезок от минимума до максимума.
        /// </summary>
        private Segment _segment;

        private int _value;

        /// <summary>
        /// Бесконечная характеристика.
        /// (Не тратится, всегда IsMaximum)
        /// </summary>
        internal bool IsInfinity { get; set; } = false;

        /// <summary>
        /// Текущий уровень характеристики (всегда в диапазоне от минимума до максимума).
        /// </summary>
        internal int Value
        { 
            get => _value;
            set => _value = FloatComparer.EnsureRange(value, Minimum, Maximum);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Characteristic(int value, int maximum, int minimum = 0)
        {
            _segment = new(minimum, maximum);
            Value = value;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Characteristic(int value)
        {
            _segment = new(0, value);
            Value = value;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Characteristic() : this(0, 0, 0)
        {
        }

        /// <summary>
        /// Перегрузка приведения к строке.ы
        /// </summary>
        public override string ToString()
        {
            return $"{Minimum}..{Value}..{Maximum}";
        }

        /// <summary>
        /// Процентное значение характеристики относительно диапазона.
        /// </summary>
        internal float ValuePercent => IsInfinity ? 100f : 100f * Value / _segment.Length;

        /// <summary>
        /// Значение характеристики минимально.
        /// </summary>
        internal bool IsMinimum => IsInfinity ? false : Value == Minimum;

        /// <summary>
        /// Значение характеристики максимально.
        /// </summary>
        internal bool IsMaximum => IsInfinity ? true : Value == Maximum;

        /// <summary>
        /// Максимальное значение характеристики.
        /// </summary>
        internal int Maximum
        {
            get => _segment.Stop;
            set
            {
                _segment = new(Minimum, value);
            }
        }

        /// <summary>
        /// Минимальное значение характеристики.
        /// </summary>
        internal int Minimum
        {
            get => _segment.Start;
            set
            {
                _segment = new(value, Maximum);
            }
        }

        /// <summary>
        /// Восстановить характеристику полностью.
        /// (Установить в максимум)
        /// </summary>
        internal void Restore()
        {
            Value = Maximum;
        }

        /// <summary>
        /// Сбросить характеристику полностью.
        /// (Установить в минимум)
        /// </summary>
        internal void Reset()
        {
            Value = Minimum;
        }

        /// <summary>
        /// Увеличить характеристику.
        /// </summary>
        internal void Increase(int value = 1)
        {
            // Увеличиваем характеристику не выше максимума.
            Increment(value);
        }

        /// <summary>
        /// Прирастить характеристику (с учетом знака).
        /// </summary>
        private void Increment(int value)
        {
            // Увеличиваем характеристику не выше максимума.
            // Уменьшаем характеристику не ниже минимума.
            if (!IsInfinity)
            {
                Value += value;
            }
        }

        /// <summary>
        /// Уменьшить характеристику.
        /// </summary>
        internal void Decrease(int value = 1)
        {
            // Уменьшаем характеристику не ниже минимума.
            Increment(-value);
        }
    }
}
