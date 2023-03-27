/*
Класс для текстовых ресурсов. Взят из другого проекта для FloatComparer.

2021-10
*/

namespace Scge
{
    /// <summary>
    /// Статический класс для текстовых ресурсов
    /// </summary>
    public static class TextResources
    {
        /// <summary>
        /// FloatComparerQickFormat
        /// </summary>
        public const string FloatComparerQickFormatDateTime = @"d\.' 'hh\:mm\:ss' '\.ffff";

        /// <summary>
        /// Десятичные приставки СИ
        /// </summary>
        public const string FloatComparerQickFormatPrefixKilo = "k";
        public const string FloatComparerQickFormatPrefixMega = "M";
        public const string FloatComparerQickFormatPrefixGiga = "G";
        public const string FloatComparerQickFormatPrefixTera = "T";
        public const string FloatComparerQickFormatPrefixPeta = "P";
        public const string FloatComparerQickFormatPrefixExa = "E";
        public const string FloatComparerQickFormatPrefixZetta = "Z";
        public const string FloatComparerQickFormatPrefixYotta = "Y";
    }
}
