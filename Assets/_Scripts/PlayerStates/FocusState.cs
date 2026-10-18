using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusState : PlayerState
{
    private float _timeTilFocusComplete = 3.0f;

    private float _defaultFOV = 60.0f;
    private float _finalFOV = 40.0f;

    private float _targetFOV;

    private Tweener _zoomInTween;
    private Tweener _zoomOutTween;

    private float _doorOpenBuffer = 0.5f;

    private int _originalLookObjectCount = 0;

    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Camera.main.fieldOfView = this._defaultFOV;
        this._targetFOV = Camera.main.fieldOfView;

        this._zoomInTween = DOTween.To(() => this._targetFOV, x => this._targetFOV = x, this._finalFOV, this._timeTilFocusComplete);

        this._originalLookObjectCount = this.characterController.lookObjectStack.Count;
    }

    public override void ExitState()
    {
        base.ExitState();
        
        float currentFieldOfView = Camera.main.fieldOfView;
        
        this._zoomOutTween = DOTween.To(() => currentFieldOfView, x => currentFieldOfView = x, this._defaultFOV, 0.2f).OnUpdate(() =>
        {
            Camera.main.fieldOfView = currentFieldOfView;
        });
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
            Camera.main.fieldOfView = this._targetFOV;
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
