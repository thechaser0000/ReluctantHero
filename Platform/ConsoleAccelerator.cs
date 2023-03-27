/*
Ускоритель вывода цветного текста в консоль в фиксированную область.
Запоминает состояние области вывода и выводит в консоль только измененные участки построчно, минимизируя операции перемещения курсора, смены цвета фона и текста, вывода символов.
Использование:
* Записать текущий кадр в ConsoleAccelerator.Snapshot с помощью свойств и методов, аналогичных таковым из пространства имен Console: Write(), BacgroundColor, ForegroundColor, Column, Left
* Вызвть метод ConsoleAccelerator.Render()
* Компонент построит разностный снимок с предыдущим состоянием кадра и выведет в консоль только изменения. Если изменений нет, то никакого вывода в консоль не будет. Затраты на вычисление разностного снимка малы по сравнению с затратами на вывод в консоль.

Дополнительно:
* Вероятно, строить весь интерфейс с использованием единого ускорителя удобнее, чем по-разному отрисовывать отдельные области интерфейса, даже если они обновляются с разной частотой.
* В компонент встроена сборка статистики: время, вызовы, количество переключения контекста консоли, выведенные строки и символы. Доступны последние, средние и суммарные измерения.
* При малой динамике изменения картинки отрисовка изменений в 30 раз быстрее полной отрисовки кадра (возможно, это всё равно быстрее, чем работать напрямую с консолью), при высокой динамике - в 5 раз быстрее. Конечно, всё это сильно зависит от конкретного наполнения. 

2022-10-31
*/

using Scge;
using System.Text;

namespace PlatformConsole
{

    /// <summary>
    /// Ускоритель вывода цветного текста в консоль.
    /// </summary>
    internal class ConsoleAccelerator
    {
        // верхний, нижний и двойной квадраты
        internal const char TopSquare = '\u2580';
        internal const char BottomSquare = '\u2584';
        internal const char BothSquares = '\u2588';
        internal const char Space = ' ';

        /// <summary>
        /// Цвет по умолчанию.
        /// </summary>
        internal ConsoleColor DefaultColor = ConsoleColor.Black;

        /// <summary>
        /// Очистить буфер.
        /// </summary>
        internal void Clear()
        {
            Snapshot.Reset();
        }

        /// <summary>
        /// Символ по умолчанию.
        /// </summary>
        internal const char DefaultChar = ' ';

        #region IConsoleWriter

        /// <summary>
        /// Пустой символ
        /// </summary>
        internal Char EmptyChar = ' ';

        /// <summary>
        /// Текущая активная колонка снимка.
        /// </summary>
        internal int CursorColumn { get; set; }

        /// <summary>
        /// Текущая активная линия снимка.
        /// </summary>
        internal int CursorLine { get; set; }

        /// <summary>
        /// Текущий цвет фона снимка.
        /// </summary>
        internal ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Текущий цвет фона текста.
        /// </summary>
        internal ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Копировать спрайт в указанную позицию.
        /// </summary>
        internal void CopySpriteTo(ConsoleSprite sprite, int left, int top)
        {
            for (int relativeColumn = 0; relativeColumn < sprite.Width; relativeColumn++)
            {
                for (int relativeLine = 0; relativeLine < sprite.Height; relativeLine++)
                {
                    int column = left + relativeColumn;
                    int line = top + relativeLine;

                    // Пропускаем строки и столбцы за пределом области вывода.
                    if (!Rectangle.Horizontal.IsInclude(column) || !Rectangle.Vertical.IsInclude(line))
                    {
                        continue;
                    }

                    ColoredChar character = sprite.Image[relativeColumn, relativeLine];

                    if (character.IsFull)
                    // перезаписываем непустые символы
                    {
                        Snapshot.Characters[column, line] = character;
                    }
                    else if (!character.IsEmpty)
                    // полупустые символы копируем хитрее
                    {
                        ColoredChar oldCharacter = Snapshot.Characters[column, line];
                        ConsoleColor? topColor = character.TopColor ?? oldCharacter.TopColor;
                        ConsoleColor? bottomColor = character.BottomColor ?? oldCharacter.BottomColor;
                        Snapshot.Characters[column, line] = new(topColor, bottomColor);
                    }
                }
            }
        }

        /// <summary>
        /// Записывает указанный текст в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void Write(string text)
        {
            int column = CursorColumn;
            foreach (char @char in text)
            {
                /// Незатейливая проверка выхода за пределы снимка
                if (column == Width)
                {
                    break;
                    throw new IndexOutOfRangeException();
                }

                Snapshot.Characters[column, CursorLine] = new(BackgroundColor, ForegroundColor, @char, false);
                column++;
            }

            CursorColumn = column;
        }

        /// <summary>
        /// Записывает указанный текст в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора и переводится строка.
        /// </summary>
        internal void WriteLn(string text)
        {
            int oldColumn = CursorColumn;
            Write(text);
            CursorLine++;
            CursorColumn = oldColumn;
        }

        /// <summary>
        /// Записывает указанный текст, дополненный пробелами слева до нужной длины, в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void WritePadLeft(string text, int length)
        {
            Write(text.PadLeft(length, EmptyChar));
        }

        /// <summary>
        /// Записывает указанный текст, дополненный пробелами слева до нужной длины, в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void WritePadRight(string text, int length)
        {
            Write(text.PadRight(length, EmptyChar));
        }

        /// <summary>
        /// Записывает указанный текст, дополненный пробелами слева до нужной длины, в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void WriteEmpty(int length)
        {
            Write("".PadRight(length, EmptyChar));
        }

        /// <summary>
        /// Записывает указанный текст, дополненный пробелами с обеих сторон до нужной длины, в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void WritePadBoth(string text, int length)
        {
            int difference = length - text.Length;
            Write(text.PadLeft(length - difference / 2, EmptyChar).PadRight(length, EmptyChar));
        }

        /// <summary>
        /// Записывает указанный символ в текущую позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void Write(char @char)
        {
            Snapshot.Characters[CursorColumn, CursorLine] = new(BackgroundColor, ForegroundColor, @char, false);
            CursorColumn++;
        }

        /// <summary>
        /// Записывает указанный текст в указанную позицию курсора с текущими цветами фона и текста. При этом меняется текущая позиция курсора.
        /// </summary>
        internal void Write(int left, int top, string text)
        {
            SetCursorPosition(left, top);
            Write(text);
        }

        /// <summary>
        /// Установить цвета внутри области снимка.
        /// </summary>
        internal void SetColors(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Установить позицию курсора внутри области снимка.
        /// </summary>
        internal void SetCursorPosition(int left, int top)
        {
            CursorColumn = left;
            CursorLine = top;
        }
        #endregion

        /// <summary>
        /// Последний отображенный снимок.
        /// </summary>
        private ConsoleSnapshot LastSnapshot { get; init; }
        
        /// <summary>
        /// Новый снимок.
        /// </summary>
        private ConsoleSnapshot Snapshot { get; init; }

        /// <summary>
        /// Статистика.
        /// </summary>
        internal ConsoleAcceleratorStatistics Statistics { get; init; }

        /// <summary>
        /// Разностный снимок.
        /// </summary>
        private ConsoleSnapshot DifferenceSnapshot { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleAccelerator(int left, int top, int width, int height, ConsoleColor defaultColor)
        {
            Rectangle = new(new(left, top), width, height);
            LastSnapshot = new(this);
            Snapshot = new(this);
            DifferenceSnapshot = new(this);
            Statistics = new();
            DefaultColor = defaultColor;
        }

        /// <summary>
        /// Прямоугольник отрисовки.
        /// </summary>
        internal Rectangle Rectangle { get; private set; }

        /// Ширина области вывода (столбцов)
        /// </summary>
        internal int Width => Rectangle.Width;

        /// <summary>
        /// Высота области вывода (линий)
        /// </summary>
        internal int Height => Rectangle.Height;

        /// <summary>
        /// Отрисовать в консоли указанный снимок.
        /// </summary>
        private void RenderSnapshot(ConsoleSnapshot snapshot)
        {
            ColoredChar character;

            // простой и точный посимвольный способ отрисовки, жаль, что исключительно медленный
            /*for (int line = 0; line < Height; line++)
            {
                for (int column = 0; column < Width; column++)
                {
                    character = DifferenceSnapshot.Characters[column, line];

                    if (!character.IsEmpty)
                    {
                        Console.BackgroundColor = character.BackgroundColor;
                        Console.ForegroundColor = character.ForegroundColor;
                        Console.SetCursorPosition(column, line);
                        Console.Write(character);
                    }
                }
            }

            return;
            */

            ConsoleColor oldBackgroundColor = Console.BackgroundColor;
            ConsoleColor oldForegroundColor = Console.ForegroundColor;
            int oldColumn = Console.CursorLeft;
            int oldLine = Console.CursorTop;

            StringBuilder currentString = new();
            ConsoleColor currentBackgroundColor = oldBackgroundColor;
            ConsoleColor currentForegroundColor = oldForegroundColor;
            int currentColumn = oldColumn;
            int currentLine = oldLine;

            /// Выводим на консоль текущую строку с минимум переключений контекста.
            void WriteCurrentString()
            {
                // ничего не делаем, если строка пуста
                if (0 == currentString.Length)
                {
                    return;
                }

                if (currentBackgroundColor != oldBackgroundColor)
                {
                    Console.BackgroundColor = currentBackgroundColor;
                    oldBackgroundColor = currentBackgroundColor;

                    Statistics.AcceptSwitchContext();
                }

                if (currentForegroundColor != oldForegroundColor)
                {
                    Console.ForegroundColor = currentForegroundColor;
                    oldForegroundColor = currentForegroundColor;

                    Statistics.AcceptSwitchContext();
                }

                // переключаем курсор не более одного раза
                if (currentColumn != oldColumn || currentLine != oldLine)
                {
                    Console.SetCursorPosition(currentColumn + Rectangle.Left, currentLine + Rectangle.Top);
                    oldColumn = currentColumn;
                    oldLine = currentLine;

                    Statistics.AcceptSwitchContext();
                }

                Console.Write(currentString);

                Statistics.AcceptWriteString(currentString.Length);

                // вычисляем положение курсора (вдруг это быстрее чем запрашивать Console.CursorLeft )
                oldColumn = currentColumn + currentString.Length;
                currentString.Clear();
            }

            // перебираем строки
            for (int line = 0; line < Height; line++)
            {
                currentLine = line;

                // перебираем колонки
                for (int column = 0; column < Width; column++)
                {
                    character = snapshot.Characters[column, line];

                    // если встретили пустой символ - печатаем сформированную строку (если она у вас конечно есть)
                    if (character.IsEmpty)
                    {
                        WriteCurrentString();
                    }
                    // Нашли символ в строке
                    else
                    {
                        // Этот символ относиться к старой строке (не поменялись цвета и старая строка есть) - добавляем его
                        if (0 < currentString.Length && currentBackgroundColor == character.BackgroundColor && currentForegroundColor == character.ForegroundColor)
                        {
                            currentString.Append(character);
                        }
                        // Этот символ относиться к новой строке - печатаем старую строку и начинаем новую
                        {
                            WriteCurrentString();
                            currentColumn = column + Rectangle.Left;
                            currentBackgroundColor = character.BackgroundColor ?? DefaultColor;
                            currentForegroundColor = character.ForegroundColor ?? DefaultColor;
                            currentString.Append(character);
                        }

                        // Этот символ последний в строке - печатаем
                        if (column == Width - 1)
                        {
                            WriteCurrentString();
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Отрисовать разностный снимок.
        /// </summary>
        internal void Render(bool differences = true)
        {
            Statistics.StartRender();

            if (differences)
            // отрисовать различия
            {
                BuildStrictDifferenceSnapshot();
                RenderSnapshot(DifferenceSnapshot);
            }
            else
            // отрисовать всё
            {
                RenderSnapshot(Snapshot);
            }

            Statistics.StopRender();
        }

        /// <summary>
        /// Построить строгий разностный снимок.
        /// </summary>
        private void BuildStrictDifferenceSnapshot()
        {
            // перебираем колонки
            for (int column = 0; column < Width; column++)
            {
                // перебираем строки
                for (int line = 0; line < Height; line++)
                {
                    if (!LastSnapshot.Characters[column, line].Equals(Snapshot.Characters[column, line]))
                    // если символы разные - помещаем новый символ в снимок
                    {
                        DifferenceSnapshot.Characters[column, line] = Snapshot.Characters[column, line];
                    }
                    else
                    // если символы одинаковые - помещаем пустой символ в снимок
                    {
                        DifferenceSnapshot.Characters[column, line] = new(); 
                    }

                    LastSnapshot.Characters[column, line] = Snapshot.Characters[column, line];
                }
            }
        }
    }
}
