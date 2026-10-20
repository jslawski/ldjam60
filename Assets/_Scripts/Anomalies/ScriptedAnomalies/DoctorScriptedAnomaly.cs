using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScriptedAnomaly : ScriptedAnomaly
{
    public MaterialAnomaly _paintingAnomaly;
    public TextSequence _doctorSequence;

    public PlayerCharacterController _characterController;

    public override void StartSequence()
    {
        base.StartSequence();        
    }

    public override void Activate()
    {
        base.Activate();

        this._characterController.currentTextSequence = this._doctorSequence;
        this._characterController.ChangeState(new ReadState());

        StartCoroutine(this.WaitForFinish());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        StopAllCoroutines();

        AnomalyManager.instance.StartAnomalyHunt(1);
    }

    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(1.0f);    

        while (TextSequenceManager.instance.sequenceFinished == false)
        {        
            yield return null;
        }

        this.Deactivate();
    }
}
