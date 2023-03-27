/*
Слой. Компонент сцены.  

Представляет собой прямоугольную сетку, имеющую размеры сцены. 
Сетка состоит из линейного списка ячеек. 
Доступ к ячейкам возможен, также, по строкам и столбцам.   
Слой может быть невидимым (предполагается, что он не отрисовывается)
Каждая ячейка может быть пустой или содержать 1 элемент.
 
2022-10-06
*/

namespace Scge
{
    /// <summary>
    /// Слой.
    /// </summary>
    internal class Layer
    {
        /// <summary>
        /// Сцена.
        /// </summary>
        internal Scene Scene { get; }

        /// <summary>
        /// Индексы ячейки допустимы.
        /// </summary>
        internal bool PermissibleIndices(int columnIndex, int rowIndex)
        {
            return 0 <= columnIndex && columnIndex < Width && 0 <= rowIndex && rowIndex < Height;
        }

        /// <summary>
        /// Возвращает индекс ячейки по индексам столбца и строки.
        /// </summary>
        internal int GetCellIndex(int columnIndex, int rowIndex)
        {
            return  Cell.GetCellIndex(columnIndex, rowIndex, this);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Layer(Scene scene)
        {
            IsVisible = true;
            IsVisibleOnMap = true;
            Scene = scene;

            // Создаём ячейки
            Cells = new();

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    Cell cell = new(GetCellIndex(column, row), this);
                    Cells.Add(cell);
                }
            }

            // Создаём столбцы
            Columns = new();
            for (int column = 0; column < Width; column++)
            {
                Columns.Add(new());
                for (int row = 0; row < Height; row++)
                {
                    Cell cell = Cells[GetCellIndex(column, row)];
                    Columns[column].Add(cell);
                }
            }

            // Создаём строки
            Rows = new();
            for (int row = 0; row < Height; row++)
            {
                Rows.Add(new());
                // Заполняем строки
                for (int column = 0; column < Width; column++)
                {
                    Cell cell = Cells[GetCellIndex(column, row)];
                    Rows[row].Add(cell);
                }
            }
        }

        /// <summary>
        /// Ширина слоя, ячеек.
        /// </summary>
        internal int Width => Scene.Width;

        /// <summary>
        /// Высота слоя, ячеек.
        /// </summary>
        internal int Height => Scene.Height;

        /// <summary>
        /// Прямоугольник слоя.
        /// </summary>
        internal Rectangle Rectangle => Scene.Rectangle;

        /// <summary>
        /// Видим.
        /// </summary>
        internal bool IsVisible { get; set; }

        /// <summary>
        /// Видим на карте.
        /// </summary>
        internal bool IsVisibleOnMap { get; set; }

        /// <summary>
        /// Список ячеек.
        /// </summary>
        internal List<Cell> Cells { get; }

        /// <summary>
        /// Список столбцов.
        /// </summary>
        internal List<List<Cell>> Columns { get; }

        /// <summary>
        /// Список строк.
        /// </summary>
        internal List<List<Cell>> Rows { get; }
    }
}

