using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

    private float _maxVerticalAngle = 90.0f;

    [SerializeField]
    private GameObject _stickyNotePrefab;
    [HideInInspector]
    public GameObject currentStickyNote;
    private float _stickyNoteZDistanceFromCamera = 0.25f;
    [HideInInspector]
    public Vector3 stickyNoteTargetPosition = Vector3.zero;
    [HideInInspector]
    public Vector3 stickyNoteTargetForward = Vector3.zero;

    [SerializeField]
    private GameObject _discardedStickyNotePrefab;
    private Vector3 _discardedNoteOffsetPosition = new Vector3(-0.5f, -0.5f, 0.3f);

    public PlayerState _currentState;

    private GameObject _heldObject;
    private Vector3 _cachedObjectPosition;
    private Quaternion _cachedObjectRotation;

    [SerializeField]
    private LayerMask _raycastLayer;
    [SerializeField]
    private LayerMask _postedNoteLayer;
    [SerializeField]
    private LayerMask _lookObjectLayer;
    [SerializeField]
    private LayerMask _anomalyCheckLayer;

    private RaycastHitCheck _hitCheck = new RaycastHitCheck();
    private float _raycastMaxDistance = 3.0f;
    private float _closeRaycastDistance = 2.0f;
    private float _lookRaycastDistance = 100.0f;

    private float _placementOffset = 0.0011f;

    [SerializeField]
    private GameObject _hudCanvas;

    [HideInInspector]
    public Stack<LookObject> lookObjectStack;
    [HideInInspector]
    public Vector3 _cachedPlayerPosition;
    [HideInInspector]
    public Quaternion _cachedPlayerRotation;

    [SerializeField]
    public Image _fadeImage;

    public TextSequence currentTextSequence;

    public Ray lastRay;

    private void Awake()
    {
        this._cameraTransform = Camera.main.transform;
        this._playerRigidbody = GetComponent<Rigidbody>();

        this.lookObjectStack = new Stack<LookObject>();
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
        if (this.currentStickyNote != null)
        {
            return;
        }

        Vector3 instantiationPosition = this._cameraTransform.position + (this._cameraTransform.forward * this._stickyNoteZDistanceFromCamera) - this._cameraTransform.up;
        Quaternion instantiationRotation = Quaternion.Euler(this._cameraTransform.rotation.eulerAngles.x, this._cameraTransform.rotation.eulerAngles.y, this._cameraTransform.rotation.eulerAngles.z);
        
        this.currentStickyNote = Instantiate(this._stickyNotePrefab, instantiationPosition, instantiationRotation);

        this.HoldObject(this.currentStickyNote);
    }

    public void HoldObject(GameObject targetObject)
    {
        if (this._heldObject != null)
        {
            this.PutObjectBack();
        }

        this._heldObject = targetObject;
    
        this._cachedObjectPosition = targetObject.transform.position;
        this._cachedObjectRotation = targetObject.transform.rotation;
    
        Vector3 targetPosition = this._cameraTransform.position + (this._cameraTransform.forward * this._stickyNoteZDistanceFromCamera);
        Quaternion targetRotation = this._cameraTransform.rotation;

        //targetObject.transform.position = targetPosition;
        //targetObject.transform.rotation = targetRotation;

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
        this.UpdateStickyNoteCurrentPosition();

        if (this._hitCheck.success == true)
        {
            this.currentStickyNote.transform.DOKill();
            this.currentStickyNote.GetComponent<StickyNote>().PostNote(this.stickyNoteTargetPosition, this.stickyNoteTargetForward);
            this.currentStickyNote = null;
            this._heldObject = null;
        }
    }

    public void UpdateStickyNoteCurrentPosition()
    {
        if (this.lookObjectStack.Count > 0)
        {
            this.CastMouseRay(this._raycastLayer);
        }
        else
        {
            this.CastCameraRay(this._raycastLayer, this._raycastMaxDistance);
        }
        
        if (this._hitCheck.success == true)
        {
            this.stickyNoteTargetPosition = this._hitCheck.hitInfo.point + (this._hitCheck.hitInfo.normal * this._placementOffset);
            this.stickyNoteTargetForward = -this._hitCheck.hitInfo.normal;
        }
        else
        {
            this.stickyNoteTargetPosition = this._cameraTransform.position + (this._cameraTransform.forward * this._raycastMaxDistance);
            this.stickyNoteTargetForward = this._cameraTransform.forward;
        }

        this.currentStickyNote.transform.position = this.stickyNoteTargetPosition;
        this.currentStickyNote.transform.forward = this.stickyNoteTargetForward;
    }

    public void DiscardStickyNote()
    {
        this.currentStickyNote.transform.DOKill();

        Destroy(this.currentStickyNote);
        
        this.currentStickyNote = null;

        Vector3 offsetVector = (-this._cameraTransform.right * this._discardedNoteOffsetPosition.x) + 
                               (this._cameraTransform.up * this._discardedNoteOffsetPosition.y) + 
                               (this._cameraTransform.forward * this._discardedNoteOffsetPosition.z);


        Vector3 spawnPosition = this._cameraTransform.position + offsetVector;
        Instantiate(this._discardedStickyNotePrefab, spawnPosition, this._cameraTransform.rotation);
    }

    public void TryPickupStickyNote()
    {
        if (this.lookObjectStack.Count > 0)
        {
            this.CastMouseRay(this._postedNoteLayer);
        }
        else
        {
            this.CastCameraRay(this._postedNoteLayer, this._raycastMaxDistance);
        }
        
        if (this._hitCheck.success == true && this.currentStickyNote == null)
        {
            this.currentStickyNote = this._hitCheck.hitInfo.collider.gameObject;
            this.currentStickyNote.GetComponent<StickyNote>().PrepareToDraw();
            this.HoldObject(this._hitCheck.hitInfo.collider.gameObject);
        }        
    }

    public void TryGetLookObject()
    {
        if (this.lookObjectStack.Count > 0)
        {
            this.CastMouseRay(this._lookObjectLayer);
        }
        else 
        {
            this.CastCameraRay(this._lookObjectLayer, this._closeRaycastDistance);
        }

        LookObject latestLookObject = null;

        if (this.lookObjectStack.Count > 0)
        {
            latestLookObject = this.lookObjectStack.Peek();
        }
        
        if (this._hitCheck.success == true)
        {
            LookObject newLookObject = this._hitCheck.hitInfo.collider.gameObject.GetComponentInParent<LookObject>();
            if (latestLookObject != newLookObject)
            {
                this.lookObjectStack.Push(newLookObject);
            }
        }
    }

    public void TryInteract()
    {
        if (this.lookObjectStack.Count > 0)
        {
            this.CastMouseRay(this._raycastLayer);
        }
        else
        {
            this.CastCameraRay(this._raycastLayer, this._closeRaycastDistance);
        }

        if (this._hitCheck.success == true)
        {
            if (this._hitCheck.hitInfo.collider.tag == "Interact")
            {
                this._hitCheck.hitInfo.collider.gameObject.GetComponent<InteractObject>().Interact();
            }            
        }
    }

    private void CastCameraRay(LayerMask targetLayer, float raycastDistance, bool useLastRay = false)
    {
        Ray cameraRay = new Ray(Camera.main.transform.position - (Camera.main.transform.forward * 2.0f), Camera.main.transform.forward);
        this._hitCheck = new RaycastHitCheck();

        if (useLastRay == true)
        {
            cameraRay = lastRay;
        }



        this.lastRay = cameraRay;

        if (Physics.Raycast(cameraRay, out this._hitCheck.hitInfo, raycastDistance, targetLayer) == true)
        {
            this._hitCheck.success = true;
        }
        else
        {
            this._hitCheck.success = false;
        }
    }

    private void CastMouseRay(LayerMask targetLayer, bool useLastRay = false)
    {
        Vector3 mouseWorldPosition = Input.mousePosition;
        //mouseWorldPosition.z = 2.0f;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseWorldPosition);

        //Debug.DrawLine(mouseWorldPosition, Camera.main.transform.forward * 100.0f, Color.red, 5.0f);

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (useLastRay == true)
        {
            mouseRay = lastRay;
        }

        Debug.DrawRay(mouseRay.origin, mouseRay.direction * 100.0f, Color.red, 5.0f);

        this.lastRay = mouseRay;

        if (Physics.Raycast(mouseRay, out this._hitCheck.hitInfo, 100.0f, targetLayer) == true)
        {
            this._hitCheck.success = true;
        }
        else
        {
            this._hitCheck.success = false;
        }
    }

    public void AttemptAnomalyFix()
    {
        
        if (this.lookObjectStack.Count > 0)
        {
            this.CastMouseRay(this._anomalyCheckLayer, true);    
        }
        else
        {
            this.CastCameraRay(this._anomalyCheckLayer, 10.0f, true);
        }
        



        if (this._hitCheck.success == true)
        {
            if (this._hitCheck.hitInfo.collider.tag == "Anomaly")
            {
                this.FixAnomaly(this._hitCheck.hitInfo.collider.gameObject);        
            }
        }
    }

    private void FixAnomaly(GameObject targetObject)
    {
        BaseAnomaly[] anomalyComponents = targetObject.GetComponentsInParent<BaseAnomaly>();

        foreach (BaseAnomaly anomaly in anomalyComponents)
        {
            if (anomaly.isActive == true)
            {
                anomaly.Deactivate();
            }
        }
    }

    public void EnterLookTransform()
    {
        if (this.lookObjectStack.Count <= 0)
        {
            Debug.LogError("No current look objects");
            return;
        }

        this._fadeImage.DOColor(Color.black, 0.2f).OnComplete(this.EnterLookObjectCallback);
    }

    private void EnterLookObjectCallback()
    {
        LookObject latestLookObject = this.lookObjectStack.Peek();

        if (this.lookObjectStack.Count == 1)
        {
            this._cachedPlayerPosition = this.transform.position;
            this._cachedPlayerRotation = this._cameraTransform.rotation;//Quaternion.Euler(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up));
        }

        this.transform.position = latestLookObject.lookScenePosition;
        this._cameraTransform.rotation = Quaternion.identity;

        this._fadeImage.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), 0.2f);
    }

    public void ExitLookTransform()
    {
        this._fadeImage.DOColor(Color.black, 0.2f).OnComplete(this.ExitLookObjectCallback);
    }

    private void ExitLookObjectCallback()
    {
        this.lookObjectStack.Pop();

        if (this.lookObjectStack.Count > 0)
        {
            Vector3 previousPosition = this.lookObjectStack.Peek().lookScenePosition;
            this.transform.position = previousPosition;
        }
        else
        {
            this.ReturnToHouse();
        }

        this._fadeImage.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), 0.2f);
    }

    private void ReturnToHouse()
    {
        this.transform.position = this._cachedPlayerPosition;
        this._cameraTransform.rotation = this._cachedPlayerRotation;        

        this._cachedPlayerPosition = Vector3.zero;
        this._cachedPlayerRotation = Quaternion.identity;

        this.ChangeState(new MoveState());
    }

    public void EnableHUD()
    {
        this._hudCanvas.SetActive(true);
    }

    public void DisableHUD()
    {
        this._hudCanvas.SetActive(false);
    }
}
