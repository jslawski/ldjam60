using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingExtraAnomaly : BaseAnomaly
{
    [Header("Make an object disappear, or an extra object appear")]
    private MeshRenderer[] _allRenderers;

    [Header("Enable this if the anomaly makes an object disappear.\n" +
    "Disable this if the anomaly is making an extra object appear.")]
    [SerializeField]
    private bool _isMissingAnomaly = false;

    protected override void Start()
    {
        base.Start();

        this._allRenderers = this.transform.parent.gameObject.GetComponentsInChildren<MeshRenderer>();

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

        if (this._isMissingAnomaly == true)
        {
            this.DisableAllRenderers();
        }
        else
        {
            this.EnableAllRenderers();
        }
        
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (this._isMissingAnomaly == true)
        {
            this.EnableAllRenderers();
        }
        else
        {
            this.DisableAllRenderers();
        }

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
