/*
Интерфейс - взрывающийся.
При нулевом здоровье сущность взрывается, нанося вокруг урон взрывом.

2023-01-08 
*/

using Scge;

namespace TestGame
{
    /// <summary>
    /// Взрывающийся.
    /// </summary>
    internal interface IExploding
    {
        /// <summary>
        /// Единственная ячейка.
        /// </summary>
        internal Cell? Cell { get; set; }

        /// <summary>
        /// Взорвать.
        /// </summary>
        internal void BlowUp()
        {
            // Находим соседей и наносим им урон
            List<Cell> neighbors = Cell.GetNeighbors();
            foreach (Cell cell in neighbors)
            {
                if (!cell.IsEmpty && cell.Entity is IDamageableOfExplosion entity)
                {
                    entity.Damage(GameSeed.BarrelExplosiveDamage);
                }
            }
        }
    }
}
