/*
Зерно игры - список основных констант. 
 
2022-10-27
*/

namespace TestGame
{
    /// <summary>
    /// Зерно игры - список основных констант.
    /// </summary>
    internal static class GameSeed
    {
        /// <summary>
        /// Счётчик обращений
        /// </summary>
        private static int accessCounter = 0;

        /// <summary>
        /// Неизвестный цвет (при каждом вызове меняет значение, позволяет отследить проблемы отрисовки). 
        /// </summary>
        internal static ConsoleColor UnknownColor
        {
            get
            {
                accessCounter++;
                return 0 == accessCounter % 2 ? ConsoleColor.Green : ConsoleColor.Red;
            }
        }

        /// <summary>
        /// Цвет панелей.
        /// </summary>
        internal const ConsoleColor PanelColor = ConsoleColor.Gray;

        /// <summary>
        /// Альтернативный цвет панелей.
        /// </summary>
        internal const ConsoleColor PanelColorAlt = ConsoleColor.DarkGray;

        /// <summary>
        /// Параметры отрисовки текста
        /// </summary>
        internal const ConsoleColor PlainTextColor = ConsoleColor.DarkGray;

        /// <summary>
        /// Параметры отрисовки выделенного текста
        /// </summary>
        internal const ConsoleColor HighlightTextColor = ConsoleColor.Red;

        /// <summary>
        /// Альтернативные Параметры отрисовки текста
        /// </summary>
        internal const ConsoleColor PlainTextColorAlt = ConsoleColor.Black;

        /// <summary>
        /// Прозрачный цвет.
        /// </summary>
        internal const ConsoleColor TransparentColor = ConsoleColor.Black; // ConsoleColor.DarkBlue;

        /// <summary>
        /// Цвет на карте чекпоинта.
        /// </summary>
        internal const ConsoleColor MapColorCheckpoint = ConsoleColor.DarkMagenta;

        /// <summary>
        /// Цвет на карте врага.
        /// </summary>
        internal const ConsoleColor MapColorEnemy = ConsoleColor.DarkRed;

        /// <summary>
        /// Цвет на карте препятствий.
        /// </summary>
        internal const ConsoleColor MapColorObstacle = ConsoleColor.DarkYellow;

        /// <summary>
        /// Цвет на карте строений.
        /// </summary>
        internal const ConsoleColor MapColorBuilding = ConsoleColor.Yellow;

        /// <summary>
        /// Цвет на карте пропасти.
        /// </summary>
        internal const ConsoleColor MapColorChasm = ConsoleColor.Black;

        /// <summary>
        /// Цвет на карте зарослей.
        /// </summary>
        internal const ConsoleColor MapColorThickets = ConsoleColor.Green;

        /// <summary>
        /// Цвет на карте поверхности.
        /// </summary>
        internal const ConsoleColor MapColorGround = ConsoleColor.White;

        /// <summary>
        /// Цвет на карте тумана войны.
        /// </summary>
        internal const ConsoleColor MapColorFogOfWar = ConsoleColor.Gray;

        /// <summary>
        /// Цвет на карте воды.
        /// </summary>
        internal const ConsoleColor MapColorWater = ConsoleColor.Blue;

        /// <summary>
        /// Цвет на карте героя.
        /// </summary>
        internal const ConsoleColor MapColorHero = ConsoleColor.Magenta;

        /// <summary>
        /// Цвет на карте союзника.
        /// </summary>
        internal const ConsoleColor MapColorAssociate = ConsoleColor.Cyan;

        /// <summary>
        /// Цвет на карте трофея.
        /// </summary>
        internal const ConsoleColor MapColorTrophy = ConsoleColor.DarkGray;

        /// <summary>
        /// Цвет на карте нейтрального персонажа.
        /// </summary>
        internal const ConsoleColor MapColorNpc = ConsoleColor.Gray;

        // размеры окна консоли (102х70)
        internal const int WindowWidth = 106 - 8 + 4; // 107 max
        internal const int WindowHeight = 35;  // 35 max

        /// <summary>
        /// Длительность тика.
        /// </summary>
        internal const int TickDuration = 250;

        // REF эти области нужно переделать на Rectangle
        // Параметры вюпорта игрового поля
        internal const int FieldColumns = 11;
        internal const int FieldLines = 11;
        internal const int FieldLeft = 2;
        internal const int FieldTop = 1;
        internal const int FieldCellWidth = 6;
        internal const int FieldCellHeight = 3;
        internal const int FieldWidth = FieldColumns * FieldCellWidth;
        internal const int FieldHeight = FieldLines * FieldCellHeight + 1;

        // Параметры вюпорта карты
        internal const int MapLeft = FieldLeft + FieldWidth + 2;
        internal const int MapTop = FieldTop;
        internal const int MapColumns = WindowWidth - FieldWidth - 8;
        internal const int MapLines = 26 + 6;
        internal const int MapWidth = MapColumns + 2;
        internal const int MapHeight = (MapLines + 1) / 2;

        // Параметры вюпорта интерфейса
        internal const int InterfaceLeft = MapLeft;
        internal const int InterfaceTop = MapTop + MapHeight + 2;
        internal const int InterfaceWidth = MapWidth;
        internal const int InterfaceHeight = WindowHeight - MapHeight - 4;

        // Параметры игрового мира
        /// <summary>
        /// Начальное количество ячеек этапа.
        /// </summary>
        internal const int StartCells = 1000;
        /// <summary>
        /// Прирост ячеек на этап
        /// </summary>
        internal const int IncrementCells = 300;
        
        /// <summary>
        /// Максимальное соотношение сторон сцены.
        /// </summary>
        internal const int MaxAspectRatio = 3;
        
        /// <summary>
        /// Плотность врагов на 1к ячеек.
        /// </summary>
        internal const int EnemyDensity1k = 4;

        /// <summary>
        /// Плотность ящиков на 1к ячеек.
        /// </summary>
        internal const int BoxDensity1k = 4;

        /// <summary>
        /// Доля троп на поверхности.
        /// </summary>
        internal const float PathGroundRate = 0.03f; // 0.15f;

        /// <summary>
        /// Время (тиков) от проигрыша до завершения эпизода.
        /// </summary>
        internal const int LeftToFail = 6;

        /// <summary>
        /// Время (тиков) от выигрыша до завершения эпизода.
        /// </summary>
        internal const int LeftToWin = 1;

        /// <summary>
        /// Дистанция очистки тумана войны
        /// </summary>
        internal const int FogOfWarFreeDistance = FieldColumns / 2;
        
        // Плотность препятствий (1 по умолчанию).
        internal const int BarrierDensity = 1;

        /// <summary>
        /// Характеристики героя.
        /// </summary>
        internal const int HeroStartHealth = 1;
        internal const int HeroStartAmmo = 0;

        internal const int HeroBaseHealth = 20;
        internal const int HeroBaseAmmo = 30;
        // Показатели урона.
        internal const int HeroShotDamage = 5;
        internal const int HeroKnifeDamage = 3;
        // Базовый опыт для увелиячения уровня на 1 (при 62% приросте: 20-52-104-189... ).
        internal const int HeroBaseExperience = 20;
        // Здоровье анны
        internal const int AnnaHealth = 20;

        /// <summary>
        /// Вместимость ящиков с призами.
        /// </summary>
        internal const int TrophyHealthCapacity = 4;
        internal const int TrophyAmmoCapaciy = 5;

        // Опыт за убийство.
        internal const int CowExperience = -25;
        internal const int GuardianExperience = 5;
        internal const int ChaserExperience = 7;
        internal const int ShooterExperience = 12;
        internal const int SniperExperience = 15;
        internal const int BearExperience = 20;
        // Опыт за взятие приза.
        internal const int TrophyExperience = 1;
        // Опыт за нахождение Анны.
        internal const int AnnaFoundExperience = 25;

        /// <summary>
        /// Характеристики врагов.
        /// </summary>

        // Здоровье медведя.
        internal const int BearHealth = 10;
        // Вероятность преследования медведем (скорость преследования).
        internal const float BearChaseProbability = 0.40f;
        // Вероятность блуждания медведя.
        internal const float BearStrayProbability = 0.05f;
        // Вероятность удара когтями медведя.
        internal const float BearClawsProbability = 0.25f;
        // Дистанция преследования медведя.
        internal const float BearChaseDistance = 4;

        // Повреждение от взрыва бочки
        internal const int BarrelExplosiveDamage = 10;

        // Здоровье двери.
        internal const int DoorHealth = 10;
        // Здоровье преследователя.
        internal const int ChaserHealth = 5;
        // Здоровье стрелка.
        internal const int ShooterHealth = 7;

        // Повреждение от ножа врага.
        internal const int EnemyKnifeDamage = 3;
        // Повреждение от когтей врага.
        internal const int EnemyClawsDamage = 6;
        // Повреждение от выстрела врага.
        internal const int EnemyShotDamage = 5;

        // Вероятность удара ножом преследователем.
        internal const float EnemyKnifeProbability = 0.25f;
        // Вероятность преследования преследователем (скорость преследования).
        internal const float EnemyChaseProbability = 0.7f;
        // Дистанция преследования перследователя.
        internal const int EnemyChaseDistance = 6;
        // Вероятность выстрела стрелка.
        internal const float EnemyShotProbability = 0.5f;
        // Дистанция выстрела стрелка.
        internal const int EnemyShotDistance = 8;
        // Дистанция выстрела снайпера.
        internal const int SniperShotDistance = 32;
        
        // Задержка (тиков) выстрела врага.
        internal const int EnemyShotDelay = 7;
        // Задержка (тиков) удара ножом.
        internal const int EnemyKnifeDelay = 4;
        // Задержка (тиков) удара когтей.
        internal const int EnemyClawsDelay = 10;

        // Вероятность блуждания врага.
        internal const float EnemyStrayProbability = 0.1f;
        // Вероятность блуждания коровы.
        internal const float CowStrayProbability = 0.02f;
        // Вероятность поворота коровы.
        internal const float CowTurnProbability = 0.05f;
    }
}
