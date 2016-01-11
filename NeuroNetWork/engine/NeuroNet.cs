using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeuroNetwork
{
    public class NeuroNet: DependencyObject 
    {
        // private static Random rnd= new Random(0);


        int maxEpochs = 2000;
        double learnRate = 0.05;
        double momentum = 0.01;
        double weightDecay = 0.0001;

        private double[,] arrayIhWeight;
        private double[,] arrayHoWeight;
        private double[] arrayBiasHidden;
        private double[] arrayBiasOutput;

        private List<double> listInput;
        private List<double>[] lastListResult;
       
        private double[,] ihPrevWeightsDelta;  // for momentum with back-propagation
        private double[] hPrevBiasesDelta;
        private double[,] hoPrevWeightsDelta;
        private double[] oPrevBiasesDelta;

        // back-prop specific arrays (these could be local to method UpdateWeights)
        private double[] oGrads ; // output gradients for back-propagation
        private double[] hGrads ; // hidden gradients for back-propagation
       
        int lastTrainEpochs;

        private Random rnd;

        public NeuroNet()
        {

        }
        
        public void SetNeuroNet(int inputscount, int hiddenCount, int outputCount) {

            rnd = new Random(0);

            arrayIhWeight = new double[inputscount, hiddenCount];
            arrayHoWeight = new double[hiddenCount, outputCount];
            arrayBiasHidden = new double[arrayIhWeight.GetLength(1)];
            arrayBiasOutput = new double[arrayHoWeight.GetLength(1)];

            ihPrevWeightsDelta = new double[inputscount, hiddenCount];
            hoPrevWeightsDelta = new double[hiddenCount, outputCount];

            hPrevBiasesDelta = new double[arrayIhWeight.GetLength(1)];
            oPrevBiasesDelta = new double[arrayHoWeight.GetLength(1)];
            lastTrainEpochs = -1;

            oGrads = new double[arrayHoWeight.GetLength(1)];
            hGrads = new double[arrayHoWeight.GetLength(0)];
            
            /////////// Initialisation des poids et des biais /////////////////////////////
            double lo = -0.01;
            double hi = 0.01;
            for (int i = 0; i < arrayIhWeight.GetLength(0); i++)
            {
             
                for (int j = 0; j < arrayIhWeight.GetLength(1); j++)
                {
                    arrayIhWeight[i, j] = (hi - lo) * rnd.NextDouble() + lo;
                }
            }

            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                arrayBiasHidden[i] = (hi - lo) * rnd.NextDouble() + lo;
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                   arrayBiasOutput[j] = (hi - lo) * rnd.NextDouble() + lo;
                    arrayHoWeight[i, j] = (hi - lo) * rnd.NextDouble() + lo;
                }
            }                      
        }
        

        //////////////////////////////////
        /// Resolve Functions
        ///////////////////////////////////
        public List<double>[] Resolve(double[] inputsList)
        {
            List<double>[] listResult = new List<double>[2];  // en 0 les résultats de la couche hidden, en 1 les résultats de la couche output
           
            for (int i = 0; i < 2; i++)
            {
                listResult[i] = new List<double>();
            }
            listInput = inputsList.ToList();           

            // couche hidden
            for (int i = 0; i < arrayIhWeight.GetLength(1); ++i)
            {
                double weightInputSum = arrayBiasHidden[i];
               
                for(int j=0;j<listInput.Count;++j)
                {
                    weightInputSum +=  (arrayIhWeight[j, i] * listInput.ElementAt(j));                  
                }
                listResult[0].Add(HyperbolicTan(weightInputSum)); // remplit la liste des résultats avec les valeurs après activation des neurones hidden
            }

            // couche Output
            List<double> listTemp = new List<double>();
            for (int i = 0; i < arrayHoWeight.GetLength(1);++i)
            {
                double weightInputSum = arrayBiasOutput[i];
             
                for (int j = 0; j < arrayHoWeight.GetLength(0); ++j)
                {
                    weightInputSum +=  (arrayHoWeight[j, i] * listResult[0].ElementAt(j));
                   
                }
                listTemp.Add(weightInputSum); // remplit la liste des résultatsTemporaire avec les valeurs AVANT activation des neurones output               
            }

            
            double[] arrayWeightSum = new double[arrayHoWeight.GetLength(1)];
            int neuroneIndex = 0;
            foreach (double d in listTemp)
            {
                arrayWeightSum[neuroneIndex++] = d;
            }
            
            double[] resultTemp = Softmax(arrayWeightSum);
            for (int i = 0; i < resultTemp.Length; i++)
            {
                listResult[1].Add(resultTemp[i]);
            }
            
            return listResult;
        }

        private double HyperbolicTan(double x)
        {
            if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
            else if (x > 20.0) return 1.0;
            else return Math.Tanh(x);
        }

        private double[] Softmax(double[] oSums)
        {
           
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                if (oSums[i] > max) max = oSums[i];
          
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;

            return result; 
        }


        //////////////////////////////////
        /// Getters 
        //////////////////////////////////
        public List<double> GetListInput()
        {
            return listInput;
        }

        public List<double>[] GetLastListResult()
        {
            return lastListResult;
        }

        public double[,] GetArrayIhWeight()
        {
            return arrayIhWeight;
        }

        public double[,] GetArrayHoWeight()
        {
            return arrayHoWeight;
        }

        public int GetLastEpochs()
        {
            return lastTrainEpochs;
        }


        //////////////////////////////////
        /// Training functions
        //////////////////////////////////
        private void UpdateWeights(double[] tValues, double learnRate, double momentum, double weightDecay)
        {
         
            // compute ouput grad
            for (int i = 0; i < oGrads.Length; i++)
            {
                double derivative = (1 - lastListResult[1].ElementAt(i)) * lastListResult[1].ElementAt(i);
                oGrads[i] = derivative * (tValues[i] - lastListResult[1].ElementAt(i));
            }

            //Compute Hidden grad
            for (int i = 0; i < hGrads.Length; i++)
            {
                double derivative = (1 - lastListResult[0].ElementAt(i)) * (1 + lastListResult[0].ElementAt(i));
                double somme = 0.0;
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    somme = somme + oGrads[j] * arrayHoWeight[i, j];
                }
                hGrads[i] = derivative * somme;
            }

            // Update Input-hidden weights
            for (int i = 0; i < arrayIhWeight.GetLength(0); i++)
            {
                for (int j = 0; j < arrayIhWeight.GetLength(1); j++)
                {
                    double delta = learnRate * hGrads[j] * listInput[i];
                    arrayIhWeight[i, j] += delta;
                    arrayIhWeight[i, j] += momentum * ihPrevWeightsDelta[i, j];
                    arrayIhWeight[i, j] -= (weightDecay * arrayIhWeight[i, j]);
                    ihPrevWeightsDelta[i, j] = delta;
                }
            }

            // Update Hidden Bias
            for (int i = 0; i < arrayBiasHidden.Length; i++)
            {
                double delta = learnRate * hGrads[i]*1.0;
                arrayBiasHidden[i] += delta;
                arrayBiasHidden[i] += momentum * hPrevBiasesDelta[i];
                arrayBiasHidden[i] -= (weightDecay * arrayBiasHidden[i]);
                hPrevBiasesDelta[i] = delta;

            }


            // Update hidden-output weights
            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    double delta = learnRate * oGrads[j] * lastListResult[0].ElementAt(i);
                    arrayHoWeight[i, j] += delta;
                    arrayHoWeight[i, j] += momentum * hoPrevWeightsDelta[i, j];
                    arrayHoWeight[i, j] -= (weightDecay * arrayHoWeight[i, j]);
                    hoPrevWeightsDelta[i, j] = delta;
                }
            }

            // Update output Bias
            for (int i = 0; i < arrayBiasOutput.Length; i++)
            {
                double delta = learnRate * oGrads[i]*1.0;
                arrayBiasOutput[i] += delta;
                arrayBiasOutput[i] += momentum * oPrevBiasesDelta[i];
                arrayBiasOutput[i] -= (weightDecay * arrayBiasOutput[i]);
                oPrevBiasesDelta[i] = delta;
            }
        }

        //public void Train(double[][] trainData, int maxEpochs, double learnRate, double momentum, double weightDecay)

        public void Train(IrisFlower iris)
        {
            int epoch = 0;
            double[] inputValues = new double[arrayIhWeight.GetLength(0)];
            double[] testvalues = new double[arrayHoWeight.GetLength(1)];

            int[] sequence = new int[iris.trainDatas.GetLength(0)];
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            while (epoch < maxEpochs)
            {
                lastTrainEpochs = epoch;
                double mse = MeanSquaredError(iris.trainDatas);
                if (mse < 0.020) break; // consider passing value in as parameter
                                        //if (mse < 0.001) break; // consider passing value in as parameter

                Shuffle(sequence); // visit each training data in random order
                for (int i = 0; i < iris.trainDatas.Length; ++i)
                {
                    int idx = sequence[i];
                    Array.Copy(iris.trainDatas[idx], inputValues, arrayIhWeight.GetLength(0));
                    Array.Copy(iris.trainDatas[idx], arrayIhWeight.GetLength(0), testvalues, 0, arrayHoWeight.GetLength(1));
                    lastListResult=Resolve(inputValues); // copy xValues in, compute outputs (store them internally)
                    UpdateWeights(testvalues, learnRate, momentum, weightDecay); // find better weights
                } // each training tuple
                ++epoch;
            }           
        }

        private void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }

        private double MeanSquaredError(double[][] trainData) // used as a training stopping condition
        {
            // average squared error per training tuple
            double sumSquaredError = 0.0;
            double[] xValues = new double[arrayIhWeight.GetLength(0)]; // first numInput values in trainData
            double[] tValues = new double[arrayHoWeight.GetLength(1)]; // last numOutput values

            // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
            for (int i = 0; i < trainData.Length; ++i)
            {
                Array.Copy(trainData[i], xValues, arrayIhWeight.GetLength(0));
                Array.Copy(trainData[i], arrayIhWeight.GetLength(0), tValues, 0, arrayHoWeight.GetLength(1)); // get target values
                double[] yValues = this.Resolve(xValues)[1].ToArray() ; // compute output using current weights
                for (int j = 0; j < arrayHoWeight.GetLength(1); ++j)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }
           
            return sumSquaredError / trainData.Length;
        }


        //////////////////////////////////
        /// display functions
       //////////////////////////////////

        public List<string> getWeightsList()
        {
            List<string> ret = new List<string>();
            StringBuilder strB;

            for (int i = 0; i < arrayIhWeight.GetLength(0);i++)
            {
                strB = new StringBuilder();
                strB.Append("ih" + i + " : ");
                for (int j = 0; j < arrayIhWeight.GetLength(1);j++)
                {
                    strB.Append(arrayIhWeight[i, j].ToString().Substring(0,6)+"/"+ arrayBiasHidden[j].ToString().Substring(0, 6) + " ||  ");
                }
                
                ret.Add(strB.ToString());
            }

            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                strB = new StringBuilder();
                strB.Append("oh" + i + " : ");
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    strB.Append(arrayHoWeight[i, j].ToString().Substring(0, 6) + "/" + arrayBiasOutput[j].ToString().Substring(0, 6) + " ||  ");
                }

                ret.Add(strB.ToString());
            }
            return ret;
        }
        
        public double AccuracyOld(double[][] datas)
        {
            double[] inputs = new double[arrayIhWeight.GetLength(0)];
            double[] rightResult = new double[arrayHoWeight.GetLength(1)];
            int correctAnswers = 0;

            // pour chaque ligne des données
            for (int i = 0; i < datas.Length; ++i)
            {
                // résout la ligne
                Array.Copy(datas[i], inputs, arrayIhWeight.GetLength(0));
                Array.Copy(datas[i], arrayIhWeight.GetLength(0), rightResult, 0, arrayHoWeight.GetLength(1));
                List<double>[] listOutput = Resolve(inputs);

                // check si le résultat est bon
                int resultIndex = 0;
                double bestValue = listOutput[1].ElementAt(0);
                for (int j=0;j<listOutput[1].Count;j++)
                {
                    if (listOutput[1].ElementAt(j)>bestValue)
                    {
                        resultIndex = j;
                        bestValue = listOutput[1].ElementAt(j);
                    }
                }
                if (rightResult[resultIndex] == 1.0)
                {
                    correctAnswers++;
                }
            }
            return (correctAnswers*1.0) / datas.Length;

        }


        public double Accuracy(double[][] testData)
        {
            // percentage correct using winner-takes all
            int numCorrect = 0;
            int numWrong = 0;
            double[] xValues = new double[arrayIhWeight.GetLength(0)]; // inputs
            double[] tValues = new double[arrayHoWeight.GetLength(1)]; // targets
            double[] yValues; // computed Y

            for (int i = 0; i < testData.Length; ++i)
            {
                Array.Copy(testData[i], xValues, arrayIhWeight.GetLength(0)); // parse test data into x-values and t-values
                Array.Copy(testData[i], arrayIhWeight.GetLength(0), tValues, 0, arrayHoWeight.GetLength(1));
                
                List<double> inputList = new List<double>();
              
                yValues = Resolve(xValues)[1].ToArray<double>(); ;
                int maxIndex = MaxIndex(yValues); // which cell in yValues has largest value?

                if (tValues[maxIndex] == 1.0) // ugly. consider AreEqual(double x, double y)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numCorrect + numWrong); // ugly 2 - check for divide by zero
        }

        private static int MaxIndex(double[] vector) // helper for Accuracy()
        {
            // index of largest value
            int bigIndex = 0;
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i]; bigIndex = i;
                }
            }
            return bigIndex;
        }
    }

}
