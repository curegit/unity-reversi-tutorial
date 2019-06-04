using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ReversiTutorial.Model;

namespace ReversiTutorial
{
	/// <summary>
	/// ゲームの進行管理
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// 石のオブジェクト
		/// </summary>
		[SerializeField]
		private GameObject disk;

		/// <summary>
		/// すべてのタイル
		/// </summary>
		[SerializeField]
		private Square[] squares;

		/// <summary>
		/// 黒にAIを使うか（ゲーム途中での変更不可）
		/// </summary>
		[SerializeField]
		private bool useAIDark;

		/// <summary>
		/// 白にAIを使うか（ゲーム途中での変更不可）
		/// </summary>
		[SerializeField]
		private bool useAILight;

		/// <summary>
		/// AIの最低思考時間
		/// </summary>
		[SerializeField, Range(0, 5000)]
		private int minAIDelayMs = 1000;

		/// <summary>
		/// ゲームマネージャのシングルトン
		/// </summary>
		public static GameManager instance { get; private set; }

		/// <summary>
		/// ゲームモードを指定するための静的変数
		/// </summary>
		public static int newGameMode { get; set; }

		/// <summary>
		/// 盤の状態を表すオブジェクト
		/// </summary>
		public Board board { get; private set; }

		/// <summary>
		/// 石の場所からインスタンスを引く辞書
		/// </summary>
		private Dictionary<Position, Disk> disks = new Dictionary<Position, Disk>();

		/// <summary>
		/// AIが思考中かどうか
		/// </summary>
		private bool thinking;

		/// <summary>
		/// 初期化時に呼ばれる
		/// </summary>
		public void Awake()
		{
			if (instance)
			{
				// シングルトンの初期化違反
				Debug.LogWarning("GameManager already initialized");
			}
			else
			{
				instance = this;
			}
		}

		/// <summary>
		/// 初期化時に呼ばれる
		/// </summary>
		private void Start()
		{
			// 空の盤をつくる
			board = new Board();
			// 初期配置のとおりに石のゲームオブジェクトを置く
			foreach (var dark in board.DiskPositions(Player.Dark))
			{
				PlaceDiskObject(dark.x, dark.z, Player.Dark);
			}
			foreach (var light in board.DiskPositions(Player.Light))
			{
				PlaceDiskObject(light.x, light.z, Player.Light);
			}
			// 人間/AIの切り替えはここでやる
			/* ここにコードを追加 */
		}

		/// <summary>
		/// 毎フレームの処理
		/// </summary>
		private void Update()
		{
			if (board.turn == Player.Dark && useAIDark || board.turn == Player.Light && useAILight)
			{
				if (board.CanMove() && !thinking)
				{
					thinking = true;
					LetAIThink();
				}
			}
		}

		/// <summary>
		/// 現在のプレイヤーが特定の場所に石を置く
		/// </summary>
		/// <param name="x">x方向の区画座標</param>
		/// <param name="z">z方向の区画座標</param>
		public void Place(int x, int z)
		{
			if (board.CanPlace(x, z))
			{
				var side = board.turn;
				var turnovers = board.GetTurnovers(x, z);
				board = board.Place(x, z);
				PlaceDiskObject(x, z, side);
				foreach (var turnover in turnovers)
				{
					disks[turnover].Flip(side);
				}
			}
		}

		/// <summary>
		/// 現在のプレイヤーがパスする
		/// </summary>
		public void Pass()
		{
			if (!board.CanMove())
			{
				board = board.Pass();
			}
		}

		/// <summary>
		/// 特定のプレイヤーの石を新しく配置する
		/// </summary>
		/// <param name="x">x方向の区画座標</param>
		/// <param name="z">z方向の区画座標</param>
		/// <param name="side">プレイヤー</param>
		private void PlaceDiskObject(int x, int z, Player side)
		{
			var square = squares.Single(s => x == s.x && z == s.z);
			var pos = square.Position();
			var newDisk = Instantiate(disk, pos, Quaternion.identity).GetComponent<Disk>();
			newDisk.square = square;
			newDisk.FlipImmediate(side);
			disks.Add(new Position(x, z), newDisk);
		}

		/// <summary>
		/// 現在のプレイヤーがマスを選択ときに呼ばれる
		/// </summary>
		/// <param name="x">x方向の区画座標</param>
		/// <param name="z">z方向の区画座標</param>
		public void PlayerClick(int x, int z)
		{
			if (board.turn == Player.Dark && !useAIDark || board.turn == Player.Light && !useAILight)
			{
				Place(x, z);
			}
		}

		/// <summary>
		/// 現在のプレイヤーがパスを選んだときに呼ばれる
		/// </summary>
		public void PlayerPass()
		{
			if (board.turn == Player.Dark && !useAIDark || board.turn == Player.Light && !useAILight)
			{
				Pass();
			}
		}

		/// <summary>
		/// AIに思考させておく
		/// </summary>
		public async void LetAIThink()
		{
			if (board.IsEnd())
			{
				return;
			}
			else if (!board.CanMove())
			{
				Pass();
				return;
			}
			else
			{
				var moment = Task.Delay(minAIDelayMs);
				var think = Task.Run(() => AI.Think(board));
				await Task.WhenAll(think, moment);
				var pos = think.Result;
				Place(pos.x, pos.z);
				thinking = false;
			}
		}
	}
}
