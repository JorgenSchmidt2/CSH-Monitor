using CSH_Monitor.Core.Interfaces.Infrastructure;

namespace CSH_Monitor.Infrastructure.Messaging
{
    public class Messenger : IMessenger
    {
        /// <summary>
        /// Хранилище подписчиков с использованием слабых ссылок
        /// </summary>
        private readonly Dictionary<Type, List<WeakReference>> _subscribers = new Dictionary<Type, List<WeakReference>>();

        /// <summary>
        /// Объект для синхронизации доступа к словарю
        /// </summary>
        private readonly object _syncRoot = new object();

        public void Publish<T>(T message)
        {
            Type messageType = typeof(T);
            List<WeakReference> subscribersToRemove = null;

            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(messageType, out var weakSubscribers))
                {
                    // Создаем копию списка для итерации, чтобы избежать проблем при модификации
                    var subscribersCopy = weakSubscribers.ToList();

                    foreach (var weakSubscriber in subscribersCopy)
                    {
                        if (weakSubscriber.IsAlive)
                        {
                            if (weakSubscriber.Target is Action<T> handler)
                            {
                                try
                                {
                                    handler(message);
                                }
                                catch (Exception ex)
                                {
                                    // Логируем ошибку, но продолжаем уведомлять других подписчиков
                                    //System.Diagnostics.Debug.WriteLine($"Ошибка при обработке сообщения: {ex}");
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
                        _subscribers.Remove(messageType);
                    }
                }
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type messageType = typeof(T);

            lock (_syncRoot)
            {
                if (!_subscribers.TryGetValue(messageType, out var weakSubscribers))
                {
                    weakSubscribers = new List<WeakReference>();
                    _subscribers[messageType] = weakSubscribers;
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
        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type messageType = typeof(T);

            lock (_syncRoot)
            {
                if (_subscribers.TryGetValue(messageType, out var weakSubscribers))
                {
                    var toRemove = weakSubscribers
                        .Where(wr => wr.IsAlive && wr.Target is Action<T> existingHandler && existingHandler.Equals(handler))
                        .ToList();

                    foreach (var item in toRemove)
                    {
                        weakSubscribers.Remove(item);
                    }

                    if (weakSubscribers.Count == 0)
                    {
                        _subscribers.Remove(messageType);
                    }
                }
            }
        }

        /// <summary>
        /// Отписывает все обработчики для указанного объекта-получателя
        /// </summary>
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
                        .Where(wr => wr.IsAlive && wr.Target?.GetType().DeclaringType == target.GetType())
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
        /// Проверяет, есть ли уже такой обработчик в списке подписчиков
        /// </summary>
        private bool IsAlreadySubscribed<T>(List<WeakReference> weakSubscribers, Action<T> handler)
        {
            foreach (var weakRef in weakSubscribers)
            {
                if (weakRef.IsAlive && weakRef.Target is Action<T> existingHandler)
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
    }
}