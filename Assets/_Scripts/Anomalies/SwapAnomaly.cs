using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SwapAnomaly : BaseAnomaly
{
    [SerializeField]
    private SwapAnomaly _otherSwapAnomaly;

    public override void Activate()
    {
        base.Activate();

        Vector3 targetPosition = this._otherSwapAnomaly.transform.position;
        Quaternion targetRotation = this._otherSwapAnomaly.transform.rotation;

        if (this._otherSwapAnomaly.isActive == false)
        {
            this._otherSwapAnomaly.Activate();
        }

        this.transform.position = targetPosition;
        this.transform.rotation = targetRotation;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Vector3 targetPosition = this._otherSwapAnomaly.transform.position;
        Quaternion targetRotation = this._otherSwapAnomaly.transform.rotation;

        if (this._otherSwapAnomaly.isActive == true)
        {
            this._otherSwapAnomaly.Deactivate();
        }

        this.transform.position = targetPosition;
        this.transform.rotation = targetRotation;
    }

    public override bool IsVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);

        bool isThisObjectVisible = (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
        bool isSwapObjectVisible = this.IsSwapObjectVisible();

        return (isThisObjectVisible || isSwapObjectVisible);
    }

    private bool IsSwapObjectVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this._otherSwapAnomaly.gameObject.transform.position);
        return (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }

    public override bool IsAnomalyActive()
    {
        bool thisObjectAnomalyActive = false;
        bool swapObjectAnomalyActive = this.IsSwapObjectAnomalyActive();

        BaseAnomaly[] allAnomalyComponents = GetComponentsInChildren<BaseAnomaly>();

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
        BaseAnomaly[] allAnomalyComponents = this._otherSwapAnomaly.gameObject.GetComponentsInChildren<BaseAnomaly>();

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
