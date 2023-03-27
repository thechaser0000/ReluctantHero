/*
Игра.

2022-10-14 
*/

using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Игра
    /// </summary>
    internal static class Game
    {
        /// <summary>
        /// Рендерер для игрового поля.
        /// </summary>
        internal static ConsoleFieldRenderer FieldRenderer { get; private set; }

        /// <summary>
        /// Рендерер для карты.
        /// </summary>
        internal static ConsoleMapRenderer MapRenderer { get; private set; }

        /// <summary>
        /// Рендерер для пользовательского интерфейса.
        /// </summary>
        internal static GuiRenderer InterfaceRenderer { get; private set; }

        /// <summary>
        /// Рендерер текста.
        /// </summary>
        internal static ConsoleTextRenderer TextRenderer { get; private set; }

        /// <summary>
        /// Библиотека спрайтов
        /// </summary>
        internal static SpriteLibrary SpriteLibrary { get; private set; }

        /// <summary>
        /// Герой.
        /// </summary>
        internal static Hero Hero { get; private set; }

        /// <summary>
        ///  Этап.
        /// </summary>
//        public static int Stage { get; private set; } = 1;

        private static readonly AutoResetEvent AutoEvent = new(false);

        /// <summary>
        /// Таймер.
        /// </summary>
        private static readonly Timer Timer = new(OnTimer, AutoEvent, Timeout.Infinite, 0);

        // Ускоритель вывода на консоль.
        internal static ConsoleAccelerator Accelerator { get; private set; }

        /// <summary>
        /// Перейти к сцене Интро.
        /// </summary>
        internal static void GoToIntro()
        {
            Engine.GoToScene(Wait, Intro);
        }

        /// <summary>
        /// Перейти к сцене Эпилог.
        /// </summary>
        internal static void GoToEpilogue()
        {
            Engine.GoToScene(Wait, Epilogue);
        }

        /// <summary>
        /// Перейти к сцене Справка.
        /// </summary>
        internal static void GoToHelp()
        {
            Engine.GoToScene(Wait, Help);
        }

        /// <summary>
        /// Перейти к сцене Постскриптум.
        /// </summary>
        internal static void GoToPostscriptum()
        {
            Engine.GoToScene(Wait, Postscriptum);
        }

        /// <summary>
        /// Выйти в систему.
        /// </summary>
        internal static void GoToSystem()
        {
            // REF ну так себе выход
            Environment.Exit(0);
        }

        /// <summary>
        /// Перейти к сцене Главное меню.
        /// </summary>
        internal static void GoToMainMenu()
        {
            Engine.GoToScene(Wait, MainMenu);
        }

        /// <summary>
        /// Перейти к эпизоду 1.
        /// </summary>
        private static void GoToEpisode1()
        {
            Engine.GoToScene(Wait, Episode1);
            //            Engine.GoToScene(Wait, Episode2);
        }

        /// <summary>
        /// Перейти к эпизоду 1.
        /// </summary>
        private static void GoToEpisode2()
        {
            Engine.GoToScene(Wait, Episode2);
        }

        /// <summary>
        /// Перейти к геймоверу.
        /// </summary>
        private static void GoToGameOver(GameOverScenario scenario)
        {
            GameOver.Scenario = scenario;
            Engine.GoToScene(Wait, GameOver);
        }

        /// <summary>
        /// Перейти к сцене пролога.
        /// </summary>
        private static void GoToPrologue()
        {
            Engine.GoToScene(Wait, Prologue);
        }

        static Game()
        {
            // Подготавливаем экран.
            try
            {
                Console.SetWindowSize(GameSeed.WindowWidth, GameSeed.WindowHeight);
            }
            catch { }

            Console.BackgroundColor = GameSeed.TransparentColor;
            Console.ForegroundColor = GameSeed.TransparentColor;
            Console.Clear();

            Accelerator = new(0, 0, GameSeed.WindowWidth, GameSeed.WindowHeight, GameSeed.TransparentColor) { };
            InputController = new();

            SpriteLibrary = new(GameSeed.FieldCellWidth, GameSeed.FieldCellHeight);

            // рендерер игрового поля
            FieldRenderer = new(Game.Accelerator, GameSeed.FieldCellWidth, GameSeed.FieldCellHeight, GameSeed.TransparentColor)
            {
                DrawKind = PlatformConsole.DrawKind.Item,
                Left = GameSeed.FieldLeft,
                Top = GameSeed.FieldTop
            };

            // подключаем сторонний способ отрисовки ячеек (включается через режим отрисовки)
            FieldRenderer.CellDrawing += Game.CellDrawing;

            // рендерер карты
            MapRenderer = new(Game.Accelerator)
            {
                Left = GameSeed.MapLeft,
                Top = GameSeed.MapTop,

                TransparentColor = GameSeed.TransparentColor,
                DefaultColor = GameSeed.MapColorGround,
                ScrollBarColor = GameSeed.PanelColor,
                ScrollBarTrackerColor = GameSeed.PanelColorAlt,
                OutOfBoundsColor = GameSeed.PanelColorAlt
            };

            // подключаем сторонний способ отрисовки ячеек (включается через режим отрисовки)
            MapRenderer.CellDrawing += Game.MapCellDrawing;

            // рендерер интерфейса
            InterfaceRenderer = new(Game.Accelerator);

            // Рендерер текста.
            TextRenderer = new(Game.Accelerator);
        }

        /// <summary>
        /// В данный момент выполняется тик.
        /// </summary>
        private static bool TickIsRunning { get; set; }

        /// <summary>
        /// Экспериментальная "своя" отрисовка ячейки карты игрового поля
        /// </summary>
        internal static void MapCellDrawing(object sender, MapCellDrawingEventArgs e)
        {
            // штатными средствами рисуется верхний или нижний полусимвол, имитируя сразу две ячейки, а  унас выводится решётка, где цвет фона и символа соответствует цветам верхней и нижней ячеек
            Accelerator.Write('#');
        }

        /// <summary>
        /// Экспериментальная "своя" отрисовка ячейки игрового поля
        /// </summary>
        internal static void CellDrawing(object sender, CellDrawingEventArgs args)
        {
            // рисуем подложку (она нужна если нет слоя бэкграунда - для подстраховки)
            Accelerator.SetColors(FieldRenderer.TransparentColor, FieldRenderer.TransparentColor);

            //рисуем нужный блок
            for (int line = 0; line < args.Rectangle.Height; line++)
            {
                Accelerator.SetCursorPosition(args.Rectangle.Left, args.Rectangle.Top + line);
                Accelerator.Write(FieldRenderer.EmptyText);
            }

            // Перебираем ячейки послойно.
            foreach (Cell cell in args.Cells)
            {
                ConsoleEntity entity = (cell.Entity as ConsoleEntity)!;
                Accelerator.CopySpriteTo(entity.SpriteSelector.Selected, args.Rectangle.Left, args.Rectangle.Top);
            }
        }

        /// <summary>
        /// Отрисовка ячейки игрового поля в один цвет символами.
        /// </summary>
        internal static void AsciiCellDrawing(object sender, CellDrawingEventArgs e)
        {
            Accelerator.SetColors(GameSeed.PlainTextColor, GameSeed.PanelColor);

            // меняем цвета на символы
            for (int line = 0; line < e.Rectangle.Height; line++)
            {
                Accelerator.SetCursorPosition(e.Rectangle.Left, e.Rectangle.Top + line);
            }
        }

        /// <summary>
        /// Загрузить игру.
        /// </summary>
        private static void LoadAll()
        {
            Intro = new();
            Prologue = new();
            MainMenu = new();
            GameOver = new();
            Episode1 = new();
            Episode2 = new();
            Epilogue = new();
            Postscriptum = new();
            Help = new();

            Engine.Scenes.Add(Intro);
            Engine.Scenes.Add(Prologue);
            Engine.Scenes.Add(MainMenu);
            Engine.Scenes.Add(GameOver);
            Engine.Scenes.Add(Episode1);
            Engine.Scenes.Add(Episode2);
            Engine.Scenes.Add(Epilogue);
            Engine.Scenes.Add(Postscriptum);
            Engine.Scenes.Add(Help);

            PerformanceStatistics.Reset();
        }

        /// <summary>
        /// Обработчик такта таймера.
        /// </summary>
        private static void OnTimer(object? state)
        {
            // Выход из ативной сцены
            static void ExitFromActiveScene()
            {
                // переходим в главное меню по завершении интро, эпилога или из меню паузы
                if ((Engine.ActiveScene is Intro)
                    || (Engine.ActiveScene is Epilogue)
                    || (Engine.ActiveScene is Help)
                    || (Engine.ActiveScene is Postscriptum)
                    || (Engine.ActiveScene is Episode1 && Episode1.Result == EpisodeResult.Abort)
                    || (Engine.ActiveScene is Episode2 && Episode2.Result == EpisodeResult.Abort))
                {
                    GoToMainMenu();
                    return;
                }

                // переходим в меню геймовера после геймовера
                if ((Engine.ActiveScene is Episode1 && Episode1.Result == EpisodeResult.Fail)
                    || (Engine.ActiveScene is Episode2 && Episode2.Result == EpisodeResult.Fail))
                {
                    // Если проиграли в эпизоде 2 и анна мертва, то выбираем соответствующий сценарий.
                    GoToGameOver(Engine.ActiveScene == Episode1 || !Episode2.AnnaIsDead ? GameOverScenario.AlbertIsDead : GameOverScenario.AnnaIsDead);
                    return;
                }

                // переходим к эпизоду 1 после пролога
                if (Engine.ActiveScene is Prologue)
                {
                    //                        GoToEpisode2();
                    GoToEpisode1();
                    return;
                }

                // переходим к эпизоду 2 после эпизода 1
                if (Engine.ActiveScene is Episode1 && Episode1.Result == EpisodeResult.Win)
                {
                    GoToEpisode2();
                    return;
                }

                // переходим к эпилогу после победы
                if (Engine.ActiveScene is Episode2 && Episode2.Result == EpisodeResult.Win)
                {
                    GoToEpilogue();
                    return;
                }

                // развилка при выходе из меню геймовера
                if (Engine.ActiveScene is GameOver gameOver)
                {
                    switch (gameOver.Result)
                    {
                        // Выход.
                        case GameOverResult.Exit:
                            GoToSystem();
                            //Engine.NeedExit = true;
                            break;
                        // Новая игра
                        case GameOverResult.NewGame:
                            GoToPrologue();
                            break;
                        // Главное меню
                        case GameOverResult.MainMenu:
                            GoToMainMenu();
                            break;
                    }
                    return;
                }

                // развилка при выходе из главного меню
                if (Engine.ActiveScene is MainMenu mainMenu)
                {
                    switch (mainMenu.Result)
                    {
                        // Выход.
                        case MainMenuResult.Exit:
                            GoToSystem();
                            //Engine.NeedExit = true;
                            break;
                        // Справка
                        case MainMenuResult.Help:
                            GoToHelp();
                            break;
                        // Новая игра
                        case MainMenuResult.NewGame:
                            GoToPrologue();
                            break;
                        // Интро
                        case MainMenuResult.Intro:
                            GoToIntro();
                            break;
                        // Постскриптум
                        case MainMenuResult.Postscriptum:
                            GoToPostscriptum();
                            break;
                    }
                    return;
                }
            }

            if (!TickIsRunning)
            // в данный момент не выполняется другой тик
            {
                InputController.DoTick();
                CheckInput();

                TickIsRunning = true;
                PerformanceStatistics.BeginTick();

                Engine.DoTick();

                // Пайплайн сцен
                // загружаем все сцены на первом тике
                if (1 == Engine.TickCount)
                {
                    LoadAll();
                    Engine.GoToScene(Intro);
                }

                if (Engine.ActiveScene.State is SceneState.Exit)
                // Если активная сцена завершена.
                {
                    ExitFromActiveScene();
                }

                InputController.AfterTick();
                TickIsRunning = false;
            }
            else
            // в данный момент выполняется другой тик
            {
                PerformanceStatistics.SkipTick();
            }
        }

        /// <summary>
        /// Контроллер ввода
        /// </summary>
        internal static InputController InputController { get; private set; }

        /// <summary>
        /// Проверить состояние устройств ввода.
        /// </summary>
        private static void CheckInput()
        {
            // Быстрое восстановление характеристик по F6
            if (InputController.QuickRestore)
            {
                if (Engine.ActiveScene is Episode episode)
                {
                    episode.Hero.Health.Restore();
                    episode.Hero.ShotGun.Ammo.Restore();
                    episode.Hero.ShotGun.IsEnabled = true;
                    episode.Hero.Knife.IsEnabled = true;
                    return;
                }
            }

            // Быстрая победа по F7
            if (InputController.QuickWin)
            {
                if (Engine.ActiveScene is Episode episode)
                {
                    episode.LeftToWin = 0;
                    return;
                }
            }

            // Быстрое поражение по F8
            if (InputController.QuickFail)
            {
                if (Engine.ActiveScene is Episode episode)
                {
                    episode.LeftToFail = 0;
                    return;
                }
            }

            // Сброс по F10
            if (InputController.EngineReset)
            {
                ResetGame();
                return;
            }

            // Пауза по F9
            if (InputController.EnginePause)
            {
                if (Engine.State is EngineState.Running)
                {
                    Engine.Pause();
                }
                else if (Engine.State is EngineState.Paused)
                {
                    Engine.Start();
                }
                return;
            }
        }

        /// <summary>
        /// Запустить новую сессию игры.
        /// </summary>
        internal static void ResetGame()
        {
            Timer.Change(Timeout.Infinite, 0);

            GenerateNewWorld();

            TickIsRunning = false;
            Timer.Change(0, GameSeed.TickDuration);

            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                InputController.ConsoleKeyPress(key);
                // обработка нажатия клавиш
            }

            AutoEvent.WaitOne();
        }

        /// <summary>
        /// Сгенерировать игровой мир.
        /// </summary>
        private static void GenerateNewWorld()
        {
            Engine.Reset();
            Accelerator.Statistics.Reset();
            PerformanceStatistics.Reset();

            Hero = new();

            Wait = new();
            Engine.Scenes.Add(Wait);
            //Engine.ActiveScene = Wait;
            Engine.GoToScene(Wait);

            Engine.Start();
        }

        /// <summary>
        /// Все сцены.
        /// </summary>
        internal static Intro Intro { get; set; }
        internal static Wait Wait { get; set; }
        internal static Prologue Prologue { get; set; }
        internal static MainMenu MainMenu { get; set; }
        internal static GameOver GameOver { get; set; }
        internal static Episode1 Episode1 { get; set; }
        internal static Episode2 Episode2 { get; set; }
        internal static Epilogue Epilogue { get; set; }
        internal static Postscriptum Postscriptum { get; set; }
        internal static Help Help { get; set; }
    }
}
