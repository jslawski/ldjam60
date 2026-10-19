using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterAnomaly : ScriptedAnomaly
{
    private PlayerCharacterController _characterController;

    [SerializeField]
    private TextSequence _anomalyTextSequence;

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
        this._characterController.currentTextSequence = this._anomalyTextSequence;
        this._characterController.ChangeState(new ReadState());

        AnomalyManager.instance.StartAnomalyHunt(5);
    }
}
