[English Version](./Documentation~/README.en-US.md)

[Korean Version](./Documentation~/README.ko-KR.md)

# Extensões de Localização para TMPro

Esta biblioteca fornece um conjunto de métodos de extensão para `TextMeshProUGUI` e uma classe auxiliar para fazer a interface com o pacote de Localização (Localization) da Unity, simplificando o processo de buscar e atribuir textos, fontes e materiais localizados dinamicamente via código.

## Como Instalar

Você pode adicionar esta biblioteca ao seu projeto Unity usando o Unity Package Manager (UPM) com uma URL Git:

```
https://github.com/adrianuspax/MyScriptsUnity.git?path=/Assets/TMProLocalization
```

## Referência da API

Esta biblioteca é dividida em duas classes estáticas principais: `LocalizationManager` e `TextMeshProUGUIExtensions`.

---

### `TextMeshProUGUIExtensions`

Esta classe fornece métodos de extensão para o componente `TextMeshProUGUI`, permitindo que você manipule diretamente suas propriedades com assets localizados.

Para usar esses métodos, você deve importar o namespace no seu script:
```csharp
using ASPax.Extensions.Localization;
```

#### **`SetLocalizationString(string entry, string table)`**
Busca um texto localizado da tabela especificada e o atribui à propriedade `text` do componente `TextMeshProUGUI`. Este é um método ideal para componentes que não precisam ser rastreados para mudanças de localidade automaticamente por um `LocalizeStringEvent`.

*   **Parâmetros:**
    *   `entry`: A chave (nome) do texto na Tabela de Textos (String Table).
    *   `table`: O nome da Coleção de Tabelas de Texto (String Table Collection).

*   **Exemplo:**
    ```csharp
    using TMPro;
    using UnityEngine;
    using ASPax.Extensions.Localization;

    public class MyUI : MonoBehaviour
    {
        public TextMeshProUGUI titleText;

        void Start()
        {
            // Define o texto para o valor de "main_title" da tabela "UI_Text"
            titleText.SetLocalizationString("main_title", "UI_Text");
        }
    }
    ```

#### **`SetLocalizationStringTrackedUGUI(string entry, string table, MonoBehaviour monoBehaviour)`**
Atualiza a referência de texto em um componente `TextMeshProUGUI` que já é rastreado por um `GameObjectLocalizer` ou `LocalizeStringEvent`. Isso permite que você altere qual texto localizado é exibido, mantendo a capacidade do componente de se atualizar automaticamente quando a localidade do aplicativo muda.

*   **Parâmetros:**
    *   `entry`: A nova chave do texto.
    *   `table`: O novo nome da coleção de tabelas.
    *   `monoBehaviour`: Uma referência ao `MonoBehaviour` que chama o método (geralmente `this`), necessária para executar a corrotina de atualização.

*   **Retorna:** `true` se a referência foi atribuída com sucesso.

*   **Exemplo:**
    ```csharp
    // Neste exemplo, suponha que `scoreText` tenha um componente `LocalizeStringEvent`.
    public class ScoreDisplay : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public void ShowFinalScore()
        {
            // Altera o texto de "current_score" para "final_score"
            scoreText.SetLocalizationStringTrackedUGUI("final_score", "UI_Text", this);
        }
    }
    ```

#### **`SetLocalizationFont(string tableEntryReference, string tableReference)`**
Busca uma `TMP_FontAsset` localizada de uma Tabela de Assets (Asset Table) e a atribui à propriedade `font` do componente `TextMeshProUGUI`.

*   **Parâmetros:**
    *   `tableEntryReference`: A chave do asset da fonte na Tabela de Assets.
    *   `tableReference`: O nome da Coleção de Tabelas de Assets.

*   **Exemplo:**
    ```csharp
    // Altera a fonte para uma específica do idioma selecionado
    myText.SetLocalizationFont("jp_font", "Fonts");
    ```

#### **`SetLocalizationMaterial(string tableEntryReference, string tableReference)`**
Busca um `Material` localizado de uma Tabela de Assets e o atribui à propriedade `fontMaterial` do componente `TextMeshProUGUI`.

*   **Parâmetros:**
    *   `tableEntryReference`: A chave do asset do material na Tabela de Assets.
    *   `tableReference`: O nome da Coleção de Tabelas de Assets.

*   **Exemplo:**
    ```csharp
    // Aplica um material/estilo diferente à fonte
    myText.SetLocalizationMaterial("outline_style_red", "Font_Materials");
    ```

---

### `LocalizationManager`

Uma classe auxiliar estática para lidar com operações assíncronas do sistema de Localização da Unity.

#### **`SetLocalizationString(string entry, string table, UnityAction<string> action)`**
Busca de forma assíncrona um texto localizado e executa um callback com o resultado. Este é o método principal usado pela extensão `SetLocalizationString`.

*   **Retorna:** Um `AsyncOperationHandle<string>` para a operação.

*   **Exemplo:**
    ```csharp
    void Start()
    {
        LocalizationManager.SetLocalizationString("welcome_msg", "UI_Text", (localizedText) => {
            Debug.Log(localizedText);
        });
    }
    ```

#### **`SetAsyncOperationHandler(...)`**
Um conjunto de métodos sobrecarregados para gerenciar a conclusão de qualquer operação de localização assíncrona (`AsyncOperationHandle`). Você pode fornecer um callback que é executado quando a operação termina, usando um manipulador baseado em eventos ou uma corrotina gerenciada por um `MonoBehaviour`.

*   **Exemplo (baseado em evento):**
    ```csharp
    var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI_Text", "loading");
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // Este código executa quando o texto é carregado
        loadingText.text = op.Result;
    });
    ```

*   **Exemplo (baseado em corrotina):**
    ```csharp
    var operation = LocalizationSettings.InitializationOperation;
    LocalizationManager.SetAsyncOperationHandler(operation, (op) => {
        // Este código executa após a inicialização do sistema de localização
        Debug.Log("Localization Ready!");
    }, this);
    ```

#### **`InitializationOperation`**
Uma propriedade estática que fornece acesso direto a `LocalizationSettings.InitializationOperation`.

*   **Exemplo:**
    ```csharp
    // Aguarda manualmente até que o sistema de localização seja inicializado
    yield return LocalizationManager.InitializationOperation;
    ```
