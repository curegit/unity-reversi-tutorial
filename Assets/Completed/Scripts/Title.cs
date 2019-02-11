using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReversiTutorial.Completed
{
	public class Title : MonoBehaviour
	{

		public void OnPlayButtonClicked(int mode)
		{
			//
			GameManager.newGameMode = mode;
			//
			SceneManager.LoadScene(1);
		}

		public void OnQuitButtonClicked()
		{
			Application.Quit();
		}
	}
}