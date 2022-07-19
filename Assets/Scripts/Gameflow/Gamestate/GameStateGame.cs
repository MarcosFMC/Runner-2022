using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateGame : GameState
{
    public GameObject gameUI;
    [SerializeField] private TextMeshProUGUI scoreCount;
    [SerializeField] private TextMeshProUGUI fishCount;
    public override void Construct()
    {
        _brain.motor.ResumePlayer();
        GameManager.Instance.ChangeCamera(GameCamera.Game);

        GameStats.Instance.OnCollectFish += OnCollectFish;
        GameStats.Instance.OnScoreChange += OnScoreChange;
        gameUI.SetActive(true);
    }

    private void OnCollectFish(int amountCollected)
    {
        fishCount.text = "x" + amountCollected.ToString("000");
    }
    private void OnScoreChange(float score)
    {
        scoreCount.text = score.ToString("000000");
    }

    public override void UpdateState()
    {
        GameManager.Instance.worldGeneration.ScanPosition();
        GameManager.Instance.sceneChunkGeneration.ScanPosition();
    }

    public override void Destruct()
    {
        gameUI.SetActive(false);
        GameStats.Instance.OnCollectFish -= OnCollectFish;
        GameStats.Instance.OnScoreChange -= OnScoreChange;
    }



}
