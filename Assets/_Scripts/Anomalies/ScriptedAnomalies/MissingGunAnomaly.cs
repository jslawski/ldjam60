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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            TextSequenceManager.instance.StartTextSequence(this._anomalyTextSequence);

            MusicManager.instance.PlayPlace();
            MusicManager.instance.AddDistress();
            
            this.Deactivate();
        }
    }

    public override void Deactivate()
    {
        AudioChannelSettings channelSettings = new AudioChannelSettings(false, 1, 1, 1, "SFX");
        AudioManager.instance.Play(this.glassBreakSound, channelSettings);
        this.missingGunObject.SetActive(true);

        AnomalyManager.instance.StartAnomalyHunt(5);
        AnomalyManager.instance.RefillPills();
    }
}
