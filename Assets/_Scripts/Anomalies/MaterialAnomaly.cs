using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnomaly : BaseAnomaly
{
    [SerializeField]
    private MeshRenderer _changingRenderer;    

    [SerializeField]
    private Material _defaultMaterial;
    [SerializeField]
    private Material _anomalyMaterial;

    public override void Activate()
    {
        base.Activate();
        this._changingRenderer.material = this._anomalyMaterial;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        this._changingRenderer.material = this._defaultMaterial;
    }
}
