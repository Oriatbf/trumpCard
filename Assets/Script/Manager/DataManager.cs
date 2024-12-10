using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VInspector;

[Serializable]
public class PlayerData
{
    public int stage = 0;
    public int gold = 0;
    public List<int> relicID = new List<int>();
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
            StartCoroutine(LoadPlayerRelic());
        }
        else
        {
            var playerData = new PlayerData();
            Data = playerData;
        }
    }

    IEnumerator LoadPlayerRelic()
    {
        yield return new WaitUntil(() => CharacterRelicData.Inst);
        
        CharacterRelicData.Inst.LoadPlayerRelic(Data.relicID);
    }

    public void Save()
    {
        if (Data == null) return;
        
        var json = JsonConvert.SerializeObject(Data, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        File.WriteAllText(_DataFilePath, json);
    }

    [Button]
    public void ResetData()
    {
        Data = new PlayerData();
        Save();
        StartCoroutine(LoadPlayerRelic());
    }

    [Button]
    private void DeleteJson()
    {
        File.Delete(_DataFilePath);
    }
}
