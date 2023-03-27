/*
Предок игровых сцен.
Содержит базовый функционал для игровых сцен.

2023-01-03 
*/

using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Результат эпизода.
    /// </summary>
    internal enum EpisodeResult
    {
        /// <summary>
        /// Прервать.
        /// </summary>
        Abort = 0,
        /// <summary>
        /// Победа.
        /// </summary>
        Win = 1,
        /// <summary>
        /// Поражение.
        /// </summary>
        Fail = 2,
    }

    /// <summary>
    /// Предок игровых сцен.
    /// </summary>
    internal abstract class Episode : Scene
    {
        /// <summary>
        /// Слой поверхности.
        /// </summary>
        internal protected Layer TerrainLayer { get; set; }

        /// <summary>
        /// Слой взаимодействия.
        /// </summary>
        internal protected Layer InteractionLayer { get; set; }

        /// <summary>
        /// Слой тумана войны.
        /// </summary>
        internal protected Layer FogOfWarLayer { get; set; }

        /// <summary>
        /// Туман войны.
        /// </summary>
        internal protected FogOfWar FogOfWar { get; set; }

        /// <summary>
        /// Результат эпизода.
        /// </summary>
        public EpisodeResult Result { get; protected set; }

        /// <summary>
        /// Герой.
        /// </summary>
        internal Hero Hero => Game.Hero;

        /// <summary>
        /// Вьюпорт карты.
        /// </summary>
        internal protected Viewport MapViewport { get; init; }

        /// <summary>
        /// Вьюпорт игрового поля.
        /// </summary>
        internal protected Viewport FieldViewport { get; init; }

        /// <summary>
        /// Таймер победы.
        /// (Тиков до победы).
        /// </summary>
        internal protected int LeftToWin { get; set; }

        /// <summary>
        /// Таймер поражения.
        /// (Тиков до поражения).
        /// </summary>
        internal protected int LeftToFail { get; set; }

        /// <summary>
        /// Меню паузы.
        /// </summary>
        internal protected Menu PauseMenu { get; set; }

        /// <summary>
        /// Индикатор.
        /// </summary>
        internal protected Hud Hud { get; set; }

        /// <summary>
        /// Фрейм истории.
        /// </summary>
        internal protected TextFrame storyFrame;

        /// <summary>
        /// История.
        /// </summary>
        internal protected List<string> story;

        /// <summary>
        /// Строка истории.
        /// </summary>
        internal protected int storyLine;

        /// <summary>
        /// Пауза.
        /// </summary>
        internal protected bool IsPause { get; set; }

        /// <summary>
        /// История.
        /// </summary>
        internal protected bool IsStory { get; set; }

        /// <summary>
        /// Сгенерировать фрейм истории.
        /// </summary>
        protected virtual void GenerateHistory()
        {
            int horizontalMargin = (GameSeed.WindowWidth - story[0].Length) / 2;
            int verticalMargin = (GameSeed.WindowHeight - story.Count) / 2;

            // Фрейм истории
            storyFrame = new(GameSeed.TransparentColor, ConsoleColor.DarkYellow)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Rectangle = new(GameSeed.WindowWidth, GameSeed.WindowHeight),
                Margins = new(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin)
            };
            TextFrames.Add(storyFrame);
        }

        /// <summary>
        /// сгенерировать фрейм паузы.
        /// </summary>
        internal protected void GeneratePause()
        {
            PauseMenu = new(GameSeed.WindowHeight / 2 - 5,
                0,
                "ПАУЗА",
                "" + ConsoleFieldRenderer.UpArrow + ConsoleFieldRenderer.DownArrow + "-выбрать, Enter-подтвердить");
            PauseMenu.IsVisible = false;

            PauseMenu.AddItem("Продолжить");
            PauseMenu.AddItem("Информация");
            PauseMenu.AddItem("Главное меню");
            PauseMenu.AddItem("Выход");

            TextFrames.AddRange(PauseMenu.GetFrames());
            IsPause = true; 
        }

        /// <summary>
        /// Сгенерировать индикатор.
        /// </summary>
        internal protected void GenerateHud()
        {
            // Индикатор.
            Hud = new(
                new(GameSeed.InterfaceLeft, GameSeed.InterfaceTop, GameSeed.InterfaceLeft + GameSeed.InterfaceWidth - 1, GameSeed.InterfaceTop + GameSeed.InterfaceHeight - 1),
                ConsoleColor.White,
                ConsoleColor.Gray,
                ConsoleColor.Red,
                ConsoleColor.Cyan,
                ConsoleColor.DarkYellow,
                ConsoleColor.DarkGreen)
            { IsVisible = false };

            TextFrames.AddRange(Hud.GetFrames());
            ProgressBars.AddRange(Hud.GetProgressBars());
            Pictures.AddRange(Hud.GetPictures());
        }

        /// <summary>
        /// Установить вьюпорты.
        /// </summary>
        internal protected void SetVieports()
        {
            // вьюпорты следует за героем
            FieldViewport.CenterOn(this, Hero!.Cells.Center!.Point);
            // вьюпорты следует за героем
            MapViewport.CenterOn(this, Hero!.Cells.Center!.Point);
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

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            // Приближаем победу.
            if (0 < LeftToWin)
            {
                LeftToWin--;
            }

            // Приближаем поражение.
            if (0 < LeftToFail)
            {
                LeftToFail--;
            }

            // Начинаем игру/включаем паузу/выключаем паузу по Esc
            if (Game.InputController.Escape.Impacted)
            {
                if (!storyFrame.IsVisible)
                // История скрыта (ставим на паузу / снимаем с паузы)
                {
                    IsPause = !IsPause;
                    PauseMenu.IsVisible = IsPause;
                    PauseMenu.SelectedIndex = 0;
                }
                else
                // Отображена история (снимаем с паузы)
                {
                    storyFrame.IsVisible = false;
                    Hud.IsVisible = true;
                    IsPause = false;
                }
            }

            if (PauseMenu.IsVisible)
            {
                if (Game.InputController.MoveDown.Impacted)
                // "Стрелка вниз" - Перемещаемся по меню вниз
                {
                    PauseMenu.SelectedIndex++;
                }

                if (Game.InputController.MoveUp.Impacted)
                // "Стрелка вверх" - Перемещаемся по меню вверх
                {
                    PauseMenu.SelectedIndex--;
                }

                if (Game.InputController.Enter.Impacted)
                // "Ввод" - Принимаем выбранный пункт меню
                {
                    switch (PauseMenu.SelectedIndex)
                    {
                        // Продолжить
                        case 0:
                            IsPause = false;
                            PauseMenu.IsVisible = false;
                            break;
                        // Информация
                        case 1:
                            Hud.IsVisible = false;
                            storyFrame.IsVisible = true;
                            PauseMenu.IsVisible = false;
                            break;
                        // Главное меню
                        case 2:
                            Result = EpisodeResult.Abort;
                            State = SceneState.Exit;
                            break;
                        // Выход
                        case 3:
                            Result = EpisodeResult.Abort;
                            State = SceneState.Exit;
                            Game.GoToSystem();
                            break;
                    }
                }

            }

            // Выводим историю
            if (storyFrame.IsVisible && Engine.IsNumberedTick(1))
            {
                if (storyLine < story.Count)
                {
                    storyFrame.Strings.Add(story[storyLine]);
                    storyLine++;
                }
            }

            if (!IsPause)
            {
                // Передаём управление сущностям.
                base.DoTick();
            }

            Hud.Update();

            // проверяем условия победы и поражения.
            CheckEvents();
            PerformanceStatistics.EndLogic();

            // Отрисовываем.
            DoRendering();
            PerformanceStatistics.EndRendering();
        }

        /// <summary>
        /// Проверяем события в игре.
        /// </summary>
        internal protected virtual void CheckEvents()
        {
            // проверяем опыт
            if (Hero.Experience.IsMaximum)
            // Опыт на максимуме - повышаем уровень и сбрасываем опыт.
            {
                Hero.EvolutionLevel++;
                // Пересчитываем максимумы характеристик (+10% за каждый уровень)
                Hero.Health.Maximum = Hero.Health.Maximum * 11 / 10;
                Hero.ShotGun.Ammo.Maximum = Hero.ShotGun.Ammo.Maximum * 11 / 10;
                // Пересчитываем опыт, необходимый для роста уровня (нужно набрать на +62% больше за каждый новый уровень)
                Hero.Experience.Maximum += (int)(Math.Pow(1.62f, Hero.EvolutionLevel) * GameSeed.HeroBaseExperience);
            }

            // Проверяем таймер поражения.
            if (0 == LeftToFail)
            {
                Result = EpisodeResult.Fail;
                State = SceneState.Exit;
            }

            // Проверяем таймер победы.
            if (0 == LeftToWin)
            {
                Result = EpisodeResult.Win;
                State = SceneState.Exit;
            }
        }

        /// <summary>
        /// Сгенерировать трофей из сущности в зависимости от её типа. 
        /// </summary>
        internal void GenerateTrophy(object sender, EntityEventArgs e)
        {
            // Корова - даёт аптечку.
            if (e.Entity is Cow cow)
            {
                new HealthTrophy(StructureGenerator.GetNearestEmptyCell(TerrainLayer, cow.Cell));
            }

            // Преследователь/страж - дает аптечку, реже - патроны или ничего.
            if (e.Entity is Chaser chaser)
            {
                // REF эти вероятности можно вынести в зерно
                if (Randomizer.ItIsTrue(0.5))
                {
                    new HealthTrophy(StructureGenerator.GetNearestEmptyCell(TerrainLayer, chaser.Cell));
                }
                else
                {
                    if (Randomizer.ItIsTrue(0.5))
                    {
                        new AmmoTrophy(StructureGenerator.GetNearestEmptyCell(TerrainLayer, chaser.Cell));
                    }
                }
            }

            // Стрелок/снайпер - даёт пушку (если у героя нет) и патроны.
            if (e.Entity is Shooter shooter)
            {
                if (!Hero.ShotGun.IsEnabled)
                {
                    new ShotGunTrophy(StructureGenerator.GetNearestEmptyCell(TerrainLayer, shooter.Cell));
                }
                new AmmoTrophy(StructureGenerator.GetNearestEmptyCell(TerrainLayer, shooter.Cell));
            }
        }

        /// <summary>
        /// Загрузить образ.
        /// </summary>
        internal protected abstract void LoadImage();

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            LeftToWin = -1;
            LeftToFail = -1;

            IsPause = false;
            storyLine = 0;

            // Создаем слои
            TerrainLayer = new(this);
            Layers.Add(TerrainLayer);
            InteractionLayer = new(this);
            Layers.Add(InteractionLayer);
            FogOfWarLayer = new(this) { IsVisible = true };
            Layers.Add(FogOfWarLayer);

            LoadImage();

            GenerateFogOfWar();
            GenerateGround();

            GenerateHud();
            GenerateHistory();
            GeneratePause();
        }

        /// <summary>
        /// Разогнать туман войны.
        /// </summary>
        internal protected void FreeFogOfWar()
        {
            List<Cell> fogOfWarFreeList = new();
            int fogOfWarFreeCenterColumn = Hero!.Cells.Center!.ColumnIndex;
            int fogOfWarFreeCenterRow = Hero!.Cells.Center!.RowIndex;
            fogOfWarFreeList.AddRange(StructureGenerator.GetEllipse(FogOfWarLayer,
                fogOfWarFreeCenterColumn - GameSeed.FogOfWarFreeDistance,
                fogOfWarFreeCenterRow - GameSeed.FogOfWarFreeDistance,
                fogOfWarFreeCenterColumn + GameSeed.FogOfWarFreeDistance,
                fogOfWarFreeCenterRow + GameSeed.FogOfWarFreeDistance, 0));
            FogOfWar.Cells.Remove(fogOfWarFreeList);
        }

        /// <summary>
        /// Герой перместился.
        /// </summary>
        internal protected void HeroMoved(object sender, EntityEventArgs e)
        {
            if (State is SceneState.Running)
            {
                SetVieports();
                FreeFogOfWar();
            }
        }

        /// <summary>
        /// Образ.
        /// </summary>
        internal protected SceneImage Image { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Episode() : base()
        {
            // Создаём области просмотра.
            MapViewport = new();
            FieldViewport = new();

            // рендерер игрового поля
            FieldViewport.SetBounds(GameSeed.FieldColumns, GameSeed.FieldLines);
            RenderersViewports.Add((Game.FieldRenderer, FieldViewport));
            // рендерер карты
            MapViewport.SetBounds(GameSeed.MapColumns, GameSeed.MapLines);
            RenderersViewports.Add((Game.MapRenderer, MapViewport));
            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            Result = EpisodeResult.Abort;
        }

        /// <summary>
        /// Сгенерировать территорию.
        /// </summary>
        internal protected abstract void GenerateGround();

        /// <summary>
        /// Генерировать героя.
        /// </summary>
        internal protected virtual void GenerateHero()
        {
            Entities.Add(Hero);
            // обработка перемещения героя (вьюпорт, туман)
            Hero.Moved += HeroMoved;
            // обработка создания героем сущностей
            Hero.SpriteSelector.SpriteSelection -= Hero.SpriteSelection;
            Hero.SpriteSelector.SpriteSelection += Hero.SpriteSelection;
            Hero.RelatedLayer = TerrainLayer;
        }

        /// <summary>
        /// Сгененрировать туман войны.
        /// </summary>
        internal protected void GenerateFogOfWar()
        {
            // генерируем туман
            FogOfWar = new(StructureGenerator.GetRectangle(FogOfWarLayer, Rectangle, 0));
        }

        /// <summary>
        /// Очистить сцену.
        /// </summary>
        internal override void Clear()
        {
            FogOfWar.Clear();

            Hero.Clear();
            Hero.Moved -= HeroMoved;

            base.Clear();
        }
        
        /// <summary>
        /// Запустить сцену.
        /// (Поменять статус на Активна)
        /// </summary>
        internal override void StartOrResume()
        {
            base.StartOrResume();

            GenerateHero();
            SetVieports();
        }
    }
}
