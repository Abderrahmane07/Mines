using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRevealed { get; set; }
        public int Neighbors { get; set; }
        public string Value { get; set; }

        public Cell()
        {
            IsBomb = false;
            IsFlagged = false;
            IsRevealed = false;
            Neighbors = 0;
            Value = "X ";
        }
    }
}
