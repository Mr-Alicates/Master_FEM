using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace POC3D.ViewModel.Calculations
{
    public class NumericMatrix
    {
        private Matrix<double> _rawMatrix;

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

                return _rawMatrix.At(row, column);
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
            return m1._rawMatrix * m2._rawMatrix;
        }

        public static NumericMatrix operator +(NumericMatrix m1, NumericMatrix m2)
        {
            if (m2 == null) return m1;

            if (m1 == null) return m2;

            if (m1.Rows != m2.Rows ||
                m1.Columns != m2.Columns)
                throw new InvalidOperationException("Invalid operation: Rows and Columns mismatch");

            return m1._rawMatrix + m2._rawMatrix;
        }

        public void RemoveColumn(int index)
        {
            _rawMatrix = _rawMatrix.RemoveColumn(index);
        }

        public void RemoveRow(int index)
        {
            _rawMatrix = _rawMatrix.RemoveRow(index);
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

        public NumericMatrix Solve(NumericMatrix otherMatrix)
        {
            var determinant = _rawMatrix.Determinant();

            var vector = ToVector(otherMatrix);

            var result = _rawMatrix.Solve(vector);

            return result;
        }

        private static Vector<double> ToVector(NumericMatrix matrix)
        {
            var rawMatrix = matrix._rawMatrix;

            if (matrix.Columns > 1) throw new InvalidOperationException("Not a vector!");

            var result = Vector<double>.Build
                .Dense(rawMatrix.EnumerateRows().Count(), rowIndex => rawMatrix[rowIndex, 0]);

            return result;
        }

        public static implicit operator NumericMatrix(Matrix<double> rawMatrix)
        {
            return new NumericMatrix(rawMatrix);
        }

        public static implicit operator NumericMatrix(Vector<double> rawVector)
        {
            var rawMatrix = Matrix<double>.Build
                .Dense(rawVector.Count, 1, (rowIndex, columnIndex) => rawVector[rowIndex]);

            return new NumericMatrix(rawMatrix);
        }
    }
}