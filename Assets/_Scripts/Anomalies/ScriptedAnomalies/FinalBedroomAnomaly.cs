using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBedroomAnomaly : ScriptedAnomaly
{
    public GameObject finalBedroomObject;
    public GameObject voidBedroomObject;

    private PlayerCharacterController _characterController;

    [SerializeField]
    private TextSequence _anomalyTextSequence;
    public TextSequence voidTextSequence;

    public GameObject finalPhoneTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            this.StartSequence();
            this.Deactivate();
        }
    }

    public override void StartSequence()
    {
        base.StartSequence();

        this._characterController.currentTextSequence = this._anomalyTextSequence;
        this._characterController.ChangeState(new ReadState());

        MusicManager.instance.StopAll();

        StartCoroutine(WaitForSequenceEnd());
    }

    public override void Activate()
    {
        base.Activate();
        this.finalBedroomObject.SetActive(true);        
    }

    public override void Deactivate()
    {
        base.Deactivate();

        AnomalyManager.instance.ForceNextScriptedAnomaly();
    }

    private IEnumerator WaitForSequenceEnd()
    {
        yield return new WaitForSeconds(1.0f);

        while (TextSequenceManager.instance.sequenceFinished == false)
        {
            yield return null;
        }

        this.voidBedroomObject.SetActive(true);

        this._characterController.currentTextSequence = this.voidTextSequence;
        this._characterController.ChangeState(new ReadState());

        MusicManager.instance.AddDistress();
    }
}
