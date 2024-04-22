using UnityEngine;

public class ChallengeDataReader : MonoBehaviour
{
    public TextAsset UIText;

    public ChallengeData dataList;

    private void Awake()
    {
        dataList = JsonUtility.FromJson<ChallengeData>(UIText.text);
    }
}
