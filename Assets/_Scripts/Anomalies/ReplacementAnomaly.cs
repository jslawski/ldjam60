using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementAnomaly : BaseAnomaly
{
    [SerializeField]
    private GameObject _originalObject;

    [SerializeField]
    private GameObject _replacementObject;

    protected override void Awake()
    {
        base.Awake();

        this._originalObject.SetActive(true);
        this._replacementObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();

        this._originalObject.SetActive(false);
        this._replacementObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        this._originalObject.SetActive(true);
        this._replacementObject.SetActive(false);
    }
}
