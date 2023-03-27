/*
Выстрел. Огнестрельный выстрел.  
Наследуется от простой направленной сущности.
Летит из заданной точки в заданном направлении, наносит ущерб первому оказавшемуся на пути носителю IShotDamaged, самоуничтожается при встрече с любым препятствием.
  
2022-11-16
 */

using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Выстрел.
    /// </summary>
    internal class Shot : Directional
    {
        /// <summary>
        /// Поврежение.
        /// </summary>
        internal Characteristic Damage { get; init; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Shot(Cell cell, Direction direction, Characteristic damage) : base(cell)
        {
            Direction = direction;
            Damage = damage;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.ShotSpriteSet);
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            HandleCollision();
        }

        /// <summary>
        /// Обработтаь столкновение.
        /// </summary>
        private void HandleCollision()
        {
            // не уничтожаем выстрел только если перед ним находится пустая ячейка.
            bool needDelete = true;

            // Проверяем, есть ли перед выстрелом противник, который может принять урон.
            if (IsUnicellular)
            // если выстрел одноклеточный
            {
                // проверяем ячейку перед персонажем
                Cell neighbor = Cell.GetNeighbor(Direction);

                if (neighbor?.Entity is IDamageableOfShot opponent)
                // нашли в ячейке противника, подверженного урону от прикосновения, наносим урон
                {
                    opponent.Damage(Damage.Value);
                }
                else if (neighbor?.IsEmpty == true)
                // перед выстрелом находится пустая ячейка.
                {
                    needDelete = false;
                }

                NeedDelete = needDelete;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Выполнить перемещение.
        /// </summary>
        internal override void DoMove()
        {
            // Переместиться на одну ячейку (возвращает истину если удалось переместиться).
            bool MoveOne(Cell destination)
            {
                // Проверить столкновение 
                if (IsUnicellular)
                // если персонаж одноклеточный
                {
                    if (!(destination?.IsEmpty == true))
                    // ячейка перед выстрелом занята или не существует
                    {
                        return false;
                    }
                }
                else
                // Такого не должно быть
                {
                    throw new NotImplementedException();
                }

                // Переместить дальше.
                this.MoveTo(destination);
                return true;
            }

            // ищем оппонента перед персонажем
            Cell neighbor = Cell.GetNeighbor(Direction);
            // делаем 1 или 2 шага
            if (MoveOne(neighbor))
            {
                neighbor = neighbor.GetNeighbor(Direction);
                if (!MoveOne(neighbor))
                {
                    HandleCollision();
                }
            }

            base.DoMove();
        }

    }
}
