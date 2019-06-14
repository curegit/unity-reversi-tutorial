using System;
using System.Linq;
using System.Collections.Generic;


using UnityEngine;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// 盤の状態を表すイミュータブルなオブジェクト
	/// </summary>
	[Serializable]
	public class Board
	{
		/// <summary>
		/// 盤の辺の大きさ (4, 6, or 8)
		/// </summary>
		public int size { get; private set; }

		/// <summary>
		/// どのプレイヤーの番か
		/// </summary>
		public Player turn { get; private set; }

		/// <summary>
		/// 石の状態 (nullが空)
		/// </summary>
		private Player?[,] disks;

		/// <summary>
		/// サイズ8で初期化するコンストラクタ
		/// </summary>
		public Board() : this(8) { }

		/// <summary>
		/// 初期状態の盤をつくるコンストラクタ
		/// </summary>
		/// <param name="size">盤の辺の大きさ (4, 6, or 8)</param>
		public Board(int size)
		{
			if (size != 4 && size != 6 && size != 8)
			{
				throw new ArgumentException();
			}
			this.size = size;
			turn = Player.Dark;
			disks = new Player?[size, size];
			disks[size / 2, size / 2 - 1] = Player.Dark;
			disks[size / 2 - 1, size / 2 - 1] = Player.Light;
			disks[size / 2 - 1, size / 2] = Player.Dark;
			disks[size / 2, size / 2] = Player.Light;
		}

		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="board">ソース</param>
		private Board(Board board)
		{
			size = board.size;
			turn = board.turn;
			disks = (Player?[,])board.disks.Clone();
		}

		/// <summary>
		/// 盤のコピーを返す（コピーコンストラクタのラップ）
		/// </summary>
		/// <returns>コピーされた盤</returns>
		private Board Copy()
		{
			return new Board(this);
		}

		/// <summary>
		/// 石を置いた次の状態の盤を返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>次の状態の盤</returns>
		public Board Place(int x, int z)
		{
			if (!CanPlace(x, z))
			{
				throw new InvalidOperationException();
			}
			var b = Copy();
			
			var turnovers = b.GetTurnovers(x, z, turn);
			foreach (var turnover in turnovers)
			{
				b.disks[turnover.x, turnover.z] = b.disks[turnover.x, turnover.z] == Player.Dark ? Player.Light : Player.Dark;
			}
			b.disks[x, z] = turn;
			b.turn = b.turn == Player.Dark ? Player.Light : Player.Dark;
			return b;
		}

		/// <summary>
		/// 自分の番をパスした盤を返す
		/// </summary>
		/// <returns>次の状態の盤</returns>
		public Board Pass()
		{
			if (CanMove())
			{
				throw new InvalidOperationException();
			}
			var b = Copy();
			b.turn = b.turn == Player.Dark ? Player.Light : Player.Dark;
			return b;
		}

		/// <summary>
		/// 特定のセルに石が置かれているか調べる
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>空なら真</returns>
		public bool IsEmpty(int x, int z)
		{
			return disks[x, z] == null;
		}

		/// <summary>
		/// 特定セルの石を返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>石の種類</returns>
		public Player? GetDisk(int x, int z)
		{
			return disks[x, z];
		}

		/// <summary>
		/// 空のセルの数を返す
		/// </summary>
		/// <returns>空のセルの数</returns>
		public int CountEmpty()
		{
			return Enumerable.Range(0, size).SelectMany(x => Enumerable.Range(0, size).Select(z => new Position(x, z))).Count(v => IsEmpty(v.x, v.z));
		}

		/// <summary>
		/// セルの埋まっている割合を返す
		/// </summary>
		/// <returns>石がある割合</returns>
		public double FillRate()
		{
			return 1.0 - (double)CountEmpty() / size * size;
		}

		/// <summary>
		/// 特定プレイヤーのすべての石の位置を返す
		/// </summary>
		/// <param name="player">特定プレイヤー</param>
		/// <returns>石の位置のシーケンス</returns>
		public IEnumerable<Position> DiskPositions(Player player)
		{
			return Enumerable.Range(0, size).SelectMany(x => Enumerable.Range(0, size).Select(z => new Position(x, z))).Where(v => disks[v.x, v.z] == player);
		}

		/// <summary>
		/// 特定プレイヤーの石の数を返す
		/// </summary>
		/// <param name="player">特定プレイヤー</param>
		/// <returns>石の数</returns>
		public int Count(Player player)
		{
			return Enumerable.Range(0, size).SelectMany(x => Enumerable.Range(0, size).Select(z => new Position(x, z))).Count(v => disks[v.x, v.z] == player);
		}

		/// <summary>
		/// 特定プレイヤーの石の数から相手プレイヤーの石の数を引いたものを返す
		/// </summary>
		/// <param name="player">特定のプレイヤー</param>
		/// <returns>石の差</returns>
		public int Balance(Player player)
		{
			return Count(player) - Count(player == Player.Dark ? Player.Light : Player.Dark);
		}

		/// <summary>
		/// 現在の番のプレイヤーが特定のセルに石を置いたときに返せる石をすべて返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>返せる石のコレクション</returns>
		public IEnumerable<Position> GetTurnovers(int x, int z)
		{
			return GetTurnovers(x, z, turn);
		}

		/// <summary>
		/// 特定のプレイヤーが特定のセルに石を置いたときに返せる石をすべて返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <param name="player">特定プレイヤー</param>
		/// <returns>返せる石のコレクション</returns>
		public IEnumerable<Position> GetTurnovers(int x, int z, Player player)
		{
			if (x < 0 || size <= x || z < 0 || size <= z || !IsEmpty(x, z))
			{
				throw new ArgumentOutOfRangeException();
			}
			var list = new List<Position>();
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i != 0 || j != 0)
					{
						var tmp = new List<Position>();
						for (int k = 1; true; k++)
						{
							var a = x + i * k;
							var b = z + j * k;
							if (a < 0 || size <= a || b < 0 || size <= b || IsEmpty(a, b))
							{
								break;
							}
							else if (disks[a, b] == player)
							{
								list.AddRange(tmp);
								break;
							}
							else
							{
								tmp.Add(new Position(a, b));
							}
						}
					}
				}
			}


			return list.AsReadOnly();
		}

		/// <summary>
		/// 現在の番のプレイヤーが特定のセルに石を置いたときに返せる石の数を返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>返せる石の数</returns>
		public int CountTurnovers(int x, int z)
		{
			return CountTurnovers(x, z, turn);
		}

		/// <summary>
		/// 特定のプレイヤーが特定のセルに石を置いたときに返せる石の数を返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <param name="player">特定プレイヤー</param>
		/// <returns>返せる石の数</returns>
		public int CountTurnovers(int x, int z, Player player)
		{
			return GetTurnovers(x, z, player).Count();
		}

		/// <summary>
		/// 現在自分の番のプレイヤーが特定のセルに石を置けるかどうかを返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <returns>石を置けるかどうか</returns>
		public bool CanPlace(int x, int z)
		{
			return CanPlace(x, z, turn);
		}

		/// <summary>
		/// あるプレイヤーが特定のセルに石を置けるかどうかを返す
		/// </summary>
		/// <param name="x">x方向の位置</param>
		/// <param name="z">z方向の位置</param>
		/// <param name="player">特定のプレイヤー</param>
		/// <returns>石を置けるかどうか</returns>
		public bool CanPlace(int x, int z, Player player)
		{
			if (!IsEmpty(x, z))
			{
				return false;
			}
			return CountTurnovers(x, z, player) > 0;
		}

		/// <summary>
		/// 現在自分の番のプレイヤーが打つことができる手を返す
		/// </summary>
		/// <returns>現在自分の番のプレイヤーが打つことができる手</returns>
		public IEnumerable<Position> GetMoves()
		{
			return GetMoves(turn);
		}

		/// <summary>
		/// あるプレイヤーが打つことができる手を返す
		/// </summary>
		/// <param name="player">特定のプレイヤー</param>
		/// <returns>あるプレイヤーが打つことができる手</returns>
		public IEnumerable<Position> GetMoves(Player player)
		{
			return Enumerable.Range(0, size).SelectMany(x => Enumerable.Range(0, size).Select(z => new Position(x, z))).Where(v => CanPlace(v.x, v.z, player));
		}

		/// <summary>
		/// 現在自分の番のプレイヤーが打つことができる手が存在するか返す
		/// </summary>
		/// <returns>打つことができる手が存在するか</returns>
		public bool CanMove()
		{
			return GetMoves().Any();
		}

		/// <summary>
		/// あるプレイヤーが打つことができる手が存在するか返す
		/// </summary>
		/// <param name="player">特定のプレイヤー</param>
		/// <returns>打つことができる手が存在するか</returns>
		public bool CanMove(Player player)
		{
			return GetMoves(player).Any();
		}

		/// <summary>
		/// ゲームが終了しているか判定する
		/// </summary>
		/// <returns>ゲームが終了しているか</returns>
		public bool IsEnd()
		{
			return !CanMove(Player.Dark) && !CanMove(Player.Light);
		}
	}
}
