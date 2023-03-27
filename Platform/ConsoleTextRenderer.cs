/*
Пререндерер текстовых фреймов.
 
2022-12-08
*/


using PlatformConsole;
using Scge;
using System.Reflection.PortableExecutable;

namespace PlatformConsole
{
    /// <summary>
    /// Пререндерер текстовых фреймов.
    /// </summary>
    internal class ConsoleTextRenderer : Renderer
    {

        /// <summary>
        /// Ускоритель вывода на консоль.
        /// </summary>
        private ConsoleAccelerator Accelerator { get; init; }

        internal ConsoleTextRenderer(ConsoleAccelerator accelerator)
        {
            Accelerator = accelerator;
        }

        /// <summary>
        /// Отрисовать один текстовый фрейм.
        /// </summary>
        private void RenderTextFrame(TextFrame frame)
        {
            // Не отрисовываем невидимый компонент.
            if (!frame.IsVisible)
            {
                return;
            }

            // в зависимости от выравнивания в двух направлениях выводим текст
            int textLines = frame.Strings.Count;

            if (frame.Rectangle.Height - frame.Margins.Top - frame.Margins.Bottom < textLines)
            // строк больше чем высота фрейма
            {
                throw new NotImplementedException();
            }

            // Определяем стартовую и финишную линии для вывода текста.
            int firstLine = frame.Margins.Top + frame.VerticalAlignment switch
            {
                VerticalAlignment.Top => frame.Rectangle.Top,
                VerticalAlignment.Bottom => frame.Rectangle.Top + frame.Rectangle.Height - frame.Margins.Bottom - frame.Margins.Top - textLines,
                VerticalAlignment.Center => frame.Rectangle.Top + (frame.Rectangle.Height - frame.Margins.Bottom - frame.Margins.Top - textLines) / 2,
            };
            int lastLine = firstLine + frame.Strings.Count - 1;
            int stringIndex = 0;

            Accelerator.SetColors(frame.BackgroundColor, frame.ForegroundColor);
            Accelerator.SetCursorPosition(frame.Rectangle.Left, frame.Rectangle.Top);

            string leftMargin = "".PadLeft(frame.Margins.Left, ' ');
            string rightMargin = "".PadLeft(frame.Margins.Right, ' ');

            for (int line = frame.Rectangle.Top; line <= frame.Rectangle.Bottom; line++)
            // Перебираем все строки фрейма.
            {
                Accelerator.SetCursorPosition(frame.Rectangle.Left, line);
                if (line < firstLine || lastLine < line)
                {
                    // Выводим пустые строки (до и после строк с текстом).
                    Accelerator.WriteEmpty(frame.Rectangle.Width);
                }
                else
                {
                    if (frame.Rectangle.Width - frame.Margins.Left - frame.Margins.Right < frame.Strings[stringIndex].Length)
                    // строки длинее чем ширина фрейма
                    {
                        throw new NotImplementedException();
                    }

                    // Выводим текст построчно с нужным выравниванием.
                    switch (frame.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            Accelerator.WritePadRight(leftMargin + frame.Strings[stringIndex++] + rightMargin, frame.Rectangle.Width);
                            break;
                        case HorizontalAlignment.Right:
                            Accelerator.WritePadLeft(leftMargin + frame.Strings[stringIndex++] + rightMargin, frame.Rectangle.Width);
                            break;
                        case HorizontalAlignment.Center:
                            Accelerator.WritePadBoth(leftMargin + frame.Strings[stringIndex++] + rightMargin, frame.Rectangle.Width);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовать один прогрессбар.
        /// </summary>
        private void RenderProgressBars(ProgressBar progressBar)
        {
            // Не отрисовываем невидимый компонент.
            if (!progressBar.IsVisible)
            {
                return;
            }

            Accelerator.SetCursorPosition(progressBar.Rectangle.Left, progressBar.Rectangle.Top);
            // Суть следующих строк такова: если показатель не нулевой, то значение занимает хотя бы один символ, а если показатель не максимален, то значение будет хотя бы на символ меньше полной длины полоски 
            int valueCount = progressBar.Characteristic.IsMinimum ? 0 : Math.Max(1, (int)Math.Round((progressBar.Characteristic.ValuePercent * progressBar.Width / 100f)));
            int spaceCount = progressBar.Characteristic.IsMaximum ? 0 : Math.Max(1, progressBar.Width - valueCount);
            valueCount = progressBar.Width - spaceCount;

            Accelerator.BackgroundColor = progressBar.ForegroundColor;
            Accelerator.Write("".PadLeft(valueCount, Accelerator.EmptyChar));
            Accelerator.BackgroundColor = progressBar.BackgroundColor;
            Accelerator.Write("".PadLeft(spaceCount, Accelerator.EmptyChar));
        }

        /// <summary>
        /// Отрисовать одну картинку.
        /// (с прозрачностью).
        /// </summary>
        private void RenderPicture(Picture picture)
        {
            // Не отрисовываем невидимый компонент.
            if (!picture.IsVisible)
            {
                return;
            }

            Accelerator.CopySpriteTo(picture.Sprite, picture.Left, picture.Top);
        }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        internal override void RenderScene(Scene scene, Viewport viewport)
        {
            // REF тут тож нужен общий список и слои.

            // Визуализируем текстовые фреймы.
            foreach (var frame in scene.TextFrames)
            {
                RenderTextFrame(frame);
            }

            // Визуализируем прогрессбары.
            foreach (var progressBar in scene.ProgressBars)
            {
                RenderProgressBars(progressBar);
            }

            // Визуализируем картинки.
            foreach (var picture in scene.Pictures)
            {
                RenderPicture(picture);
            }

            // Визуализируем текстовые фреймы переднего плана.
            foreach (var frame in scene.OverlayTextFrames)
            {
                RenderTextFrame(frame);
            }
        }
    }
}
