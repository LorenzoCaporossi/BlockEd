using System;
using System.Collections.Generic;

public enum Validity
{
    Validated,
    Unvalidated,
    pendingValidation
}

[System.Serializable]
public class TransactionData
{
    //public int panelID;
    public string transactionID;
    public string timeStamp;
    public string senderAddress;
    public string recipientAddress;
    public string amount;
    public Validity status;
    public string blockchainNetwork;
    public bool isFeeApplicable;
    public string transactionFee;
}

[System.Serializable]
public class TransactionDataList
{
    public List<TransactionData> data;
}