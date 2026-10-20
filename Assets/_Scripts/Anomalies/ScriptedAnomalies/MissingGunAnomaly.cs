using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingGunAnomaly : ScriptedAnomaly
{
    private PlayerCharacterController _characterController;

    [SerializeField]
    private TextSequence _anomalyTextSequence;

    public AudioClip glassBreakSound;
    public GameObject missingGunObject;

    public Collider cookingIngredients;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            TextSequenceManager.instance.StartTextSequence(this._anomalyTextSequence);

            MusicManager.instance.PlayPlace();
            MusicManager.instance.AddDistress();

            this.cookingIngredients.enabled = false;

            this.Deactivate();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
    
        AudioChannelSettings channelSettings = new AudioChannelSettings(false, 0.8f, 0.8f, 0.5f, "SFX");
        AudioManager.instance.Play(this.glassBreakSound, channelSettings);
        this.missingGunObject.SetActive(true);

        AnomalyManager.instance.StartAnomalyHunt(5);
    }
}
