using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TestAnomalyManager : MonoBehaviour
{    
    private BaseAnomaly[] _allAnomalyObjects;

    private List<BaseAnomaly> _allEnabledAnomalyComponents;

    [SerializeField]
    private TextMeshProUGUI _tutorialLabel;

    [SerializeField]
    private float _startupTime = 3.0f;

    private void Awake()
    {
        this._allAnomalyObjects = GetComponentsInChildren<BaseAnomaly>();
        this._allEnabledAnomalyComponents = new List<BaseAnomaly>();

        this.FilterEnabledAnomalyComponents();
    }

    private void FilterEnabledAnomalyComponents()
    {
        foreach (BaseAnomaly anomalyComponent in this._allAnomalyObjects)
        {
            if (anomalyComponent.enabled == true)
            { 
                this._allEnabledAnomalyComponents.Add(anomalyComponent);
            }
        }
    }

    private void FixedUpdate()
    {
        //this.ReportObjectVisibility();

        if (this.AreAllAnomalyObjectsVisible() == true)
        {
            this._startupTime -= Time.deltaTime;
        }

        this._tutorialLabel.text = "Study the Objects..." + "\n" + Math.Round(this._startupTime, 1).ToString() + "s";

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
        foreach (BaseAnomaly anomaly in this._allEnabledAnomalyComponents) 
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
        foreach (BaseAnomaly anomaly in this._allEnabledAnomalyComponents)
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
        foreach (BaseAnomaly anomaly in this._allEnabledAnomalyComponents)
        {
            if (anomaly.IsAnomalyActive() == true)
            {
                //Debug.LogError(anomaly.gameObject.name + " IS ACTIVE!");
                return true;
            }
        }

        return false;
    }

    private void TriggerRandomAnomaly()
    {
        int randomIndex = UnityEngine.Random.Range(0, this._allEnabledAnomalyComponents.Count);

        this._allEnabledAnomalyComponents[randomIndex].Activate();
    }

    private void ReportObjectVisibility()
    {
        foreach (BaseAnomaly anomaly in this._allEnabledAnomalyComponents)
        {
            if (anomaly.IsVisible() == true)
            {
                Debug.LogError(anomaly.gameObject.name + " IsVisible: " + anomaly.IsVisible());
            }
        }        
    }
}
