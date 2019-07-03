using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using System;

public class LevelStorageManager : MonoBehaviour
{

	public static LevelStorageManager Instance;

	public LevelStorageClass Levels;
	public StagesCompletionClass StagesCompletion;
	bool SetUp = false;

	private void Awake()
	{
		Instance = this;
		/*Levels = new LevelStorageClass();
		PlaytraGamesLtd.Utils.Serialize<LevelStorageClass>(Levels,  System.IO.Path.Combine(Application.streamingAssetsPath, "LevelsStorage.xml"));
		Levels = null;*/
#if UNITY_EDITOR
		Levels = PlaytraGamesLtd.Utils.DeserializeResourcesCSV("LevelsStorage");
		//Levels = PlaytraGamesLtd.Utils.DeserializeStreamingAssetsCSV(System.IO.Path.Combine(Application.streamingAssetsPath, "LevelsStorage.csv"));
#elif UNITY_ANDROID && !UNITY_EDITOR
		//Levels = PlaytraGamesLtd.Utils.DeserializeResourcesCSV("LevelsStorage.csv");
		Levels = PlaytraGamesLtd.Utils.DeserializeResourcesCSV("LevelsStorage");
#elif UNITY_IOS && !UNITY_EDITOR
		//Levels = PlaytraGamesLtd.Utils.DeserializeCSV(System.IO.Path.Combine(Application.streamingAssetsPath, "LevelsStorage.csv"));
		Levels = PlaytraGamesLtd.Utils.DeserializeResourcesCSV("LevelsStorage");
#endif
	}

    public void UnlockAllStages()
    {
        foreach (StageCompletionClass item in StagesCompletion.Stages)
        {
            foreach (LevelCompletionCLass level in item.Levels)
            {
                level.LevelCompletion = LevelsCompletionType.Unlock;
            }

            item.ExtraLevel.LevelCompletion = LevelsCompletionType.Unlock;
        }
    }

    public void LockAllStages()
    {
        foreach (StageCompletionClass item in StagesCompletion.Stages)
        {
            if (!item.Equals(StagesCompletion.Stages[0]))
            {
                foreach (LevelCompletionCLass level in item.Levels)
                {
                    level.LevelCompletion = LevelsCompletionType.Lock;
                }

                item.ExtraLevel.LevelCompletion = LevelsCompletionType.Lock;
            }
            
        }
    }

    private void Update()
	{
		if(Levels != null && !SetUp)
		{
			SetUp = true;
			string res = PlayerPrefs.GetString("StageCompletion");
		    if(string.IsNullOrEmpty(res))
    		{
				StagesCompletion = new StagesCompletionClass();
		        foreach (LevelStageClass stage in Levels.LevelStage)
				{
		            StagesCompletion.Stages.Add(new StageCompletionClass());

				}

		        foreach (StageCompletionClass stage in StagesCompletion.Stages)
                {
		            stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
            		stage.Levels.Add(new LevelCompletionCLass());
		            stage.ExtraLevel = new LevelCompletionCLass();
                }

		        StagesCompletion.Stages[0].Levels[0].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[1].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[2].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[3].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[4].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[5].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[6].LevelCompletion = LevelsCompletionType.Unlock;
                StagesCompletion.Stages[0].Levels[7].LevelCompletion = LevelsCompletionType.Unlock;
		StagesCompletion.Stages[0].ExtraLevel.LevelCompletion = LevelsCompletionType.Unlock;

				PlayerPrefs.SetString("StageCompletion", PlaytraGamesLtd.Utils.SerializeToString<StagesCompletionClass>(StagesCompletion));
    		}
			else
    		{
				StagesCompletion = PlaytraGamesLtd.Utils.DeserializeFromString<StagesCompletionClass>(res);
    		}
		}
	}


	public void SaveStagesCompletion()
	{
		PlayerPrefs.SetString("StageCompletion", PlaytraGamesLtd.Utils.SerializeToString<StagesCompletionClass>(StagesCompletion));
	}

	public LevelStageClass GetLevelSectionFromID(int levelSectionid)
	{
		return Levels.LevelStage.Where(r => r.ID == levelSectionid).First();
	}

	public LevelStageClass GetLevelSectionFromPosition(int levelPos)
    {
		return Levels.LevelStage.Where(r => r.StagePosition == levelPos).First();
    }

	public LevelClass GetLevelClassFromID(int levelSectionid, int levelClassid)
    {
		LevelStageClass section = GetLevelSectionFromID(levelSectionid);
		if(section.ExtraLevel.LevelID == levelClassid)
		{
			return section.ExtraLevel;
		}
		else
		{
			return section.LevelsInStage.Where(r=> r.LevelID == levelClassid).First();

		}
    }
}
