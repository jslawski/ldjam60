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
    public GameObject cookiesIngredients;

    private PlayerCharacterController _characterController;

    public GameObject tutorialUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            this.StartSequence();

            this.tutorialUI.SetActive(false);
        }
    }

    public override void StartSequence()
    {
        base.StartSequence();

        this._characterController.currentTextSequence = this.textSequence;
        this._characterController.ChangeState(new ReadState());

        StartCoroutine(this.WaitForFinish());
    }

    public override void Activate()
    {
        base.Activate();

        AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1.0f, 0.2f, "SFX", this.sourceDoor.transform);
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
        this.cookiesIngredients.SetActive(true);

        AnomalyManager.instance.RefillPills();
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
