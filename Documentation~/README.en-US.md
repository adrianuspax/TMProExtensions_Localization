[Portuguese Version](./README.md)

[Korean Version](./Documentation~/README.ko-KR.md)

# TMPro Localization Extensions

This library provides a set of extension methods for `TextMeshProUGUI` and a helper class to interface with Unity's Localization package, simplifying the process of fetching and assigning localized strings, fonts, and materials dynamically via code.

## How to Install

You can add this library to your Unity project using the Unity Package Manager (UPM) with a Git URL:

```
https://github.com/adrianuspax/TMProExtensions_Localization.git
```

## API Reference

This library is divided into two main static classes: `LocalizationManager` and `TextMeshProUGUIExtensions`.

---

### `TextMeshProUGUIExtensions`

This class provides extension methods for the `TextMeshProUGUI` component, allowing you to directly manipulate its properties with localized assets.

To use these methods, you must import the namespace in your script:
```csharp
using ASPax.Extensions.Localization;
```

#### **`SetLocalizationString(string entry, string table)`**
Fetches a localized string from the specified table and assigns it to the `text` property of the `TextMeshProUGUI` component. This is a "fire-and-forget" method for components that do not need to be tracked for locale changes automatically by a `LocalizeStringEvent`.

*   **Parameters:**
    *   `entry`: The key (name) of the string entry in the String Table.
    *   `table`: The name of the String Table Collection.

*   **Example:**
    ```csharp
    using TMPro;
    using UnityEngine;
    using ASPax.Extensions.Localization;

    public class MyUI : MonoBehaviour
    {
        public TextMeshProUGUI titleText;

        void Start()
        {
            // Sets the text to the value of "main_title" from the "UI_Text" table
            titleText.SetLocalizationString("main_title", "UI_Text");
        }
    }
    ```

#### **`SetLocalizationStringTrackedUGUI(string entry, string table, MonoBehaviour monoBehaviour)`**
Updates the string reference on a `TextMeshProUGUI` component that is already tracked by a `GameObjectLocalizer` or `LocalizeStringEvent`. This allows you to change which localized string is displayed while keeping the component's ability to auto-update when the application's locale changes.

*   **Parameters:**
    *   `entry`: The new key of the string entry.
    *   `table`: The new table collection name.
    *   `monoBehaviour`: A reference to the `MonoBehaviour` calling the method (usually `this`), required to run the update coroutine.

*   **Returns:** `true` if the reference was assigned successfully.

*   **Example:**
    ```csharp
    // In this example, assume `scoreText` has a `LocalizeStringEvent` component attached.
    public class ScoreDisplay : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public void ShowFinalScore()
        {
            // Change the text from "current_score" to "final_score"
            scoreText.SetLocalizationStringTrackedUGUI("final_score", "UI_Text", this);
        }
    }
    ```

#### **`SetLocalizationFont(string tableEntryReference, string tableReference)`**
Fetches a localized `TMP_FontAsset` from an Asset Table and assigns it to the `font` property of the `TextMeshProUGUI` component.

*   **Parameters:**
    *   `tableEntryReference`: The key of the font asset in the Asset Table.
    *   `tableReference`: The name of the Asset Table Collection.

*   **Example:**
    ```csharp
    // Change the font to a specific one for the selected language
    myText.SetLocalizationFont("jp_font", "Fonts");
    ```

#### **`SetLocalizationMaterial(string tableEntryReference, string tableReference)`**
Fetches a localized `Material` from an Asset Table and assigns it to the `fontMaterial` property of the `TextMeshProUGUI` component.

*   **Parameters:**
    *   `tableEntryReference`: The key of the material asset in the Asset Table.
    *   `tableReference`: The name of the Asset Table Collection.

*   **Example:**
    ```csharp
    // Apply a different material/style to the font
    myText.SetLocalizationMaterial("outline_style_red", "Font_Materials");
    ```

---

### `LocalizationManager`

A static helper class for handling asynchronous operations from Unity's Localization system.

#### **`SetLocalizationString(string entry, string table, UnityAction<string> action)`**
Asynchronously fetches a localized string and executes a callback with the result. This is the core method used by the `SetLocalizationString` extension.

*   **Returns:** An `AsyncOperationHandle<string>` for the operation.

*   **Example:**
    ```csharp
    void Start()
    {
        LocalizationManager.SetLocalizationString("welcome_msg", "UI_Text", (localizedText) => {
            Debug.Log(localizedText);
        });
    }
    ```

#### **`SetAsyncOperationHandler(...)`**
A set of overloaded methods to manage the completion of any asynchronous localization operation (`AsyncOperationHandle`). You can provide a callback that executes when the operation is done, either by using an event-based handler or a coroutine managed by a `MonoBehaviour`.

*   **Example (Event-based):**
    ```csharp
    var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Text", "loading");
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // This code runs when the string is loaded
        loadingText.text = op.Result;
    });
    ```

*   **Example (Coroutine-based):**
    ```csharp
    var operation = LocalizationSettings.InitializationOperation;
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // This code runs after the localization system is initialized
        Debug.Log("Localization Ready!");
    }, this);
    ```

#### **`InitializationOperation`**
A static property that provides direct access to the `LocalizationSettings.InitializationOperation`.

*   **Example:**
    ```csharp
    // Manually yield until the localization system is initialized
    yield return LocalizationManager.InitializationOperation;
    ```