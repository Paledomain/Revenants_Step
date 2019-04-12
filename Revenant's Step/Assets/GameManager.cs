using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public enum GameState { Stealth, Combat };
public class GameManager : MonoBehaviour {

    //the game manager
    public static GameManager gm;

    //properties
    public string lastScene;
    public int lastSceneBuildNum;
    public int sceneNo;


    public float stealth = 100;
    public bool gameover = false;

    public GameObject[] turns;//to hold the the turns of the agents
    List<ITurnBased> turnList = new List<ITurnBased>();
    int currentTurn = 0;
    GameState currentState = GameState.Combat;

    private void Awake()
    {
        if(gm == null)//for singleton pattern
        {
            DontDestroyOnLoad(gameObject);
            gm = this;
            foreach (GameObject anObject in turns)//kinda loses singleton pattern, maybe use another class to get this info
            {
                turnList.Add(anObject.GetComponent<ITurnBased>());
            }
        }
        else if (gm != null)
        {
            gm.turns = turns;
            turnList.Clear();
            foreach (GameObject anObject in turns)//this is so that we dont lose our turns list in the case of changing scenes
            {
                turnList.Add(anObject.GetComponent<ITurnBased>());
            }
            Destroy(gameObject);
        }

    }

    //---------------------------------------------------------------//
    //------------------------Change Scenes--------------------------//
    //---------------------------------------------------------------//

    public void SceneChanger(string sceneName)//scene changing facilitator
    {
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        //ChangeToStringScene(sceneName);
        //sceneNo = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeToIntScene(int sceneNo)//change to scene no
    {
        Debug.Log("change to int scene" + sceneNo);
        lastScene = SceneManager.GetActiveScene().name;
        Debug.Log(lastScene);

        SceneManager.LoadScene(sceneNo);
    }

    public void ChangeToStringScene(string sceneName)//change to scene name
    {
        Debug.Log("change to string scene " + sceneName);
        lastScene = SceneManager.GetActiveScene().name;
        lastSceneBuildNum = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(lastScene);

        SceneManager.LoadScene(sceneName);
    }

    public void ChangeToLastScene()//change to the scene stored at lastScene
    //unfortunately this bit seems to be problematic
    {
        Debug.Log("change to last scene" + lastScene);
        if (string.IsNullOrEmpty(lastScene))
        {
            Debug.Log("unable to load last scene");

            Debug.Log(lastScene);
        }
        else
            SceneManager.LoadScene(lastScene);
    }

    public void Quit()//quit
    {
        Application.Quit();
    }

    //---------------------------------------------------------------//
    //------------------------Load and Save--------------------------//
    //---------------------------------------------------------------//

    public void Save()//save
    {
        //preparation
        string path = Application.persistentDataPath + "/saves/";

        BinaryFormatter bf = new BinaryFormatter();
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        FileStream file = File.Create(path + SceneManager.GetActiveScene().name + "_" + System.DateTime.Now.ToString("dd-MM-yy H-mm-ss" + ".rev"));

        SaveData data = new SaveData();//save relevant info
        data.lastScene = lastScene;
        data.sceneNo = sceneNo;

        bf.Serialize(file, data);//finalize
        file.Close();
    }

    public void Load(string filePath)//load
    {
        //preperation
        Debug.Log("Enter load function");
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);//load
            file.Close();

            //finalize
            lastScene = data.lastScene;
            sceneNo = data.sceneNo;
            Debug.Log("Change Scene on Load command");

            ChangeToIntScene(sceneNo);
        }

    }

    //---------------------------------------------------------------//
    //--------------------------Game State---------------------------//
    //---------------------------------------------------------------//

    private void Update()//check game state to enforce turn-based or realtime structure
    {
        if(stealth > 0 && currentState != GameState.Stealth)//entering stealth
        {
            foreach (ITurnBased turnObject in turnList)
            {
                turnObject.StartStealth();
            }
            currentState = GameState.Stealth;
        }
        
        if(stealth <= 0 && currentState != GameState.Combat)//entering combat
        {
            currentState = GameState.Combat;
            foreach (ITurnBased turnObject in turnList)
            {
                turnObject.TransitionToCombat();
            }
        }

        if(currentState == GameState.Combat)//combat
        {
            if (turnList[currentTurn].DoTurn())
            {
                currentTurn++;//count up turn list to do each agent's turn
                if (currentTurn >= turnList.Count)
                {
                    currentTurn = 0;
                }
                turnList[currentTurn].StartTurn();
            }

        }
        else//stealth
        {
            foreach (ITurnBased turnObject in turnList)
            {
                turnObject.doStealth();
            }
        }

    }

    public void GameOver(bool yes)//gameover, tbi
    {
        gameover = yes;
    }

}

[System.Serializable]
class SaveData//data inside the save file
{
    public string lastScene;
    public int sceneNo;
}



