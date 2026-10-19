using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusState : PlayerState
{
    private float _timeTilFocusComplete = 3.0f;

    private Vector3 defaultPosition;
    private float _defaultFOV = 60.0f;
    private float _finalFOV = 40.0f;

    private Vector3 _finalPosition;

    private float _targetFOV;
    private Vector3 _targetPosition;

    private Tweener _zoomInTween;
    private Tweener _zoomOutTween;

    private float _doorOpenBuffer = 2.8f;

    private int _originalLookObjectCount = 0;

    private float _currentValue = 0;

    private int oldLookStackCount = 0;

    private Vector3 _defaultLocalPosition = new Vector3(0.0f, 0.465f, 0.281f);

    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);
        
        Camera.main.fieldOfView = this._defaultFOV;
        this._targetFOV = Camera.main.fieldOfView;
        this._targetPosition = Camera.main.transform.position;

        this.defaultPosition = Camera.main.transform.position;

        this.oldLookStackCount = this.characterController.lookObjectStack.Count;

        if (this.characterController.lookObjectStack.Count > 0)
        {
            Vector3 mousePosition = Input.mousePosition;            
            mousePosition.z = 1.5f;            

            this._finalPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            this._finalPosition.z = Camera.main.transform.position.z;            
        }

        this._zoomInTween = DOTween.To(() => this._currentValue, x => this._currentValue = x, 1.0f, this._timeTilFocusComplete);

        this._originalLookObjectCount = this.characterController.lookObjectStack.Count;
    }

    public override void ExitState()
    {
        base.ExitState();
        
        float currentFieldOfView = Camera.main.fieldOfView;

        Vector3 currentPosition = Camera.main.transform.position;    

        DOTween.To(() => currentFieldOfView, x => currentFieldOfView = x, this._defaultFOV, 0.2f).OnUpdate(() =>
        {
            Camera.main.fieldOfView = currentFieldOfView;
        });

        if (this.characterController.lookObjectStack.Count == this.oldLookStackCount)
        {
            Camera.main.transform.DOLocalMove(this._defaultLocalPosition, 0.2f);
        }
    }

    public override void UpdateState()
    {               
        if (PlayerControlsManager.instance.IsHoldingLeftClick() == false)
        {
            this._zoomInTween.Kill();

            if (this._doorOpenBuffer > 0.0f)
            {
                this.characterController.TryGetLookObject();

                if (this.characterController.lookObjectStack.Count > this._originalLookObjectCount)
                {
                    this.characterController.ChangeState(new LookState());
                    this.characterController.EnterLookTransform();
                }
                else
                {
                    this.characterController.TryInteract();
                    this.RevertToPreviousState();
                }
            }
            else
            {
                this.RevertToPreviousState();
            }
        }
        else if (Camera.main.fieldOfView > this._finalFOV)
        {
            //Camera.main.fieldOfView = this._targetFOV;

            Camera.main.fieldOfView = Mathf.Lerp(this._targetFOV, this._finalFOV, this._currentValue);
            
            if (this.characterController.lookObjectStack.Count > 0)
            {
                Camera.main.transform.position = Vector3.Lerp(this.defaultPosition, this._finalPosition, this._currentValue);
            }            

            this._doorOpenBuffer -= Time.deltaTime;
        }
        else
        {
            this.characterController.AttemptAnomalyFix();
            this.RevertToPreviousState();
        }
    }

    private void RevertToPreviousState()
    {
        if (this.characterController.lookObjectStack.Count > 0)
        {
            this.characterController.ChangeState(new LookState());
        }
        else
        {
            this.characterController.ChangeState(new MoveState());
        }
    }
}
