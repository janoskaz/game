/* Dice has Nr of facets, which delimits range of possible rolls
 */
using System;

/// <summary>
/// Dice.
/// </summary>
namespace Game
{
	public class Dice		
	{
		/// <summary>
		/// The random number.
		/// </summary>
		private Random random;
		
		/// <summary>
		/// The nr facets of the dice.
		/// </summary>
		public int NrFacets {get; private set;}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Arena.Dice"/> class.
		/// </param>
		public Dice( int n )
		{
			this.NrFacets = n;
			this.random = new Random();
		}
		
		/// <summary>
		/// Roll a Dice.
		/// </summary>
		public int Roll()
		{
			return random.Next(1, this.NrFacets+1);
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Game.Dice"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Game.Dice"/>.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("[Dice] with {0} facets", this.NrFacets);
		}
		
	}
}

