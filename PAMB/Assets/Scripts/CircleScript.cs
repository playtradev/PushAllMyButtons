using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
	public int Position;
	public float Timer = 0;
	public Vector3 PosOffset;
	public Rigidbody RB;
	public DifficultyType Difficulty;
	public RuntimeAnimatorController OwnAnimator;

	public Animator Anim;

	public List<Transform> Buttons = new List<Transform>();

	private void Awake()
	{
		RB = GetComponent<Rigidbody>();
		Position = transform.GetSiblingIndex();
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		PosOffset = transform.position;
		Timer = 0;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		/*if(Timer <= 1)
		{
			Timer += Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(PosOffset, Rotator.Instance.transform.position, Timer);
            
		}*/
    }

    public void Wobbling()
	{
		Anim.SetBool("Wobble", true);
	}

    public void StopWobbling()
	{
		Anim.SetBool("Wobble", false);
	}

}
