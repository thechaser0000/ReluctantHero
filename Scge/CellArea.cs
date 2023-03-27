/*
Область из ячеек.
Содержит методы добавления, удаления и расчётов различных метрик.
Расположение ячеек произвольное.

2022-11-14
 */

namespace Scge
{
    /// <summary>
    /// Область из ячеек.
    /// </summary>
    internal class CellArea
    {
        /// <summary>
        /// Сущность.
        /// </summary>
        internal Entity? Entity { get; private set; }

        /// <summary>
        /// Список ячеек.
        /// </summary>
        internal List<Cell> Items { get; init; }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public CellArea(Entity? entity = null, List<Cell>? cells = null)
        {
            Entity = entity;
            Items = new List<Cell>();
            Add(cells);
        }

        /// <summary>
        /// Добавить указанный список ячеек.
        /// </summary>
        internal void Add(List<Cell>? cells)
        {
            if (cells is not null)
            {
                Entity ??= cells?[0].Entity;
                Items.AddRange(cells);

                foreach (Cell cell in cells)
                {
                    cell.Entity = Entity;
                }

                CalculateCenter();
            }
        }

        /// <summary>
        /// Добавить указанную ячейку.
        /// </summary>
        internal void Add(Cell cell)
        {
            Add(new List<Cell>() { cell });
        }

        /// <summary>
        /// Удалить указанную ячейку.
        /// </summary>
        internal void Remove(Cell cell)
        {
            Remove(new List<Cell>() { cell });
        }

        /// <summary>
        /// Заменить старую ячейку на новую.
        /// </summary>
        internal void Replace(Cell oldCell, Cell newCell)
        {
            int index = Items.IndexOf(oldCell);
            Items[index] = newCell;

            CalculateCenter();
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        internal void Clear()
        {
            foreach(Cell cell in Items)
            {
                cell.Entity = null;
            }

            Items.Clear();
            Entity = null;
            CalculateCenter();
        }

        /// <summary>
        /// Количество ячеек.
        /// </summary>
        internal int Count => Items.Count;

        /// <summary>
        /// Удалить указанный список ячеек.
        /// </summary>
        internal void Remove(List<Cell>? cells)
        {
            if (cells is not null)
            {
                foreach (Cell cell in cells)
                {
                    cell.Entity = null;
                    Items.Remove(cell);
                }

                CalculateCenter();
            }
        }

        /// <summary>
        /// Вычислить центральный элемент персонажа.
        /// </summary>
        private void CalculateCenter()
        {
            if (0 == Items.Count)
            // нет элементов
            {
                Center = null;
            }
            else if (1 == Items.Count)
            // единственный элемент
            {
                Center = Items[0];
            }
            else
            // несколько элементов - нужно считать средний.
            {
                int totalColumns = 0;
                int totalRows = 0;
                int count = Items.Count;
                Layer layer = Items[0].Layer;

                // вычисляем среднюю ячейку
                foreach (Cell cell in Items)
                {
                    totalColumns += cell.ColumnIndex;
                    totalRows += cell.RowIndex;
                }

                Cell centerCell = layer.Columns[totalColumns / count][totalRows / count];
                List<int> lengths = new();

                // для каждого элемента вычисляем расстояние до средней ячейки
                foreach (Cell cell in Items)
                {
                    lengths.Add(centerCell.GetPathLengthTo(cell));
                }

                int minPathLength = int.MaxValue;
                int minPathLengthIndex = -1;

                // выбираем элемент с наименьшим расстоянием до центральной
                for (int index = 0; index < count; index++)
                {
                    if (lengths[index] < minPathLength)
                    {
                        minPathLength = lengths[index];
                        minPathLengthIndex = index;
                    }
                }

                Center = Items[minPathLengthIndex];
            }
        }

        /// <summary>
        /// Центральная ячейка области.
        /// </summary>
        internal Cell? Center { get; private set; }
    }
}
