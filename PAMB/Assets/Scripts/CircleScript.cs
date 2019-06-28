using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour
{

	public float Timer = 0;
	public Vector3 PosOffset;

	private void OnEnable()
	{
		PosOffset = transform.position;
		Timer = 0;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if(Timer <= 1)
		{
			Timer += Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(PosOffset, Rotator.Instance.transform.position, Timer);
            
		}
		else if(GameManagerScript.Instance.GameState == GameStateType.Intro)
		{
			GameManagerScript.Instance.GameState = GameStateType.Start;
		}
    }
}
