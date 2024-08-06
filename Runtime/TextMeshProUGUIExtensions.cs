using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.Localization.PropertyVariants.TrackedObjects;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ASP.Extensions
{
    /// <summary>
    /// Code extension for Text Mesh Pro UGUI
    /// </summary>
    public static class TextMeshProUGUIExtensions
    {
        private static Coroutine coroutine;
        /// <summary>
        /// Set Localization String Reference
        /// </summary>
        /// <param name="tmp">Text Mesh Pro UGUI</param>
        /// <param name="entry">Reference to the <see cref="TableEntry.Key"/> or <see cref="TableEntry.KeyId"</param>
        /// <param name="table">Reference to the <see cref="LocalizationTable.TableCollectionName"/> or <see cref="SharedTableData.TableCollectionNameGuid"</param>
        /// <param name="monoBehaviour">Mono Behaviour (Use this)</param>
        /// <returns>Returns true if the localization reference was successfully assigned</returns>
        public static bool SetLocalizationStringTrackedUGUI(this TextMeshProUGUI tmp, string entry, string table, MonoBehaviour monoBehaviour)
        {
            if (tmp.TryGetComponent(out GameObjectLocalizer gameObjectLocalizer))
            {
                TrackedUGuiGraphic trackedText = gameObjectLocalizer.GetTrackedObject<TrackedUGuiGraphic>(tmp);
                LocalizedStringProperty textVariant = trackedText.GetTrackedProperty<LocalizedStringProperty>("m_text");
                textVariant.LocalizedString.SetReference(table, entry);
                coroutine ??= monoBehaviour.StartCoroutine(_applyLocaleVariant());
            }
            else if (tmp.TryGetComponent(out LocalizeStringEvent localizeStringEvent))
            {
                localizeStringEvent.StringReference.SetReference(table, entry);
            }
            else
            {
                Debug.LogWarning($"The referenced TextMeshPro has no GameObjectLocalizer or LocalizeStringEvent built-in!", monoBehaviour);
            }

            return coroutine == null;

            IEnumerator _applyLocaleVariant()
            {
                AsyncOperationHandle operation;

                do
                {
                    operation = gameObjectLocalizer.ApplyLocaleVariant(LocalizationSettings.SelectedLocale);
                    yield return null;
                } while (!operation.IsDone);

                coroutine = null;
            }
        }
        /// <summary>
        /// Set Localization String
        /// </summary>
        /// <param name="entry">The key that the string is stored in StringTable</param>
        /// <param name="table">The key that the string is stored in Table Collection</param>
        /// <returns>Return the localization string</returns>
        public static void SetLocalizationString(this TextMeshProUGUI tmp, string entry, string table)
        {
            LocalizationManager.SetLocalizationString(entry, table, (result) => tmp.text = result);
        }
        /// <summary>
        /// Set Localization Font
        /// </summary>
        /// <param name="tableEntryReference">The key that the string is stored in StringTable</param>
        /// <param name="tableReference">The key that the string is stored in Table Collection</param>
        public static void SetLocalizationFont(this TextMeshProUGUI tmp, string tableEntryReference, string tableReference)
        {
            if (string.IsNullOrEmpty(tableReference))
            {
                Debug.LogWarning($"The {nameof(tableReference)} cannot be null or empty!");
                return;
            }

            if (string.IsNullOrEmpty(tableEntryReference))
            {
                Debug.LogWarning($"The {nameof(tableEntryReference)} cannot be null or empty!");
                return;
            }

            TMP_FontAsset font = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<TMP_FontAsset>(tableReference, tableEntryReference).Result;

            if (font == null)
            {
                Debug.LogWarning($"The key was not found: \"{tableEntryReference}\"!");
                return;
            }

            tmp.font = font;
        }
        /// <summary>
        /// Set Localization Material
        /// </summary>
        /// <param name="tableEntryReference">The key that the string is stored in StringTable</param>
        /// <param name="tableReference">The key that the string is stored in Table Collection</param>
        public static void SetLocalizationMaterial(this TextMeshProUGUI tmp, string tableEntryReference, string tableReference)
        {
            if (string.IsNullOrEmpty(tableReference))
            {
                Debug.LogWarning($"The {nameof(tableReference)} cannot be null or empty!");
                return;
            }

            if (string.IsNullOrEmpty(tableEntryReference))
            {
                Debug.LogWarning($"The {nameof(tableEntryReference)} cannot be null or empty!");
                return;
            }

            Material material = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<Material>(tableReference, tableEntryReference).Result;

            if (material == null)
            {
                Debug.LogWarning($"The key was not found: \"{tableEntryReference}\"!");
                return;
            }

            tmp.fontMaterial = material;
        }
    }
}
