using UnityEngine;

public class TransactionDataReader : MonoBehaviour
{
    public TextAsset TransactionText;

    public TransactionDataList dataList;

    private void Awake()
    {
        dataList = JsonUtility.FromJson<TransactionDataList>(TransactionText.text);
    }

    public TransactionData GetTransactionDataList(int listNumber)
    {
        return dataList.data[listNumber];
    }
}
