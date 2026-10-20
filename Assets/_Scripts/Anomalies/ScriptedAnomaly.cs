using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedAnomaly : MonoBehaviour
{    
    protected Collider _anomalyCollider;

    public bool isActive = false;

    protected virtual void Awake()
    {
        this._anomalyCollider = GetComponent<Collider>();
        this._anomalyCollider.enabled = false;
    }

    public virtual void StartSequence()
    { 
    
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
    }

    public virtual bool IsVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);

        return (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }
}
