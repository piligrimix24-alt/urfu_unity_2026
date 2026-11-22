using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;

    private int _diceCount = 3;
    private Vector3 _spawnCenter = new Vector3(0f, 5f, 0f);
    private float _spawnRadius = 6f;
    private List<Dice> _diceList = new List<Dice>();
    private bool _isRolling = false;

    private int _minPossible;
    private int _maxPossible;
    private int _targetScore;

    public static event Action<int, int> OnRoundResult;
    public static event Action<int> OnTargetChanged;
    public static event Action<int> OnDiceCountChanged;

    private void Awake()
    {
        _minPossible = _diceCount * 1;
        _maxPossible = _diceCount * 6;
    }

    private void Start()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPlayerRoll += OnPlayerRoll;

        SpawnDice(_diceCount);
        GenerateTarget();
        OnDiceCountChanged?.Invoke(_diceCount);
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPlayerRoll -= OnPlayerRoll;
    }

    private void OnPlayerRoll(object sender, EventArgs e)
    {
        RollAllDice();
    }

    public void SetDiceCount(int count)
    {
        if (count <= 0) return;

        _diceCount = count;
        _minPossible = _diceCount * 1;
        _maxPossible = _diceCount * 6;

        SpawnDice(_diceCount);
        GenerateTarget();
        OnDiceCountChanged?.Invoke(_diceCount);
    }

    private void GenerateTarget()
    {
        _targetScore = UnityEngine.Random.Range(_minPossible, _maxPossible - 3);
        OnTargetChanged?.Invoke(_targetScore);
    }

    public void SpawnDice(int count)
    {
        foreach (var dice in _diceList)
        {
            if (dice != null) Destroy(dice.gameObject);
        }
        _diceList.Clear();

        for (int i = 0; i < count; i++)
        {
            var offset = UnityEngine.Random.insideUnitSphere * _spawnRadius;
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
            if (dice != null) total += dice.GetTopFaceValue();
        }

        OnRoundResult?.Invoke(total, _targetScore);
        _isRolling = false;
    }
}