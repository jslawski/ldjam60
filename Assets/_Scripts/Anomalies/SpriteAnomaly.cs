using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnomaly : BaseAnomaly
{
    [SerializeField]
    private SpriteRenderer _changingRenderer;

    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite _anomalySprite;

    public override void Activate()
    {
        base.Activate();
        this._changingRenderer.sprite = this._anomalySprite;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        this._changingRenderer.sprite = this._defaultSprite;
    }
}
