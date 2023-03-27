/*
Вьюпорт. 

Представляет собой область сцены, подлежащую отрисовке
Содержит методы абсолютной и относительной установки области отрисовки.

2022-10-06
*/

namespace Scge
{
    /// <summary>
    /// Вьюпорт сцены
    /// </summary>
    internal class Viewport
    {
        public Viewport()
        {
            Rectangle= new (); 
        }

        /// <summary>
        /// Начальный индекс колонки.
        /// </summary>
        internal int StartColumnIndex => Rectangle.Left;

        /// <summary>
        /// Завершающий индекс колонки.
        /// </summary>
        internal int StopColumnIndex => Rectangle.Right;

        /// <summary>
        /// Начальный индекс строки.
        /// </summary>
        internal int StartRowIndex => Rectangle.Top;

        /// <summary>
        /// Завершающий индекс строки.
        /// </summary>
        internal int StopRowIndex => Rectangle.Bottom;

        /// <summary>
        /// Ширина
        /// </summary>
        internal int Width => StopColumnIndex - StartColumnIndex + 1;

        /// <summary>
        ///  Высота
        /// </summary>
        internal int Height => StopRowIndex - StartRowIndex + 1;

        /// <summary>
        /// Прямоугольник вьюпорта
        /// </summary>
        internal Rectangle Rectangle { get; set; }

        /// <summary>
        /// Собрать экземпляр структуры
        /// </summary>
        internal void SetBounds(int Width, int Height)
        {
            SetBounds(0, 0, Width - 1, Height - 1);
        }

        /// <summary>
        /// Собрать экземпляр структуры
        /// </summary>
        internal void SetBounds(int startColumnIndex, int startRowIndex, int stopColumnIndex, int stopRowIndex)
        {
            Rectangle= new Rectangle(startColumnIndex, startRowIndex, stopColumnIndex, stopRowIndex);
        }

        /// <summary>
        /// Центрировать вьюпорт по указанной точке.
        /// </summary>
        internal void CenterOn(Scene scene, Point point)
        {
            CenterOn(scene, point.X, point.Y);
        }

        /// <summary>
        /// Центрировать вьюпорт по указанной точке (вьюпорт может быть больше сцены).
        /// </summary>
        private void CenterOn(Scene scene, int columnIndex, int rowIndex)
        {
            // начальный индекс
            int newStartRowIndex;
            // конечный индекс
            int newStopRowIndex;

            // начальный индекс
            int newStartColumnIndex;
            // конечный индекс
            int newStopColumnIndex;

            if (scene.Height < Height) 
            // вьюпорт выше сцены (центрируем)
            {
                // вычисляем начальный индекс
                newStartRowIndex = (scene.Height - Height) / 2;
                // вычисляем конечный индекс
                newStopRowIndex = newStartRowIndex + Height - 1;
            }
            else
            {
                // вычисляем начальный индекс, не меньший 0 
                newStartRowIndex = rowIndex - Height / 2;
                newStartRowIndex = 0 <= newStartRowIndex ? newStartRowIndex : 0;
                // вычисляем конечный индекс, не больший максимального индекса
                newStopRowIndex = newStartRowIndex + Height - 1;
                if (scene.Height - 1 < newStopRowIndex)
                {
                    newStopRowIndex = scene.Height - 1;
                    newStartRowIndex = newStopRowIndex - Height + 1;
                }
            }

            if (scene.Width < Width)
            // вьюпорт шире сцены (центрируем)
            {
                // вычисляем начальный индекс
                newStartColumnIndex = (scene.Width - Width) / 2;
                // вычисляем конечный индекс
                newStopColumnIndex = newStartColumnIndex + Width - 1;
            }
            else
            {
                // вычисляем начальный индекс, не меньший 0 
                newStartColumnIndex = columnIndex - Width / 2;
                newStartColumnIndex = 0 <= newStartColumnIndex ? newStartColumnIndex : 0;
                // вычисляем конечный индекс, не больший максимального индекса
                newStopColumnIndex = newStartColumnIndex + Width - 1;
                if (scene.Width - 1 < newStopColumnIndex)
                {
                    newStopColumnIndex = scene.Width - 1;
                    newStartColumnIndex = newStopColumnIndex - Width + 1;
                }
            }

            Rectangle = new(newStartColumnIndex, newStartRowIndex, newStopColumnIndex, newStopRowIndex);
        }
    }
}
