using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedAnomaly : MonoBehaviour
{
    [SerializeField]
    protected Collider _anomalyCollider;

    protected virtual void Awake()
    {
        this._anomalyCollider.enabled = false;
    }

    public virtual void StartSequence()
    { 
    
    }

    public virtual void Activate()
    {
        this._anomalyCollider.enabled = true;
    }

    public virtual void Deactivate()
    {
        this._anomalyCollider.enabled = false;
    }

    public virtual bool IsVisible()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);

        return (viewportPosition.x >= 0.0f && viewportPosition.x <= 1.0f && viewportPosition.y >= 0.0f && viewportPosition.y <= 1.0f && viewportPosition.z > 0.0f);
    }
}
