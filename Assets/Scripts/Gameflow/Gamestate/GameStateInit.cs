using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateInit : GameState
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI fishCountText;



    public override void Construct()
    {
        GetComponent<GameStateDeath>().hasDiedFirstTime = false;
        GetComponent<GameStateDeath>().respawnBotton.SetActive(true);
        _brain.motor.hasDied = false;
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        highscoreText.text = "Highscore: " + SaveManager.Instance.save.Highscore.ToString();
        fishCountText.text = "Fish: " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);
    }
    public void OnPlayClick()
    {
        _brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
        GetComponent<GameStateDeath>().EnableRevive();
    }
    public void OnShopClick()
    {
       _brain.ChangeState(GetComponent<GameStateShop>());
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }
}
