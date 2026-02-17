using CSH_Monitor.Core.Interfaces.Infrastructure;

namespace CSH_Monitor.Infrastructure.Messaging
{
    public class ListMessenger : IListMessenger
    {
        /// <summary>
        /// Хранилище подписчиков с использованием слабых ссылок
        /// </summary>
        private readonly Dictionary<Type, List<WeakReference>> _subscribers = new Dictionary<Type, List<WeakReference>>();

        /// <summary>
        /// Объект для синхронизации доступа к словарю
        /// </summary>
        private readonly object _syncRoot = new object();

        /// <summary>
        /// Публикует сообщения для всех подписанных обработчиков
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения</typeparam>
        /// <param name="messages">Коллекция сообщений</param>
        public void Publish<T>(IEnumerable<T> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            Type elementType = typeof(T);
            List<WeakReference> subscribersToRemove = null;

            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(elementType, out var weakSubscribers))
                {
                    // Создаем копию списка для итерации
                    var subscribersCopy = weakSubscribers.ToList();

                    foreach (var weakSubscriber in subscribersCopy)
                    {
                        if (weakSubscriber.IsAlive)
                        {
                            if (weakSubscriber.Target is Action<IEnumerable<T>> handler)
                            {
                                try
                                {
                                    handler(messages);
                                }
                                catch (Exception ex)
                                {
                                    // Логируем ошибку, но продолжаем уведомлять других подписчиков
                                    System.Diagnostics.Debug.WriteLine($"Ошибка при обработке списка сообщений: {ex}");
                                }
                            }
                        }
                        else
                        {
                            // Помечаем мертвые ссылки для удаления
                            if (subscribersToRemove == null)
                                subscribersToRemove = new List<WeakReference>();
                            subscribersToRemove.Add(weakSubscriber);
                        }
                    }

                    // Удаляем мертвые ссылки
                    if (subscribersToRemove != null)
                    {
                        foreach (var dead in subscribersToRemove)
                        {
                            weakSubscribers.Remove(dead);
                        }
                    }

                    // Если список стал пустым, удаляем ключ
                    if (weakSubscribers.Count == 0)
                    {
                        _subscribers.Remove(elementType);
                    }
                }
            }
        }

        /// <summary>
        /// Регистрирует обработчик для сообщений указанного типа
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения для подписки</typeparam>
        /// <param name="handler">Обработчик сообщений</param>
        public void Subscribe<T>(Action<IEnumerable<T>> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type elementType = typeof(T);

            lock (_syncRoot)
            {
                if (!_subscribers.TryGetValue(elementType, out var weakSubscribers))
                {
                    weakSubscribers = new List<WeakReference>();
                    _subscribers[elementType] = weakSubscribers;
                }

                // Проверяем, не подписан ли уже этот обработчик
                if (!IsAlreadySubscribed(weakSubscribers, handler))
                {
                    weakSubscribers.Add(new WeakReference(handler));
                }
            }
        }

        /// <summary>
        /// Отписывает обработчик от сообщений указанного типа
        /// </summary>
        /// <typeparam name="T">Тип элементов сообщения</typeparam>
        /// <param name="handler">Обработчик для отписки</param>
        public void Unsubscribe<T>(Action<IEnumerable<T>> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type elementType = typeof(T);

            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(elementType, out var weakSubscribers))
                {
                    var toRemove = weakSubscribers
                        .Where(wr => wr.IsAlive && wr.Target is Action<IEnumerable<T>> existingHandler && existingHandler.Equals(handler))
                        .ToList();

                    foreach (var item in toRemove)
                    {
                        weakSubscribers.Remove(item);
                    }

                    if (weakSubscribers.Count == 0)
                    {
                        _subscribers.Remove(elementType);
                    }
                }
            }
        }

        /// <summary>
        /// Отписывает все обработчики для указанного объекта-получателя
        /// </summary>
        /// <param name="target">Объект, чьи обработчики нужно отписать</param>
        public void UnsubscribeAllForTarget(object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_syncRoot)
            {
                var typesToRemove = new List<Type>();

                foreach (var kvp in _subscribers)
                {
                    var toRemove = kvp.Value
                        .Where(wr => wr.IsAlive && IsHandlerFromTarget(wr.Target, target))
                        .ToList();

                    foreach (var item in toRemove)
                    {
                        kvp.Value.Remove(item);
                    }

                    if (kvp.Value.Count == 0)
                    {
                        typesToRemove.Add(kvp.Key);
                    }
                }

                foreach (var type in typesToRemove)
                {
                    _subscribers.Remove(type);
                }
            }
        }

        /// <summary>
        /// Проверяет, принадлежит ли обработчик указанному целевому объекту
        /// </summary>
        private bool IsHandlerFromTarget(object handler, object target)
        {
            if (handler == null) return false;

            var handlerType = handler.GetType();
            if (!handlerType.IsGenericType) return false;

            // Получаем метод, который вызывает обработчик (делегат)
            var delegateInfo = handlerType.GetMethod("Invoke");
            if (delegateInfo == null) return false;

            // Получаем объект, которому принадлежит метод
            var targetObject = delegateInfo.IsStatic ? null : handlerType.GetProperty("Target")?.GetValue(handler);

            return targetObject == target;
        }

        /// <summary>
        /// Проверяет, есть ли уже такой обработчик в списке подписчиков
        /// </summary>
        private bool IsAlreadySubscribed<T>(List<WeakReference> weakSubscribers, Action<IEnumerable<T>> handler)
        {
            foreach (var weakRef in weakSubscribers)
            {
                if (weakRef.IsAlive && weakRef.Target is Action<IEnumerable<T>> existingHandler)
                {
                    if (existingHandler.Equals(handler))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Очищает все подписки
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _subscribers.Clear();
            }
        }

        /// <summary>
        /// Получает количество активных подписчиков для указанного типа (для отладки)
        /// </summary>
        public int GetSubscriberCount<T>()
        {
            Type elementType = typeof(T);

            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(elementType, out var weakSubscribers))
                {
                    // Очищаем мертвые ссылки перед подсчетом
                    weakSubscribers.RemoveAll(wr => !wr.IsAlive);
                    return weakSubscribers.Count;
                }
                return 0;
            }
        }
    }
}
