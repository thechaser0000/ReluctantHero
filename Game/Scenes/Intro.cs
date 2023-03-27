/*
Интро.
Кат-сцена с выводом логотипа.
В несколько этапов формируем логотип B48.

2022-12-14 
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Экран ожидания.
    /// </summary>
    internal class Intro : Scene
    {
        // Отдельные элементы логотипа.
        private List<Logo> sprites;

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            int row = (GameSeed.WindowHeight / GameSeed.FieldCellHeight) / 2;
            int column = ((GameSeed.WindowWidth / GameSeed.FieldCellWidth) - 7) / 2;

            Layer layer = new(this);
            Layers.Add(layer);
            sprites = new()
            {
                new(layer.Rows[row][column++]) { SpriteIndex = 1, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 2, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 3, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 4, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 5, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 6, IsVisible = false },
                new(layer.Rows[row][column++]) { SpriteIndex = 7, IsVisible = false }
            };

            State = SceneState.Running;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Intro() : base()
        {
            Name = "Intro";
            // 100% экрана (17x11)
            SetSize(GameSeed.WindowWidth / GameSeed.FieldCellWidth, GameSeed.WindowHeight / GameSeed.FieldCellHeight);

            // Рендерер ячеек.
            RenderersViewports.Add((Game.FieldRenderer, new() { Rectangle = Rectangle }));
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
            // Незамысловатая анимация, привязанная к тикам. 
            {
                if (1 == TickCount)
                // отображаем буквы
                {
                    sprites[0].IsVisible = true;
                    sprites[6].IsVisible = true;
                }

                if (2 == TickCount)
                // отображаем буквы
                {
                    sprites[1].IsVisible = true;
                    sprites[5].IsVisible = true;
                }

                if (3 == TickCount)
                // отображаем буквы
                {
                    sprites[2].IsVisible = true;
                    sprites[4].IsVisible = true;
                }

                if (4 == TickCount)
                // отображаем буквы
                {
                    sprites[3].IsVisible = true;
                }

                if (5  < TickCount)
                // двигаем лишние буквы вверх и вниз
                {
                    sprites[1].Move(Direction.Down);
                    sprites[3].Move(Direction.Down);
                    sprites[2].Move(Direction.Up);
                    sprites[4].Move(Direction.Up);

                    // прячем лишние буквы с глаз по достижении границ экрана
                    sprites[2].IsVisible = sprites[2].Cell!.RowIndex != 0;
                    sprites[4].IsVisible = sprites[4].Cell!.RowIndex != 0;
                    sprites[1].IsVisible = sprites[1].Cell!.RowIndex != sprites[1].Cell!.Layer.Rectangle.Bottom;
                    sprites[3].IsVisible = sprites[3].Cell!.RowIndex != sprites[3].Cell!.Layer.Rectangle.Bottom;
                }

                if (5 + 2 == TickCount)
                // трамбуем остальные буквы
                {
                    sprites[5].Move(Direction.Left);
                }

                if (5 + 3 == TickCount)
                // трамбуем остальные буквы
                {
                    sprites[5].Move(Direction.Left);
                }

                if (5 + 5 == TickCount)
                // трамбуем остальные буквы
                {
                    sprites[6].Move(Direction.Left);
                    sprites[0].Move(Direction.Right);
                }

                if (5 + 6 == TickCount)
                // трамбуем остальные буквы
                {
                    sprites[6].Move(Direction.Left);
                    sprites[0].Move(Direction.Right);
                }

                if (5 + 9 == TickCount)
                // подменяем буквы
                {
                    sprites[0].SpriteIndex = 8;
                    sprites[5].SpriteIndex = 9;
                    sprites[6].SpriteIndex = 10;
                }

                if (5 + 15 == TickCount)
                // прячем буквы
                {
                    sprites[0].IsVisible = false;
                    sprites[5].IsVisible = false;
                    sprites[6].IsVisible = false;
                }

                if (5 + 15 < TickCount || Game.InputController.Escape.Impacted)
                // Завершаем сцену через несколько секунд или по Esc
                {
                    State = SceneState.Exit;
                }
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


