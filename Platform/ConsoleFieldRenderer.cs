/*
Рендерер для отрисовки в консоль.
Использует ConsoleAccelerator для оптимизации вывода в консоль Winwows.
Позволяет установить свой обработчик рисования ячеек.
Можно использовать повторно.

2022-10-09
*/

using Scge;

namespace PlatformConsole
{
    /// <summary>
    /// Тип отрисовки ячейки.
    /// </summary>
    internal enum DrawKind
    {
        FirstDrawKind = 0,
        CellIndex = 0,
        Item = 1,
        ColumnIndex = 2,
        RowIndex = 3,
        Debug = 4,
        CustomDraw = 5,
        LastDrawKind = 5,
    }

    /// <summary>
    /// Аргументы обработчика отрисовки ячейки
    /// </summary>
    internal class CellDrawingEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal CellDrawingEventArgs(List<Cell> cells, Scge.Rectangle consoleRectangle)
        {
            Cells = cells;
            Rectangle = consoleRectangle;
        }

        /// <summary>
        /// Отрисовываемые ячейки.
        /// </summary>
        internal List<Cell> Cells { get; init; }

        /// <summary>
        /// Прямоугольник вывода в консоль.
        /// </summary>
        internal Scge.Rectangle Rectangle { get; init; }
    }

    /// <summary>
    /// Рендерер для отрисовки поля в консоль.
    /// </summary>
    internal class ConsoleFieldRenderer : Renderer
    {
        /// <summary>
        /// Обработчик отрисовки ячейки.
        /// </summary>
        public delegate void CellDrawingEventHandler(object sender, CellDrawingEventArgs e);

        /// <summary>
        /// Событие самостоятельной отрисовки ячейки
        /// </summary>
        internal event CellDrawingEventHandler CellDrawing;

        // Псевдографика
        internal const char RightArrow = '>';
        internal const char LeftArrow = '<';
        internal const char UpArrow = '\u2191';
        internal const char DownArrow = '\u2193';

        /// <summary>
        /// Прозрачный цвет.
        /// </summary>
        internal ConsoleColor TransparentColor { get; init; }

        /// <summary>
        /// Ускоритель вывода на консоль.
        /// </summary>
        private ConsoleAccelerator Accelerator { get; init; }

        /// <summary>
        /// Ширина ячейки.
        /// </summary>
        internal int CellWidth { get; init; }
        /// <summary>
        /// Высота ячейки.
        /// </summary>
        internal int CellHeight { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleFieldRenderer(ConsoleAccelerator accelerator, int cellWidth, int cellHeight, ConsoleColor transparentColor) : base()
        {
            TransparentColor = transparentColor;
            Accelerator = accelerator;
            CellHeight = cellHeight;
            CellWidth = cellWidth;

            EmptyText = string.Join("", Enumerable.Repeat(" ", CellWidth));
            HalfEmptyText = string.Join("", Enumerable.Repeat(" ", (CellWidth - 1) / 2));
        }

        /// <summary>
        /// Тип отрисовки.
        /// </summary>
        internal DrawKind DrawKind { get; set; }

        /// <summary>
        /// Левая точка области отрисовки.
        /// </summary>
        internal int Left { get; set; }

        /// <summary>
        /// Верхняя точка области отрисовки.
        /// </summary>
        internal int Top { get; set; }

        /// <summary>
        /// Пустой текст.
        /// </summary>
        internal string EmptyText { get; init; }

        /// <summary>
        /// Половина пустого текста.
        /// </summary>
        internal string HalfEmptyText { get; init; }

        // Отрисовка по умолчанию
        private void DrawCells(CellDrawingEventArgs args)
        {

            //рисуем блок "прозрачным" цветом
            Accelerator.SetColors(TransparentColor, TransparentColor);
            for (int line = 0; line < CellHeight; line++)
            {
                Accelerator.SetCursorPosition(args.Rectangle.Left, args.Rectangle.Top + line);
                Accelerator.Write(EmptyText);
            }

            if (0 < args.Cells.Count && DrawKind is not DrawKind.Item)
            // отладка - рисуем самую верхнюю ячейку
            {
                Cell cell = args.Cells[^1];
                ConsoleColor BackgroundColor = (cell.Entity as ConsoleEntity).MapColor;
                ConsoleColor ForegroundColor = (cell.Entity as ConsoleEntity).ForegroundColor;
                string Text = EmptyText;

                switch (DrawKind)
                {
                    // индекс ячейки
                    case DrawKind.CellIndex:
                        Text = string.Format("{0," + CellWidth + "}", cell.Index);
                        break;
                    // индекс столбца
                    case DrawKind.ColumnIndex:
                        Text = string.Format("{0," + CellWidth + "}", cell.ColumnIndex);
                        break;
                    // индекс строки
                    case DrawKind.RowIndex:
                        Text = string.Format("{0," + CellWidth + "}", cell.RowIndex);
                        break;
                    // прочая отладочная информация
                    case DrawKind.Debug:
                        // количество отрисовываемых слоев
                        Text = string.Format("{0," + CellWidth + "}", args.Cells.Count);
                        break;
                }

                Accelerator.SetColors(BackgroundColor, ForegroundColor);

                //рисуем нужный блок с текстом посередине
                for (int line = 0; line < CellHeight; line++)
                {
                    Accelerator.SetCursorPosition(args.Rectangle.Left, args.Rectangle.Top + line);

                    if (CellHeight / 2 == line || CellHeight == 1)
                    //        в центральной линии выводим текст
                    {
                        Accelerator.Write(Text);
                    }
                    else
                    {
                        Accelerator.Write(EmptyText);
                    }
                }
            }
            else
            // Рисуем стопку элементов.
            {
                
                // Это штатный алгоритм, пока уберем его
                // Перебираем ячейки послойно.
                foreach (Cell cell in args.Cells)
                {
                    ConsoleEntity entit = (cell.Entity as ConsoleEntity)!;
                    Accelerator.CopySpriteTo(entit.SpriteSelector.Selected, args.Rectangle.Left, args.Rectangle.Top);
                }
                return;
                
                
                ConsoleEntity entity = (args.Cells[^1].Entity as ConsoleEntity)!;

                //рисуем нужный блок с текстом посередине
                for (int line = 0; line < CellHeight; line++)
                {
                    Accelerator.SetCursorPosition(args.Rectangle.Left, args.Rectangle.Top + line);
                    Accelerator.SetColors(entity.MapColor, entity.ForegroundColor);
                    string text;

                    // код ниже пока не удалять - он рисует направление последней попытки перемещения и здоровье
                    char direction;

                    direction = entity.Direction switch
                    {
                        Direction.Left => LeftArrow,
                        Direction.Right => RightArrow,
                        Direction.Down => DownArrow,
                        Direction.Up => UpArrow,
                        _ => '*',
                    };
                    text = direction.ToString();

                    if (entity is IDamageable damageable)
                    {
                        text += damageable.Health.Value + "/" + damageable.Health.Maximum;
                    }

                    text = text.PadRight(CellWidth, ' ');

                    if (CellHeight / 2 == line || CellHeight == 1)
                    //  в центральной линии выводим текст
                    {
                        Accelerator.Write(text);
                    }
                    else
                    {
                        Accelerator.Write(EmptyText);
                    }
                }
            }
        }
     
        /// <summary>
        /// Осуществить предварительный рендеринг в буфер ячейки в указанной позиции.
        /// </summary>
        private void RenderTransparentCell(List<Cell> cells, int relativeColumn, int relativeRow)
        {
            Rectangle drawRectangle = new(new(Left + relativeColumn * CellWidth, Top + relativeRow * CellHeight), CellWidth, CellHeight);

            if (CellDrawing is not null && DrawKind is DrawKind.CustomDraw)
            // Пользовательская отрисовка.
            {
                CellDrawing.Invoke(this, new(cells, drawRectangle));
            }
            else
            // Cтандартная отрисовка.
            {
                DrawCells(new(cells, drawRectangle));
            }
        }
          
        /// <summary>
        /// Отрисовать.
        /// </summary>
        internal override void RenderScene(Scene scene, Viewport viewport)
        {
            /*
            * Первая версия для каждой позиции ячейки перебирала слои с самого верхнего по самый глубокий. Как только обнаруживалась видимая ячейка - поиск прекращался. В предельном случае возвращалась пустая ячейка. Далее отрисовывается ячейка или пустая ячейка.
            * Вторая версия для каждой позиции ячейки перебирает слои с самого верхнего по самый глубокий. В список помещаются все видимые ячейки (в порядке, обратном порядку проверки). Возвращается список или пустой список. Далее последовательно отрисовываются ячейки списка. "Спрайты" ячеек могут содержать прозрачные области и накладываться друг на друга.
            */

            int relativeColumn;
            int relativeRow = 0;

            // ячейки 
            for (int row = viewport.StartRowIndex; row <= viewport.StopRowIndex; row++)
            {
                relativeColumn = 0;
                for (int column = viewport.StartColumnIndex; column <= viewport.StopColumnIndex; column++)
                {
                    // перебираем слои с верхнего по нижний и ищем видимые ячейки с содержимым 
                    Cell cell;
                    List<Cell> cells = new();

                    for (int layerIndex = scene.Layers.Count - 1; 0 <= layerIndex; layerIndex--)
                    {
                        Layer layer = scene.Layers[layerIndex];
                        if (layer.IsVisible)
                        {
                            cell = layer.Cells[row * layer.Width + column];
                            if (cell.HasVisible)
                            // ячейка содержит видимое содержимое
                            {
                                // Вставляем ячейки более глубокого слоя в начало списка
                                cells.Insert(0, cell);
                            }
                            else
                            // ячейка не содержит видимого содержимого
                            {
                                continue;
                            }
                        }
                    }

                    RenderTransparentCell(cells, relativeColumn, relativeRow);
                    relativeColumn++;
                }
                relativeRow++;
            }
        }
    }
}
