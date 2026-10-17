using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    private struct RaycastHitCheck
    {
        public bool success;
        public RaycastHit hitInfo;
    }

    private Transform _cameraTransform;
    private Rigidbody _playerRigidbody;

    private Vector3 _moveDirection = Vector3.zero;
    private float _moveAcceleration = 5.0f;
    public float _maxMoveVelocity = 15.0f;
    private Coroutine _moveLerpCoroutine = null;

    private Quaternion _targetRotation = Quaternion.identity;
    private float _cameraRotateSpeed = 12.0f;
    private Coroutine _cameraSlerpCoroutine = null;

    private float _maxVerticalAngle = 90.0f;

    [SerializeField]
    private GameObject _stickyNotePrefab;
    public GameObject currentStickyNote;
    private float _stickyNoteZDistanceFromCamera = 0.25f;

    [SerializeField]
    private GameObject _discardedStickyNotePrefab;
    private Vector3 _discardedNoteOffsetPosition = new Vector3(-0.5f, -0.5f, 0.3f);

    public PlayerState _currentState;

    private GameObject _heldObject;
    private Vector3 _cachedObjectPosition;
    private Quaternion _cachedObjectRotation;

    [SerializeField]
    private LayerMask _raycastLayer;
    private RaycastHitCheck _hitCheck = new RaycastHitCheck();
    private float _raycastMaxDistance = 3.0f;

    private float _placementOffset = 0.0011f;

    private void Awake()
    {
        this._cameraTransform = Camera.main.transform;
        this._playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        this.ChangeState(new MoveState());
    }

    private void Update()
    {        
        this._currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        this._currentState.FixedUpdateState();    
    }

    public void ChangeState(PlayerState newState)
    {
        if (this._currentState != null)
        {
            this._currentState.ExitState();
        }
            
        this._currentState = newState;
        this._currentState.EnterState(this);

        Debug.LogError("New State: " + this._currentState.GetType().Name);
    }

    private float GetClampedXAngle()
    {
        float xRotation = this._cameraTransform.rotation.eulerAngles.x + PlayerControlsManager.instance.lookDelta.x;

        if (xRotation > 180.0f)
        {
            xRotation -= 360.0f;
        }

        xRotation = Mathf.Clamp(xRotation, -this._maxVerticalAngle, this._maxVerticalAngle);

        return xRotation;
    }

    public void UpdateCameraRotation()
    {
        Quaternion yRotation = Quaternion.AngleAxis(PlayerControlsManager.instance.lookDelta.y, Vector3.up);
        Quaternion xRotation = Quaternion.Euler(this.GetClampedXAngle(), this._cameraTransform.rotation.eulerAngles.y, 0.0f);

        this._targetRotation = yRotation * xRotation;

        this._cameraTransform.rotation = this._targetRotation;

        return;

        if (this._cameraSlerpCoroutine != null)
        {
            StopCoroutine(this._cameraSlerpCoroutine);
        }

        this._cameraSlerpCoroutine = StartCoroutine(this.LerpToTargetRotation());
    }

    private IEnumerator LerpToTargetRotation()
    {
        while (this._cameraTransform.rotation != this._targetRotation)
        {
            this._cameraTransform.rotation = Quaternion.Lerp(this._cameraTransform.rotation, this._targetRotation, this._cameraRotateSpeed * Time.fixedDeltaTime);

            if (this._cameraTransform.rotation.eulerAngles.y > 360)
            {
                this._cameraTransform.rotation = Quaternion.Euler(this._cameraTransform.rotation.eulerAngles.x, (this._cameraTransform.rotation.eulerAngles.y - 360.0f), this._cameraTransform.rotation.eulerAngles.z);
            }
            else if (this._cameraTransform.rotation.eulerAngles.y < -360)
            {
                this._cameraTransform.rotation = Quaternion.Euler(this._cameraTransform.rotation.eulerAngles.x, (this._cameraTransform.rotation.eulerAngles.y + 360.0f), this._cameraTransform.rotation.eulerAngles.z);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void UpdateLateralVelocity()
    {
        Vector3 lateralVelocity = this._moveDirection * this._maxMoveVelocity;

        if (this._moveLerpCoroutine != null)
        {
            StopCoroutine(this._moveLerpCoroutine);
        }

        this._moveLerpCoroutine = StartCoroutine(this.LerpToTargetLateralVelocity(lateralVelocity));
    }

    private IEnumerator LerpToTargetLateralVelocity(Vector3 lateralVelocity)
    {
        Vector3 targetVelocity = new Vector3(lateralVelocity.x, this._playerRigidbody.velocity.y, lateralVelocity.z);

        int iteration = 0;

        while (this._playerRigidbody.velocity != targetVelocity)
        {
            this._playerRigidbody.velocity = Vector3.Lerp(this._playerRigidbody.velocity, targetVelocity, this._moveAcceleration * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
            iteration++;
        }
    }

    public void UpdateLatestMoveDirection()
    {
        Vector3 returnVector = Vector3.zero;

        if (PlayerControlsManager.instance.IsPressingForward() == true)
        {
            returnVector += new Vector3(this._cameraTransform.forward.x, 0.0f, this._cameraTransform.forward.z);
        }

        if (PlayerControlsManager.instance.IsPressingBack() == true)
        {
            returnVector -= new Vector3(this._cameraTransform.forward.x, 0.0f, this._cameraTransform.forward.z);
        }

        if (PlayerControlsManager.instance.IsPressingLeft() == true)
        {
            returnVector -= this._cameraTransform.right;
        }

        if (PlayerControlsManager.instance.IsPressingRight() == true)
        {
            returnVector += this._cameraTransform.right;
        }

        this._moveDirection = returnVector.normalized;
    }

    public void StopMovement()
    {
        this._moveDirection = Vector3.zero;
        
        if (this._moveLerpCoroutine != null)
        {
            StopCoroutine(this._moveLerpCoroutine);
            this._moveLerpCoroutine = null;
        }

        this._playerRigidbody.velocity = Vector3.zero;
    }

    public void InstantiateStickyNote()
    {
        Vector3 instantiationPosition = this._cameraTransform.position + (this._cameraTransform.forward * this._stickyNoteZDistanceFromCamera);
        Quaternion instantiationRotation = Quaternion.Euler(this._cameraTransform.rotation.eulerAngles.x, this._cameraTransform.rotation.eulerAngles.y, this._cameraTransform.rotation.eulerAngles.z);
        
        this.currentStickyNote = Instantiate(this._stickyNotePrefab, instantiationPosition, instantiationRotation);

        this.HoldObject(this.currentStickyNote);
    }

    public void HoldObject(GameObject targetObject)
    {
        this._heldObject = targetObject;
    
        this._cachedObjectPosition = targetObject.transform.position;
        this._cachedObjectRotation = targetObject.transform.rotation;
    
        Vector3 targetPosition = this._cameraTransform.position + (this._cameraTransform.forward * this._stickyNoteZDistanceFromCamera);
        Quaternion targetRotation = this._cameraTransform.rotation;

        targetObject.transform.DOMove(targetPosition, 0.4f).SetEase(Ease.OutExpo);
        targetObject.transform.DORotateQuaternion(targetRotation, 0.4f);
    }

    public void PutObjectBack()
    {
        this._heldObject.transform.DOMove(this._cachedObjectPosition, 0.4f).SetEase(Ease.OutExpo);
        this._heldObject.transform.DORotateQuaternion(this._cachedObjectRotation, 0.4f);

        this._heldObject = null;
        this._cachedObjectPosition = Vector3.zero;
        this._cachedObjectRotation = Quaternion.identity;
    }

    public void AttemptPostStickyNote()
    {
        if (this._hitCheck.success == true)
        {
            this.currentStickyNote.layer = LayerMask.NameToLayer("PostedNote");
            this.currentStickyNote = null;
        }

        this._heldObject = null;
    }

    public void UpdateStickyNoteCurrentPosition()
    {
        this.CastCameraRay();

        if (this._hitCheck.success == true)
        {
            this.currentStickyNote.transform.position = this._hitCheck.hitInfo.point + (this._hitCheck.hitInfo.normal * this._placementOffset);
            this.currentStickyNote.transform.forward = -this._hitCheck.hitInfo.normal;
        }
        else
        {
            this.currentStickyNote.transform.position = this._cameraTransform.position + (this._cameraTransform.forward * this._raycastMaxDistance);
            this.currentStickyNote.transform.forward = this._cameraTransform.forward;
        }
    }

    public void DiscardStickyNote()
    {
        Destroy(this.currentStickyNote);
        this.currentStickyNote = null;

        Vector3 offsetVector = (-this._cameraTransform.right * this._discardedNoteOffsetPosition.x) + 
                               (this._cameraTransform.up * this._discardedNoteOffsetPosition.y) + 
                               (this._cameraTransform.forward * this._discardedNoteOffsetPosition.z);


        Vector3 spawnPosition = this._cameraTransform.position + offsetVector;
        Instantiate(this._discardedStickyNotePrefab, spawnPosition, this._cameraTransform.rotation);
    }

    private void CastCameraRay()
    {
        Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        this._hitCheck = new RaycastHitCheck();


        if (Physics.Raycast(cameraRay, out _hitCheck.hitInfo, this._raycastMaxDistance, this._raycastLayer) == true)
        {
            this._hitCheck.success = true;
        }
        else
        {
            this._hitCheck.success = false;
        }
    }
}
