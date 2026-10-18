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

    public override void EnterState(PlayerCharacterController characterController)
    {
        base.EnterState(characterController);

        Camera.main.fieldOfView = this._defaultFOV;
        this._targetFOV = Camera.main.fieldOfView;

        this._zoomInTween = DOTween.To(() => this._targetFOV, x => this._targetFOV = x, this._finalFOV, this._timeTilFocusComplete);
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
            this.characterController.ChangeState(new MoveState());
        }
        else if (Camera.main.fieldOfView > this._finalFOV)
        {
            Camera.main.fieldOfView = this._targetFOV;
        }
        else
        {
            this.characterController.AttemptAnomalyFix();
            this.characterController.ChangeState(new MoveState());
        }
    }
}
