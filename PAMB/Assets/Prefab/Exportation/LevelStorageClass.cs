using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStorageClass
{
	public List<LevelStageClass> LevelStage;

    public LevelStorageClass()
	{

		LevelStage = new List<LevelStageClass>();
		/*LevelStage = new List<LevelStageClass>();
		LevelStage.Add(new LevelStageClass());
		LevelStage.Add(new LevelStageClass());
		LevelStage.Add(new LevelStageClass());*/
	}

	public LevelStorageClass(List<LevelStageClass> levelStage)
    {
		LevelStage = levelStage;
    }


}


public class LevelStageClass
{
	public int ID;
	public int StagePosition;
	public List<LevelClass> LevelsInStage;
	public LevelClass ExtraLevel;
	public int UnlockedLevels;
	public bool PassToNextStage = false;
	public bool IsCircuitVisible = true;


	public LevelStageClass()
	{
		LevelsInStage = new List<LevelClass>();
		/*LevelsInStage = new List<LevelClass>();
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		LevelsInStage.Add(new LevelClass());
		ExtraLevel = new LevelClass();*/
	}

	public LevelStageClass(int id, List<LevelClass> levelsInStage, LevelClass extraLevel)
    {
		ID = id;
		LevelsInStage = levelsInStage;
		ExtraLevel = extraLevel;
    }

}


public class LevelClass
{

	public int LevelID;
	public int TargetScore;

	public LevelClass()
	{

	}


	public LevelClass(int levelID, int targetScore)
	{
		LevelID = levelID;
		TargetScore = targetScore;
	}
   
}


public class StagesCompletionClass
{
	public List<StageCompletionClass> Stages;

    public StagesCompletionClass()
	{
		Stages = new List<StageCompletionClass>();
	}
}


public class StageCompletionClass
{
	public List<LevelCompletionCLass> Levels;
	public LevelCompletionCLass ExtraLevel;

    public StageCompletionClass()
	{
		Levels = new List<LevelCompletionCLass>();
		ExtraLevel = new LevelCompletionCLass();
	}

}

public class LevelCompletionCLass
{
	public LevelsCompletionType LevelCompletion = LevelsCompletionType.Unlock;
}


public enum SphereType
{
	Normal
}

public enum LevelsCompletionType
{
	Lock,
    Unlock,
    Complete,
    Mastered
}
