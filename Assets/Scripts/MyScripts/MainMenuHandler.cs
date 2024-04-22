using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public static MainMenuHandler Instance;


  
    public GameObject waitPanel;
    public TextMeshProUGUI waitPanelTxt;
    public GameObject StartBtn;

    public TMP_InputField roomNameInpField;

    public string sceneName;


    public bool isMasterClient;

    public GameObject multiPlayerObject;

    private void Awake()
    {
        Instance = this;
     
      

    }

    private void OnEnable()
    {
        PhotonHandler.onRoomJoinWithMasterStatus += RoomCreated;
        PhotonHandler.onOtherPlayerEnteredRoom+= PlayerEntered;
    }
    private void OnDisable()
    {
        PhotonHandler.onRoomJoinWithMasterStatus -= RoomCreated;
        PhotonHandler.onOtherPlayerEnteredRoom -= PlayerEntered;

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    public void JoinRoom()
    {
        if (roomNameInpField.text.Length < 3) return;
        PhotonHandler.Instance.JoinRoom(roomNameInpField.text);
    }
    public void CreateRoom()
    {
        PhotonHandler.Instance.CreateRoom(Random.Range(100000,999999).ToString());
    }

    public void LoadScene()
    {
        PhotonHandler.Instance.LoadGameScene(sceneName);
    }
    public void RoomCreated(bool isMaster, bool status, string msg)
    {
        if (status)
        {
            multiPlayerObject.SetActive(true);
            isMasterClient = isMaster;
            if(!isMaster)
            {
                StartBtn.SetActive(false);
            }
            else
            {
                StartBtn.GetComponent<Button>().interactable=false;
            }
            waitPanelTxt.text=msg;
            waitPanel.SetActive(true);
        }
    }

    public void PlayerEntered(string msg)
    {
        if(isMasterClient)
        {
            if(PhotonHandler.Instance.CheckRoomFilledMaster())
            {
              
                StartBtn.GetComponent<Button>().interactable = true;
            }
        }
        waitPanelTxt.text = msg;
    }
}
