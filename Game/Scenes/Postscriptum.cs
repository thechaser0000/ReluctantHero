/*
Постскриптум. 

* История разработки
* Об авторах
* Технические подробности
* EULA

2023-01-19 
*/

using Scge;
using PlatformConsole;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Security;
using TestGame;

namespace TestGame
{
    /// <summary>
    /// Содержимое страницы.
    /// </summary>
    internal class PostscrptumPageContent
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        internal string Title { get; set; }
        /// <summary>
        /// Параграф.
        /// </summary>
        internal List<string> Paragraphs { get; set; }
    }

    /// <summary>
    /// Страница постскриптума.
    /// </summary>
    internal class PostscriptumPage
    {
        private bool _isVisible;

        /// <summary>
        /// Видим.
        /// </summary>
        internal bool IsVisible
        { 
            get => _isVisible;
            set
            {
                _isVisible = value;
                Picture.IsVisible = value;
                Frame.IsVisible = value;
            }
        }

        internal string Title;

        /// <summary>
        /// Картинка.
        /// </summary>
        internal Picture Picture { get; set; }
        /// <summary>
        /// Текст.
        /// </summary>
        internal TextFrame Frame { get; set; }

        internal PostscriptumPage(ConsoleSprite sprite, PostscrptumPageContent content)
        {
            Picture = new(sprite)
            {
                Left = 16,
                Top = 1,
                IsVisible = false
            };
            
            Frame = new()
            {
                Rectangle = new(new Segment(Game.Accelerator.Rectangle.Left + 16, Game.Accelerator.Rectangle.Right - 16), new Segment(Picture.Rectangle.Bottom + 4, Game.Accelerator.Rectangle.Bottom)),
                BackgroundColor =ConsoleColor.Black,
                ForegroundColor =ConsoleColor.Cyan,
                IsVisible = false
            };

            Frame.Strings.AddRange(content.Paragraphs);

            Title = content.Title;
        }
    }
    
    
    /// <summary>
    /// Постскриптум.
    /// </summary>
    internal class Postscriptum : Scene
    {
        ///// <summary>
        ///// Фрейм истории.
        ///// </summary>
        //private TextFrame storyFrame;

        private int pageIndex;

        /// <summary>
        /// Заголовок страницы.
        /// </summary>
        private TextFrame PageTitle { get; set; }

        /// <summary>
        /// Перейти к следующей странице.
        /// </summary>
        private bool NextPage()
        {
            if (pageIndex < pages.Count - 1)
            {
                pages[pageIndex].IsVisible = false;
                pageIndex++;
                pages[pageIndex].IsVisible = true;
                PageTitle.AsText = pages[pageIndex].Title;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Перейти к предыдущей странице.
        /// </summary>
        private bool PreviousPage()
        {
            if (0 < pageIndex)
            {
                pages[pageIndex].IsVisible = false;
                pageIndex--;
                pages[pageIndex].IsVisible = true;
                PageTitle.AsText = pages[pageIndex].Title;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Перейти к первой странице.
        /// </summary>
        private void SetFirstPage()
        {
            pages[pageIndex].IsVisible = false;
            pageIndex = 0;
            pages[pageIndex].IsVisible = true;
            PageTitle.AsText = pages[pageIndex].Title;
        }

        /// <summary>
        /// История.
        /// </summary>
        private readonly PostscrptumPageContent story;

        /// <summary>
        /// Техника.
        /// </summary>
        private readonly PostscrptumPageContent technique;

        /// <summary>
        /// Автор.
        /// </summary>
        private readonly PostscrptumPageContent author;

        /// <summary>
        /// Права.
        /// </summary>
        private readonly PostscrptumPageContent rights;

        ///// <summary>
        ///// Страницы истории.
        ///// </summary>
        private List<PostscriptumPage> pages;

        /// <summary>
        /// Загрузить сцену. 
        /// (Создать и установить значения всех объектов)
        /// </summary>
        internal override void Load()
        {
            pages = new()
            {
                new (Game.SpriteLibrary.PostscriptumIcons[0], story),
                new (Game.SpriteLibrary.PostscriptumIcons[1], technique),
                new (Game.SpriteLibrary.PostscriptumIcons[2], author),
                new (Game.SpriteLibrary.PostscriptumIcons[3], rights),
            };

            foreach(var page in pages)
            {
                page.IsVisible = false;
                TextFrames.Add(page.Frame);
                Pictures.Add(page.Picture);
            };

            PageTitle = new()
            {
                Rectangle = new(new Segment(pages[0].Picture.Rectangle.Left, pages[0].Picture.Rectangle.Right), new Segment(pages[0].Picture.Rectangle.Bottom + 2, pages[0].Picture.Rectangle.Bottom + 2)),
                ForegroundColor = ConsoleColor.DarkYellow,
            };
            TextFrames.Add(PageTitle);

            SetFirstPage();

            State = SceneState.Ready;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Postscriptum() : base()
        {
            Name = "Postscriptum";

            // Рендерер текста.
            RenderersViewports.Add((Game.TextRenderer, null));
            // Рендерер интерфейса.
            RenderersViewports.Add((Game.InterfaceRenderer, null));

            // Инициализируем историю
            story = new()
            {
                Title = "История разработки",
                Paragraphs = 
                new()
                {
                    // 50 символов
                    new("Разработка началась с попытки сделать движок для простых игр,         "),
                    new("встраиваемых в серьезные приложения в виде пасхальных яиц. Движок     "),
                    new("должен был просто оперировать сущностями, выстроенными в виде         "),
                    new("прямоугольной сетки. Вся отрисовка должна была быть реализована в     "),
                    new("целевом приложении (WinForms, WPF, да что угодно).                    "),
                    new(""),
                    new("Для целей отладки был реализован текстовый вывод в консоль, возник    "),
                    new("персонаж, перемещающийся между ячейками, появились какие-то враги.    "),
                    new("Вырисовывающаяся картина показалась забавной и, ради веселья, мир игры"),
                    new("стал расширятся. Появились слои, появилась прокрутка мира, появилась  "),
                    new("мини-карта. Карта подтолкнула к идее отказаться от простого текстового"),
                    new("вывода и реализовать своеобразные спрайты. Появился герой, враги      "),
                    new("получили характеры, завязался сюжет игры. Было решено сделать два     "),
                    new("полноценных уровня с несколько отличающимися механиками. Герой обрел  "),
                    new("партнера, появилась потребность в прологе и эпилоге. Параллельно шла  "),
                    new("борьба с производительностью, поскольку стандартный вывод в консоль   "),
                    new("работал медленно, а использовать иные способы отрисовки не хотелось.  "),
                    new("Были и иные проблемы, но в каком проекте их не бывает?                "),
                    new(""),
                    new("Бурлили новые идеи, но приходилось себя сдерживать, чтобы не тратить  "),
                    new("слишком много времени на демонстрационный проект. В итоге получилось  "),
                    new("то, что получилось - простая, короткая, но полностью законченная игра."),
                }
            };

            // Инициализируем технические подробности
            technique = new()
            {
                Title = "Технические подробности",
                Paragraphs =
                new()
                {
                    new("Только факты:"),
                    new("* Для отрисовки текста и графики используется класс System.Console"),
                    new("* Для ускорения вывода в консоль перерисовываются только изменившиеся"),
                    new("в новом кадре части экрана (класс ConsoleAccelerator)"),
                    new("* Все ресурсы хранятся внутри исходного кода программы, загрузка"),
                    new("данных с диска не осуществляется"),
                    new("* Игровой мир обновляется 4 раза в секунду, что является компромиссом"),
                    new("между геймплеем и производительностью вывода на консоль"),
                    new("* Класс GameSeed позволяет настраивать параметры игры и баланс"),
                    new("* Расматривалось воспроизведение звуков на основе стороннего кода,"),
                    new("использовавшего winmm.dll, но было решено не усложнять проект"),
                    new("* Все персонажи перемещаются за такт ровно на 1 клетку, что "),
                    new("сказывается на плавности и невозможности увеличить фреймрейт"),
                    new("* Некоторые анимационные вставки реализованы непосредственно"),
                    new("на движке, в некоторых же просто двигаются спрайты"),
                    new("* В проекте около 23 000 строк, включая подробные комментарии"),
                    new("* Из них графика занимает более 6 000 строк (класс SpriteLibrary)"),
                    new("* Разрешение графики составляет 102 на 70 точек " + ConsoleAccelerator.BottomSquare + ConsoleAccelerator.TopSquare),
                    new("* Приложение не использует многопоточность"),
                    new("* Приложение написано в Visual Studio на .NET 6 / C# 10"),
                }
            };

            // Инициализируем информацию об авторе
            author = new()
            {
                Title = "О команде и техническом долге",
                Paragraphs =
                new()
                {
                    new("Разработка началась в ноябре 2022 силами одного человека. Сейчас, во  "),
                    new("время написания этих строк, заканчивается март 2023 года и вся команда"),
                    new("по-прежнему умещается на одном стуле."),
                    new(""),
                    new("В проекте не использовались никакие сторонние ресурсы, будь то код,   "),
                    new("графика или геймплейные решения. Впрочем, не исключено, что какие-то  "),
                    new("фрагменты кода всё же были взяты из интернета."),
                    new(""),
                    new("Были приложены серьёзные усилия к написанию аккуратного, доступного   "),
                    new("для понимания кода, оставлялись корректные комментарии, вовремя       "),
                    new("проводился рефакторинг и исправлялись замечания компилятора. Но в     "),
                    new("какой-то момент проект вышел за пределы первоначального замысла и     "),
                    new("главной целью стало довести его до конца, не погрязнув в мелочах. В   "),
                    new("итоге проект вряд ли можно назвать образцовым."),
                    new(""),
                    new("Исходные коды проекта с самого начала управлялись Subversion, поэтому "),
                    new("их целостность была под полным контролем. Готовый проект, приведенный "),
                    new("в порядок настолько, насколько хватило сил и желания, был опубликован "),
                    new("на Github уже в окончательном виде."),
                    new(""),
                    new("Ссылка на Github автора: https://github.com/thechaser0000"),
                }
            };

            // Инициализируем информацию о правах
            rights = new()
            {
                Title = "О правах",
                Paragraphs =
                new()
                {
                  //new("0123456789012345678901234567890123456789012345678901234567890123456789"),
                    new("Данное приложение является открытым и свободным программным           "),
                    new("обеспечением под лицензией MIT (https://opensource.org/license/mit/)  "),
                    new(""),
                    new("Данная лицензия разрешает лицам, получившим копию данного программного"),
                    new("обеспечения и сопутствующей документации (в дальнейшем именуемыми     "),
                    new("«Программное Обеспечение»), безвозмездно использовать Программное     "),
                    new("Обеспечение без ограничений, включая неограниченное право на          "),
                    new("использование, копирование, изменение, слияние, публикацию,           "),
                    new("распространение, сублицензирование и/или продажу копий Программного   "),
                    new("Обеспечения, а также лицам, которым предоставляется данное Программное"),
                    new("Обеспечение, при соблюдении следующих условий:                        "),
                    new(""),
                    new("Указанное выше уведомление об авторском праве и данные условия должны "),
                    new("быть включены во все копии или значимые части данного Программного    "),
                    new("Обеспечения."),
                    new(""),
                    new("Полный исходный код проекта размещен в репозитории GitHub по адресу"),
                    new("https://github.com/thechaser0000/ReluctantHero"),
                    new("Полный текст лицензии размещен в репозитории GitHub по адресу"),
                    new("https://github.com/thechaser0000/ReluctantHero/blob/main/LICENSE"),
                    new(""),
                    new("Copyright (c) 2023 B48"),
                }
            };
        }

        /// <summary>
        /// Сделать тик.
        /// </summary>
        internal override void DoTick()
        {
            if (999 < TickCount || Game.InputController.Escape.Impacted)
            // Переходим к следующей вкладке или выходим, если вкладок нет.
            {
                if (!NextPage())
                {
                    State = SceneState.Exit;
                }
            }

            if (Game.InputController.MoveRight.Impacted)
            // Переходим к следующей вкладке
            {
                NextPage();
            }

            if (Game.InputController.MoveLeft.Impacted)
            // Переходим к предыдующей вкладке
            {
                PreviousPage();
            }

            // Передаём управление сущностям.
            base.DoTick();

            PerformanceStatistics.EndLogic();

            // Отрисовываем.
            DoRendering();
            PerformanceStatistics.EndRendering();
        }

        /// <summary>
        /// Выполнить отрисовку.
        /// </summary>
        internal override void DoRendering()
        {
            // Пререндеринг слоёв - формирование итогового кадра.
            base.DoRendering();

            // Осуществить итоговый рендеринг кадра
            Game.Accelerator.Render(true);
            Game.Accelerator.Clear();
        }
    }

}

