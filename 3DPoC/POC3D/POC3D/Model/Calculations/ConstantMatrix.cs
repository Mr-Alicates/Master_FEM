using System;
using System.Linq;

namespace POC3D.Model.Calculations
{
    public class ConstantMatrix
    {
        private readonly string[][] _rawMatrix;

        protected ConstantMatrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _rawMatrix = Enumerable.Range(0, rows)
                .Select(x => new string[columns])
                .ToArray();
        }

        public int Rows { get; }

        public int Columns { get; }

        public string this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows) throw new InvalidOperationException("Invalid row");

                if (column < 0 || column >= Columns) throw new InvalidOperationException("Invalid column");

                return _rawMatrix[row][column];
            }
            set
            {
                if (row < 0 || row >= Rows) throw new InvalidOperationException("Invalid row");

                if (column < 0 || column >= Columns) throw new InvalidOperationException("Invalid column");

                _rawMatrix[row][column] = value;
            }
        }
    }
}