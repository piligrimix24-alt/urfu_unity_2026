using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [Header("Количество кубиков")]
    [SerializeField] private int diceCount = 3;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private Vector3 spawnCenter = new Vector3(0f, 5f, 0f);
    private float spawnRadius = 6f;
    private List<Dice> _diceList = new List<Dice>();
    private bool _isRolling = false;

    private void Start()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPlayerRoll += OnPlayerRoll;

        SpawnDice(diceCount);
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPlayerRoll -= OnPlayerRoll;
    }

    private void OnPlayerRoll(object sender, System.EventArgs e)
    {
        RollAllDice();
    }
    // ============================================================mew
    public void SpawnDice(int count)
    {
        for (int i = 0; i < diceCount; i++)
        {
            var offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0f;
            var pos = spawnCenter + offset;
            var dice = Instantiate(dicePrefab, pos, Quaternion.identity);
            _diceList.Add(dice);
        }
    }
    public void RollAllDice()
    {
        if (_isRolling) return;
        _isRolling = true;
        if (scoreText != null) scoreText.text = "Бросок...";

        foreach (var dice in _diceList)
        {
            if (dice != null) dice.Roll();
        }
        StartCoroutine(WaitForAllDiceStopped());
    }

    private IEnumerator WaitForAllDiceStopped()
    {
        yield return new WaitForSeconds(0.8f);
        var timeout = 5f;
        var elapsed = 0f;
        var allStopped = false;
        while (!allStopped && elapsed < timeout)
        {
            allStopped = true;
            foreach (var dice in _diceList)
            {
                if (dice != null && !dice.IsStopped())
                {
                    allStopped = false;
                    break;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        int total = 0;
        foreach (var dice in _diceList)
        {
            total += dice.GetTopFaceValue();
        }
        if (scoreText != null)
            scoreText.text = $"Сумма: {total}";
        _isRolling = false;
    }
}