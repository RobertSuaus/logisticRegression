using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogisticRegression
{
    public partial class MainWindow : Form
    {
        public String filePath="";
        public double[][] initValues = new Double[100][];
        public double[] bestWeights = new Double[4];
        LogisticClassifier lc;
        public MainWindow()
        {
            InitializeComponent();
        }

        //Cuando main corre ya todos los datos están inicializados
        public void main()
        {
            //Graficar los valores iniciales
            log("Displaying initial values graph...");
            foreach (double[] data in initValues)
            {
                if (data[2] == 0)
                {
                    graph.Series["NotApproved"].Points.AddXY(data[0], data[1]);
                }
                else
                {
                    graph.Series["Approved"].Points.AddXY(data[0], data[1]);
                }
            }
            log("Data succesfully displayed.");
            log("Creating the logistic classifier");
            log("With 2 attributes: Score 1 and Score 2");
            //normalize data
            int[] columns = new int[] {0, 1};
            double[][] means = normalize(initValues, columns);
            int numFeatures = 2;
            lc = new LogisticClassifier(numFeatures, logBox);
            int maxEpochs = 1000;
            bestWeights = lc.Train(initValues, maxEpochs, 0.01);
            testToolStripMenuItem.Enabled=true;
            log("Weights found.");
            log("Ready to test sigmoid function with Weight Vector: ");
            showVector(bestWeights, 4);
            
            
        }

        //*****************************
        //*****************************
        //      Funciones para la ventana
        
        //Imprime a la ventana de registros
        public void log(String str)
        {
            logBox.AppendText(str);
            logBox.AppendText(Environment.NewLine);
        }

        public void log_noLine(String str)
        {
            logBox.AppendText(str);
        }

        //Solicita la ruta del archivo
        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = true;
            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                filePath = choofdlog.FileName;
                log("Selected file: " + filePath);
                log("Loading data from file... Please wait");
                readFileData();
            }

            else
            {
                filePath = string.Empty;
            }
        }

        //Leer los datos y almacenarlos en un double[][]
        private void readFileData()
        {
            var fileContents = System.IO.File.ReadAllLines(filePath);
            int i = 0;
            foreach (string line in fileContents)
            {
                var fields = line.Split(',');
                if (fields.Count() != 3)
                {
                    MessageBox.Show("Invalid set of data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log("Invalid file, try again...");
                    filePath = "";
                    return ;
                }
                var test1 = double.Parse(fields[0]);
                var test2 = double.Parse(fields[1]);
                var result = double.Parse(fields[2]);
                initValues[i] = new double[] { test1, test2, result };
                i++;
            }
            log("Data succesfully loaded");
            log("Ready to run!");
        }

        //Corre la aplicacion (Llama a main una vez que este todo listo)
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filePath == "")
            {
                MessageBox.Show("You must select a file.", 
                   "Hold it!",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            main();
            
        }

        //Mensaje de ayuda
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tarea 3 de IA\nRoberto González\nCristian Xool", "Regresion Logistica",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void showVector(double[] vector, int decimals) 
        {
            for (int i = 0; i < vector.Length; ++i)
                log_noLine(vector[i].ToString("F" + decimals) + " ");
            log("");
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        public double[][] normalize(double[][] rawData, int[] columns)
        {
            int numRows = rawData.Length;
            int numCols = rawData[0].Length;

            double[][] result = new double[2][];
            for (int i = 0; i < 2; ++i)
                result[i] = new double[numCols];

            for (int c=0; c< numCols; ++c)
            {
                //means of all cols
                double sum = 0.0;
                for (int r = 0; r < numRows; ++r)
                    sum += rawData[r][c];
                double mean = sum / numRows;
                result[0][c] = mean; //save
                //stdDevs of all cols
                double sumSquares = 0.0;
                for (int r = 0; r < numRows; ++r)
                    sumSquares += (rawData[r][c] - mean) * (rawData[r][c] - mean);
                double stdDev = Math.Sqrt(sumSquares / numRows);
                result[1][c] = stdDev;
            }
            //normalize
            for (int c=0; c<columns.Length; c++)
            {
                int j = columns[c];
                double mean = result[0][j];
                double stdDev = result[1][j];
                for (int i = 0; i < numRows; ++i)
                    rawData[i][j] = (rawData[i][j] - mean) / stdDev;
            }
            return result;
        }

        private void allDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double trainAccuracy = lc.Accuracy(initValues, bestWeights);
            log("Prediction accuracy on init data = " + trainAccuracy.ToString("F2"));
        }

        private void singleValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
    
}
