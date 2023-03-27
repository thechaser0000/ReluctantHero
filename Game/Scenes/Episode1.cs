/*
Эпизод 1. 
Дорога.

**

Вытянутый уровень с рекой, деревьями, скалами и проселочной дорогой между ними, размером несколько тысяч ячеек. Дорога в нескольких местах разветвляется и заводит в тупик. 
На дороге встречаются несколько бандитов, убийство которых даёт аптечки и патроны. Аптечки пополняют здоровье. Патроны просто накапливаются.
В конце концов герой добирается до входа в пещеру в скалах. Вход охраняет чувак с ружьём. Убив его герой получает огнестрельное оружие. 

Условие победы:
* Пистолет получен
* Герой берёт чекпойнт (вход в пещеру)
Условие поражения:
* Исчерпание здоровья героя
 
2022-12-11 
*/

using Scge;
using System.Data;

namespace TestGame
{
    /// <summary>
    /// Эпизод 1.
    /// </summary>
    internal class Episode1 : Episode
    {
        /// <summary>
        /// Поверхность земли (песок).
        /// </summary>
        internal SandGround SandGround { get; private set; }

        /// <summary>
        /// Поверхность земли (тропа на песке)
        /// (0 =,1 ||)
        /// </summary>
        internal List<SandPath> SandPaths { get; private set; }

        /// <summary>
        /// Деревья.
        /// </summary>
        internal Tree Tree { get; private set; }

        /// <summary>
        /// Страж.
        /// </summary>
        internal List<Guardian> Guardians { get; private set; }

        /// <summary>
        /// Преследователь.
        /// </summary>
        internal List<Chaser> Chasers { get; private set; }

        /// <summary>
        /// Стрелок.
        /// </summary>
        internal List<Shooter> Shooters { get; private set; }

        /// <summary>
        /// Машины.
        /// </summary>
        internal List<CarPart> CarParts  { get; private set; }

        /// <summary>
        /// Коровы.
        /// </summary>
        internal List<Cow> Cows { get; private set; }

        /// <summary>
        /// Заборы.
        /// </summary>
        internal List<WoodFence> WoodFence { get; private set; }

        /// <summary>
        /// Горы.
        /// </summary>
        internal Mountine Mountine { get; private set; }

        /// <summary>
        /// Вход в пещеру.
        /// </summary>
        internal CaveEntrance CaveEntrance { get; private set; }

        /// <summary>
        /// Двери.
        /// </summary>
        internal List<Door> Doors { get; private set; }

        /// <summary>
        /// Озера/реки.
        /// </summary>
        internal Lake Lake { get; private set; }

        /// <summary>
        /// Болота.
        /// </summary>
        internal Marsh Marsh { get; private set; }

        /// <summary>
        /// Броды.
        /// </summary>
        internal Ford Ford { get; private set; }

        /// <summary>
        /// Полосы шоссе.
        /// (0-1-2)
        /// </summary>
        internal List<HighwayLine> HighwayLines { get; private set; }

        /// <summary>
        /// Мост.
        /// (0 =,1 ||)
        /// </summary>
        internal List<WoodBridge> Bridges { get; private set; }

        /// <summary>
        ///  Обработка удаления сущностей из дополнительных списков.
        /// </summary>
        private void EntityDeleting(object sender, EntityEventArgs e)
        {
            if (State is SceneState.Running)
            {
                // если удаляется враг, удаляем его из этих списков
                // также, начисляем герою опыт (хотя, формально, нужно проверять, от чьей руки погиб враг)

                if (e.Entity is Guardian guardian)
                // преследователь или страж
                {
                    Hero.Experience.Increase(GameSeed.GuardianExperience);
                    guardian.Deleting -= EntityDeleting;
                    Guardians.Remove(guardian);
                    return;
                }

                if (e.Entity is Chaser chaser)
                // преследователь или страж
                {
                    Hero.Experience.Increase(GameSeed.ChaserExperience);
                    chaser.Deleting -= EntityDeleting;
                    Chasers.Remove(chaser);
                    return;
                }


                if (e.Entity is Shooter shooter)
                // стрелок
                {
                    Hero.Experience.Increase(GameSeed.ShooterExperience);
                    shooter.Deleting -= EntityDeleting;
                    Shooters.Remove(shooter);
                    return;
                }

                if (e.Entity is Cow cow)
                // корова
                {
                    Hero.Experience.Increase(GameSeed.CowExperience);
                    cow.Deleting -= EntityDeleting;
                    Cows.Remove(cow);
                    return;
                }

                if (e.Entity is Door door)
                // дверь
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    door.Deleting -= EntityDeleting;
                    Doors.Remove(door);
                    return;
                }

                if (e.Entity is Target target)
                // мишень
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    target.Deleting -= EntityDeleting;
                    return;
                }

                if (e.Entity is Flag flag)
                // мишень
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    flag.Deleting -= EntityDeleting;
                    return;
                }
            }
        }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            // Создаём сой облаков под слоем тумана войны.
            CloudsLayer = new(this) { IsVisible = true, IsVisibleOnMap = false };

            base.Load();

//            Layers.Insert(Layers.IndexOf(FogOfWarLayer) - 1, CloudsLayer);
            Layers.Insert(Layers.IndexOf(FogOfWarLayer), CloudsLayer);
            GenerateClouds();

            State = SceneState.Ready;
        }

        /// <summary>
        /// Загрузить образ.
        /// </summary>
        internal protected override void LoadImage()
        {
            // песчаная поверхность
            SandGround = new();
            // тропа на песчаной поверхности
            SandPaths = new() { 
                new() { Direction = Direction.Right },
                new() { Direction = Direction.Up }};
            // мосты
            Bridges = new() {
                new() { Direction = Direction.Right },
                new() { Direction = Direction.Up }};
            // деревья
            Tree = new();
            // озера
            Lake = new();
            // болота
            Marsh = new();
            // броды
            Ford = new();
            // горы
            Mountine = new();
            // заборы
            WoodFence = new()
            { 
                new() { Direction8 = Direction8.None },
                new() { Direction8 = Direction8.Left },
                new() { Direction8 = Direction8.Right },
                new() { Direction8 = Direction8.Up },
                new() { Direction8 = Direction8.Down },
                new() { Direction8 = Direction8.LeftUp },
                new() { Direction8 = Direction8.LeftDown },
                new() { Direction8 = Direction8.RightUp },
                new() { Direction8 = Direction8.RightDown }
            };
            // стражи
            Guardians = new();
            // преследователи
            Chasers = new();
            // стрелки
            Shooters = new();
            // коровы
            Cows = new();
            // части машин
            CarParts = new()
            {
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Rear, BodyStyle = CarBodyStyle.Sedan},
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Center, BodyStyle = CarBodyStyle.Sedan},
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Front, BodyStyle = CarBodyStyle.Sedan},
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Rear, BodyStyle = CarBodyStyle.Pickup},
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Center, BodyStyle = CarBodyStyle.Pickup},
                new(null!) { Direction = Direction.Right, Position = CarPartPosition.Front, BodyStyle = CarBodyStyle.Pickup}
            };
            // Вход в пещеру
            CaveEntrance = new() { Direction = Direction.Left };
            // шоссе
            HighwayLines = new()
            {
                new (){ Position = HighwayLinePosition.Left},
                new (){ Position = HighwayLinePosition.Center},
                new (){ Position = HighwayLinePosition.Right},
            };
            // Двери
            Doors = new();

            Entities.Add(SandGround);
            Entities.AddRange(SandPaths);
            Entities.Add(Tree);
            Entities.Add(Lake);
            Entities.Add(Mountine);
            Entities.AddRange(WoodFence);
            Entities.Add(CaveEntrance);
            Entities.Add(Marsh);
            Entities.Add(Ford);
            Entities.AddRange(HighwayLines);
            Entities.AddRange(Bridges);
            Entities.AddRange(CarParts);

            // Обрабатываем образ.
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Width; columnIndex++)
                {
                    byte cellType = Image.Items[rowIndex, columnIndex];
                    switch(cellType)
                    {
                    /*
                    0*  TerrainLayer
                    00  песок с вкраплениями дорожек
                    01  тропа =
                    02  тропа ||
                    03  болото
                    04  озеро
                    05  шоссе левая полоса
                    06  шоссе средняя полоса
                    07  шоссе правая полоса
                    08  брод

                    2*  InteractionLayer
                    23  дерево
                    24  мост =
                    25  мост ||
                    26  гора
                    27  вход в пещеру ->
                    28  дверь правая

                    4*  Забор
                    40  None = 0,       45 43 47
                    41  Left = 1,       41 40 42
                    42  Right = 2,      46 44 48
                    43  Up = 3,
                    44  Down = 4,
                    45  LeftUp = 5,
                    46  LeftDown = 6,
                    47  RightUp = 7,
                    48  RightDown = 8

                    6*  NPC
                    60  Guardian
                    61  Chaser
                    62  Shooter
                    63  Cow

                    7*  Автомобили
                    70  Седан, вправо, перед     70,71,72
                    71  Седан, вправо, центр     73,74,75
                    72  Седан, вправо, зад
                    73  Пикап, вправо, перед
                    74  Пикап, вправо, центр 
                    75  Пикап, вправо, зад

                    8 

                    9*  Трофеи
                    90  Аптечка
                    91  Патроны
                    92  Огнестрел
                    */

                        // 01  тропа =
                        case 1:
                            SandPaths[0].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 02  тропа ||
                        case 2:
                            SandPaths[1].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 03  болото
                        case 3:
                            Marsh.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 04  озеро
                        case 4:
                            Lake.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 05  шоссе левая полоса
                        case 5:
                            HighwayLines[0].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 06  шоссе средняя полоса
                        case 6:
                            HighwayLines[1].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 07  шоссе правая полоса
                        case 7:
                            HighwayLines[2].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 08  брод
                        case 8:
                            Ford.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;

                        // 23  дерево
                        case 23:
                            Tree.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 24 мост =
                        case 24:
                            Bridges[0].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 25 мост ||
                        case 25:
                            Bridges[1].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 26  гора
                        case 26:
                            Mountine.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 27 вход в пещеру ->
                        case 27:
                            CaveEntrance.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 28 дверь правая
                        case 28:
                            Door door = new(InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Direction = Direction.Right
                            };
                            door.Deleting += EntityDeleting;
                            Doors.Add(door);
                            break;

                        // 40-48  забор
                        case >= 40 and <= 48:
                            WoodFence[cellType-40].Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;

                        // 49 мишень
                        case 49:
                            Target target = new(InteractionLayer.Columns[columnIndex][rowIndex]);
                            target.Deleting += EntityDeleting;
                            break;

                        // 50 флаг
                        case 50:
                            Flag flag = new(InteractionLayer.Columns[columnIndex][rowIndex]);
                            flag.Deleting += EntityDeleting;
                            break;

                        // 60  Guardian
                        case 60:
                            Guardian guardian = new(InteractionLayer.Columns[columnIndex][rowIndex]) { RelatedLayer = TerrainLayer};
                            guardian.Hided += GenerateTrophy;
                            guardian.Deleting += EntityDeleting;
                            Guardians.Add(guardian);
                            break;
                        // 61  Chaser
                        case 61:
                            Chaser chaser = new(InteractionLayer.Columns[columnIndex][rowIndex]) { Target = Hero, RelatedLayer = TerrainLayer };
                            chaser.Hided += GenerateTrophy;
                            chaser.Deleting += EntityDeleting;
                            Chasers.Add(chaser);
                            break;
                        // 62  Shooter
                        case 62:
                            Shooter shooter = new(InteractionLayer.Columns[columnIndex][rowIndex]) { Target = Hero, RelatedLayer = TerrainLayer };
                            shooter.Hided += GenerateTrophy;
                            shooter.Deleting += EntityDeleting;
                            Shooters.Add(shooter);
                            break;
                        // 63  Cow
                        case 63:
                            Cow cow = new(InteractionLayer.Columns[columnIndex][rowIndex]) { Direction = Direction.Left, RelatedLayer = TerrainLayer };
                            cow.Hided += GenerateTrophy;
                            cow.Deleting += EntityDeleting;
                            Cows.Add(cow);
                            break;
                        // 70-75  забор
                        case >= 70 and <= 75:
                            CarParts[cellType - 70].Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 90 аптечка
                        case 90:
                            new HealthTrophy(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 91 Патроны
                        case 91:
                            new AmmoTrophy(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 92 Огнестрел
                        case 92:
                            new ShotGunTrophy(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяем события в игре
        /// </summary>
        internal protected override void CheckEvents()
        // REF можно немножко вынести 
        {
            // ищем все чекпойнты среди соседей героя
            List<Cell> neighbors = Hero.Cell!.GetNeighbors();
            // REF 2 тоже нужно как-то рефакторить
            List<Trophy> trophies = new();
            // Выход соседствует с игроком.
            bool exitIsNeighbour = false;

            // обрабатываем список сущностей, соседних с героем
            foreach (Cell neighbor in neighbors)
            {
                if (neighbor.Entity is Trophy trophy)
                // трофей поблизости
                {
                    trophies.Add(trophy);
                }

                if (neighbor.Entity is CaveEntrance)
                // вход в пещеру поблизости
                {
                    exitIsNeighbour = true;
                }
            }

            // обрабатываем трофеи, соседние с героем
            foreach (Trophy box in trophies)
            {
                if (box is ShotGunTrophy shotGunTrophy)
                // Трофей - огнестрел.
                {
                    shotGunTrophy.Clear();
                    // Добавляем огнестрел.
                    Hero.ShotGun.IsEnabled = true;
                }

                if (box is AmmoTrophy ammoTrophy)
                // Трофей - патроны.
                {
                    ammoTrophy.Clear();
                    // Добавляем немного патронов.
                    Hero.ShotGun.Ammo.Increase(ammoTrophy.Capacity);
                }

                if (box is HealthTrophy healthTrophy)
                // Трофей - аптечка.
                {
                    healthTrophy.Clear();
                    // Добавляем немного здоровья.
                    Hero.Health.Increase(healthTrophy.Capacity);
                }

                Hero.Experience.Increase(GameSeed.TrophyExperience);
            }

            // Проверяем условие поражения.
            // Здоровье на нуле.
            if (Hero.Health.IsMinimum && LeftToFail == -1)
            {
                LeftToFail = GameSeed.LeftToFail;
            }

            // Проверяем условие победы.
            // Стоим рядом с выходом, есть огнестрел.
            if (exitIsNeighbour && Hero.ShotGun.IsEnabled && LeftToWin == -1)
            {
                LeftToWin = GameSeed.LeftToWin;
            }

            base.CheckEvents();
        }

        /// <summary>
        /// Сгенерировать поверхность.
        /// </summary>
        internal protected override void GenerateGround()
        {
            // все свободные ячейки слоя замощаем песком
            for (int columnIndex = 0; columnIndex < TerrainLayer.Scene.Width; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < TerrainLayer.Scene.Height; rowIndex++)
                {
                    Cell cell = TerrainLayer.Columns[columnIndex][rowIndex];
                    if (cell.IsEmpty)
                    {
                        if (Randomizer.ItIsTrue(GameSeed.PathGroundRate))
                        {
                            if (Randomizer.ItIsTrue(0.5))
                            {
                                SandPaths[0].Cells.Add(cell);
                            }
                            else
                            {
                                SandPaths[1].Cells.Add(cell);
                            }
                        }
                        else
                        {
                            SandGround.Cells.Add(cell);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Генерировать героя.
        /// </summary>
        internal protected override void GenerateHero()
        {
            base.GenerateHero(); 

            // Герой.
            Hero.Health.Value = GameSeed.HeroStartHealth;
            Hero.Health.Maximum = GameSeed.HeroBaseHealth;
            Hero.ShotGun.Ammo.Value = GameSeed.HeroStartAmmo;
            Hero.ShotGun.Ammo.Maximum = GameSeed.HeroBaseAmmo;
            Hero.ShotGun.IsEnabled = false;
            Hero.Knife.IsEnabled = true;
            Hero.IsVisible = true;
            Hero.EvolutionLevel = 0;
            Hero.Experience.Reset();
            Hero.Experience.Maximum = GameSeed.HeroBaseExperience;
            Hero.IsDead = false;

            // Нормальный старт
            Hero.Cell = InteractionLayer.Columns[5][^13];
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Episode1() : base()
        {
            Name = "Episode1";
            Image = new Episode1Image();
            SetSize(Image.Rectangle);

            // Инициализируем историю
            story = new()
            {
                // 50 символов

                new("                     ЭПИЗОД 1                     "),
                new(""),
                new("    Вы очнулись в кустах на краю дороги. Ваша     "),
                new("голова разбита - нужно как-то остановить кровь.   "),
                new("Документы и телефон забрали, но в глубине кармана "),
                new("лежит  складной  нож  -  хоть  какое-то  оружие!  "),
                new(""),
                new("    Вокруг - никого. Лишь едва заметная каменистая"),
                new("дорога убегает в холмы. Где жена, где машина, и   "),
                new("что  же  здесь  происходит?                       "),
                new(""),
                new(""),
                new(""),
                new("                       ЦЕЛИ                       "),
                new(""),
                new("1. Выяснить, куда ведет дорога.                   "),
                new("2. Раздобыть оружие посерьёзнее.                  "),
                new(""),
                new(""),
                new(""),
                new("    Нажмите Esc чтобы наконец-то начать игру.     "),
            };

            Load();
        }

        /// <summary>
        /// Слой облаков.
        /// </summary>
        private Layer CloudsLayer { get; set; }

        /// <summary>
        /// Облака
        /// </summary>
        internal Cloud Clouds { get; private set; }

        /// <summary>
        /// Сгенерировать облака.
        /// </summary>
        private void GenerateClouds()
        {
            IEnumerable<Cell> sorted = StructureGenerator.GetBlurs(CloudsLayer, 3, 8, 3, 4, 15).OrderBy(cell => cell.Index);
            Clouds = new(sorted.ToList());
        }

        /// <summary>
        /// Очистить сцену.
        /// </summary>
        internal override void Clear()
        {
            Clouds.Clear();

            base.Clear();
        }
    }
}
    
