﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rotator : MonoBehaviour
{

	public delegate void Eruption(int v);
	public Eruption EruptionEvent;


	public static Rotator Instance;

	public List<CircleScript> Levels = new List<CircleScript>();
	public int CurrentLevel = 0;
	public CircleScript CurrentCircle;
	[Range(3,1000)]
	public float ExplosionForce;
    


	private void Awake()
	{
		Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		

    }

    // Update is called once per frame
    void Update()
    {
		if(!GameManagerScript.Instance.Exploded)
		{
			transform.Rotate(Vector3.forward * GameManagerScript.Instance.Speed, Space.Self);
		}
    }

    public void StartLevel()
	{
		Levels = Levels.OrderBy(r => r.Position).ToList();
		CurrentLevel = GameManagerScript.Instance.CurrentLevel.y;
		transform.position += new Vector3(CurrentLevel * 40, 0, 0);
		CurrentCircle = Levels[CurrentLevel];
        CurrentCircle.gameObject.SetActive(true);
        foreach (Transform item in CurrentCircle.Buttons)
        {
            item.gameObject.SetActive(true);
        }
		UIManagerScript.Instance.LevelComplete.enabled = GameManagerScript.Instance.levels[CurrentLevel] == "0" ? false : true;
        GameManagerScript.Instance.Difficulty = CurrentCircle.Difficulty;
	}


    public void Explosion()
	{
		Vector3 baseExplosion = Vector3.down + new Vector3(1f, 0, 1f);//  + new Vector3(1f,0,1f)      Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f)
		Debug.DrawRay(baseExplosion, Vector3.up * 10, Color.red, 10);
		GameManagerScript.Instance.LevelCompleted(CurrentLevel);
		RaycastHit[] hits = Physics.RaycastAll(new Ray(baseExplosion, Vector3.up * 10), 10);
		EruptionEvent(CurrentLevel);
		CameraProjectionChange.Instance.SetCameraShakeAnim();
		foreach (RaycastHit item in hits)
		{
			if(item.collider.tag != "Button")
			{
				item.collider.gameObject.AddComponent<BoxCollider>();
				item.collider.gameObject.GetComponent<MeshCollider>().enabled = false;
				item.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				item.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
				//item.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((item.collider.transform.position - baseExplosion) * ExplosionForce, item.point, ForceMode.Impulse);

			}
		}

	}

   


    public void GoToNextLevel()
	{
		CurrentLevel = GameManagerScript.Instance.CurrentLevel.y;
		CurrentCircle.gameObject.SetActive(false);
		if(CurrentLevel < Levels.Count)
		{

			ButtonsRotator.Instance.ResetAnimator();
			UIManagerScript.Instance.LevelComplete.enabled = GameManagerScript.Instance.levels[CurrentLevel] == "0" ? false : true;
			CurrentCircle.RB.velocity = Vector3.zero;
			CurrentCircle.RB.angularVelocity = Vector3.zero;
			CurrentCircle.RB.useGravity = false;
			CurrentCircle.transform.localPosition = Vector3.zero;
			CurrentCircle.transform.localEulerAngles = Vector3.zero;
			foreach (Transform item in CurrentCircle.Buttons)
            {
				item.gameObject.SetActive(false);
            }
			CurrentCircle = Levels[CurrentLevel];

			foreach (Transform item in transform.Cast<Transform>().Where(r=> r != transform).ToList())
			{
				item.gameObject.SetActive(false);
			}

			foreach (Transform item in CurrentCircle.Buttons)
            {
				item.gameObject.SetActive(true);
            }
       
			GameManagerScript.Instance.Difficulty = CurrentCircle.Difficulty;
            CurrentCircle.gameObject.SetActive(true);
		}
		else
		{
			
		}
	}



}
