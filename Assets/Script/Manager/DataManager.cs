using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VInspector;

[Serializable]
public class PlayerData
{
    public int stage = 0;
    public int gold = 0;
}
public class DataManager : MonoBehaviour
{
   
    
    public static DataManager Inst;
    private string _DataFilePath;
    public PlayerData Data = new PlayerData();


    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }

        _DataFilePath = Path.Combine(Application.persistentDataPath, "data.json");
        if (File.Exists(_DataFilePath))
        {
            var playerDataJson = File.ReadAllText(_DataFilePath);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);
            Data = playerData;
        }
        else
        {
            var playerData = new PlayerData();
            Data = playerData;
            Save();
        }
    }

    public void Save()
    {
        if (Data == null) return;
        var json = JsonConvert.SerializeObject(Data, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        File.WriteAllText(_DataFilePath, json);
    }

    public void ResetData()
    {
        
    }

    [Button]
    private void DeleteJson()
    {
        File.Delete(_DataFilePath);
    }
}
