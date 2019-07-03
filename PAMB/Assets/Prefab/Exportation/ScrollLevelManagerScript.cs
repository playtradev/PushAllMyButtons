using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class ScrollLevelManagerScript : MonoBehaviour {

	public delegate void StageStateUpdate(int state);
    public event StageStateUpdate UpdateState;


    public bool IsButtonAlreadyClicked
	{
		get
		{
			return _IsButtonAlreadyClicked;
		}
		set
		{
			_IsButtonAlreadyClicked = value;
			if(_IsButtonAlreadyClicked)
			{
				Invoke("ChangeIsButtonAlreadyClickedValue", 0.1f);	
			}
		}
	}



	public static ScrollLevelManagerScript Instance;


	public bool IsMoving = false;
	public float DeltaY;
    
	[SerializeField]
	private RectTransform Mask;
	[SerializeField]
	private RectTransform ParentCanvas;
	[SerializeField]
    private List<RectTransform> Stages = new List<RectTransform>();
	private Camera MainCamera;
	private List<StageScript> StagesScript = new List<StageScript>();
	private bool IsLevelSelectionOpen = false;
    private float mouseLastPositionY = 0;
    private float TargetPosition;
    private float inertialSpace;
	private ScrollState CurrentScrollState;
	private CurrentMovementStatus isMovingUpOrDown = CurrentMovementStatus.None; // True Up False Down
	private Animator LevelMenuManager;
	private bool _IsButtonAlreadyClicked = false;
	private int CurrentStage = 0;
    

    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
		LevelMenuManager = GetComponent<Animator>();
		MainCamera = Camera.main;
        foreach (RectTransform item in Stages)
        {
            StagesScript.Add(item.GetComponent<StageScript>());
        }
        //bring the stages in the right position
        //ResetStages(0);
    }
    
	private void ChangeIsButtonAlreadyClickedValue()
	{
		IsButtonAlreadyClicked = false;
	}

    public void ResetStages(int stage)
    {
		Debug.Log(stage);
		CurrentStage = stage;
        for (int i = 0; i < 3; i++)
		{
			StagesScript[i].UpdateAnim(new Vector2Int(0, i - 1));
			StagesScript[i].Pos = stage + (i - 1);
			StagesScript[i].LoadLevels();
		}
	}

	public void Update()
    {
		
		if (IsLevelSelectionOpen && CurrentScrollState == ScrollState.Open)
        {
			if (Input.touches.Length > 0)
            {
                List<Touch> touches = Input.touches.ToList();
              
				if (touches[0].phase == TouchPhase.Began)
                {
					mouseLastPositionY = Input.mousePosition.y;
                }
				else if (touches[0].phase == TouchPhase.Ended)
                {
					if (!IsMoving)
                    {
                        
                        Vector3 mouse = Input.mousePosition;
                        DeltaY = mouse.y - mouseLastPositionY;
                        isMovingUpOrDown = DeltaY < 0 ? CurrentMovementStatus.Up : DeltaY > 0 ? CurrentMovementStatus.Down : CurrentMovementStatus.None;
						//if (Mathf.Abs(DeltaY) > 10 && ((CurrentStage > 0 && isMovingUpOrDown == CurrentMovementStatus.Up) ||
                      //     (LevelStorageManager.Instance.Levels.LevelStage.Count - 1 > CurrentStage && isMovingUpOrDown == CurrentMovementStatus.Down)))
                      //  {
							IsMoving = true;
                            CurrentStage += isMovingUpOrDown == CurrentMovementStatus.Up ? -1 : 1;
                            UpdateState(isMovingUpOrDown == CurrentMovementStatus.Up ? -1 : 1);
                       // }
                    }
					DeltaY = 0;
                }
            }
            else if (!Input.touchSupported && Input.GetMouseButtonDown(0))
            {
				mouseLastPositionY = Input.mousePosition.y;
            }
            else if (!Input.touchSupported && Input.GetMouseButtonUp(0))
            {
				if(!IsMoving)
				{
					Vector3 mouse = Input.mousePosition;
                    DeltaY = mouse.y - mouseLastPositionY;
                    isMovingUpOrDown = DeltaY < 0 ? CurrentMovementStatus.Up : DeltaY > 0 ? CurrentMovementStatus.Down : CurrentMovementStatus.None;
					//if (Mathf.Abs(DeltaY) > 10 && ((CurrentStage > 0 && isMovingUpOrDown == CurrentMovementStatus.Up) ||
                      //         (LevelStorageManager.Instance.Levels.LevelStage.Count - 1 > CurrentStage && isMovingUpOrDown == CurrentMovementStatus.Down)))
                    //{
						IsMoving = true;
                        CurrentStage += isMovingUpOrDown == CurrentMovementStatus.Up ? -1 : 1;
                        UpdateState(isMovingUpOrDown == CurrentMovementStatus.Up ? -1 : 1);
                    //}
				}
				DeltaY = 0;
            }
        }
    }


	public void CustomizationOpen()
	{
		//TODO
        //GameManagerScript.Instance.GameState = GameState.MenuCostumization;
	}

	public void CustomizationClose()
    {
		for (int i = 0; i < 3; i++)
        {
            StagesScript[i].LoadLevels();
        }
		//TODO
        //GameManagerScript.Instance.GameState = GameState.MenuLevelSelection;
    }

    public void MoveToStage(int stage)
    {
        ResetStages(stage);
    }

    public void SlideUp()
    {
		//Activate the panel only when necessary to save performances     #fastfix
        Mask.gameObject.SetActive(true);

        //fire audio
        Invoke("SlideUpWithDelay", 0.05f);
    }

	public void SlideUpWithDelay()
    {
		if (CurrentScrollState == ScrollState.Close)
        {
			SetScrollAsOpenOrClosed(true);
			CurrentScrollState = ScrollState.Open;
            //Audio//ManagerScript.Instance.OpenMenu();
            LevelMenuManager.SetBool("PopInOut", true);
			MoveToStage(CurrentStage);
        }
    }
    public void SlideDown()
    {
        Invoke("SlideDownWithDelay", 0.05f);
    }

    public void SlideDownWithDelay()
    {
//		
		if (CurrentScrollState == ScrollState.Open)
        {
			SetScrollAsOpenOrClosed(false);
			IsMoving = false;
			isMovingUpOrDown = CurrentMovementStatus.None;
			CurrentScrollState = ScrollState.Close;
            //AudioManagerScript.Instance.CloseMenu();
            LevelMenuManager.SetBool("PopInOut", false);
        }
    }

	public void SetScrollAsOpenOrClosed(bool value)
    {
        IsLevelSelectionOpen = value;
        //deactivate the panel to save performances     #fastfix
        
		Mask.gameObject.SetActive(IsLevelSelectionOpen);
    }

}



public enum CurrentMovementStatus
{
	None,
    Down,
    Up
}

public enum ScrollState
{
	Close,
    Open
}