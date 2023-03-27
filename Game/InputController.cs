/*
Контроллер ввода.
Экспериментальный класс.
Проверяет состояние клавиш, анализирует их комбинации и устанавливает управляющие флаги. 
Также может отслеживать короткие нажатия между тиками благодаря внешнему отслеживанию нажатий (ConsoleKeyPress), что существенно улучшает отклик при низкой частоте тиков.

2022-10-27
*/

using System.Runtime.InteropServices;

namespace TestGame
{
    /// <summary>
    /// Коды виртуальных клавиш.
    /// (Взято откуда-то из MSDN или подобного ресурса)
    /// </summary>
    enum VirtualKey : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        //
        VK_XBUTTON1 = 0x05,
        VK_XBUTTON2 = 0x06,
        //
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        //
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        //
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        //
        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
        VK_HANGUL = 0x15,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        //
        VK_ESCAPE = 0x1B,
        //
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        //
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,
        //
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        //
        VK_SLEEP = 0x5F,
        //
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        //
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        //
        VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
                                   //
        VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
        VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
        VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
        VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
        VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
                                 //
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        //
        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,
        //
        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,
        //
        VK_OEM_1 = 0xBA,   // ';:' for US
        VK_OEM_PLUS = 0xBB,   // '+' any country
        VK_OEM_COMMA = 0xBC,   // ',' any country
        VK_OEM_MINUS = 0xBD,   // '-' any country
        VK_OEM_PERIOD = 0xBE,   // '.' any country
        VK_OEM_2 = 0xBF,   // '/?' for US
        VK_OEM_3 = 0xC0,   // '`~' for US
                           //
        VK_OEM_4 = 0xDB,  //  '[{' for US
        VK_OEM_5 = 0xDC,  //  '\|' for US
        VK_OEM_6 = 0xDD,  //  ']}' for US
        VK_OEM_7 = 0xDE,  //  ''"' for US
        VK_OEM_8 = 0xDF,
        //
        VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
        VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
        VK_ICO_HELP = 0xE3,  //  Help key on ICO
        VK_ICO_00 = 0xE4,  //  00 key on ICO
                           //
        VK_PROCESSKEY = 0xE5,
        //
        VK_ICO_CLEAR = 0xE6,
        //
        VK_PACKET = 0xE7,
        //
        VK_OEM_RESET = 0xE9,
        VK_OEM_JUMP = 0xEA,
        VK_OEM_PA1 = 0xEB,
        VK_OEM_PA2 = 0xEC,
        VK_OEM_PA3 = 0xED,
        VK_OEM_WSCTRL = 0xEE,
        VK_OEM_CUSEL = 0xEF,
        VK_OEM_ATTN = 0xF0,
        VK_OEM_FINISH = 0xF1,
        VK_OEM_COPY = 0xF2,
        VK_OEM_AUTO = 0xF3,
        VK_OEM_ENLW = 0xF4,
        VK_OEM_BACKTAB = 0xF5,
        //
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }

    /// <summary>
    /// Состояние клавиши.
    /// </summary>
    internal class KeyState
    {
        /// <summary>
        /// Клнструктор.
        /// </summary>
        internal KeyState()
        {

        }

        /// <summary>
        /// Нажата в данный момент.
        /// </summary>
        internal bool Down { get; set; }

        /// <summary>
        /// Отпущена в данный момент.
        /// </summary>
        internal bool Up => !Down;

        /// <summary>
        /// Была нажата.
        /// </summary>
        internal bool Pressed { get; set; }

        /// <summary>
        /// Было воздействие (кнопка была нажата либо нажата сейчас).
        /// </summary>
        internal bool Impacted => Pressed || Down;

        /// <summary>
        /// Сброс состояния.
        /// (статус Pressed должен жить не более тика )
        /// </summary>
        internal void Reset()
        {
            Down = false;
            Pressed = false;
        }
    }

    /// <summary>
    /// Контроллер ввода
    /// </summary>
    internal class InputController
    {
        [DllImport("USER32.dll")]
        static extern short GetKeyState(VirtualKey nVirtKey);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        private const int KEY_PRESSED = 0x8000;


        /// <summary>
        /// Событие нажатия клавиши клавиатуры (вызывается откуда-то извне).
        /// </summary>
        internal void ConsoleKeyPress(ConsoleKeyInfo keyInfo)
        {
            // устанавливаем флаги перемещения для увеличения отзывчивости

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveLeft.Pressed = true;
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight.Pressed = true;
                    break;
                case ConsoleKey.UpArrow:
                    MoveUp.Pressed = true;
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown.Pressed = true;
                    break;
                case ConsoleKey.Spacebar:
                    Fire.Pressed = true;
                    break;
                case ConsoleKey.Escape:
                    Escape.Pressed = true;
                    break;
                case ConsoleKey.Enter:
                    Enter.Pressed = true;
                    break;
            }
        }

        /// <summary>
        /// Виртуальная клавиша нажата.
        /// </summary>
        private static bool VirtualKeyPressed(VirtualKey keyCode)
        {
            return Convert.ToBoolean(GetKeyState(keyCode) & KEY_PRESSED);
        }

        /// <summary>
        /// Виртуальная клавиша включена (порой работает некорректно).
        /// </summary>
        //private static bool VirtualKeyToggled(VirtualKey keyCode)
        //{
        //    return Convert.ToBoolean(GetKeyState(keyCode) & KEY_TOGGLED);
        //}

        #region Layers visible

        /// <summary>
        /// Переключить видимость слоя поверхности.
        /// </summary>
        internal bool SwitchLayer0Visible { get; private set; } = false;

        /// <summary>
        /// Переключить видимость  слой взаимодействия.
        /// </summary>
        internal bool SwitchLayer1Visible { get; private set; } = false;

        /// <summary>
        /// Переключить видимость  слой облаков
        /// </summary>
        internal bool SwitchLayer2Visible { get; private set; } = false;

        /// <summary>
        /// Переключить видимость  слой тумана войны
        /// </summary>
        internal bool SwitchLayer3Visible { get; private set; } = false;

        /// <summary>
        /// Переключить видимость  тестовый слой
        /// </summary>
        internal bool SwitchLayer4Visible { get; private set; } = false;
        #endregion

        #region Hero control
        /// <summary>
        /// Нажата любая клавиша перемещения.
        /// </summary>
        internal bool Move => MoveDown.Impacted || MoveLeft.Impacted || MoveRight.Impacted || MoveUp.Impacted;

        /// <summary>
        /// Переместиться влево.
        /// </summary>
        internal KeyState MoveLeft { get; private set; } = new();

        /// <summary>
        /// Переместиться влево.
        /// </summary>
        internal KeyState MoveRight { get; private set; } = new();

        /// <summary>
        /// Переместиться вниз.
        /// </summary>
        internal KeyState MoveDown { get; private set; } = new();

        /// <summary>
        /// Переместиться вверх.
        /// </summary>
        internal KeyState MoveUp { get; private set; } = new();

        /// <summary>
        /// Огонь.
        /// </summary>
        internal KeyState Fire { get; private set; } = new();

        /// <summary>
        /// Ввод.
        /// </summary>
        internal KeyState Enter { get; private set; } = new();

        /// <summary>
        /// Эскейп.
        /// </summary>
        internal KeyState Escape { get; private set; } = new();

        #endregion

        #region Renderer Control
        /// <summary>
        /// Переключение активности рендереров.
        /// </summary>
        internal bool SwitchRenderer0Active { get; private set; }
        internal bool SwitchRenderer1Active { get; private set; }
        internal bool SwitchRenderer2Active { get; private set; }
        
        /// <summary>
        /// Переключить режим отрисовки.
        /// </summary>
        internal bool SwitchRenderer0Mode { get; private set; }

        #endregion


        #region Other control
        
        /// <summary>
        /// Пауза движка
        /// </summary>
        internal bool EnginePause { get; private set; }

        /// <summary>
        /// Сброс движка
        /// </summary>
        internal bool EngineReset { get; private set; }

        /// <summary>
        /// Быстрая победа.
        /// </summary>
        internal bool QuickWin { get; private set; }

        /// <summary>
        /// Быстрое поражение.
        /// </summary>
        internal bool QuickFail { get; private set; }

        /// <summary>
        /// Быстрое восстановление.
        /// </summary>
        internal bool QuickRestore { get; private set; }

        /// <summary>
        /// Зажата кнопка альтернативного алгоритма
        /// </summary>
        internal bool HoldAlternative { get; private set; } = false;

        #endregion

        #region Interface kind
        
        /// <summary>
        /// Показать панель отладки.
        /// </summary>
        internal bool ShowDebug { get; private set; } = false;
        
        /// <summary>
        /// Показать панель производительности.
        /// </summary>
        internal bool ShowPerformance { get; private set; } = false;
        
        /// <summary>
        /// Показать панель справки.
        /// </summary>
        internal bool ShowHelp { get; private set; } = false;
        
        /// <summary>
        /// Показать панель геймплея.
        /// </summary>
        internal bool ShowGameplay { get; private set; } = false;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        internal InputController()
        {
        }

        internal void AfterTick()
        {
            // сбрасываем флаги перемещения 
            MoveRight.Reset();
            MoveLeft.Reset();
            MoveUp.Reset();
            MoveDown.Reset();
            Fire.Reset();

            Escape.Reset();
            Enter.Reset();
        }

        /// <summary>
        /// Выполнить тик движка.
        /// </summary>
        internal void DoTick()
        {
            // устанавливаем флаги перемещения 
            MoveRight.Down = VirtualKeyPressed(VirtualKey.VK_RIGHT);
            MoveLeft.Down = VirtualKeyPressed(VirtualKey.VK_LEFT);
            MoveUp.Down = VirtualKeyPressed(VirtualKey.VK_UP);
            MoveDown.Down = VirtualKeyPressed(VirtualKey.VK_DOWN);
            Fire.Down = VirtualKeyPressed(VirtualKey.VK_SPACE);

            Escape.Down = VirtualKeyPressed(VirtualKey.VK_ESCAPE);
            Enter.Down = VirtualKeyPressed(VirtualKey.VK_RETURN);

            // устанавливаем флаг сброса игры

            QuickRestore = VirtualKeyPressed(VirtualKey.VK_F6);
            QuickWin = VirtualKeyPressed(VirtualKey.VK_F7);
            QuickFail = VirtualKeyPressed(VirtualKey.VK_F8);

            EnginePause = VirtualKeyPressed(VirtualKey.VK_F9);
            EngineReset = VirtualKeyPressed(VirtualKey.VK_F10);
            HoldAlternative = VirtualKeyPressed(VirtualKey.VK_F12);

            // устанавливаем флаги смены видимости слоев
            SwitchLayer0Visible = VirtualKeyPressed(VirtualKey.VK_NUMPAD0);
            SwitchLayer1Visible = VirtualKeyPressed(VirtualKey.VK_NUMPAD1);
            SwitchLayer2Visible = VirtualKeyPressed(VirtualKey.VK_NUMPAD2);
            SwitchLayer3Visible = VirtualKeyPressed(VirtualKey.VK_NUMPAD3);
            SwitchLayer4Visible = VirtualKeyPressed(VirtualKey.VK_NUMPAD4);

            // устанавливаем флаги смены активности рендереров
            SwitchRenderer0Active = VirtualKeyPressed(VirtualKey.VK_NUMPAD9);
            SwitchRenderer1Active = VirtualKeyPressed(VirtualKey.VK_NUMPAD8);
            SwitchRenderer2Active = VirtualKeyPressed(VirtualKey.VK_NUMPAD7);

            // Переключить режим отрисовки поля
            SwitchRenderer0Mode = VirtualKeyPressed(VirtualKey.VK_END);

            // Переключить режим отрисовки интерфейса
            // справка
            if (VirtualKeyPressed(VirtualKey.VK_F1))
            {
                ShowHelp = true;
                ShowDebug = false;
                ShowGameplay = false;
                ShowPerformance = false;
            }
            // отладка
            if (VirtualKeyPressed(VirtualKey.VK_F2))
            {
                ShowDebug = true;
                ShowGameplay = false;
                ShowHelp = false;
                ShowPerformance = false;
            }
            // производительность
            if (VirtualKeyPressed(VirtualKey.VK_F3))
            {
                ShowPerformance = true;
                ShowDebug = false;
                ShowGameplay = false;
                ShowHelp = false;
            }
            // геймплей
            if (VirtualKeyPressed(VirtualKey.VK_F4))
            {
                ShowGameplay = true;
                ShowDebug = false;
                ShowHelp = false;
                ShowPerformance = false;
            }

            return;
        }
    }
}
