using System;

namespace ReversiTutorial.Model
{
	/// <summary>
	/// 盤上の位置を表す
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
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Position other)
		{
			return x == other.x && z == other.z;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is Position && (Position)obj == this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return x ^ z;
		}

		public static bool operator ==(Position position1, Position position2)
		{
			return position1.Equals(position2);
		}

		public static bool operator !=(Position position1, Position position2)
		{
			return !position1.Equals(position2);
		}
	}
}
