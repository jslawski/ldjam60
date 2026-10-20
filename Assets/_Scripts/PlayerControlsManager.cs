using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    public static PlayerControlsManager instance;

    private PlayerControls _playerControls;

    [HideInInspector]
    public Vector3 lookDelta = Vector3.zero;

    [HideInInspector]
    public bool leftClickInitiated = false;
    [HideInInspector]
    public bool rightClickInitiated = false;
    [HideInInspector]
    public bool spacePressInitiated = false;
    [HideInInspector]
    public bool backPressInitiated = false;

    private static float mouseSensitivity = 0.1f;

    public PlayerCharacterController characterController;

    private void Awake()
    {       
        if (instance == null)
        {
            instance = this;
        }

        this._playerControls = new PlayerControls();

        this._playerControls.PlayerMap.Look.performed += this.UpdateLookDirection;
        this._playerControls.PlayerMap.Look.canceled += this.StopLookDirection;

        this._playerControls.PlayerMap.LeftClick.performed += this.ExecuteLeftClick;
        this._playerControls.PlayerMap.RightClick.performed += this.ExecuteRightClick;
        this._playerControls.PlayerMap.Space.performed += this.ExecuteSpace;
        this._playerControls.PlayerMap.Back.performed += this.ExecuteBack;
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

    public bool IsHoldingLeftClick()
    {
        return this._playerControls.PlayerMap.LeftClick.inProgress;
    }

    private void ExecuteLeftClick(InputAction.CallbackContext context)
    {
        this.leftClickInitiated = true;
    }

    private void ExecuteRightClick(InputAction.CallbackContext context)
    {
        this.rightClickInitiated = true;
    }

    private void ExecuteSpace(InputAction.CallbackContext context)
    {
        this.spacePressInitiated = true;
    }

    private void ExecuteBack(InputAction.CallbackContext context)
    {
        this.backPressInitiated = true;
    }

    public void UpdateMouseSensitivity(float value)
    {
        PlayerControlsManager.mouseSensitivity = value;
    }

    public void ResetAllInitiatedInputs()
    {
        this.leftClickInitiated = false;
        this.rightClickInitiated = false;
        this.spacePressInitiated = false;
        this.backPressInitiated = false;
    }
}