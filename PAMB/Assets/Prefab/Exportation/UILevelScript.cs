using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelScript : MonoBehaviour 
{
	public Image LockedLevelSprite;
	private Image LevelImage;
	public Vector2Int LevelID;
    public Animator Star;
    public Animator CardColor;
	public TextMeshProUGUI UILevelNum;
	public LevelsCompletionType Completion;
	public Animator ExtraLevelAnim;
    

    int score = 0;

	private void Awake()
	{
		LevelImage = GetComponent<Image>();
	}

	public void SetUILevele(Sprite lvl, Vector2Int levelID)
	{
        
        LevelID = levelID;
		Completion = levelID.y < 8 ? LevelStorageManager.Instance.StagesCompletion.Stages[(int)levelID.x].Levels[(int)levelID.y].LevelCompletion
		                    : LevelStorageManager.Instance.StagesCompletion.Stages[(int)levelID.x].ExtraLevel.LevelCompletion;
        //Debug.Log(levelID + "   " + Completion.ToString());
        if (LevelImage != null)
        {
            LevelImage.sprite = lvl;
            UILevelNum.text = ((LevelID.x * 8) + LevelID.y + 1).ToString();
            if (LevelID.y == 8 && ExtraLevelAnim != null)
            {
                switch (Completion)
                {
                    case LevelsCompletionType.Lock:
                        LevelImage.raycastTarget = false;
                        ExtraLevelAnim.SetInteger("UIState", 0);
                        break;
                    case LevelsCompletionType.Unlock:
                        LevelImage.raycastTarget = true;
                        ExtraLevelAnim.SetInteger("UIState", 2);
                        break;
                    case LevelsCompletionType.Complete:
                        LevelImage.raycastTarget = true;
                        ExtraLevelAnim.SetInteger("UIState", 2);
                        break;
                    case LevelsCompletionType.Mastered:
                        LevelImage.raycastTarget = true;
                        ExtraLevelAnim.SetInteger("UIState", 2);
                        break;
                }
            }

            switch (Completion)
            {
                case LevelsCompletionType.Lock:
                    LockedLevelSprite.enabled = true;
                    Star.SetInteger("UIState", 1);
                    CardColor.SetInteger("UIState", 0);
                    break;
                case LevelsCompletionType.Unlock:
                    LockedLevelSprite.enabled = false;
                    Star.SetInteger("UIState", 1);
                    CardColor.SetInteger("UIState", 0);
                    break;
                case LevelsCompletionType.Complete:
                    LockedLevelSprite.enabled = false;
                    Star.SetInteger("UIState", 2);
                    CardColor.SetInteger("UIState", 1);
                    break;
                case LevelsCompletionType.Mastered:
                    LockedLevelSprite.enabled = false;
                    Star.SetInteger("UIState", 3);
                    CardColor.SetInteger("UIState", 1);
                    break;
            }
        }
       

        //change color if the card is the selected one
		//TODO
        // Vector2 currentLevel = GameManagerScript.Instance.CurrentLevel;
       /* if (LevelID == currentLevel && CardColor != null)
        {
            CardColor.SetInteger("UIState", 2);
        }*/

    }

    public void TouchEnter()
	{
		if (!ScrollLevelManagerScript.Instance.IsMoving && Mathf.Abs(ScrollLevelManagerScript.Instance.DeltaY) < 3)
		{
			ExtraLevelAnim.SetInteger("UIState", Completion > LevelsCompletionType.Lock ? 3 : 1);
		}
	}

	public void TouchExit()
    {
		if(!ScrollLevelManagerScript.Instance.IsMoving && Mathf.Abs(ScrollLevelManagerScript.Instance.DeltaY) < 3)
		{
			ExtraLevelAnim.SetInteger("UIState", Completion > LevelsCompletionType.Lock ? 2 : 0);
			if(Completion == LevelsCompletionType.Lock && LevelStorageManager.Instance.StagesCompletion.Stages[LevelID.x].Levels[0].LevelCompletion > LevelsCompletionType.Lock)
			{


                //TODO
				//GameManagerScript.Instance.UserOptToWatchAd(LevelID);
			}
		}
    }

    
    public void GoToLevel()
    {
		if(Completion > LevelsCompletionType.Lock && !ScrollLevelManagerScript.Instance.IsMoving && Mathf.Abs(ScrollLevelManagerScript.Instance.DeltaY) < 3)
		{
			if(!ScrollLevelManagerScript.Instance.IsButtonAlreadyClicked)
			{
				ScrollLevelManagerScript.Instance.IsButtonAlreadyClicked = true;
				CardColor.SetInteger("UIState", 2);
                Debug.Log(LevelID.x);
                ScrollLevelManagerScript.Instance.SlideDown();
				//TODO
                //GameManagerScript.Instance.GoToLevel(LevelID);
                //fire audio
                //AudioManagerScript.Instance.SelectLevel();
			}
            
		}

    }
    internal void LevelSpriteUpdate(Sprite sprite)
    {
        LevelImage.sprite = sprite;
    }

    internal void UpdateScore(int v)
    {
        score = v;
        Star.SetInteger("UIState", v);
    }
}
