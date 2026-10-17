using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
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

    private void Awake()
    {
        this._cameraTransform = Camera.main.transform;
        this._playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this._moveDirection = this.GetLatestMoveDirection();
    }

    private void FixedUpdate()
    {
        this.UpdateCameraRotation();

        this.UpdateLateralVelocity();
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

    private void UpdateCameraRotation()
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

    private void UpdateLateralVelocity()
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

    private Vector3 GetLatestMoveDirection()
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

        return returnVector.normalized;
    }
}
