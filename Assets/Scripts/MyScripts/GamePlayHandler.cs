using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayHandler : MonoBehaviour
{
    public static GamePlayHandler Instance;
    public GameObject multiplayerCanvas;
   public void SetIsMultiPlayer(bool val)
    {
        GameManagerMy.Instance.isMultiplayer = val;
    }
    public void CheckMultiPlayer()
    {
        if(GameManagerMy.Instance.isMultiplayer)
        {
            multiplayerCanvas.SetActive(true);
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
