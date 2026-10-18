using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingExtraAnomaly : BaseAnomaly
{
    private MeshRenderer[] _allRenderers;

    [SerializeField]
    private bool _isMissingAnomaly = false;

    protected override void Awake()
    {
        base.Awake();

        this._allRenderers = GetComponentsInChildren<MeshRenderer>();

        if (this._isMissingAnomaly)
        {
            this.EnableAllRenderers();
        }
        else
        {
            this.DisableAllRenderers();
        }
    }

    public override void Activate()
    {
        base.Activate();
        this.DisableAllRenderers();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        this.EnableAllRenderers();
    }

    private void EnableAllRenderers()
    { 
        foreach(MeshRenderer renderer in this._allRenderers) 
        {
            renderer.enabled = true;
        }
    }

    private void DisableAllRenderers()
    {
        foreach (MeshRenderer renderer in this._allRenderers)
        {
            renderer.enabled = false;
        }
    }
}
