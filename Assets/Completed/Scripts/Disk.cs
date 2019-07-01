using UnityEngine;
using ReversiTutorial.Model;

namespace ReversiTutorial.Completed
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
		private float thickness = default;

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
		private Vector3 darkRotation = new Vector3(0f, 0f, 0f);

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
			var delta = Time.deltaTime / animTime;
			// 現在のアニメーション割合（0～1）を求める
			animDarkToLight = Mathf.MoveTowards(animDarkToLight, destination, delta);
			// アニメーション割合（0～1）に応じて球面線形補間によって回転させる
			transform.rotation = Quaternion.Slerp(Quaternion.Euler(darkRotation), Quaternion.Euler(lightRotation), animDarkToLight);
			// 盤面上（空中でない）の位置を求める
			var pos = Vector3.up * thickness / 2 + flipHeight * Vector3.up * Mathf.Sin(Mathf.PI * animDarkToLight);
			// アニメーション位置も含めた位置を求めて移動させる
			transform.position = square.Position() + pos;
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
