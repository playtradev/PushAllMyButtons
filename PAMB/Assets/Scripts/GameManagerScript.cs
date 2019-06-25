using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript Instance;
	public TextMeshProUGUI Points;
	public float Speed = 0;

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
		if(Input.GetKey(KeyCode.A))
		{
			Speed++;
		}



		if(Input.GetMouseButtonDown(0))
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



    }
}
