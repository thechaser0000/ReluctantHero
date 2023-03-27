/*
Сущность игрового мира. Предок всех остальных сущностей. 

2022-11 
*/

using PlatformConsole;
using System.Runtime.CompilerServices;

namespace Scge
{
    /// <summary>
    /// Сущность.
    /// </summary>
    internal class Entity
    {
        /// <summary>
        /// Связанный слой.
        /// (Может использоваться для проверки ячеек над или под сущностью)
        /// </summary>
        internal Layer? RelatedLayer { get; set; }

        /// <summary>
        /// Сцена.
        /// </summary>
        internal Scene? Scene => Cells?.Center?.Layer.Scene;

        /// <summary>
        /// Должен быть удален.
        /// (Помеченная таким образом сущность будет удалена со слоя на следующем тике)
        /// </summary>
        internal bool NeedDelete { get; set; }

        /// <summary>
        /// Первый тик (запрещает перемещение - сущность будет отрисована, но не успеет переместиться из ячейки появления)
        /// </summary>
//        internal bool IsFirstTick { get; private set; }

        protected bool _IsVisible;

        /// <summary>
        /// Видим.
        /// </summary>
        internal virtual bool IsVisible
        {
            get => _IsVisible;
            set
            {
                if (value != _IsVisible)
                {
                    _IsVisible= value;

                    if (!value)
                    // Генерируем событие скрытия
                    {
                        Hided?.Invoke(this, new(this));
                    }
                }
            }
        }

        /// <summary>
        /// Область ячеек, в которой находится сущность.
        /// </summary>
        internal CellArea Cells { get; set; }

        /// <summary>
        /// Здоровье.
        /// </summary>
        public Characteristic Health { get; init; }

        /// <summary>
        /// Умер.
        /// </summary>
        public virtual bool IsDead { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Entity(List<Cell>? cells = null)
        {

            Health = new(1);
//            IsFirstTick = true;
            NeedDelete = false;
            IsVisible = true;
            Direction = Direction.None;
            Cells = new(this, cells);

            // Добавляем к списку сущностей автоматически.
            Scene?.Entities.Add(this);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Entity(Cell cell) : this(new List<Cell>() { cell })
        {
            // всё делает другой конструктор
        }

        /// <summary>
        /// Выполнить перемещение.
        /// </summary>
        internal virtual void DoMove()
        {
//            IsFirstTick = false;
            // REF 2 следовало бы проверять фактическое перемещение, хотя бы сравнивать положение на предыдущем и текущем шаге + направление
            Moved?.Invoke(this, new(this));
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal virtual void DoCheck()
        {
        }

        /// <summary>
        /// Вызвать событие Created.
        /// </summary>
        internal void InvokeEntityCreated(Entity owner, Entity entity)
        {
            Created?.Invoke(this, new(owner, entity));
        }

        /// <summary>
        /// Тэг.
        /// </summary>
        internal object? Tag { get; set; }

        /// <summary>
        /// Очистить (удаляются все связи между Cell и Entity).
        /// </summary>
        internal virtual void Clear()
        {
            // Вызываем событие удаления
            Deleting?.Invoke(this, new(this));

            IsVisible = false;
            Cells.Clear();
            Tag = null;
            Direction = Direction.None;
        }

        /// <summary>
        /// Является одноклеточным.
        /// </summary>
        internal bool IsUnicellular => 1 == Cells.Count;

        /// <summary>
        /// Единственная ячейка.
        /// </summary>
        public Cell? Cell
        {
            get => IsUnicellular ? Cells.Center : null;
            set
            {
                Cells.Clear();

                if (value is not null)
                {
                    value.Entity = this;
                    Cells.Add(value);
                }
            }
        }

        /// <summary>
        /// Текущее направление.
        /// </summary>
        internal virtual Direction Direction { get; set; }

        /// <summary>
        /// Поменять местами сущности.
        /// </summary>
        internal static void ExchangeEntity(Cell source, Cell destination)
        {
            Entity? sourceEntity = source.Entity;
            Entity? destinationEntity = destination.Entity;

            /// меняем местами сущности
            (source.Entity, destination.Entity) = (destination.Entity, source.Entity);

            // меняем местами ячейки сущностей
            sourceEntity?.Cells.Replace(source, destination);

            destinationEntity?.Cells.Replace(destination, source);
        }

        /// <summary>
        /// Переместить сущность из ячейки в указанном направлении.
        /// </summary>
        internal static Direction MoveEntity(Cell source, Direction direction, bool allowTeleportation = false)
        {
            Cell? destination = source.GetNeighbor(direction);

            // клетка, куда предполагаем переместиться, доступна и не занята 
            if (destination is not null && destination.IsEmpty)
            {
                //// если источник содержит сущность, которая перемещается только по суше и у неё есть связанный слой, то проверяем, является ли сущность в ячейке назначения сушей. 
                //if (!source.IsEmpty && source.Entity is IMoveable && source.Entity.RelatedLayer is not null)
                //{
                //    Cell relatedDestination = source.Entity.RelatedLayer.Cells[destination.Index];
                //    if (!relatedDestination.IsEmpty && relatedDestination.Entity is not ILand)
                //    {
                //        // в целевой ячейке нельзя находиться
                //        return Direction.None;
                //    }
                //}

                // обмениваем сущности в клетках (по сути, одна из них - null)
//                ExchangeEntity(destination, source);
                return MoveEntityTo(source, destination);

//                return direction;
            }
            else
            {
                // пытаемся переместиться в занятую ячейку или за пределы мира
                if (destination is not null)
                // целевая ячейка занята
                {
                    return Direction.None;
                }
                else
                // целевая ячейка за пределами сцены
                {
                    // телепортируем элемент на противоположную сторону карты
                    if (allowTeleportation)
                    {
                        destination = source.GetReflection(direction);

                        return MoveEntityTo(source, destination);
                    }
                    else
                    {
                        return Direction.None;
                    }
                }
            }
        }

        /// <summary>
        /// Переместить сущность из одной ячейки в другую.
        /// </summary>
        internal static Direction MoveEntityTo(Cell source, Cell destination)
        {
            // клетка, куда предполагаем переместиться не занята 
            if (destination.IsEmpty)
            {
                // если источник содержит сущность, которая перемещается только по суше и у неё есть связанный слой, то проверяем, является ли сущность в ячейке назначения сушей. 
                if (!source.IsEmpty && source.Entity is IMoveable && source.Entity.RelatedLayer is not null)
                {
                    Cell relatedDestination = source.Entity.RelatedLayer.Cells[destination.Index];
                    if (!relatedDestination.IsEmpty && relatedDestination.Entity is not ILand)
                    {
                        // в целевой ячейке нельзя находиться
                        return Direction.None;
                    }
                }

                // обмениваем итемы в клетках (по сути, одна из них - null)
                ExchangeEntity(destination, source);

                (Direction horizontal, Direction vertical) = Cell.GetDirection(source, destination);

                // всегда возвращаем направление по горизонтали при его наличии, либо направление по вертикали.    
                return horizontal is not Direction.None ? horizontal : vertical;
            }
            else
            {
                // пытаемся переместиться в занятую ячейку или за пределы мира
                return Direction.None;
            }
        }

        /// <summary>
        /// Перемещает сущность из группы ячеек в указанном направлении (возвращает Success или код последней неудачи).
        /// </summary>
        internal static Direction MoveEntity(List<Cell> source, Direction direction, bool allowTeleportation = false)
        {
            //            MoveResult result = 0 == source.Count ? MoveResult.NotAssignedEntity : MoveResult.Success;
            Direction result = 0 == source.Count ? Direction.None : direction;

            List<Cell> destination = new();

            for (int index = 0; index < source.Count; index++)
            {
                Cell sourceCell = source[index];
                Cell destinationCell = sourceCell.GetNeighbor(direction);

                // назначение существует не занято - это хорошо
                if (destinationCell is not null && destinationCell.IsEmpty)
                {
                    destination.Add(destinationCell);
                }
                else if (destinationCell is not null && !destinationCell.IsEmpty)
                // назначение существует, но занято 
                {
                    if (source.Find(c => c == destinationCell) is not null)
                    // назначение занято одной из исходных ячеек - это тоже хорошо
                    {
                        destination.Add(destinationCell);
                    }
                    else
                    // назначение занято чужой ячейкой - откатываем всё и выходим
                    {
                        return Direction.None;
                    }
                }
                else if (destination is not null)
                // назначение не существует - мы достигли границы сцены
                {
                    if (allowTeleportation)
                    // можно телепортироваться - пробуем это сделать (может и не получиться)
                    {
                        destinationCell = sourceCell.GetReflection(direction);

                        // ниже повторяется код из ветки выше (можно от этого избавиться, начав телепортацию сразу после установки назначения )
                        // (либо можно упростить код, не проверяя на существование - отражение всегда существует)
                        #region Repeat
                        // назначение существует не занято - это хорошо
                        if (destinationCell is not null && destinationCell.IsEmpty)
                        {
                            destination.Add(destinationCell);
                        }
                        else if (destinationCell is not null && !destinationCell.IsEmpty)
                        // назначение существует, но занято 
                        {
                            if (source.Find(c => c == destinationCell) is not null)
                            // назначение занято одной из исходных ячеек - это тоже хорошо
                            {
                                destination.Add(destinationCell);
                            }
                            else
                            // назначение занято чужой ячейкой - откатываем всё и выходим
                            {
                                //return MoveResult.CellEngaged;
                                return Direction.None;
                            }
                        }
                        #endregion
                    }
                    else
                    // телепортация запрещена - откатываем всё и выходим
                    {
                        return Direction.None;
                    }
                }
            }

            // если всё нормально, то производим смену ячеек в сущности (я джва дня бился над этим).
            if (result is not Direction.None)
            {
                // у нас есть исходный список ячеек с какими-то сущностями (их может быть несколько)
                // у нас есть список с сущностями исходных ячеек (сущности из обоих списков ячеек будут меняться по мере обработки, так как сущности привязаны к ячейкам)
                // у нас есть новый список ячеек с сущностями, которые находятся там в настоящий момент (до перемещения).

                // нам нужно обновить у сущностей список их ячеек
                // нам нужн поменять у ячеек сущности

                // перемещаем сущности из источника в отдельный список
                List<Entity> entities = new ();
                for (int index = 0; index < source.Count; index++)
                {
                    entities.Add(source[index].Entity);
                    source[index].Entity = null;
                }

                // восстанавливаем сущности в новых ячейках
                for (int index = 0; index < source.Count; index++)
                {
                    Cell sourceCell = source[index];
                    Cell destinationCell = destination[index];
                    Entity entity = entities[index];

                    if (entity is not null)
                    {
                        entity.Cells.Replace(sourceCell, destinationCell);
                        destinationCell.Entity = entity;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Переместить элемент в указанную ячейку.
        /// </summary>
        internal Direction MoveTo(Cell destination)
        {
            if (IsUnicellular)
            {
                return MoveEntityTo(Cell!, destination);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Переместить элемент в направлении указанной ячейки на 1 клетку
        /// </summary>
        internal Direction ChaseTo(Cell target)
        {
            // Алгоритм не строит пути, а просто пытается приблизиться к цели по алгоритму, иногда отступая назад, чтобы попытаться обойти препятствие:

            if (IsUnicellular)
            {
                // 1. Пытаемся приблизится по горизонтали/вертикали (выбираем случайно)
                // 2. Если не удалось, пытаемся приблизится по вертикали/горизонтали (противоположно предыдущему шагу)
                // REF ? Улучшение: неплохо бы добавить параметр для случая, если преследователь может телепортироваться на другую сторону карты (путь через границу может оказаться быстрее). Причем таких методов много.
                (Direction horizontalDirection, Direction verticalDirection) = Cell.GetDirection(Cell, target);
                Direction direction;

                if (Randomizer.ItIsTrue(1 / 2.0))
                {
                    direction = Move(horizontalDirection);
                    if (direction is not Direction.None)
                    {
                        return direction;
                    };

                    direction = Move(verticalDirection);
                    if (direction is not Direction.None)
                    {
                        return direction;
                    }
                }
                else
                {
                    direction = Move(verticalDirection);
                    if (direction is not Direction.None)
                    {
                        return direction;
                    }

                    direction = Move(horizontalDirection);
                    if (direction is not Direction.None)
                    {
                        return direction;
                    };
                }

                // 3. Если не удалось, двигаемся в случайном направлении или стоим на месте
                //                return Move((Direction)Randomizer.Random.Next(5));
                return Move(Randomizer.RandomDirection);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Переместить элемент в указанном направлении на 1 клетку.
        /// </summary>
        internal Direction Move(Direction direction, bool allowTeleportation = false)
        {
            if (direction is not Direction.None)
            {

                if (IsUnicellular)
                // сущность занимает одну ячейку
                {
                    return MoveEntity(Cell!, direction, allowTeleportation);
                }
                else
                {
                    if (0 < Cells.Count)
                    // элемент находится в нескольких ячейках (нужно переместить их все по одной в определенном порядке, чтобы они не мешали друг другу)
                    {
                        return MoveEntity(Cells.Items, direction, allowTeleportation);
                    }
                }
            }
            
            return Direction.None;
        }

        /// <summary>
        /// Обработчик простого события с сущностью.
        /// </summary>
        public delegate void EntityEventHandler(object sender, EntityEventArgs e);

        /// <summary>
        /// Обработчик создания сущности внутри другой сущности.
        /// </summary>
        public delegate void EntityCreatedEventHandler(object sender, EntityCreatedEventArgs e);

        /// <summary>
        /// Событие удаления сущности.
        /// </summary>
        internal event EntityEventHandler Deleting;

        /// <summary>
        /// Событие скрытия сущности.
        /// </summary>
        internal event EntityEventHandler Hided;

        /// <summary>
        /// Сущность переместилась
        /// </summary>
        internal event EntityEventHandler Moved;

        /// <summary>
        /// Создана новая сущность внутри данной.
        /// </summary>
        internal event EntityCreatedEventHandler Created;
    }

    /// <summary>
    /// Аргументы простого события с сущностью.
    /// </summary>
    internal class EntityEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal EntityEventArgs(Entity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Удаляемая сущность.
        /// </summary>
        internal Entity Entity { get; init; }
    }

    /// <summary>
    /// Аргументы обработчика создания сущности внутри другой сущности.
    /// </summary>
    internal class EntityCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal EntityCreatedEventArgs(Entity owner, Entity entity)
        {
            Entity = entity;
            Owner = owner;
        }

        /// <summary>
        /// Владелец.
        /// </summary>
        internal Entity Owner { get; init; }

        /// <summary>
        /// Создаваемая сущность.
        /// </summary>
        internal Entity Entity { get; init; }
    }
}
