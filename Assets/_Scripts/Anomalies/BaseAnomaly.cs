using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnomaly : MonoBehaviour
{
    [SerializeField]
    protected Collider _anomalyCollider;

    [HideInInspector]
    public bool isActive = false;

    protected virtual void Start()
    {
        this.SetupAnomalyCollider();

        this._anomalyCollider.enabled = false;
        this.isActive = false;
    }

    protected void SetupAnomalyCollider()
    {
        Collider[] allColliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in allColliders)
        {
            if (collider.tag == "Anomaly")
            {
                this._anomalyCollider = collider;
                break;
            }
        }
    }

    public virtual void Activate()
    {
        this._anomalyCollider.enabled = true;
        this.isActive = true;
    }

    public virtual void Deactivate()
    { 
        this._anomalyCollider.enabled = false;
        this.isActive = false;
        AnomalyManager.instance.ResolveAnomaly();
    }

    public virtual bool IsVisible()
    { 
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);

        return (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }

    public virtual bool IsAnomalyActive()
    { 
        BaseAnomaly[] allAnomalyComponents = GetComponents<BaseAnomaly>();

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
