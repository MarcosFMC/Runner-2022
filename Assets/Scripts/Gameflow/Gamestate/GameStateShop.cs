using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameStateShop : GameState
{
    public GameObject shopUI;
    public TextMeshProUGUI totalFish;
    public TextMeshProUGUI currentHatName;
    public HatLogic hatLogic;
    private bool isInit = false;

    //Shop
    public GameObject hatPrefab;
    public Transform hatContainer;
    private Hat[] hats;

    //Completion Circle
    public Image completionCircle;
    public TextMeshProUGUI completionText;
    private int hatCount;
    private int unlockedHatCount;
    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Shop);
        totalFish.text = SaveManager.Instance.save.Fish.ToString("000");
        shopUI.SetActive(true);


        if (!isInit)
        {
            hats = Resources.LoadAll<Hat>("Hat/");
            PopulateShop();
            currentHatName.text = hats[SaveManager.Instance.save.CurrentHatIndex].ItemName;
            isInit = true;
        }
        ResetCompletionCircle();
    }

    public void OnHomeClick()
    {
        _brain.ChangeState(GetComponent<GameStateInit>());
    }
    private void PopulateShop()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(hatPrefab, hatContainer);

            //Button
            go.GetComponent<Button>().onClick.AddListener(() => OnHatClick(index)); 
            //Thumbnail
            go.transform.GetChild(1).GetComponent<Image>().sprite = hats[index].Thumbnail;
            //ItemName
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hats[index].ItemName;
            //Price
            if(SaveManager.Instance.save.UnlockedHatFlag[i]== 0)
            {
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = hats[index].ItemPrice.ToString();
            }
            else
            {
                go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                unlockedHatCount++;
            }
           

        }
    }

    private void OnHatClick(int i)
    {
        if (SaveManager.Instance.save.UnlockedHatFlag[i] == 1)
        {
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectedHat(i);
            SaveManager.Instance.Save();
        }
        else if (hats[i].ItemPrice <= SaveManager.Instance.save.Fish)
        {
            SaveManager.Instance.save.Fish -= hats[i].ItemPrice;
            SaveManager.Instance.save.UnlockedHatFlag[i] = 1;
            SaveManager.Instance.save.CurrentHatIndex = i;
            currentHatName.text = hats[i].ItemName;
            hatLogic.SelectedHat(i);
            SaveManager.Instance.Save();
            totalFish.text = SaveManager.Instance.save.Fish.ToString("000");

            hatContainer.GetChild(i).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            unlockedHatCount++;
            ResetCompletionCircle();
        }   
        else
        {
            Debug.Log("Not enough fish");
        }

    }

    public override void Destruct()
    {
        shopUI.SetActive(false);
    }

    private void ResetCompletionCircle()
    {

        hatCount = hats.Length - 1;
        int currentUnlockedCount = unlockedHatCount - 1;
        completionCircle.fillAmount = (float) currentUnlockedCount / (float)hatCount; 
        completionText.text = currentUnlockedCount + " / " + hatCount;
    }
    
}
