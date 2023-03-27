/*
Ненаправленный отрезок на прямой. 
Вспомогательный невизуальный класс для манипуляций с дискретными отрезками на прямой.
Можно сконструировать экземпляр, задав любую комбинацию из двух выбранных параметров: начальной точки, конечной точки и длины. По умолчанию начальная и конечная точки равны нулю.
Если попытаться задать начальную точку больше конечной или наоборот, то при присваивании они поменяются местами. Таким образом при чтении экземпляра класса начало отрезка всегда будет левее конца.
Не зависит от платформы и движка.

2022-10-25
*/

namespace Scge
{
    /// <summary>
    /// Отрезок.
    /// </summary>
    internal struct Segment
    {
        private int? _start = null;
        private int? _stop = null;

        /// <summary>
        /// Значение входит в отрезок.
        /// </summary>
        internal bool IsInclude(int value) => Start <= value && value <= Stop;

        /// <summary>
        /// Признак неинициализированности начальной точки.
        /// </summary>
        internal bool StartIsNull => _start is null;

        /// <summary>
        /// Признак неинициализированности конечной точки.
        /// </summary>
        internal bool StopIsNull => _stop is null;

        /// <summary>
        /// Начальная точка.
        /// </summary>
        internal int Start
        {
            get => _start ?? 0;
            init
            {
                if (value != _start)
                {
                    // если новая начальная точка корректна
                    if (_stop is null || value <= _stop)
                    { 
                        _start = value;
                    }
                    else
                    // новая начальная точка правее конечной точки
                    {
                        (_start, _stop) = (_stop, value);
                    }
                }
            }
        }

        /// <summary>
        /// Конечная точка.
        /// </summary>
        internal int Stop
        {
            get => _stop ?? 0;
            init
            {
                if (value != _stop)
                {
                    // если новая конечная точка корректна
                    if (_start is null || _start <= value)
                    {
                        _stop = value;
                    }
                    else
                    // новая конечная точка левее начальной точки
                    {
                        (_start, _stop) = (value, _start);
                    }
                }
            }
        }

        /// <summary>
        /// Полудлина отрезка. 
        /// </summary>
        internal int HalfLength => Length / 2;

        /// <summary>
        /// Середина отрезка.
        /// </summary>
        internal int Middle => Start + HalfLength; 

        /// <summary>
        /// Длина отрезка.
        /// </summary>
        internal int Length
        {
            get => Math.Abs(Stop - Start) + 1;
            init
            {
                // если задана только конечная точка, вычисляем точку старта относительно неё
                if (_stop is not null && _start is null)
                {
                    _start = _stop - value + 1;
                }
                else
                // в остальных случаях вычисляем конечную точку относительно старта (если нет и старта, то считаем его равным 0)
                {
                    _start = Start;
                    _stop = Start + value - 1;
                }
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Segment(int start, int stop)
        {
            Start = start;
            Stop = stop;
        }
    }

}
