using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPhoneAnomaly : ScriptedAnomaly
{
    public AudioClip phoneRing;
    public TextSequence textSequence;
    public Transform phoneTransform;

    private PlayerCharacterController _characterController;

    [SerializeField]
    private TextSequence _anomalyTextSequence;

    public LookObject finalLookObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            this.StartSequence();
            this.Deactivate();
        }

        MusicManager.instance.RemoveDistress();
        MusicManager.instance.PlayFeeling();
    }

    public override void StartSequence()
    {
        AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1, 1, "SFX", this.phoneTransform);
        TextSequenceManager.instance.StartTextSequence(this._anomalyTextSequence, this.FinishGame);        
    }

    private void FinishGame()
    {
        this._characterController.lookObjectStack.Push(this.finalLookObject);
        this._characterController.ChangeState(new LookState());
        this._characterController.EnterLookTransform();
    }
}
