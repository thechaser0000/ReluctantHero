/*
Преследователь. Простой враг.
Наследуется от простой направленной сущности.
Периодически ударяет ножом (всех).
Преследует указанную цель, если она находится в зоне досягаемости.
Может преследовать других врагов.

2022-11-16
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Страж (враг).
    /// </summary>
    internal class Guardian: Chaser
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        internal Guardian(Cell cell) : base(cell)
        {
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.GuardianSpriteSet);
        }
    }

    /// <summary>
    /// Преследователь (враг).
    /// </summary>
    internal class Chaser : Directional, IDamageableOfShot, IDamageableOfKnife, IMoveable, IDamageableOfExplosion
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
        internal Knife Knife { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Chaser(Cell cell) : base(cell)
        {
            Knife = new(this, GameSeed.EnemyKnifeDamage) { IsEnabled = true, Delay = new(GameSeed.EnemyKnifeDelay) };
            MapColor = GameSeed.MapColorEnemy;
            Health.Maximum = GameSeed.ChaserHealth;
            Health.Value = Health.Maximum;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            return (int)Direction + (IsAttack ? 5 : 0);
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.ChaserSpriteSet);
        }

        /// <summary>
        /// Находится в состоянии атаки.
        /// </summary>
        internal bool IsAttack => Knife.IsAttack;

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
            Knife.DoCheck();

            if (IsUnicellular)
            // если персонаж одноклеточный
            {
                // Периодически лупим ножом всех подряд, кто принимает урон, даже своих братьев.
                if (Randomizer.ItIsTrue(GameSeed.EnemyKnifeProbability) && Direction is not Direction.None)
                {
                    // Атакуем только героя, Анну или медведя
                    Entity? target = Cell.GetNeighbor(Direction)?.Entity;

                    if (target is null || (target is Hero || target is Anna || target is Bear))
                    {
                        Knife.Attack();
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Преследователь с вероятностью перемещается к герою, если видит его, или перемещается в случайном направлении.
        /// </summary>
        private static Direction MoveEnemy(Entity enemy, Entity? target, double chaseProbability)
        {
            if (target is not null && !target.NeedDelete)
            // цель существует
            {
                if (Cell.GetPathLength(enemy.Cell!, target.Cell!) <= GameSeed.EnemyChaseDistance && Randomizer.ItIsTrue(chaseProbability))
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

            if (Randomizer.ItIsTrue(GameSeed.EnemyStrayProbability))
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
        /// Выполнить тик игрового цикла.
        /// </summary>
        internal override void DoMove()
        {
            if (IsDead)
            {
                return;
            }

            Knife.IsAttack = false;

            // Враг ходит за целью или слоняется без дела.
            Direction direction = MoveEnemy(this, Target, GameSeed.EnemyChaseProbability);

            if (direction is not Direction.None)
            // Поворачиваем в сторону перемещения.
            {
                Direction = direction;
            }

            base.DoMove();
        }
    }
}
