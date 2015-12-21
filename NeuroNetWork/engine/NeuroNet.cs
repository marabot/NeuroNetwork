using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroNetwork.engine.entities;


namespace NeuroNetwork.engine
{
    class NeuroNet
    {
        private static Random rnd;

        private double[,] arrayIhWeight;
        private double[,] arrayHoWeight;
        private double[] arrayBiasHidden;
        private double[] arrayBiasOutput;

        private List<double> listInput;
        private List<double>[] listResult;



        // back-prop momentum specific arrays (could be local to method Train)
        private double[,] ihPrevWeightsDelta;  // for momentum with back-propagation
        private double[] hPrevBiasesDelta;
        private double[,] hoPrevWeightsDelta;
        private double[] oPrevBiasesDelta;

        private double biasStart;

        ////////////////////////////

        public NeuroNet(int inputscount, int hiddenCount, int outputCount) {

            rnd = new Random(0);
            biasStart = 2;
            arrayIhWeight = new double[inputscount, hiddenCount];
            arrayHoWeight = new double[hiddenCount, outputCount];
            arrayBiasHidden = new double[arrayIhWeight.GetLength(1)];
            arrayBiasOutput = new double[arrayHoWeight.GetLength(1)];

            ihPrevWeightsDelta = new double[inputscount, hiddenCount];
            hoPrevWeightsDelta = new double[hiddenCount, outputCount];

            hPrevBiasesDelta = new double[arrayIhWeight.GetLength(1)];
            oPrevBiasesDelta = new double[arrayHoWeight.GetLength(1)];
                    

            /////////// Initialisation des poids et des biais /////////////////////////////
            double lo = -0.01;
            double hi = 0.01;
            for (int i = 0; i < inputscount; i++)
            {
             
                for (int j = 0; j < arrayIhWeight.GetLength(1); j++)
                {
                    arrayIhWeight[i, j] = (hi - lo) * rnd.NextDouble() + lo;
                }
            }

            for (int i = 0; i < arrayIhWeight.GetLength(1); i++)
            {
                arrayBiasHidden[i] = biasStart;
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    if (i == 0) arrayBiasOutput[j] = biasStart;
                    arrayHoWeight[i, j] = (hi - lo) * rnd.NextDouble() + lo;
                }
            }           
        }

        public List<double>[] Resolve(double[] inputsList)
        {
            listResult = new List<double>[2];
           
            for (int i = 0; i < 2; i++)
            {
                listResult[i] = new List<double>();
            }
            listInput = inputsList.ToList();           

            // couche hidden
            for (int i = 0; i < arrayIhWeight.GetLength(1); i++)
            {
                double weightInputSum = arrayBiasHidden[i];
                int inputIndex = 0;
                for(int j=0;j<listInput.Count;j++)
                {
                    weightInputSum = weightInputSum + arrayIhWeight[inputIndex, i] * listInput.ElementAt<double>(inputIndex);
                    inputIndex++;
                }
                listResult[0].Add(HyperbolicTan(weightInputSum)); // remplit la liste des résultats avec les valeurs après activation des neurones hidden
            }

            // couche Output
            List<double> listTemp = new List<double>();
            for (int i = 0; i < arrayHoWeight.GetLength(1); i++)
            {
                double weightInputSum = arrayBiasOutput[i];


                int inputIndex = 0;
                for (int j = 0; j < arrayHoWeight.GetLength(0); j++)
                {
                    weightInputSum = weightInputSum + arrayHoWeight[inputIndex, i] * listResult[0].ElementAt<double>(inputIndex);
                    inputIndex++;

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


        public List<double> GetListInput()
        {
            return listInput;
        }

        public List<double>[] GetListResult()
        {
            return listResult;
        }

        public double[,] GetArrayIhWeight()
        {
            return arrayIhWeight;
        }

        public double[,] GetArrayHoWeight()
        {
            return arrayHoWeight;
        }


        private double HyperbolicTan(double x)
        {
            return Math.Tanh(x);
        }


        private double[] Softmax(double[] oSums)
        {
            // determine max output sum
            // does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                if (oSums[i] > max) max = oSums[i];

            // determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;

            return result; // now scaled so that xi sum to 1.0
        }

        private void UpdateWeights(double[] tValues, double learnRate, double momentum, double weightDecay)
        {

            // back-prop specific arrays (these could be local to method UpdateWeights)
            double[] oGrads = new double[arrayHoWeight.GetLength(1)]; // output gradients for back-propagation
            double[] hGrads = new double[arrayHoWeight.GetLength(0)]; // hidden gradients for back-propagation

            // compute ouput grad
            for (int i = 0; i < arrayHoWeight.GetLength(1); i++)
            {
                double derivative = (1 - listResult[1].ElementAt(i)) * listResult[1].ElementAt(i);
                oGrads[i] = derivative * (tValues[i] * listResult[1].ElementAt(i));
            }

            //Compute Hidden grad
            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                double derivative = (1 - listResult[0].ElementAt(i)) * (1 + listResult[0].ElementAt(i));
                double somme = 0;
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    somme = somme + oGrads[j] * arrayHoWeight[i, j];
                }
                hGrads[i] = derivative + somme;
            }

            // Update Input-hidden weights
            for (int i = 0; i < arrayIhWeight.GetLength(0); i++)

                for (int j = 0; j < arrayIhWeight.GetLength(1); j++)
                {
                    double delta = learnRate * hGrads[j] *listInput[i];
                    arrayIhWeight[i, j] += delta;
                    arrayIhWeight[i, j] += momentum * ihPrevWeightsDelta[i, j];
                    arrayIhWeight[i, j] -= (weightDecay * arrayIhWeight[i, j]);
                    ihPrevWeightsDelta[i, j] = delta;
                }

            // Update Hidden Bias
            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                double delta = learnRate * hGrads[i];
                arrayBiasHidden[i] += delta;
                arrayBiasHidden[i] += momentum * hPrevBiasesDelta[i];
                arrayBiasHidden[i] -= (weightDecay * arrayBiasHidden[i]);
                hPrevBiasesDelta[i] = delta;

            }


            // Update hidden-output weights
            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)

                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    double delta = learnRate * oGrads[j] * listResult[0].ElementAt(i);
                    arrayHoWeight[i, j] += delta;
                    arrayHoWeight[i, j] += momentum * hoPrevWeightsDelta[i, j];
                    arrayHoWeight[i, j] -= (weightDecay * arrayHoWeight[i, j]);
                    hoPrevWeightsDelta[i, j] = delta;
                }


            // Update output Bias
            for (int i = 0; i < arrayHoWeight.GetLength(1); i++)
            {
                double delta = learnRate * oGrads[i];
                arrayBiasOutput[i] += delta;
                arrayBiasOutput[i] += momentum * oPrevBiasesDelta[i];
                arrayBiasOutput[i] -= (weightDecay * arrayBiasOutput[i]);
                oPrevBiasesDelta[i] = delta;

            }
        }

        public void Train(double[][] trainData, int maxEpochs, double learnRate, double momentum, double weightDecay)
        {
            int epoch = 0;
            double[] inputValues = new double[arrayIhWeight.GetLength(0)];
            double[] testvalues = new double[arrayHoWeight.GetLength(1)];

            int[] sequence = new int[trainData.GetLength(0)];
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            while (epoch < maxEpochs)
            {
                double mse = MeanSquaredError(trainData);
                if (mse < 0.020) break; // consider passing value in as parameter
                                        //if (mse < 0.001) break; // consider passing value in as parameter

                Shuffle(sequence); // visit each training data in random order
                for (int i = 0; i < trainData.Length; ++i)
                {
                    int idx = sequence[i];
                    Array.Copy(trainData[idx], inputValues, arrayIhWeight.GetLength(0));
                    Array.Copy(trainData[idx], arrayIhWeight.GetLength(0)-1, testvalues, 0, arrayHoWeight.GetLength(1));
                    Resolve(inputValues); // copy xValues in, compute outputs (store them internally)
                    UpdateWeights(testvalues, learnRate, momentum, weightDecay); // find better weights
                } // each training tuple
                ++epoch;
            }
        }

        public void MakeTest(double[][] allDatas, ref double[][] trainDatas, ref double[][] testDatas)
        {
            int trainCount = Convert.ToInt16(0.8 * allDatas.Length);
            int testCount = allDatas.Length - trainCount;

            trainDatas = new double[trainCount][];
            testDatas = new double[testCount][];

            List<int> indexList=new List<int>();
            for (int i = 149; i>= 0; i--)
            {
                indexList.Add(i);
            }

            /// pour chque ligne du alldatas
            for (int j = 0; j < allDatas.Length; j++)
            {
                int randomIndex = rnd.Next(0, indexList.Count);

                /// soit aucun index atteint, soit un des deux, soit aucun
                if (trainCount > 0 && testCount > 0)
                    {
                        int randomDataSet = rnd.Next(0, 1);
                    if (randomDataSet == 0)
                    {
                        trainDatas[trainCount-1] = allDatas[indexList.ElementAt(randomIndex)];
                        indexList.Remove(randomIndex);
                        trainCount--;
                    }
                    else
                    {
                        testDatas[testCount-1] = allDatas[indexList.ElementAt(randomIndex)];
                        indexList.Remove(randomIndex);
                        testCount--;
                    }
                }
                else if(trainCount>0)
                    {
                    trainDatas[trainCount-1] = allDatas[indexList.ElementAt(randomIndex)];
                    indexList.Remove(randomIndex);
                    trainCount--;
                }
                else 
                {
                    testDatas[testCount-1] = allDatas[indexList.ElementAt(randomIndex)];
                    indexList.Remove(randomIndex);
                    testCount--;
                }
            }
        }

        private static void Shuffle(int[] sequence)
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
                Array.Copy(trainData[i], arrayIhWeight.GetLength(0), tValues, 0, arrayHoWeight.GetLength(1)-1); // get target values
                double[] yValues = this.Resolve(xValues)[1].ToArray() ; // compute output using current weights
                for (int j = 0; j < arrayHoWeight.GetLength(1); ++j)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }

            return sumSquaredError / trainData.Length;
        }

    


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
                    strB.Append(arrayIhWeight[i, j].ToString().Substring(0,5)+"/"+ arrayBiasHidden[j].ToString().Substring(0, 5) + " ||  ");
                }
                
                ret.Add(strB.ToString());
            }

            for (int i = 0; i < arrayHoWeight.GetLength(0); i++)
            {
                strB = new StringBuilder();
                strB.Append("oh" + i + " : ");
                for (int j = 0; j < arrayHoWeight.GetLength(1); j++)
                {
                    strB.Append(arrayHoWeight[i, j].ToString().Substring(0, 4) + "/" + arrayBiasOutput[j].ToString().Substring(0, 4) + " ||  ");
                }

                ret.Add(strB.ToString());
            }
            return ret;
        }
    }
}
