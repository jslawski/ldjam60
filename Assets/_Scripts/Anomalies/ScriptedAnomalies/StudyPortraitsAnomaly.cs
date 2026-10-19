using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyPortraitsAnomaly : ScriptedAnomaly
{
    public GameObject whiteFigure;

    public override void StartSequence()
    {
        base.StartSequence();

        this.Deactivate();
    }

    public override void Activate()
    {
        base.Activate();
        this.whiteFigure.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        this.whiteFigure.SetActive(false);

        AnomalyManager.instance.ForceNextScriptedAnomaly();
    }
}
