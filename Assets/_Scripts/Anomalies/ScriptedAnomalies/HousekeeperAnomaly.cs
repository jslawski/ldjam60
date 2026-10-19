using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousekeeperAnomaly : ScriptedAnomaly
{
    public SpriteAnomaly brokenBird;
    public GameObject ladyObject;
    public AudioClip brokenGlassSound;
    public GameObject sourceDoor;
    public TextSequence textSequence;
    public GameObject pillsObject;
    public GameObject findMedsNote;
    public GameObject findMedsNoteComplete;
    public GameObject bakeCookiesNote;
    public InteractObject cookiesIngredients;


    public override void StartSequence()
    {
        base.StartSequence();

        TextSequenceManager.instance.StartTextSequence(this.textSequence, this.Deactivate);
    }

    public override void Activate()
    {
        base.Activate();

        AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1, 1, "SFX", this.sourceDoor.transform);
        AudioManager.instance.Play(this.brokenGlassSound, channelSettings);
        this.ladyObject.SetActive(true);
        this.brokenBird.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        this.ladyObject.SetActive(false);
        this.pillsObject.SetActive(true);
        this.findMedsNote.SetActive(false);
        this.findMedsNoteComplete.SetActive(true);
        this.bakeCookiesNote.SetActive(true);
        this.cookiesIngredients.enabled = true;

        AnomalyManager.instance.RefillPills();
    }
}
