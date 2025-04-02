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
            SetAsyncOperationHandle(operation, (operation) => action?.Invoke(operation.Result));
            return operation;
        }
        /// <summary>
        /// Set Async Operation Handle
        /// </summary>
        /// <typeparam name="T">Type Generic T</typeparam>
        /// <param name="operation">The Async Operation Handle</param>
        /// <param name="action">Function to set action</param>
        public static void SetAsyncOperationHandle<T>(AsyncOperationHandle<T> operation, UnityAction<AsyncOperationHandle<T>> action)
        {
            _update(operation);

            void _update(AsyncOperationHandle<T> operation)
            {
                if (operation.IsDone)
                    action?.Invoke(operation);
                else
                    operation.Completed += _update;
            }
        }
    }
}
