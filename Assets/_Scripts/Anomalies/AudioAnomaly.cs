using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnomaly : BenignAnomaly
{
    [SerializeField]
    protected bool _activatedOnTrigger;

    [SerializeField]
    protected bool _loopAudio;

    [SerializeField]
    protected AudioClip _clip;
    protected AudioChannelSettings _channelSettings;

    private int _channelID;

    protected override void Awake()
    {
        base.Awake();

        this._channelSettings = new AudioChannelSettings(this._loopAudio);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (this._activatedOnTrigger == true)
            {
                this.Trigger();
            }
            else
            {
                this.Deactivate();
            }
        }
    }

    public override void Trigger()
    {
        base.Trigger();

        this._channelID = AudioManager.instance.Play(this._clip, this._channelSettings);
    }

    public override void Activate()
    {
        base.Activate();

        if (this._activatedOnTrigger == false)
        {
           this._channelID = AudioManager.instance.Play(this._clip, this._channelSettings);
        }
    }

    public override void Deactivate() 
    {
        base.Deactivate();
        
        if (this._activatedOnTrigger == false)
        {
            AudioManager.instance.Stop(this._channelID);
        }
    }
}
