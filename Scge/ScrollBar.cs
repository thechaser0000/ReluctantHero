/*
Скроллбар. 
Вспомогательный невизуальный класс для расчета параметров скроллбара в зависимости от его размера, размера прокручиваемой области и размера вьюпорта.
Не зависит от платформы и движка.

2022-10-12 
*/

namespace Scge
{
    /// <summary>
    /// Скроллбар.
    /// </summary>
    internal class ScrollBar
    {
        private int _trackerlength;
        private int _vieportLength;
        private int _contentLength;
        private int _scrollBarLength;
        private int _contentOffset;
        private int _trackerOffset;

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ScrollBar(int contentLength, int vieportLength, int scrollBarLength, int contentOffset)
        {
            _vieportLength = vieportLength;
            _contentLength = contentLength;
            _scrollBarLength = scrollBarLength;
            _contentOffset = contentOffset;

            CalculateTrackerLength();
            CalculateTrackerOffset();
        }

        /// <summary>
        /// Вычислить длину ползунка.
        /// </summary>
        private void CalculateTrackerLength()
        {
            _trackerlength = ScrollBarLength * VieportLength / ContentLength;
        }

        /// <summary>
        /// Вычислить смещение ползунка.
        /// </summary>
        private void CalculateTrackerOffset()
        {
            _trackerOffset = ScrollBarLength * ContentOffset / ContentLength;
        }

        /// <summary>
        /// Длина вьюпорта.
        /// </summary>
        internal int VieportLength
        {
            get => _vieportLength;
            set
            {
                _vieportLength = value;
                CalculateTrackerLength();
            }
        }

        /// <summary>
        /// Длина содержимого.
        /// </summary>
        internal int ContentLength
        {
            get => _contentLength;
            set
            {
                _contentLength = value;
                CalculateTrackerLength();
            }
        }

        /// <summary>
        /// Длина полосы прокрутки.
        /// </summary>
        internal int ScrollBarLength
        {
            get => _scrollBarLength;
            set
            {
                _scrollBarLength = value;
                CalculateTrackerLength();
            }
        }

        /// <summary>
        /// Смещение контента относительно вьюпорта.
        /// </summary>
        internal int ContentOffset
        {
            get => _contentOffset;
            set
            {
                _contentOffset = value;
                CalculateTrackerOffset();
            }
        }

        /// <summary>
        /// Длина ползунка.
        /// </summary>
        internal int TrackerLength => _trackerlength;

        /// <summary>
        /// Смещение ползунка.
        /// </summary>
        internal int TrackerOffset => _trackerOffset;
    }
}
