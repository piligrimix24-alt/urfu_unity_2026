using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;

    [Header("Количество кубиков")]
    [SerializeField] private int diceCount = 3;

    [Header("Ссылки на компоненты")]
    [SerializeField] private GameUI gameUI;

    private Vector3 _spawnCenter = new Vector3(0f, 5f, 0f);
    private float _spawnRadius = 6f;
    private List<Dice> _diceList = new List<Dice>();
    private bool _isRolling = false;

    private int _minPossible;
    private int _maxPossible;
    private int _targetScore;

    private void Start()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPlayerRoll += OnPlayerRoll;

        _minPossible = diceCount * 1;
        _maxPossible = diceCount * 6;

        SpawnDice(diceCount);

        if (gameUI != null)
        {
            gameUI.UpdateDiceCount(diceCount);
            GenerateTarget();
        }
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

    private void GenerateTarget()
    {
        _targetScore = Random.Range(_minPossible, _maxPossible - 3);

        if (gameUI != null)
        {
            gameUI.UpdateTarget(_targetScore);
        }
    }

    public void SpawnDice(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var offset = Random.insideUnitSphere * _spawnRadius;
            offset.y = 0f;
            var pos = _spawnCenter + offset;
            var dice = Instantiate(dicePrefab, pos, Quaternion.identity);
            _diceList.Add(dice);
        }
    }

    public void RollAllDice()
    {
        if (_isRolling) return;
        _isRolling = true;

        if (gameUI != null) gameUI.ResetColor();
        GenerateTarget();
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
        if (gameUI != null)
        {
            gameUI.ShowRoundResult(total, _targetScore);
        }
        _isRolling = false;
    }
}