/*
Интерфейс "Повреждаемый от взрыва".
Обладатель этого интерфейса получает ущерб от выстрела.

2022-11-16
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Повреждаемый от взрыва.
    /// </summary>
    internal interface IDamageableOfExplosion : IDamageable
    {
    }
}
