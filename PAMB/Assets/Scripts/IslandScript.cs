using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
	public ParticleSystem Eruption;
    public ParticleSystem Eruption2;

	public int Level;

    // Start is called before the first frame update
    void Start()
    {
		Rotator.Instance.EruptionEvent+= Instance_EruptionEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    void Instance_EruptionEvent(int v)
	{
		if(Level == v)
		{
			Eruption.gameObject.SetActive(true);
            Eruption2.gameObject.SetActive(true);
            Invoke("EruptionNotActive", 5);
		}
	}

	private void EruptionNotActive()
    {
        Eruption.gameObject.SetActive(false);
        Eruption2.gameObject.SetActive(false);
    }

}
