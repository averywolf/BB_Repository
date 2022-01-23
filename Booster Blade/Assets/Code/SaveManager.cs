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
        public int continueIndex; //index of the level that's loaded if you select to continue the game, might need to be set independently of LevelData
        bool beatGame = false;
        int totalDeathCount= 0;
        int totalHitsTaken = 0;

        public List<LevelData> levelDatas;
        //game should look at this when continuing
        int currentLevelIndex;

        public Dictionary<int, LevelData> currentRunLevelDict;

        //checkpoint stuff? is that necessary?
        public CurrentRunData()
        {
            currentRunLevelDict= new Dictionary<int, LevelData>();
        }

        public void SetContinueIndex(int indexToSet)
        {
            continueIndex = indexToSet;
        }
        public void SerializeLevelData()
        {
            levelDatas = new List<LevelData>();
            foreach (KeyValuePair<int, LevelData> info in currentRunLevelDict)
            {
                levelDatas.Add(info.Value);
            }
        }

        //might have something to do with how dictionaries are ordered?
        public void LoadCurrentRunDict()
        {
            currentRunLevelDict = new Dictionary<int, LevelData>();
            for (int i = 0; i < levelDatas.Count; i++)
            {
                currentRunLevelDict[levelDatas[i].levelIndex] = levelDatas[i];
            }
        }

    }
    /// <summary>
    /// In Bullet Catcher
    /// - You beat a level, UpdateBestTime is called for the level's index with the time you got. (should do the similar with LevelManager)
    /// - Update best time sets the values in fightDict for that level index
    /// - Do the comparison for if the time is better right there?
    /// - Save() is called, which before writing it to JSON uses SerializeFightData()
    /// - SerializeFightData populates fightdata based on what's in the fightDict
    /// WHAT IS FIGHTDATA???
    /// Fightdata appears to be unnecessary, unless you want to see the information in levelDict in the inspector.
    /// </summary>


    //stores data relevant to the whole game
    //only cleared when wiping save
    [System.Serializable]
    public class RecordsData
    {
        public Dictionary<int, LevelData> recordLevelDict;
        public List<LevelData> levelDatas;

        //also should store lowest death count
        public RecordsData()
        {
            recordLevelDict = new Dictionary<int, LevelData>();
        }
        ////resets levelDatas to then fill it up with the information in the dictionary
        //fills this up so it can be saved to JSON (dictionaries can't be saved, but lists can)
        public void SerializeRecordsData()
        {
            levelDatas = new List<LevelData>();
            foreach (KeyValuePair<int, LevelData> info in recordLevelDict)
            {
                levelDatas.Add(info.Value);
            }
        }

        //might have something to do with how dictionaries are ordered?
        public void LoadRecordsDict()
        {
            recordLevelDict = new Dictionary<int, LevelData>();
            for (int i = 0; i < levelDatas.Count; i++)
            {
                recordLevelDict[levelDatas[i].levelIndex] = levelDatas[i];
            }
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

    //Based on RetrieveFightData, likely will not work without LoadingFightData first
    private LevelData RetrieveLevelTime(int lvlIndex, Dictionary<int, LevelData> dictToRetrieve)
    {
       if(dictToRetrieve == null)
        {
            Debug.LogWarning("NO DICT!");
        }

        if (dictToRetrieve.ContainsKey(lvlIndex))
        {
            return dictToRetrieve[lvlIndex];
        }
        else
        {
            Debug.LogWarning("Could not find leveldata at key" + lvlIndex);
            //this makes an empty set of LevelData to look at, I think
            return dictToRetrieve[lvlIndex] = new LevelData();
        }
    }
    public LevelData RetrieveCurrentTime(int lvlIndex)
    {
        //only need to call LoadDict here since the dictionary won't matter outside of looking at times (since it's the level data list that's viewable in inspector and always updates when saving)
        currentRunData.LoadCurrentRunDict();
        return RetrieveLevelTime(lvlIndex, currentRunData.currentRunLevelDict);
    }
    public LevelData RetrieveRecordTime(int lvlIndex)
    {
        recordsData.LoadRecordsDict();
        Debug.Log("Looking for best time at index " + lvlIndex);
        //for (int i = 0; i <recordsData.recordsLevelDatas.Count; i++)
        //{
        //    if(recordsData.recordsLevelDatas[i].levelIndex== lvlIndex)
        //    {
        //        return recordsData.recordsLevelDatas[i];
        //    }
        //}
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
            Debug.Log("No data found at " + lvlIndex + ", creating it and setting the time as " + timeAchieved +" now.");
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
       // recordsData.recordsLevelDatas.Add(new LevelData(lvlIndex, timeAchieved));

    }
    public void SaveCurrentTimes(int lvlIndex, float timeAchieved)
    {
        SaveLevelTime(lvlIndex, timeAchieved, currentRunData.currentRunLevelDict);
    }

    //Saves both the current run and records to disk
    public void SaveBothData()
    {
        currentRunData.SerializeLevelData();
        string run;
        run = JsonUtility.ToJson(currentRunData);
        //not 100% sure but I hope you don't need to grab Application.peristentDataPath again here, like if it changed during runtime
        System.IO.File.WriteAllText(runFilePath, run);

        recordsData.SerializeRecordsData();
        string records;
        records = JsonUtility.ToJson(recordsData);
        System.IO.File.WriteAllText(recordsFilePath, records);
    }
    
    //called at the beginning of each scene I think
    //might need to Load Level Data here, it doesn't appear to show up in the inspector yet
    public void LoadBothData()
    {
        if (File.Exists(recordsFilePath))
        {
            recordsData = JsonUtility.FromJson<RecordsData>((File.ReadAllText(recordsFilePath)));
            
        }
        else
        {
            recordsData = new RecordsData(); //prob temporary for solving bug
            
        }

        if (File.Exists(runFilePath))
        {
            currentRunData = JsonUtility.FromJson<CurrentRunData>((File.ReadAllText(runFilePath)));
        }
        else
        {
            currentRunData = new CurrentRunData();
            //might need to serialize here?
        }
        hasLoaded = true;

    }

 


    public void SetUpSavesAtLevelStart(int curSceneIndex)
    {
        currentRunData.SetContinueIndex(curSceneIndex);

        SaveBothData();
        
    }
    public void LogLevelCompletionData(int curLevelIndex, float endTime)
    {   // Called at the end of the level by level manager before moving to intermission. updates the current times and results
        // Maybe will create a variation that doesn't save currentTime (like for time trials?)
        Debug.Log("Completion time was: " + endTime + ". Saving data for lvlIndex " +curLevelIndex + ".");
        currentTimeInLevel = endTime; //unsure about this

       
        SaveCurrentTimes(curLevelIndex, endTime);
        float timeToBeat = RetrieveRecordTime(curLevelIndex).bestTime;

        if (timeToBeat == 999999)
        {
            Debug.Log("First time clearing level " + curLevelIndex);
            SaveNewRecord(curLevelIndex, endTime);
        }
        else if(endTime < timeToBeat)
        {//player beat their record!
            SaveNewRecord(curLevelIndex, endTime);
            //have a variable called Old best time? intermission might look at that
        }
        else
        {

        }

        lastSavedAtCheckpoint = false;
        isGoingToIntermissionFromLevel = true;
        SaveBothData(); //sure? levelmanager might have been missing this
    }

    //Called when starting a New Game
    public void DeleteRunProgress()
    {
        if (File.Exists(runFilePath))
        {
            File.Delete(runFilePath);
            currentRunData = new CurrentRunData(); //reset to default values
            SaveBothData();
        }
        else
        {
            currentRunData = new CurrentRunData(); //reset to default values

            SaveBothData();
        }
        LoadBothData();
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
