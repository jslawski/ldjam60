using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager instance;
    [SerializeField]
    private Queue<ScriptedAnomaly> _scriptedAnomalies; 
    private BaseAnomaly[] _allAnomalies;
    private BenignAnomaly[] _allBenignAnomalies;

    public int numActiveAnomalies = 0;
    public int maxNumAnomalies = 10;

    public int targetAnomaliesToResolve = 3;

    private float _minTimeBetweenAnomalies = 45.0f;
    private float _maxTimeBetweenAnomalies = 90.0f;

    private float _minTimeBetweenBenignAnomalies = 120.0f;
    private float _maxTimeBetweenBenignAnomalies = 240.0f;

    private Coroutine _anomalyGenerator;
    private Coroutine _benignAnomalyGenerator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        this._allAnomalies = GetComponentsInChildren<BaseAnomaly>();
    }

    public void StartAnomalyHunt(int targetAnomalies)
    {
        this.targetAnomaliesToResolve = targetAnomalies;

        this.StartAnomalyGeneration();
    }

    private void StartAnomalyGeneration()
    {
        if (this._anomalyGenerator == null)
        {
            this._anomalyGenerator = StartCoroutine(this.GenerateAnomalies());
        }

        if (this._benignAnomalyGenerator == null)
        {
            this._benignAnomalyGenerator = StartCoroutine(this.GenerateBenignAnomalies());
        }
    }

    private IEnumerator GenerateAnomalies()
    {
        float timeUntilNextAnomaly = Random.Range(this._minTimeBetweenAnomalies, this._maxTimeBetweenAnomalies);    

        while (this.numActiveAnomalies < this.maxNumAnomalies && this.targetAnomaliesToResolve > 0)
        {
            yield return new WaitForSeconds(timeUntilNextAnomaly);

            this.ActivateRandomAnomaly();
        }

        if (this.numActiveAnomalies > this.maxNumAnomalies)
        {
            Debug.LogError("GAME OVER TRIGGERED");
        }
        else if (this.targetAnomaliesToResolve == 0)
        {
            StartCoroutine(this.ActivateNextScriptedAnomaly());
        }
    }

    private IEnumerator GenerateBenignAnomalies()
    {
        float timeUntilNextAnomaly = Random.Range(this._minTimeBetweenBenignAnomalies, this._maxTimeBetweenBenignAnomalies);

        while (this.numActiveAnomalies < this.maxNumAnomalies)
        {
            yield return new WaitForSeconds(timeUntilNextAnomaly);

            this.ActivateRandomBenignAnomaly();
        }
    }

    public void StopAnomalyGeneration() 
    {
        if (this._anomalyGenerator != null)
        {
            StopCoroutine(this._anomalyGenerator);
        }
    }

    private void ActivateRandomAnomaly()
    {
        int randomIndex = Random.Range(0, this._allAnomalies.Length);
        BaseAnomaly randomAnomaly = this._allAnomalies[randomIndex];

        while (randomAnomaly.IsVisible() == true)
        {
            randomIndex = Random.Range(0, this._allAnomalies.Length);
            randomAnomaly = this._allAnomalies[randomIndex];
        }

        randomAnomaly.Activate();

        this.numActiveAnomalies++;
    }

    private void ActivateRandomBenignAnomaly()
    {
        int randomIndex = Random.Range(0, this._allBenignAnomalies.Length);
        BenignAnomaly randomAnomaly = this._allBenignAnomalies[randomIndex];

        int attempts = 10;

        while ((randomAnomaly.IsVisible() == true || randomAnomaly.hasBeenActivated == true) && attempts > 0)
        {
            randomIndex = Random.Range(0, this._allBenignAnomalies.Length);
            randomAnomaly = this._allBenignAnomalies[randomIndex];
            attempts--;
        }

        if (attempts > 0)
        {
            randomAnomaly.Activate();
        }
    }

    private IEnumerator ActivateNextScriptedAnomaly()
    {
        ScriptedAnomaly nextScriptedAnomaly = this._scriptedAnomalies.Dequeue();

        while (nextScriptedAnomaly.IsVisible() == true)
        {
            yield return null;
        }

        nextScriptedAnomaly.Activate();
    }

    public void ResolveAnomaly()
    {
        this.numActiveAnomalies--;
        this.targetAnomaliesToResolve--;
    }
}
