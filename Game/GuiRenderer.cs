/*
Рендерер интерфейса. 

REF Перейти на ConsoleTextRenderer, код формировнаия фреймов вынести в саму сцену.
 
2022-11
*/

using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Рендерер интерфейса
    /// </summary>
    internal class GuiRenderer : Renderer
    {
        /// <summary>
        /// Ускоритель вывода на консоль.
        /// </summary>
        private ConsoleAccelerator Accelerator { get; init; }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal GuiRenderer(ConsoleAccelerator accelerator)
        {
            Accelerator = accelerator;
        }

        /// <summary>
        /// Общие настройки
        /// </summary>
        private const int width = GameSeed.InterfaceWidth;
        private const ConsoleColor textColor = GameSeed.PlainTextColor;
        private const ConsoleColor textColorAlt = GameSeed.PlainTextColorAlt;
        private const ConsoleColor highlightTextColor = GameSeed.HighlightTextColor;
        private const Char EmptyChar = ' ';

        /// <summary>
        /// Записать цветную строку из 2 столбцов
        /// </summary>
        private void WriteColumns(int column1Width, string column1, string column2)
        {
            int column2Width = width - column1Width;
            int oldColumn = Accelerator.CursorColumn;

            Accelerator.BackgroundColor = GameSeed.PanelColor;
            Accelerator.ForegroundColor = highlightTextColor;
            Accelerator.WritePadLeft(column1 + EmptyChar, column1Width);
            Accelerator.ForegroundColor = textColor;
            Accelerator.WritePadRight(column2, column2Width);

            Accelerator.CursorLine++;
            Accelerator.CursorColumn = oldColumn;
        }

        /// <summary>
        /// Записать цветную строку из N столбцов
        /// </summary>
        private void WriteColumns(params string[] columns)
        {
            int columnCount = columns.Length;
            int columnWidth = (width) / columnCount;
            int rest = width - columnWidth * columnCount;
            int oldColumn = Accelerator.CursorColumn;
            Accelerator.BackgroundColor = GameSeed.PanelColor;

            for (int index = 0; index < columnCount; index++)
            {
                // если число столбцов чётное, то выделяем цветом нечетные столбцы, 
                if (columnCount % 2 == 0)
                {
                    Accelerator.ForegroundColor = (index % 2 == 0) ? textColor : highlightTextColor;
                }
                else
                // иначе (если число столбцов нечётное) выделяем цветом четные столбцы, кроме 0 
                {
                    Accelerator.ForegroundColor = (index % 2 != 0) || (index == 0) ? textColor : highlightTextColor;
                }
                Accelerator.WritePadLeft(columns[index], columnWidth);
            }

            Accelerator.WritePadRight("", rest);
            Accelerator.CursorLine++;
            Accelerator.CursorColumn = oldColumn;
        }

        private void WriteHeader(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            int oldColumn = Accelerator.CursorColumn;

            Accelerator.ForegroundColor = textColorAlt;
            Accelerator.BackgroundColor = background ?? GameSeed.PanelColorAlt;
            Accelerator.CursorColumn = GameSeed.InterfaceLeft;
            Accelerator.WritePadBoth(text, width);

            Accelerator.CursorLine++;
            Accelerator.CursorColumn = oldColumn;
        }

        /// <summary>
        /// Заполнить остаток панели цветом фона
        /// </summary>
        private void FillRemain(int? fromLine = null)
        {
            // Дополняем блок пустыми строками
            for (int restLine = fromLine ?? Accelerator.CursorLine; restLine < (GameSeed.InterfaceTop + GameSeed.InterfaceHeight - 1); restLine++)
            {
                Accelerator.SetCursorPosition(GameSeed.InterfaceLeft, restLine);
                Accelerator.WritePadRight("", GameSeed.InterfaceWidth);
            }
        }

        /// <summary>
        /// Отрисовать панель геймплея.
        /// </summary>
        private void RenderGameplay()
        {
            Accelerator.SetCursorPosition(GameSeed.InterfaceLeft, GameSeed.InterfaceTop);
            WriteHeader($"STAGE-?");
            FillRemain();
        }

        /// <summary>
        /// Отрисовать панель справки.
        /// </summary>
        private void RenderHelp()
        {
            const int column1Width = 4;
            Accelerator.SetCursorPosition(GameSeed.InterfaceLeft, GameSeed.InterfaceTop);
            WriteHeader("CONTROL");

            WriteColumns(column1Width, ConsoleFieldRenderer.LeftArrow.ToString(), "Move left");
            WriteColumns(column1Width, ConsoleFieldRenderer.RightArrow.ToString(), "Move right");
            WriteColumns(column1Width, ConsoleFieldRenderer.UpArrow.ToString(), "Move up");
            WriteColumns(column1Width, ConsoleFieldRenderer.DownArrow.ToString(), "Move down");
            WriteColumns(column1Width, "Esc", "Reset game");

            FillRemain();
        }

        /// <summary>
        /// Отрисовать панель отладки.
        /// </summary>
        private void RenderDebug()
        {
            int entityCellCount = Engine.ActiveScene.GetEntityCellCount();
            int cellEntityCount = Engine.ActiveScene.GetCellEntityCount();

            Accelerator.SetCursorPosition(GameSeed.InterfaceLeft, GameSeed.InterfaceTop);
            WriteHeader("SCENE");
            WriteColumns(
                "Wdt", FloatComparer.QickFormat(Engine.ActiveScene.Width), 
                "Hgt", FloatComparer.QickFormat(Engine.ActiveScene.Height),
                "Cap", FloatComparer.QickFormat(Engine.ActiveScene.Rectangle.Capacity),
                "Sed", "?" /*FloatComparer.QickFormat((int)Engine.ActiveScene.Seed)*/
                );
            WriteHeader("ENTITIES");
            WriteColumns(
                "Cnt", FloatComparer.QickFormat(Engine.ActiveScene.Entities.Count),
                "EnC", FloatComparer.QickFormat(entityCellCount),
                "ClE", FloatComparer.QickFormat(cellEntityCount),
                "+/-", FloatComparer.QickFormat(cellEntityCount - entityCellCount)
                );

            FillRemain();
        }

        /// <summary>
        /// Отрисовать панель производительности.
        /// </summary>
        private void RenderPerformance()
        {
            Accelerator.SetCursorPosition(GameSeed.InterfaceLeft, GameSeed.InterfaceTop);
            WriteHeader("PERFORMANCE");

            WriteColumns("", "LogD", "Log%", "RenD", "Ren%", "WorD", "Wor%");
            WriteColumns("Last",
                FloatComparer.QickFormat(PerformanceStatistics.LastTickLogicDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.LastTickLogicPercent),
                FloatComparer.QickFormat(PerformanceStatistics.LastTickRenderingDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.LastTickRenderingPercent),
                FloatComparer.QickFormat(PerformanceStatistics.LastTickWorkDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.LastTickWorkPercent)
                );
            WriteColumns("Aver",
                FloatComparer.QickFormat(PerformanceStatistics.AverageLogicDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.AverageLogicPercent),
                FloatComparer.QickFormat(PerformanceStatistics.AverageRenderingDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.AverageRenderingPercent),
                FloatComparer.QickFormat(PerformanceStatistics.AverageWorkDurationMs),
                FloatComparer.QickFormat(PerformanceStatistics.AverageWorkPercent)
                );
            WriteHeader("TICKS");
            WriteColumns("Tick", FloatComparer.QickFormat(PerformanceStatistics.TotalTickCount), "LastD", FloatComparer.QickFormat(PerformanceStatistics.LastTickDurationMs), "AvgD", FloatComparer.QickFormat(PerformanceStatistics.AverageTickDurationMs));
            WriteColumns("Skip", FloatComparer.QickFormat(PerformanceStatistics.TotalSkippedTickCount), "Avg%",  FloatComparer.QickFormat(PerformanceStatistics.AverageSkippedTickPercent), "", "");

            WriteHeader("CONSOLE ACCELERATOR");
            WriteColumns(
                "Lst",
                "StrW",
                FloatComparer.QickFormat(Accelerator.Statistics.LastWriteStringCount),
                "CtSw",
                FloatComparer.QickFormat(Accelerator.Statistics.LastSwitchContextCount),
                "Dur",
                FloatComparer.QickFormat(Accelerator.Statistics.LastRenderDuration / TimeSpan.TicksPerMillisecond)
                );
            WriteColumns(
                "Avg",
                "StrW",
                FloatComparer.QickFormat(Accelerator.Statistics.AverageWriteStringCount),
                "CtSw",
                FloatComparer.QickFormat(Accelerator.Statistics.AverageSwitchContextCount),
                "Dur",
                FloatComparer.QickFormat(Accelerator.Statistics.AverageRenderDuration / TimeSpan.TicksPerMillisecond)
                );
            WriteColumns(
                "Totl",
                "StrW",
                FloatComparer.QickFormat(Accelerator.Statistics.WriteStringCount),
                "CtSw",
                FloatComparer.QickFormat(Accelerator.Statistics.SwitchContextCount),
                "Dur",
                FloatComparer.QickFormat(Accelerator.Statistics.RenderDuration / TimeSpan.TicksPerMillisecond)
                );

            FillRemain();
        }

        /// <summary>
        /// Отрисовать самую важную информацию поверх экрана
        /// </summary>
        private void RenderOverlay()
        {
            return;
            //UPD Отключать эту информацию горячей клавишей.

            int entityCellCount = Engine.ActiveScene.GetEntityCellCount();
            int cellEntityCount = Engine.ActiveScene.GetCellEntityCount();
            int runningSceneCount = 0;
            int visibleSceneCount = 0;

            foreach (var scene in Engine.Scenes)
            {
                if (scene.State == SceneState.Running)
                {
                    runningSceneCount++;
                }
                if (scene.IsVisible)
                {
                    visibleSceneCount++;
                }
            }

            // Основные параметры.
            Accelerator.SetCursorPosition(0, 0);
            Accelerator.SetColors(GameSeed.TransparentColor, GameSeed.PlainTextColor);
            Accelerator.Write(string.Format("Lod% {0, -5} StW {1, -5} LAv% {2, -5} SWA {3, -5} Tck {4, -5} Run {5, -5} Vis {6, -5} E1S {7} E2S {8}", 
                FloatComparer.QickFormat(PerformanceStatistics.LastTickWorkPercent), 
                FloatComparer.QickFormat(Accelerator.Statistics.LastWriteStringCount),
                FloatComparer.QickFormat(PerformanceStatistics.AverageWorkPercent),
                FloatComparer.QickFormat(Accelerator.Statistics.AverageWriteStringCount),
                FloatComparer.QickFormat(Engine.TickCount),
                runningSceneCount,
                visibleSceneCount,
                Engine.Scenes.Find(x => x.Name == "Episode1")?.State,
                Engine.Scenes.Find(x => x.Name == "Episode2")?.State
                ));


            /*
            // Цветовой тест.
            Accelerator.SetCursorPosition(0, 2);
            for (int color = 0; color < 16; color++)
            {
                Accelerator.SetColors((ConsoleColor)color, (ConsoleColor)(15 - color));
                Accelerator.WriteLn(color.ToString().PadLeft(2));
            }
            */


            // Параметры сцены
            Accelerator.SetCursorPosition(0, Accelerator.Rectangle.Bottom);
            Accelerator.SetColors(GameSeed.TransparentColor, GameSeed.PlainTextColor);
            Accelerator.Write(string.Format("{4}({10})  Ent {0, -5} Pic {1, -5} Fra {2, -5} Bal {6, -5} Tck {3, -5} Cpc {5, -5} ({7}x{8}) Lay {9, -5}",
                FloatComparer.QickFormat(Engine.ActiveScene.Entities.Count),
                FloatComparer.QickFormat(Engine.ActiveScene.Pictures.Count),
                FloatComparer.QickFormat(Engine.ActiveScene.TextFrames.Count),
                FloatComparer.QickFormat(Engine.ActiveScene.TickCount),
                Engine.ActiveScene.Name,
                FloatComparer.QickFormat(Engine.ActiveScene.Rectangle.Capacity),
                entityCellCount - cellEntityCount,
                Engine.ActiveScene.Rectangle.Width,
                Engine.ActiveScene.Rectangle.Height,
                Engine.ActiveScene.Layers.Count,
                Engine.ActiveScene.State
                ));
        }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        internal override void RenderScene(Scene scene, Viewport viewport)
        {
            // Отрисовать оверлей с общей информацией.
            RenderOverlay();
        }
    }
}
