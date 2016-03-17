using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticRegression
{
    class Solution : IComparable<Solution>
    {
        public double[] weights; // a potential solution
        public double error; // MSE of weights

        public Solution(int numFeatures)
        {
            this.weights = new double[numFeatures + 1]; // problem dim + constant
            this.error = 0.0;
        }

        public int CompareTo(Solution other) // low-to-high error
        {
            if (this.error < other.error)
                return -1;
            else if (this.error > other.error)
                return 1;
            else
                return 0;

        }
    }
}
