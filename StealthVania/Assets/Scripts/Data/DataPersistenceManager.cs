using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    
    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager");
        }
        instance = this;
    }
    
    public void NewGame()
    {
        this.gameData.progress = 0;
        this.gameData.respawnx = 0;
        this.gameData.respawny = 0;
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData == null || MainMenu.newgame == true)
        {
            
            Debug.Log("No Game data to load. Creating new game.");
            NewGame();
            return;
        }
        //push loaded data to other scripts
        foreach (IDataPersistence dataPersOb in dataPersistenceObjects)
        {
            dataPersOb.LoadData(gameData);
        }


    }

    public bool hasSave()
    {
        this.gameData = dataHandler.Load();

        Debug.Log(this.gameData.progress);
        return !(this.gameData.progress == 0);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersOb in dataPersistenceObjects)
        {
            dataPersOb.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
