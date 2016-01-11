using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork
{
    public class DataFeeder
    {
        private static Random rnd;

        public double[][] AllData { get; protected set; }
        public double[][] trainDatas { get; protected set; }
        public double[][] testDatas { get; protected set; }

        public double[] mainNormalizeMean { get; protected set; }
        public double[] mainNormalizeSd { get; protected set; }

        public int inputsCount { get; protected set; }
        public int outputsCount { get; protected set; }

        public DataFeeder()
        {
            rnd = new Random(0);
        }

        public void Normalize(double[][] dataMatrix)
        {
            double[] cols = new double[inputsCount];
            for (int i = 0; i < inputsCount; ++i)
            {
                cols[i] = i;
            }            
            
            // normalize specified cols by computing (x - mean) / sd for each value
            foreach (int col in cols)
            {
                double sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += dataMatrix[i][col];
                double mean = sum / dataMatrix.Length;
                sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += (dataMatrix[i][col] - mean) * (dataMatrix[i][col] - mean);
                // thanks to Dr. W. Winfrey, Concord Univ., for catching bug in original code
                double sd = Math.Sqrt(sum / (dataMatrix.Length - 1));
                for (int i = 0; i < dataMatrix.Length; ++i)
                    dataMatrix[i][col] = (dataMatrix[i][col] - mean) / sd;

                mainNormalizeMean[col] = mean;
                mainNormalizeSd[col] = sd;

            }
        }

        public void NormalizeOneInput(double[] inputs)
        {
            double[] cols = new double[] { 0, 1, 2, 3 };

            // normalize specified cols by computing (x - mean) / sd for each value
            foreach (int col in cols)
            {
                inputs[col] = (inputs[col] - mainNormalizeMean[col]) / mainNormalizeSd[col];

            }
        }

        protected void MakeTrainTest(double[][] allData, double[][] trainData, double[][] testData)
        {
            // split allData into 80% trainData and 20% testData
            Random rnd = new Random(0);
            int totRows = allData.Length;
            int numCols = allData[0].Length;

            int trainRows = (int)(totRows * 0.80); // hard-coded 80-20 split
            int testRows = totRows - trainRows;

            trainData = new double[trainRows][];
            testData = new double[testRows][];

            int[] sequence = new int[totRows]; // create a random sequence of indexes
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }

            int si = 0; // index into sequence[]
            int j = 0; // index into trainData or testData

            for (; si < trainRows; ++si) // first rows to train data
            {
                trainData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], trainData[j], numCols);
                ++j;
            }

            j = 0; // reset to start of test data
            for (; si < totRows; ++si) // remainder to test data
            {
                testData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], testData[j], numCols);
                ++j;
            }
        } // MakeTrainTest        


        // split en 80% train et 20% test de façon aléatoire
        protected void MakeTest()
        {
            int trainCount = Convert.ToInt16(0.8 * AllData.Length);
            int testCount = AllData.Length - trainCount;

            trainDatas = new double[trainCount][];
            testDatas = new double[testCount][];

            List<int> indexList = new List<int>();
            for (int i = AllData.GetLength(0)-1; i > 0; i--)
            {
                indexList.Add(i);
            }

            /// pour chque ligne du alldatas
            for (int j = 0; j < AllData.Length; j++)
            {
                int randomIndex = rnd.Next(0, indexList.Count);

                /// soit aucun index atteint, soit un des deux, soit aucun
                if (trainCount > 0 && testCount > 0)
                {
                    int randomDataSet = rnd.Next(0, 1);
                    if (randomDataSet == 0)
                    {
                        trainDatas[trainCount - 1] = AllData[indexList.ElementAt(randomIndex)];
                        indexList.Remove(randomIndex);
                        trainCount--;
                    }
                    else
                    {
                        testDatas[testCount - 1] = AllData[indexList.ElementAt(randomIndex)];
                        indexList.Remove(randomIndex);
                        testCount--;
                    }
                }
                else if (trainCount > 0)
                {
                    trainDatas[trainCount - 1] = AllData[indexList.ElementAt(randomIndex)];
                    indexList.Remove(randomIndex);
                    trainCount--;
                }
                else
                {
                    testDatas[testCount - 1] = AllData[indexList.ElementAt(randomIndex)];
                    indexList.Remove(randomIndex);
                    testCount--;
                }
            }
        }

        
    }
}
