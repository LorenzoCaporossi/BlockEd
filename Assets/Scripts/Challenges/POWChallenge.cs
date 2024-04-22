using System.Security.Cryptography;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using JetBrains.Annotations;
using System;
using System.Transactions;

public class POWChallenge : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI blockDataDescription;
    public GameObject image;
    public TMP_InputField nonceInput;
    public TextMeshProUGUI hashValue;
    public TextMeshProUGUI triesCount;
    public GameObject wrongText;
    public Button button_1;
    public Color solutionColor;
    public Color closeColor;

    [Space]
    public ChallengeDataReader jsonData;
    public UIHandler uIHandler;

    private string headerValue = "ABC123";

    private int solution = 734;
    private int tries = 10;
    string transactionNames;



    public TextMeshProUGUI OtherPlayerTxt;
    public GameObject wonPanel;
    public GameObject failPanel;
    public bool CubeDone;
    public void Start()
    {
        wrongText.SetActive(false);
        tries = 10;
        triesCount.text = tries.ToString();
        title.text = jsonData.dataList.powTitle;
        nonceInput.text = "";
        hashValue.text = "12345abcde...";
        image.SetActive(false);
        UpdateBlockData();

        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Check Hash";
        button_1.onClick.RemoveAllListeners();
        button_1.onClick.AddListener(() => UpdateHashValue());
    }
    private void OnEnable()
    {
        if (CubeDone)
        {
            OtherPlayerTxt.gameObject.SetActive(true);
        }
        else
        {
            OtherPlayerTxt.gameObject.SetActive(false);

        }
    }
    private void UpdateBlockData()
    {
        transactionNames = "";
        foreach (var item in GameManager.instance.buttonsSelected)
        {
            transactionNames += $"{item} ";
        }
        blockDataDescription.text = $"| Transactions: {transactionNames} | Nonce: ???]";
    }

    public string ToSHA256(string code)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));

            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    public void UpdateHashValue()
    {
        if (!nonceInput.text.Equals(""))
        {
            image.SetActive(false);
            string input = headerValue + nonceInput.text;
            string hash = ToSHA256(input);
            hashValue.text = hash;

            if (!hash.StartsWith("000"))
            {
                wrongText.SetActive(true);
                button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
            }
            else
            {
                if(GameManagerMy.Instance.isMultiplayer)
                {
                    if (!CubeDone)
                    {
                        wonPanel.SetActive(true);
                    }
                    else
                    {
                        button_1.GetComponent<Image>().color = solutionColor;
                        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Solution";
                        button_1.onClick.RemoveAllListeners();
                        button_1.onClick.AddListener(() => ShowSolution());
                        failPanel.SetActive(true);
                    }
                    MultiPlayHandler.Instance.SendPlayerDone();
                }
               
             

                wrongText.SetActive(false);
                button_1.onClick.RemoveAllListeners();
                button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
                button_1.onClick.AddListener(() => uIHandler.ValidateCube());
                button_1.onClick.AddListener(() => uIHandler.Button1Logic());
                button_1.onClick.AddListener(() => this.gameObject.SetActive(false));
                if (GameManagerMy.Instance.isMultiplayer) PhotonHandler.Instance.LeaveRoom();
                return;
            }

            if (tries > 1)
                tries--;
            else
            {
                tries--;
                button_1.GetComponent<Image>().color = solutionColor;
                button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Solution";
                button_1.onClick.RemoveAllListeners();
                button_1.onClick.AddListener(() => ShowSolution());
                if (GameManagerMy.Instance.isMultiplayer) PhotonHandler.Instance.LeaveRoom();
            }

            triesCount.text = tries.ToString();
        }
    }

    public void ShowSolution()
    {
        title.text = $"A correct solution would be: {solution}";
        button_1.GetComponent<Image>().color = closeColor;
        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        uIHandler.panelID = 5;
        button_1.onClick.RemoveAllListeners();
        button_1.onClick.AddListener(() => uIHandler.ValidateCube());
        button_1.onClick.AddListener(() => uIHandler.Button1Logic());
        button_1.onClick.AddListener(() => this.gameObject.SetActive(false));
        if (GameManagerMy.Instance.isMultiplayer) PhotonHandler.Instance.LeaveRoom();
        CubeDone = false;
    }

    public void ShowImage()
    {
        if (nonceInput.text.Equals(""))
            image.SetActive(true);
    }
}
