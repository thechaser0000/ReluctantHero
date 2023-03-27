/*
Рендерер. Компонент сцены.

Является абстрактным классом. Наследники должны реализовывать конкретные способы отрисовки.
Движок может содержать и обслуживать одновременно несколько рендереров.
Может быть неактивным и тогда не отрисовывается.
Один и тот же рендерер может отрисовывать разные сцены, для каждой сцены может быть свой вьюпорт
  
2022-10-09 
*/
namespace Scge
{
    /// <summary>
    /// Абстрактный класс рендерера
    /// </summary>
    internal abstract class Renderer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal Renderer()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Активен.
        /// </summary>
        internal bool IsEnabled { get; set; }

        /// <summary>
        /// Отрисовать указанную сцену с указанным вьюпортом.
        /// </summary>
        internal abstract void RenderScene(Scene scene, Viewport? viewport);
    }
}
