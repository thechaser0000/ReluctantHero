/*
Стрелок. Простой враг.
Наследуется от простой направленной сущности.
Стреляет, если на линии огня есть цель.
Пытается переместиться так, чтобы цель оказалася на линии огня.

2022-12-02
*/

using PlatformConsole;
using Scge;
using System;

namespace TestGame
{
    /// <summary>
    /// Преследователь (враг).
    /// </summary>
    internal class Shooter : Directional, IDamageableOfShot, IDamageableOfKnife, IMoveable, IDamageableOfExplosion
    {
        /// <summary>
        /// Умер.
        /// (И подлежит удалению)
        /// </summary>
        public override bool IsDead
        {
            get => base.IsDead;
            set
            {
                base.IsDead = value;
                NeedDelete = value;
            }
        }

        /// <summary>
        /// Огнестрельное оружие.
        /// </summary>
        internal ShotGun ShotGun { get; set; }

        /// <summary>
        /// Цель.
        /// </summary>
        internal Entity? Target { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Shooter(Cell cell) : base(cell)
        {
            ShotGun = new(this, 0, 0, GameSeed.EnemyShotDamage) { IsEnabled = true, IsInfinityAmmo = true, Delay = new(GameSeed.EnemyShotDelay) };
            MapColor = GameSeed.MapColorEnemy;
            Health.Maximum = GameSeed.ShooterHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.ShooterSpriteSet);
        }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            ShotGun.DoCheck();

            if (IsUnicellular)
            // если персонаж одноклеточный
            {
                if (Target is not null && !Target.NeedDelete)
                // цель существует
                {
                    Direction viewDirection = Cell.GetViewDirection(this.Cell!, Target.Cell!);
                    // Если цель стоит на линии огня и персонаж повернут лицом к цели - стреляем
                    if (viewDirection is not Direction.None && viewDirection == Direction)
                    {
                        ShotGun.Attack();
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Враги с вероятностью перемещается к герою, если видит его, или перемещается в случайном направлении.
        /// </summary>
        private static Direction MoveEnemy(Entity enemy, Entity? target, double trackingProbability, int shotDistance)
        {
            if (target is not null && !target.NeedDelete)
            // цель существует
            {
                if (Cell.GetPathLength(enemy.Cell!, target.Cell!) <= shotDistance && Randomizer.ItIsTrue(trackingProbability))
                // цель в пределах видимости и сработала вероятность - приближаемся к линии оня.
                {
                    Direction viewDirection = Cell.GetViewDirection(enemy.Cell!, target.Cell!);

                    if (viewDirection is not Direction.None)
                    // находимся на линии огня
                    {
                        return viewDirection;
                    }
                    else
                    // находимся не на линии огня
                    {
                        (Direction horizontalDirection, Direction verticalDirection) = Cell.GetDirection(enemy.Cell, target.Cell);
                        (int columnDistance, int rowDistance) = Cell.GetDistance(enemy.Cell!, target.Cell!);
                        Direction direction = Direction.None;

                        // приближаемся к линии огня по кратчайшему направлению
                        if (Math.Abs(columnDistance) < Math.Abs(rowDistance))
                        // сокращаем дистанцию по столбцу
                        {
                            direction = enemy.Move(horizontalDirection);
                        }

                        // сокращаем дистанцию по строке
                        if (direction is Direction.None)
                        {
                            direction = enemy.Move(verticalDirection);
                        }

                        return direction;
                    }
                }
            }

            // враг перемещается в случайном направлении или стоит на месте
            if (Randomizer.ItIsTrue(GameSeed.EnemyStrayProbability))
            {
                return enemy.Move(Randomizer.RandomAssuredDirection);
            }
            else
            {
                return Direction.None;
            }

        }

        /// <summary>
        /// Выполнить тик игрового цикла.
        /// </summary>
        internal override void DoMove()
        {
            ShotGun.IsAttack = false;

            // Враг выходит на линию огня или слоняется без дела.
            Direction direction = MoveEnemy(this, Target, GameSeed.EnemyChaseProbability, GameSeed.EnemyShotDistance);

            if (direction is not Direction.None)
            // Поворачиваем в сторону перемещения.
            {
                Direction = direction;
            }

            base.DoMove();
        }
    }

    /// <summary>
    /// Снайпер.
    /// (Дальнобойный стрелок)
    /// </summary>
    internal class Sniper: Shooter
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        internal Sniper(Cell cell) : base(cell)
        {
            ShotGun = new(this, 0, 0, GameSeed.EnemyShotDamage) { IsEnabled = true, IsInfinityAmmo = true, Delay = new(GameSeed.EnemyShotDelay) };
            MapColor = GameSeed.MapColorEnemy;
            Health.Maximum = GameSeed.ShooterHealth;
            Health.Restore();
        }

        /// <summary>
        /// Выполнить тик игрового цикла.
        /// </summary>
        internal override void DoMove()
        {
            ShotGun.IsAttack = false;

            // Враг выходит на линию огня или слоняется без дела.
            Direction direction = MoveEnemy(this, Target, GameSeed.EnemyChaseProbability, GameSeed.SniperShotDistance);

            if (direction is not Direction.None)
            // Поворачиваем в сторону перемещения.
            {
                Direction = direction;
            }
        }

        /// <summary>
        /// Снайпер с вероятностью поворачиваются к герою, если видит его, или стоит на месте
        /// </summary>
        private static Direction MoveEnemy(Entity enemy, Entity? target, double trackingProbability, int shotDistance)
        {
            if (target is not null && !target.NeedDelete)
            // цель существует
            {
                if (Cell.GetPathLength(enemy.Cell!, target.Cell!) <= shotDistance && Randomizer.ItIsTrue(trackingProbability))
                // цель в пределах видимости и сработала вероятность - атакуем или приближаемся к линии оня.
                {
                    return Cell.GetViewDirection(enemy.Cell!, target.Cell!);
                }
            }

            return Direction.None;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.SniperSpriteSet);
        }
    }
}
