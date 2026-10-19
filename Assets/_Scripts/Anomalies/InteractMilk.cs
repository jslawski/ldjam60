using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMilk : InteractObject
{
    public GameObject cookiesNote;
    public GameObject cookiesCompleteNote;
    public GameObject findIntruderNote;

    public override void Interact()
    {
        base.Interact();

        this.cookiesNote.SetActive(false);
        this.cookiesCompleteNote.SetActive(true);
        this.findIntruderNote.SetActive(true);

        this.gameObject.SetActive(false);

        AnomalyManager.instance.ForceNextScriptedAnomaly();

        
    }
}
