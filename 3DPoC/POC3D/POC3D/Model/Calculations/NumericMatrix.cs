using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace POC3D.Model.Calculations
{
    public class NumericMatrix
    {
        private readonly Matrix<double> _rawMatrix;

        public NumericMatrix(int rows, int columns)
            : this(Matrix<double>.Build.Dense(rows, columns))
        {
        }

        private NumericMatrix(Matrix<double> rawMatrix)
        {
            _rawMatrix = rawMatrix;
        }

        public int Rows => _rawMatrix.EnumerateRows().Count();

        public int Columns => _rawMatrix.EnumerateColumns().Count();

        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows) throw new InvalidOperationException("Invalid row");

                if (column < 0 || column >= Columns) throw new InvalidOperationException("Invalid column");

                return _rawMatrix.At(row,column);
            }
            set
            {
                if (row < 0 || row >= Rows) throw new InvalidOperationException("Invalid row");

                if (column < 0 || column >= Columns) throw new InvalidOperationException("Invalid column");

                _rawMatrix.At(row, column, value);
            }
        }

        public static NumericMatrix operator *(NumericMatrix m1, NumericMatrix m2)
        {
            if (m1.Rows != m2.Columns ||
                m1.Columns != m2.Rows)
                throw new InvalidOperationException("Invalid operation: Rows and Columns mismatch");

            return m1._rawMatrix * m2._rawMatrix;
        }

        public NumericMatrix Transpose()
        {
            return _rawMatrix.Transpose();
        }

        public NumericMatrix GetSubMatrix(int originRow, int originColumn, int size)
        {
            return _rawMatrix.SubMatrix(originRow, size, originColumn, size);
        }

        public void SetSubMatrix(int originRow, int originColumn, NumericMatrix source)
        {
            _rawMatrix.SetSubMatrix(originRow, originColumn, source._rawMatrix);
        }

        public double CalculateDeterminant()
        {
            return _rawMatrix.Determinant();
        }

        public static implicit operator NumericMatrix(Matrix<double> rawMatrix)
        {
            return new NumericMatrix(rawMatrix);
        }
    }
}