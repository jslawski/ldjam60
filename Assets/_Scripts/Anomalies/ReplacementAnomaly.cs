using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementAnomaly : BaseAnomaly
{
    [SerializeField]
    private MeshRenderer _originalRenderer;

    [SerializeField]
    private MeshRenderer _anomalyRenderer;

    protected override void Awake()
    {
        base.Awake();

        this._originalRenderer.enabled = true;
        this._anomalyRenderer.enabled = false;
    }

    public override void Activate()
    {
        base.Activate();

        this._originalRenderer.enabled = false;
        this._anomalyRenderer.enabled = true;
        
    }

    public override void Deactivate()
    {
        base.Deactivate();

        this._originalRenderer.enabled = true;
        this._anomalyRenderer.enabled = false;
    }
}
