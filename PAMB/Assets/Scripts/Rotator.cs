using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

	public static Rotator Instance;

	public List<CircleScript> Levels = new List<CircleScript>();

	public int CurrentLevel = 0;
	public CircleScript CurrentCircle;
	[Range(3,1000)]
	public float ExplosionForce;

	// Start is called before the first frame update
	void Start()
    {
		Instance = this;
		CurrentCircle = Levels[CurrentLevel];
		CurrentCircle.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
		if(!GameManagerScript.Instance.Exploded)
		{
			transform.Rotate(Vector3.forward * GameManagerScript.Instance.Speed, Space.Self);
		}
    }

    public void Explosion()
	{
		Vector3 baseExplosion = transform.position + Vector3.down + new Vector3(1.5f,0,1.5f);//Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f)
		Debug.DrawRay(baseExplosion, Vector3.up * 10, Color.red, 10);

		RaycastHit[] hits = Physics.RaycastAll(new Ray(baseExplosion, Vector3.up * 10), 10);

		foreach (RaycastHit item in hits)
		{
			if(item.collider.tag != "Button")
			{
				Camera.main.transform.position = new Vector3(0, 30, -5.5f);
				Camera.main.orthographic = false;
				item.collider.gameObject.AddComponent<BoxCollider>();
				item.collider.gameObject.GetComponent<MeshCollider>().enabled = false;
				item.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				item.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
				item.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((item.collider.transform.position - baseExplosion) * ExplosionForce, item.point, ForceMode.Impulse);

			}
		}
	}


    public void GoToNextLevel()
	{
		CurrentLevel++;
		CurrentCircle.gameObject.SetActive(false);
		if(CurrentLevel < Levels.Count)
		{
			CurrentCircle = Levels[CurrentLevel];
            CurrentCircle.gameObject.SetActive(true);
		}
		else
		{
			GameManagerScript.Instance.GameState = GameStateType.End;
			UIManagerScript.Instance.SetWinPanelAnim(true);
		}
	}



}
