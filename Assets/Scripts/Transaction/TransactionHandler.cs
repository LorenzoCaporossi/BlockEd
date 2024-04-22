using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransactionHandler : MonoBehaviour
{
    public string uniqueTransactionID;

    [Header("UI References")]
    public TMP_InputField transactionID;
    public TMP_InputField timestamp;
    public TMP_InputField senderAddress;
    public TMP_InputField recipientAddress;
    public TMP_InputField amount;
    public TMP_InputField status;
    public TMP_InputField blockchainNetwork;
    public TMP_InputField transactionFee;
    private bool isTransactionApplicable;
    public Button button_1;
    public Button button_2;
    public Transform UIParent;

    [Header("Prefabs")]
    public TextMeshProUGUI transactionPrefab;
    public GameObject showDataPanel;
    public GameObject customizeDataPanel;

    [Header("Json")]
    public TransactionDataReader JsonData;
    public UIHandler handler;

    public List<GameObject> tempGameObjects;
    private TransactionDataList dataList;
    public int dataID;

    private void Start()
    {
        dataList = JsonData.dataList;
        uniqueTransactionID = Guid.NewGuid().ToString();

        CloseCustomizePanel();
    }

    public void OpenCustomizePanel(int i)
    {
        showDataPanel.SetActive(false);
        customizeDataPanel.SetActive(true);
        DisplayTransactionData(dataList.data[i]);

        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        button_1.onClick.RemoveAllListeners();
        button_1.onClick.AddListener(() => UpdateTransactionData(i));
        button_1.onClick.AddListener(() => CloseCustomizePanel());

        button_2.GetComponentInChildren<TextMeshProUGUI>().text = "Customize";
        button_2.onClick.RemoveAllListeners();
        button_2.onClick.AddListener(CustomizeData);
    }

    public void CloseCustomizePanel()
    {
        showDataPanel.SetActive(true);
        customizeDataPanel.SetActive(false);
        DisplayTransactions();

        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
        button_1.onClick.RemoveAllListeners();
        button_1.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
            handler.EnableLines(true);
            });

        button_2.GetComponentInChildren<TextMeshProUGUI>().text = "Add Transaction";
        button_2.onClick.RemoveAllListeners();
        button_2.onClick.AddListener(() => AddNewTransaction());
    }

    public void AddNewTransaction()
    {
        senderAddress.text = "";
        recipientAddress.text = "";
        amount.text = "";
        status.text = "";
        blockchainNetwork.text = "";
        transactionFee.text = "";

        showDataPanel.SetActive(false);
        customizeDataPanel.SetActive(true);

        CustomizeData();

        transactionID.text = uniqueTransactionID.ToString();
        transactionID.interactable = false;
        timestamp.text = DateTime.Now.ToString();
        timestamp.interactable = false;

        button_1.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        button_1.onClick.RemoveAllListeners();
        button_1.onClick.AddListener(() => CloseCustomizePanel());

        button_2.GetComponentInChildren<TextMeshProUGUI>().text = "Save";
        button_2.onClick.RemoveAllListeners();
        button_2.onClick.AddListener(() => SaveNewTransactionData());
    }

    public void CloseTransactionPanel()
    {
        this.gameObject.SetActive(false);
    }
        

    public void DisplayTransactions()
    {
        if (tempGameObjects.Count > 0)
        {
            foreach (var tempObject in tempGameObjects)
            {
                Destroy(tempObject);
            }
            tempGameObjects.Clear();
        }

        for (int i = 1; i <= dataList.data.Count; i++)
        {
            int transactionIndex = i - 1;
            TextMeshProUGUI transaction = Instantiate(transactionPrefab, UIParent);
            transaction.text = $"[Transaction {i}]";
            transaction.GetComponent<Button>().onClick.RemoveAllListeners();
            transaction.GetComponent<Button>().onClick.AddListener(() => OpenCustomizePanel(transactionIndex));

            tempGameObjects.Add(transaction.gameObject);
        }
    }

    public void DisplayTransactionData(TransactionData transactionData)
    {
        if (dataList != null && dataList.data.Count > 0)
        {
            if (transactionData.transactionID == null)
                transactionData.transactionID = uniqueTransactionID;

            transactionID.text = transactionData.transactionID;
            timestamp.text = transactionData.timeStamp;
            senderAddress.text = transactionData.senderAddress;
            recipientAddress.text = transactionData.recipientAddress;
            amount.text = transactionData.amount;
            status.text = transactionData.status.ToString();
            blockchainNetwork.text = transactionData.blockchainNetwork;
            transactionFee.text = $"{transactionData.transactionFee}, {transactionData.isFeeApplicable}";

            transactionID.interactable = false;
            timestamp.interactable = false;
            senderAddress.interactable = false;
            recipientAddress.interactable = false;
            amount.interactable = false;
            status.interactable = false;
            blockchainNetwork.interactable = false;
            transactionFee.interactable = false;
        }
    }

    public void CustomizeData()
    {
        transactionID.interactable = true;
        timestamp.interactable = true;
        senderAddress.interactable = true;
        recipientAddress.interactable = true;
        amount.interactable = true;
        status.interactable = true;
        blockchainNetwork.interactable = true;
        transactionFee.interactable = true;
    }

    public TransactionData LoadTransactionData(TransactionData transactionData)
    {
        transactionData = new TransactionData();

        transactionData.transactionID = transactionID.text;
        transactionData.timeStamp = timestamp.text;
        transactionData.senderAddress = senderAddress.text;
        transactionData.recipientAddress = recipientAddress.text;
        transactionData.amount = amount.text;
        transactionData.status = Validity.Validated;
        transactionData.blockchainNetwork = blockchainNetwork.text;
        if (transactionFee.text.Contains(","))
        {
            string text = transactionFee.text;
            string[] parts = text.Split(",", StringSplitOptions.RemoveEmptyEntries);
            transactionData.isFeeApplicable = parts[1] == "true" ? true : false;
            transactionData.transactionFee = parts[0];
        }
        else
        {
            transactionData.transactionFee = transactionFee.text;
        }

        return transactionData;
    }

    public void SaveNewTransactionData()
    {
        if (!AreFieldsEmpty() && dataList.data.Count <= 6)
        {
            Debug.Log($"{GetType().Name}-> {dataList.data.Count} Trasaction Data Saved!");
            dataList.data.Add(LoadTransactionData(dataList.data[dataList.data.Count - 1]));

            string jsonData = JsonUtility.ToJson(dataList, prettyPrint: true);

            string filePath = Path.Combine(Application.dataPath, "Resources/Transaction Panel Data.txt");

            File.WriteAllText(filePath, jsonData);
        }
    }

    public void UpdateTransactionData(int i)
    {
        if (!AreFieldsEmpty())
        {
            Debug.Log($"{GetType().Name}-> Trasaction Data Updated for Transaction {i}!");
            string filePath = "Assets/Resources/Transaction Panel Data.txt";
            //TextAsset json = File.ReadAllText(JsonData.TransactionText.text);
            dataList = JsonUtility.FromJson<TransactionDataList>(JsonData.TransactionText.text);

            if (dataList.data.Count >= i)
            {
                dataList.data[i].senderAddress = senderAddress.text;
                dataList.data[i].recipientAddress = recipientAddress.text;
                dataList.data[i].amount = amount.text;
                dataList.data[i].status = Validity.Validated;
                dataList.data[i].blockchainNetwork = blockchainNetwork.text;
                if (transactionFee.text.Contains(","))
                {
                    string text = transactionFee.text;
                    string[] parts = text.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    dataList.data[i].isFeeApplicable = parts[1] == "true" ? true : false;
                    dataList.data[i].transactionFee = parts[0];
                }
                else
                {
                    dataList.data[i].transactionFee = transactionFee.text;
                }
            }

            string updatedJson = JsonUtility.ToJson(dataList, prettyPrint: true);

            File.WriteAllText(filePath, updatedJson);
        }
    }

    private bool AreFieldsEmpty()
    {
        if (
            senderAddress.text.Equals("") ||
            recipientAddress.text.Equals("") ||
            amount.text.Equals("") ||
            status.text.Equals("") ||
            blockchainNetwork.text.Equals("") ||
            transactionFee.text.Equals("")
        )
        {
            Debug.Log($"{GetType().Name}-> Empty fields!");
            return true;
        }
        else
            return false;
    }
}
