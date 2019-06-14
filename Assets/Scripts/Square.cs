using UnityEngine;
using UnityEngine.EventSystems;

namespace ReversiTutorial
{
	/// <summary>
	/// 盤の一区画
	/// </summary>
	[RequireComponent(typeof(MeshRenderer))]
	public class Square : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		/// <summary>
		/// x方向の区画座標
		/// </summary>
		[SerializeField]
		private int _x;

		/// <summary>
		/// z方向の区画座標
		/// </summary>
		[SerializeField]
		private int _z;

		/// <summary>
		/// 何も起こっていないときのマテリアル
		/// </summary>
		[SerializeField]
		private Material defaultMaterial;

		/// <summary>
		/// ポインターで指されているときのマテリアル
		/// </summary>
		[SerializeField]
		private Material pointedMaterial;

		/// <summary>
		/// 配置可能区画を表すためのハイライトされたマテリアル
		/// </summary>
		[SerializeField]
		private Material highlightedMaterial;

		/// <summary>
		/// ポインターで指されているかどうかのフラグ
		/// </summary>
		private bool pointed;

		/// <summary>
		/// キャッシュされたレンダラー
		/// </summary>
		private MeshRenderer meshRenderer;

		/// <summary>
		/// x方向の区画座標
		/// </summary>
		public int x => _x;

		/// <summary>
		/// z方向の区画座標
		/// </summary>
		public int z => _z;

		/// <summary>
		/// 初期化時に呼ばれる
		/// </summary>
		private void Start()
		{
			// デフォルトマテリアルが指定されていなければ現在のものを使う
			defaultMaterial = defaultMaterial ? defaultMaterial : GetComponent<MeshRenderer>()?.sharedMaterial;
			// レンダラーへの参照をキャッシュ
			meshRenderer = GetComponent<MeshRenderer>();
		}

		/// <summary>
		/// 毎フレームの処理
		/// </summary>
		private void Update()
		{
			if (pointed)
			{
				// ポインターで指されていたら、それ用のマテリアルを適用
				meshRenderer.sharedMaterial = pointedMaterial;
			}
			else
			{
				if (GameManager.instance.board.CanPlace(x, z))
				{
					// 配置可能な場所ならハイライトマテリアルを適用
					meshRenderer.sharedMaterial = highlightedMaterial;
				}
				else
				{
					// それ以外では普通のマテリアル
					meshRenderer.sharedMaterial = defaultMaterial;
				}
			}
		}

		/// <summary>
		/// この区画の中央の位置を返す
		/// </summary>
		/// <returns>区画の中央の位置ベクトル</returns>
		public Vector3 Position()
		{
			return transform.position;
		}

		/// <summary>
		/// ポインターでクリックされたとき呼ばれる
		/// </summary>
		/// <param name="eventData">イベントデータ</param>
		public void OnPointerClick(PointerEventData eventData)
		{
			// マネージャーにクリックされたことを通知する
			GameManager.instance.PlayerClick(x, z);
		}

		/// <summary>
		/// ポインターがこの区画範囲に入ったときに呼ばれる
		/// </summary>
		/// <param name="eventData">イベントデータ</param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			// ポインターで指されているフラグを立てる
			pointed = true;
		}

		/// <summary>
		/// ポインターがこの区画範囲から出たときに呼ばれる
		/// </summary>
		/// <param name="eventData">イベントデータ</param>
		public void OnPointerExit(PointerEventData eventData)
		{
			// ポインターで指されているフラグを下ろす
			pointed = false;
		}
	}
}
