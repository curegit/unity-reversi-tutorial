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
			// 
			var delta = Time.deltaTime / animTime;
			// 
			animDarkToLight = Mathf.MoveTowards(animDarkToLight, destination, delta);
			// 
			transform.rotation = Quaternion.Slerp(Quaternion.Euler(darkRotation), Quaternion.Euler(lightRotation), animDarkToLight);
			// 
			transform.position = (square ? square.Position() : Vector3.zero) + Vector3.up * thickness / 2 + flipHeight * Vector3.up * Mathf.Sin(Mathf.PI * animDarkToLight);
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
