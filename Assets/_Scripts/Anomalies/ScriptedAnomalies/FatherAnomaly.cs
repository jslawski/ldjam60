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

        AnomalyManager.instance.StartAnomalyHunt(3);
        AnomalyManager.instance.RefillPills();
        
        this.pillsObject.SetActive(false);

        MusicManager.instance.AddDistress();

        StartCoroutine(this.DadDialogue());
        StartCoroutine(this.RingPhone());
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

    private IEnumerator RingPhone()
    {
        while (true)
        {
            AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1, 1, "SFX");
            this.audioID = AudioManager.instance.Play(this.phoneRing, channelSettings);

            yield return new WaitForSeconds(5.0f);
        }
    }
}
