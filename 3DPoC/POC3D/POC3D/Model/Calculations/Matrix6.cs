using System;

namespace POC3D.Model.Calculations
{
    public class Matrix6
    {
        private static readonly int[] Indexes = {1, 2, 3, 4, 5, 6};

        private readonly double[][] _rawMatrix;

        public Matrix6(double[][] rawMatrix)
        {
            if (rawMatrix.Length != 6) throw new InvalidOperationException("A 6x6 matrix is required");

            _rawMatrix = rawMatrix;

            foreach (var row in rawMatrix)
                if (row == null || row.Length != 6)
                    throw new InvalidOperationException("A 6x6 matrix is required");
        }

        public double this[int row, int column]
        {
            get
            {
                if (row < 1 || row > 6) throw new InvalidOperationException("Indices are 1-based");

                if (column < 1 || column > 6) throw new InvalidOperationException("Indices are 1-based");

                return _rawMatrix[row - 1][column - 1];
            }
        }

        public Matrix6 Transpose()
        {
            double[][] newRawMatrix =
            {
                new double[6],
                new double[6],
                new double[6],
                new double[6],
                new double[6],
                new double[6]
            };

            foreach (var row in Indexes)
            foreach (var column in Indexes)
                newRawMatrix[row - 1][column - 1] = this[column, row];

            return new Matrix6(newRawMatrix);
        }
    }
}