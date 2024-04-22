using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class POSChallenge : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TMP_InputField stakeInput;
    public TextMeshProUGUI yourBid;
    public TextMeshProUGUI aBid;
    public TextMeshProUGUI bBid;
    public Button button_1;
    public GameObject recordedPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Space]
    public ChallengeDataReader jsonData;
    public UIHandler handler;
    private ChallengeData data;

    public int randomNumberIncrement = 5;

    int random1, random2;
    bool won = false;
    int wonNumber;
    string wonPerson;

    public void Start()
    {
        data = jsonData.dataList;

        title.text = data.posTitle;

        stakeInput.text = "";

        recordedPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        button_1.gameObject.SetActive(true);

        title.transform.parent.gameObject.SetActive(false);

        //button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Stake";
        //button_1.onClick.RemoveAllListeners();
        //button_1.onClick.AddListener(() => Stake());
    }

    public void Stake(int num)
    {
            random1 = Random.Range(32, GameManager.instance.coinCount + randomNumberIncrement);
            random2 = Random.Range(32, GameManager.instance.coinCount + randomNumberIncrement);
            yourBid.text = num.ToString();
            aBid.text = random1.ToString();
            bBid.text = random2.ToString();

            if (num > random1 && num > random2)
            {
                won = true;
            }
            else if(random1 > num && random1 > random2)
            {
                won = false;
                wonNumber = random1;
                wonPerson = "A Staker";
            }
            else if(random2 > num && random2 > random1)
            {
                won = false;
                wonNumber = random2;
                wonPerson = "B Staker";
            }
            
        
    }

    public void CheckStake()
    {
        if (!stakeInput.text.Equals(""))
        {
            int num = int.Parse(stakeInput.text);
            if (num >= 32 && num <= GameManager.instance.coinCount)
            {
                Stake(num);
                button_1.gameObject.SetActive(false);
                recordedPanel.gameObject.SetActive(true);
            }
        }
    }

    public void OpenTitle()
    {
        title.transform.parent.gameObject.SetActive(true);
    }

    public void UpdateTitle()
    {
        string finalText = data.updateTitle;
        if (finalText.Contains('|'))
        {
            string[] parts = finalText.Split('|');
            finalText = $"{parts[0]} {GameManager.instance.coinCount} {parts[1]}";
        }

        title.text = finalText;
    }

    public void CheckWin()
    {
        if (won)
        {
            handler.ValidateCube();
            winPanel.SetActive(true);
        }
        else
        {
            string finalText = data.loseText;
            if (finalText.Contains('<'))
            {
                string[] parts = finalText.Split('<');
                finalText = $"{parts[0]} {wonPerson} {parts[1]}";
            }

            if (finalText.Contains('>'))
            {
                string[] parts = finalText.Split('>');
                finalText = $"{parts[0]} {wonNumber} {parts[1]}";
            }

            losePanel.GetComponentInChildren<TextMeshProUGUI>().text = finalText;
            //GameManager.instance.coinCount -= GameManager.instance.gasFee;
            //GameManager.instance.UpdateCoinText();
            losePanel.SetActive(true);
        }
    }

    public void ClosePOS()
    {
        handler.panelID = 4;
        handler.Button1Logic();
        gameObject.SetActive(false);
    }
}
