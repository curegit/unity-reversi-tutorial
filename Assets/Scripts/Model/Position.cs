using System;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// 盤上の位置を表すオブジェクト
	/// </summary>
	[Serializable]
	public struct Position : IEquatable<Position>
	{
		/// <summary>
		/// x座標
		/// </summary>
		public int x;

		/// <summary>
		/// z座標
		/// </summary>
		public int z;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="x">x座標</param>
		/// <param name="z">z座標</param>
		public Position(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		/// <summary>
		/// 位置を比較する
		/// </summary>
		/// <param name="other">比較相手</param>
		/// <returns>真偽値</returns>
		public bool Equals(Position other)
		{
			return x == other.x && z == other.z;
		}

		/// <summary>
		/// オブジェクトを比較する
		/// </summary>
		/// <param name="obj">比較相手</param>
		/// <returns>真偽値</returns>
		public override bool Equals(object obj)
		{
			return obj is Position && (Position)obj == this;
		}

		/// <summary>
		/// ハッシュ値を返す
		/// </summary>
		/// <returns>ハッシュ</returns>
		public override int GetHashCode()
		{
			return x ^ z;
		}

		/// <summary>
		/// 等値比較演算子
		/// </summary>
		/// <param name="position1">左オペランド</param>
		/// <param name="position2">右オペランド</param>
		/// <returns>真偽値</returns>
		public static bool operator ==(Position position1, Position position2)
		{
			return position1.Equals(position2);
		}

		/// <summary>
		/// 不等比較演算子
		/// </summary>
		/// <param name="position1">左オペランド</param>
		/// <param name="position2">右オペランド</param>
		/// <returns>真偽値</returns>
		public static bool operator !=(Position position1, Position position2)
		{
			return !position1.Equals(position2);
		}
	}
}
