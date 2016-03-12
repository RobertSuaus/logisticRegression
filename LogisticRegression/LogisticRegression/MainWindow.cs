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
        public MainWindow()
        {
            InitializeComponent();
        }

        //Cuando main corre ya todos los datos están inicializados
        public void main()
        {
            //Graficar los valores iniciales
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
            log("Displaying initial values graph...");
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
    }
    
}
