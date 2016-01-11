using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace NeuroNetwork
{
    public class ReadNumber : DataFeeder
    {
        private static Random rnd;

        List<double[]> allDatas;

        public ReadNumber()
        {

            allDatas = new List<double[]>();
            inputsCount = 100;
            outputsCount = 4;
            mainNormalizeMean = new double[100];
            mainNormalizeSd = new double[100];

            string currentDir = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(currentDir + "/numbersPics");

            //pour chaque fichier
            foreach (string f in files)
            {
                    Bitmap img = new Bitmap(f);

                    double[] oneData = new double[104];

                    StringBuilder dispVal = new StringBuilder();
                    for (int i = 0; i < img.Width; ++i )
                    {

                        for (int j = 0; j < img.Height; ++j)
                        {
                            Color pixelColor = img.GetPixel(i, j);
                            dispVal.Append(pixelColor.Name + "\n");
                            if (pixelColor.Name.Substring(2, 1) == "0")
                            {

                                oneData[i  * 10 + j ] = -1;
                            }
                            else {
                                oneData[i * 10 + j] = 1;
                            }
                        }
                    }

                if (Convert.ToInt32(Path.GetFileName(f).Substring(0, 1)) == 1)
                {
                    oneData[100] = 1;
                }
                else if (Convert.ToInt32(Path.GetFileName(f).Substring(0, 1)) == 2)
                {
                    oneData[101] = 1;
                }
                else if (Convert.ToInt32(Path.GetFileName(f).Substring(0, 1)) == 3)
                {
                    oneData[102] = 1;
                }
                else if (Convert.ToInt32(Path.GetFileName(f).Substring(0, 1)) == 4)
                {

                    oneData[103] = 1;
                }
                  
                Console.WriteLine(dispVal.ToString() + oneData.Last().ToString());
                allDatas.Add(oneData);                
            }

            AllData = new double[files.Length][];

            for (int i = 0; i < files.Length; ++i)
            {
                AllData[i] = allDatas.ElementAt(i);
            }


            StringBuilder str = new StringBuilder();

            foreach (double[] data in allDatas)
            {
                foreach (double v in data)
                {
                    str.Append(v);
                    if (!v.Equals(allDatas.Last())) str.Append(";");
                }
                str.Append("\n");
            }

            Normalize(AllData);

            MakeTest();

            Console.WriteLine(str.ToString());
            Console.ReadLine();
        }

        public double[] GetinputsOneTest()
        {
            string currentDir = Directory.GetCurrentDirectory();            
            Bitmap img = new Bitmap(currentDir + "/numbersPicsTest/test.bmp");

            double[] oneData = new double[100];

            StringBuilder dispVal = new StringBuilder();
            for (int i = 0; i < img.Width;++ i)
            {

                for (int j = 0; j < img.Height;++ j )
                {
                    Color pixelColor = img.GetPixel(i, j);
                    dispVal.Append(pixelColor.Name + "\n");
                    if (pixelColor.Name.Substring(2, 1) == "0")
                    {

                        oneData[i  * 10 + j ] = -1;
                    }
                    else {
                        oneData[i  * 10 + j ] = 1;
                    }
                }
            }

           
            return oneData;
        }

    }
}
