using UnityEngine;
using ReversiTutorial.Model;

namespace ReversiTutorial
{
	/// <summary>
	/// 黒白の石
	/// </summary>
	public class Disk : MonoBehaviour
	{
		/// <summary>
		/// 厚さ
		/// </summary>
		[SerializeField]
		private float thickness;

		/// <summary>
		/// ひっくり返しにかける時間
		/// </summary>
		[SerializeField]
		private float animTime = 0.5f;

		/// <summary>
		/// ひっくり返すときの高さ
		/// </summary>
		[SerializeField]
		private float flipHeight = 1f;

		/// <summary>
		/// 黒の状態であるときの回転
		/// </summary>
		[SerializeField]
		private Vector3 darkRotation;

		/// <summary>
		/// 白の状態であるときの回転
		/// </summary>
		[SerializeField]
		private Vector3 lightRotation = new Vector3(180f, 0f, 0f);

		/// <summary>
		/// この石が置かれている区画
		/// </summary>
		public Square square { get; set; }

		/// <summary>
		/// アニメーションの目標（黒が0、白が1）
		/// </summary>
		private float destination;

		/// <summary>
		/// アニメーションの状態（黒が0、白が1）
		/// </summary>
		private float animDarkToLight;

		/// <summary>
		/// 毎フレームの処理
		/// </summary>
		private void Update()
		{
			// アニメーション割合（0～1）の変化量を計算
			/* TODO: var delta = ここに式を追加 */

			// 現在のアニメーション割合（0～1）を求める
			/* TODO: 適切な引数を与える animDarkToLight = Mathf.MoveTowards(___);*/

			// アニメーション割合（0～1）に応じて球面線形補間によって回転させる
			/* TODO: 黒と白の間の回転を補間するコードを追加 */

			// 盤面上（空中でない）の位置を求める
			/* TODO: 基準位置を求めるコードを追加 */

			// アニメーション位置も含めた位置を求めて移動させる
			/* TODO: アニメーションの位置を補間するコードを追加 */
		}

		/// <summary>
		/// ひっくり返す
		/// </summary>
		/// <param name="side">どっち側へ返すか</param>
		public void Flip(Player side)
		{
			destination = side == Player.Dark ? 0f : 1f;
		}

		/// <summary>
		/// アニメーションなしにすぐにひっくり返す
		/// </summary>
		/// <param name="side">どっち側へ返すか</param>
		public void FlipImmediate(Player side)
		{
			destination = side == Player.Dark ? 0f : 1f;
			animDarkToLight = destination;
		}
	}
}
