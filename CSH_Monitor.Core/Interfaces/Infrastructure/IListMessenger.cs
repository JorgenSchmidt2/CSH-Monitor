namespace CSH_Monitor.Core.Interfaces.Infrastructure
{
    public interface IListMessenger
    {
        /// <summary>
        /// Публикует коллекцию сообщений для всех подписанных обработчиков
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения</typeparam>
        /// <param name="messages">Коллекция сообщений</param>
        void Publish<T>(IEnumerable<T> messages);

        /// <summary>
        /// Регистрирует обработчик для сообщений указанного типа
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения для подписки</typeparam>
        /// <param name="handler">Обработчик сообщений</param>
        void Subscribe<T>(Action<IEnumerable<T>> handler);

        /// <summary>
        /// Отписывает обработчик от сообщений указанного типа
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения</typeparam>
        /// <param name="handler">Обработчик для отписки</param>
        void Unsubscribe<T>(Action<IEnumerable<T>> handler);

        /// <summary>
        /// Отписывает все обработчики для указанного объекта-получателя
        /// </summary>
        /// <param name="target">Объект, чьи обработчики нужно отписать</param>
        void UnsubscribeAllForTarget(object target);

        /// <summary>
        /// Очищает все подписки
        /// </summary>
        void Clear();

        /// <summary>
        /// Получает количество активных подписчиков для указанного типа (для отладки)
        /// </summary>
        int GetSubscriberCount<T>();
    }
}