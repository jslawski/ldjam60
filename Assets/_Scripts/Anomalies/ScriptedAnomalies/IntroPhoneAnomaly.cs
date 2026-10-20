using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPhoneAnomaly : ScriptedAnomaly
{
    public GameObject phoneObject;
    public AudioClip phoneRing;
    public int audioID;

    public Collider interactCollider;

    public override void StartSequence()
    {
        base.StartSequence();

        this.Deactivate();
    }

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(this.RingPhone());
    }

    public override void Deactivate() 
    {
        base.Deactivate();
    
        AudioManager.instance.Stop(audioID);
        AnomalyManager.instance.ForceNextScriptedAnomaly();

        this.interactCollider.enabled = false;

        StopAllCoroutines();
    }

    private IEnumerator RingPhone()
    {
        while (true)
        {
            AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1, 1, "SFX", this.phoneObject.transform);
            this.audioID = AudioManager.instance.Play(this.phoneRing, channelSettings);

            yield return new WaitForSeconds(3.0f);
        }
    }
}
