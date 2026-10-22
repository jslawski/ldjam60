using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DioramaObject : MonoBehaviour
{
    private float _maxDistanceFromCamera;    

    [Range(0.05f, 1.0f)]
    public float normalizedDistanceFromCamera;

    private Vector3 _originalScale;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        this._originalScale = transform.localScale;
        this._originalPosition = transform.localPosition;
        this._maxDistanceFromCamera = this.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        //float normalizedScalar = this.GetNormalizedScalar();

        this.UpdateScale();
        this.UpdatePosition();

    }

    private float GetNormalizedScalar()
    {
        float totalFrustumDistance = this._maxDistanceFromCamera - Camera.main.nearClipPlane;
        float realFrustumDistance = this.normalizedDistanceFromCamera * totalFrustumDistance;

        float normalizedDistance = (realFrustumDistance - Camera.main.nearClipPlane) / (this._maxDistanceFromCamera - Camera.main.nearClipPlane);

        return normalizedDistance;
    }

    private float GetNewZPosition()
    {
        float totalFrustumDistance = this._maxDistanceFromCamera - Camera.main.nearClipPlane;
        float realFrustumDistance = this.normalizedDistanceFromCamera * totalFrustumDistance;

        return Camera.main.transform.position.z + Camera.main.nearClipPlane +  realFrustumDistance;
    }

    private void UpdateScale()
    {
        this.transform.localScale = this._originalScale * this.normalizedDistanceFromCamera;
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = this._originalPosition;
        newPosition.x *= this.normalizedDistanceFromCamera;
        newPosition.y *= this.normalizedDistanceFromCamera;
        newPosition.z = GetNewZPosition();

        this.transform.localPosition = newPosition;

    }
}
