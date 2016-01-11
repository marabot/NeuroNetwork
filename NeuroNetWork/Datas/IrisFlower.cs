using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork
{
    public class IrisFlower : DataFeeder
    {
        private static Random rnd;
       
        public IrisFlower()
        {
           
            AllData = new double[150][];
            inputsCount = 4;
            outputsCount = 3;

            mainNormalizeMean = new double[4];
            mainNormalizeSd = new double[4];

              AllData[0] = new double[] { 5.1, 3.5, 1.4, 0.2, 0, 0, 1 }; // sepal length, width, petal length, width
              AllData[1] = new double[] { 4.9, 3.0, 1.4, 0.2, 0, 0, 1 }; // Iris setosa = 0 0 1
              AllData[2] = new double[] { 4.7, 3.2, 1.3, 0.2, 0, 0, 1 }; // Iris versicolor = 0 1 0
              AllData[3] = new double[] { 4.6, 3.1, 1.5, 0.2, 0, 0, 1 }; // Iris virginica = 1 0 0
              AllData[4] = new double[] { 5.0, 3.6, 1.4, 0.2, 0, 0, 1 };
              AllData[5] = new double[] { 5.4, 3.9, 1.7, 0.4, 0, 0, 1 };
              AllData[6] = new double[] { 4.6, 3.4, 1.4, 0.3, 0, 0, 1 };
              AllData[7] = new double[] { 5.0, 3.4, 1.5, 0.2, 0, 0, 1 };
              AllData[8] = new double[] { 4.4, 2.9, 1.4, 0.2, 0, 0, 1 };
              AllData[9] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };

              AllData[10] = new double[] { 5.4, 3.7, 1.5, 0.2, 0, 0, 1 };
              AllData[11] = new double[] { 4.8, 3.4, 1.6, 0.2, 0, 0, 1 };
              AllData[12] = new double[] { 4.8, 3.0, 1.4, 0.1, 0, 0, 1 };
              AllData[13] = new double[] { 4.3, 3.0, 1.1, 0.1, 0, 0, 1 };
              AllData[14] = new double[] { 5.8, 4.0, 1.2, 0.2, 0, 0, 1 };
              AllData[15] = new double[] { 5.7, 4.4, 1.5, 0.4, 0, 0, 1 };
              AllData[16] = new double[] { 5.4, 3.9, 1.3, 0.4, 0, 0, 1 };
              AllData[17] = new double[] { 5.1, 3.5, 1.4, 0.3, 0, 0, 1 };
              AllData[18] = new double[] { 5.7, 3.8, 1.7, 0.3, 0, 0, 1 };
              AllData[19] = new double[] { 5.1, 3.8, 1.5, 0.3, 0, 0, 1 };

              AllData[20] = new double[] { 5.4, 3.4, 1.7, 0.2, 0, 0, 1 };
              AllData[21] = new double[] { 5.1, 3.7, 1.5, 0.4, 0, 0, 1 };
              AllData[22] = new double[] { 4.6, 3.6, 1.0, 0.2, 0, 0, 1 };
              AllData[23] = new double[] { 5.1, 3.3, 1.7, 0.5, 0, 0, 1 };
              AllData[24] = new double[] { 4.8, 3.4, 1.9, 0.2, 0, 0, 1 };
              AllData[25] = new double[] { 5.0, 3.0, 1.6, 0.2, 0, 0, 1 };
              AllData[26] = new double[] { 5.0, 3.4, 1.6, 0.4, 0, 0, 1 };
              AllData[27] = new double[] { 5.2, 3.5, 1.5, 0.2, 0, 0, 1 };
              AllData[28] = new double[] { 5.2, 3.4, 1.4, 0.2, 0, 0, 1 };
              AllData[29] = new double[] { 4.7, 3.2, 1.6, 0.2, 0, 0, 1 };

              AllData[30] = new double[] { 4.8, 3.1, 1.6, 0.2, 0, 0, 1 };
              AllData[31] = new double[] { 5.4, 3.4, 1.5, 0.4, 0, 0, 1 };
              AllData[32] = new double[] { 5.2, 4.1, 1.5, 0.1, 0, 0, 1 };
              AllData[33] = new double[] { 5.5, 4.2, 1.4, 0.2, 0, 0, 1 };
              AllData[34] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
              AllData[35] = new double[] { 5.0, 3.2, 1.2, 0.2, 0, 0, 1 };
              AllData[36] = new double[] { 5.5, 3.5, 1.3, 0.2, 0, 0, 1 };
              AllData[37] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
              AllData[38] = new double[] { 4.4, 3.0, 1.3, 0.2, 0, 0, 1 };
              AllData[39] = new double[] { 5.1, 3.4, 1.5, 0.2, 0, 0, 1 };

              AllData[40] = new double[] { 5.0, 3.5, 1.3, 0.3, 0, 0, 1 };
              AllData[41] = new double[] { 4.5, 2.3, 1.3, 0.3, 0, 0, 1 };
              AllData[42] = new double[] { 4.4, 3.2, 1.3, 0.2, 0, 0, 1 };
              AllData[43] = new double[] { 5.0, 3.5, 1.6, 0.6, 0, 0, 1 };
              AllData[44] = new double[] { 5.1, 3.8, 1.9, 0.4, 0, 0, 1 };
              AllData[45] = new double[] { 4.8, 3.0, 1.4, 0.3, 0, 0, 1 };
              AllData[46] = new double[] { 5.1, 3.8, 1.6, 0.2, 0, 0, 1 };
              AllData[47] = new double[] { 4.6, 3.2, 1.4, 0.2, 0, 0, 1 };
              AllData[48] = new double[] { 5.3, 3.7, 1.5, 0.2, 0, 0, 1 };
              AllData[49] = new double[] { 5.0, 3.3, 1.4, 0.2, 0, 0, 1 };

              AllData[50] = new double[] { 7.0, 3.2, 4.7, 1.4, 0, 1, 0 };
              AllData[51] = new double[] { 6.4, 3.2, 4.5, 1.5, 0, 1, 0 };
              AllData[52] = new double[] { 6.9, 3.1, 4.9, 1.5, 0, 1, 0 };
              AllData[53] = new double[] { 5.5, 2.3, 4.0, 1.3, 0, 1, 0 };
              AllData[54] = new double[] { 6.5, 2.8, 4.6, 1.5, 0, 1, 0 };
              AllData[55] = new double[] { 5.7, 2.8, 4.5, 1.3, 0, 1, 0 };
              AllData[56] = new double[] { 6.3, 3.3, 4.7, 1.6, 0, 1, 0 };
              AllData[57] = new double[] { 4.9, 2.4, 3.3, 1.0, 0, 1, 0 };
              AllData[58] = new double[] { 6.6, 2.9, 4.6, 1.3, 0, 1, 0 };
              AllData[59] = new double[] { 5.2, 2.7, 3.9, 1.4, 0, 1, 0 };

              AllData[60] = new double[] { 5.0, 2.0, 3.5, 1.0, 0, 1, 0 };
              AllData[61] = new double[] { 5.9, 3.0, 4.2, 1.5, 0, 1, 0 };
              AllData[62] = new double[] { 6.0, 2.2, 4.0, 1.0, 0, 1, 0 };
              AllData[63] = new double[] { 6.1, 2.9, 4.7, 1.4, 0, 1, 0 };
              AllData[64] = new double[] { 5.6, 2.9, 3.6, 1.3, 0, 1, 0 };
              AllData[65] = new double[] { 6.7, 3.1, 4.4, 1.4, 0, 1, 0 };
              AllData[66] = new double[] { 5.6, 3.0, 4.5, 1.5, 0, 1, 0 };
              AllData[67] = new double[] { 5.8, 2.7, 4.1, 1.0, 0, 1, 0 };
              AllData[68] = new double[] { 6.2, 2.2, 4.5, 1.5, 0, 1, 0 };
              AllData[69] = new double[] { 5.6, 2.5, 3.9, 1.1, 0, 1, 0 };

              AllData[70] = new double[] { 5.9, 3.2, 4.8, 1.8, 0, 1, 0 };
              AllData[71] = new double[] { 6.1, 2.8, 4.0, 1.3, 0, 1, 0 };
              AllData[72] = new double[] { 6.3, 2.5, 4.9, 1.5, 0, 1, 0 };
              AllData[73] = new double[] { 6.1, 2.8, 4.7, 1.2, 0, 1, 0 };
              AllData[74] = new double[] { 6.4, 2.9, 4.3, 1.3, 0, 1, 0 };
              AllData[75] = new double[] { 6.6, 3.0, 4.4, 1.4, 0, 1, 0 };
              AllData[76] = new double[] { 6.8, 2.8, 4.8, 1.4, 0, 1, 0 };
              AllData[77] = new double[] { 6.7, 3.0, 5.0, 1.7, 0, 1, 0 };
              AllData[78] = new double[] { 6.0, 2.9, 4.5, 1.5, 0, 1, 0 };
              AllData[79] = new double[] { 5.7, 2.6, 3.5, 1.0, 0, 1, 0 };

              AllData[80] = new double[] { 5.5, 2.4, 3.8, 1.1, 0, 1, 0 };
              AllData[81] = new double[] { 5.5, 2.4, 3.7, 1.0, 0, 1, 0 };
              AllData[82] = new double[] { 5.8, 2.7, 3.9, 1.2, 0, 1, 0 };
              AllData[83] = new double[] { 6.0, 2.7, 5.1, 1.6, 0, 1, 0 };
              AllData[84] = new double[] { 5.4, 3.0, 4.5, 1.5, 0, 1, 0 };
              AllData[85] = new double[] { 6.0, 3.4, 4.5, 1.6, 0, 1, 0 };
              AllData[86] = new double[] { 6.7, 3.1, 4.7, 1.5, 0, 1, 0 };
              AllData[87] = new double[] { 6.3, 2.3, 4.4, 1.3, 0, 1, 0 };
              AllData[88] = new double[] { 5.6, 3.0, 4.1, 1.3, 0, 1, 0 };
              AllData[89] = new double[] { 5.5, 2.5, 4.0, 1.3, 0, 1, 0 };

              AllData[90] = new double[] { 5.5, 2.6, 4.4, 1.2, 0, 1, 0 };
              AllData[91] = new double[] { 6.1, 3.0, 4.6, 1.4, 0, 1, 0 };
              AllData[92] = new double[] { 5.8, 2.6, 4.0, 1.2, 0, 1, 0 };
              AllData[93] = new double[] { 5.0, 2.3, 3.3, 1.0, 0, 1, 0 };
              AllData[94] = new double[] { 5.6, 2.7, 4.2, 1.3, 0, 1, 0 };
              AllData[95] = new double[] { 5.7, 3.0, 4.2, 1.2, 0, 1, 0 };
              AllData[96] = new double[] { 5.7, 2.9, 4.2, 1.3, 0, 1, 0 };
              AllData[97] = new double[] { 6.2, 2.9, 4.3, 1.3, 0, 1, 0 };
              AllData[98] = new double[] { 5.1, 2.5, 3.0, 1.1, 0, 1, 0 };
              AllData[99] = new double[] { 5.7, 2.8, 4.1, 1.3, 0, 1, 0 };

              AllData[100] = new double[] { 6.3, 3.3, 6.0, 2.5, 1, 0, 0 };
              AllData[101] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
              AllData[102] = new double[] { 7.1, 3.0, 5.9, 2.1, 1, 0, 0 };
              AllData[103] = new double[] { 6.3, 2.9, 5.6, 1.8, 1, 0, 0 };
              AllData[104] = new double[] { 6.5, 3.0, 5.8, 2.2, 1, 0, 0 };
              AllData[105] = new double[] { 7.6, 3.0, 6.6, 2.1, 1, 0, 0 };
              AllData[106] = new double[] { 4.9, 2.5, 4.5, 1.7, 1, 0, 0 };
              AllData[107] = new double[] { 7.3, 2.9, 6.3, 1.8, 1, 0, 0 };
              AllData[108] = new double[] { 6.7, 2.5, 5.8, 1.8, 1, 0, 0 };
              AllData[109] = new double[] { 7.2, 3.6, 6.1, 2.5, 1, 0, 0 };

              AllData[110] = new double[] { 6.5, 3.2, 5.1, 2.0, 1, 0, 0 };
              AllData[111] = new double[] { 6.4, 2.7, 5.3, 1.9, 1, 0, 0 };
              AllData[112] = new double[] { 6.8, 3.0, 5.5, 2.1, 1, 0, 0 };
              AllData[113] = new double[] { 5.7, 2.5, 5.0, 2.0, 1, 0, 0 };
              AllData[114] = new double[] { 5.8, 2.8, 5.1, 2.4, 1, 0, 0 };
              AllData[115] = new double[] { 6.4, 3.2, 5.3, 2.3, 1, 0, 0 };
              AllData[116] = new double[] { 6.5, 3.0, 5.5, 1.8, 1, 0, 0 };
              AllData[117] = new double[] { 7.7, 3.8, 6.7, 2.2, 1, 0, 0 };
              AllData[118] = new double[] { 7.7, 2.6, 6.9, 2.3, 1, 0, 0 };
              AllData[119] = new double[] { 6.0, 2.2, 5.0, 1.5, 1, 0, 0 };

              AllData[120] = new double[] { 6.9, 3.2, 5.7, 2.3, 1, 0, 0 };
              AllData[121] = new double[] { 5.6, 2.8, 4.9, 2.0, 1, 0, 0 };
              AllData[122] = new double[] { 7.7, 2.8, 6.7, 2.0, 1, 0, 0 };
              AllData[123] = new double[] { 6.3, 2.7, 4.9, 1.8, 1, 0, 0 };
              AllData[124] = new double[] { 6.7, 3.3, 5.7, 2.1, 1, 0, 0 };
              AllData[125] = new double[] { 7.2, 3.2, 6.0, 1.8, 1, 0, 0 };
              AllData[126] = new double[] { 6.2, 2.8, 4.8, 1.8, 1, 0, 0 };
              AllData[127] = new double[] { 6.1, 3.0, 4.9, 1.8, 1, 0, 0 };
              AllData[128] = new double[] { 6.4, 2.8, 5.6, 2.1, 1, 0, 0 };
              AllData[129] = new double[] { 7.2, 3.0, 5.8, 1.6, 1, 0, 0 };

              AllData[130] = new double[] { 7.4, 2.8, 6.1, 1.9, 1, 0, 0 };
              AllData[131] = new double[] { 7.9, 3.8, 6.4, 2.0, 1, 0, 0 };
              AllData[132] = new double[] { 6.4, 2.8, 5.6, 2.2, 1, 0, 0 };
              AllData[133] = new double[] { 6.3, 2.8, 5.1, 1.5, 1, 0, 0 };
              AllData[134] = new double[] { 6.1, 2.6, 5.6, 1.4, 1, 0, 0 };
              AllData[135] = new double[] { 7.7, 3.0, 6.1, 2.3, 1, 0, 0 };
              AllData[136] = new double[] { 6.3, 3.4, 5.6, 2.4, 1, 0, 0 };
              AllData[137] = new double[] { 6.4, 3.1, 5.5, 1.8, 1, 0, 0 };
              AllData[138] = new double[] { 6.0, 3.0, 4.8, 1.8, 1, 0, 0 };
              AllData[139] = new double[] { 6.9, 3.1, 5.4, 2.1, 1, 0, 0 };

              AllData[140] = new double[] { 6.7, 3.1, 5.6, 2.4, 1, 0, 0 };
              AllData[141] = new double[] { 6.9, 3.1, 5.1, 2.3, 1, 0, 0 };
              AllData[142] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
              AllData[143] = new double[] { 6.8, 3.2, 5.9, 2.3, 1, 0, 0 };
              AllData[144] = new double[] { 6.7, 3.3, 5.7, 2.5, 1, 0, 0 };
              AllData[145] = new double[] { 6.7, 3.0, 5.2, 2.3, 1, 0, 0 };
              AllData[146] = new double[] { 6.3, 2.5, 5.0, 1.9, 1, 0, 0 };
              AllData[147] = new double[] { 6.5, 3.0, 5.2, 2.0, 1, 0, 0 };
              AllData[148] = new double[] { 6.2, 3.4, 5.4, 2.3, 1, 0, 0 };
              AllData[149] = new double[] { 5.9, 3.0, 5.1, 1.8, 1, 0, 0 };

            Normalize(AllData);
         
            MakeTest();     
        }   
    }
}
