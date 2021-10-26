﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{
    //FightIndex
    //drop player in same position after each fight ends

    // This is a singleton
    // static = there can only be one of this variable
    public static SaveManager instance;
    string filePath; //where our file will be saved

    public SaveData activeSave;

    public bool hasLoaded = false;

    string BBData;
    public bool startStageFromBeginning= true;

    private void Awake()
    {
        //if no save exists, create one

        Debug.Log("Savemanager is being called after reloading scene");

        //choses a place in the system directory that won't change
        //apparently, you can name the data whatever you want
        //filePath = Application.persistentDataPath + "/save.data";
        filePath = Application.persistentDataPath + "/BB_data.json";
        // Check there are no other copies of this class in the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        Load(); // [This means SaveManager Loads data at the start of each scene, I think. Look into this.]
    }
    //Time doesn't reset when hitting a checkpoint


    // [Consider looking into constructors--I might need to reset it when deleting progress/hitting new game]


    [System.Serializable]
    public class SaveData
    {
        public bool lastSavedAtCheckpoint=false;
        // public Dictionary<int, FightData> fightDict;
        public float spawnX = 0.0f;
        public float spawnY = 0.0f;
        public string lastLevel = "";
        // public List<FightData> fightDatas;
        public SaveData() //default constructor
        {
          //  fightDict = new Dictionary<int, FightData>();
          //  fightDatas = new List<FightData>();
        }
        public void RegisterRespawnPoint(Vector3 checkpointPosition, string sceneName)
        {
            spawnX = checkpointPosition.x;
            spawnY = checkpointPosition.y;
            lastLevel = sceneName;
            lastSavedAtCheckpoint = true;
            Debug.Log("spawnX= " + spawnX + " spawnY= " + spawnY);
        }

        //public FightData RetrieveFightData(int levelIndex)
        //{
        //    //    LoadFightData();
        //    //    if (fightDict.ContainsKey(levelIndex))
        //    //    {
        //    //        return fightDict[levelIndex];
        //    //    }
        //    //    else
        //    //    {
        //    //        //fightDict[levelIndex] = new FightData(levelIndex);
        //    //        return null;//fightDict[levelIndex];
        //    //    }
        //    return null;
        //}
    //public void SerializeFightData()
    //    {
    //        //    fightDatas = new List<FightData>();
    //        //    foreach (KeyValuePair<int, FightData> info in fightDict)
    //        //    {
    //        //        fightDatas.Add(info.Value);
    //        //        //might need to look at key?
    //        //    }
    //    }
        ////Fill up dictionary with fightdata
        //public void LoadFightData()
        //{
        //    fightDict = new Dictionary<int, FightData>();
        //    for (int i = 0; i < fightDatas.Count; i++)
        //    {
        //        fightDict[fightDatas[i].levelIndex] = fightDatas[i];
        //        //fightDict[fightDatas[i].levelIndex]=
        //    }
        //}

        //public void UpdateBestTime(int levelIndex, float achievedTime, bool usedAssists)
        //{
        //    if (fightDict.ContainsKey(levelIndex))
        //    {
        //        //prioritize times that you got without assists
        //        if (fightDict[levelIndex].bestTime > achievedTime || fightDict[levelIndex].beatWithAssists && !usedAssists)
        //        {
        //            if (!fightDict[levelIndex].beatWithAssists && usedAssists)
        //            {
        //                //don't update with new time, even if the one you did with assists got a better time
        //            }
        //            else
        //            {
        //                fightDict[levelIndex].bestTime = achievedTime;
        //                fightDict[levelIndex].beatWithAssists = usedAssists;
        //            }
        //        }
        //        fightDict[levelIndex].bugWasCaught = true; //registers bug as caught
        //    }
        //    else
        //    {
        //        //If there's not any data there, add some
        //        fightDict[levelIndex] = new FightData(levelIndex, achievedTime, usedAssists);
        //        fightDict[levelIndex].bugWasCaught = true;
        //    }
        //}

    }
    //for saving, add all the Values inside the dictionary to a List<LevelScore> and 
    //serialize that to show the score, all you would have to do is populate the dictionary
    //upon loading, and then check if there's a score for the level in question.


    public void Save()
    {
      //  activeSave.SerializeFightData();
        BBData = JsonUtility.ToJson(activeSave);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/BB_data.json", BBData);
        Debug.Log("Should have saved information to JSON.");

    }

    public void Load()
    {
        // should probably check if file exists first, not sure if this code is good
        if (File.Exists(filePath))
        {
         //   activeSave.LoadFightData();
            activeSave = JsonUtility.FromJson<SaveData>((File.ReadAllText(Application.persistentDataPath + "/BB_data.json")));
            Debug.Log("Should have loaded data to JSON.");
            hasLoaded = true;

        }
        else
        {

            Debug.Log("Couldn't find the save? Creating a new one.");
            activeSave = new SaveData(); //prob temporary for solving bug
          //  activeSave.SerializeFightData();

         //   activeSave.LoadFightData();
            Debug.Log("Should have loaded data to JSON.");
            hasLoaded = true;
        }

    }

    public void DeleteSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            activeSave = new SaveData(); //reset to default values
            Debug.Log("Save file deleted");
            Save();
        }
        else
        {
            activeSave = new SaveData(); //reset to default values

            Save();
            Debug.Log("There was no save file to delete, but activeSave was overwritten anyway");
        }
        Load();
    }

    // [make a function that sets SaveSpawn]
    // [called when moving to a new scene?]

}
