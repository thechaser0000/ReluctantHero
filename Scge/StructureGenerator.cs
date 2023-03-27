/*
Генератор структур.
Статический класс, генерирующий в слое различные структуры из ячеек.
Помогает строить уровни.

2022-10-12
 */

using System.Data.Common;

namespace Scge
{

    /// <summary>
    /// Генератор структур.
    /// </summary>
    internal static class StructureGenerator
    {
        /// <summary>
        /// Возвращает случайную свободную ячейку в указанном прямоугольнике. Может вернуть null.
        /// </summary>
        internal static Cell? GetRandomEmptyCell(Layer layer, Scge.Rectangle rectangle, Predicate<Cell> cellIsAllowed = null)
        {
            int tryCount = rectangle.Capacity;

            for (int index = 0; index < tryCount; index++ )
            {
                // выбираем случайную ячейку из линейного списка (это проще, чем выбирать строку и столбец)
                Cell cell = layer.Cells[Randomizer.RandomBetwen(0, tryCount)];

                if (cell.IsEmpty)
                // нашли свободную ячейку
                {
                    // Если есть внешний обработчик, то проверяем что он нам вернул
                    if (cellIsAllowed?.Invoke(cell) == false)
                    {
                        continue;
                    }

                    return cell;
                }
            }

            return null;
        }

        /// <summary>
        /// Возвращает случайную ближайшую к указанной свободную ячейку не ближе и не далее указанных расстояний. Может вернуть null.
        /// </summary>
        internal static Cell? GetNearestEmptyCell(Layer terrayn, Cell seed, int minRadius = 0, int maxRadius = 0, Predicate<Cell> cellIsAllowed = null)
        {
            Layer layer = seed.Layer;
            bool success = false;
            int area = 1;
            int tryCount = area * area;
            int guardRadius = 0 < maxRadius ? maxRadius : Math.Max(layer.Width, layer.Height) / 2;

            // внутренние границы допустимой области
            int excludeFromColumn = Math.Max(seed.ColumnIndex - minRadius, 0);
            int excludeToColumn = Math.Min(seed.ColumnIndex + minRadius, layer.Width - 1);
            int excludeFromRow = Math.Max(seed.RowIndex - minRadius, 0);
            int excludeToRow = Math.Min(seed.RowIndex + minRadius, layer.Height - 1);

            // выбираем для героя случайное свободное место примерно в центре карты. Если место занято - постепенно расширяем диапазон возможных значений и повторяем попытку.
            while (!success)
            {
                // внешние границы допустимой области
                int fromColumn = Math.Max(seed.ColumnIndex - area, 0);
                int toColumn = Math.Min(seed.ColumnIndex + area, layer.Width - 1);
                int fromRow = Math.Max(seed.RowIndex - area, 0);
                int toRow = Math.Min(seed.RowIndex + area, layer.Height - 1);

                while (!success && 0 < tryCount)
                {
                    tryCount--;
                    int column = Randomizer.RandomBetwen(fromColumn, toColumn);
                    int row = Randomizer.RandomBetwen(fromRow, toRow);

                    // если попали за внутренние границы
                    if ((excludeFromColumn <= column && column <= excludeToColumn) && (excludeFromRow <= row && row <= excludeToRow))
                    {
                        continue;
                    }

                    // если нашли свободную ячейку (и под ней ILand)
                    if (layer.Columns[column][row].IsEmpty && terrayn.Columns[column][row].Entity is ILand)
                    {
                        // Если есть внешний обработчик, то проверяем что он нам вернул
                        if (cellIsAllowed?.Invoke(layer.Columns[column][row]) == false)
                        {
                            continue;
                        }

//                        success = true;
                        return layer.Columns[column][row];
                    }
                }

                // если исчерпано количество попыток то увеличиваем область и сбрасываем счетчик попыток
                if (!success)
                {

                    area++;
                    tryCount = area * area;

                    // достигли предела поиска
                    if (guardRadius < area)
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Возвращает список ячеек внутри указанной области (не включая границы области). Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell>? GetInnerArea(Layer layer, List<Cell> bound, Cell seed)
        {
            List<Cell> innerArea = new();

            AddToInnerArea(layer, bound, seed, ref innerArea);
            return innerArea;
        }

        /// <summary>
        /// Возвращает список ячеек внутри указанной области (не включая границы области).
        /// </summary>
        private static void AddToInnerArea(Layer layer, List<Cell> bound, Cell? seed, ref List<Cell> innerArea)
        {
            if (seed is not null && bound.Find(c => c.Equals(seed)) is null && (innerArea is null || innerArea.Find(c => c.Equals(seed)) is null))
            {
                innerArea!.Add(seed);

                AddToInnerArea(layer, bound, seed.GetNeighbor(Direction.Left), ref innerArea);
                AddToInnerArea(layer, bound, seed.GetNeighbor(Direction.Up), ref innerArea);
                AddToInnerArea(layer, bound, seed.GetNeighbor(Direction.Right), ref innerArea);
                AddToInnerArea(layer, bound, seed.GetNeighbor(Direction.Down), ref innerArea);
            }
        }

        /// <summary>
        /// Сгенерировать эллипс с указанной толщиной границы. Если толщина 0 - фигура будет залитой. Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell> GetEllipse(Layer layer, int startColumnIndex, int startRowIndex, int stopColumnIndex, int stopRowIndex, int thikness = 0)
        {
            List<Cell> selectedCells = new();
            // скорректированная толщина - не менее 1 и не более минимального полуразмера

            // всегда рисуем внешнюю границу
            selectedCells.AddRange(GetEllipse1(layer, startColumnIndex, startRowIndex, stopColumnIndex, stopRowIndex));

            // если толщина отрицательная или 1 - выходим
            if (thikness < 0 || 1 == thikness)
            {
                return selectedCells;
            }
            else
            // в остальных случаях делаем заливку
            {
                // 1 BUG затравка может попасть в область вне слоя, а это AV.
                // Нужен метод проверки на существование ячейки с такими координатами
                Cell seed = layer.Columns[Cell.GetCenterPosition(startColumnIndex, stopColumnIndex)][Cell.GetCenterPosition(startRowIndex, stopRowIndex)];
                selectedCells.AddRange(GetInnerArea(layer, selectedCells, seed));
            };

            // нужна толщина границы больше 1, но не 100%, вычитаем из залитого круга круг поменьше
            if (1 < thikness && thikness < Math.Min(Cell.GetHalfLength(startColumnIndex, stopColumnIndex), Cell.GetHalfLength(startRowIndex, stopRowIndex)))
            {
                int delta = thikness;
                List<Cell> deletedCells = new();
                deletedCells.AddRange(GetEllipse(layer, startColumnIndex + delta, startRowIndex + delta, stopColumnIndex - delta, stopRowIndex - delta, 0));

                foreach(Cell cell in deletedCells)
                {
                    selectedCells.Remove(cell);
                }
            }

            return selectedCells;
        }

        /// <summary>
        /// Сгенерировать эллипс с указанной толщиной границы. Если толщина 0 - фигура будет залитой. Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell> GetEllipse(Layer layer, Scge.Rectangle rectangle, int thikness = 0)
        {
            return GetEllipse(layer, rectangle.Start.X, rectangle.Start.Y, rectangle.Stop.X, rectangle.Stop.Y, thikness);
        }

        /// <summary>
        /// Генерирует эллипс с толщиной границы 1. Длина и ширина должны быть не менее 2.
        /// </summary>
        private static List<Cell> GetEllipse1(Layer layer, int startColumnIndex, int startRowIndex, int stopColumnIndex, int stopRowIndex)
        {
            /* 
            Реализовано по мотивам Алгоритма Брезенхема для генерации эллипса
            0. Сам класисческий алгоритм я закодить не смог (лениво углублятся в теорию), поэтому сделал свой велосипед (рабочий, но ужасный, с точки зрения производительности)
            1. Строим 1/4 часть эллипса и клонируем её
            2. Дуга состоит из двух частей: в первой итерируем по X (пока угол между нормалью к оси X больше 45*), во второй - по Y
            3. Считаем относительно нулевого расположения центра эллипса и затем переносим
            */
            List<Cell> selectedCells = new();

            // нечетные ширина или высота
            int oddWidth = 1 - (stopColumnIndex - startColumnIndex) % 2;
            int oddHeight = 1 - (stopRowIndex - startRowIndex) % 2;

            int a = (stopColumnIndex - startColumnIndex + 1) / 2 - 1 + oddWidth;
            int b = (stopRowIndex - startRowIndex + 1) / 2 - 1 + oddHeight;
            int a2 = a * a;
            int b2 = b * b;
            int x = 0;
            int y = b;
            bool xPhase;// = true;

            void AddXY(int newX, int newY)
            {
                int columnIndex = newX + a + startColumnIndex;
                int rowIndex = b - newY + startRowIndex;

                if (layer.PermissibleIndices(columnIndex, rowIndex))
                {
                    selectedCells.Add(layer.Columns[columnIndex][rowIndex]);
                }
            }

            while (0 <= y && y <= b && x <= a)
            {
                AddXY(x + 1 - oddWidth, y);
                AddXY(x + 1 - oddWidth, -y - 1 + oddHeight);
                AddXY(-x, y);
                AddXY(-x, -y - 1 + oddHeight);

                // точка, в которой мы должны перейти от итерации по X к итерации по Y, а ещё учитываем, вдоль какой оси вытянут эллипс
                xPhase = (b + 1) * (b + 1) * x <= (a + 1) * (a + 1) * y && (x < a - 1 && b <= a) || (x < a - 2 && a < b);

                if (xPhase)
                {
                    x++;
                    y = (int)Math.Round(Math.Sqrt((1.0 - 1.0 * x * x / a2) * b2));
                }
                else
                {
                    y--;
                    x = (int)Math.Round(Math.Sqrt((1.0 - 1.0 * y * y / b2) * a2));
                }
            };

            return selectedCells;
        }

        /// <summary>
        /// Сгенерировать залитую область неправильной формы в указанных границах. Заливка может быть не сплошной.
        /// </summary>
        internal static List<Cell> GetBlur(Layer layer, int startColumnIndex, int startRowIndex, int stopColumnIndex, int stopRowIndex, bool isEmptyOnly)
        {
            List<Cell> selectedCells = new();
            Cell cell;
            int pathLength;
            // Приблизительно центральная ячейка (может быть виртуалной)
            int centerCellColumnIndex = Cell.GetCenterPosition(startColumnIndex, stopColumnIndex);
            int centerCellRowIndex = Cell.GetCenterPosition(startRowIndex, stopRowIndex);
            //// Максимальное расстояние от центра до дальнйшей ячейки (правый нижний угол), но не менее 1, чтобы генерировалась хотя бы одна ячейка
            int maxPathLength = Math.Max(1, Cell.GetPathLength(centerCellColumnIndex, centerCellRowIndex, stopColumnIndex, stopRowIndex));

            for (int columnIndex = startColumnIndex; columnIndex <= stopColumnIndex; columnIndex++)
            {
                for (int rowIndex = startRowIndex; rowIndex <= stopRowIndex; rowIndex++)
                {
                    if (!layer.PermissibleIndices(columnIndex, rowIndex))
                    {
                        continue;
                    }

                    cell = layer.Columns[columnIndex][rowIndex];
                    if (cell.IsEmpty || !isEmptyOnly)
                    {
                        pathLength = cell.GetPathLengthTo(centerCellColumnIndex, centerCellRowIndex);
                        double distanceFunction = 1.0 * pathLength / maxPathLength;
                        if (Randomizer.ItIsTrue(1.0 - distanceFunction * distanceFunction))
                        {
                            selectedCells.Add(cell);
                        }
                    }
                }
            }

            return selectedCells;
        }

        /// <summary>
        /// Сгенерировать залитую область неправильной формы в указанных границах. Ячейки не проверяются на занятость. Заливка может быть не сплошной.
        /// </summary>
        internal static List<Cell> GetBlur(Layer layer, Scge.Rectangle rectangle, bool isEmptyOnly)
        {
            return GetBlur(layer, rectangle.Start.X, rectangle.Start.Y, rectangle.Stop.X, rectangle.Stop.Y, isEmptyOnly);
        }

        /// <summary>
        /// Генерирует группу клякс в указанной области и возвращает созданные элементы. Уже занятые ячейки исключаются из результата.
        /// </summary>
        internal static List<Cell> GetBlurs(Layer layer, int minWidth, int maxWidth, int minHeight, int maxHeight, int distance)
        {
            // генерируем кляксы (настраиваем диапазон размеров облаков и желаемое расстояние между ними)
            List<Cell> blurCells = new();

            int cloudCount = layer.Rectangle.Capacity / (distance * distance);
            for (int cloudIndex = 0; cloudIndex < cloudCount; cloudIndex++)
            {
                int startColumn = Randomizer.Random.Next(layer.Width);
                int startRow = Randomizer.Random.Next(layer.Height);

                // добавляем в промежуточный список
                List<Cell> newCells =
                    StructureGenerator.GetBlur(layer, startColumn, startRow,
                    startColumn + Randomizer.Random.Next(minWidth, maxWidth) - 1,
                    startRow + Randomizer.Random.Next(minHeight, maxHeight) - 1, true);

                // добавляем в основной список без повторов
                foreach (Cell newCel in newCells)
                {
                    if (-1 == blurCells.IndexOf(newCel))
                    {
                        blurCells.Add(newCel);
                    }
                }
            }

            return blurCells;
        }

        /// <summary>
        /// Сгенерировать прямоугольную область c указанной толщиной границы, если толщина 0, то область будет залитой. Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell> GetRectangle(Layer layer, int startColumnIndex, int startRowIndex, int stopColumnIndex, int stopRowIndex, int thikness = 0)
        {
            List<Cell> selectedCells = new();
            Cell cell;
            bool ignoredColumn;// = false;
            bool ignoredRow;// = false;

            for (int columnIndex = startColumnIndex; columnIndex <= stopColumnIndex; columnIndex++)
            {
                // колонка вне области заливки
                ignoredColumn = 0 < thikness && startColumnIndex + thikness - 1 < columnIndex && columnIndex < stopColumnIndex - thikness + 1;
                for (int rowIndex = startRowIndex; rowIndex <= stopRowIndex; rowIndex++)
                {
                    // строка вне области заливки
                    ignoredRow = 0 < thikness && startRowIndex + thikness - 1 < rowIndex && rowIndex < stopRowIndex - thikness + 1;

                    // пропускаем шаг цикла, если попали в область, свободную от заливки
                    if (ignoredColumn && ignoredRow)
                    {
                        continue;
                    }

                    // пропускаем шаг цикла, если попали за пределы слоя
                    if (!layer.PermissibleIndices(columnIndex, rowIndex))
                    {
                        continue;
                    }

                    cell = layer.Columns[columnIndex][rowIndex];
                    selectedCells.Add(cell);
                }
            }

            return selectedCells;
        }

        /// <summary>
        /// Сгенерировать прямоугольную область c указанной толщиной границы, если толщина 0, то область будет залитой. Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell> GetRectangle(Layer layer, Scge.Rectangle rectangle, int thikness = 0)
        {
            return GetRectangle(layer, rectangle.Start.X, rectangle.Start.Y, rectangle.Stop.X, rectangle.Stop.Y, thikness);
        }

        /// <summary>
        /// Сгенерировать несколько прямоугольных областей на основе указанных прямоугольников c указанной толщиной границы, если толщина 0, то область будет залитой. Ячейки не проверяются на занятость.
        /// </summary>
        internal static List<Cell> GetRectangles(Layer layer, List<Scge.Rectangle> rectangles, int thikness = 0)
        {
            List<Cell> list = new();
            foreach (Scge.Rectangle rectangle in rectangles)
            {
                list.AddRange(GetRectangle(layer, rectangle, thikness));
            }
            
            return list;
        }

    }
}
