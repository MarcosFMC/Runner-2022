using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameStateDeath : GameState, IUnityAdsListener
{

    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI _highscore;
    [SerializeField] private TextMeshProUGUI _currentScore;
    [SerializeField] private TextMeshProUGUI _fishTotal;
    [SerializeField] private TextMeshProUGUI _currentFish;

    [SerializeField] private Image completionCircle;
    public float timeToDecision = 2.5f;
    private float deathTime;


    public bool hasDiedFirstTime;
    public GameObject respawnBotton;

    private void Start()
    {
        Advertisement.AddListener(this);
    }
    public override void Construct()
    {
        GameManager.Instance.motor.PausePlayer();

        deathTime = Time.time;
        deathUI.SetActive(true);
        

        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            _currentScore.color = Color.green;
        }
        else
        {
            _currentScore.color = Color.white;
        }
         
        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;

        SaveManager.Instance.Save();

        _highscore.text = "Highscore: " + SaveManager.Instance.save.Highscore;
        _currentScore.text = GameStats.Instance.score.ToString("000000");
        _fishTotal.text = "Total fish: " + SaveManager.Instance.save.Fish;
        _currentFish.text = GameStats.Instance.fishCollectedThisSession.ToString("000");

    }
    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        completionCircle.fillAmount = 1 - ratio;
          
        if(ratio > 1)
        {
            completionCircle.gameObject.SetActive(false);
        }
    }

    public void TryResumeGame()
    {
        AdManager.Instance.showRewardedAd();
    }
    public void ResumeGame()
    {
        if (!hasDiedFirstTime)
        {
            _brain.ChangeState(GetComponent<GameStateGame>());
            GameManager.Instance.motor.RespawnPlayer();
            hasDiedFirstTime = true;
            respawnBotton.SetActive(false);
        }
       
    }
    public void ToMenu()
    {
        _brain.ChangeState(GetComponent<GameStateInit>());
        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();
    }

    public override void Destruct()
    {
        deathUI.SetActive(false);
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        completionCircle.gameObject.SetActive(false);
        switch (showResult)
        {
            case ShowResult.Failed:
                ToMenu();
                break;
            case ShowResult.Finished:
                ResumeGame();
                break;
            case ShowResult.Skipped:
                break;
        }
    }

    public void EnableRevive()
    {
        completionCircle.gameObject.SetActive(true);
    }
}
