using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherAnomaly : ScriptedAnomaly
{
    public AudioClip phoneRing;
    public TextSequence textSequence;
    public Transform phoneTransform;
    public int audioID;

    public GameObject pillsObject;

    public override void StartSequence()
    {
        base.StartSequence();
    }

    public override void Activate()
    {
        base.Activate();

        AudioChannelSettings channelSettings = new AudioChannelSettings(true, 1, 1, 1, "SFX", this.phoneTransform);
        this.audioID = AudioManager.instance.Play(this.phoneRing, channelSettings);
        AnomalyManager.instance.StartAnomalyHunt(5);
        AnomalyManager.instance.RefillPills();
        this.pillsObject.SetActive(false);
        StartCoroutine(this.DadDialogue());
    }

    public override void Deactivate()
    {
        base.Deactivate();
        AudioManager.instance.Stop(this.audioID);
        StopAllCoroutines();
    }

    private IEnumerator DadDialogue()
    {
        while (true)
        {
            TextSequenceManager.instance.StartTextSequence(this.textSequence);
            yield return new WaitForSeconds(30.0f);
        }
    }
}
