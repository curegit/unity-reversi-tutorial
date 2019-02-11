using System;
using System.Linq;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// 盤と手を静的に評価する評価関数たち
	/// </summary>
	public static class Evaluation
	{
		/// <summary>
		/// 4x4盤評価関数用の重み（縦横対称）
		/// </summary>
		private static readonly int[,] weight4 = new int[2, 2] { { 1000, -1000 }, { -1000, 100 } };

		/// <summary>
		/// 6x6盤評価関数用の重み（縦横対称）
		/// </summary>
		private static readonly int[,] weight6 = new int[3, 3] { { 5000, -2000, 800 }, { -2000, -4000, -500 }, { 800, -500, 100 } };

		/// <summary>
		/// 8x8盤評価関数用の重み（縦横対称）
		/// </summary>
		private static readonly int[,] weight8 = new int[4, 4] { { 10000, -3000, 1000, 800 }, { -3000, -5000, -450, -500 }, { 1000, -450, 30, 10 }, { 800, -500, 10, 50 } };

		/// <summary>
		/// 特定のプレイヤーに対する盤のヒューリスティックな評価を返す
		/// </summary>
		/// <param name="board">評価する盤</param>
		/// <param name="player">どちらのプレイヤーの評価か</param>
		/// <returns>特定のプレイヤーに対する盤の評価</returns>
		public static int Evaluate(Board board, Player player)
		{
			/*
			var squares = Enumerable.Range(0, board.size).SelectMany(x => Enumerable.Range(0, board.size).Select(z => new Position(x, z)));
			var my = squares.Where(v => board.GetDisk(v.x, v.z) == player).Sum(v => Weight(v.x, v.z, board.size));
			var opp = squares.Where(v => board.GetDisk(v.x, v.z) != player && board.GetDisk(v.x, v.z) != null).Sum(v => Weight(v.x, v.z, board.size));
			*/
			var players = board.DiskPositions(player);
			var opponents = board.DiskPositions(player == Player.Dark ? Player.Light : Player.Dark);

			var my = players.Sum(v => Weight(v.x, v.z, board.size));
			var opp = opponents.Sum(v => Weight(v.x, v.z, board.size));
		

			return my - opp;
		}

		/// <summary>
		/// 特定のセルの重みを返す
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="z">z座標</param>
		/// <param name="size">盤の辺の大きさ</param>
		/// <returns>セルの重み</returns>
		public static int Weight(int x, int z, int size)
		{
			switch (size)
			{
				case 4:
					return weight4[x < 2 ? x : 3 - x, z < 2 ? z : 3 - z];
				case 6:
					return weight6[x < 3 ? x : 5 - x, z < 3 ? z : 5 - z];
				case 8:
					return weight8[x < 4 ? x : 7 - x, z < 4 ? z : 7 - z];
				default:
					throw new ArgumentException();
			}
		}

		/// <summary>
		/// 現在番のプレイヤーが特定のセルに打ったときの重み調節済み開放度を求める
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <param name="x">打つx座標</param>
		/// <param name="z">打つz座標</param>
		/// <returns>重み付きの開放度</returns>
		public static int WeightedOpenness(Board board, int x, int z)
		{
			return WeightedOpenness(board, x, z, board.turn);
		}

		/// <summary>
		/// 特定のプレイヤーが特定のセルに打ったときの重み調節済み開放度を求める
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <param name="x">打つx座標</param>
		/// <param name="z">打つz座標</param>
		/// <param name="player">打つプレイヤー</param>
		/// <returns>重み付きの開放度</returns>
		public static int WeightedOpenness(Board board, int x, int z, Player player)
		{
			return 90 * CalculateOpenness(board, x, z, player);
		}

		/// <summary>
		/// 現在番のプレイヤーが特定のセルに打ったときの開放度を求める
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <param name="x">打つx座標</param>
		/// <param name="z">打つz座標</param>
		/// <returns>開放度</returns>
		public static int CalculateOpenness(Board board, int x, int z)
		{
			return CalculateOpenness(board, x, z, board.turn);
		}

		/// <summary>
		/// 開放度理論に従って手に対する開放度を計算する
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <param name="x">打つx座標</param>
		/// <param name="z">打つz座標</param>
		/// <param name="player">打つプレイヤー</param>
		/// <returns>開放度</returns>
		public static int CalculateOpenness(Board board, int x, int z, Player player)
		{
			if (!board.CanPlace(x, z, player))
			{
				throw new ArgumentException();
			}
			var turnovers = board.GetTurnovers(x, z, player);
			var openness = 0;
			foreach (var turnover in turnovers)
			{
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						if (i != 0 || j != 0)
						{
							var a = turnover.x + i;
							var b = turnover.z + j;
							if (0 <= a && a < board.size && 0 <= b && b < board.size && board.IsEmpty(a, b))
							{
								openness++;
							}
						}
					}
				}
			}
			return openness;
		}
	}
}
