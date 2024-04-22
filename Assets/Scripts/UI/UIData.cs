using System.Collections.Generic;

[System.Serializable]
public class UIData
{
    public int panelID;
    public string title;
    public string[] description;
    public string[] image;
    public string button1;
    public string button2;
}

[System.Serializable]
public class UIDataList
{
    public List<UIData> data;
}