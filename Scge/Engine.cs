/*
Статический класс игрового движка.
Не содержит практически ничего интересного, кроме примитивного управления сценами. 
 
2022-10
*/

using static System.Formats.Asn1.AsnWriter;

namespace Scge
{

    /// <summary>
    /// Состояния движка.
    /// </summary>
    internal enum EngineState
    {
        /// <summary>
        /// Подготовка к запуску.
        /// </summary>
        Preparing = 0,
        /// <summary>
        /// Запущен (обрабатывает тики).
        /// </summary>
        Running = 1,
        /// <summary>
        /// Приостановлен (пропускает тики).
        /// </summary>
        Paused = 2,
    }

    /// <summary>
    /// Движок.
    /// </summary>
    internal static class Engine
    {
        internal static EngineState State { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        static Engine()
        {
            State = EngineState.Preparing;
            Scenes = new();
            _activeSceneIndex = -1;
        }

        /// <summary>
        /// Запустить движок или снять с паузы.
        /// </summary>
        internal static void Start()
        {
            State = EngineState.Running;
        }

        /// <summary>
        /// Поставить движок на паузу.
        /// </summary>
        internal static void Pause()
        {
            State= EngineState.Paused;
        }

        /// <summary>
        /// Сцены.
        /// </summary>
        internal static List<Scene> Scenes { get; set; }

        /// <summary>
        /// Индекс активной сцены.
        /// </summary>
        private static int _activeSceneIndex;

        /// <summary>
        /// Индекс активной сцены.
        /// </summary>
        internal static int ActiveSceneIndex
        { 
            get => _activeSceneIndex;
            set
            {
                if (value != ActiveSceneIndex)
                {
                    // Приостанавливаем текущую сцену.
                    if (0 <= ActiveSceneIndex)
                    {
                        Scenes[ActiveSceneIndex].Suspend();
                    }

                    // Меняем сцену на активную, если она готова.
                    Scenes[value].StartOrResume();
                    _activeSceneIndex = value;
                }
            }
        }

        /// <summary>
        /// Перейти к целевой сцене через сцену загрузки.
        /// </summary>
        internal static void GoToScene(Scene waitScene, Scene targetScene)
        {
            ActiveScene = waitScene;
            GoToScene(targetScene, true);
        }

        /// <summary>
        /// Перейти к сцене.
        /// </summary>
        internal static void GoToScene(Scene scene, bool reload = false)
        {
            if (reload)
            // Перезагружаем сцену при необходимости.
            {
                scene.Clear();
                scene.Load();
            }

            ActiveScene = scene;
        }

        /// <summary>
        /// Активная сцена
        /// </summary>
        internal static Scene? ActiveScene
        {
            get
            {
                return 0 <= ActiveSceneIndex ? Scenes[ActiveSceneIndex] : null;
            }

            private set
            {
                ActiveSceneIndex = Scenes.IndexOf(value);
            }
        }
        
        /// <summary>
        /// Счетчик тиков.
        /// </summary>
        internal static int TickCount {get; private set; }

        /// <summary>
        /// Выполнить тик движка.
        /// </summary>
        internal static void DoTick()
        {
            if (State is EngineState.Running)
            {
                // обрабатываем все объекты активной сцены и вызываем рендереры (неактивные сцены засыпают)
                if (ActiveScene.State is SceneState.Running)
                {
                    ActiveScene.BeforeTick();
                    ActiveScene.DoTick();
                }
                
                TickCount++;
            }
        }

        /// <summary>
        /// Является ли тик N-м по порядку (каждым вторым, каждым 25-м и т.п.).
        /// </summary>
        internal static bool IsNumberedTick(int number) => 0 == TickCount % number;

        /// <summary>
        /// Необходимо выйти из программы.
        /// </summary>
        internal static bool NeedExit { get; set; } = false;

        /// <summary>
        /// Сброс движка
        /// </summary>
        internal static void Reset()
        {
            State= EngineState.Preparing;

            foreach (Scene scene in Scenes)
            {
                scene.Clear();
            }

            Scenes.Clear();
            _activeSceneIndex = -1;
            TickCount = 0;
        }
    }
}
