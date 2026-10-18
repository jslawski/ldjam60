using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TestAnomalyManager : MonoBehaviour
{
    [SerializeField]
    private BaseAnomaly[] _allAnomalyObjects;

    [SerializeField]
    private TextMeshProUGUI _tutorialLabel;

    [SerializeField]
    private float _startupTime = 3.0f;

    private void Awake()
    {
        this._allAnomalyObjects = GetComponentsInChildren<BaseAnomaly>();
    }

    private void FixedUpdate()
    {
        //this.ReportObjectVisibility();

        if (this.AreAllAnomalyObjectsVisible() == true)
        {
            this._startupTime -= Time.deltaTime;
        }

        this._tutorialLabel.text = "Study the Objects..." + "\n" + Math.Round(this._startupTime, 2).ToString() + "s";

        if (this._startupTime > 0.0f)
        {
            return;
        }

        if (this.AreAllAnomalyObjectsHidden() == true && this.IsAnomalyActive() == false)
        {
            this.TriggerRandomAnomaly();
        }
        else if (this.IsAnomalyActive() == false)
        {
            this._tutorialLabel.text = "Turn around!";
        }
        else
        {
            this._tutorialLabel.text = "FIND THE ANOMALY!";
        }

    }

    private bool AreAllAnomalyObjectsHidden()
    { 
        foreach (BaseAnomaly anomaly in this._allAnomalyObjects) 
        {        
            if (anomaly.IsVisible() == true)
            {
                return false;
            }
        }

        //Debug.LogError("ALL OBJECTS ARE HIDDEN!");

        return true;
    }

    private bool AreAllAnomalyObjectsVisible()
    {    
        foreach (BaseAnomaly anomaly in this._allAnomalyObjects)
        {        
            if (anomaly.IsVisible() == false)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsAnomalyActive()
    {
        foreach (BaseAnomaly anomaly in this._allAnomalyObjects)
        {
            if (anomaly.IsAnomalyActive() == true)
            {
                Debug.LogError(anomaly.gameObject.name + " IS ACTIVE!");
                return true;
            }
        }

        return false;
    }

    private void TriggerRandomAnomaly()
    {
        int randomIndex = UnityEngine.Random.Range(0, this._allAnomalyObjects.Length);

        this._allAnomalyObjects[randomIndex].Activate();
    }

    private void ReportObjectVisibility()
    {
        foreach (BaseAnomaly anomaly in this._allAnomalyObjects)
        {
            if (anomaly.IsVisible() == true)
            {
                Debug.LogError(anomaly.gameObject.name + " IsVisible: " + anomaly.IsVisible());
            }
        }        
    }
}
