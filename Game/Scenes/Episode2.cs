/*
Эпизод 2. 
Пещеры.

**

Уровень примерно квадратной формы со множеством пешер и тупиков. В пещерах попадаются обычные враги и враги с ружьями, в темных пещерах имеются медведи. Убийство врагов даёт здоровье и патроны. Анна находится в одной из пещер в глубине карты. 
Найдя её, герой должен пройти с ней к выходу. Анна автоматически следует за героем. Анна получает урон.

Условие победы:
* Герой находится у входа в пещеру
* Анна рядом с героем

Условие поражения:
* Исчерпание здоровья героя
* Исчерпание здоровья жены

2022-12-11 
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Эпизод 2.
    /// </summary>
    internal class Episode2 : Episode
    {
        /// <summary>
        /// Анна мертва.
        /// </summary>
        public bool AnnaIsDead => Anna.IsDead; 

        /// <summary>
        /// Слой взрывов.
        /// </summary>
        private Layer ExplosionsLayer { get; set; }

        /// <summary>
        /// Слой кровли.
        /// </summary>
        private Layer RoofLayer { get; set; }

        /// <summary>
        /// Поверхность земли (камень).
        /// </summary>
        internal RockGround RockGround { get; private set; }

        /// <summary>
        /// Камень (варианты текстуры).
        /// </summary>
        internal List<Rock> Rocks { get; private set; }

        /// <summary>
        /// Каменный перешеек.
        /// </summary>
        internal List<RockBridge> RockBridges { get; private set; }

        /// <summary>
        /// Поверхность земли (тропа на камне)
        /// (0 =,1 ||)
        /// </summary>
        internal List<RockPath> RockPaths { get; private set; }

        /// <summary>
        /// Точка выхода.
        /// </summary>
        internal Cell ExitPoint { get; private set; }

        /// <summary>
        /// Мост.
        /// (0 =,1 ||)
        /// </summary>
        internal List<WoodBridge> Bridges { get; private set; }

        /// <summary>
        /// Озера/реки.
        /// </summary>
        internal Lake Lake { get; private set; }

        /// <summary>
        /// Крепь.
        /// </summary>
        internal CaveSupport CaveSupport { get; private set; }

        /// <summary>
        /// Куча.
        /// </summary>
        internal Heap Heap { get; private set; }

        /// <summary>
        /// Костер.
        /// </summary>
        internal Bonfire Bonfire { get; private set; }

        /// <summary>
        /// Пропасти.
        /// </summary>
        internal Chasm Chasm { get; private set; }

        /// <summary>
        /// Анна.
        /// </summary>
        internal Anna Anna { get; private set; }

        /// <summary>
        /// Бочка-триггер.
        /// </summary>
        internal Barrel TriggeredBarrel { get; set; }

        /// <summary>
        /// Преследователи (создаются по триггеру).
        /// </summary>
        internal List<Cell> TriggeredChaserCells { get; set; }

        /// <summary>
        /// Точки спауна врагов (создаются по условию).
        /// </summary>
        internal List<Cell> ChaserSpawnCells { get; set; }

        /// <summary>
        /// Анна найдена.
        /// </summary>
        internal bool AnnaFound { get; private set; }

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            // Создаём слой взрывов под слоем тумана войны.
            ExplosionsLayer = new(this) { IsVisible = true, IsVisibleOnMap = false };
            // Создаём слой кровли над слоем взаимодействия.
            RoofLayer = new(this) { IsVisible = true, IsVisibleOnMap = false };

            base.Load();

            Layers.Insert(Layers.IndexOf(FogOfWarLayer) - 1, ExplosionsLayer);
            Layers.Insert(Layers.IndexOf(InteractionLayer) + 1, RoofLayer);

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Episode2() : base()
        {
            Name = "Episode2";
            Image = new Episode2Image();
            SetSize(Image.Rectangle);

            // Инициализируем историю
            story = new()
            {
                // 50 символов

                new("                     ЭПИЗОД 2                     "),
                new(""),
                new("    Вы  вошли  в  огромную пещеру.  Здесь  повсюду"),
                new("следы  присутствия  людей:  какие-то  ящики,  кучи"),
                new("мусора,  стальные  бочки.                         "),
                new("    Редкие  костерки  чуть  разгоняют  тьму,      "),
                new("скрывающую  глубокие  расщелины  и  многочисленные"),
                new("проходы. Ненадёжные потолки укреплены подпорками. "),
                new("    Совсем рядом раздаются тихие голоса о чем-то  "),
                new("беседующих  людей.  Наверняка  где-то  здесь  они "),
                new("прячут  вашу  жену.                               "),
                new(""),
                new(""),
                new(""),
                new("                       ЦЕЛИ                       "),
                new(""),
                new("1. Найти Анну.                                    "),
                new("2. Выбраться с ней из этого ада.                  "),
                new(""),
                new(""),
                new(""),
                new("    Нажмите Esc чтобы продолжить игру.            "),
            };

            Load();
        }

        protected internal override void LoadImage()
        {
            AnnaFound = false;
            // каменная поверхность
            RockGround = new();
            // тропа на каменной поверхности
            RockPaths = new() {
                new() { Direction = Direction.Right },
                new() { Direction = Direction.Up }};
            // мосты
            Bridges = new() {
                new() { Direction = Direction.Right },
                new() { Direction = Direction.Up }};
            // каменный перешеек
            RockBridges = new() {
                new() { Direction = Direction.Right },
                new() { Direction = Direction.Up }};
            // озера
            Lake = new();
            // пропасти
            Chasm = new();
            // крепь
            CaveSupport = new();
            // куча
            Heap = new();
            // Костер
            Bonfire = new();
            // Бочка-триггер

            // Камни
            Rocks = new() {
                new() {  },
                new() { Dark = true }};

            TriggeredChaserCells = new();
            ChaserSpawnCells = new();

            Entities.AddRange(Rocks);
            Entities.Add(RockGround);
            Entities.AddRange(RockPaths);
            Entities.Add(Lake);
            Entities.Add(Chasm);
            Entities.AddRange(Bridges);
            Entities.AddRange(RockBridges);
            Entities.Add(CaveSupport);
            Entities.Add(Heap);
            Entities.Add(Bonfire);

            // Обрабатываем образ.
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Width; columnIndex++)
                {
                    byte cellType = Image.Items[rowIndex, columnIndex];
                    switch (cellType)
                    {
                        // 10  тропа =
                        case 10:
                            RockPaths[0].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 11  тропа ||
                        case 11:
                            RockPaths[1].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 04  озеро
                        case 4:
                            Lake.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 04  пропасть
                        case 12:
                            Chasm.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 13  скалистый перешеек =
                        case 13:
                            RockBridges[0].Cells.Add(RoofLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 14  скалистый перешеек ||
                        case 14:
                            RockBridges[1].Cells.Add(RoofLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 20  скала
                        case 20:
                            Rocks[0].Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 21  темная скала
                        case 21:
                            Rocks[1].Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 24 мост =
                        case 24:
                            Bridges[0].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 25 мост ||
                        case 25:
                            Bridges[1].Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 28 дверь правая
                        case 28:
                            Door door = new(InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Direction = Direction.Right
                            };
                            door.Deleting += EntityDeleting;
                            //Doors.Add(door);
                            break;
                        // 30 Ящик.
                        case 30:
                            Box box = new(InteractionLayer.Columns[columnIndex][rowIndex]);
                            box.Deleting += EntityDeleting;
                            box.Color = SimpleColor.Yellow;
                            break;
                        // 31 Бочка (взрывается).
                        case 31:
                            Barrel barrel = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Color = SimpleColor.Red
                            };
                            barrel.Deleting += EntityDeleting;
                            break;
                        // 32 Бочка пустая (не взрывается).
                        case 32:
                            Barrel emptyBarrel = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Color = SimpleColor.Blue,
                                AllowBlowUp = false
                            };
                            break;
                        // 33 Крепь.
                        case 33:
                            CaveSupport.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 34 Куча.
                        case 34:
                            Heap.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 35 Костер.
                        case 35:
                            Bonfire.Cells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 36  скриптованный объект 1 (бочка рядом с мостом)
                        case 36:
                            TriggeredBarrel = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Color = SimpleColor.Red
                            };
                            TriggeredBarrel.Deleting += EntityDeleting;
                            break;

                        // 37 скриптованный объект 1 (вход/выход)
                        case 37:
                            Hero.Cell = InteractionLayer.Columns[columnIndex][rowIndex];
                            ExitPoint = InteractionLayer.Columns[columnIndex][rowIndex];
                            RockGround.Cells.Add(TerrainLayer.Columns[columnIndex][rowIndex]);
                            break;

                        // 38 скриптованный объект 1 (Triggered chaser)
                        case 38:
                            TriggeredChaserCells.Add(InteractionLayer.Columns[columnIndex][rowIndex]); 
                            // Запомним ячейки и создадим преследователей после нахождения Анны
                            break;

                        // 39 скриптованный объект 1 (LowAmmo triggered chaser )
                        case 39:
                            ChaserSpawnCells.Add(InteractionLayer.Columns[columnIndex][rowIndex]);
                            // Запомним ячейки и создадим преследователей после нахождения Анны
                            break;

                        // 2 дверь верхняя
                        case 29:
                            Door doorUp = new(InteractionLayer.Columns[columnIndex][rowIndex])
                            {
                                Direction = Direction.Up
                            };
                            doorUp.Deleting += EntityDeleting;
                            break;

                        // 60  Guardian
                        case 60:
                            Guardian guardian = new(InteractionLayer.Columns[columnIndex][rowIndex]) { RelatedLayer = TerrainLayer };
                            guardian.Hided += GenerateTrophy;
                            guardian.Deleting += EntityDeleting;
                            break;
                        // 61  Chaser
                        case 61:
                            Chaser chaser = new(InteractionLayer.Columns[columnIndex][rowIndex]) { Target = Hero, RelatedLayer = TerrainLayer };
                            chaser.Hided += GenerateTrophy;
                            chaser.Deleting += EntityDeleting;
                            break;
                        // 62  Shooter
                        case 62:
                            Shooter shooter = new(InteractionLayer.Columns[columnIndex][rowIndex]) { Target = Hero, RelatedLayer = TerrainLayer };
                            shooter.Hided += GenerateTrophy;
                            shooter.Deleting += EntityDeleting;
                            break;
                        // 64  Bear
                        case 64:
                            Bear bear = new(InteractionLayer.Columns[columnIndex][rowIndex]) { RelatedLayer = TerrainLayer, Target = Hero };
                            bear.Deleting += EntityDeleting;
                            break;
                        // 65  Анна
                        case 65:
                            Anna = new(InteractionLayer.Columns[columnIndex][rowIndex]) { RelatedLayer = TerrainLayer, Target = Hero };
                            Anna.Health.Maximum = GameSeed.AnnaHealth;
                            Anna.Health.Restore();
                            break;
                        // 66 Снайпер
                        case 66:
                            Sniper sniper = new(InteractionLayer.Columns[columnIndex][rowIndex]) { RelatedLayer = TerrainLayer, Target = Hero };
                            sniper.Hided += GenerateTrophy;
                            sniper.Deleting += EntityDeleting;
                            break;

                        // 73-75  тачки
                        case 73:
                            CarPart carPartRear = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex]) { Direction = Direction.Right, Position = CarPartPosition.Rear, BodyStyle = CarBodyStyle.Pickup };
                            carPartRear.Deleting += EntityDeleting;
                            break;
                        case 74:
                            CarPart carPartCenter = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex]) { Direction = Direction.Right, Position = CarPartPosition.Center, BodyStyle = CarBodyStyle.Pickup };
                            carPartCenter.Deleting += EntityDeleting;
                            break;
                        case 75:
                            CarPart carPartFront = new(ExplosionsLayer, InteractionLayer.Columns[columnIndex][rowIndex]) { Direction = Direction.Right, Position = CarPartPosition.Front, BodyStyle = CarBodyStyle.Pickup };
                            carPartFront.Deleting += EntityDeleting;
                            break;

                        // 90 аптечка
                        case 90:
                            new HealthTrophy(InteractionLayer.Columns[columnIndex][rowIndex]);
                            break;
                        // 91 Патроны
                        case 91:
                            new AmmoTrophy(InteractionLayer.Columns[columnIndex][rowIndex]);
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
            // REF ? взятие чекпоинтов вынести в героя (не так-то это просто: может в сами чекпойнты перенести, может добавить интерфейс)
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
            }

            // обрабатываем трофеи, соседние с героем
            foreach (Trophy box in trophies)
            {
                // он вроде бы и ни к чему, но для отладки пригодится
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

            // Если у альберта нет патронов (а по сюжету их нужно минимум 4 чтобы вскрыть две двери) - раз в минуту генерируем врага в одной из триггерных ячеек.
            if (Hero.ShotGun.Ammo.IsMinimum && IsNumberedTick(4 * 60))
            {
                Cell cell = ChaserSpawnCells[Randomizer.RandomBetwen(0, ChaserSpawnCells.Count - 1)];
                // если ячейка не занята случайным персонажем
                if (cell.IsEmpty)
                {
                    Chaser chaser = new(cell) { Target = Hero, RelatedLayer = TerrainLayer };
                    chaser.Hided += GenerateTrophy;
                    chaser.Deleting += EntityDeleting;
                }
            }

            // нашли Анну
            if (!AnnaFound)
            {
                AnnaFound = Cell.GetPathLength(Hero.Cell, Anna.Cell) < 3;

                if (AnnaFound)
                {
                    // Даём немного опыта
                    Hero.Experience.Increase(GameSeed.AnnaFoundExperience);

                    // Создаём преследователей 1 раз
                    foreach (Cell cell in TriggeredChaserCells)
                    {
                        // если ячейка не занята случайным персонажем
                        if (cell.IsEmpty)
                        {
                            Chaser chaser = new(cell) { Target = Hero, RelatedLayer = TerrainLayer };
                            chaser.Hided += GenerateTrophy;
                            chaser.Deleting += EntityDeleting;
                        }
                    }
                }
            }

            // Взрываем бочку, если Альберт рядом
            if (AnnaFound && TriggeredBarrel != null && !TriggeredBarrel.NeedDelete)
            {
                int albertsBarrelDistance = Cell.GetPathLength(Hero.Cell, TriggeredBarrel.Cell);

                if (albertsBarrelDistance <= 5)
                {
                    TriggeredBarrel.Health.Reset();
                }
            }

            // Проверяем условие поражения.
            // Здоровье на нуле или здоровье Анны на нуле.
            if ((Hero.Health.IsMinimum || Anna.Health.IsMinimum) && LeftToFail == -1)
            {
                LeftToFail = GameSeed.LeftToFail;
            }

            // Проверяем условие победы.
            // Стоим с Анной рядом с выходом.
            if (AnnaFound && LeftToWin == -1 && Anna != null && !Anna.IsDead)
            {
                int albertsExitDistance = Cell.GetPathLength(Hero.Cell, ExitPoint);
                int annaExitDistance = Cell.GetPathLength(Anna.Cell, ExitPoint);

                if (albertsExitDistance <= 3 && annaExitDistance <= 3)
                {
                    LeftToWin = GameSeed.LeftToWin;
                }
            }

            base.CheckEvents();
        }

        /// <summary>
        /// Сгенерировать поверхность.
        /// </summary>
        protected internal override void GenerateGround()
        {
            // все свободные ячейки слоя замощаем каменной поверхностью
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
                                RockPaths[0].Cells.Add(cell);
                            }
                            else
                            {
                                RockPaths[1].Cells.Add(cell);
                            }
                        }
                        else
                        {
                            RockGround.Cells.Add(cell);
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

            Hero.IsVisible = true;
        }

        /// <summary>
        ///  Обработка удаления сущностей из дополнительных списков.
        /// </summary>
        private void EntityDeleting(object sender, EntityEventArgs e)
        {
            if (State is SceneState.Running)
            {
                // если удаляется враг, удаляем его из этих списков
                // также, начисляем герою опыт (хотя, формально, нужно проверять, от чьей руки погиб враг)

                // Преследователь.
                if (e.Entity is Guardian guardian)
                {
                    Hero.Experience.Increase(GameSeed.GuardianExperience);
                    guardian.Deleting -= EntityDeleting;
                    return;
                }

                // Преследователь или страж.
                if (e.Entity is Chaser chaser)
                {
                    Hero.Experience.Increase(GameSeed.ChaserExperience);
                    chaser.Deleting -= EntityDeleting;
                    return;
                }

                // Стрелок или снайпер.
                if (e.Entity is Shooter shooter)
                {
                    Hero.Experience.Increase(GameSeed.ShooterExperience);
                    shooter.Deleting -= EntityDeleting;
                    return;
                }

                // Снайпер.
                if (e.Entity is Sniper sniper)
                {
                    Hero.Experience.Increase(GameSeed.SniperExperience);
                    sniper.Deleting -= EntityDeleting;
                    return;
                }

                // Медведь.
                if (e.Entity is Bear bear)
                {
                    Hero.Experience.Increase(GameSeed.BearExperience);
                    bear.Deleting -= EntityDeleting;
                    return;
                }

                // Часть автомобиля.
                if (e.Entity is CarPart carPart)
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    carPart.Deleting -= EntityDeleting;
                    return;
                }

                // Дверь.
                if (e.Entity is Door door)
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    door.Deleting -= EntityDeleting;
                    return;
                }

                // Ящик.
                if (e.Entity is Box box)
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    box.Deleting -= EntityDeleting;
                    return;
                }

                // Бочка.
                if (e.Entity is Barrel barrel)
                {
                    Hero.Experience.Increase(GameSeed.TrophyExperience);
                    barrel.Deleting -= EntityDeleting;
                    
                    // Бочка-триггер - разрушаем мост и стену.
                    if (e.Entity == TriggeredBarrel)
                    {
                        // ищем соседей бочки
                        List <Cell> neighbors = TriggeredBarrel.Cell.GetNeighbors();
                        for (int index = neighbors.Count - 1; 0 <= index; index--)
                        {
                            Entity? entity = neighbors[index].Entity;
                        
                            // проверяем наш слой (взаимодействия)
                            if (entity is not null)
                            {
                                // Скала.
                                if (entity is Rock rock)
                                {
                                    // Создаем поверхность скалы на слое территории.
                                    TerrainLayer.Cells[neighbors[index].Index].Entity = RockGround;
                                    // Удаляем сущность.
                                    neighbors[index].Entity = null;
                                }
                            }

                            // проверяем слой поверхности
                            Cell cell = TerrainLayer.Cells[neighbors[index].Index];
                            entity = cell.Entity;
                            // Мост
                            if (entity is not null && entity is WoodBridge bridge)
                            {
                                // Меняем мост на воду.
                                cell.Entity = Lake;
                            }
                        }

                        return;
                    }
                }
            }
        }
    }
}

