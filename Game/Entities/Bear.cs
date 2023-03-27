/*
Медведь.
Мирный NPC

2023-01-12
*/

using PlatformConsole;
using Scge;
using System;


namespace TestGame
{
    /// <summary>
    /// Медведь.
    /// </summary>
    internal class Bear : Simple, IDamageableOfShot, IDamageableOfKnife, IMoveable, IDamageableOfExplosion
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
        /// Нож.
        /// </summary>
        internal Knife Claws { get; set; }

        /// <summary>
        /// Находится в состоянии атаки.
        /// </summary>
        internal bool IsAttack => Claws.IsAttack;

        /// <summary>
        /// Цель.
        /// </summary>
        internal Entity? Target { get; set; }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            // Проверяем состояние оружия.
            Claws.DoCheck();

            if (IsUnicellular)
            // если персонаж одноклеточный
            {
                // Периодически лупим когтями всех подряд, кто принимает урон, даже своих братьев.
                if (Randomizer.ItIsTrue(GameSeed.BearClawsProbability) && Direction is not Direction.None)
                {
                    Claws.Attack();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction + (IsAttack ? 5 : 0);
        }

        /// <summary>
        /// Преследователь с вероятностью перемещается к герою, если видит его, или перемещается в случайном направлении.
        /// </summary>
        private static Direction MoveEnemy(Entity enemy, Entity? target, double chaseProbability)
        {
            if (target is not null && !target.NeedDelete)
            // цель существует
            {
                if (Cell.GetPathLength(enemy.Cell!, target.Cell!) <= GameSeed.BearChaseDistance && Randomizer.ItIsTrue(chaseProbability))
                // цель в пределах видимости и сработала вероятность - приближаемся к цели.
                {
                    Direction chase = enemy.ChaseTo(target.Cell!);
                    if (chase is not Direction.None)
                    {
                        return chase;
                    }
                    // если переместиться не удалось - поворачивается к герою (чтобы атаковать)
                    {
                        (Direction horizontalDirection, Direction verticalDirection) = Cell.GetDirection(enemy.Cell, target.Cell);

                        return horizontalDirection is not Direction.None ? horizontalDirection : verticalDirection;
                    }
                }
            }

            if (Randomizer.ItIsTrue(GameSeed.BearStrayProbability))
            // враг перемещается или поворачивается в случайном направлении
            {
                Direction move = enemy.Move(Randomizer.RandomAssuredDirection);

                if (move is not Direction.None)
                {
                    return move;
                }
                else
                {
                    return Randomizer.RandomAssuredDirection;
                }
            }
            else
            // враг стоит на месте
            {
                return Direction.None;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Bear(Cell cell) : base(cell)
        {
            Claws = new(this, GameSeed.EnemyClawsDamage) { IsEnabled = true, Delay = new(GameSeed.EnemyClawsDelay) };
            MapColor = GameSeed.MapColorNpc;
            Health.Maximum = GameSeed.BearHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.BearSpriteSet);
        }

        /// <summary>
        /// Выполнить тик игрового цикла.
        /// </summary>
        internal override void DoMove()
        {
            Claws.IsAttack = false;

            // Враг ходит за целью или слоняется без дела.
            Direction direction = MoveEnemy(this, Target, GameSeed.BearChaseProbability);

            if (direction is not Direction.None)
            // Поворачиваем в сторону перемещения.
            {
                Direction = direction;
            }

            base.DoMove();
        }

    }
}
