using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public int panelID;
    public Image panelImage;

    [Header("UI Colors")]
    public Color activeColor;
    public Color inactiveColor;

    [Header("Button COlors")]
    public Color playAgainButtonColor;
    public Color ConnectButtonColor;

    [Header("UI Prefabs")]
    public TextMeshProUGUI titlePrefab;
    public TextMeshProUGUI descriptionPrefab;
    public Button button_1;
    public Button button_2;
    public Button connectButton;
    public Transform UIParent;

    [Header("Image Prefabs")]
    public GameObject imageHolderPrefab;
    public Image greenCubePrefab;
    public Image redCubePrefab;
    public Image chainPrefab;
    public Image proofOfWork;
    public Image proofOfStake;
    private GameObject imageHolder;

    [Header("Panel")]
    public GameObject transactionPanel;
    public GameObject validationPanel;
    public GameObject POWChallengePanel;
    public GameObject POSChallengePanel;
    public GameObject ValidationChallengePanel;
    public GameObject StartPanel;

    [Header("Line")]
    public LineRenderer linePrefab;

    [Header("Json")]
    public UIDataReader JsonData;

    public bool group1Connected;
    public bool group2Connected;
    private UIDataList dataList;
    public List<GameObject> tempGameObjects;
    public int dataID;


    private void Start()
    {
        panelID = 0;
        dataID = 0;
        group1Connected = false;
        group2Connected = false;

        dataList = JsonData.dataList;

        transactionPanel.SetActive(false);
        validationPanel.SetActive(false);
        POWChallengePanel.SetActive(false);
        POSChallengePanel.SetActive(false);
        ValidationChallengePanel.SetActive(false);

        connectButton.gameObject.SetActive(false);

        if (tempGameObjects.Count > 0)
        {
            foreach (var tempObject in tempGameObjects)
            {
                Destroy(tempObject);
            }
            tempGameObjects.Clear();
        }

        if (dataList != null && dataList.data.Count > 0)
        {
            DisplayUIData(dataList.data[dataID]);
        }
    }

    string transactionNames;

    public void DisplayUIData(UIData uiData)
    {
        if (panelID == uiData.panelID)
        {
            panelImage.color = activeColor;

            if (!uiData.title.Equals(""))
            {
                TextMeshProUGUI title = Instantiate(titlePrefab, UIParent);
                tempGameObjects.Add(title.gameObject);
                string finalText = uiData.title;
                if (finalText.Contains('|'))
                {
                    transactionNames = "";
                    string[] parts = finalText.Split('|');
                    foreach (var item in GameManager.instance.buttonsSelected)
                    {
                        transactionNames += $"{item} ";
                    }
                    finalText = $"{parts[0]} {transactionNames} {parts[1]}";
                }
                title.text = finalText;
            }

            if(uiData.description != null)
            {
                foreach (var dataDescription in uiData.description)
                {
                    TextMeshProUGUI description = Instantiate(descriptionPrefab, UIParent);
                    tempGameObjects.Add(description.gameObject);
                    string finalText = dataDescription;
                    if (finalText.Contains('|'))
                    {
                        string[] parts = finalText.Split('|');
                        finalText = $"{parts[0]} {GameManager.instance.gasFee} {parts[1]}";
                    }
                    else if (finalText.Contains('>'))
                    {
                        transactionNames = "";
                        string[] parts = finalText.Split('>');
                        foreach (var item in GameManager.instance.buttonsSelected)
                        {
                            transactionNames += $"{item} ";
                        }
                        finalText = $"{parts[0]} {transactionNames} {parts[1]}";
                    }
                    description.text = finalText;
                }
            }

            if(uiData.image != null)
            {
                imageHolder = Instantiate(imageHolderPrefab, UIParent);
                tempGameObjects.Add(imageHolder);
                foreach (var image in uiData.image)
                {
                    Image img = DrawImage(image, imageHolder.transform);
                    tempGameObjects.Add(img.gameObject);
                }
            }
            
            if (button_1 != null)
            {
                button_1.gameObject.SetActive(true);
                button_1.GetComponentInChildren<TextMeshProUGUI>().text = uiData.button1;
                button_1.onClick.RemoveAllListeners();
                button_1.onClick.AddListener(Button1Logic);
            }

            if (button_2 != null && !uiData.button2.Equals(""))
            {
                button_2.gameObject.SetActive(true);
                button_2.GetComponentInChildren<TextMeshProUGUI>().text = uiData.button2;
                button_2.onClick.RemoveAllListeners();
                button_2.onClick.AddListener(Button2Logic);
            }
            else if (uiData.button2.Equals(""))
            {
                button_2.gameObject.SetActive(false);
                button_2.onClick.RemoveAllListeners();
            }

            if (dataID < dataList.data.Count - 1)
                dataID++;

            //panelID++;

            connectButton.gameObject.SetActive(false);
        }
        else if(panelID == 11)
        {
            OpenPOWChallengePanel();
            panelID++;
        }
        else if (panelID == 19)
        {
            OpenPOSChallengePanel();
            panelID++;
        }
        else if (panelID == 28)
        {
            OpenValidationChallengePanel();
            panelID++;
        }
        else
        {
            panelImage.color = inactiveColor;

            if (GameManager.instance.Cubes != null)
            {
                if (!tempGameObjects.Contains(imageHolder))
                {
                    imageHolder = Instantiate(imageHolderPrefab, this.transform);
                    tempGameObjects.Add(imageHolder.gameObject);
                }
                foreach (var cube in GameManager.instance.Cubes)
                {
                    Image img = DrawImage(cube, imageHolder.transform);
                    if (cube == "Green Cube")
                    {
                        img.GetComponent<Button>().onClick.RemoveAllListeners();
                        img.GetComponent<Button>().onClick.AddListener(() => OpenTransactionPanel());
                    }
                    else if (cube == "Red Cube")
                    {
                        img.GetComponent<Button>().onClick.RemoveAllListeners();
                        img.GetComponent<Button>().onClick.AddListener(() => OpenValidationPanel());
                    }
                }
                if (group1Connected)
                {
                    ConnectCubes();
                }
            }
            button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
            //panelID++;
        }

        if (panelID == 5 || panelID == 13 || panelID == 6 || panelID == 14)
        {
            Debug.Log("Button deactivated");
            button_1.gameObject.SetActive(false);
            button_2.gameObject.SetActive(false);
        }

        if (
            GameManager.instance.Cubes.Count >= 2
            && GameManager.instance.Cubes[0] == "Green Cube"
            && GameManager.instance.Cubes[1] == "Green Cube"
            && !group1Connected
            && (panelID == 13 || panelID == 5)
            )
        {
            Debug.Log("Show Connect Button");
            connectButton.gameObject.SetActive(true);
            connectButton.GetComponent<Image>().color = ConnectButtonColor;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Add blocks to Blockchain";
            panelID = 26;
            dataID = 19;
            connectButton.onClick.RemoveAllListeners();
            connectButton.onClick.AddListener(() =>
            {
                Button1Logic();
                connectButton.gameObject.SetActive(false);
            });
        }

        if (
            GameManager.instance.Cubes.Count >= 4
            && GameManager.instance.Cubes[0] == "Green Cube"
            && GameManager.instance.Cubes[1] == "Green Cube"
            && GameManager.instance.Cubes[2] == "Green Cube"
            && GameManager.instance.Cubes[3] == "Green Cube"
            && !group2Connected
            && (panelID == 13 || panelID == 5)
            )
        {
            connectButton.gameObject.SetActive(true);
            connectButton.GetComponent<Image>().color = ConnectButtonColor;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 35;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Add blocks to Blockchain";
            panelID = 26;
            dataID = 19;
            connectButton.onClick.RemoveAllListeners();
            connectButton.onClick.AddListener(() =>
            {
                Button1Logic();
                connectButton.gameObject.SetActive(false);
            });
        }

        if (
            GameManager.instance.Cubes.Count >= 4
            && GameManager.instance.Cubes[0] == "Green Cube"
            && GameManager.instance.Cubes[1] == "Green Cube"
            && GameManager.instance.Cubes[2] == "Green Cube"
            && GameManager.instance.Cubes[3] == "Green Cube"
            && group1Connected && group2Connected
            && (panelID == 13 || panelID == 5)
            )
        {
            connectButton.gameObject.SetActive(true);
            connectButton.GetComponent<Image>().color = playAgainButtonColor;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 48;
            connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play Again?";
            connectButton.onClick.RemoveAllListeners();
            connectButton.onClick.AddListener(() => RestartGame());
        }
    }

    public void ValidateCube()
    {
        Debug.Log($"{GetType().Name}-> Cube Validated!");
        for (int i = 0; i < GameManager.instance.Cubes.Count; i++)
        {
            if (GameManager.instance.Cubes[i] == "Red Cube")
            {
                GameManager.instance.Cubes[i] = "Green Cube";
                break;
            }
        }
        GameManager.instance.coinCount += GameManager.instance.gasFee;
        GameManager.instance.UpdateCoinText();
    }

    public void EnableButton1(bool toggle)
    {
        button_1.gameObject.SetActive(toggle);
    }

    private Image DrawImage(string image, Transform imageHolder)
    {
        switch (image)
        {
            case "Green Cube":
                return Instantiate(greenCubePrefab, imageHolder);
            case "Red Cube":
                return Instantiate(redCubePrefab, imageHolder);
            case "Chain":
                return Instantiate(chainPrefab, imageHolder);
            case "Proof of Work":
                Debug.Log("Proof of work");
                return Instantiate(proofOfWork, panelImage.transform);
            case "Proof of Stake":
                Debug.Log("Proof of work");
                return Instantiate(proofOfStake, panelImage.transform);
            default:
                return chainPrefab;
        }
    }

    public void CreateCube(string cubeName)
    {
        if (!tempGameObjects.Contains(imageHolder))
        {
            imageHolder = Instantiate(imageHolderPrefab, this.transform);
            tempGameObjects.Add(imageHolder.gameObject);
        }

        if (GameManager.instance.Cubes.Count < 4)
        {
            Image img = DrawImage(cubeName, imageHolder.transform);

            if (cubeName == "Green Cube")
            {
                img.GetComponent<Button>().onClick.RemoveAllListeners();
                img.GetComponent<Button>().onClick.AddListener(() => OpenTransactionPanel());
            }
            else if (cubeName == "Red Cube")
            {
                img.GetComponent<Button>().onClick.RemoveAllListeners();
                img.GetComponent<Button>().onClick.AddListener(() => OpenValidationPanel());
            }
            GameManager.instance.Cubes.Add(cubeName);
        }
    }

    private void OpenTransactionPanel()
    {
        EnableLines(false);

        transactionPanel.SetActive(true);
    }

    private void OpenValidationPanel()
    {
        EnableLines(false);

        validationPanel.SetActive(true);
    }

    public void EnableLines(bool toggle)
    {
        if (line != null)
            line.enabled = toggle;
        
        if (line2 != null || line3 != null)
        {
            line2.enabled = toggle;
            line3.enabled = toggle;
        }
    }

    private void OpenPOWChallengePanel()
    {
        POWChallengePanel.GetComponent<POWChallenge>().Start();
        POWChallengePanel.SetActive(true);
    }

    private void OpenPOSChallengePanel()
    {
        POSChallengePanel.GetComponent<POSChallenge>().Start();
        POSChallengePanel.SetActive(true);
    }

    private void OpenValidationChallengePanel()
    {
        ValidationChallengePanel.GetComponent<ValidationChallenge>().Start();
        ValidationChallengePanel.SetActive(true);
    }

    private void RestartGame()
    {
        StartPanel.SetActive(true);
        GameManager.instance.Start();
        this.Start();
    }

    public void RemoveCube()
    {
        if (GameManager.instance.Cubes.Count > 0)
        {
            GameManager.instance.Cubes.RemoveAt(GameManager.instance.Cubes.Count - 1);
            Transform lastChildObject = imageHolder.transform.GetChild(imageHolder.transform.childCount - 1);
            Destroy(lastChildObject.gameObject);
        }
        else
            Debug.LogWarning($"{GetType().Name}-> No Cubes Exists!");
    }

    public void Button1Logic()
    {
        if(tempGameObjects.Count > 0)
        {
            foreach (var tempObject in tempGameObjects)
            {
                Destroy(tempObject);
            }
            tempGameObjects.Clear();
        }

        panelID++;
        DisplayUIData(dataList.data[dataID]);
    }

    public void Button2Logic()
    {
        if (tempGameObjects.Count > 0)
        {
            foreach (var tempObject in tempGameObjects)
            {
                Destroy(tempObject);
            }
            tempGameObjects.Clear();
        }

        panelID--;
        dataID--;
        dataID--;
        DisplayUIData(dataList.data[dataID]);
    }

    LineRenderer line, line2, line3;

    public void ConnectCubes()
    {
        if (group1Connected && line == null)
        {
            line = Instantiate(linePrefab, this.transform);

            tempGameObjects.Add(line.gameObject);
        }

        if(group2Connected && line2 == null && line3 == null)
        {
            line2 = Instantiate(linePrefab, this.transform);
            line3 = Instantiate(linePrefab, this.transform);

            tempGameObjects.Add(line2.gameObject);
            tempGameObjects.Add(line3.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (imageHolder != null)
        {
            if (group1Connected && line != null && GameManager.instance.Cubes.Count >= 2)
            {
                line.SetPosition(0, new Vector3(imageHolder.transform.GetChild(0).position.x + 0.6f, 0, 0));
                line.SetPosition(1, new Vector3(imageHolder.transform.GetChild(1).position.x - 0.6f, 0, 0));
            }

            if (group2Connected && line2 != null && line3 != null && GameManager.instance.Cubes.Count >= 4)
            {
                line2.SetPosition(0, new Vector3(imageHolder.transform.GetChild(1).position.x + 0.6f, 0, 0));
                line2.SetPosition(1, new Vector3(imageHolder.transform.GetChild(2).position.x - 0.6f, 0, 0));

                line3.SetPosition(0, new Vector3(imageHolder.transform.GetChild(2).position.x + 0.6f, 0, 0));
                line3.SetPosition(1, new Vector3(imageHolder.transform.GetChild(3).position.x - 0.6f, 0, 0));
            }
        }
    }

    public void ConnectGroup2Cubes()
    {
        LineRenderer line = Instantiate(linePrefab, this.transform);
        line.SetPosition(0, new Vector3(-3.75f, 0, 0));
        line.SetPosition(1, new Vector3(-2f, 0, 0));

        LineRenderer line2 = Instantiate(linePrefab, this.transform);
        line2.SetPosition(0, new Vector3(-0.75f, 0, 0));
        line2.SetPosition(1, new Vector3(0.75f, 0, 0));

        LineRenderer line3 = Instantiate(linePrefab, this.transform);
        line3.SetPosition(0, new Vector3(2f, 0, 0));
        line3.SetPosition(1, new Vector3(3.5f, 0, 0));
    }
}
