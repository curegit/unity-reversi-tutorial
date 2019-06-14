using System;
using System.Linq;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// 
	/// </summary>
	public static class AI
	{
		/// <summary>
		/// 
		/// </summary>
		private const double SearchDefinitiveThreshold4 = 0.0;

		/// <summary>
		/// 
		/// </summary>
		private const double SearchDefinitiveThreshold6 = 0.5;

		/// <summary>
		/// 
		/// </summary>
		private const double SearchDefinitiveThreshold8 = 0.83;

		/// <summary>
		/// 
		/// </summary>
		private const int GotMove = 10000000;

		/// <summary>
		/// 
		/// </summary>
		private const int ShittyMove = -10000000;

		/// <summary>
		/// 
		/// </summary>
		private const int GuaranteedRecursion = 3;

		/// <summary>
		/// 
		/// </summary>
		private static readonly bool DontDecreaseRecursionOnPass = true;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public static Position Think(Board board)
		{
			if (!board.CanMove())
			{
				throw new InvalidOperationException();
			}
			if (board.size == 4 && board.FillRate() > SearchDefinitiveThreshold4 || board.size == 6 && board.FillRate() > SearchDefinitiveThreshold6 || board.size == 8 && board.FillRate() > SearchDefinitiveThreshold8)
			{
				return SearchTreeDefinitive(board, true);
			}
			else
			{
				return SearchTreeHeuristic(board);
			}
		}


		private static Position SearchTreeDefinitive(Board board, bool optimistic = false)
		{
			var moves = board.GetMoves();
			int max = int.MinValue;
			var pos = new Position();
			for (int i = 0; i < moves.Count(); i++)
			{
				int value = SearchTreeDefinitive(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), board.turn, i == moves.Count() - 1 ? max : (int?)null);
				if (max < value)
				{
					max = value;
					pos = moves.ElementAt(i);
				}
			}
			if (max <= 0 && optimistic)
			{
				// TODO: ミニマックスで勝ち筋がないときのAIの行動選択を実装
			}
			return pos;
		}


		private static int SearchTreeDefinitive(Board board, Player player, int? alphabeta)
		{
			if (board.IsEnd())
			{
				return board.Balance(player);
			}
			if (!board.CanMove())
			{
				return SearchTreeDefinitive(board.Pass(), player, null);
			}
			var moves = board.GetMoves();
			if (board.turn == player)
			{
				int max = int.MinValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeDefinitive(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, i == moves.Count() - 1 ? max : (int?)null);
					if (max < value)
					{
						max = value;
						if (max >= alphabeta)
						{
							return max;
						}
					}
				}
				return max;
			}
			else
			{
				int min = int.MaxValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeDefinitive(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, i == moves.Count() - 1 ? min : (int?)null);
					if (min > value)
					{
						min = value;
						if (min <= alphabeta)
						{
							return min;
						}
					}
				}
				return min;
			}
		}

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


				int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), board.turn, i == moves.Count() - 1 ? max : (int?)null, GuaranteedRecursion - 1) - Evaluation.WeightedOpenness(board, moves.ElementAt(i).x, moves.ElementAt(i).z);




				if (max < value)
				{
					max = value;
					pos = moves.ElementAt(i);
				}
			}



			return pos;
		}

		private static int SearchTreeHeuristic(Board board, Player player, int? alphabeta, int recursion)
		{



			if (board.IsEnd())
			{
				return board.Balance(player) > 0 ? GotMove : ShittyMove;
			}
			if (recursion <= 0 && board.turn != player)
			{



				var a = Evaluation.Evaluate(board, player);

				return a;
			}
			if (!board.CanMove())
			{
				return SearchTreeHeuristic(board.Pass(), player, null, DontDecreaseRecursionOnPass ? recursion : recursion - 1);
			}
			var moves = board.GetMoves();
			if (board.turn == player)
			{
				int max = int.MinValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, i == moves.Count() - 1 ? max : (int?)null, recursion - 1);
					if (value > max)
					{
						max = value;
						if (max >= alphabeta)
						{
							return max;
						}
					}
				}
				return max;
			}
			else
			{
				int min = int.MaxValue;
				for (int i = 0; i < moves.Count(); i++)
				{
					int value = SearchTreeHeuristic(board.Place(moves.ElementAt(i).x, moves.ElementAt(i).z), player, i == moves.Count() - 1 ? min : (int?)null, recursion - 1);
					if (min > value)
					{
						min = value;
						if (min <= alphabeta)
						{
							return min;
						}
					}
				}
				return min;
			}
		}
	}
}