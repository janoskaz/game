/* Dice has Nr of facets, which delimits range of possible rolls
 */
using System;

/// <summary>
/// Dice is a static class with one method - roll, which accepts nr of facets of the dice.
/// </summary>
namespace Game
{
	static class Dice
	{
		public static Random rnd = new Random();
		
		/// <summary>
		/// Rolls the dice with specified range from 1 to n+1.
		/// </summary>
		/// <param name='n'>
		/// N.
		/// </param>
		public static int Roll(int n)
		{
			return rnd.Next(1, n+1);
		}
	}
}

