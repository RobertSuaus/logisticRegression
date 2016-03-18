using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogisticRegression
{
    public class LogisticClassifier
    {
        private int numFeatures; // number of independent variables aka features 
        private double[] weights; // b0 = constant private 
        private TextBox textArea;
        static Random _random = new Random(Guid.NewGuid().GetHashCode());
        Random rnd;
        public LogisticClassifier(int numFeatures, TextBox textArea)
        {
            this.numFeatures = numFeatures; // number of features/predictors 
            this.weights = new double[numFeatures + 1]; // [0] = b0 constant 
            this.textArea = textArea;
        }

        public double[] Train(double[][] trainData, int maxEpochs, double alpha)
        {
            int epoch = 0;
            int[] sequence = new int[trainData.Length];
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;
            while (epoch < maxEpochs)
            {
                ++epoch;
                if (epoch % 100 == 0 && epoch != maxEpochs)
                {
                    double mse = Error(trainData, weights);
                    textArea.AppendText("Epoch = "+ epoch);
                    textArea.AppendText(" error = " + mse.ToString("F4"));
                    textArea.AppendText(Environment.NewLine);
                }
                Shuffle(sequence); // Process data in random order
                for (int ti = 0; ti < trainData.Length; ++ti)
                {
                    int i = sequence[ti];
                    double computed = ComputeOutput(trainData[i], weights);
                    int targetIndex = trainData[i].Length - 1;
                    double target = trainData[i][targetIndex];
                    weights[0] += alpha * (target - computed) * 1;
                    for (int j = 1; j < weights.Length; ++j)
                        weights[j] += alpha * (target - computed) * trainData[i][j - 1];
                }
            } // While loop
            return this.weights;
        } // Train

        static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        private double Error(double[][] trainData, double[] weights)
        {
            // mean squared error using supplied weights
            int yIndex = trainData[0].Length - 1; // y-value (0/1) is last column
            double sumSquaredError = 0.0;
            for (int i = 0; i < trainData.Length; ++i) // each data
            {
                double computed = ComputeOutput(trainData[i], weights);
                double desired = trainData[i][yIndex]; // ex: 0.0 or 1.0
                sumSquaredError += (computed - desired) * (computed - desired);
            }
            return sumSquaredError / trainData.Length;
        }

        public double ComputeOutput(double[] dataItem, double[] weights)
        {
            double z = 0.0;
            z += weights[0]; // the b0 constant
            for (int i = 0; i < weights.Length - 1; ++i) // data might include Y
                z += (weights[i + 1] * dataItem[i]); // skip first weight
            return 1.0 / (1.0 + Math.Exp(-z)); //Sigmoid function
        }

        public int ComputeDependent(double[] dataItem, double[] weights)
        {
            double sum = ComputeOutput(dataItem, weights);
            if (sum <= 0.5)
                return 0;
            else
                return 1;
        }

        public double Accuracy(double[][] trainData, double[] weights)
        {
            int numCorrect = 0;
            int numWrong = 0;
            int yIndex = trainData[0].Length - 1;
            for (int i = 0; i < trainData.Length; ++i)
            {
                double computed = ComputeDependent(trainData[i], weights); // implicit cast
                double desired = trainData[i][yIndex]; // 0.0 or 1.0
                log("Row #" +i);
                log("Real Y Value: "+desired.ToString() + " Computed Y Value: "+computed.ToString());
                if (computed == desired) // risky?
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numWrong + numCorrect);
        }

        public void log(String str)
        {
            textArea.AppendText(str);
            textArea.AppendText(Environment.NewLine);
        }

        
    }
}