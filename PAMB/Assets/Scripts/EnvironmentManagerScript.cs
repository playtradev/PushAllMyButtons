using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManagerScript : MonoBehaviour
{
	public static EnvironmentManagerScript Instance;



	public Transform Vulcano;
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
        
    }

    public void MoveToNext(float x)
	{
		Vulcano.position += Vector3.right * x;
	}

	public void MoveToPrev(float x)
    {
		Vulcano.position -= Vector3.right * x;
    }

}
