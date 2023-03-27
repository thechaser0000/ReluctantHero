/*
Пролог. 
Кат-сцена.

2022-12-11 
*/

using Scge;
using PlatformConsole;
using System.Diagnostics;

namespace TestGame
{
    /// <summary>
    /// Пролог.
    /// </summary>
    internal class Prologue : Scene
    {
        /// <summary>
        /// Неподвижная земля.
        /// </summary>
        private List<Simple> fixedGroundEntities;

        /// <summary>
        /// Подвижная земля.
        /// </summary>
        private List<ConsoleEntity> mobileGroundEntities;

        /// <summary>
        /// Машина Альберта.
        /// </summary>
        private List<CarPart> albertsCar;

        /// <summary>
        /// Машина девушки.
        /// </summary>
        private List<CarPart> ladysCar;

        /// <summary>
        /// Альберт.
        /// </summary>
        private Hero albert;

        /// <summary>
        /// Девушка.
        /// </summary>
        private Lady lady;

        /// <summary>
        /// Бандит.
        /// </summary>
        private Bandit bandit;

        /// <summary>
        /// Вспышка.
        /// </summary>
        private Square splash;

        /// <summary>
        /// Ширина сцены, ячеек.
        /// </summary>
        const int SceneWidth = 7;

        /// <summary>
        /// Фрейм истории.
        /// </summary>
        private TextFrame storyFrame;

        /// <summary>
        /// История.
        /// </summary>
        private List<string> story;

        /// <summary>
        /// Строка истории.
        /// </summary>
        private int storyLine;

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            storyLine = 0;

            // формируем неподвижный слой
            Layer fixedGround = new(this);
            Layers.Add(fixedGround);
            // шоссе
            fixedGroundEntities = new()
            {
                new HighwayLine(fixedGround.Columns[2]) { Direction = Direction.Up, Position = HighwayLinePosition.Left },
                new HighwayLine(fixedGround.Columns[3]) { Direction = Direction.Up, Position = HighwayLinePosition.Center },
                new HighwayLine(fixedGround.Columns[4]) { Direction = Direction.Up, Position = HighwayLinePosition.Right }
            };

            // песок
            for (int column = 0; column < SceneWidth; column++)
            {
                if ( column < 2 || 4 < column)
                {
                    fixedGroundEntities.Add(new SandGround(fixedGround.Columns[column]));
                }
            }

            // формируем подвижный слой
            Layer mobileGround = new(this);
            Layers.Add(mobileGround);
            Tree tree = new();
            tree.Cells.Add(mobileGround.Columns[0][15]);
            tree.Cells.Add(mobileGround.Columns[0][5]);
            tree.Cells.Add(mobileGround.Columns[5][6]);
            tree.Cells.Add(mobileGround.Columns[6][1]);
            tree.Cells.Add(mobileGround.Columns[6][13]);

            // Тропы размещаем вручную.
            SandPath horizontalSandPath = new() { Direction = Direction.Left };
            SandPath verticalSandPath = new() { Direction = Direction.Up };
            horizontalSandPath.Cells.Add(mobileGround.Columns[1][3]) ;
            horizontalSandPath.Cells.Add(mobileGround.Columns[0][11]);
            verticalSandPath.Cells.Add(mobileGround.Columns[5][2]);
            verticalSandPath.Cells.Add(mobileGround.Columns[1][5]);

            verticalSandPath.Cells.Add(mobileGround.Columns[5][8]);
            verticalSandPath.Cells.Add(mobileGround.Columns[5][9]);
            verticalSandPath.Cells.Add(mobileGround.Columns[5][10]);
            verticalSandPath.Cells.Add(mobileGround.Columns[5][11]);
            horizontalSandPath.Cells.Add(mobileGround.Columns[6][10]);

            mobileGroundEntities = new()
            {
                tree,
                horizontalSandPath,
                verticalSandPath
            };
            Entities.AddRange(mobileGroundEntities);

            // Формируем слой взаимодействия.
            Layer interaction = new(this);
            Layers.Add(interaction);

            // Тачка героя
            albertsCar = new()
            {
                new(null!, interaction.Columns[4][^3]) { Direction = Direction.Up, Position = CarPartPosition.Front},
                new(null!, interaction.Columns[4][^2]) { Direction = Direction.Up, Position = CarPartPosition.Center},
                new(null!, interaction.Columns[4][^1]) { Direction = Direction.Up, Position = CarPartPosition.Rear}
            };

            // Герой.
            albert = new() { Cell = interaction.Columns[3][^6], IsVisible = false, Direction = Direction.Left};
            Entities.Add(albert);

            // Девушка.
            lady = new(interaction.Columns[3][1]) { Direction = Direction.Down };
//            Entities.Add(lady);

            // Бандит.
            bandit = new(interaction.Columns[5][4]) { Direction = Direction.Up, IsVisible = false };

            // Машина девушки
            ladysCar = new()
            {
                new CarPart(null!, interaction.Columns[4][2]) {Direction = Direction.Up, BodyStyle = CarBodyStyle.Pickup, Position = CarPartPosition.Rear},
                new CarPart(null!, interaction.Columns[4][1]) {Direction = Direction.Up, BodyStyle = CarBodyStyle.Pickup, Position = CarPartPosition.Center},
                new CarPart(null!, interaction.Columns[4][0]) {Direction = Direction.Up, BodyStyle = CarBodyStyle.Pickup, Position = CarPartPosition.Front}
            };

            // формируем слой вспышки
            Layer splashLayer = new(this);
            Layers.Add(splashLayer);
            splash = new(splashLayer.Cells)
            {
                IsVisible = false,
                SpriteIndex = (int)ConsoleColor.White
            };

            // Фрейм истории
            storyFrame = new(GameSeed.TransparentColor, ConsoleColor.DarkYellow) { 
                HorizontalAlignment = HorizontalAlignment.Left, 
                VerticalAlignment = VerticalAlignment.Bottom, 
                Rectangle = new(SceneWidth * GameSeed.FieldCellWidth + 6, 2, GameSeed.WindowWidth - 5, GameSeed.WindowHeight - 3) };
            TextFrames.Add(storyFrame);

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Prologue() : base()
        {
            Name = "Prologue";
            // 100% экрана
            SetSize(GameSeed.WindowWidth / GameSeed.FieldCellWidth, GameSeed.WindowHeight / GameSeed.FieldCellHeight + 6);

            // Рендерер ячеек.
            RenderersViewports.Add((Game.FieldRenderer, new() { Rectangle = new(new Segment(0, SceneWidth - 1), new(3, GameSeed.WindowHeight / GameSeed.FieldCellHeight + 2)) }));
            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            // Инициализируем историю
            story = new()
            {
                // 50 символов

                new("                      ПРОЛОГ                      "),
                new(""),
                new("    Альберт наслаждался отпуском, путешествуя на  "),
                new("машине по диким местам вместе со своей женой Анной"),
                new(""),
                new(""),
                new(""),
                new("    Мчась  по  трассе,  они  заметили  стоящий  на"),
                new("дороге  пикап  и  девушку,  делающую  знаки       "),
                new("остановиться."),
                new(""),
                new(""),
                new("    Альберт припарковался на обочине и вышел из   "),
                new("машины. От девушки он унал, что пикап заглох и не "),
                new("заводится."),
                new(""),
                new(""),
                new("    Подойдя к открытому капоту, Альберт склонился "),
                new("над двигателем но тут же получил удар в затылок..."),
                new(""),
                new(""),
                new(""),
                new(""),
                new(""),
                new(""),
                new(""),
                new(""),
                new(""),
                new("")
            };
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            // Выводим историю
            if (IsNumberedTick(4))
            {
                if (storyLine < story.Count)
                {
                    storyFrame.Strings.Add(story[storyLine]);
                    storyLine++;
                }
                else
                {
                    // Завершаем сцену
                    State = SceneState.Exit;
                }
            }

            // первые 30 тиков едем по 2 клетки за такт
            if (TickCount < 25)
            {
                // Прокуручиваем фон вниз, постепенно останавливаясь
                foreach (var entity in mobileGroundEntities)
                {
                    entity.Move(Direction.Down, true);
                }

                foreach (var entity in mobileGroundEntities)
                {
                    entity.Move(Direction.Down, true);
                }
            }
            else
            {
                if (TickCount < 33)
                {
                    // Прокуручиваем фон вниз, постепенно останавливаясь
                    foreach (var entity in mobileGroundEntities)
                    {
                        entity.Move(Direction.Down, true);
                    }
                }
                else if (IsNumberedTick(2) && TickCount <= 38)
                {
                    // Прокуручиваем фон вниз, постепенно останавливаясь
                    foreach (var entity in mobileGroundEntities)
                    {
                        entity.Move(Direction.Down, true);
                    }
                }
            }

            // появление машины альберта
            if (0 < TickCount && TickCount <= 12 && IsNumberedTick(3))
            {
                foreach (var car in albertsCar)
                {
                    car.Move(Direction.Up);
                }
            }

            // появление машины девушки (остановка)
            if (TickCount == 40 || TickCount == 42 || TickCount == 45 || TickCount == 48 || TickCount == 52)
            {
                lady.Move(Direction.Down);

                foreach (var car in ladysCar)
                {
                    car.Move(Direction.Down);
                }
                // Прокуручиваем фон вниз, постепенно останавливаясь
                foreach (var entity in mobileGroundEntities)
                {
                    entity.Move(Direction.Down, true);
                }
            }

            // альберт выходит из машины и идёт к девушке
            if (54 < TickCount)
            {
                switch (TickCount)
                {
                    // Альберт вышел из машины
                    case 55:
                        albert.IsVisible = true;
                        break;
                    // Альберт идёт к девушке и стоит
                    case 58:
                        albert.Direction = Direction.Up;
                        break;
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                        albert.Move(Direction.Up);
                        break;
                    // Альберт обходит девушку
                    case 72:
                        albert.Direction = Direction.Left;
                        albert.Move(Direction.Left);
                        break;
                    case 73:
                    case 74:
                    case 75:
                        albert.Direction = Direction.Up;
                        albert.Move(Direction.Up);
                        break;
                    // Альберт идёт к капоту
                    case 76:
                    case 77:
                        lady.Direction = Direction.Up;
                        lady.Move(Direction.Up);

                        albert.Direction = Direction.Right;
                        albert.Move(Direction.Right);
                        break;
                    case 78:
                        lady.Move(Direction.Up);
                        bandit.Move(Direction.Up);
                        bandit.IsVisible = true;

                        albert.Direction = Direction.Down;
                        break;
                    case 79:
                        bandit.Move(Direction.Left);
                        bandit.Direction = Direction.Left;

                        lady.Direction = Direction.Right;
                        lady.Move(Direction.Right);
                        break;
                    case 80:
                        bandit.Move(Direction.Down);
                        bandit.Direction = Direction.Down;
                        break;
                    case 85:
                        bandit.IsAttack = true;
                        break;

                    // Вспышка и конец сцены
                    case 86: 
                        albert.IsDead = true;
                        break;
                    case 87: // +
                    case 88: // --
                    case 90: // +
                        bandit.IsAttack = false;
                        splash.IsVisible = !splash.IsVisible;
                        break;
                    case 87 + 4: // off
                        splash.SpriteIndex = (int)GameSeed.TransparentColor;
                        splash.IsVisible = true;
                        break;

                }
            }

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
