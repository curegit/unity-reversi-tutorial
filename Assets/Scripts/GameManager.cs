using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ReversiTutorial.Model;

namespace ReversiTutorial
{
	/// <summary>
	/// 
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private GameObject disk;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private Square[] squares;

		/// <summary>
		/// （ゲーム途中での変更不可）
		/// </summary>
		[SerializeField]
		private bool useAIDark;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField]
		private bool useAILight;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField, Range(0, 5000)]
		private int minAIDelayMs = 1000;

		/// <summary>
		/// 
		/// </summary>
		public static GameManager instance { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public static int newGameMode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Board board { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<Position, Disk> disks = new Dictionary<Position, Disk>();

		/// <summary>
		/// 
		/// </summary>
		private bool thinking;

		/// <summary>
		/// 
		/// </summary>
		public void Awake()
		{
			// 
			if (instance)
			{
				Debug.LogWarning("GameManager already initialized");
			}
			else
			{
				instance = this;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void Start()
		{
			// 
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
		/// 
		/// </summary>
		private void Update()
		{
			//
			if (board.turn == Player.Dark && useAIDark || board.turn == Player.Light && useAILight)
			{
				//
				if (board.CanMove() && !thinking)
				{
					thinking = true;
					LetAIThink();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		public void Place(int x, int z)
		{

			if (board.CanPlace(x, z))
			{

				var side = board.turn;
				var turnovers = board.GetTurnovers(x, z);

				// 
				board = board.Place(x, z);
				// 
				PlaceDiskObject(x, z, side);
				// 
				foreach (var turnover in turnovers)
				{
					disks[turnover].Flip(side);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Pass()
		{
			if (!board.CanMove())
			{
				board = board.Pass();
			}
		}


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
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		public void PlayerClick(int x, int z)
		{
			if (board.turn == Player.Dark && !useAIDark || board.turn == Player.Light && !useAILight)
			{
				Place(x, z);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void PlayerPass()
		{
			if (board.turn == Player.Dark && !useAIDark || board.turn == Player.Light && !useAILight)
			{
				Pass();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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
