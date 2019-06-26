using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ReversiTutorial.Model;

namespace ReversiTutorial
{
	/// <summary>
	/// UIの制御
	/// </summary>
	public class UI : MonoBehaviour
	{
		/// <summary>
		/// パスボタン
		/// </summary>
		[SerializeField]
		private Button passButton;

		/* TODO: ここに石の数を表示するUIテキストのフィールドを追加 */

		/// <summary>
		/// 結果を表示するテキスト
		/// </summary>
		[SerializeField]
		private Text resultText;

		/// <summary>
		/// ゲーム終了時にだすダイアログ
		/// </summary>
		[SerializeField]
		private GameObject gameEndDialog;

		/// <summary>
		/// 初期化時に呼ばれる
		/// </summary>
		private void Start()
		{
			resultText.gameObject.SetActive(false);
			gameEndDialog.SetActive(false);
		}

		/// <summary>
		/// 毎フレームの処理
		/// </summary>
		private void Update()
		{
			// パスボタンの有効/無効を切り替える
			if (!GameManager.instance.isAITurn && !GameManager.instance.board.CanMove() && !GameManager.instance.board.IsEnd())
			{
				passButton.interactable = true;
			}
			else
			{
				passButton.interactable = false;
			}

			// 石の数を更新
			/* TODO: 石の数のテキストを更新する処理を追加 */

			// ゲームの終了時の処理
			if (GameManager.instance.board.IsEnd())
			{
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
		/// パスボタンが押されたとき
		/// </summary>
		public void OnPassButtonClicked()
		{
			GameManager.instance.Pass();
		}

		/// <summary>
		/// 再戦ボタンが押されたとき
		/// </summary>
		public void OnRetryButtonClicked()
		{
			SceneManager.LoadScene(1);
		}

		/// <summary>
		/// 終了ボタンが押されたとき
		/// </summary>
		public void OnExitButtonClicked()
		{
			SceneManager.LoadScene(0);
		}
	}
}
