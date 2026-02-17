namespace CSH_Monitor.Core.Interfaces.Infrastructure
{
    public interface IMessenger
    {
        /// <summary>
        /// Публикует сообщение для всех подписанных обработчиков
        /// </summary>
        void Publish<T>(T message);

        /// <summary>
        /// Регистрирует обработчик для сообщений указанного типа
        /// </summary>
        void Subscribe<T>(Action<T> handler);

        /// <summary>
        /// Отписывает обработчик от сообщений указанного типа
        /// </summary>
        void Unsubscribe<T>(Action<T> handler);

        /// <summary>
        /// Отписывает все обработчики для указанного объекта-получателя
        /// </summary>
        void UnsubscribeAllForTarget(object target);

        /// <summary>
        /// Очищает все подписки
        /// </summary>
        void Clear();
    }
}
