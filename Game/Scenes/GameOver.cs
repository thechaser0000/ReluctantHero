/*
Экран завершения игры.
Переход в главное меню или начало новой игры.

2022-12-11 
*/

using Scge;
using PlatformConsole;

namespace TestGame
{

    /// <summary>
    /// Результат главного меню.
    /// </summary>
    internal enum GameOverResult
    {
        /// <summary>
        /// Начать новую игру.
        /// </summary>
        NewGame = 0,
        /// <summary>
        /// В главное меню.
        /// </summary>
        MainMenu = 1,
        /// <summary>
        /// Выйти из игры.
        /// </summary>
        Exit = 2,
    }

    /// <summary>
    /// Сценарий завершения игры.
    /// </summary>
    internal enum GameOverScenario
    {
        /// <summary>
        /// Альберт мертв.
        /// </summary>
        AlbertIsDead = 0,
        /// <summary>
        /// Анна мертва.
        /// </summary>
        AnnaIsDead = 1
    }

    /// <summary>
    /// Пролог.
    /// </summary>
    internal class GameOver : Scene
    {
        /// <summary>
        /// Сценарий завершения игры.
        /// </summary>
        internal GameOverScenario Scenario { get; set; }


        /// <summary>
        ///  Надписи на надгробии.
        /// </summary>
        internal List<TextFrame> TombstounText { get; set; }
        /// <summary>
        /// Картинки для анимационной сцены.
        /// </summary>
        internal Picture TombstonePicture { get; set; }
        internal List<Picture> BackTombstonePictures { get; set; }
        internal List<Picture> GrassPictures { get; set; }
        internal Picture MoonPicture { get; set; }
        internal List<Picture> CloudPictures { get; set; }
        internal List<Picture> BackCloudPictures { get; set; }
        internal List<Picture> RainPictures { get; set; }
        // свойства для правильной прокрутки
        private int grassLength;
        private int cloudLength;
        private int rainTop;
        private const int rainCount = 20;

        /// <summary>
        /// Результат работы меню.
        /// </summary>
        internal GameOverResult Result { get; private set; }

        /// <summary>
        /// Меню.
        /// </summary>
        internal Menu Menu { get; private set; }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            // Меню.
            Menu = new(GameSeed.WindowHeight / 2 - 5, 
                0,
                "ВЫ ПРОИГРАЛИ" + (Scenario == GameOverScenario.AlbertIsDead ? " - Альберт мертв" : " - Анна мертва"),
                "" + ConsoleFieldRenderer.UpArrow + ConsoleFieldRenderer.DownArrow + "-выбрать, Enter-подтвердить");

            Menu.AddItem("Новая игра");
            Menu.AddItem("Главное меню");
            Menu.AddItem("Выход");

            TextFrames.AddRange(Menu.GetFrames());
            Menu.IsVisible = false;

            // Надгробие (средний план).
            int tombstoneTop = GameSeed.WindowHeight - Game.SpriteLibrary.TombstoneSprite.Height;
            int tombstoneLeft = GameSeed.WindowWidth;
            TombstonePicture = new(Game.SpriteLibrary.TombstoneSprite)
            {
                Left = tombstoneLeft,
                Top = tombstoneTop
            };

            // Фоновые надгробия (задний план).
            int backTombstoneTop = GameSeed.WindowHeight - Game.SpriteLibrary.BackTombstoneSpriteSet[0].Height;
            BackTombstonePictures = new()
            {
                new Picture(Game.SpriteLibrary.BackTombstoneSpriteSet[0])
                {
                    Top = backTombstoneTop,
                    Left = GameSeed.WindowWidth * 3 / 100
                },
                new Picture(Game.SpriteLibrary.BackTombstoneSpriteSet[1])
                {
                    Top = backTombstoneTop,
                    Left = GameSeed.WindowWidth * 24 / 100
                },
                new Picture(Game.SpriteLibrary.BackTombstoneSpriteSet[2])
                {
                    Top = backTombstoneTop,
                    Left = GameSeed.WindowWidth * 46 / 100
                },
                new Picture(Game.SpriteLibrary.BackTombstoneSpriteSet[3])
                {
                    Top = backTombstoneTop,
                    Left = GameSeed.WindowWidth * 65 / 100
                },
                new Picture(Game.SpriteLibrary.BackTombstoneSpriteSet[4])
                {
                    Top = backTombstoneTop,
                    Left = GameSeed.WindowWidth * 87 / 100
                },
            };

            // Трава (передний план).
            int grassTop = GameSeed.WindowHeight - Game.SpriteLibrary.GrassLineSpriteSet[0].Height;
            int grassLeft = Game.Accelerator.Rectangle.Left;
            int grassWidth = Game.SpriteLibrary.GrassLineSpriteSet[0].Width;
            GrassPictures = new();

            // Собираем набор достаточной длины.
            grassLength = 0;
            while (grassLength < Game.Accelerator.Rectangle.Width + 2 * grassWidth)
            {
                GrassPictures.Add(
                    new Picture(Game.SpriteLibrary.GrassLineSpriteSet[Randomizer.RandomBetwen(0, Game.SpriteLibrary.GrassLineSpriteSet.Count - 1)])
                    {
                        Top = grassTop,
                        Left = grassLeft + grassLength,
                    });
                grassLength += grassWidth;
            }

            // Фоновые облака
            int backCloudTop = Game.Accelerator.Rectangle.Top;
            int backCloudLeft = Game.Accelerator.Rectangle.Left;
            int backCloudWidth = Game.SpriteLibrary.BackCloudSprite.Width;
            int backCloudLength = 0;
            BackCloudPictures = new();
            while (backCloudLength < Game.Accelerator.Rectangle.Width)
            {
                BackCloudPictures.Add(
                    new Picture(Game.SpriteLibrary.BackCloudSprite)
                    {
                        Top = backCloudTop,
                        Left = backCloudLeft + backCloudLength,
                    });
                backCloudLength += backCloudWidth;
            }

            // Облака (задний план).
            int cloudTop = Game.Accelerator.Rectangle.Top;
            int cloudLeft = Game.Accelerator.Rectangle.Left + 20;
            int cloudWidth = Game.SpriteLibrary.CloudLineSpriteSet[0].Width;
            CloudPictures = new();

            // Собираем набор достаточной длины.
            cloudLength = 0;
            CloudPictures.Add(
                new Picture(Game.SpriteLibrary.CloudLineSpriteSet[0])
                {
                    Top = cloudTop,
                    Left = cloudLeft + cloudLength,
                });
            cloudLength += cloudWidth;
            
            CloudPictures.Add(
                new Picture(Game.SpriteLibrary.CloudLineSpriteSet[1])
                {
                    Top = cloudTop,
                    Left = cloudLeft + cloudLength,
                });
            cloudLength += cloudWidth;

            CloudPictures.Add(
                new Picture(Game.SpriteLibrary.CloudLineSpriteSet[0])
                {
                    Top = cloudTop,
                    Left = cloudLeft + cloudLength,
                });
            cloudLength += cloudWidth;

            // Луна (сверхзадний план).
            MoonPicture = new(Game.SpriteLibrary.MoonSprite)
            {
                Left = Game.Accelerator.Rectangle.Left,
                Top = Game.Accelerator.Rectangle.Top + 1,
            };

            // Дождь (ох...)
            RainPictures = new();

            // Заполняем весь экран, кроме туч и травы.
            rainTop = cloudTop + CloudPictures[0].Height;
            for (int index = 0; index < rainCount; index++)
            {
                Picture rainPicture = new(Game.SpriteLibrary.RainSpriteSet[Randomizer.RandomTo(Game.SpriteLibrary.RainSpriteSet.Count)])
                {
                    Left = Game.Accelerator.Width * index / rainCount + Randomizer.RandomBetwen(-2, 3),
                    Top = Randomizer.RandomBetwen(rainTop, grassTop - Game.SpriteLibrary.RainSpriteSet[0].Width)
                };
                RainPictures.Add(rainPicture);
            }

            // Надписи на надгробии
            tombstoneLeft = 20;
            TombstounText = new()
            {
                new TextFrame()
                {
                    IsVisible = false,
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.DarkGray,
                    AsText = Scenario == GameOverScenario.AlbertIsDead ? "Альберт " : "  Анна  ",
                    Rectangle = new(new Point(tombstoneLeft + 11, tombstoneTop + 2), 8, 1)
                },
                new TextFrame()
                {
                    IsVisible = false,
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.DarkGray,
                    // Альберт родился примерно 30 лет назад, Анна - около 27
                    AsText = DateTime.Today.AddDays(-11111 - (Scenario == GameOverScenario.AlbertIsDead ? 0 : 1111)).ToString("dd.MM.yyyy") + "-",
                    Rectangle = new(new Point(tombstoneLeft + 8, tombstoneTop + 4), 11, 1)
                },
                new TextFrame()
                {
                    IsVisible = false,
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.DarkGray,
                    // Вы умерли сегодня
                    AsText = "-" + DateTime.Today.ToString("dd.MM.yyyy"),
                    Rectangle = new(new Point(tombstoneLeft + 11, tombstoneTop + 5), 11, 1)
                },
            };

            OverlayTextFrames.AddRange(TombstounText);

            Pictures.Add(MoonPicture);
            Pictures.AddRange(BackCloudPictures);
            Pictures.AddRange(CloudPictures);
            Pictures.AddRange(BackTombstonePictures);
            Pictures.Add(TombstonePicture);
            Pictures.AddRange(RainPictures);
            Pictures.AddRange(GrassPictures);

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal GameOver() : base()
        {
            Name = "GameOver";
            Result = GameOverResult.Exit;

            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            Load();
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            bool isStrafe = false;

            if (Menu.IsVisible)
            {

                if (Game.InputController.MoveDown.Impacted)
                // "Стрелка вниз" - Перемещаемся по меню вниз
                {
                    Menu.SelectedIndex++;
                }

                if (Game.InputController.MoveUp.Impacted)
                // "Стрелка вверх" - Перемещаемся по меню вверх
                {
                    Menu.SelectedIndex--;
                }

                if (Game.InputController.Enter.Impacted)
                // "Ввод" - Принимаем выбранный пункт меню
                {
                    Result = (GameOverResult)Menu.SelectedIndex;
                    State = SceneState.Exit;
                }
            }
            else
            {
                if (Game.InputController.Escape.Impacted && !Menu.IsVisible)
                // "Esc" - Показываем меню
                {
                    Menu.IsVisible = true;
                }
            }

            if (20 < TombstonePicture.Left)
            {
                isStrafe = true;
                // Двигаем надгробие.
                TombstonePicture.Left -= 2;

                // Двигаем фоновые надгробия.
                if (IsNumberedTick(1))
                {
                    foreach (var item in BackTombstonePictures)
                    {
                        item.Left--;

                        // Телепортируем картинку за правый край экрана.
                        if (item.Rectangle.Right < Game.Accelerator.Rectangle.Left)
                        {
                            item.Left = Game.Accelerator.Rectangle.Right;
                        }
                    }
                }

                // Двигаем траву.
                if (IsNumberedTick(1))
                {
                    int delta;
                    foreach (var item in GrassPictures)
                    {
                        item.Left -= 4;

                        // Телепортируем картинку в конец очереди.
                        delta = Game.Accelerator.Rectangle.Left - item.Rectangle.Right;
                        if (0 < delta)
                        {
                            item.Left += grassLength;
                        }
                    }
                }

                // Двигаем луну.
                if (IsNumberedTick(10))
                {
                    MoonPicture.Left--;
                }
            }
            else //if (!Menu.IsVisible)
            {
                foreach(var item in TombstounText)
                {
                    item.IsVisible = true;
                }
                // Показываем меню
                Menu.IsVisible = true;
            }

            // Двигаем тучи.
            if (IsNumberedTick(12))
            {
                int delta;
                foreach (var item in CloudPictures)
                {
                    item.Left -= 4;

                    // Телепортируем картинку в конец очереди.
                    delta = Game.Accelerator.Rectangle.Left - item.Rectangle.Right;
                    if (0 < delta)
                    {
                        item.Left += cloudLength;
                    }
                }
            }

            // Двигаем дождь.
            if (IsNumberedTick(1))
            {
                for (int index = 0; index < RainPictures.Count; index++)
                {
                    Picture item = RainPictures[index];

                    item.Left -= 1 + (isStrafe ? 1 : 0);
                    item.Top += 4;

                    // Телепортируем картинку вправо.
                    if (item.Rectangle.Right < Game.Accelerator.Rectangle.Left)
                    {
                        item.Left = Game.Accelerator.Rectangle.Right;
                    }

                    // Пересоздаём картинку где-то сверху.
                    if (Game.Accelerator.Rectangle.Bottom <= item.Top)
                    {
                        item.Left = Game.Accelerator.Width * index / rainCount + Randomizer.RandomBetwen(-2, 3); 

                        item.Top = rainTop + Randomizer.RandomTo(RainPictures[0].Height);
                    }
                }
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
