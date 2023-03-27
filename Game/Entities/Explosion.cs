/*
Взрыв. 
Анимированная сущность.
Создается невидимым. Уничтожается после завершения взрыва.
Не содержит дополнительной логики.

2023-01-09 
*/


using PlatformConsole;
using Scge;

namespace TestGame
{
    /// <summary>
    /// Взрыв.
    /// </summary>
    internal class Explosion: Simple
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Explosion(Cell cell) : this(new List<Cell>() { cell }) { }
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Explosion(List<Cell>? cells = null) : base(cells)
        {
            IsVisible = false; 
        }

        /// <summary>
        /// Взорвать.
        /// </summary>
        internal void BlowUp()
        {
            IsExplode = true;
            IsVisible = true;
        }

        /// <summary>
        /// Взрывается.
        /// </summary>
        internal bool IsExplode { get; private set; }

        /// <summary>
        /// Выполнить проверки.
        /// </summary>
        internal override void DoCheck()
        {
            base.DoCheck();

            // Каждый тик меняем индекс спрайта.
            if (IsExplode && SpriteSelector.SelectedIndex < (SpriteSelector.Items.Count - 1))
            {
                SpriteSelector.SelectedIndex++;
            }
            else
            // уничтожаем взрыв.
            {
                IsExplode = false;
                IsVisible = false;
                NeedDelete = true;
            }
        }

        /// <summary>
        /// Процедура заполнения списка спрайтов. Простая неориентированная сущность.
        /// </summary>
        protected override void FillProcedure(List<ConsoleSprite> sprites)
        {
            // Связываем с нужным списком спрайтов.
            sprites.AddRange(Game.SpriteLibrary.ExplosionSpriteSet);
        }

    }
}
