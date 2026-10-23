using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;
    private bool _isDisposed = false;
    public event EventHandler OnPlayerRoll;

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Dice.Roll.started += PlayerRoll_started;
    }
    private void OnDestroy()
    {
        Dispose();
    }

    private void OnApplicationQuit()
    {
        Dispose();
    }
    // ============================================================mew
    private void Dispose()
    {
        if (_isDisposed) return;
        if (_playerInputActions != null)
        {
            _playerInputActions.Dice.Roll.started -= PlayerRoll_started;
            _playerInputActions.Disable();
            _playerInputActions.Dispose();
            _playerInputActions = null;
        }
        _isDisposed = true;
    }

    private void PlayerRoll_started(InputAction.CallbackContext context)
    {
        Debug.Log("Roll dice");
        OnPlayerRoll?.Invoke(this, EventArgs.Empty);
    }
}

