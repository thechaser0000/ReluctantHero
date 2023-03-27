/*

Индикатор (HUD)
(Для эпизода 1 и эпизода 2)

2022-12-23
*/

using Scge;
using PlatformConsole;

namespace TestGame
{
    /// <summary>
    /// Индикатор.
    /// </summary>
    internal class Hud
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
                if (_isVisible != value)
                {
                    _isVisible = value;

                    // текстовые фреймы
                    foreach (var frame in GetFrames())
                    {
                        frame.IsVisible = IsVisible;
                    }

                    // прогрессбары
                    foreach (var progressBar in GetProgressBars())
                    {
                        progressBar.IsVisible = IsVisible;
                    }

                    // картинки
                    foreach (var pictures in GetPictures())
                    {
                        pictures.IsVisible = IsVisible;
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Hud(Rectangle rectangle, ConsoleColor backgroundColor, ConsoleColor inactiveColor, ConsoleColor healthColor, ConsoleColor ammoColor, ConsoleColor experienceColor, ConsoleColor annaColor)
        {
            pictures = new();
            frames = new();

            Rectangle = rectangle;
            int column1 = Rectangle.Left;
            int column2 = Rectangle.Left + 6 + 2;
            int column3 = Rectangle.Left + 6 + 2 + 6 + 2;
            int column4 = Rectangle.Left + 6 + 2 + 6 + 2 + 6 + 2;

            // 4 картинки вертикально
            // Здоровье.
            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column1,
                Top = Rectangle.Top
            });

            pictures.Add(new(Game.SpriteLibrary.TrophySpriteSet[3])
            {
                Left = column1,
                Top = Rectangle.Top
            });

            healthLabel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                BackgroundColor = ConsoleColor.Black,
                ForegroundColor = healthColor,
                Rectangle = new(column2, Rectangle.Top, Rectangle.Right, Rectangle.Top)
            };

            healthProgressBar = new()
            {
                BackgroundColor = backgroundColor,
                ForegroundColor = healthColor,
                Top = Rectangle.Top + 2,
                Horizontal = new(column2, Rectangle.Right)
            };

            // Патроны.
            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column1,
                Top = rectangle.Top + 4
            });

            pictures.Add(new(Game.SpriteLibrary.TrophySpriteSet[4])
            {
                Left = column1,
                Top = rectangle.Top + 4
            });

            ammoLabel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                BackgroundColor = ConsoleColor.Black,
                ForegroundColor = ammoColor,
                Rectangle = new(column2, Rectangle.Top + 4, Rectangle.Right, Rectangle.Top + 4)
            };

            ammoProgressBar = new()
            {
                BackgroundColor = backgroundColor,
                ForegroundColor = ammoColor,
                Top = ammoLabel.Rectangle.Top + 2,
                Horizontal = new(column2, Rectangle.Right)
            };

            // Опыт
            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column1,
                Top = rectangle.Top + 8
            });

            pictures.Add(new(Game.SpriteLibrary.TrophySpriteSet[6])
            {
                Left = column1,
                Top = rectangle.Top + 8
            });

            experienceLabel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                BackgroundColor = ConsoleColor.Black,
                ForegroundColor = experienceColor,
                AsText = "Опыт",
                Rectangle = new(column2, Rectangle.Top + 8, Rectangle.Right, Rectangle.Top + 8)
            };

            experienceProgressBar = new()
            {
                BackgroundColor = backgroundColor,
                ForegroundColor = experienceColor,
                Top = experienceLabel.Rectangle.Top + 2,
                Horizontal = new(column2, Rectangle.Right)
            };

            // Оружие
            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column1,
                Top = rectangle.Top + 12
            });

            pictures.Add(new(Game.SpriteLibrary.TrophySpriteSet[7])
            {
                Left = column1,
                Top = rectangle.Top + 12
            });

            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column2,
                Top = rectangle.Top + 12
            });

            pictures.Add(new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column3,
                Top = rectangle.Top + 12
            });

            annaDummy = new(Game.SpriteLibrary.GuiSpriteSet[0])
            {
                Left = column4,
                Top = rectangle.Top + 12
            };
            pictures.Add(annaDummy);

            knifePicture = new(Game.SpriteLibrary.TrophySpriteSet[5])
            {
                Left = column2,
                Top = Rectangle.Top + 12,
            };

            shotGunPicture = new(Game.SpriteLibrary.TrophySpriteSet[2])
            {
                Left = column3,
                Top = Rectangle.Top + 12,
            };

            // Врезка со здоровьем Анны
            annaHealthLabel = new() {
                HorizontalAlignment = HorizontalAlignment.Center,
                BackgroundColor = ConsoleColor.Black,
                ForegroundColor = annaColor,
                AsText = "Анна",
                IsVisible = true, 
                Rectangle = new(new(column4, Rectangle.Top + 12), 6, 1)
            };

            annaHealthProgressBar = new()
            {
                BackgroundColor = backgroundColor,
                ForegroundColor = annaColor,
                IsVisible = true,
                Top = Rectangle.Top + 12 + 2,
                Horizontal = new(column4, Rectangle.Right)
            };

            _isVisible = true;
        }

        /// <summary>
        /// Прямоугольник индикатора.
        /// </summary>
        internal Rectangle Rectangle { get; set; }

        /// <summary>
        /// Здоровье (подпись).
        /// </summary>
        private TextFrame healthLabel;

        /// <summary>
        /// Заглушка, закрывающая параметры Анны.
        /// </summary>
        private Picture annaDummy;

        /// <summary>
        /// Патроны (подпись).
        /// </summary>
        private TextFrame ammoLabel;

        /// <summary>
        /// Опыт (подпись).
        /// </summary>
        private TextFrame experienceLabel;

        /// <summary>
        /// Здоровье Анны (подпись).
        /// </summary>
        private TextFrame annaHealthLabel;

        /// <summary>
        /// Здоровье (прогрессбар).
        /// </summary>
        private ProgressBar healthProgressBar;

        /// <summary>
        /// Патроны (прогрессбар).
        /// </summary>
        private ProgressBar ammoProgressBar;

        /// <summary>
        /// Опыт (прогрессбар).
        /// </summary>
        private ProgressBar experienceProgressBar;

        /// <summary>
        /// Здоровье Анны (прогрессбар).
        /// </summary>
        private ProgressBar annaHealthProgressBar;

        /// <summary>
        /// Огнестрел (картинка).
        /// </summary>
        private Picture shotGunPicture;

        /// <summary>
        /// Нож (картинка).
        /// </summary>
        private Picture knifePicture;

        /// <summary>
        /// Прочие картинки.
        /// </summary>
        private List<Picture> pictures;

        /// <summary>
        /// Прочие фреймы.
        /// </summary>
        private List<TextFrame> frames;

        /// <summary>
        /// Обновить.
        /// </summary>
        internal void Update()
        {
            healthLabel.AsText = string.Format("{0} {1}/{2}", "Здоровье", Game.Hero.Health.Value, Game.Hero.Health.Maximum);
            ammoLabel.AsText = string.Format("{0} {1}/{2}", "Патроны", Game.Hero.ShotGun.Ammo.Value, Game.Hero.ShotGun.Ammo.Maximum);
            experienceLabel.AsText = string.Format("{3} {4}  {0} {1}/{2}", "Опыт", Game.Hero.Experience.Value, Game.Hero.Experience.Maximum, "Ур.", Game.Hero.EvolutionLevel);
            healthProgressBar.Characteristic = Game.Hero.Health;
            ammoProgressBar.Characteristic = Game.Hero.ShotGun.Ammo;
            experienceProgressBar.Characteristic = Game.Hero.Experience;

            if (Engine.ActiveScene is Episode2 && Game.Episode2.Anna != null)
            {
                annaHealthProgressBar.Characteristic = Game.Episode2.Anna.Health;
            }

            annaDummy.IsVisible = IsVisible && !(Engine.ActiveScene is Episode2 && Game.Episode2.AnnaFound);

            knifePicture.IsVisible = IsVisible && Game.Hero.Knife.IsEnabled;
            shotGunPicture.IsVisible = IsVisible && Game.Hero.ShotGun.IsEnabled;
        }

        /// <summary>
        /// Возвразщает все используемые текстовые фреймы.
        /// </summary>
        internal List<TextFrame> GetFrames()
        {
            List<TextFrame> result = new()
            {
                healthLabel,
                ammoLabel,
                experienceLabel,
                annaHealthLabel
            };

            result.AddRange(frames);

            return result;
        }

        /// <summary>
        /// Возвразщает все используемые картинки.
        /// </summary>
        internal List<Picture> GetPictures()
        {
            List<Picture> result = new();

            result.AddRange(pictures);
            result.Add(knifePicture);
            result.Add(shotGunPicture);

            return result;
        }

        /// <summary>
        /// Возвразщает все используемые прогрессбары.
        /// </summary>
        internal List<ProgressBar> GetProgressBars()
        {
            List<ProgressBar> result = new()
            {
                healthProgressBar,
                ammoProgressBar,
                experienceProgressBar,
                annaHealthProgressBar
            };

            return result;
        }
    }
}

