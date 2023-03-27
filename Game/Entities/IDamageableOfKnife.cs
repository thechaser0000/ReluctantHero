/*
Интерфейс "Повреждаемый от прикосновения".
Обладатель этого интерфейса получает ущерб от прикосновения.

2022-11-16
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Повреждаемый от ножа.
    /// </summary>
    internal interface IDamageableOfKnife : IDamageable
    {
    }
}
