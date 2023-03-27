/*
Интерфейс "Повреждаемый от выстрела".
Обладатель этого интерфейса получает ущерб от выстрела.

2022-11-16
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Повреждаемый от выстрела.
    /// </summary>
    internal interface IDamageableOfShot : IDamageable
    {
    }
}
