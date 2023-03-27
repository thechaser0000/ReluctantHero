/*
Селектор спрайтов для сущности консоли.
Устанавливают текущий спрайт в зависимости от состояния сущности.
 
2022-11-24
*/


namespace PlatformConsole
{
    /// <summary>
    /// Селектор спрайтов для сущности консоли.
    /// </summary>
    internal class ConsoleSpriteSelector
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleSpriteSelector()
        {
            SelectedIndex = -1;
            Items = new();
        }

        /// <summary>
        /// Событие выбора текущего спрайта
        /// </summary>
        internal event Func<int, int> SpriteSelection;

        /// <summary>
        /// Процедура заполнения списка спрайтов. 
        /// </summary>
        internal delegate void FillProcedure(List<ConsoleSprite> spriteList);

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ConsoleSpriteSelector(FillProcedure fillProcedure, int selectedIndex = -1) : this()
        { 
            SelectedIndex = selectedIndex;

            // обратный вызов процедуры заполнения списка
            fillProcedure?.Invoke(Items);
        }

        /// <summary>
        /// Количество спрайтов в наборе.
        /// </summary>
        internal int Count => Items.Count;

        /// <summary>
        /// Выбранный спрайт.
        /// </summary>
        internal ConsoleSprite? Selected
        {
            get
            {
                if (SpriteSelection is not null)
                // Если есть событие определения выбранного спрайт, то вызываем его.
                {
                    SelectedIndex = SpriteSelection.Invoke(SelectedIndex);
                };

                // Возвращаем выбранный спрайт или ничего.
                return 0 <= SelectedIndex ? Items[SelectedIndex] : null;
            }
        }

        /// <summary>
        /// Индекс выбранного спрайта.
        /// </summary>
        internal int SelectedIndex { get; set; }

        /// <summary>
        /// Список спрайтов.
        /// </summary>
        internal List<ConsoleSprite> Items { get; init; }
    }

}
