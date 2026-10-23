using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diceResultText;
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private TextMeshProUGUI diceCountText;

    [Header("Цвета")]
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private Color drawColor;
    [SerializeField] private Color defaultColor;

    public void UpdateTarget(int targetScore)
    {
        if (targetText != null)
        {
            targetText.text = $"Цель: {targetScore}";
        }
    }

    public void UpdateDiceCount(int count)
    {
        if (diceCountText != null)
        {
            diceCountText.text = $"Кубиков: {count}";
        }
    }

    public void ShowRoundResult(int totalScore, int targetScore)
    {
        string resultMessage = $"Твой результат: {totalScore}\n";

        if (totalScore > targetScore)
        {
            resultMessage += "Победа!";
            if (diceResultText != null) diceResultText.color = winColor;
        }
        else if (totalScore < targetScore)
        {
            resultMessage += "Поражение";
            if (diceResultText != null) diceResultText.color = loseColor;
        }
        else
        {
            resultMessage += "Ничья";
            if (diceResultText != null) diceResultText.color = drawColor;
        }

        if (diceResultText != null)
        {
            diceResultText.text = resultMessage;
        }
    }

    public void ResetColor()
    {
        if (diceResultText != null)
        {
            diceResultText.color = defaultColor;
        }
    }
}