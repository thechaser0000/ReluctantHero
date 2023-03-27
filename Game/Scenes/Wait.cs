/*
Экран ожидания.
Сцена с призывом немного подождать.

2022-12-11 
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Экран ожидания.
    /// </summary>
    internal class Wait : Scene
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Wait() : base()
            {
            Name = "Wait";

            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            // фрейм 1
            TextFrame frame = new(GameSeed.TransparentColor, ConsoleColor.Yellow, "...") { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,  Rectangle = new(GameSeed.WindowWidth, GameSeed.WindowHeight) };
            TextFrames.Add(frame);

            State = SceneState.Running;
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            // Передаём управление сущностям.
            base.DoTick();

            // проверяем условия победы и поражения.
            PerformanceStatistics.EndLogic();

            // Отрисовываем.
            DoRendering();
            PerformanceStatistics.EndRendering();
        }

        /// <summary>
        /// Выполнить отрисовку.
        /// </summary>
        internal override void DoRendering()
        {
            // Пререндеринг слоёв - формирование итогового кадра.
            base.DoRendering();

            // Осуществить итоговый рендеринг кадра
            Game.Accelerator.Render(true);
            Game.Accelerator.Clear();
        }
    }
}
