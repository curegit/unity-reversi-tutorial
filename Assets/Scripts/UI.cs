using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ReversiTutorial.Model;

namespace ReversiTutorial
{
	/// <summary>
	/// 
	/// </summary>
	public class UI : MonoBehaviour
	{
		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private Button passButton;

		/* TODO: ここに石の数を表示するUIテキストのフィールドを追加 */

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private Text resultText;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private GameObject gameEndDialog;

		/// <summary>
		/// 
		/// </summary>
		private void Start()
		{
			resultText.gameObject.SetActive(false);
			gameEndDialog.SetActive(false);
		}

		/// <summary>
		/// 
		/// </summary>
		private void Update()
		{
			// 
			if (!GameManager.instance.board.CanMove() && !GameManager.instance.board.IsEnd())
			{
				passButton.interactable = true;
			}
			else
			{
				passButton.interactable = false;
			}
			// 石の数を更新
			/* TODO: 石の数のテキストを更新する処理を追加 */

			//
			if (GameManager.instance.board.IsEnd())
			{
				//
				var b = GameManager.instance.board.Balance(Player.Dark);
				if (b > 0)
				{
					resultText.text = "The Dark Wins";
				}
				else if (b < 0)
				{
					resultText.text = "The Light Wins";
				}
				else
				{
					resultText.text = "Even";
				}
				//
				resultText.gameObject.SetActive(true);
				gameEndDialog.SetActive(true);
			}
			else
			{
				resultText.gameObject.SetActive(false);
				gameEndDialog.SetActive(false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnPassButtonClicked()
		{
			GameManager.instance.Pass();
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnRetryButtonClicked()
		{
			SceneManager.LoadScene(1);
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnExitButtonClicked()
		{
			SceneManager.LoadScene(0);
		}
	}
}
