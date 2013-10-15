namespace Introcs.Practice {

using System;

/// <summary>Classic puzzles class.</summary>
public static class ClassicPuzzles {
	private class HanoiArgs {
		public int src, dest, spare, ndisks, acc, splits;
		
		public HanoiArgs(int src = 0, int dest = 0, int spare = 0,
			int ndisks = 0, int acc = 0, int splits = 0)
		{
			this.src = src; this.dest = dest; this.spare = spare;
			this.ndisks = ndisks; this.acc = acc; this.splits = splits;
		}
	}
	
	private static void HanoiHelper(HanoiArgs argsIn, int exp2Ndisks,
		int[][] arr)
	{
		int idx = argsIn.acc - 1;
		
		if (0 == argsIn.ndisks)
			return;
		HanoiArgs ha1 = new HanoiArgs(argsIn.src, argsIn.spare, argsIn.dest,
			argsIn.ndisks - 1,
			argsIn.acc - exp2Ndisks / (int)Math.Pow(2.0f, argsIn.splits + 1),
			argsIn.splits + 1);
		HanoiArgs ha2 = new HanoiArgs(argsIn.spare, argsIn.dest, argsIn.src,
			argsIn.ndisks - 1,
			argsIn.acc + exp2Ndisks / (int)Math.Pow(2.0f, argsIn.splits + 1),
			argsIn.splits + 1);
		arr[idx][0] = argsIn.src;
		arr[idx][1] = argsIn.dest;
		HanoiHelper(ha1, exp2Ndisks, arr);
		HanoiHelper(ha2, exp2Ndisks, arr);
	}
	
	public static int[][] Hanoi(int src, int dest, int spare, int ndisks)
	{
		int exp2Ndisks = (int)Math.Pow(2.0f, ndisks);
		int lenArr = exp2Ndisks - 1, splits = 1;
		
		int[][] arr = new int[lenArr][];
		for (int i = 0; lenArr > i; ++i)
			arr[i] = new int[2];
		
		HanoiArgs argsIn = new HanoiArgs(src, dest, spare, ndisks,
			exp2Ndisks / (int)Math.Pow(2.0f, splits), splits);
		HanoiHelper(argsIn, exp2Ndisks, arr);
		return arr;
	}
	
	private static bool Safep(int r, int c, int col, int[] board)
	{
		return (board[c] == r) || (Math.Abs(board[c] - r) == col - c);
	}
	
	private static int NqueensHelper(int queensNdx, int numqueens, int col,
		int[] board)
	{
		if (numqueens == col)
			return queensNdx - 1;
		for (int r = 0, c = 0; numqueens > r; ++r) {
			for (c = 0; col > c && !Safep(r, c, col, board); ++c);
			
			if (col > c)
				continue;
			board[col] = r;
			queensNdx = NqueensHelper(queensNdx, numqueens, col + 1, board);
			if (0 == queensNdx)
				return queensNdx;
		}
		return queensNdx;
	}
	
	public static int[] Nqueens(int ndx, int numqueens)
	{
		int[] board = new int[numqueens];
		int queensNdx = ndx;
		queensNdx = NqueensHelper(queensNdx, numqueens, 0, board);
		return board;
	}
	
	/// <summary>Main entry point.</summary>
    /// <param name="args">An array</param>
    /// <returns>The exit code.</returns>
	public static int Main(string[] args)
    {
		int n = 8;
		Console.Write("Nqueens(0, {0}) : [{1}]\n", n,
			String.Join(", ", Nqueens(0, n)));
		return 0;
	}
}

}
