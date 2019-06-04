using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReversiTutorial.Completed
{
	/// <summary>
	/// タイトルシーンの制御
	/// </summary>
	public class Title : MonoBehaviour
	{
		/// <summary>
		/// ゲームモードが選ばれたとき
		/// </summary>
		/// <param name="mode">ゲームモード</param>
		public void OnPlayButtonClicked(int mode)
		{
			// ゲームモードを静的変数に入れておく
			GameManager.newGameMode = mode;
			// ゲームシーンをロード
			SceneManager.LoadScene(1);
		}

		/// <summary>
		/// ゲームプログラムの終了ボタンが押されたとき
		/// </summary>
		public void OnQuitButtonClicked()
		{
			Application.Quit();
		}
	}
}
