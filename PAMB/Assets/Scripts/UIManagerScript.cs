using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
	public static UIManagerScript Instance;
	public Animator WinPanel;
	public Animator StartPanel;
	public Animator StageCompletedAnim;
	public Image LevelComplete;
	private bool StartPanelBool = true;

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

    public void StartLevel()
	{
		GameManagerScript.Instance.GameState = GameStateType.Start;
		StartPanelBool = !StartPanelBool;
		StartPanel.SetBool("Visible", StartPanelBool);
		CameraProjectionChange.Instance.SetChangeProjection(OrtoPersType.Orto);
	}


    public void SetStageCompletetdAnim(bool v)
	{
		StageCompletedAnim.SetBool("InOut", v);
	}

}
