using MultiThreading.Task3.MatrixMultiplier.Matrices;
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
	public class MatricesMultiplierParallel : IMatricesMultiplier
	{
		public IMatrix Multiply(IMatrix m1, IMatrix m2)
		{
			var resultMatrix = new Matrix(m1.RowCount, m2.ColCount, true);

			for (long i = 0; i < m1.RowCount; i++)
			{
				Parallel.For(0, (int)m2.ColCount, j =>
				{
					resultMatrix.SetElement(i, j, CalcSum(i, j, m1, m2));
				});
			};

			return resultMatrix;
		}

		private long CalcSum(long i, int j, IMatrix m1, IMatrix m2)
		{
			long sum = 0;
			for (byte k = 0; k < m1.ColCount; k++)
			{
				sum += m1.GetElement(i, k) * m2.GetElement(k, j);
			}
			return sum;
		}
	}
}
