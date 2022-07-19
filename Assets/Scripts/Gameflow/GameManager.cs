using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameCamera
{
    Init = 0,
    Game = 1,
    Shop = 2,
    Respawn = 3
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private GameState _state;

    public PlayerMotor motor;

    public GameObject[] cameras;

    public WorldGeneration worldGeneration;

    public SceneChunkGeneration sceneChunkGeneration;


    private void Start()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        _state = GetComponent<GameStateInit>();
        _state.Construct();
    }
    private void Update()
    {
        _state.UpdateState();
    }


    public void ChangeState(GameState s)
    {
        _state.Destruct();
        _state = s;
        _state.Construct(); 
    }

    public void ChangeCamera(GameCamera c)
    {
        foreach (GameObject go in cameras)
        {
            go.SetActive(false);
        }
        cameras[(int)c].SetActive(true);
    }
}
