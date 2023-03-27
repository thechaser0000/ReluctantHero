/*
Справка. 

2023-01-19 
*/

using Scge;
using PlatformConsole;
using System.Diagnostics;

namespace TestGame
{
    /// <summary>
    /// Справка.
    /// </summary>
    internal class Help : Scene
    {
        /// <summary>
        /// Фрейм истории.
        /// </summary>
        private TextFrame contentFrame;

        /// <summary>
        /// Фрейм заголовков.
        /// </summary>
        private List<TextFrame> headersFrame;

        /// <summary>
        /// Фрейм справки.
        /// </summary>
        private TextFrame tooltipFrame;

        /// <summary>
        /// Контент.
        /// </summary>
        private List<string> content;

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            // Максимальная ширина текста
            int maxLength = 0;
            foreach (var line in content)
            {
                maxLength = Math.Max(maxLength, line.Length);
            }
            int horizontalIndent = (GameSeed.WindowWidth - maxLength) / 2;
            int verticalIndent = (GameSeed.WindowHeight - content.Count) / 2;

            // Фрейм контента
            contentFrame = new(GameSeed.TransparentColor, ConsoleColor.DarkYellow)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Rectangle = new(new Segment(horizontalIndent + 1, GameSeed.WindowWidth - horizontalIndent), new(verticalIndent - 2, GameSeed.WindowHeight - verticalIndent - 3))
            };
            TextFrames.Add(contentFrame);
            contentFrame.Strings.AddRange(content);

            // Фрейм подсказки
            tooltipFrame = new(GameSeed.TransparentColor, ConsoleColor.DarkGray)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                AsText = "Esc - выйти",
                Rectangle = new(new Segment(0, GameSeed.WindowWidth - 1), new(GameSeed.WindowHeight - verticalIndent - 1, GameSeed.WindowHeight - 1))
            };
            TextFrames.Add(tooltipFrame);

            // Фрейм заголовков
            headersFrame = new()
            {
                new(GameSeed.TransparentColor, ConsoleColor.DarkBlue)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Rectangle = new(0, 7, GameSeed.WindowWidth - 1, 7),
                    AsText = "УПРАВЛЕНИЕ"
                },
                new(GameSeed.TransparentColor, ConsoleColor.DarkBlue)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Rectangle = new(0, 17, GameSeed.WindowWidth - 1, 17),
                    AsText = "СОВЕТЫ"
                },
            };
            TextFrames.AddRange(headersFrame);

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Help() : base()
        {
            Name = "Help";

            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            // Инициализируем контент
            content = new()
            {
                new("              УПРАВЛЕНИЕ               "),
                new(""),
                new("Right  Двигаться вправо"),
                new("Left   Двигаться влево"),
                new("Up     Двигаться вверх"),
                new("Down   Двигаться вниз"),
                new("Space  Атаковать"),
                new("Esc    Приостановить"),
                new(""),
                new(""),
                new("                СОВЕТЫ                 "),
                new(""),
                new("* Уничтожайте врагов"),
                new("* Собирайте аптечки и патроны"),
                new("* Разрушайте предметы интерьера"),
                new("* Приобретайте опыт и повышайте уровень"),
                new("* Будьте осторожны с животными"),
            };
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            if (999 < TickCount || Game.InputController.Escape.Impacted)
            // Завершаем сцену через несколько секунд или по Esc
            {
                State = SceneState.Exit;
            }

            // Передаём управление сущностям.
            base.DoTick();

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



