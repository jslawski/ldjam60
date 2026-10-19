using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SwapAnomaly : BaseAnomaly
{
    [Header("Swap places between two objects in the scene.\n" +
    "Drag the base node of a GameObject here to 'link' the two objects.")]
    [SerializeField]
    private GameObject _objectSwapTarget;

    private SwapAnomaly _otherSwapAnomaly;

    protected override void Start()
    {
        base.Start();

        this._otherSwapAnomaly = this._objectSwapTarget.GetComponentInChildren<SwapAnomaly>();
    }

    public override void Activate()
    {
        base.Activate();

        Vector3 targetPosition = this._otherSwapAnomaly.transform.parent.position;
        Quaternion targetRotation = this._otherSwapAnomaly.transform.parent.rotation;

        if (this._otherSwapAnomaly.isActive == false)
        {
            this._otherSwapAnomaly.Activate();
        }

        this.transform.parent.position = targetPosition;
        this.transform.parent.rotation = targetRotation;
    }

    public override void Deactivate()
    {
        this._anomalyCollider.enabled = false;
        this.isActive = false;

        Vector3 targetPosition = this._otherSwapAnomaly.transform.parent.position;
        Quaternion targetRotation = this._otherSwapAnomaly.transform.parent.rotation;

        if (this._otherSwapAnomaly.isActive == true)
        {
            AnomalyManager.instance.ResolveAnomaly(); 
            this._otherSwapAnomaly.Deactivate();            
        }

        this.transform.parent.position = targetPosition;
        this.transform.parent.rotation = targetRotation;
    }

    public override bool IsVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.parent.position);

        bool isThisObjectVisible = (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
        bool isSwapObjectVisible = this.IsSwapObjectVisible();

        return (isThisObjectVisible || isSwapObjectVisible);
    }

    private bool IsSwapObjectVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this._otherSwapAnomaly.gameObject.transform.parent.position);
        return (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }

    public override bool IsAnomalyActive()
    {
        bool thisObjectAnomalyActive = false;
        bool swapObjectAnomalyActive = this.IsSwapObjectAnomalyActive();

        BaseAnomaly[] allAnomalyComponents = GetComponents<BaseAnomaly>();

        foreach (BaseAnomaly anomalyComponent in allAnomalyComponents)
        {
            if (anomalyComponent.isActive == true)
            {
                thisObjectAnomalyActive = true;
            }
        }

        return (thisObjectAnomalyActive || swapObjectAnomalyActive);
    }

    private bool IsSwapObjectAnomalyActive()
    {
        BaseAnomaly[] allAnomalyComponents = this._otherSwapAnomaly.gameObject.GetComponents<BaseAnomaly>();

        foreach (BaseAnomaly anomalyComponent in allAnomalyComponents)
        {
            if (anomalyComponent.isActive == true)
            {
                return true;
            }
        }

        return false;
    }
}
