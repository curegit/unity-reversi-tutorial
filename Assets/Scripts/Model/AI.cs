using System;
using System.Linq;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// オセロのAI
	/// </summary>
	public static class AI
	{
		/// <summary>
		/// 4x4で完全読みをする盤の埋まり率
		/// </summary>
		private const double SearchDefinitiveThreshold4 = 0.0;

		/// <summary>
		/// 6x6で完全読みをする盤の埋まり率
		/// </summary>
		private const double SearchDefinitiveThreshold6 = 0.6;

		/// <summary>
		/// 8x8で完全読みをする盤の埋まり率
		/// </summary>
		private const double SearchDefinitiveThreshold8 = 0.85;

		/// <summary>
		/// 神手
		/// </summary>
		private const int GotMove = 10000000;

		/// <summary>
		/// 糞手
		/// </summary>
		private const int ShittyMove = -10000000;

		/// <summary>
		/// ヒューリスティックで読む深さ
		/// </summary>
		private const int GuaranteedRecursion = 3;

		/// <summary>
		/// パスしたときは再帰深さを減らさない
		/// </summary>
		private static readonly bool DontDecreaseRecursionOnPass = true;

		/// <summary>
		/// 次の手を考える
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <returns>次に打つ位置</returns>
		public static Position Think(Board board)
		{
			if (!board.CanMove())
			{
				throw new InvalidOperationException();
			}
			if (board.size == 4 && board.FillRate() > SearchDefinitiveThreshold4 || board.size == 6 && board.FillRate() > SearchDefinitiveThreshold6 || board.size == 8 && board.FillRate() > SearchDefinitiveThreshold8)
			{
				return SearchTreeDefinitive(board);
			}
			else
			{
				return SearchTreeHeuristic(board);
			}
		}

		/// <summary>
		/// 全探索
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <returns>最良の手の位置</returns>
		private static Position SearchTreeDefinitive(Board board)
		{
			if (board.IsEnd() || !board.CanMove())
			{
				throw new InvalidOperationException();
			}
			var moves = board.GetMoves();
			int max = int.MinValue;
			var pos = new Position();
			for (int i = 0; i < moves.Count(); i++)
			{
				var next = board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z);
				int value = SearchTreeDefinitive(next, board.turn);
				if (max < value)
				{
					max = value;
					pos = moves.ElementAt(i);
				}
			}
			return pos;
		}

		/// <summary>
		/// 全探索の補助関数
		/// </summary>
		/// <param name="board">この時点での盤</param>
		/// <param name="player">この時点での番</param>
		/// <returns>評価値</returns>
		private static int SearchTreeDefinitive(Board board, Player player)
		{
			if (board.IsEnd())
			{
				return board.Balance(player);
			}
			if (!board.CanMove())
			{
				return SearchTreeDefinitive(board.Pass(), player);
			}
			var moves = board.GetMoves();
			if (board.turn == player)
			{
				int max = int.MinValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeDefinitive(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player);
					if (max < value)
					{
						max = value;
					}
				}
				return max;
			}
			else
			{
				int min = int.MaxValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeDefinitive(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player);
					if (min > value)
					{
						min = value;
					}
				}
				return min;
			}
		}

		/// <summary>
		/// ヒューリスティック探索
		/// </summary>
		/// <param name="board">現在の盤</param>
		/// <returns>最良と思われる手の位置</returns>
		private static Position SearchTreeHeuristic(Board board)
		{
			if (board.IsEnd() || !board.CanMove())
			{
				throw new InvalidOperationException();
			}
			var moves = board.GetMoves();
			int max = int.MinValue;
			var pos = new Position();
			for (int i = 0; i < moves.Count(); i++)
			{
				int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), board.turn, GuaranteedRecursion - 1) - Evaluation.WeightedOpenness(board, moves.ElementAt(i).x, moves.ElementAt(i).z);
				if (max < value)
				{
					max = value;
					pos = moves.ElementAt(i);
				}
			}
			return pos;
		}

		/// <summary>
		/// ヒューリスティック探索の補助関数
		/// </summary>
		/// <param name="board">この時点での盤</param>
		/// <param name="player">この時点での番</param>
		/// <param name="recursion">残りの再帰</param>
		/// <returns>評価値</returns>
		private static int SearchTreeHeuristic(Board board, Player player, int recursion)
		{
			if (board.IsEnd())
			{
				return board.Balance(player) > 0 ? GotMove : ShittyMove;
			}
			if (recursion <= 0)
			{
				return Evaluation.Evaluate(board, player);
			}
			if (!board.CanMove())
			{
				return SearchTreeHeuristic(board.Pass(), player, DontDecreaseRecursionOnPass ? recursion : recursion - 1);
			}
			var moves = board.GetMoves();
			if (board.turn == player)
			{
				int max = int.MinValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, recursion - 1);
					if (value > max)
					{
						max = value;
					}
				}
				return max;
			}
			else
			{
				int min = int.MaxValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, recursion - 1);
					if (min > value)
					{
						min = value;
					}
				}
				return min;
			}
		}
	}
}
