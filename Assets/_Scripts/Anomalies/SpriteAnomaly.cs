using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnomaly : BaseAnomaly
{
    [Header("Swap the sprite of an object with a different sprite.\n" +
    "The sprite can also be flipped.")]
    private SpriteRenderer _changingRenderer;

    [SerializeField]
    private Sprite _defaultSprite;
    [SerializeField]
    private Sprite _anomalySprite;
    [SerializeField]
    private bool _flipX;
    [SerializeField]
    private bool _flipY;

    protected override void Start()
    {
        base.Start();

        this._changingRenderer = GetComponentInParent<SpriteRenderer>();
    }

    public override void Activate()
    {
        base.Activate();
        this._changingRenderer.sprite = this._anomalySprite;

        if (this._flipX == true)
        {
            this._changingRenderer.flipX = true;
        }
        if (this._flipY == true)
        {
            this._changingRenderer.flipY = true;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        this._changingRenderer.sprite = this._defaultSprite;
        this._changingRenderer.flipX = false;
        this._changingRenderer.flipY = false;
    }
}
