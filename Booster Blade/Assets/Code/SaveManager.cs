using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;

    string recordsFilePath; //where our file will be saved
    string runFilePath;
    //public SaveData activeSave;
    public CurrentRunData currentRunData;
    public RecordsData recordsData;
    public TempLevelData tempLevelData = new TempLevelData();
    public bool hasLoaded = false;

    
    public bool startStageFromBeginning= true;
    public float currentTimeInLevel; //should be reset to 0 when starting level from beginning,

    public bool hasNotBeganLevel = true;
    public bool isGoingToIntermissionFromLevel = false;
    public bool lastSavedAtCheckpoint = false;
    public int tempCheckpointID= -1; 
    //move lastSavedAtCheckpoint over here?
    private void Awake()
    {
        //if no save exists, create one
        recordsFilePath = Application.persistentDataPath + "/BB_records.json";
        runFilePath = Application.persistentDataPath + "/BB_runData.json";
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

        LoadBothData(); // [This means SaveManager Loads data at the start of each scene, I think. Look into this.]
        //maybe don't load at the beginning of each scene
    }
    //Time doesn't reset when hitting a checkpoint
 

    // [Consider looking into constructors--I might need to reset it when deleting progress/hitting new game]
    public void RegisterCheckPoint(int checkPointID)
    {
        tempCheckpointID = checkPointID;
        lastSavedAtCheckpoint = true;
    }

    [System.Serializable]
    public class SaveDataOld
    {

        public Dictionary<int, LevelData> levelDict;

        public float currentLevelTime=0; //time in current stage. if player quits level early this is not saved. only saved after reaching the end.

        //maybe save deaths?

        public SaveDataOld() //default constructor
        {
            levelDict = new Dictionary<int, LevelData>();
        //initialize level data
        }

        public void SetCurrentLevelTime(float time)
        {
            currentLevelTime = time;
        }

        //called when the player reaches the results screen and gets a high score
        public void SaveCompleteLevelData(int lvlIndex, float timeAchieved)
        {
            //maybe handle overwritting logic outside of save system?
            if (levelDict.ContainsKey(lvlIndex))
            {
                levelDict[lvlIndex].bestTime = timeAchieved;

            }
            else
            {
                //not sure if calling the constructor to fill out this information is needed
                levelDict[lvlIndex] = new LevelData(lvlIndex, timeAchieved);
                //The player hasn't beaten this level before! Add the data.
            }
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

    //persists between scenes and reloads but is not saved to memory ever
    public class TempLevelData
    {
        public int tempLevelID;

        public TempLevelData()
        {
            Debug.Log("Created temp data, should only be called once!");
            //resetsStuffIGuess
        }
        //lastSavedAtCheckpoint
        //startingCheckpoint
        //checkpoint position
    }


    //stores data relevant to the run the player is on. cleared when starting new game or wiping save.
    //disregarded if doing time attack
    [System.Serializable]
    public class CurrentRunData
    {
        public int continueIndex; //level that's loaded if you select to continue the game
        bool beatGame = false;
        int totalDeathCount= 0;
        int totalHitsTaken = 0;
        //game should look at this when continuing
        int currentLevelIndex;

        public Dictionary<int, LevelData> currentRunLevelDict;

        //checkpoint stuff? is that necessary?
        public CurrentRunData()
        {
            currentRunLevelDict= new Dictionary<int, LevelData>();
        }
    }

    //stores data relevant to the whole game
    //only cleared when wiping save
    [System.Serializable]
    public class RecordsData
    {
        public Dictionary<int, LevelData> recordLevelDict;
        //lowest death count
        
    }

    private LevelData RetrieveLevelTime(int lvlIndex, Dictionary<int, LevelData> dictToRetrieve)
    {
        if (dictToRetrieve.ContainsKey(lvlIndex))
        {
            return dictToRetrieve[lvlIndex];
        }
        else
        {
            //this makes an empty set of LevelData to look at, I think
            return dictToRetrieve[lvlIndex] = new LevelData();
        }
    }
    public LevelData RetrieveCurrentTime(int lvlIndex)
    {
        return RetrieveLevelTime(lvlIndex, currentRunData.currentRunLevelDict);
    }
    public LevelData RetrieveRecordTime(int lvlIndex)
    {
        return RetrieveLevelTime(lvlIndex, recordsData.recordLevelDict);
    }
    private void SaveLevelTime(int lvlIndex, float timeAchieved, Dictionary<int, LevelData> dictToSave)
    {
        if (recordsData.recordLevelDict.ContainsKey(lvlIndex))
        {
            dictToSave[lvlIndex].bestTime = timeAchieved;

        }
        else
        {
            //not sure if calling the constructor to fill out this information is needed
            dictToSave[lvlIndex] = new LevelData(lvlIndex, timeAchieved);
            //The player hasn't beaten this level before! Add the data.
        }
        //unsure about this, also should probably just be "save record"
        SaveBothData();
    }
    //called when a record at lvlIndex needs to be updated
    public void SaveNewRecord(int lvlIndex, float timeAchieved)
    {
        SaveLevelTime(lvlIndex, timeAchieved, recordsData.recordLevelDict);
    }
    public void SaveCurrentTimes(int lvlIndex, float timeAchieved)
    {
        SaveLevelTime(lvlIndex, timeAchieved, currentRunData.currentRunLevelDict);
    }



    //public void Save()
    //{
    //    string BBData;
    //    BBData = JsonUtility.ToJson(recordsData);
    //    System.IO.File.WriteAllText(Application.persistentDataPath + "/BB_data.json", BBData);
    //    Debug.Log("Should have saved information to JSON.");
    //}

    //Saves both the current run and records to disk
    public void SaveBothData()
    {
        string run;
        run = JsonUtility.ToJson(currentRunData);
        //not 100% sure but I hope you don't need to grab Application.peristentDataPath again here, like if it changed during runtime
        System.IO.File.WriteAllText(runFilePath, run);

        Debug.Log("Should have saved run info to JSON.");

        string records;
        records = JsonUtility.ToJson(recordsData);
        System.IO.File.WriteAllText(recordsFilePath, records);
        Debug.Log("And should have saved records to JSON.");
    }

    //called at the beginning of each scene I think
    public void LoadBothData()
    {
        if (File.Exists(recordsFilePath))
        {
            recordsData = JsonUtility.FromJson<RecordsData>((File.ReadAllText(recordsFilePath)));
        }
        else
        {

            Debug.Log("Couldn't find the records? Creating a new one.");
            recordsData = new RecordsData(); //prob temporary for solving bug

        }
        Debug.Log("Should have loaded recordsdata to JSON.");


        if (File.Exists(runFilePath))
        {
            currentRunData = JsonUtility.FromJson<CurrentRunData>((File.ReadAllText(runFilePath)));
        }
        else
        {

            Debug.Log("Couldn't find the run? Creating a new one.");
            currentRunData = new CurrentRunData(); //prob temporary for solving bug

        }
        Debug.Log("Should have loaded rundata to JSON.");

        hasLoaded = true;

    }
    //public void Load()
    //{
    //    // should probably check if file exists first, not sure if this code is good
    //    if (File.Exists(recordsFilePath))
    //    {
    //        //   activeSave.LoadFightData();
    //        //activeSave = JsonUtility.FromJson<SaveData>((File.ReadAllText(Application.persistentDataPath + "/BB_data.json")));
    //        recordsData = JsonUtility.FromJson<RecordsData>((File.ReadAllText(Application.persistentDataPath + recordsFilePath)));
    //        Debug.Log("Should have loaded data to JSON.");
    //        hasLoaded = true;

    //    }
    //    else
    //    {

    //        Debug.Log("Couldn't find the save? Creating a new one.");
    //        activeSave = new SaveData(); //prob temporary for solving bug
    //                                     //  activeSave.SerializeFightData();

    //        //   activeSave.LoadFightData();
    //        Debug.Log("Should have loaded data to JSON.");
    //        hasLoaded = true;
    //    }

    //}


    //public void DeleteSave()
    //{
    //    if (File.Exists(recordsFilePath))
    //    {
    //        File.Delete(recordsFilePath);
    //        activeSave = new SaveData(); //reset to default values
    //        Debug.Log("Save file deleted");
    //        Save();
    //    }
    //    else
    //    {
    //        activeSave = new SaveData(); //reset to default values

    //        Save();
    //        Debug.Log("There was no save file to delete, but activeSave was overwritten anyway");
    //    }
    //    Load();
    //}

    public void DeleteRunProgress()
    {
        if (File.Exists(runFilePath))
        {
            File.Delete(runFilePath);
            currentRunData = new CurrentRunData(); //reset to default values
            Debug.Log("Current run has been deleted.");
            SaveBothData();
        }
        else
        {
            currentRunData = new CurrentRunData(); //reset to default values

            SaveBothData();
            Debug.Log("There was no save file to delete, but CurrentRun e was overwritten anyway");
        }
        LoadBothData();
    }
   

    public void BeginNewRun()
    {
        //only resets currentData
    }
    public void WipeSave()
    {
        if (File.Exists(runFilePath))
        {
            File.Delete(runFilePath);
            currentRunData = new CurrentRunData(); //reset to default values
            Debug.Log("Current run has been deleted.");
            SaveBothData();
        }
        else
        {
            currentRunData = new CurrentRunData(); //reset to default values

            SaveBothData();
            Debug.Log("There was no save file to delete, but CurrentRun was overwritten anyway");
        }

        if (File.Exists(recordsFilePath))
        {
            File.Delete(runFilePath);
            recordsData = new RecordsData(); //reset to default values
            Debug.Log("Current records has been deleted.");
            SaveBothData();
        }
        else
        {
            recordsData = new RecordsData();//reset to default values

            SaveBothData();
            Debug.Log("There was no save file to delete, but records was overwritten anyway");
        }
        LoadBothData();
        //gets rid of ALL save data, not just for the current run
    }

    // [make a function that sets SaveSpawn]
    // [called when moving to a new scene?]

}
