using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStats : MonoBehaviour
{
    private static GameStats _instance;
    public static GameStats Instance { get { return _instance; } }

    //Score
    public float score;
    public float highscore;
    public float distanceModifier = 1.5f;

    //Fish
    public int totalFish;
    public int fishCollectedThisSession;
    public float pointsPerFish = 10.0f;

    //Action
    public Action<int> OnCollectFish;
    public Action<float> OnScoreChange;

    //Internal Cooldown
    private float lastScoreUpdate;
    private float scoreUpdateDelta = 0.2f;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }


    public void Update()
    {
        float s = GameManager.Instance.motor.transform.position.z * distanceModifier;
        s += fishCollectedThisSession * pointsPerFish;

        if (s > score)
        {
            score = s;
            if(Time.time - lastScoreUpdate > scoreUpdateDelta)
            {
                lastScoreUpdate = Time.time;
                OnScoreChange?.Invoke(score);
            }
        }
           
    }
    public void CollectedFish()
    {
        fishCollectedThisSession++;
        OnCollectFish?.Invoke(fishCollectedThisSession);
    }

    public void ResetSession()
    {
        score = 0f;
        fishCollectedThisSession = 0;
        OnCollectFish?.Invoke(fishCollectedThisSession);
        OnScoreChange?.Invoke(score);
    }
}
