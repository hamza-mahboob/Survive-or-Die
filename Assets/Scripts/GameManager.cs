using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Object = System.Object;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<GameObject> objectives = new List<GameObject>();
    public GameObject gameOverScreen, gameWinScreen;
    public TextMeshProUGUI totalTimeTextWinScreen;
    public TextMeshProUGUI totalTimeTextLoseScreen;

    private float avg, totalTime;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //total time
        totalTime += Time.deltaTime;

        //calculate average of all objectives, if its 100% then all objectives are at 100%
        avg = 0;

        Debug.Log(objectives.Count);
        foreach (GameObject objective in objectives)
        {
            avg = (avg + objective.GetComponent<Objective>().objectiveProgress) / objectives.Count;
            Debug.Log("Avg:" + avg);
            if (avg >= 10)
            {
                //game win
                GameWin();
            }
        }

        //if only killer left in room
        if (PhotonNetwork.PlayerList.Length <= 1 && !PhotonNetwork.IsMasterClient)
            GameOver();
        else if (PhotonNetwork.PlayerList.Length <= 1 && PhotonNetwork.IsMasterClient)
            GameWin();
    }

    public void AddObjectivePhotonPrefab(GameObject objective)
    {
        objectives.Add(objective);
    }

    void GameWin()
    {
        Debug.Log("Game WIN !!");
        //set game win screen active
        totalTimeTextWinScreen.text = "Total Time: " + ((int) totalTime + "s");
        gameWinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    void GameOver()
    {
        Debug.Log("Game OVER !!");
        //set game over screen active
        totalTimeTextLoseScreen.text = "Total Time: " + ((int) totalTime + "s");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void OKButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

        Time.timeScale = 1;
    }
}