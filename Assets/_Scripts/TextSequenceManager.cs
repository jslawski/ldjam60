using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextSequenceManager : MonoBehaviour
{
    public static TextSequenceManager instance;

    [SerializeField]
    private TextMeshProUGUI _displayText;

    private TextSequence _currentSequence;
    private int _currentIndex = 0;

    private Color _originalColor;

    private bool _playerClicked = false;

    private float _timeToDisplayInSeconds = 1.5f;

    Coroutine _displayCoroutine = null;
    private string _pendingText = string.Empty;

    [HideInInspector]
    public bool sequenceFinished = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this._originalColor = this._displayText.color;
    }

    private void Update()
    {            
        if (this._playerClicked == true)
        {
            this._playerClicked = false;

            if (this._currentSequence.activeText == true)
            {
                return;
            }

            if (this._displayCoroutine == null)
            {
                this.AdvanceText();
            }
            else
            {
                StopCoroutine(this._displayCoroutine);
                this._displayText.text = this._pendingText;
                this._displayCoroutine = null;
            }
        }
    }

    public void RegisterClick()
    {
        this._playerClicked = true;
    }

    public void StartTextSequence(TextSequence newSequence)
    {
        StopAllCoroutines();        
        this._currentSequence = newSequence;
        this._currentIndex = 0;
        this._displayText.text = string.Empty;
        this.sequenceFinished = false;

        this.DisplayCurrentText();

    }

    private void DisplayCurrentText()
    {
        this._displayText.text = string.Empty;
        this._originalColor = this._currentSequence.textColor[this._currentIndex];
        this._displayCoroutine = StartCoroutine(this.TypeWriteText());
    }

    private IEnumerator TypeWriteText()
    {
        this._pendingText = this._currentSequence.texts[this._currentIndex];
        float percentageOfDialoguePerFixedUpdate = (1.0f / this._timeToDisplayInSeconds) * Time.fixedDeltaTime;
        int lettersToDisplayPerFixedUpdate = Mathf.FloorToInt(this._pendingText.Length * percentageOfDialoguePerFixedUpdate);
        string nextLettersToDisplay = string.Empty;

        int minimumLettersToDisplayPerFixedUpdate = 1;

        if (lettersToDisplayPerFixedUpdate < minimumLettersToDisplayPerFixedUpdate)
        {
            lettersToDisplayPerFixedUpdate = minimumLettersToDisplayPerFixedUpdate;
        }

        for (int i = 0; i < this._pendingText.Length; i++)
        {
            if (nextLettersToDisplay.Length < lettersToDisplayPerFixedUpdate)
            {
                nextLettersToDisplay += this._pendingText[i];
            }
            else
            {
                this._displayText.text += nextLettersToDisplay;
                nextLettersToDisplay = string.Empty;
                yield return new WaitForFixedUpdate();
                nextLettersToDisplay += this._pendingText[i];
            }
        }

        if (nextLettersToDisplay != string.Empty)
        {
            this._displayText.text += nextLettersToDisplay;
        }

        this._displayCoroutine = null;

        if (this._currentSequence.activeText == true)
        {
            StartCoroutine(this.FadeOutAfterDelay());
        }
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);

        this.FinishSequence();
    }

    public void AdvanceText()
    {
        this._currentIndex++;

        if (this._currentIndex < this._currentSequence.texts.Length)
        {
            this.DisplayCurrentText();
        }
        else
        {
            this.FinishSequence();
        }
    }

    private void FinishSequence()
    {
        Color fadedColor = this._displayText.color;
        fadedColor.a = 0.0f;

        this._displayText.DOColor(fadedColor, 0.5f).OnComplete(this.ClearText);
        this.sequenceFinished = true;
    }

    private void ClearText()
    {
        this._displayText.text = string.Empty;
    }
}
