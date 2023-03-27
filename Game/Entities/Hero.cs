/*
Герой. Главный герой, протагонист.
Наследуется от простой направленной сущности.
 
2022-11-16
*/

using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Герой.
    /// </summary>
    internal class Hero : Simple, IDamageableOfKnife, IMoveable, IDamageableOfShot, IDamageableOfExplosion
    {
        /// <summary>
        /// Опыт.
        /// </summary>
        internal Characteristic Experience { get; set; }

        /// <summary>
        /// Огнестрельное оружие.
        /// </summary>
        internal ShotGun ShotGun { get; init; }

        /// <summary>
        /// Нож.
        /// </summary>
        internal Knife Knife { get; set; }

        /// <summary>
        /// Уровень развития.
        /// </summary>
        internal int EvolutionLevel { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Hero() : base()
        {
            MapColor = GameSeed.MapColorHero;
            ShotGun = new(this, GameSeed.HeroStartAmmo, GameSeed.HeroBaseAmmo, GameSeed.HeroShotDamage) { Delay = new(4)};
            Knife = new(this, GameSeed.HeroKnifeDamage) { Delay = new(2) };
            Health.Maximum = GameSeed.HeroBaseHealth;
            Health.Value = GameSeed.HeroStartHealth;
            Experience = new(0, GameSeed.HeroBaseExperience);
            EvolutionLevel = 0;
        }

        /// <summary>
        /// Обработчик выбора текущего спрайта.
        /// </summary>
        internal override int SpriteSelection(int currentSelectedIndex)
        {
            if (IsAlive)
            {
                return (int)Direction + (Knife.IsAttack ? 5 : 0) + (ShotGun.IsAttack ? 10 : 0);
            }
            else
            {
                return 15;
            }
        }

        /// <summary>
        /// Выполнить перемещение.
        /// </summary>
        internal override void DoMove()
        {
            Knife.IsAttack = false;
            ShotGun.IsAttack = false;

            if (IsDead)
            {
                return;
            }

            // обработка управления героя
            // поворот
            Direction direction = Direction.None; 
            if (Game.InputController.MoveUp.Impacted)
            // вверх
            {
                direction = Direction.Up;
            }
            else if (Game.InputController.MoveDown.Impacted)
            // вниз
            {
                direction = Direction.Down;
            }
            else if (Game.InputController.MoveRight.Impacted)
            // вправо
            {
                direction = Direction.Right;
            }
            else if (Game.InputController.MoveLeft.Impacted)
            // влево
            {
                direction = Direction.Left;
            }
            else
            {
                // сохраняем направление
            }

            // двигаем героя
            if (direction != Direction.None)
            {
                Direction = direction;
                Move(Direction, false);
            }

            base.DoMove();
        }

        /// <summary>
        /// Находится в состоянии атаки.
        /// </summary>
        internal bool IsAttack => Knife.IsAttack || ShotGun.IsAttack;

        /// <summary>
        /// Выполнить проверки (после перемещения).
        /// </summary>
        internal override void DoCheck()
        {
            base.DoCheck();

            // Проверяем состояние оружия.
            Knife.DoCheck();
            // Проверяем состояние оружия.
            ShotGun.DoCheck();


            // Нажат выстрел.
            if (Game.InputController.Fire.Impacted && IsAlive)
            {
                //                IsAttack = true;
                // Пробуем стрелять. 
                if (!ShotGun.Attack())
                // Тычем ножом, если выстрел не удался.
                {
                    Knife.Attack();
                }
            }
        }

        /// <summary>
        /// Жив.
        /// </summary>
        internal bool IsAlive => !IsDead;

        /// <summary>
        /// Процедура заполнения списка спрайтов.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.HeroSpriteSet);
        }
    }
}
