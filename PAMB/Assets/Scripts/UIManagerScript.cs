using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour
{
	public static UIManagerScript Instance;
	public Animator WinPanel;

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

	public void SetWinPanelAnim(bool v)
	{
		WinPanel.SetBool("InOut", v);
	}
}
