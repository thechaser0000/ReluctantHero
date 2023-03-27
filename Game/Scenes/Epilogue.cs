/*
Эпилог. 
Анимационная сцена на спрайтах.

Герой с женой уезжают вдаль.

2022-12-11
*/

using Scge;
using PlatformConsole;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;

namespace TestGame
{
    /// <summary>
    /// Метапозиция картинки относительно дороги.
    /// </summary>
    internal class PictureMetaPosition
    {
        // Секция.
        internal int section;
        // Расстояние от дороги.
        internal int gapToRoad;
    }

    /// <summary>
    /// Эпилог.
    /// </summary>
    internal class Epilogue : Scene
    {
        /// <summary>
        /// Поверхность.
        /// </summary>
        private TextFrame Ground { get; set; }

        /// <summary>
        /// Небо.
        /// </summary>
        private TextFrame Sky { get; set; }

        /// <summary>
        /// Высоко небо.
        /// </summary>
        private TextFrame HighSky { get; set; }

        /// <summary>
        /// Горы.
        /// </summary>
        private List<Picture> Mountines { get; set; }

        /// <summary>
        /// Пул облаков.
        /// </summary>
        private List<Picture> CloudPool { get; set; }

        /// <summary>
        /// Машина.
        /// </summary>
        private Picture Car { get; set; }

        /// <summary>
        /// Дорога.
        /// </summary>
        private List<Picture> Road { get; set; }

        /// <summary>
        /// Пул деревьев.
        /// </summary>
        private List<Picture> TreePool { get; set; }

        /// <summary>
        /// Пул пятен.
        /// </summary>
        private List<Picture> SpotPool { get; set; }

        /// <summary>
        /// Лес полоса 0.
        /// </summary>
        private Picture ForrestLine0 { get; set; }
        /// <summary>
        /// Лес полоса 1.
        /// </summary>
        private Picture ForrestLine1 { get; set; }
        /// <summary>
        /// Лес полоса 2.
        /// </summary>
        private Picture ForrestLine2 { get; set; }

        /// <summary>
        /// Озеро полоса 0.
        /// </summary>
        private Picture LakeLine0 { get; set; }
        /// <summary>
        /// Озеро полоса 1.
        /// </summary>
        private Picture LakeLine1 { get; set; }
        /// <summary>
        /// Озеро полоса 2.
        /// </summary>
        private Picture LakeLine2 { get; set; }

        /// <summary>
        /// Солнце.
        /// </summary>
        private Picture Sun { get; set; }

        private int roadTop;
        private int roadBottom;
        private int roadHorizontalCenter;
        private int horizontalShift;
        private int roadLines;
        private float shiftPerLine;

        /// <summary>
        /// Максимальный модуль сдвига (изгиба) дороги.
        /// </summary>
        private const int MaxRoadShift = 40;
        private const int StraightShift = 5;
        // Горы.
        private const int mountineCount = 40;
        private const int mountineHalfGap = 3;
        // Высота поверхности.
        private const int groundHeight = 20;
        // Облака.
        private const int cloudCount = 8;
        // Деревья.
        private const int treeCount = 3;
        // Пятна.
        private const int spotCount = 6;

        /// <summary>
        ///  Рзметка горизонтальных секций дороги: 1/1/2/3/4/5/8/11/15/.
        /// </summary>
        private readonly int[] sections = new int[] { 0, 1, 2, 4, 7, 11, 16, 24, 35, 50, int.MaxValue };

        /// <summary>
        /// Остаток от деления номера выделенной секции .
        /// </summary>
        private int highlightetSectionModulo = 0;
        
        /// <summary>
        /// Делитель номера выделенной секции (будет выделена каждая X-я секция дороги) .
        /// </summary>
        private readonly int highlightetSectionDivider = 3;

        /// <summary>
        /// Базовая позиция автомобиля.
        /// </summary>
        private Scge.Point carPosition;
        
        /// <summary>
        ///  Победная надпись.
        /// </summary>
        internal List<TextFrame> WinText { get; set; }

        /// <summary>
        /// Ручной контроль управления (вкючается при достижении правого края или при нажатии стрелочек).
        /// </summary>
        private bool isManualControl;

        /// <summary>
        /// Возвращает индекс секции по индексу линии.
        /// </summary>
        private int SectionByLine(int line)
        {
            return Array.FindIndex(sections, a => line <= a);
        }

        /// <summary>
        /// Возвращает ширину секции.
        /// </summary>
        private int GetSectionWidth(int section)
        {
            return (sections[section]+1);
        }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            var rectangle = Game.Accelerator.Rectangle;
            horizontalShift = 0;
            isManualControl = false;

            // Солнце
            Sun = new(Game.SpriteLibrary.EpilogueSun)
            {
                Top = rectangle.Top,
                Left = rectangle.Left - MaxRoadShift - 4,
            };
            Pictures.Add(Sun);

            // Поверхность.
            Ground = new("")
            {
                Color = ConsoleColor.Yellow,
                Rectangle = new(rectangle.Horizontal, new(rectangle.Bottom - groundHeight, rectangle.Bottom)),
            };
            TextFrames.Add(Ground);

            // Высокое небо.
            HighSky = new("")
            {
                Color = ConsoleColor.Blue,
                Rectangle = new(rectangle.Horizontal, new(rectangle.Top, rectangle.Top + 1)),
            };
            TextFrames.Add(HighSky);

            // Небо.
            Sky = new("")
            {
                Color = ConsoleColor.DarkBlue,
                Rectangle = new(rectangle.Horizontal, new(rectangle.Top + 2, Ground.Rectangle.Top - 1)),
            };
            TextFrames.Add(Sky);

            // Дорога 
            roadTop = Ground.Rectangle.Top;
            roadBottom = Ground.Rectangle.Bottom;
            roadHorizontalCenter = rectangle.Horizontal.Middle;
            roadLines = (roadBottom - roadTop + 1) * 2;

            int line = 0;

            Road = new();
            int segmentHalfWidth = 1;
            // Вручную создаём картинки по числу линий.
            for (int index = roadTop; index <= roadBottom; index++)
            {
                // нечетный спрайт линии (верхняя полулиния)
                ConsoleSprite sprite = new(2 * segmentHalfWidth, 1, null, index == roadTop ? ConsoleColor.DarkGray : ConsoleColor.Gray, ConsoleAccelerator.TopSquare);
                ConsoleSprite highliteSprite = new(2 * segmentHalfWidth, 1, null, ConsoleColor.DarkGray, ConsoleAccelerator.TopSquare);
                Picture picture = new(new List<ConsoleSprite>() { sprite, highliteSprite })
                {
                    Top = index,
                    Left = roadHorizontalCenter - (sprite.Width / 2),
                };
                Road.Add(picture);

                // четный спрайт линии (нижняя полулиния)
                sprite = new(2 * segmentHalfWidth, 1, index == roadTop ? ConsoleColor.DarkGray : ConsoleColor.Gray, null, ConsoleAccelerator.BottomSquare);
                highliteSprite = new(2 * segmentHalfWidth, 1, ConsoleColor.DarkGray, null, ConsoleAccelerator.BottomSquare);
                picture = new(new List<ConsoleSprite>() { sprite, highliteSprite })
                {
                    Top = index,
                    Left = roadHorizontalCenter - (sprite.Width / 2),
                };
                Road.Add(picture);
                segmentHalfWidth++;

                line++;
            };
            Pictures.AddRange(Road);

            // Горы.
            Mountines = new();

            // гора свлева от дороги
            Picture newPicture = new(Game.SpriteLibrary.EpilogueMountines[Randomizer.RandomBetwen(0, Game.SpriteLibrary.EpilogueMountines.Count - 1)])
            {
                Left = roadHorizontalCenter - Game.SpriteLibrary.EpilogueMountines[0].Width - mountineHalfGap,
                Top = Ground.Rectangle.Top - Game.SpriteLibrary.EpilogueMountines[0].Height,
            };
            Mountines.Add(newPicture);

            // гора справа от дороги
            newPicture = new(Game.SpriteLibrary.EpilogueMountines[Randomizer.RandomBetwen(0, Game.SpriteLibrary.EpilogueMountines.Count - 1)])
            {
                Left = roadHorizontalCenter + mountineHalfGap,
                Top = Ground.Rectangle.Top - Game.SpriteLibrary.EpilogueMountines[0].Height,
            };
            Mountines.Add(newPicture);

            // много гор
            for (int index = 0; index < (mountineCount - 1) / 2; index++)
            {
                // влево от дороги
                Picture picture = new(Game.SpriteLibrary.EpilogueMountines[Randomizer.RandomBetwen(0, Game.SpriteLibrary.EpilogueMountines.Count - 1)])
                {
                    Left = Randomizer.RandomBetwen(-rectangle.Width / 2, rectangle.Width / 2) - Game.SpriteLibrary.EpilogueMountines[0].Width - mountineHalfGap,
                    Top = Ground.Rectangle.Top - Game.SpriteLibrary.EpilogueMountines[0].Height,
                };
                Mountines.Add(picture);

                // вправо от дороги
                picture = new(Game.SpriteLibrary.EpilogueMountines[Randomizer.RandomBetwen(0, Game.SpriteLibrary.EpilogueMountines.Count - 1)])
                {
                    Left = Randomizer.RandomBetwen(rectangle.Width / 2, 3 * rectangle.Width / 2) + mountineHalfGap,
                    Top = Ground.Rectangle.Top - Game.SpriteLibrary.EpilogueMountines[0].Height,
                };
                Mountines.Add(picture);
            };
            Pictures.AddRange(Mountines);

            // Облака
            CloudPool = new();

            for (int index = 0; index < cloudCount; index++)
            {
                // влево от дороги
                Picture picture = new(Game.SpriteLibrary.EpilogueClouds[Randomizer.RandomBetwen(0, Game.SpriteLibrary.EpilogueClouds.Count - 1)])
                {
                    Left = Randomizer.RandomBetwen(-rectangle.Width / 2, 3 * rectangle.Width / 2 - Game.SpriteLibrary.EpilogueClouds[0].Width),
                    Top = rectangle.Top + 1,
                };
                CloudPool.Add(picture);
            }
            Pictures.AddRange(CloudPool);

            // Машина.
            carPosition = new(roadHorizontalCenter - 2, rectangle.Bottom - Game.SpriteLibrary.EpilogueCar[0].Height);
            Car = new(Game.SpriteLibrary.EpilogueCar)
            {
                Left = carPosition.X,
                Top = carPosition.Y,
            };
            Pictures.Add(Car);

            // Лес
            ForrestLine0 = new(Game.SpriteLibrary.EpilogueForrest[0])
            {
                Top = roadTop - 1,
                Left = Road[sections[1]].Left - Game.SpriteLibrary.EpilogueForrest[0].Width - 20, 
            };
            Pictures.Add(ForrestLine0);

            ForrestLine1 = new(Game.SpriteLibrary.EpilogueForrest[1])
            {
                Top = roadTop,
                Left = Road[sections[4]].Left - Game.SpriteLibrary.EpilogueForrest[1].Width - 25,
            };
            Pictures.Add(ForrestLine1);

            ForrestLine2 = new(Game.SpriteLibrary.EpilogueForrest[2])
            {
                Top = roadTop + 1,
                Left = Road[sections[5]].Left - Game.SpriteLibrary.EpilogueForrest[2].Width - 40,
            };
            Pictures.Add(ForrestLine2);

            // Озеро
            LakeLine0 = new(Game.SpriteLibrary.EpilogueLake[0])
            {
                Top = roadTop,
                Left = Road[sections[1]].Rectangle.Right + 18,
            };
            Pictures.Add(LakeLine0);

            LakeLine1 = new(Game.SpriteLibrary.EpilogueLake[1])
            {
                Top = roadTop + 1,
                Left = Road[sections[4]].Rectangle.Right + 27,
            };
            Pictures.Add(LakeLine1);

            LakeLine2 = new(Game.SpriteLibrary.EpilogueLake[2])
            {
                Top = roadTop + 2,
                Left = Road[sections[5]].Rectangle.Right + 35,
            };
            Pictures.Add(LakeLine2);

            // Пул пятен
            SpotPool = new();
            for (int index = 0; index < spotCount - 1; index++)
            {
                Picture spot = new(Game.SpriteLibrary.EpilogueSpots)
                {
                    Tag = new PictureMetaPosition()
                    {
                        gapToRoad = Randomizer.RandomBetwen(1, 2) * Randomizer.RandomSign(),
                        section = Randomizer.RandomBetwen(0, sections.Length - 3),
                    }
                };
                SpotPool.Add(spot);

                PictureMetaPosition position = (spot.Tag as PictureMetaPosition);
                if (0 < position!.gapToRoad)
                {
                    spot.Left = Road[sections[position!.section]].Rectangle.Right + (position!.gapToRoad - 1) * GetSectionWidth(position!.section) + 1;
                }
                else
                {
                    spot.Left = Road[sections[position!.section]].Left + (position!.gapToRoad + 1) * GetSectionWidth(position!.section) - 1;
                }
                spot.SpriteIndex = position.section + 1;
                spot.Top = roadTop + sections[position.section] / 2;
            }
            Pictures.AddRange(SpotPool);

            // Пул деревьев
            TreePool = new();
            for (int index = 0; index < treeCount - 1; index++)
            {
                Picture tree = new(Game.SpriteLibrary.EpilogueTrees)
                {
                    Tag = new PictureMetaPosition()
                    {
                        gapToRoad = Randomizer.RandomBetwen(1, 3) * Randomizer.RandomSign(),
                        section = Randomizer.RandomBetwen(0, sections.Length - 3),
                    }
                };
                TreePool.Add(tree);

                PictureMetaPosition position = (tree.Tag as PictureMetaPosition);
                if (0 < position!.gapToRoad)
                {
                    tree.Left = Road[sections[position!.section]].Rectangle.Right + (position!.gapToRoad) * GetSectionWidth(position!.section);
                }
                else
                {
                    tree.Left = Road[sections[position!.section]].Left + (position!.gapToRoad) * GetSectionWidth(position!.section);
                }
                tree.SpriteIndex = position.section + 1;
                tree.Top = roadTop + sections[position.section] / 2;

            }
            Pictures.AddRange(TreePool);

            // Победная надпись.
            WinText = new()
            {
                new()
                {
                    AsText = "ВЫ ПОБЕДИЛИ!",
                    Rectangle = new (rectangle.Horizontal, new (3, 3)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BackgroundColor = ConsoleColor.DarkBlue,
                    ForegroundColor = ConsoleColor.Cyan,
                },

                new()
                {
                    Rectangle = new (rectangle.Horizontal, new (5, 8)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BackgroundColor = ConsoleColor.DarkBlue,
                    ForegroundColor = ConsoleColor.DarkCyan,
                }
            };

            WinText[1].Strings.Add("Альберт и Анна вырвались из цепких лап банды");
            WinText[1].Strings.Add("и отправились урегулировать вопросы с законом.");
            WinText[1].Strings.Add("Но наша история на этом заканчивается.");
            WinText[1].Strings.Add("Спасибо, что прошли игру до конца! ");

            TextFrames.AddRange(WinText);

            State = SceneState.Running;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Epilogue() : base()
        {
            Name = "Epilogue";

            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            int roadShift = 0;

            if (999 < TickCount || Game.InputController.Escape.Impacted)
            // Завершаем сцену через несколько секунд или по Esc
            {
                State = SceneState.Exit;
            }

            // ручное управление активируем не раньше чем через 10 тиков
            if (10 < TickCount && Game.InputController.MoveLeft.Impacted && -MaxRoadShift < horizontalShift)
            // Поворачиваем налево
            {
                isManualControl = true;
                roadShift = -1;
                horizontalShift -= 1;
            }
            else if (10 < TickCount && Game.InputController.MoveRight.Impacted && horizontalShift < MaxRoadShift)
            // Поворачиваем направо
            {
                isManualControl = true;
                roadShift = 1;
                horizontalShift += 1;
            }

            // Автоматическое управление.
            if (!isManualControl)
            {
                roadShift = -1;
                horizontalShift -= 1;

                if (horizontalShift <= -MaxRoadShift)
                {
                    isManualControl = true;
                }
            }
            else
            // Ручное управление.
            {
                if (40 < TickCount)
                // Скрываем победные титры.
                {
                    foreach (var item in WinText)
                    {
                        item.IsVisible = false;
                    }
                }
            }

            if (horizontalShift < -StraightShift)
            // Дорога сильно изогнута влево.
            {
                Car.SpriteIndex = roadShift <= 0 ? 0: 1;
            }
            else if (StraightShift < horizontalShift)
            // Дорога сильно изогнута вправо.
            {
                Car.SpriteIndex = 0 <= roadShift? 2 : 1;
            }
            else
            // Дорога почти прямо.
            {
                Car.SpriteIndex = roadShift + 1;
            }

            // Немного смещаем машину на дороге.
            if (IsNumberedTick(3))
            {
                Car.Left = FloatComparer.EnsureRange(Car.Left + Randomizer.RandomBetwen(-1, 1), carPosition.X - 2, carPosition.X + 1);
            }

            // Имитируем поворот дороги, сдвигая линии пропорционально кубу обратного номера их строки
            if (roadShift != 0)
            {
                shiftPerLine = 1f * horizontalShift / (roadLines * roadLines * roadLines);
                int permissibleShift = int.MaxValue;

                for (int line = 0; line < roadLines; line++)
                {
                    int localShift = (int)Math.Round(shiftPerLine * (roadLines - line - 1) * (roadLines - line - 1) * (roadLines - line - 1));

                    if (Math.Abs(permissibleShift) < Math.Abs(localShift))
                    {
                        localShift = permissibleShift;
                    }
                    else
                    {
                        permissibleShift = localShift;
                    }

                    Road[line].Left = roadHorizontalCenter - (Road[line].Width / 2) + localShift;
                }

                // Двигаем лес.
                ForrestLine0.Left = Road[sections[1]].Left - Game.SpriteLibrary.EpilogueForrest[0].Width - 20;
                ForrestLine1.Left = Road[sections[4]].Left - Game.SpriteLibrary.EpilogueForrest[1].Width - 25;
                ForrestLine2.Left = Road[sections[5]].Left - Game.SpriteLibrary.EpilogueForrest[2].Width - 40;

                // Двигаем озеро.
                LakeLine0.Left = Road[sections[1]].Rectangle.Right + 18;
                LakeLine1.Left = Road[sections[4]].Rectangle.Right + 27;
                LakeLine2.Left = Road[sections[5]].Rectangle.Right + 35;
            }

            // Эмулируем движение по дороге вперед.
            if (IsNumberedTick(1))
            {
                if (highlightetSectionModulo < highlightetSectionDivider - 1)
                {
                    highlightetSectionModulo++;
                }
                else
                {
                    highlightetSectionModulo = 0;
                }

                // Двигаем пятна
                foreach (var spot in SpotPool)
                {
                    PictureMetaPosition? position = (PictureMetaPosition)spot.Tag;

                    spot.SpriteIndex = position.section + 1;
                    spot.Top = roadTop + sections[position.section] / 2 - spot.Sprite.Height / 2;

                    if (0 < position!.gapToRoad)
                    {
                        spot.Left = Road[sections[position!.section]].Rectangle.Right + (position!.gapToRoad - 1) * GetSectionWidth(position!.section) + 1;
                    }
                    else
                    {
                        spot.Left = Road[sections[position!.section]].Left + (position!.gapToRoad + 1) * GetSectionWidth(position!.section) - spot.Sprite.Width - 1;
                    }

                    // Телепортируем пятна.
                    if (position.section < (sections.Length - 3))
                    {
                        position.section++;
                    }
                    else
                    {
                        position.gapToRoad = Randomizer.RandomBetwen(1, 2) * Randomizer.RandomSign();
                        position.section = Randomizer.RandomBetwen(0, 2);
                    }
                }

                // Двигаем деревья
                foreach (var tree in TreePool)
                {
                    PictureMetaPosition? position = (PictureMetaPosition)tree.Tag;

                    tree.SpriteIndex = position.section + 1;
                    tree.Top = roadTop + sections[position.section] / 2 - tree.Sprite.Height / 2;

                    if (0 < position!.gapToRoad)
                    {
                        tree.Left = Road[sections[position!.section]].Rectangle.Right + (position!.gapToRoad) * GetSectionWidth(position!.section);
                    }
                    else
                    {
                        tree.Left = Road[sections[position!.section]].Left + (position!.gapToRoad) * GetSectionWidth(position!.section) - tree.Sprite.Width + 1;
                    }

                    // Телепортируем дерево.
                    if (position.section < (sections.Length - 3))
                    {
                        position.section++;
                    }
                    else
                    {
                        position.gapToRoad = Randomizer.RandomBetwen(1, 3) * Randomizer.RandomSign();
                        position.section = Randomizer.RandomBetwen(0, 2);
                    }
                }

                // Окрашиваем дорогу.
                for (int line = 0; line < roadLines; line++)
                {
                    int section = SectionByLine(line);

                    // Окрашиваем секцию.
                    Road[line].SpriteIndex = section % highlightetSectionDivider == highlightetSectionModulo ? 0 : 1;
                }
            }

            // Двигаем горы, облака и солнце при поворотах
            // Довольно быстро накапливается ошибка и дорога на горизонте утыкается в горы. Да и ладно.
            if (roadShift != 0)
            {
                foreach (var item in Mountines)
                {
                    item.Left += roadShift;
                };

                foreach (var item in CloudPool)
                {
                    item.Left += roadShift;
                };

                Sun.Left += roadShift;
            }

            // Мерцаем солнцем
            if (IsNumberedTick(1))
            {
                Sun.NextSprite();
            }

            // Двигаем облака от ветра
            if (IsNumberedTick(8))
            {
                foreach (var item in CloudPool)
                {
                    item.Left++;

                    // Переносим облако влево.
                    if ((3 * Game.Accelerator.Rectangle.Width / 2) < item.Left)
                    {
                        item.Left = - Game.Accelerator.Rectangle.Width / 2 + Randomizer.RandomBetwen(-10, 10);
                    }
                };
            }

            // Передаём управление сущностям.
            base.DoTick();

            // проверяем условия победы и поражения.
            //            CheckEvents();
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
