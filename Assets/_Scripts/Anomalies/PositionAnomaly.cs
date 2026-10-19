using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAnomaly : BaseAnomaly
{
    [Header("Move an object from one position to another")]
    private Vector3 _defaultPosition;
    private Vector3 _defaultRotationEuler;

    [Header("This collider keeps track of where the object originally WAS")]
    [SerializeField]
    private Collider _oldPositionCollider;

    [Header("Set the position and rotation you want the object to move to")]
    [SerializeField]
    private Vector3 _anomalyPosition;
    [SerializeField]
    private Vector3 _anomalyRotation;

    protected override void Start()
    {
        base.Start();

        this._defaultPosition = this.transform.parent.position;
        this._defaultRotationEuler = this.transform.parent.rotation.eulerAngles;
        this._oldPositionCollider.enabled = false;
    }

    public override void Activate()
    {
        base.Activate();

        this.transform.parent.position = this._anomalyPosition;
        this.transform.parent.rotation = Quaternion.Euler(this._anomalyRotation);
        this._oldPositionCollider.enabled = true;
        this._oldPositionCollider.gameObject.transform.position = this._defaultPosition;
    }

    public override void Deactivate() 
    { 
        base.Deactivate();
        this.transform.parent.position = this._defaultPosition;
        this.transform.parent.rotation = Quaternion.Euler(this._defaultRotationEuler);
        this._oldPositionCollider.enabled = false;
    }
}
