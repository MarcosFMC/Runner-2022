using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    private static AdManager _instance;
    public static AdManager Instance { get { return _instance; } }



    [SerializeField] private string gameID;
    [SerializeField] private string rewardedVideoPlacementID;
    [SerializeField] private bool testMode;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Advertisement.Initialize(gameID, testMode);
    }

    public void showRewardedAd()
    {
        ShowOptions so = new ShowOptions();
        Advertisement.Show(rewardedVideoPlacementID, so);
    }
}
