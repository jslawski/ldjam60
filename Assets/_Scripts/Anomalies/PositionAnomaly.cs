using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAnomaly : BaseAnomaly
{        
    private Vector3 _defaultPosition;
    private Vector3 _defaultRotationEuler;

    [SerializeField]
    private Collider _oldPositionCollider;

    [SerializeField]
    private Vector3 _anomalyPosition;
    [SerializeField]
    private Vector3 _anomalyRotation;

    protected override void Awake()
    {
        base.Awake();

        this._defaultPosition = this.transform.position;
        this._defaultRotationEuler = this.transform.rotation.eulerAngles;
        this._oldPositionCollider.enabled = false;
    }

    public override void Activate()
    {
        base.Activate();

        this.transform.position = this._anomalyPosition;
        this.transform.rotation = Quaternion.Euler(this._anomalyRotation);
        this._oldPositionCollider.enabled = true;
    }

    public override void Deactivate() 
    { 
        base.Deactivate();
        this.transform.position = this._defaultPosition;
        this.transform.rotation = Quaternion.Euler(this._defaultRotationEuler);
        this._oldPositionCollider.enabled = false;
    }
}
