/*
Главное меню. 
Просмотр кат-сцены, постскриптума, справки, выход или начало новой игры.

2022-12-11 
*/

using Scge;
using PlatformConsole;
using System;
using System.Collections.Generic;

namespace TestGame
{

    /// <summary>
    /// Результат главного меню.
    /// </summary>
    internal enum MainMenuResult
    {
        /// <summary>
        /// Начать новую игру.
        /// </summary>
        NewGame = 0,
        /// <summary>
        /// Показать справку.
        /// </summary>
        Help = 1,
        /// <summary>
        /// Показать интро.
        /// </summary>
        Intro = 2,
        /// <summary>
        /// Показать эпилог.
        /// </summary>
        Postscriptum = 3,
        /// <summary>
        /// Выйти из игры.
        /// </summary>
        Exit = 4,
    }

    /// <summary>
    /// Экран ожидания.
    /// </summary>
    internal class MainMenu : Scene
    {
        // Отдельные элементы тайтла.
        private List<Title> cells;

        /// <summary>
        /// Результат работы меню.
        /// </summary>
        internal MainMenuResult Result { get; private set; }

        /// <summary>
        /// Меню.
        /// </summary>
        internal Menu Menu { get; private set; }

        /// <summary>
        /// Альберт.
        /// </summary>
        internal Picture Albert { get; private set; }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            Albert = new(Game.SpriteLibrary.MainMenuAlbert)
            {
                Left = -2,
                Top = 11,
            };

            Pictures.Add(Albert);

            int row = 1; 
            int column = ((GameSeed.WindowWidth / GameSeed.FieldCellWidth) - 15) / 2;

            // 01234567890
            // ГЕРОЙ ПНВЛ*
            Layer layer = new(this);
            Layers.Add(layer);
            cells = new()
            {
                new(layer.Rows[row][column++]) { SpriteIndex = 0},
                new(layer.Rows[row][column++]) { SpriteIndex = 1 },
                new(layer.Rows[row][column++]) { SpriteIndex = 2 },
                new(layer.Rows[row][column++]) { SpriteIndex = 3 },
                new(layer.Rows[row][column++]) { SpriteIndex = 4 },
                new(layer.Rows[row][column++]) { SpriteIndex = 5 },
                new(layer.Rows[row][column++]) { SpriteIndex = 6 },
                new(layer.Rows[row][column++]) { SpriteIndex = 3 },
                new(layer.Rows[row][column++]) { SpriteIndex = 7 },
                new(layer.Rows[row][column++]) { SpriteIndex = 1 },
                new(layer.Rows[row][column++]) { SpriteIndex = 8 },
                new(layer.Rows[row][column++]) { SpriteIndex = 3 },
                new(layer.Rows[row][column++]) { SpriteIndex = 9 },
                new(layer.Rows[row][column++]) { SpriteIndex = 1 },
                //new(layer.Rows[row][column++]) { SpriteIndex = 10 }
            };

            Menu = new(GameSeed.WindowHeight / 2 - 5, 
                18,
                "ГЛАВНОЕ МЕНЮ",
                "" + ConsoleFieldRenderer.UpArrow + ConsoleFieldRenderer.DownArrow + "-выбрать, Enter-подтвердить");

            Menu.AddItem("Новая игра");
            Menu.AddItem("Справка");
            Menu.AddItem("Интро");
            Menu.AddItem("Постскриптум");
            Menu.AddItem("Выход");

            TextFrames.AddRange(Menu.GetFrames());

            // Строка с копирайтом
            TextFrame copyrightFrame = new(Menu.BackgroundColor, Menu.HelpColor, "Copyright (c) 2023 B48")
            {
                Rectangle = new(new Segment(Menu.Rectangle.Left - 5, Menu.Rectangle.Right + 5), new (Menu.Rectangle.Bottom + 5, Menu.Rectangle.Bottom + 5)),
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            TextFrames.Add(copyrightFrame);

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal MainMenu() : base()
        {
            Name = "MainMenu";
            Result = MainMenuResult.Exit;

            // 100% экрана (17x11)
            SetSize(GameSeed.WindowWidth / GameSeed.FieldCellWidth, GameSeed.WindowHeight / GameSeed.FieldCellHeight);

            // Рендерер ячеек.
            RenderersViewports.Add((Game.FieldRenderer, new() { Rectangle = Rectangle}));
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
                Result = (MainMenuResult)Menu.SelectedIndex;
                State = SceneState.Exit;
            }

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
