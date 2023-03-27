/*
Статистика ускорителя вывода на консоль.
Здорово выручила при разработке рендереров

2022-11-23 (Вынесено из ConsoleAccelerator)
*/

namespace PlatformConsole
{
    /// <summary>
    /// Статистика ускорителя вывода на консоль.
    /// </summary>
    internal class ConsoleAcceleratorStatistics
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleAcceleratorStatistics()
        {
            Reset();
        }

        /// <summary>
        /// Количество вызовов рендеринга.
        /// </summary>
        internal int RenderCount { get; private set; }
        
        /// <summary>
        /// Количество отрисовок строк.
        /// </summary>
        internal int WriteStringCount { get; private set; }
        
        /// <summary>
        /// Количество записанных символов.
        /// </summary>
        internal int WriteCharacterCount { get; private set; }
        
        /// <summary>
        /// Количество переключений контекста (цвет фона или текста, позиция курсора)
        /// </summary>
        internal int SwitchContextCount { get; private set; }
        
        /// <summary>
        /// Длительность рендеринга, системных тактов.
        /// </summary>
        internal long RenderDuration { get; private set; }

        /// <summary>
        /// Количество отрисовок строк в последнем рендеринге.
        /// </summary>
        internal int LastWriteStringCount { get; private set; }
        
        /// <summary>
        /// Количество записанных символов в последнем рендеринге.
        /// </summary>
        internal int LastWriteCharacterCount { get; private set; }
        
        /// <summary>
        /// Количество переключений контекста (цвет фона или текста, позиция курсора) в последнем рендеринге.
        /// </summary>
        internal int LastSwitchContextCount { get; private set; }
        
        /// <summary>
        /// Длительность последнего рендеринга, системных тактов.
        /// </summary>
        internal long LastRenderDuration { get; private set; }

        /// <summary>
        /// Количество отрисовок строк в среднем.
        /// </summary>
        internal float AverageWriteStringCount => 0 < RenderCount ? 1.0f * WriteStringCount / RenderCount : 0;
        /// <summary>
        /// Количество записанных символов в среднем.
        /// </summary>
        internal float AverageWriteCharacterCount => 0 < RenderCount ? 1.0f * WriteCharacterCount / RenderCount : 0;
        /// <summary>
        /// Количество переключений контекста (цвет фона или текста, позиция курсора) в среднем.
        /// </summary>
        internal float AverageSwitchContextCount => 0 < RenderCount ? 1.0f * SwitchContextCount / RenderCount : 0;
        /// <summary>
        /// Длительность рендеринга, системных тактов, в среднем.
        /// </summary>
        internal float AverageRenderDuration => 0 < RenderCount ? 1.0f * RenderDuration / RenderCount : 0;

        /// <summary>
        /// Время запуска последнего рендеринга.
        /// </summary>
        private long lastRenderStart;

        /// <summary>
        /// Учесть в статистике время начала рендеринга.
        /// </summary>
        internal void StartRender()
        {
            ResetLast();
            lastRenderStart = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Учесть в статистике время окончания рендеринга.
        /// </summary>
        internal void StopRender()
        {
            LastRenderDuration = (DateTime.Now.Ticks - lastRenderStart);
            RenderDuration += LastRenderDuration;

            RenderCount++;
        }

        /// <summary>
        /// Учесть в статистике запись строки.
        /// </summary>
        internal void AcceptWriteString(int characterCount)
        {
            LastWriteStringCount++;
            LastWriteCharacterCount += characterCount;

            WriteStringCount++;
            WriteCharacterCount += characterCount;
        }

        /// <summary>
        /// Учесть в статистике переключение контекста.
        /// </summary>
        internal void AcceptSwitchContext()
        {
            LastSwitchContextCount++;
            SwitchContextCount++;
        }

        /// <summary>
        /// Сброс статистики
        /// </summary>
        internal void Reset()
        {
            RenderCount = 0;
            WriteStringCount = 0;
            WriteCharacterCount = 0;
            SwitchContextCount = 0;
            RenderDuration = 0;

            ResetLast();
        }

        /// <summary>
        /// Сброс статистики последнего события
        /// </summary>
        private void ResetLast()
        {
            LastRenderDuration = 0;
            LastSwitchContextCount = 0;
            LastWriteCharacterCount = 0;
            LastWriteStringCount = 0;
        }
    }
}
