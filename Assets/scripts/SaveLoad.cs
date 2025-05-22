using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;
    public string saveFile;
        
    private void Awake() {

        instance = this;

        saveFile = Application.persistentDataPath + "/Save.json";

    }
    public void SaveInfo(Dictionary<int, int> dict)
{
    SaveData data = new SaveData(dict);
    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(saveFile, json);
    Debug.Log("Saved Data:" + json);
}
    
    public Dictionary<int, int> LoadInfo()
    {
        if (File.Exists(saveFile))
        {
            string json = File.ReadAllText(saveFile); 
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Loaded Data:" + json);
            return loadedData.ToDictionary();
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return new Dictionary<int, int>();
        }
    }


#if UNITY_EDITOR
    private void HandleModeChanged(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingPlayMode)
        {
            ISaveable[] saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();

            foreach (ISaveable saveable in saveables)
            {
                saveable.Save();
            }

            Debug.Log("Saving Data Exiting Play Mode");
            //Save();
        }
    }
#endif

    private void OnApplicationQuit()
    {
        ISaveable[] savableObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();


        foreach (ISaveable savableObject in savableObjects) 
        {
            savableObject.Save();
        }

    }

}

[System.Serializable]
public struct KeyValue
{
    public int key;
    public int value;

    public KeyValue(int k, int v)
    {
        key = k;
        value = v;
    }
}
[System.Serializable]
public class SaveData
{
    public List<KeyValue> keyValueList;

    public SaveData(Dictionary<int, int> dict)
    {
        keyValueList = new List<KeyValue>();
        foreach (var kvp in dict)
        {
            keyValueList.Add(new KeyValue(kvp.Key, kvp.Value));
        }
    }

    public Dictionary<int, int> ToDictionary()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        foreach (var kv in keyValueList)
        {
            dict[kv.key] = kv.value;
        }
        return dict;
    }
}

public interface ISaveable
{
    public void Save();

}
