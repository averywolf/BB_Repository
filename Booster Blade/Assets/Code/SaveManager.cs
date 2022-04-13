using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public CurrentRunData currentRunData;
    public RecordsData recordsData;

    string recordsFilePath; //where our Records file will be saved
    string runFilePath;//where our Run file will be saved

    //review the variables below

    public bool startStageFromBeginning= true;
    public float currentTimeInLevel; //should be reset to 0 when starting level from beginning,

    //value resets on Start() from the Main Menu and Intermission scenes.
    public bool hasNotBeganLevel = true;
    public bool isGoingToIntermissionFromLevel = false;
    public bool lastSavedAtCheckpoint = false;
    public int tempCheckpointID= -1;
    public float oldBestTime = 999999; //just used to tranfer the previous best time over to the intermission, the results are saved once you beat the level

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

    //for saving, add all the Values inside the dictionary to a List<LevelScore> and 
    //serialize that to show the score, all you would have to do is populate the dictionary
    //upon loading, and then check if there's a score for the level in question.

   

    //Stores data relevant to the run the player is on. cleared when starting new game or wiping save.
    [System.Serializable]
    public class CurrentRunData
    {
        public int continueIndex; //index of the level that's loaded if you select to continue the game, might need to be set independently of LevelData
        bool beatGame = false;
        //int totalDeathCount= 0;
        //int totalHitsTaken = 0;

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

    //Based on RetrieveFightData, likely will not work without LoadingFightData first
    private LevelData RetrieveLevelData(int lvlIndex, Dictionary<int, LevelData> dictToRetrieve)
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
            Debug.LogWarning("COULD NOT FIND LEVELDATA AT KEY " + lvlIndex);
            //need to add empty set of leveldata
            LevelData dataToRetrive = new LevelData(lvlIndex);
            return dictToRetrieve[lvlIndex] = dataToRetrive; // probably should throw exception
        }
    }
    
    public LevelData RetrieveCurrentData(int lvlIndex)
    {
        //only need to call LoadDict here since the dictionary won't matter outside of looking at times (since it's the level data list that's viewable in inspector and always updates when saving)
        currentRunData.LoadCurrentRunDict();
        return RetrieveLevelData(lvlIndex, currentRunData.currentRunLevelDict);
    }
    public LevelData RetrieveRecordData(int lvlIndex)
    {
        recordsData.LoadRecordsDict();
        Debug.Log("Looking for best time at index " + lvlIndex);

        return RetrieveLevelData(lvlIndex, recordsData.recordLevelDict);
    }

    //This saves the time for the specific dictionary called (logic is the same for Run and Record)
    private void SaveLevelTime(int lvlIndex, float timeAchieved, Dictionary<int, LevelData> dictToSave)
    {
        if (dictToSave.ContainsKey(lvlIndex))
        {
            dictToSave[lvlIndex].timeBeaten = timeAchieved;
        }
        else
        {
            Debug.Log("No data found at " + lvlIndex + ", creating it and setting the time as " + timeAchieved +" now.");
            dictToSave[lvlIndex] = new LevelData(lvlIndex, timeAchieved);
            //The player hasn't beaten this level before! Add the data.
        }

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

    //Saves both the current run and records to disk. called more often then maybe necessary
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
            Debug.Log("test records load");
            recordsData.LoadRecordsDict(); //testing, apprears to help but leads to a bug
        }
        else
        {
            recordsData = new RecordsData(); //prob temporary for solving bug
            
        }

        if (File.Exists(runFilePath))
        {
            currentRunData = JsonUtility.FromJson<CurrentRunData>((File.ReadAllText(runFilePath)));
            currentRunData.LoadCurrentRunDict();
        }
        else
        {
            currentRunData = new CurrentRunData();
            //might need to serialize here?
        }
    }

 
    //conzolodate into rezet temp data?

    public void SetUpSavesAtLevelStart(int curSceneIndex)
    {
        currentRunData.SetContinueIndex(curSceneIndex);
        oldBestTime = 999999; //reset at beginning of stage? might need to be reset at another point
        SaveBothData(); //should just save continue index, I don't think it should cause further issues
        
    }
    /// <summary>
    /// Called at the end of the level by LevelManager before moving to Intermission scene. (when player beats a stage/skips it through the debug P command)
    /// </summary>
    /// <param name="curLevelIndex"></param>
    /// <param name="endTime"></param>
    public void LogLevelCompletionData(int curLevelIndex, float endTime)
    {   
        Debug.Log("Completion time was: " + endTime + ". Saving data for lvlIndex " +curLevelIndex + ".");
        currentTimeInLevel = endTime; //unsure about this
        SaveCurrentTimes(curLevelIndex, endTime);

        float timeToBeat = RetrieveRecordData(curLevelIndex).timeBeaten;
        
        //only need to show old record if you beat the level with a better record than before
        if (timeToBeat == 999999)
        {
            Debug.Log("First time clearing level " + curLevelIndex + " during this run.");
      
        }
        else if(endTime < timeToBeat)
        {
            //player beat their record!
            //if (RetrieveRecordData(curLevelIndex).hasLevelBeenBeaten) //no point in updating oldBestTime if they haven't beaten level before
            //{
               

        }
        oldBestTime = timeToBeat;
        SaveNewRecord(curLevelIndex, endTime);
        lastSavedAtCheckpoint = false;
        isGoingToIntermissionFromLevel = true;
        RetrieveRecordData(curLevelIndex).hasLevelBeenBeaten = true;
        SaveBothData(); //sure? levelmanager might have been missing this
    }
    //
    
    //Called when starting a New Game
    public void DeleteRunProgress()
    {
        if (File.Exists(runFilePath))
        {
            File.Delete(runFilePath);
            currentRunData = new CurrentRunData(); //reset to default values
            Debug.Log("CURRENT RUN DELETED");
            SaveBothData();
        }
        else
        {
            currentRunData = new CurrentRunData(); //reset to default values
            Debug.Log("CURRENT RUN DELETED");
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


    //need to figure out when to set the continue index

    //call this in levelmanager to save
    public void SaveCollectibleStatus(int curLevelIndex, bool gotCollectible)
    {
        RetrieveCurrentData(curLevelIndex).gotStageCollectible = gotCollectible;
    }
    public void FillWithEmptyCurrentData()
    {
        //might be useful to establish in New Game after wiping save
    }
}
