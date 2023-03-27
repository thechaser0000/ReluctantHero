/*
Статистика производительности.
 
2022-10-28
*/

namespace TestGame
{
    /// <summary>
    /// Статистика производительности.
    /// </summary>
    internal static class PerformanceStatistics
    {
        private static long lastTickDuration;
        private static long lastTickLogicDuration;
        private static long lastTickRenderingDuration;

        /// <summary>
        /// Время начала последнего такта.
        /// </summary>
        private static long lastTickTime;

        /// <summary>
        /// Общее время всех тиков.
        /// </summary>
        private static long totalTickDuration;

        /// <summary>
        /// Общая длительность расчета логики во всех тактах.
        /// </summary>
        private static long totalLogicDuration;

        /// <summary>
        /// Общая длительность отрисовки во всех тактах.
        /// </summary>
        private static long totalRenderingDuration;

        internal static void SkipTick()
        {
            TotalSkippedTickCount++;
        }

        #region Last

        /// <summary>
        /// Длительность последнего такта.
        /// </summary>
        internal static float LastTickDurationMs => 1f * lastTickDuration / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Длительность расчета логики в последнем такте.
        /// </summary>
        internal static float LastTickLogicDurationMs => 1f * lastTickLogicDuration / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Процентная длительность расчета логики в последнем такте.
        /// </summary>
        internal static float LastTickLogicPercent => 0 < LastTickDurationMs ? 100f * LastTickLogicDurationMs / LastTickDurationMs : 0;

        /// <summary>
        /// Длительность отрисовки в последнем тика.
        /// </summary>
        internal static float LastTickRenderingDurationMs => 1f * lastTickRenderingDuration / TimeSpan.TicksPerMillisecond;

        /// <summary>
        /// Процентная длительность отрисовки в последнем тика.
        /// </summary>
        internal static float LastTickRenderingPercent => 0 < LastTickDurationMs ? 100f * LastTickRenderingDurationMs / LastTickDurationMs : 0;

        /// <summary>
        /// Длительность полезной работы в последнем тике (отрисовка + логика)
        /// </summary>
        internal static float LastTickWorkDurationMs => LastTickRenderingDurationMs + LastTickLogicDurationMs;

        /// <summary>
        /// Процентная загрузка последнего тика полезной работой
        /// </summary>
        internal static float LastTickWorkPercent => LastTickLogicPercent + LastTickRenderingPercent;

        #endregion Last

        #region Average

        /// <summary>
        /// Средняя длительность последнего тика.
        /// </summary>
        internal static float AverageTickDurationMs => 1 < TotalTickCount ? 1f * totalTickDuration / TimeSpan.TicksPerMillisecond / (TotalTickCount - 1) : 0;

        /// <summary>
        /// Средняя длительность расчета логики во всех тиках, мс.
        /// </summary>
        internal static float AverageLogicDurationMs => 1 < TotalTickCount ? 1f * totalLogicDuration / (TotalTickCount - 1) / TimeSpan.TicksPerMillisecond : 0;

        /// <summary>
        /// Процентная средняя длительность расчета логики во всех тиках, мс.
        /// </summary>
        internal static float AverageLogicPercent => 1 < AverageTickDurationMs ? 100f * AverageLogicDurationMs / AverageTickDurationMs : 0;

        /// <summary>
        /// Средняя длительность рендеринга во всех тиках, мс.
        /// </summary>
        internal static float AverageRenderingDurationMs => 1 < TotalTickCount ? totalRenderingDuration / (TotalTickCount - 1) / TimeSpan.TicksPerMillisecond : 0;

        /// <summary>
        /// Процентная средняя длительность рендеринга во всех тиках, мс.
        /// </summary>
        internal static float AverageRenderingPercent => 1 < AverageTickDurationMs ? 100f * AverageRenderingDurationMs / AverageTickDurationMs : 0;

        /// <summary>
        /// Средняя длительность полезной работы во всех тиках, мс.
        /// </summary>
        internal static float AverageWorkDurationMs => AverageLogicDurationMs + AverageRenderingDurationMs;

        /// <summary>
        /// Процентная средняя длительность полезной работы во всех тиках, мс.
        /// </summary>
        internal static float AverageWorkPercent => AverageLogicPercent + AverageRenderingPercent;

        /// <summary>
        /// Процентная средняя загрузка тиков полезной работой
        /// </summary>
        internal static float AverageLoadPercent => 0 < totalTickDuration ? 100f * TotalTickUsefulWorkDuration / totalTickDuration : 0;

        
        /// <summary>
        /// Средняя доля пропущенных тиков, мс.
        /// </summary>
        internal static float AverageSkippedTickPercent => 1 < TotalTickCount ? 100f * TotalSkippedTickCount / TotalTickCount : 0;

        #endregion Average

        #region Total

        /// <summary>
        /// Количество пропущенных тиков.
        /// </summary>
        internal static long TotalSkippedTickCount { get; private set; }

        /// <summary>
        /// Количество тиков.
        /// </summary>
        internal static long TotalTickCount { get; private set; }

        /// <summary>
        /// Общая длительность полезной работы во всех тиках
        /// </summary>
        private static float TotalTickUsefulWorkDuration => totalRenderingDuration + totalLogicDuration;

        #endregion Total

        /// <summary>
        /// Сброс статистики
        /// </summary>
        internal static void Reset()
        {
            TotalTickCount = 0;
            TotalSkippedTickCount = 0;

            totalTickDuration = 0;
            totalLogicDuration = 0;
            totalRenderingDuration= 0;
        }

        /// <summary>
        /// Начало такта
        /// </summary>
        internal static void BeginTick()
        {
            TotalTickCount++;
            long now = DateTime.Now.Ticks;
            if (0 < lastTickTime)
            {
                lastTickDuration = (now - lastTickTime);
                totalTickDuration += lastTickDuration;
            }

            lastTickTime = now; 
        }

        /// <summary>
        /// Конец выполнения тактом расчёта логики.
        /// </summary>
        internal static void EndLogic()
        {
            long now = DateTime.Now.Ticks;
            if (0 < lastTickTime)
            {
                lastTickLogicDuration = (now - lastTickTime);
                totalLogicDuration += lastTickLogicDuration;
            }
        }

        /// <summary>
        /// Конец выполнения тактом отрисовки.
        /// </summary>
        internal static void EndRendering()
        {
            long now = DateTime.Now.Ticks;
            if (0 < lastTickTime)
            {
                lastTickRenderingDuration = (now - lastTickTime - lastTickLogicDuration);
                totalRenderingDuration += lastTickRenderingDuration;
            }
        }

        static PerformanceStatistics()
        {
            Reset();
        }
    }
}
