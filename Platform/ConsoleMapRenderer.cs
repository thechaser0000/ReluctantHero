/*
Рендерер для отрисовки карты в консоль.
Заменят 2 смежные по вертикали ячейки одним символом псевдографики.
Использует ConsoleAccelerator для оптимизации вывода в консоль Winwows.
Позволяет установить свой обработчик рисования ячеек.
Можно использовать повторно.

2022-10-10
*/

using Scge;
using TestGame;

namespace PlatformConsole
{

    /// <summary>
    /// Аргументы обработчика отрисовки ячейки карты.
    /// </summary>
    internal class MapCellDrawingEventArgs : EventArgs
    {
        public MapCellDrawingEventArgs(Cell? topCell, Cell? bottomCell, ConsoleColor backgroundColor, ConsoleColor foregroundColor, Scge.Point position, char character)
        {
            TopCell = topCell;
            BottomCell = bottomCell;
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            Position = position;
            Character = character;
        }

        /// <summary>
        /// Верхняя отрисовываемая ячейка.
        /// </summary>
        public Cell? TopCell { get; init; }

        /// <summary>
        /// Нижняя отрисовываемая ячейка.
        /// </summary>
        public Cell? BottomCell { get; init; }

        /// <summary>
        /// Цвет фона.
        /// </summary>
        public ConsoleColor BackgroundColor { get; init; }

        /// <summary>
        /// Цвет символов.
        /// </summary>
        public ConsoleColor ForegroundColor { get; init; }

        /// <summary>
        /// Прямоугольник вывода в консоль.
        /// </summary>
        public Scge.Point Position { get; init; }

        /// <summary>
        /// Текст
        /// </summary>
        public char Character { get; init; }
    }

    /// <summary>
    /// Рендерер для отрисовки карты в консоль
    /// </summary>
    internal class ConsoleMapRenderer : Renderer
    {
        /// <summary>
        /// Обработчик отрисовки ячейки.
        /// </summary>
        internal delegate void MapCellDrawingEventHandler(object sender, MapCellDrawingEventArgs e);

        /// <summary>
        /// Разрешена пользовательская отрисовка.
        /// </summary>
        internal bool CustomDraw { get; set; }

        /// <summary>
        /// Событие самостоятельной отрисовки ячейки
        /// </summary>
        internal event MapCellDrawingEventHandler CellDrawing;
        
        /// <summary>
        /// Ускоритель вывода на консоль.
        /// </summary>
        private ConsoleAccelerator Accelerator { get; init; }

        /// <summary>
        /// Цвет карты по умолчанию.
        /// </summary>
        internal ConsoleColor DefaultColor { get; init; }

        /// <summary>
        /// Цвет за пределами карты (если размер карты превышает размер сцены).
        /// </summary>
        internal ConsoleColor OutOfBoundsColor { get; init; }

        /// <summary>
        /// Цвет полосы прокрутки.
        /// </summary>
        internal ConsoleColor ScrollBarColor { get; init; }
        /// <summary>
        /// Цвет сллайдера полосы прокрутки.
        /// </summary>
        internal ConsoleColor ScrollBarTrackerColor { get; init; }

        /// <summary>
        /// Осуществить предварительный рендеринг окружения (скроллбары).
        /// </summary>
        private void RenderEnvironment(Scene scene, Viewport viewport)
        {
            int line = Top + (viewport.Height + 1) / 2;

            // горизонтальный скроллбар
            ScrollBar horizontalScrollBar = new(scene.Width, viewport.Width, viewport.Width,  viewport.StartColumnIndex);

            Accelerator.SetCursorPosition(Left, line++);
            Accelerator.BackgroundColor = TransparentColor;
            char scrollBarChar = viewport.Height % 2 == 0 ? ConsoleAccelerator.BottomSquare : ConsoleAccelerator.TopSquare;
            for (int scrollBarIndex = 0; scrollBarIndex < horizontalScrollBar.ScrollBarLength; scrollBarIndex++)
            {
                bool inTracker = horizontalScrollBar.TrackerOffset <= scrollBarIndex && scrollBarIndex <= horizontalScrollBar.TrackerOffset + horizontalScrollBar.TrackerLength;

                Accelerator.ForegroundColor = inTracker ? ScrollBarTrackerColor : ScrollBarColor;
                Accelerator.Write(scrollBarChar);
            }

            // вертикальный скроллбар
            ScrollBar verticalScrollBar = new(scene.Height, viewport.Height,  viewport.Height, viewport.StartRowIndex);

            Accelerator.BackgroundColor = ScrollBarTrackerColor;

            int scrollBarLine = Top;
            for (int scrollBarIndex = 0; scrollBarIndex < verticalScrollBar.ScrollBarLength; scrollBarIndex++)
            {
                bool inTracker = verticalScrollBar.TrackerOffset <= scrollBarIndex && scrollBarIndex <= verticalScrollBar.TrackerOffset + verticalScrollBar.TrackerLength;

                if (scrollBarIndex % 2 == 0)
                {
                    // первый полусимвол слайдера
                    if (scrollBarIndex + 1 == verticalScrollBar.TrackerOffset)
                    {

                        Accelerator.SetCursorPosition(Left + viewport.Width + 1, scrollBarLine++);
                        Accelerator.SetColors(ScrollBarColor, ScrollBarTrackerColor);
                        Accelerator.Write(ConsoleAccelerator.BottomSquare);
                    }
                    // последний полусимвол слайдера
                    else if (scrollBarIndex == verticalScrollBar.TrackerOffset + verticalScrollBar.TrackerLength)
                    {

                        Accelerator.SetCursorPosition(Left + viewport.Width + 1, scrollBarLine++);
                        Accelerator.ForegroundColor = ScrollBarTrackerColor;
                        Accelerator.BackgroundColor = viewport.Height % 2 == 1 && scrollBarIndex == verticalScrollBar.ScrollBarLength - 1 ? TransparentColor : ScrollBarColor;
                        Accelerator.Write(ConsoleAccelerator.TopSquare);
                    }
                    else
                    {
                        Accelerator.SetCursorPosition(Left + viewport.Width + 1, scrollBarLine++);

                        if (viewport.Height % 2 == 1 && scrollBarIndex == verticalScrollBar.ScrollBarLength - 1)
                        // при нечетном количестве строк "укорачиваем" скроллбар
                        {
                            Accelerator.ForegroundColor = inTracker ? ScrollBarTrackerColor : ScrollBarColor;
                            Accelerator.BackgroundColor = TransparentColor;
                            Accelerator.Write(ConsoleAccelerator.TopSquare);
                        }
                        else
                        {
                            Accelerator.ForegroundColor = inTracker ? ScrollBarTrackerColor : ScrollBarColor;
                            Accelerator.Write(ConsoleAccelerator.BothSquares);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Прозрачный цвет.
        /// </summary>
        internal ConsoleColor TransparentColor { get; init; }

        /// <summary>
        /// Осуществить предварительный рендеринг области карты.
        /// </summary>
        internal void RenderMap(Scene scene, Viewport viewport)
        {
            int line = Top;

            Accelerator.ForegroundColor = TransparentColor;

            // ячейки (2 линии за проход)
            for (int halfRowCounter = 0; halfRowCounter < (viewport.Height + 1) / 2; halfRowCounter++)
            {
                Accelerator.SetCursorPosition(Left, line++);

                for (int column = viewport.StartColumnIndex; column <= viewport.StopColumnIndex; column++)
                {
                    char character;
                    // выбираем нужные ячейки
                    int row1 = viewport.StartRowIndex + halfRowCounter * 2;
                    int row2 = row1 + 1;
                    Cell? topCell = null;
                    Cell? bottomCell = null;
                    Cell? currentCell;

                    // флаги того, что отрисовываемая ячейка находится за пределами сцены
                    bool topCellOutOfBounds = false;
                    bool bottomCellOutOfBounds = false;

                    // перебираем слои с верхнего по нижний и ищем ячейки с содержимым, после чего прекращаем поиск (если не нашли - рисуем пустую ячейку самого глубогого слоя)
                    for (int layerIndex = scene.Layers.Count - 1; 0 <= layerIndex; layerIndex--)
                    {
                        Layer layer = scene.Layers[layerIndex];

                        // пропускаем невидимые слои и слои, которые не должны отображаться на карте
                        if (layer.IsVisible && layer.IsVisibleOnMap)
                        {
                            if (layer.PermissibleIndices(column, row1))
                            // если индексы допустимы
                            {
                                currentCell = layer.Columns[column][row1];
                                // ячейка слоя содержит видимую сущность и эта сущность не выстрел, ячейка карты ещё не заполнена
                                if (currentCell.HasVisible && topCell is null && currentCell.Entity is not Shot)
                                {
                                    topCell = currentCell;
                                }
                            }
                            else
                            {
                                topCellOutOfBounds = true;
                            }

                            if (layer.PermissibleIndices(column, row2))
                            // если индексы допустимы
                            {
                                currentCell = row2 < layer.Height && row2 <= viewport.StopRowIndex ? layer.Columns[column][row2] : null;
                                // ячейка слоя содержит видимую сущность и эта сущность не выстрел, ячейка карты ещё не заполнена
                                if (currentCell is not null && currentCell.HasVisible && bottomCell is null && currentCell.Entity is not Shot)
                                {
                                    bottomCell = currentCell;
                                }
                            }
                            else
                            {
                                bottomCellOutOfBounds = true;
                            }

                            if (topCell is not null && bottomCell is not null)
                            {
                                break;
                            }
                        }
                    }

                    // преобразуем две ячейки в символ (такого намудрил...)
                    if (topCell?.HasVisible == true)
                    // верхняя ячейка не пустая, нижняя - любая
                    {
                        if (bottomCell is not null && bottomCell.HasVisible)
                        // обе ячейки непустые
                        {
                            Accelerator.ForegroundColor = (topCell.Entity as ConsoleEntity).MapColor;
                            Accelerator.BackgroundColor = (bottomCell.Entity as ConsoleEntity).MapColor;
                            character = ConsoleAccelerator.TopSquare;
                        }
                        else
                        // только верхняя ячейка непустая
                        {
                            if (bottomCell is null)
                            {
                                Accelerator.BackgroundColor = bottomCellOutOfBounds ? OutOfBoundsColor : DefaultColor;
                            }

                            Accelerator.ForegroundColor = topCell.IsVisible ? (topCell.Entity as ConsoleEntity).MapColor : TransparentColor;
                            character = ConsoleAccelerator.TopSquare;
                        }
                    }
                    else if (bottomCell?.HasVisible == true)
                    // верхняя ячейка пустая, нижняя непустая
                    {
                        if (topCell is null)
                        {
                            Accelerator.BackgroundColor = topCellOutOfBounds ? OutOfBoundsColor : DefaultColor;
                        }
                        Accelerator.ForegroundColor = bottomCell.IsVisible ? (bottomCell.Entity as ConsoleEntity).MapColor : TransparentColor;
                        character = ConsoleAccelerator.BottomSquare;
                    }
                    else
                    // обе ячейки невидимы или не существуют
                    {
                        if (bottomCell is null)
                        {
                            Accelerator.BackgroundColor = bottomCellOutOfBounds ? OutOfBoundsColor : DefaultColor;
                        }
                        Accelerator.ForegroundColor = topCellOutOfBounds ? OutOfBoundsColor : DefaultColor;
                        character = ConsoleAccelerator.TopSquare;
                    }

                    if (CellDrawing is not null && CustomDraw)
                    // Нестандартная отрисовка (не используется).
                    {
                        CellDrawing.Invoke(this, new(topCell, bottomCell, Accelerator.BackgroundColor, Accelerator.ForegroundColor, new(Accelerator.CursorColumn, Accelerator.CursorColumn), character));
                    }
                    else
                    // Стандартная отрисовка.
                    {
                        Accelerator.Write(character);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        internal override void RenderScene(Scene scene, Viewport viewport)
        {
            RenderMap(scene, viewport);
            RenderEnvironment(scene, viewport);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleMapRenderer(ConsoleAccelerator accelerator) : base()
        {
            Left = 0;
            Top = 0;
            Accelerator = accelerator;
        }

        /// <summary>
        /// Левая точка области отрисовки.
        /// </summary>
        internal int Left { get; set; }

        /// <summary>
        /// Верхняя точка области отрисовки.
        /// </summary>
        internal int Top { get; set; }
    }
}
