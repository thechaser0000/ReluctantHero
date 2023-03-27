/*
Ячейка. Компонент слоя.  

Содержит свой индекс в списке ячеек слоя, индекс строки и колонки.
Содержить ссылку на слой, в котором находится.
Может быть невидимой и тогда не отрисовывается (?)
Может содержать единственный элемент.

Содержит метод поиска соседей.
 
2022-10-06
*/


namespace Scge
{
    /// <summary>
    /// Направление в пределах слоя (4 основных направления + без направления).
    /// </summary>
    internal enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
    }

    /// <summary>
    /// Направление в пределах слоя (8 направлений + без направления).
    /// </summary>
    internal enum Direction8
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        LeftUp = 5,
        LeftDown = 6,
        RightUp = 7,
        RightDown = 8
    }

    /// <summary>
    /// Ячейка слоя.
    /// </summary> 
    internal class Cell
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Cell(int index, Layer layer)
        {
            IsVisible = true;
            Index = index;
            Layer = layer;
        }

        /// <summary>
        /// Возвращает индекс середины отрезка.
        /// </summary>
        internal static int GetCenterPosition(int startPosition, int stopPosition)
        {
            return startPosition + (stopPosition - startPosition) / 2;
        }

        /// <summary>
        /// Возвращает длину пути между указанными ячейками.
        /// </summary>
        internal static int GetPathLength(Cell from, Cell to)
        {
            return GetPathLength(from.ColumnIndex, from.RowIndex, to.ColumnIndex, to.RowIndex);
        }

        /// <summary>
        /// Возвращает направления по двум осям от одной ячейки к другой .
        /// </summary>
        internal static (Direction, Direction) GetDirection(Cell source, Cell target)
        {
            Direction horizontalDirection = source.ColumnIndex < target.ColumnIndex ? Direction.Right : target.ColumnIndex < source.ColumnIndex ? Direction.Left : Direction.None;
            Direction verticalDirection = source.RowIndex < target.RowIndex ? Direction.Down : target.RowIndex < source.RowIndex ? Direction.Up : Direction.None;

            return (horizontalDirection, verticalDirection);
        }

        /// <summary>
        /// Возвращает индекс ячейки по индексам столбца и строки.
        /// </summary>
        internal static int GetCellIndex(int columnIndex, int rowIndex, Layer layer)
        {
            return rowIndex * layer.Width + columnIndex;
        }

        /// <summary>
        /// Возвращает длину пути между указанными ячейками без учета препятствий.
        /// </summary>
        internal static int GetPathLength(int fromColumnIndex, int fromRowIndex, int toColumnIndex, int toRowIndex)
        {
            return Math.Abs(toColumnIndex - fromColumnIndex) + Math.Abs(toRowIndex - fromRowIndex);
        }

        /// <summary>
        /// Возвращает измерение между двумя ячейками по двум направлениям.
        /// ( результат положителен, если target находится дальше по координатной оси и отрицателен в противном случае)
        /// </summary>
        internal static (int, int) GetDistance(Cell source, Cell target)
        {
            return (target.ColumnIndex - source.ColumnIndex, target.RowIndex - source.RowIndex);
        }

        /// <summary>
        /// Возвращает направление, в котором нужно повернуться, чтобы увидеть цель.
        /// Если цель не на линии, то возвращает None.
        /// </summary>
        internal static Direction GetViewDirection(Cell source, Cell target)
        {
            (Direction horizontal, Direction vertical) = GetDirection(source, target);

            // возвращаем единственное ненулевое направление или None
            return (horizontal == Direction.None) ? vertical : (vertical == Direction.None) ? horizontal : Direction.None;
        }

        /// <summary>
        /// Возвращает измерение между двумя ячейками на одной линии.
        /// </summary>
        internal static int GetLength(int fromIndex, int toIndex)
        {
            return toIndex - fromIndex + 1;
        }

        /// <summary>
        /// Возвращает половину измерения между двумя ячейками на одной линии.
        /// </summary>
        internal static int GetHalfLength(int fromIndex, int toIndex)
        {
            return GetLength(fromIndex, toIndex) / 2;
        }

        /// <summary>
        /// Возвращает длину пути между текущей ячейкой и указанной ячейкой.
        /// </summary>
        internal int GetPathLengthTo(Cell cell)
        {
            return GetPathLength(this, cell);
        }

        /// <summary>
        /// Возвращает длину пути между текущей ячейкой и указанной ячейкой.
        /// </summary>
        internal int GetPathLengthTo(int columnIndex, int rowIndex)
        {
            return GetPathLength(ColumnIndex, RowIndex, columnIndex, rowIndex);
        }

        /// <summary>
        /// Индекс ячейки в слое.
        /// </summary>
        internal int Index { get; }

        /// <summary>
        /// Индекс столбца.
        /// </summary>
        internal int ColumnIndex => Index % Layer.Width;

        /// <summary>
        /// Индекс строки.
        /// </summary>
        internal int RowIndex => Index / Layer.Width;

        /// <summary>
        /// Координаты ячейки в виде точки.
        /// </summary>
        internal Scge.Point Point => new(ColumnIndex, RowIndex);

        /// <summary>
        /// Слой ячейки.
        /// </summary>
        internal Layer Layer { get; }

        /// <summary>
        /// Видима.
        /// </summary>
        internal bool IsVisible { get; set; }

        /// <summary>
        /// Пуста (не содержит элемента).
        /// </summary>
        internal bool IsEmpty => Entity == null;

        /// <summary>
        /// Имеет видимый элемент.
        /// </summary>
        internal bool HasVisible => IsVisible && Entity?.IsVisible == true;

        /// <summary>
        /// Сущность.
        /// </summary>
        internal Entity? Entity { get; set; }

        /// <summary>
        /// Возвращает отражение ячейки в указанном направлении (ячейку, находящуюся на том же расстоянии от противоположного края).
        /// </summary>
        internal Cell? GetReflection(Direction direction)
        {
            int reflectionColumnIndex = ColumnIndex;
            int reflectionRowIndex = RowIndex;

            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    reflectionColumnIndex = Layer.Width - ColumnIndex - 1;
                    break;
                case Direction.Up:
                case Direction.Down:
                    reflectionRowIndex = Layer.Height - RowIndex - 1;
                    break;
                default:
                    return null;
            }

            return Layer.Columns[reflectionColumnIndex][reflectionRowIndex];
        }

        /// <summary>
        /// Возвращает соседа в указанном направлении или null
        /// </summary>
        internal Cell? GetNeighbor(Direction direction)
        {
            int neighborColumnIndex = ColumnIndex;
            int neighborRowIndex = RowIndex;

            switch (direction)
            {
                case Direction.Left:
                    if (0 < ColumnIndex)
                    {
                        neighborColumnIndex--;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case Direction.Right:
                    if (ColumnIndex < Layer.Width - 1)
                    {
                        neighborColumnIndex++;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case Direction.Up:
                    if (0 < RowIndex)
                    {
                        neighborRowIndex--;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case Direction.Down:
                    if (RowIndex < Layer.Height - 1)
                    {
                        neighborRowIndex++;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                default:
                    return null;
            }

            // возвращаем ячейку на строку/столбец выше/ниже
            return Layer.Columns[neighborColumnIndex][neighborRowIndex];
        }

        /// <summary>
        /// Возвращает всех соседей (0..4).
        /// </summary>
        internal List<Cell> GetNeighbors(bool IsEmptyOnly = false)
        {
            List<Cell> result = new();
            
            void AddIfNotIsNull(Cell? cell)
            {
                if (cell != null && (!IsEmptyOnly || (IsEmptyOnly && cell.IsEmpty)))
                {
                    result.Add(cell);
                }
            }

            AddIfNotIsNull(GetNeighbor(Direction.Up));
            AddIfNotIsNull(GetNeighbor(Direction.Left));
            AddIfNotIsNull(GetNeighbor(Direction.Down));
            AddIfNotIsNull(GetNeighbor(Direction.Right));

            return result;
        }
    }
}
