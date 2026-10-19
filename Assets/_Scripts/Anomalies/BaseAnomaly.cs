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
        bool spherecastCheck = true;// this.SphereCastCheck();

        return (spherecastCheck) || (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }

    private bool SphereCastCheck()
    {
        Ray cameraRay = new Ray(Camera.main.transform.position - (Camera.main.transform.forward * 2.0f), Camera.main.transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(cameraRay, 1.0f, 0.2f);

        if (hits != null)
        {
            return this.CheckIsHit(hits);
        }
        else
        {
            return false;
        }
    }

    protected bool CheckIsHit(RaycastHit[] hits)
    { 
        foreach (RaycastHit hit in hits) 
        {
            BaseAnomaly[] anomalyComponents = hit.collider.gameObject.transform.parent.gameObject.GetComponentsInChildren<BaseAnomaly>();

            foreach (BaseAnomaly anomaly in anomalyComponents)
            {
                if (anomaly.enabled == true)
                {
                    Debug.LogError("Anomaly is in field of view!");
                    return true;
                }
            }
        }

        return false;
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
