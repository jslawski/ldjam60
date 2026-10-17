using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNote : MonoBehaviour
{
    [SerializeField]
    private Transform _reticleTransform;
    [SerializeField]
    private GameObject _lineObjectPrefab;
    private LineRenderer _currentLine;

    private RaycastHit _hitInfo;
    [SerializeField]
    private LayerMask _noteLayerMask;

    private float _minDistanceBetweenPoints = 0.005f;

    private float _offset = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControlsManager.instance.characterController._currentState.GetType() != typeof(DrawState))
        {
            return;
        }

        if (PlayerControlsManager.instance.IsHoldingLeftClick() == true)
        {
            if (this.IsMouseOnNote() == true)
            {
                if (this._currentLine == null)
                {
                    this.StartNewLine();
                    this.DrawPoint();
                }
                else
                {
                    this.DrawPoint();
                }
            }
            else
            {
                this.EndLine();
            }
        }
        else
        {
            this.EndLine();
        }
    }



    private void StartNewLine()
    {
        GameObject newLineObject = Instantiate(this._lineObjectPrefab, this.transform);
        this._currentLine = newLineObject.GetComponent<LineRenderer>();
        this._currentLine.numCapVertices = 5;
    }

    private void EndLine()
    {
        this._currentLine = null;
    }

    private void DrawPoint()
    {           
        this._reticleTransform.position = this._hitInfo.point + (this._hitInfo.normal * this._offset);

        if (this._currentLine.positionCount == 0 || Vector3.Distance(this._reticleTransform.localPosition, this._currentLine.GetPosition(this._currentLine.positionCount - 1)) > this._minDistanceBetweenPoints)
        {
            this._currentLine.positionCount++;
            this._currentLine.SetPosition(this._currentLine.positionCount - 1, this._reticleTransform.localPosition);
        }    
    }

    private bool IsMouseOnNote()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out this._hitInfo, 100.0f, this._noteLayerMask) == true)
        {
            if (this._hitInfo.collider.gameObject == this.gameObject)
            { 
                return true;
            }      
        }

        return false;

        /*
        Vector3 mouseWorldPosition = Input.mousePosition;
        mouseWorldPosition.z = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseWorldPosition);

        Debug.DrawLine(mouseWorldPosition, Camera.main.transform.forward * 100.0f, Color.red, 2.0f);
        if (Physics.Raycast(mouseWorldPosition, Camera.main.transform.forward, out this._hitInfo, 100.0f, this._noteLayerMask))
        {         
            return true;
        }

        
        */

    }
}
