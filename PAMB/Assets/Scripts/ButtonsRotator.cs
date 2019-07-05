using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsRotator : MonoBehaviour
{

	public static ButtonsRotator Instance;
	[Range(0,10)]
	public int Speed;

	bool IsAnimActive = false;
	public Animator Anim;

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
		if(IsAnimActive && GameManagerScript.Instance.Difficulty != DifficultyType.hard)
		{
			IsAnimActive = false;
			Anim.enabled = IsAnimActive;
			transform.eulerAngles = new Vector3(0,-90,0);
		}

		
		if (!GameManagerScript.Instance.Exploded && GameManagerScript.Instance.Difficulty > DifficultyType.easy)
        {
			transform.Rotate(Vector3.up * (1), Space.Self);
        }

		if(!IsAnimActive && GameManagerScript.Instance.Difficulty == DifficultyType.hard)
		{
			IsAnimActive = true;
			Anim.enabled = IsAnimActive;
			Anim.runtimeAnimatorController = Rotator.Instance.CurrentCircle.OwnAnimator;
		}
    }

    public void ResetAnimator()
	{
		IsAnimActive = false;
		Anim.runtimeAnimatorController = null;
        Anim.enabled = IsAnimActive;
	}
}
