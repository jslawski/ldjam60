using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager instance;
    [SerializeField]
    private ScriptedAnomaly[] _scriptedAnomalies;
    private Queue<ScriptedAnomaly> _scriptedAnomaliesQueue; 
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

        this._scriptedAnomaliesQueue = new Queue<ScriptedAnomaly>();

        for (int i = 0; i < this._scriptedAnomalies.Length; i++)
        {
            this._scriptedAnomaliesQueue.Enqueue(this._scriptedAnomalies[i]);
        }
    }

    private void Update()
    {
        if (this.targetAnomaliesToResolve <= 0 && this._anomalyGenerator != null)
        {
            this.StopAnomalyGeneration();
            StartCoroutine(this.ActivateNextScriptedAnomaly());
        }
    }

    private void Start()
    {
        this.StartAnomalyHunt(3);
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
            //this._benignAnomalyGenerator = StartCoroutine(this.GenerateBenignAnomalies());
        }
    }

    private IEnumerator GenerateAnomalies()
    {
        float timeUntilNextAnomaly = Random.Range(this._minTimeBetweenAnomalies, this._maxTimeBetweenAnomalies);    

        while (this.numActiveAnomalies < this.maxNumAnomalies)
        {
            yield return new WaitForSeconds(timeUntilNextAnomaly);

            this.ActivateRandomAnomaly();
        }

        if (this.numActiveAnomalies > this.maxNumAnomalies)
        {
            Debug.LogError("GAME OVER TRIGGERED");
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
        StopAllCoroutines();
        this._anomalyGenerator = null;
        this._benignAnomalyGenerator = null;
    }

    private void ActivateRandomAnomaly()
    {
        int randomIndex = Random.Range(0, this._allAnomalies.Length);
        BaseAnomaly randomAnomaly = this._allAnomalies[randomIndex];

        int attempts = 10;

        while (randomAnomaly.IsVisible() == true && attempts > 0)
        {
            randomIndex = Random.Range(0, this._allAnomalies.Length);
            randomAnomaly = this._allAnomalies[randomIndex];
            attempts--;
        }

        if (attempts > 0)
        {
            randomAnomaly.Activate();
            this.numActiveAnomalies++;
        }
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
        ScriptedAnomaly nextScriptedAnomaly = this._scriptedAnomaliesQueue.Dequeue();

        while (nextScriptedAnomaly.IsVisible() == true)
        {
            Debug.LogError("Anomaly Is Visible");
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
