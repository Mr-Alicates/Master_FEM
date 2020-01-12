using System;
using System.Linq;

namespace POC3D.Model.Calculations
{
    public class Matrix : BaseMatrix<double>
    {
        public Matrix(int rows, int columns)
            : base(rows, columns)
        {
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Columns ||
                m1.Columns != m2.Rows)
                throw new InvalidOperationException("Invalid operation: Rows and Columns mismatch");

            var result = new Matrix(m2.Rows, m1.Columns);

            for (var currentRow = 0; currentRow < m1.Rows; currentRow++)
            for (var currentColumn = 0; currentColumn < m2.Columns; currentColumn++)
                result[currentRow, currentColumn] = Enumerable.Range(0, m1.Columns)
                    .Sum(index => m1[currentRow, index] * m2[index, currentColumn]);

            return result;
        }

        public Matrix Transpose()
        {
            var result = new Matrix(Columns, Rows);

            for (var currentRow = 0; currentRow < Rows; currentRow++)
            for (var currentColumn = 0; currentColumn < Columns; currentColumn++)
                result[currentColumn, currentRow] = this[currentRow, currentColumn];

            return result;
        }

        public Matrix GetSubMatrix(int originRow, int originColumn, int size)
        {
            var result = new Matrix(size, size);

            for (var row = 0; row < size; row++)
            for (var column = 0; column < size; column++)
                result[row, column] = this[row + originRow, column + originColumn];

            return result;
        }

        public void SetSubMatrix(int originRow, int originColumn, Matrix source)
        {
            for (var row = 0; row < source.Rows; row++)
            for (var column = 0; column < source.Columns; column++)
                this[row + originRow, column + originColumn] = source[row, column];
        }
    }
}