using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diceResultText;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI diceCountText;

    [SerializeField] private GameManager _gameManager;

    [Header("Ввод данных")]
    [SerializeField] private TMP_InputField diceCountInput;
    [SerializeField] private Button rollButton;

    [Header("Цвета")]
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private Color drawColor;
    [SerializeField] private Color defaultColor;

    private void Awake()
    {
        GameManager.OnRoundResult += ShowRoundResult;
        GameManager.OnTargetChanged += UpdateTarget;
        GameManager.OnDiceCountChanged += UpdateDiceCount;
    }

    private void Start()
    {
        if (rollButton != null && _gameManager != null)
        {
            rollButton.onClick.AddListener(() =>
            {
                ResetColor();
                _gameManager.RollAllDice();
            });
        }
    }

    private void OnDestroy()
    {
        GameManager.OnRoundResult -= ShowRoundResult;
        GameManager.OnTargetChanged -= UpdateTarget;
        GameManager.OnDiceCountChanged -= UpdateDiceCount;
    }

    public void ApplySettingsFromInput()
    {
        if (_gameManager == null) return;

        if (diceCountInput != null && int.TryParse(diceCountInput.text, out int count))
        {
            _gameManager.SetDiceCount(count);
        }
    }

    private void UpdateTarget(int targetScore)
    {
        if (targetText != null)
            targetText.text = $"Цель: {targetScore}";
    }

    private void UpdateDiceCount(int count)
    {
        if (diceCountText != null)
            diceCountText.text = $"Кубиков: {count}";
    }

    private void ShowRoundResult(int totalScore, int targetScore)
    {
        if (diceResultText == null) return;

        string resultMessage = $"Твой результат: {totalScore}\n";

        if (totalScore > targetScore)
        {
            resultMessage += "Победа!";
            diceResultText.color = winColor;
        }
        else if (totalScore < targetScore)
        {
            resultMessage += "Поражение";
            diceResultText.color = loseColor;
        }
        else
        {
            resultMessage += "Ничья";
            diceResultText.color = drawColor;
        }

        diceResultText.text = resultMessage;
    }

    public void ResetColor()
    {
        if (diceResultText != null)
            diceResultText.color = defaultColor;
    }
}