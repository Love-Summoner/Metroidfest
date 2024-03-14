using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    
    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
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
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if(this.gameData == null)
        {
            Debug.Log("No Game data to load. Creating new game.");
            NewGame();
        }
        //push loaded data to other scripts
        foreach (IDataPersistence dataPersOb in dataPersistenceObjects)
        {
            dataPersOb.LoadData(gameData);
        }
        Debug.Log("Loaded progress = " + gameData.progress);


    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersOb in dataPersistenceObjects)
        {
            dataPersOb.SaveData(ref gameData);
        }
        Debug.Log("Saved progress = " + gameData.progress);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
