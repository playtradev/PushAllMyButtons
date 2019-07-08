using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarScript : MonoBehaviour
{
	public RectTransform Bar;
	public Vector2 PosOffset;

	private int BasePoints;
    // Start is called before the first frame update
    void Start()
    {
		BasePoints = GameManagerScript.Instance.Speed;
		PosOffset = Bar.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
		Bar.anchoredPosition = PosOffset + (Vector2.up * (Bar.rect.height / (GameManagerScript.Instance.PointsToFinishLevel - BasePoints)) * (GameManagerScript.Instance.Speed - BasePoints));
    }


}
