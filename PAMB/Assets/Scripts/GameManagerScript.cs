using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript Instance;

	public GameStateType GameState;

	public TextMeshProUGUI Points;
	public int PointsToFinishLevel = 20;
	public int Speed = 0;
	public bool Exploded = false;
	private Vector3 CameraBasePos;

	private void Awake()
	{
		Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		CameraBasePos = Camera.main.transform.position;
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
            RaycastHit hit;
            bool found = Physics.Raycast(ray, out hit, 100);
			if(found)
			{
				if (hit.collider.tag == "Button")
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
			Exploded = true;
			Speed = 0;
			GameState = GameStateType.Intro;
			Rotator.Instance.Explosion();
			Invoke("GoToNextLevel", 3);
		}

    }


    public void GoToNextLevel()
	{
		Camera.main.transform.position = CameraBasePos;
		Exploded = false;
		Speed = 1;
		Camera.main.orthographic = true;
		Rotator.Instance.GoToNextLevel();
	}
}



public enum GameStateType
{
	Intro,
    Start,
    End
}