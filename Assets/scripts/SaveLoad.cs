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

    public void SaveInfo(SaveData PSD)
    {
        string savedata = JsonUtility.ToJson(PSD, true);

        File.WriteAllText(saveFile, savedata);
        Debug.Log(savedata);    
    }
    public SaveData LoadInfo()
    {
        if (File.Exists(saveFile))
        {
            string json = File.ReadAllText(saveFile); 
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Loaded Data:" + json);
            return loadedData;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return new SaveData(new List<LevelHighScore>());
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
public struct LevelHighScore{
    public int LevelNumber;
    public int HighScore;
}

[System.Serializable]
public class SaveData
{
    public List<LevelHighScore> HighScores;

    public SaveData(List<LevelHighScore> levelHighScores)
    {
        this.HighScores = levelHighScores;
    }
}

public interface ISaveable
{
    public void Save();
    public void Load();
}
