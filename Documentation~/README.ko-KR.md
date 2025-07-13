[Portuguese Version](../README.md)

[English Version](./README.en-US.md)

# TMPro 지역화 확장

이 라이브러리는 `TextMeshProUGUI`를 위한 확장 메서드 세트와 Unity의 지역화(Localization) 패키지와의 인터페이스를 위한 헬퍼 클래스를 제공하여, 코드를 통해 지역화된 문자열, 글꼴 및 머티리얼을 동적으로 가져오고 할당하는 과정을 단순화합니다.

## 설치 방법

Unity 패키지 관리자(UPM)와 Git URL을 사용하여 이 라이브러리를 Unity 프로젝트에 추가할 수 있습니다:

```
https://github.com/adrianuspax/MyScriptsUnity.git?path=/Assets/TMProLocalization
```

## API 참조

이 라이브러리는 `LocalizationManager`와 `TextMeshProUGUIExtensions`라는 두 개의 주요 정적 클래스로 나뉩니다.

---

### `TextMeshProUGUIExtensions`

이 클래스는 `TextMeshProUGUI` 컴포넌트를 위한 확장 메서드를 제공하여, 지역화된 에셋으로 직접 속성을 조작할 수 있게 합니다.

이 메서드들을 사용하려면 스크립트에서 네임스페이스를 가져와야 합니다:
```csharp
using ASPax.Extensions.Localization;
```

#### **`SetLocalizationString(string entry, string table)`**
지정된 테이블에서 지역화된 문자열을 가져와 `TextMeshProUGUI` 컴포넌트의 `text` 속성에 할당합니다. `LocalizeStringEvent`에 의해 로케일 변경을 자동으로 추적할 필요가 없는 컴포넌트에 이상적인 메서드입니다.

*   **매개변수:**
    *   `entry`: 문자열 테이블(String Table)에 있는 문자열 항목의 키(이름).
    *   `table`: 문자열 테이블 컬렉션(String Table Collection)의 이름.

*   **예제:**
    ```csharp
    using TMPro;
    using UnityEngine;
    using ASPax.Extensions.Localization;

    public class MyUI : MonoBehaviour
    {
        public TextMeshProUGUI titleText;

        void Start()
        {
            // "UI_Text" 테이블의 "main_title" 값으로 텍스트를 설정합니다
            titleText.SetLocalizationString("main_title", "UI_Text");
        }
    }
    ```

#### **`SetLocalizationStringTrackedUGUI(string entry, string table, MonoBehaviour monoBehaviour)`**
`GameObjectLocalizer` 또는 `LocalizeStringEvent`에 의해 이미 추적되고 있는 `TextMeshProUGUI` 컴포넌트의 문자열 참조를 업데이트합니다. 이를 통해 애플리케이션의 로케일이 변경될 때 컴포넌트의 자동 업데이트 기능을 유지하면서 표시되는 지역화된 문자열을 변경할 수 있습니다.

*   **매개변수:**
    *   `entry`: 새로운 문자열 항목의 키.
    *   `table`: 새로운 테이블 컬렉션 이름.
    *   `monoBehaviour`: 업데이트 코루틴을 실행하는 데 필요한 `MonoBehaviour`에 대한 참조 (일반적으로 `this`).

*   **반환값:** 참조가 성공적으로 할당되면 `true`를 반환합니다.

*   **예제:**
    ```csharp
    // 이 예제에서 `scoreText`에 `LocalizeStringEvent` 컴포넌트가 첨부되어 있다고 가정합니다.
    public class ScoreDisplay : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public void ShowFinalScore()
        {
            // 텍스트를 "current_score"에서 "final_score"로 변경합니다
            scoreText.SetLocalizationStringTrackedUGUI("final_score", "UI_Text", this);
        }
    }
    ```

#### **`SetLocalizationFont(string tableEntryReference, string tableReference)`**
에셋 테이블(Asset Table)에서 지역화된 `TMP_FontAsset`을 가져와 `TextMeshProUGUI` 컴포넌트의 `font` 속성에 할당합니다.

*   **매개변수:**
    *   `tableEntryReference`: 에셋 테이블에 있는 글꼴 에셋의 키.
    *   `tableReference`: 에셋 테이블 컬렉션의 이름.

*   **예제:**
    ```csharp
    // 선택된 언어에 맞는 특정 글꼴로 변경합니다
    myText.SetLocalizationFont("jp_font", "Fonts");
    ```

#### **`SetLocalizationMaterial(string tableEntryReference, string tableReference)`**
에셋 테이블에서 지역화된 `Material`을 가져와 `TextMeshProUGUI` 컴포넌트의 `fontMaterial` 속성에 할당합니다.

*   **매개변수:**
    *   `tableEntryReference`: 에셋 테이블에 있는 머티리얼 에셋의 키.
    *   `tableReference`: 에셋 테이블 컬렉션의 이름.

*   **예제:**
    ```csharp
    // 글꼴에 다른 머티리얼/스타일을 적용합니다
    myText.SetLocalizationMaterial("outline_style_red", "Font_Materials");
    ```

---

### `LocalizationManager`

Unity의 지역화 시스템에서 비동기 작업을 처리하기 위한 정적 헬퍼 클래스입니다.

#### **`SetLocalizationString(string entry, string table, UnityAction<string> action)`**
비동기적으로 지역화된 문자열을 가져오고 결과와 함께 콜백을 실행합니다. 이것은 `SetLocalizationString` 확장에서 사용하는 핵심 메서드입니다.

*   **반환값:** 작업에 대한 `AsyncOperationHandle<string>`.

*   **예제:**
    ```csharp
    void Start()
    {
        LocalizationManager.SetLocalizationString("welcome_msg", "UI_Text", (localizedText) => {
            Debug.Log(localizedText);
        });
    }
    ```

#### **`SetAsyncOperationHandler(...)`**
모든 비동기 지역화 작업(`AsyncOperationHandle`)의 완료를 관리하기 위한 오버로드된 메서드 세트입니다. 이벤트 기반 핸들러나 `MonoBehaviour`가 관리하는 코루틴을 사용하여 작업이 완료될 때 실행되는 콜백을 제공할 수 있습니다.

*   **예제 (이벤트 기반):**
    ```csharp
    var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Text", "loading");
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // 이 코드는 문자열이 로드될 때 실행됩니다
        loadingText.text = op.Result;
    });
    ```

*   **예제 (코루틴 기반):**
    ```csharp
    var operation = LocalizationSettings.InitializationOperation;
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // 이 코드는 지역화 시스템이 초기화된 후 실행됩니다
        Debug.Log("Localization Ready!");
    }, this);
    ```

#### **`InitializationOperation`**
`LocalizationSettings.InitializationOperation`에 직접 접근할 수 있는 정적 속성입니다.

*   **예제:**
    ```csharp
    // 지역화 시스템이 초기화될 때까지 수동으로 대기합니다
    yield return LocalizationManager.InitializationOperation;
    ```
