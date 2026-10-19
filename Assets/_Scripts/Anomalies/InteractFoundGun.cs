using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFoundGun : InteractObject
{
    public GameObject missingGunObject;
    public GameObject paintingWoman;

    public override void Interact()
    {
        base.Interact();

        this.missingGunObject.SetActive(false);
        this.gameObject.SetActive(false);
        this.paintingWoman.SetActive(true);

        AnomalyManager.instance.ForceNextScriptedAnomaly();
    }
}
