/*
Сцена. Компонент движка.  

Представляет собой набор слоёв и элементов.
Имеет ширину и высоту, а также, ёмкость (количество ячеек).
Может быть невидимой и тогда не отрисовывается.

REF 2 нужно выделить сцену без ячеек, либо набр ячеек поместить в отдельный объект (Layers -> CellLayers), куда переместить и размеры, и Entities 
 
2022-10-06
*/


using PlatformConsole;

namespace Scge
{
    /// <summary>
    /// Состояние сцены.
    /// </summary>
    internal enum SceneState
    {
        /// <summary>
        /// Подготавливается (создание, генерация, загрузка).
        /// </summary>
        Preparing = 0,
        /// <summary>
        /// Готова и ожидает начала работы.
        /// </summary>
        Ready = 1,
        /// <summary>
        /// Выполняется.
        /// </summary>
        Running = 2,
        ///// <summary>
        ///// Неудача.
        ///// </summary>
        //Fail = 3,
        ///// <summary>
        ///// Успех.
        ///// </summary>
        //Success = 4,
        /// <summary>
        /// Выход.
        /// </summary>
        Exit = 3,
    }

    /// <summary>
    /// Сцена.
    /// </summary>
    internal abstract class Scene
    {
        /// <summary>
        /// Имя сцены.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Состояние сцены.
        /// </summary>
        internal SceneState State { get; private protected set; }

        /// <summary>
        /// Является ли тик N-м по порядку (каждым вторым, каждым 25-м и т.п.).
        /// </summary>
        internal bool IsNumberedTick(int number) => 0 == TickCount % number;

        ///// <summary>
        ///// Зерно для генерации сцены.
        ///// </summary>
        //internal object? Seed { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        //internal Scene(object? seed)
        internal Scene()
        {
            State= SceneState.Preparing;
            // Сущности.
            Entities = new();
            // Слои ячеек.
            Layers = new();
            RenderersViewports = new();
            // Текстовые фреймы.
            TextFrames = new();
            // Текстовые фреймы переднего плана.
            OverlayTextFrames = new();
            // Прогрессбары.
            ProgressBars = new();
            // Картинки.
            Pictures = new();
            // 
            IsVisible = true;
            // Прямоугольник сцены.
            Rectangle = new();
        }

        /// <summary>
        /// Задать размер сцены.
        /// </summary>
        protected void SetSize(int width, int height)
        {
            SetSize(new(width, height));
        }

        /// <summary>
        /// Задать размер сцены.
        /// </summary>
        protected void SetSize(Rectangle rectangle)
        {
            if (0 == Layers.Count)
            {
                Rectangle = rectangle;
            }
            else
            {
                throw new Exception("Layers must be empty");
            }
        }

        /// <summary>
        /// Действия, которые нужно выполнить перед тактом.
        /// </summary>
        internal void BeforeTick()
        {
        }

        /// <summary>
        /// Выполнить тик движка (без отрисовки).
        /// </summary>
        internal virtual void DoTick()
        {
            // "сборка мусора" с конца списка (для безопасного удаления)
            for (int index = Entities.Count - 1; 0 <= index; index--)
            {
                Entity entity = Entities[index];

                // удаляем устаревшие элементы
                if (entity.NeedDelete)
                {
                    entity.Clear();
                    Entities.RemoveAt(index);
                    continue;
                }
            }

            // обработка элементов с начала списка (новые элементы с большей вероятностью попадут в конец списка и обработаются позже)
            for (int index = Entities.Count - 1; 0 <= index; index--)
            {
                Entity entity = Entities[index];

                // Обрабатываем перемещение сущностей.
                if (!entity.NeedDelete)
                {
                    entity.DoMove();
                }
            }

            // обработка элементов с начала списка (новые элементы с большей вероятностью попадут в конец списка и обработаются позже)
            for (int index = Entities.Count - 1; 0 <= index; index--)
            {
                Entity entity = Entities[index];

                if (!entity.NeedDelete)
                {
                    // проверяем повреждения
                    if (entity is IDamageable damaged)
                    {
                        damaged.CheckDamage();
                    }

                    // Обрабатываем проверки сушностей.
                    entity.DoCheck();
                }
            }

            TickCount++;
        }

        /// <summary>
        /// Счетчик тиков.
        /// </summary>
        internal int TickCount { get; private set; }

        /// <summary>
        /// Выполнить отрисовку.
        /// </summary>
        internal virtual void DoRendering()
        {
            // вызываем все рендереры/вьюпорты сцены (на самом деле - пререндереры, они лишь формируют кадр в Game.Accelerator )
            foreach ((Renderer renderer, Viewport viewpor) item in RenderersViewports)
            {
                if (item.renderer.IsEnabled)
                {
                    item.renderer.RenderScene(this, item.viewpor);
                }
            }
        }

        /// <summary>
        /// Текстовые фреймы.
        /// (не используют вьюпорт)
        /// </summary>
        internal List<TextFrame> TextFrames { get; init; }

        /// <summary>
        /// Текстовые фреймы переднего плана.
        /// (не используют вьюпорт)
        /// </summary>
        internal List<TextFrame> OverlayTextFrames { get; init; }

        /// <summary>
        /// Прогрессбары.
        /// (не используют вьюпорт)
        /// </summary>
        internal List<ProgressBar> ProgressBars { get; init; }

        /// <summary>
        /// Картинки.
        /// (не используют вьюпорт)
        /// </summary>
        internal List<Picture> Pictures { get; init; }

        /// <summary>
        /// Рендереры/вьюпорты.
        /// </summary>
        internal List<(Renderer, Viewport?)> RenderersViewports { get; }

        /// <summary>
        /// Ширина сцены.
        /// (В ячейках).
        /// </summary>
        internal int Width => Rectangle.Width;

        /// <summary>
        /// Высота сцены.
        /// (В ячейках).
        /// </summary>
        internal int Height => Rectangle.Height;

        /// <summary>
        /// Прямоугольник сцены.
        /// (В ячейках).
        /// </summary>
        internal Scge.Rectangle Rectangle { get; private set; }

        /// <summary>
        /// Возвращает количество ячеек во всех сущностях.
        /// </summary>
        internal int GetEntityCellCount()
        {
            int count = 0;
            foreach(Entity entity in Entities)
            {
                count += entity.Cells.Count;
            }

            return count;
        }

        /// <summary>
        /// Возвращает количество сущностей во всех ячейках.
        /// </summary>
        internal int GetCellEntityCount()
        {
            int count = 0;
            foreach (Layer layer in Layers)
            {
                foreach (Cell cell in layer.Cells)
                {
                    count += cell.IsEmpty ? 0 : 1;
                }
            }

            return count;
        }

        /// <summary>
        /// Вернуть самую большую свободную область слоя.
        /// (При генерации )
        /// </summary>
        // REF переместить в StructureGenerator или в Cell
        internal List<Cell> GetFreeCells(Layer layer, Layer relatedLayer)
        {
//            List<Cell> freeCells = new();

            /* Исходные данные:
             1 основной слой layer
             2 связанный слой relatedLayer

             Алгоритм:
             1 Вычислить занятую область(на которой есть сущности и под которой нет "ILand")
             2 Вычислить всю свободную область

             3 Искать свободную ячейку вне свободных областей
             4 Добавить её в новую свободную область, удалить её из всей свободной области
             5 Добавить туда же её соседей и соседей её соседей, удалить их из всей свободной области
             6 перейти к шагу 3

             7 закончить поиск при исчерпании ячеек во всей свободной области
             8 результатом будет свободная область с максимумом ячеек 
            */

            // 1 Вычислить занятую область(на которой есть сущности и под которой нет "ILand")

            List<Cell> engagedCells = new();

            foreach (Cell cell in layer.Cells)
            {
                if (cell.IsEmpty && relatedLayer is not null)
                // ячейка свободна, проверяем связанный слой
                {
                    Cell relatedCell = relatedLayer.Cells[cell.Index];

                    if (!relatedCell.IsEmpty && relatedCell.Entity is ILand)
                    {
                    }
                    else
                    // сущность не может здесь резместиться
                    {
                        engagedCells.Add(cell);
                    }
                }
                else
                // ячейка занята
                {
                    engagedCells.Add(cell);
                }
            }

            // 2 Вычислить всю свободную область
            List<Cell> allFreeCells = new(layer.Cells);
            foreach (Cell cell in engagedCells)
            {
                allFreeCells.Remove(cell);
            }
            // Нашли всю свободную область. Она может состоять из замкнутых непересекающихся областей.
            // Наша задача - найти все такие области и выбрать из них одну область максимальной площади.

            List<List<Cell>> freeCells = new();
            int freeCellsListCount = 0;
            //List<Cell> freeCellsList= new();
            //freeCells.Add(freeCellsList);

            // 3 Искать свободную ячейку вне отдельных свободных областей
            while (0 < allFreeCells.Count)
            {
                freeCellsListCount++;
                List<Cell> freeCellsList = new();
                freeCells.Add(freeCellsList);

                BuildCellsList(allFreeCells[0], freeCellsList);
                // 6 перейти к шагу 3
            }
            // 7 закончить поиск при исчерпании ячеек во всей свободной области

            // процедура, которая полностью строит новый замкнутый список
            void BuildCellsList(Cell seed, List<Cell> list)
            {
                // 4 Добавить её в новую свободную область, удалить её из всей свободной области
                // добавляем ячейку в частный список и удаляем из общего
                list.Add(seed);
                allFreeCells.Remove(seed);
                
                // ищем непустых соседей
                List<Cell> neighbors = seed.GetNeighbors(true);
                // для всех соседей проверяем допустимость и рекурсивно запускаем этот же алгоритм.
                foreach (Cell neighbor in neighbors)
                {
                    if (allFreeCells.Contains(neighbor))
                    {
                        // 5 Добавить туда же её соседей и соседей её соседей, удалить их из всей свободной области
                        BuildCellsList(neighbor, list);
                    }
                }
            }

            // 8 результатом будет свободная область с максимумом ячеек
            int longestListIndex = 0;
            int maxLength = 0;
            for (int index = 0; index < freeCells.Count; index++)
            {
                if (maxLength < freeCells[index].Count)
                {
                    maxLength = freeCells[index].Count;
                    longestListIndex = index;
                }
            }

            return freeCells[longestListIndex];
        }

        /// <summary>
        /// Элементы сцены.
        /// </summary>
        internal List<Entity> Entities { get; }

        /// <summary>
        /// Слои сцены.
        /// </summary>
        internal List<Layer> Layers { get; }

        /// <summary>
        /// Видима.
        /// </summary>
        internal bool IsVisible { get; set; }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal virtual void Load()
        {
            State = SceneState.Ready;
        }

        /// <summary>
        /// Запустить сцену.
        /// (Поменять статус на Активна)
        /// </summary>
        internal virtual void StartOrResume()
        {
            if (State is SceneState.Ready or SceneState.Running)
            {
                State = SceneState.Running;
            }    
            else
            {
                throw new Exception("Unable to activate scene");
            }
        }

        /// <summary>
        /// Приостановить сцену.
        /// (Поменять статус на Готова)
        /// </summary>
        internal virtual void Suspend()
        {
            if (State is SceneState.Ready or SceneState.Running)
            {
                State = SceneState.Ready;
            }
        }

        /// <summary>
        /// Очистить сцену.
        /// </summary>
        internal virtual void Clear()
        {
            TickCount = 0;

            foreach (Layer layer in Layers)
            {
                //foreach (Cell cell in layer.Cells)
                //{
                //    //
                //}
                layer.Cells.Clear();
            }

            OverlayTextFrames.Clear();
            TextFrames.Clear();
            Pictures.Clear();
            ProgressBars.Clear();

            Layers.Clear();
            Entities.Clear();

            State = SceneState.Preparing;
        }
    }
}
