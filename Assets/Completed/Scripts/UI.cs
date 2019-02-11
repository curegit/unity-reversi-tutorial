using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ReversiTutorial.Model;

namespace ReversiTutorial.Completed
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

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private Text darkDiskCountText;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private Text lightDiskCountText;

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
			//
			darkDiskCountText.text = $"{GameManager.instance.board.Count(Player.Dark)}";
			lightDiskCountText.text = $"{GameManager.instance.board.Count(Player.Light)}";

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
