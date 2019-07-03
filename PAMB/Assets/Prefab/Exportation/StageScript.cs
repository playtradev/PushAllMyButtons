using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageScript : MonoBehaviour {




	public LevelStageClass CurrentLevelStageClass;
	public Camera MainCam;
	public List<UILevelScript> Levels = new List<UILevelScript>();
	public UILevelScript ExtraLevel;
	public int Pos;
	public RectTransform ParentWithMask;
	public bool IsVisible = false;
	public TextMeshProUGUI StageNumber;
	public CanvasGroup canvasGroup;
	public Animator Anim;
	public Vector2Int CurrentAnimState = new Vector2Int();
    
	// Use this for initialization
	void Start () {
		ScrollLevelManagerScript.Instance.UpdateState+= Instance_UpdateState;
	}

    /*
    0,0   +1 = 1,1
    0,0   -1 = 1,-1


    0,1   +1 = 1,-1
    0,1   -1 = 0,0


    0,-1  +1 = 0,0
    0,-1  -1 = 1,1
    */


    void Instance_UpdateState(int state)
	{
		int Value = 0;
		if(CurrentAnimState == new Vector2Int(0,0))
		{
			CurrentAnimState =  state == -1 ? new Vector2Int(1,1) : new Vector2Int(1,-1);
			Value = state == -1 ? 0 : 0;
		}
		else if (CurrentAnimState == new Vector2Int(0, 1))
        {
			CurrentAnimState = state == -1 ? new Vector2Int(1, -1) : new Vector2Int(0, 0);
			Value = state == -1 ? -3 : 0;
        }
		else if (CurrentAnimState == new Vector2Int(0, -1))
        {
			CurrentAnimState = state == -1 ? new Vector2Int(0, 0) : new Vector2Int(1, 1);
			Value = state == -1 ? 0 : 3;
        }
		//Debug.Log(state);
		Pos += Value;
        LoadLevels(Pos);
		UpdateAnim();
	}

    public void EndAnim()
	{
		ScrollLevelManagerScript.Instance.IsMoving = false;
		CurrentAnimState = new Vector2Int(0, CurrentAnimState.y);
		UpdateAnim();
	}

    public void UpdateAnim()
	{
		Anim.SetInteger("UIStageState", CurrentAnimState.x);
		Anim.SetInteger("UIStagePos", CurrentAnimState.y);
	}

	public void UpdateAnim(Vector2Int animValues)
    {
		CurrentAnimState = animValues;
		UpdateAnim();
    }
    
	public void LoadLevels(int pos)
    {
		Pos = pos;
		LoadLevels();
    }

    public void LoadLevels()
	{
		/*if (Pos < LevelStorageManager.Instance.Levels.LevelStage.Count && Pos >= 0)
        {
			StageNumber.text = "Stage " + (Pos + 1);
			CurrentLevelStageClass = LevelStorageManager.Instance.GetLevelSectionFromPosition(Pos);
            FillYourself();
        }*/
	}

    public void FillYourself()
	{
		for (int i = 0; i < Levels.Count; i++)
		{
			//Levels[i].SetUILevele(UILevelsImageManager.Instance.GetSpriteFromID(CurrentLevelStageClass.LevelsInStage[i].LevelID), new Vector2Int(CurrentLevelStageClass.ID, i));
		}

		//ExtraLevel.SetUILevele(UILevelsImageManager.Instance.GetSpriteFromID(CurrentLevelStageClass.ExtraLevel.LevelID), new Vector2Int(CurrentLevelStageClass.ID, 8));
	}
}



/*iOS framework addition failed due to a CocoaPods installation failure.This will will likely result in an non-functional Xcode project.

After the failure, "pod repo update" was executed and failed. "pod install" was then attempted again, and still failed.This may be due to a broken CocoaPods installation.See: https://guides.cocoapods.org/using/troubleshooting.html for potential solutions.

pod install output:

Analyzing dependencies
[!] CocoaPods could not find compatible versions for pod "GoogleAppMeasurement":
  In snapshot (Podfile.lock):
    GoogleAppMeasurement(= 5.8.0, ~> 5.7)

  In Podfile:
    Firebase/Analytics(= 5.20.2) was resolved to 5.20.2, which depends on
      Firebase/Core(= 5.20.2) was resolved to 5.20.2, which depends on
       FirebaseAnalytics(= 5.8.1) was resolved to 5.8.1, which depends on
        GoogleAppMeasurement(= 5.8.1)

    Google-Mobile-Ads-SDK(~> 7.41) was resolved to 7.42.2, which depends on
     GoogleAppMeasurement(~> 5.7)

Specs satisfying the `GoogleAppMeasurement(= 5.8.0, ~> 5.7), GoogleAppMeasurement(~> 5.7), GoogleAppMeasurement(= 5.8.1)` dependency were found, but they required a higher minimum deployment target.


    [33mWARNING: CocoaPods requires your terminal to be using UTF-8 encoding.
    Consider adding the following to ~/.profile:

    export LANG = en_US.UTF - 8
    [0m



pod repo update output:

Updating spec repo `master`
  $ /usr/bin/git -C /Users/DevPlaytra/.cocoapods/repos/master fetch origin --progress
[!] CocoaPods was not able to update the `master` repo.If this is an unexpected issue and persists you can inspect it running `pod repo update --verbose`


    [33mWARNING: CocoaPods requires your terminal to be using UTF-8 encoding.
    Consider adding the following to ~/.profile:

    export LANG = en_US.UTF - 8
    [0m
  fatal: unable to access 'https://github.com/CocoaPods/Specs.git/': The requested URL returned error: 503

UnityEngine.Debug:LogError(Object)
Google.Logger:Log(String, LogLevel) (at Z:/tmp/tmp.EXGG54CvAs/third_party/unity/unity_jar_resolver/source/VersionHandlerImpl/src/Logger.cs:91)
Google.IOSResolver:Log(String, Boolean, LogLevel) (at Z:/tmp/tmp.FJzhDwzYlB/third_party/unity/unity_jar_resolver/source/IOSResolver/src/IOSResolver.cs:938)
Google.IOSResolver:OnPostProcessInstallPods(BuildTarget, String) (at Z:/tmp/tmp.FJzhDwzYlB/third_party/unity/unity_jar_resolver/source/IOSResolver/src/IOSResolver.cs:2140)
UnityEditor.BuildPlayerWindow:BuildPlayerAndRun()*/
