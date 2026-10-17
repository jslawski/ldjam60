using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardedNote : MonoBehaviour
{
    private Vector3 _discardDirection;

    private Rigidbody _noteRigidbody;

    [SerializeField]
    private float _discardForce = 5.0f;

    private float _aliveTime = 0.0f;

    private void Awake()
    {
        this._discardDirection = (this.transform.up + this.transform.forward - this.transform.right).normalized;
        this._noteRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this._noteRigidbody.AddForce(this._discardDirection * this._discardForce, ForceMode.Impulse);
    }
    
    void FixedUpdate()
    {
        if (this._aliveTime < 0.5f)
        {
            this._aliveTime += Time.fixedDeltaTime;
            return;
        }
    
        if (this._noteRigidbody != null)
        {
            if (this._noteRigidbody.velocity.magnitude < 0.01f)
            {
                Destroy(this._noteRigidbody);
                this._noteRigidbody = null;
            }
        }
    }
}
