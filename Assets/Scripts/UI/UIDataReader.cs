using UnityEngine;

public class UIDataReader : MonoBehaviour
{
    public TextAsset UIText;

    public UIDataList dataList;

    private void Awake()
    {
        dataList = JsonUtility.FromJson<UIDataList>(UIText.text);
    }

    public UIData GetUIDataList(int listNumber)
    {
        return dataList.data[listNumber];
    }
}
