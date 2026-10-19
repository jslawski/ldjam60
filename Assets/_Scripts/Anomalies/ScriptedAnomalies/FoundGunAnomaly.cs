using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundGunAnomaly : ScriptedAnomaly
{
    public GameObject foundGunObject;

    public override void Activate()
    {
        base.Activate();
        this.foundGunObject.SetActive(true);
    }
}
