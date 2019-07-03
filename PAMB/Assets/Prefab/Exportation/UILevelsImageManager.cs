using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class UILevelsImageManager : MonoBehaviour {

	public static UILevelsImageManager Instance;

	public List<UILevelImageClass> ListOfUILevels = new List<UILevelImageClass>();

	private void Awake()
	{
		Instance = this;
	}

	public Sprite GetSpriteFromID(int id)
	{
		return ListOfUILevels.Where(r => r.ID == id).First().LevelSprite;
	}

}


[System.Serializable]
public class UILevelImageClass
{
	public int ID;
	public Sprite LevelSprite;

    public UILevelImageClass()
	{

	}


	public UILevelImageClass(int id, Sprite levelSprite)
	{
		ID = id;
		LevelSprite = levelSprite;
	}
}