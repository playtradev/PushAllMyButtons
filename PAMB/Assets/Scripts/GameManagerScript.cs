using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript Instance;
	public DifficultyType Difficulty;
	public GameStateType GameState;
	public Vector2Int CurrentLevel;
	public TextMeshProUGUI Points;
	public int PointsToFinishLevel = 20;
	public int Speed = 0;
	public bool Exploded = false;
	public Animator MainCamAnim;
	public float VulcanoSideMovement = 40;

	private void Awake()
	{
		Instance = this;
		CurrentLevel = Vector2Int.zero;

	}

	// Start is called before the first frame update
	void Start()
    {
		CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Orto);
		GameState = GameStateType.Start;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.A))
		{
			Speed++;
		}

		if(Input.GetMouseButtonDown(0) && GameState == GameStateType.Start)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane p = new Plane(Vector3.up, Vector3.zero);
            float dist = 0;
            p.Raycast(ray, out dist);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 30);
            RaycastHit[] hit;
			hit = Physics.RaycastAll(ray,100);
			if(hit.Length > 0)
			{
				int button = hit.Where(r => r.collider.tag == "Button").ToList().Count();
				int disk = hit.Where(r => r.collider.tag == "Disk").ToList().Count();
				if (button > 0 && disk == 0)
                {
                    Speed++;
                }
				else
				{
					Speed--;
				}
				if(Speed<1)
				{
					Speed = 1;
				}
				Points.text = "Points:" + Speed;
			}
		}

		if(Speed == PointsToFinishLevel && !Exploded)
		{
			CurrentLevel.y += 1;
			Exploded = true;
			Speed = 0;
			GameState = GameStateType.Move;
			CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
			Rotator.Instance.Invoke("Explosion", 2);
			Invoke("LevelComplete", 5);
		}

    }


	public void LevelComplete()
    {
        CameraProjectionChange.Instance.MoveToNext(VulcanoSideMovement);
        Exploded = false;
        Speed = 1;
    }

    public void GoToNextLevel()
	{
		if(GameState != GameStateType.Move )
		{
			CurrentLevel.y += 1;
			GameState = GameStateType.Move;
            CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
            CameraProjectionChange.Instance.MoveToNext(VulcanoSideMovement);
            Exploded = false;
            Speed = 1;
		}

	}

	public void GoToPrevLevel()
    {
		if (GameState != GameStateType.Move && CurrentLevel.y - 1 >= 0)
		{
			CurrentLevel.y -= 1;
			GameState = GameStateType.Move;
			CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
			CameraProjectionChange.Instance.MoveToNext(-VulcanoSideMovement);
			Exploded = false;
			Speed = 1;
		}
    }

    public void SetCameraAnim(bool v)
	{
		MainCamAnim.SetBool("UpDown", v);
	}

}



public enum GameStateType
{
	Intro,
    Start,
    End,
    Move
}

public enum OrtoPersType
{
	Orto,
    Persp
}


public enum DifficultyType
{
	easy,
    med,
    hard
}
