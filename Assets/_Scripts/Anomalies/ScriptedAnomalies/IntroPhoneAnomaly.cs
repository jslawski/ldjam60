using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPhoneAnomaly : ScriptedAnomaly
{
    public GameObject phoneObject;
    public AudioClip phoneRing;
    public int audioID;

    public override void StartSequence()
    {
        base.StartSequence();

        this.Deactivate();
    }

    public override void Activate()
    {
        base.Activate();

        AudioChannelSettings channelSettings = new AudioChannelSettings(true, 1, 1, 1, "SFX", this.phoneObject.transform);
        this.audioID = AudioManager.instance.Play(this.phoneRing, channelSettings);
    }

    public override void Deactivate() 
    {
        AudioManager.instance.Stop(audioID);
        AnomalyManager.instance.ForceNextScriptedAnomaly();
    }
}
