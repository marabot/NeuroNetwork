using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork.engine.entities
{
    class Neurone
    {

        public static double bias = 2;  
        /*
        public static double Resolve(List<double> listInputs, double[,] listWeight, int PosInLayer)
        {
            double PonderateSum = bias;
            int weightIndex = 0;
            foreach (double n in listInputs)
            {
                PonderateSum = PonderateSum + n * listWeight[weightIndex,PosInLayer];
                weightIndex++;
            }

           return HyperbolicTan(PonderateSum);

        }

        public static double HyperbolicTan(double x)
        {
            return Math.Tanh(x);            
        }


        public static double[] Softmax(double[] oSums)
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
        */
    }
}
