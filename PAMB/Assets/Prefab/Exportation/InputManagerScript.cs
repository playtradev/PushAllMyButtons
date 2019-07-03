using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{

	public static InputManagerScript Instance;

    public float ToleranceMinY = 200;
    public float ToleranceMinX = 400;
    public float SpeedTollerance = 0.5f; // speed in seconds
    public float SlideSpeed = 0.01f;//pixel per frame
    public bool startSliding = false;
    float startPointY = 0;
    float endPointY = 0;
    public float deltaY = 0;
    float startPointX = 0;
    float endPointX = 0;
    public float deltaX = 0;
    float timeSliding = 0;
    float deltaTime = 0;

	private void Awake()
	{
		Instance = this;
		ToleranceMinX = Screen.width / 8;
		ToleranceMinY = Screen.height / 14;
	}


	// Update is called once per frame
	void Update()
    {
        //exit the game if the button back of the phone is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
		if (Input.touches.Length > 0)
        {
            List<Touch> touches = Input.touches.ToList();
            foreach (Touch touch in touches)
            {
				if (touch.phase == TouchPhase.Began)
                {
					startPointY = Input.mousePosition.y;
					startPointX = Input.mousePosition.x;
                    timeSliding = Time.time;
                }
				else if (touch.phase == TouchPhase.Ended)
                {
					endPointY = Input.mousePosition.y;
                    deltaY = endPointY - startPointY;
					endPointX = Input.mousePosition.x;
                    deltaX = endPointX - startPointX;
                    deltaTime = Time.time - timeSliding;

					if (Mathf.Abs(deltaX) > ToleranceMinX && Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                    {
                        ChangeLevelChecker();
                    }
                    else if (Mathf.Abs(deltaY) > ToleranceMinY && Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
                    {
                        LevelMenuChecker();
                    }
					else if (Mathf.Abs(deltaX) < 10 && Mathf.Abs(deltaY) < 10)
                    {
                        //GameManagerScript.Instance.AddBead();
                    }
                }
            }
        }
		else if (!Input.touchSupported && Input.GetMouseButtonDown(0))
        {
            startPointY = Input.mousePosition[1];
            startPointX = Input.mousePosition[0];
            timeSliding = Time.time;
        }
        else if (!Input.touchSupported && Input.GetMouseButtonUp(0))
        {
			endPointY = Input.mousePosition[1];
            deltaY = endPointY - startPointY;
			endPointX = Input.mousePosition[0];
            deltaX = endPointX - startPointX;
            deltaTime = Time.time - timeSliding;

			if (Mathf.Abs(deltaX) > ToleranceMinX && Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                ChangeLevelChecker();
            }
			else if (Mathf.Abs(deltaY) > ToleranceMinY && Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                LevelMenuChecker();
            }
			else if(Mathf.Abs(deltaX) < 10 && Mathf.Abs(deltaY) < 10)
            {
                //GameManagerScript.Instance.AddBead();
            }
        }


    }

    private void LevelMenuChecker()
    {
		if (deltaY > ToleranceMinY && deltaTime < SpeedTollerance)
        {
			ScrollLevelManagerScript.Instance.SlideUp();
        }
    }
    private void ChangeLevelChecker()
    {
		if (deltaX > ToleranceMinX && deltaTime < SpeedTollerance && startPointY > 300 && deltaTime > 0.06f )
        {
            if (GameManagerScript.Instance.CurrentLevel.x == 0 && GameManagerScript.Instance.CurrentLevel.y == 0)
            {
            }
            else
            {
				Vector2Int NextLvl = Vector2Int.up;
				NextLvl = NextLvl.y == 0 ? new Vector2Int(NextLvl.x - 1, 8) : new Vector2Int(NextLvl.x, NextLvl.y - 1);

				if (NextLvl.y == 8 || LevelStorageManager.Instance.StagesCompletion.Stages[NextLvl.x].Levels[NextLvl.y].LevelCompletion > LevelsCompletionType.Lock)
                {
                   // AudioManagerScript.Instance.SelectLevel();
                    //GameManagerScript.Instance.PreviousLevel();
                }
            }
        }
        else if (deltaX < -ToleranceMinX && deltaTime < SpeedTollerance && startPointY > 300 && deltaTime > 0.06f)
        {
			Vector2Int NextLvl = Vector2Int.up;
			NextLvl = NextLvl.y == 8 ? new Vector2Int(NextLvl.x + 1, 0) : new Vector2Int(NextLvl.x, NextLvl.y + 1);
			if (NextLvl.x < LevelStorageManager.Instance.Levels.LevelStage.Count && 
			    (NextLvl.y == 8 || LevelStorageManager.Instance.StagesCompletion.Stages[NextLvl.x].Levels[NextLvl.y].LevelCompletion > LevelsCompletionType.Lock))
            {
                //GameManagerScript.Instance.NextLevel();
                //AudioManagerScript.Instance.SelectLevel();
            }
        }
    }
}



