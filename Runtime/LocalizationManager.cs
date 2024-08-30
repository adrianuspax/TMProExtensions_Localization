using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ASP.Extensions
{
    public static class LocalizationManager
    {
        private static AsyncOperationHandle<string> operation;

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

            LocalizedStringDatabase stringDatabase = LocalizationSettings.StringDatabase;
            operation = stringDatabase.GetLocalizedStringAsync(table, entry);

            _updateString(operation);
            
            return operation;

            void _updateString(AsyncOperationHandle<string> operation)
            {
                if (!operation.IsDone)
                {
                    operation.Completed += _updateString;
                    return;
                }

                action?.Invoke(operation.Result);
            }
        }
    }
}


