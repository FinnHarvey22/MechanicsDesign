using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager_Example 
{

	public static void ChangeLevel(int p_SceneID)
	{
		SceneManager.LoadScene(p_SceneID, LoadSceneMode.Additive);
	}
}
