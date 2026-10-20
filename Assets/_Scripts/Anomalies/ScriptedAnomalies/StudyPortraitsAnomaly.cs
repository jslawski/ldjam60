using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyPortraitsAnomaly : ScriptedAnomaly
{
    public GameObject whiteFigure;
    public GameObject painter;

    public override void StartSequence()
    {
        base.StartSequence();

        this.Deactivate();
    }

    public override void Activate()
    {
        base.Activate();
        this.whiteFigure.SetActive(true);
        this.painter.SetActive(false);

        MusicManager.instance.StopAll();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        this.whiteFigure.SetActive(false);

        AnomalyManager.instance.ForceNextScriptedAnomaly();
    }
}
