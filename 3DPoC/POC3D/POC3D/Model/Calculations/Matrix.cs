using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Calculations
{
    public class Matrix
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly double[][] _rawMatrix;

        public Matrix(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _rawMatrix = Enumerable.Range(0, rows)
                .Select(x => new double[columns])
                .ToArray();
        }

        public int Rows => _rows;

        public int Columns => _columns;

        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    throw new InvalidOperationException("Invalid row");
                }

                if (column < 0 || column >= Columns)
                {
                    throw new InvalidOperationException("Invalid column");
                }

                return _rawMatrix[row][column];
            }
            set
            {
                if (row < 0 || row >= Rows)
                {
                    throw new InvalidOperationException("Invalid row");
                }

                if (column < 0 || column >= Columns)
                {
                    throw new InvalidOperationException("Invalid column");
                }

                _rawMatrix[row][column] = value;
            }
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Columns ||
                m1.Columns != m2.Rows)
            {
                throw new InvalidOperationException("Invalid operation: Rows and Columns mismatch");
            }

            Matrix result = new Matrix(m2.Rows, m1.Columns);

            for (int currentRow = 0; currentRow < m1.Rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < m2.Columns; currentColumn++)
                {
                    result[currentRow, currentColumn] = Enumerable.Range(0, m1.Columns)
                        .Sum(index => m1[currentRow, index] * m2[index, currentColumn]);
                }
            }

            return result;
        }

        public Matrix Transpose()
        {
            Matrix result = new Matrix(Columns, Rows);

            for (int currentRow = 0; currentRow < Rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < Columns; currentColumn++)
                {
                    result[currentColumn, currentRow] = this[currentRow, currentColumn];
                }
            }

            return result;
        }
    }
}
