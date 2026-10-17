using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    public static PlayerControlsManager instance;

    private PlayerControls _playerControls;

    [HideInInspector]
    public Vector3 lookDelta = Vector3.zero;
    [HideInInspector]
    public bool jumpInitiated = false;
    [HideInInspector]
    public bool shootInitiated = false;
    [HideInInspector]
    public bool shootAltInitiated = false;

    private static float mouseSensitivity = 0.25f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (instance == null)
        {
            instance = this;
        }

        this._playerControls = new PlayerControls();

        this._playerControls.PlayerMap.Look.performed += this.UpdateLookDirection;
        this._playerControls.PlayerMap.Look.canceled += this.StopLookDirection;
    }
    private void OnEnable()
    {
        this._playerControls.Enable();
    }

    private void OnDisable()
    {
        this._playerControls.Disable();
    }

    private void UpdateLookDirection(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        this.lookDelta = new Vector3(-mouseDelta.y, mouseDelta.x, 0.0f);
        this.lookDelta *= PlayerControlsManager.mouseSensitivity;
    }

    private void StopLookDirection(InputAction.CallbackContext context)
    {
        this.lookDelta = Vector3.zero;
    }

    public bool IsPressingForward()
    {
        return this._playerControls.PlayerMap.Forward.inProgress;
    }

    public bool IsPressingBack()
    {
        return this._playerControls.PlayerMap.Back.inProgress;
    }

    public bool IsPressingLeft()
    {
        return this._playerControls.PlayerMap.Left.inProgress;
    }

    public bool IsPressingRight()
    {
        return this._playerControls.PlayerMap.Right.inProgress;
    }

    public void UpdateMouseSensitivity(float value)
    {
        PlayerControlsManager.mouseSensitivity = value;
    }    
}