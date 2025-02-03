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
    public int stage = 1;
    public int gold = 100;
    public List<int> relicID = new List<int>();
    public int cardCount = 4;
    public int cardRepeat = 1;
    public int characterId = -1;
    public bool moblieVersion = false;

}
public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    private string _DataFilePath;
    public PlayerData Data = new PlayerData();
    public bool bossStage = false;


    protected override void Awake()
    {
        base.Awake();

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
        yield return new WaitUntil(() => CharacterRelicData.Inst && TopUIController.Inst);
        if (Data.characterId >= 0)
        {
            //playableNpcManager.curNpcIndex = Data.characterId;
            //playableNpcManager.SetNpcPlayer(Data.characterId);
        }
       
        CharacterRelicData.Inst.LoadPlayerRelic(Data.relicID);
        if(TopUIController.Inst == null)
            Debug.Log("없음");
        TopUIController.Inst.InstanceRelicIcon(Data.relicID,false);
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
        bossStage = false;
        Data = new PlayerData();
        TopUIController.Inst.Initialize();
        Save();
        
        StartCoroutine(LoadPlayerRelic());
    }

    [Button]
    private void DeleteJson()
    {
        File.Delete(_DataFilePath);
    }
}
