using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pills : InteractObject
{
    

    [SerializeField]
    private TextSequence _pillsTakenSequence;
    [SerializeField]
    private TextSequence _cantTakePillsSequence;
    [SerializeField]
    private TextSequence _pillsAvailableSequence;

    private void Awake()
    {
        this.available = true;
    }

    public override void Interact()
    {
        base.Interact();

        this.TryTakePills();
    }

    private void TryTakePills()
    {
        if (this.available == true)
        {
            this.available = false;
            AnomalyManager.instance.ResolveAllActiveAnomalies();
            TextSequenceManager.instance.StartTextSequence(this._pillsTakenSequence);
        }
        else
        {
            TextSequenceManager.instance.StartTextSequence(this._cantTakePillsSequence);
        }
    }

    public void Refill()
    {
        this.available = true;
        //TextSequenceManager.instance.StartTextSequence(this._pillsAvailableSequence);
    }
}
