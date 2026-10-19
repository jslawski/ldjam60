using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIngredients : InteractObject
{
    public TextSequence textSequence;
    public GameObject dranoObject;
    public GameObject fridgeMilkObject;
    public GameObject bathroomMilkObject;

    public override void Interact()
    {
        base.Interact();

        TextSequenceManager.instance.StartTextSequence(textSequence);
        this.dranoObject.SetActive(true);
        this.fridgeMilkObject.SetActive(false);
        this.bathroomMilkObject.SetActive(true);
    }
}
