using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ASPax.Extensions
{
    /// <summary>
    /// Localization Manager
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>
        /// Set localization string
        /// </summary>
        /// <param name="entry">Entry value from table</param>
        /// <param name="table">Table Value</param>
        /// <param name="action">Function to set action</param>
        /// <returns>Return Async operation Handle</returns>
        public static AsyncOperationHandle<string> SetLocalizationString(string entry, string table, UnityAction<string> action)
        {
            if (string.IsNullOrEmpty(table))
            {
                Debug.LogWarning($"The {nameof(table)} cannot be null or empty!");
                return default;
            }

            if (string.IsNullOrEmpty(entry))
            {
                Debug.LogWarning($"The {nameof(entry)} cannot be null or empty!");
                return default;
            }

            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, entry);
            SetAsyncOperationHandler(operation, (operation) => action?.Invoke(operation.Result));
            return operation;
        }
        /// <summary>
        /// Set Async Operation Handler with epecific operation.<br/>
        /// </summary>
        /// <typeparam name="T">Type Generic <typeparamref name="T"/> for <see cref="AsyncOperationHandle"/></typeparam>
        /// <param name="operation">The Async Operation Handle</param>
        /// <param name="action">Function to set action</param>
        /// <remarks>
        /// Will be used <see cref="AsyncOperationHandle.Completed"/>.
        /// </remarks>
        public static void SetAsyncOperationHandler<T>(AsyncOperationHandle<T> operation, UnityAction<AsyncOperationHandle<T>> action)
        {
            _update(operation);
            return;

            void _update(AsyncOperationHandle<T> operation)
            {
                if (operation.IsDone)
                    action?.Invoke(operation);
                else
                    operation.Completed += _update;
            }
        }
        /// <summary>
        /// Set Async Operation Handler without epecific operation.
        /// </summary>
        /// <param name="action">Function to set action</param>
        /// <remarks>
        /// Will be used <see cref="LocalizationSettings.InitializationOperation"/>.<br/>
        /// Will be used <see cref="AsyncOperationHandle.Completed"/>.
        /// </remarks>
        public static void SetAsyncOperationHandler(UnityAction action)
        {
            _update(LocalizationSettings.InitializationOperation);
            return;

            void _update(AsyncOperationHandle<LocalizationSettings> operation)
            {
                if (operation.IsDone)
                    action?.Invoke();
                else
                    operation.Completed += _update;
            }
        }
        /// <summary>
        /// Set Async Operation Handler with epecific operation
        /// </summary>
        /// <typeparam name="T">Type Generic <typeparamref name="T"/> for <see cref="AsyncOperationHandle"/></typeparam>
        /// <param name="operation">The Async Operation Handle</param>
        /// <param name="action">Function to set action</param>
        /// <param name="monoBehaviour">Used to start the coroutine. Use <see langword="this"/> in a class that inherits <see cref="MonoBehaviour"/></param>
        /// <remarks>
        /// Will be used <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>
        /// </remarks>
        public static void SetAsyncOperationHandler<T>(AsyncOperationHandle<T> operation, UnityAction<AsyncOperationHandle<T>> action, MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(_routine());
            return;

            IEnumerator _routine()
            {
                yield return operation;
                action?.Invoke(operation);
            }
        }
        /// <summary>
        /// Set Async Operation Handler without epecific operation.
        /// </summary>
        /// <param name="action">Function to set action</param>
        /// <param name="monoBehaviour">Used to start the coroutine. Use <see langword="this"/> in a class that inherits <see cref="MonoBehaviour"/></param>
        /// <remarks>
        /// Will be used <see cref="LocalizationSettings.InitializationOperation"/>.<br/>
        /// Will be used <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>
        /// </remarks>
        public static void SetAsyncOperationHandler(UnityAction action, MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(_routine());
            return;

            IEnumerator _routine()
            {
                yield return LocalizationSettings.InitializationOperation;
                action?.Invoke();
            }
        }
        /// <summary>
        /// Returns Initialization Operation from Localization Settings
        /// </summary>
        public static AsyncOperationHandle InitializationOperation => LocalizationSettings.InitializationOperation;
    }
}
