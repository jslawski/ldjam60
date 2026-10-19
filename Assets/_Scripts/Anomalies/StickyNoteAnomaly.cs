using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteAnomaly : BenignAnomaly
{
    private GameObject[] _preWrittenNotes;

    private PlayerCharacterController _characterController;

    protected override void Awake()
    {
        base.Awake();

        this._preWrittenNotes = Resources.LoadAll<GameObject>("PreWrittenNotes");
        this._characterController = GameObject.Find("PlayerCharacter").GetComponent<PlayerCharacterController>();
    }

    public override bool IsVisible()
    {
        return (this._characterController.currentStickyNote != null);
    }
}
