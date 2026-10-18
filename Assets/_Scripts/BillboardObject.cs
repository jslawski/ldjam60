using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
    }
}
