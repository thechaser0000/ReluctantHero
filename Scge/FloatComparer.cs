/*
Класс для удобного сравнения чисел с плавающей точкой (взят из другого проекта)
По факту - помойка. Когда-нибудь придётся рефакторить, но пока сойдёт и так.

2021-09
 */

namespace Scge
{
    /// <summary>
    /// Класс для удобного сравнения чисел с плавающей точкой. Постепенно оброс функционалом, но имя осталось. 
    /// </summary>
    public static class FloatComparer
    {
        /// <summary>
        /// Точность сравнения (0.001F)
        /// Есть float.Epsilon, но, на мой взгляд, оно очень мало
        /// </summary>
        public readonly static float Epsilon = 0.001F;

        /// <summary>
        /// Число четное
        /// </summary>
        public static bool IsEven(int value)
        {
            return (value % 2) == 0;
        }

        /// <summary>
        /// Число отрицательное
        /// </summary>
        public static bool IsNegative(int value)
        {
            return value < 0;
        }

        /// <summary>
        /// Возвращает значение втиснутое в указанный диапазон 
        /// </summary>
        public static int EnsureRange(int value, int from, int to)
        {
            return value < from ? from : to < value ? to : value;
        }

        /// <summary>
        /// Значение находится в указанном диапазоне
        /// </summary>
        public static bool InRange(float value, float from, float to)
        {
            return MoreOrEqual(from, value) && LessOrEqual(value, to);
        }

        /// <summary>
        /// Значение находится в указанном диапазоне
        /// </summary>
        public static bool InRange(int value, int from, int to)
        {
            return from <= value && value <= to;
        }

        /// <summary>
        /// Числа равны, если их абсолютная разность меньше или равна Epsilon
        /// </summary>
        public static bool Equal(float left, float right)
        {
            return Equal(left, right, Epsilon);
        }

        /// <summary>
        /// Числа равны, если их абсолютная разность меньше или равна epsilon
        /// </summary>
        public static bool Equal(float left, float right, float epsilon)
        {
            return Math.Abs(left - right) <= epsilon;
        }

        /// <summary>
        /// Число слева больше либо равно числу справа
        /// </summary>
        public static bool MoreOrEqual(float left, float right)
        {
            return MoreOrEqual(left, right, Epsilon);
        }

        /// <summary>
        /// Число слева меньше либо равно числу справа
        /// </summary>
        public static bool LessOrEqual(float left, float right)
        {
            return LessOrEqual(left, right, Epsilon);
        }

        /// <summary>
        /// Число слева больше либо равно числу справа
        /// </summary>
        public static bool MoreOrEqual(float left, float right, float epsilon)
        {
            return left + epsilon >= right;
        }

        /// <summary>
        /// Число слева меньше либо равно числу справа
        /// </summary>
        public static bool LessOrEqual(float left, float right, float epsilon)
        {
            return left <= right + epsilon;
        }

        /// <summary>
        /// Метод самотестирования. Вызывает исключение, если тесты не проходят.
        /// Необходимо дополнять тесты при добавлении новых методов 
        /// </summary>
        public static void SelfTest()
        {
            float a = 100;
            float b = 100 + Epsilon;
            float c = 100 + 2 * Epsilon;

            /*
            ///Просто тест быстрого форматирования  
            LogProcessor.AddSimple("FloatComparer SelfTest ");
            LogProcessor.AddSimple(0.000001234567890 + ": " + QickFormat(0.000001234567890F));
            LogProcessor.AddSimple(0.00001234567890 + ": " + QickFormat(0.00001234567890F));
            LogProcessor.AddSimple(0.0001234567890 + ": " + QickFormat(0.0001234567890F));
            LogProcessor.AddSimple(0.001234567890 + ": " + QickFormat(0.001234567890F));
            LogProcessor.AddSimple(0.01234567890 + ": " + QickFormat(0.01234567890F));
            LogProcessor.AddSimple(0.1234567890 + ": " + QickFormat(0.1234567890F));

            LogProcessor.AddSimple(1 + ": " + QickFormat(1));
            LogProcessor.AddSimple(10 + ": " + QickFormat(10));
            LogProcessor.AddSimple(100 + ": " + QickFormat(100));
            LogProcessor.AddSimple(1000 + ": " + QickFormat(1000));
            LogProcessor.AddSimple(10000 + ": " + QickFormat(10000));
            LogProcessor.AddSimple(100000 + ": " + QickFormat(100000));
            LogProcessor.AddSimple(1000000 + ": " + QickFormat(1000000));
            LogProcessor.AddSimple(10000000 + ": " + QickFormat(10000000));
            LogProcessor.AddSimple(100000000 + ": " + QickFormat(100000000));
            LogProcessor.AddSimple(1000000000 + ": " + QickFormat(1000000000));

            LogProcessor.AddSimple(1 + ": " + QickFormat(1));
            LogProcessor.AddSimple(11 + ": " + QickFormat(11));
            LogProcessor.AddSimple(101 + ": " + QickFormat(101));
            LogProcessor.AddSimple(1001 + ": " + QickFormat(1001));
            LogProcessor.AddSimple(10001 + ": " + QickFormat(10001));
            LogProcessor.AddSimple(100001 + ": " + QickFormat(100001));
            LogProcessor.AddSimple(1000001 + ": " + QickFormat(1000001));
            LogProcessor.AddSimple(10000001 + ": " + QickFormat(10000001));
            LogProcessor.AddSimple(100000001 + ": " + QickFormat(100000001));
            LogProcessor.AddSimple(1000000001 + ": " + QickFormat(1000000001));

            LogProcessor.AddSimple(9 + ": " + QickFormat(9));
            LogProcessor.AddSimple(99 + ": " + QickFormat(99));
            LogProcessor.AddSimple(999 + ": " + QickFormat(999));
            LogProcessor.AddSimple(9999 + ": " + QickFormat(9999));
            LogProcessor.AddSimple(99999 + ": " + QickFormat(99999));
            LogProcessor.AddSimple(999999 + ": " + QickFormat(999999));
            LogProcessor.AddSimple(9999999 + ": " + QickFormat(9999999));
            LogProcessor.AddSimple(99999999 + ": " + QickFormat(99999999));
            LogProcessor.AddSimple(999999999 + ": " + QickFormat(999999999));
            LogProcessor.AddSimple(9999999999 + ": " + QickFormat(9999999999));

            LogProcessor.AddSimple(1.234567890 + ": " + QickFormat(1.234567890F));
            LogProcessor.AddSimple(12.34567890F + ": " + QickFormat(12.34567890F));
            LogProcessor.AddSimple(123.4567890F + ": " + QickFormat(123.4567890F));
            LogProcessor.AddSimple(1234.567890F + ": " + QickFormat(1234.567890F));
            LogProcessor.AddSimple(12345.67890F + ": " + QickFormat(12345.67890F));
            LogProcessor.AddSimple(123456.7890F + ": " + QickFormat(123456.7890F));
            LogProcessor.AddSimple(1234567.890F + ": " + QickFormat(1234567.890F));
            LogProcessor.AddSimple(12345678.90F + ": " + QickFormat(12345678.90F));
            LogProcessor.AddSimple(123456789.0F + ": " + QickFormat(123456789.0F));
            LogProcessor.AddSimple(1234567890 + ": " + QickFormat(1234567890));

            LogProcessor.AddSimple(12345678901 + ": " + QickFormat(12345678901));
            LogProcessor.AddSimple(123456789012 + ": " + QickFormat(123456789012));
            LogProcessor.AddSimple(1234567890123 + ": " + QickFormat(1234567890123));
            LogProcessor.AddSimple(12345678901234 + ": " + QickFormat(12345678901234));
            LogProcessor.AddSimple(123456789012345 + ": " + QickFormat(123456789012345));
            LogProcessor.AddSimple(1234567890123456 + ": " + QickFormat(1234567890123456));
            LogProcessor.AddSimple(12345678901234567 + ": " + QickFormat(12345678901234567));
            LogProcessor.AddSimple(123456789012345678 + ": " + QickFormat(123456789012345678));
            LogProcessor.AddSimple(1234567890123456789 + ": " + QickFormat(1234567890123456789));
            LogProcessor.AddSimple(12345678901234567890 + ": " + QickFormat(12345678901234567890));

            LogProcessor.AddSimple(123456789E12 + ": " + QickFormat((float)123456789E12));
            LogProcessor.AddSimple(1234567890E12 + ": " + QickFormat((float)1234567890E12));
            LogProcessor.AddSimple(12345678901E12 + ": " + QickFormat((float)12345678901E12));
            LogProcessor.AddSimple(123456789012E12 + ": " + QickFormat((float)123456789012E12));
            LogProcessor.AddSimple(1234567890123E12 + ": " + QickFormat((float)1234567890123E12));
            LogProcessor.AddSimple(12345678901234E12 + ": " + QickFormat((float)12345678901234E12));
            LogProcessor.AddSimple(123456789012345E12 + ": " + QickFormat((float)123456789012345E12));
            LogProcessor.AddSimple(1234567890123456E12 + ": " + QickFormat((float)1234567890123456E12));
            LogProcessor.AddSimple(12345678901234567E12 + ": " + QickFormat((float)12345678901234567E12));
            LogProcessor.AddSimple(123456789012345678E12 + ": " + QickFormat((float)123456789012345678E12));
            */

            bool success = Equal(a, b) && Equal(b, c) && !Equal(a, c)
                && LessOrEqual(a, a) && LessOrEqual(a, b) && LessOrEqual(a, c) && LessOrEqual(c, b) && !LessOrEqual(c, a)
                && MoreOrEqual(a, a) && MoreOrEqual(b, a) && MoreOrEqual(c, a) && MoreOrEqual(b, c) && !MoreOrEqual(a, c)
                //&& b == Math.Clamp(b, a, c) && b == Math.Clamp(a, b, c) && b == EnsureRange(c, a, b)
                && InRange(a, b, c) && !InRange(c, a, b) && InRange(b, a, c)
                && !More(a, b) && More(c, a) && !More(c, b)
                && !Less(a, b) && Less(a, c) && !Less(c, b);

            if (!success)
            /* Генерируем исключение, если самотестирование не прошло */
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Быстрое форматирование строки с интервалом дат (d.hh:mm:ss.ff) 
        /// </summary>
        public static string QickFormat(TimeSpan value)
        {
            //            return value.ToString(@"d\.' 'hh\:mm\:ss' '\.ffff");
            return value.ToString(TextResources.FloatComparerQickFormatDateTime);
        }

        /// <summary>
        /// Быстрое форматирование числовой строки для отображения не более трёх значащих цифр для целей отладки (0, 0.00111, 9, 99.9, 999, 1k/M/G/T/P/E/Z/Y, 1.01k/M/G/T/P/E/Z/Y, 999k/M/G/T/P/E/Z/Y ) 
        /// Сделано на коленке, но как-то работает. Слово "быстрое" относится к использованию, а не к вычислительным ресурсам
        /// </summary>
        public static string QickFormat(float value)
        {
            string negative = value < 0 ? "-" : "";
            double module = Math.Abs(value);
            /* количество десятичных разрядов в числе */
            int depth = (int)Math.Ceiling(Math.Log10(module * 1.001));

            if (depth < -2)
            {
                return "0";
            }
            else
            {
                if (depth <= 3)
                {
                    module = Math.Truncate(module * Math.Pow(10, 3 - depth)) / Math.Pow(10, 3 - depth);
                    return negative + module.ToString();
                }
                else
                {
                    if (3 < depth)
                    {
                        module = Math.Truncate(module / Math.Pow(10, depth - 3)) * Math.Pow(10, depth - 3);

                        if (depth <= 6)
                        {
                            return negative + (module / Math.Pow(10, 3)).ToString() + TextResources.FloatComparerQickFormatPrefixKilo;
                        }
                        else
                        {
                            if (depth <= 9)
                            {
                                return negative + (module / Math.Pow(10, 6)).ToString() + TextResources.FloatComparerQickFormatPrefixMega;
                            }
                            else
                            {
                                if (depth <= 12)
                                {
                                    return negative + (module / Math.Pow(10, 9)).ToString() + TextResources.FloatComparerQickFormatPrefixGiga;
                                }
                                else
                                {
                                    if (depth <= 15)
                                    {
                                        return negative + (module / Math.Pow(10, 12)).ToString() + TextResources.FloatComparerQickFormatPrefixTera;
                                    }
                                    else
                                    {
                                        if (depth <= 18)
                                        {
                                            return negative + (module / Math.Pow(10, 15)).ToString() + TextResources.FloatComparerQickFormatPrefixPeta;
                                        }
                                        else
                                        {
                                            if (depth <= 21)
                                            {
                                                return negative + (module / Math.Pow(10, 18)).ToString() + TextResources.FloatComparerQickFormatPrefixExa;
                                            }
                                            else
                                            {
                                                if (depth <= 24)
                                                {
                                                    return negative + (module / Math.Pow(10, 21)).ToString() + TextResources.FloatComparerQickFormatPrefixZetta;
                                                }
                                                else
                                                {
                                                    if (depth <= 27)
                                                    {
                                                        return negative + (module / Math.Pow(10, 24)).ToString() + TextResources.FloatComparerQickFormatPrefixYotta;
                                                    }
                                                    else
                                                    {
                                                        return negative + module.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        module = Math.Truncate(module);
                        return negative + module.ToString();
                    }
                }
            }

        }

        /// <summary>
        /// Безопасно делит два числа. Если делитель равен нулю, то возвращает значение по умолчанию
        /// </summary>
        public static float SafeDivision(float dividend, float divider, float resultIfZero = 0)
        {
            if (!Equal(0, divider))
            {
                return dividend / divider;
            }
            else
            {
                return resultIfZero;
            }
        }

        /// <summary>
        /// Объединение числа с нулём. Заменяет NaN
        /// </summary>
        public static float Coalesce0(float value)
        {
            return float.IsNaN(value) ? 0 : value;
        }

        /// <summary>
        /// Безопасно делит два числа. Если делитель равен нулю, то возвращает значение по умолчанию
        /// </summary>
        public static long SafeDivision(long dividend, long divider, long resultIfZero = 0)
        {
            if (0 != divider)
            {
                return dividend / divider;
            }
            else
            {
                return resultIfZero;
            }
        }

        /// <summary>
        /// Число слева больше либо равно числу справа
        /// </summary>
        public static bool More(float left, float right)
        {
            return More(left, right, Epsilon);
        }

        /// <summary>
        /// Число слева меньше либо равно числу справа
        /// </summary>
        public static bool Less(float left, float right)
        {
            return Less(left, right, Epsilon);
        }

        /// <summary>
        /// Число слева больше числа справа
        /// </summary>
        public static bool More(float left, float right, float epsilon)
        {
            return left > right + epsilon;
        }

        /// <summary>
        /// Число слева меньше числа справа
        /// </summary>
        public static bool Less(float left, float right, float epsilon)
        {
            return left + epsilon < right;
        }

    }
}
