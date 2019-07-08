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
	public List<string> levels = new List<string>();

	public List<ParticleSystem> Touches = new List<ParticleSystem>();

	string saving = "";
	public int BaseSpeed;
	private void Awake()
	{
		Instance = this;
		CurrentLevel = Vector2Int.zero;
		BaseSpeed = Speed;
	}

	// Start is called before the first frame update
	void Start()
    {
		PlayerPrefs.SetString("LevelsProgression", "");
		saving = PlayerPrefs.GetString("LevelsProgression");
		if (string.IsNullOrEmpty(saving))
        {
            for (int i = 0; i < Rotator.Instance.Levels.Count; i++)
            {
				levels.Add("0");
                saving += "0,";
            }
			PlayerPrefs.SetString("LevelsProgression", saving);
            CurrentLevel.y = 0;
        }
        else
        {
            levels = saving.Split(',').ToList();
			levels.RemoveAt(levels.Count - 1);
            if (levels.Count != Rotator.Instance.Levels.Count)
            {
                for (int i = 0; i < Rotator.Instance.Levels.Count - levels.Count; i++)
                {
                    levels.Add("0");
                    saving += "0,";
                }
            }

            CurrentLevel.y = levels.LastIndexOf("1") + 1;
			if(CurrentLevel.y == -1)
			{
				CurrentLevel.y = 0;
			}
			if(CurrentLevel.y == Rotator.Instance.Levels.Count)
			{
				CurrentLevel.y--;
			}
        }
		CameraProjectionChange.Instance.transform.parent.position += new Vector3(CurrentLevel.y * 40, 0, 0);
		ButtonsRotator.Instance.StartLevel();
		Rotator.Instance.StartLevel();
		Invoke("StartMatch", 1);
    }


    private void StartMatch()
	{
		CameraProjectionChange.Instance.SetCameraAnim(false);
	}


    public void SetGameStateToStart()
	{
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
					StartCoroutine(TouchParticle(hit.Where(r => r.collider.tag == "Button").ToList().First().point));
                }
				else if(disk != 0)
				{
					Speed--;
					Rotator.Instance.CurrentCircle.Wobbling();
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
			GameState = GameStateType.Move;
			CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
			CameraProjectionChange.Instance.SetCameraShakeAnim();
			Rotator.Instance.Invoke("Explosion", 1.25f);
			if(CurrentLevel.y < Rotator.Instance.Levels.Count)
			{
				Invoke("LevelComplete", 4);
			}
			else
			{
				GameState = GameStateType.End;
                UIManagerScript.Instance.SetWinPanelAnim(true);
			}
		}

    }

	private IEnumerator TouchParticle(Vector3 v3)
	{
		ParticleSystem ps = Touches.Where(r => !r.gameObject.activeInHierarchy).FirstOrDefault();
		if(ps != null)
		{
			ps.gameObject.SetActive(true);
			ps.transform.position = v3;
			yield return new WaitForSecondsRealtime(1.1f);
			ps.gameObject.SetActive(false);
		}
	}


	public void LevelComplete()
    {
		CameraProjectionChange.Instance.Anim.enabled = false;
        CameraProjectionChange.Instance.MoveToNext(VulcanoSideMovement);
        Exploded = false;
		Speed = BaseSpeed;
    }

    public void GoToNextLevel()
	{
		if(GameState != GameStateType.Move && CurrentLevel.y < Rotator.Instance.Levels.Count)
		{
			CurrentLevel.y += 1;
			if (CurrentLevel.y < Rotator.Instance.Levels.Count)
			{
				GameState = GameStateType.Move;
				CameraProjectionChange.Instance.SetCameraAnim(true);
				CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
				Invoke("MoveN", 0.01f);
				Exploded = false;
				Speed = BaseSpeed;
			}
			else
			{
				CurrentLevel.y--;
			}
		}

	}

    private void MoveN()
	{
		CameraProjectionChange.Instance.MoveToNext(VulcanoSideMovement);
	}

	public void GoToPrevLevel()
    {
		if (GameState != GameStateType.Move && CurrentLevel.y - 1 >= 0)
		{
			CurrentLevel.y -= 1;
			GameState = GameStateType.Move;
            CameraProjectionChange.Instance.SetCameraAnim(true);
            CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Persp);
            Invoke("MoveP", 0.01f);
            Exploded = false;
			Speed = BaseSpeed;
		}
    }

	private void MoveP()
    {
        CameraProjectionChange.Instance.MoveToNext(-VulcanoSideMovement);
    }

    public void SetCameraAnim(bool v)
	{
		MainCamAnim.SetBool("UpDown", v);
	}

    public void LevelCompleted(int index)
	{
		saving = "";
		levels[index] = "1";
		for (int i = 0; i < levels.Count; i++)
		{
			saving += levels[i] + ",";
		}
		PlayerPrefs.SetString("LevelsProgression", saving);
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
