using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterAnomaly : ScriptedAnomaly
{
    private PlayerCharacterController _characterController;

    [SerializeField]
    private TextSequence _anomalyTextSequence;

    public GameObject findIntruderNote;
    public GameObject findIntruderComplete;
    public GameObject tidyUpNote;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this._characterController = other.gameObject.GetComponentInParent<PlayerCharacterController>();

            this.StartSequence();

            this.findIntruderNote.SetActive(false);
            this.findIntruderComplete.SetActive(true);
            this.tidyUpNote.SetActive(true);

            MusicManager.instance.RemoveDistress();
            MusicManager.instance.PlayPlace();            
        }
    }

    public override void StartSequence()
    {
        this._characterController.currentTextSequence = this._anomalyTextSequence;
        this._characterController.ChangeState(new ReadState());

        StartCoroutine(this.WaitForFinish());

        
    }

    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(1.0f);

        while (TextSequenceManager.instance.sequenceFinished == false)
        {
            yield return null;
        }

        AnomalyManager.instance.StartAnomalyHunt(4);
        AnomalyManager.instance.RefillPills();

        this.Deactivate();
    }
}
