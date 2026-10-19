using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScriptedAnomaly : ScriptedAnomaly
{
    public MaterialAnomaly _paintingAnomaly;
    public TextSequence _doctorSequence;

    public override void StartSequence()
    {
        base.StartSequence();

        TextSequenceManager.instance.StartTextSequence(this._doctorSequence, this.Deactivate);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        AnomalyManager.instance.StartAnomalyHunt(5);
    }
}
