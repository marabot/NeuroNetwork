using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NeuroNetwork.engine;

namespace NeuroNetWork
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        NeuroNet neuroNet;
        List<double>[] results;
        IrisFlower irisFlowerSet;

        double[] inputs;

        int maxEpochs = 2000;
        double learnRate = 0.05;
        double momentum = 0.01;
        double weightDecay = 0.0001;

        public MainWindow()
        {
            InitializeComponent();
            TextBox txtNumber = new TextBox();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Color CreatingColor = Color.FromRgb(0, 150, 150);
            createButton.Background = new SolidColorBrush(CreatingColor);

            neuroNet = new NeuroNet(Convert.ToInt16(InputCount.Text), Convert.ToInt16(NeuroneCount.Text), Convert.ToInt16(OutputCount.Text));

            irisFlowerSet = new IrisFlower();



            InputValuesList.ItemsSource = inputs;

            Color CreatedColor = Color.FromRgb(0, 200, 0);
            createButton.Background = new SolidColorBrush(CreatedColor);
        }

        private void compute_Click(object sender, RoutedEventArgs e)
        {
            inputs = new double[4];
            inputs[0] = Convert.ToDouble(input1.Text);
            inputs[1] = Convert.ToDouble(input2.Text);
            inputs[2] = Convert.ToDouble(input3.Text);
            inputs[3] = Convert.ToDouble(input4.Text);

            Normalize(inputs);
            ResultGrid.ItemsSource = inputs;
            List<double> listOuputResult = neuroNet.Resolve(inputs)[1].ToList();
            OutputList.ItemsSource = listOuputResult;
        }

        private void train_Click(object sender, RoutedEventArgs e)
        {

            Color trainingColor = Color.FromRgb(0, 150, 150);
            train.Background = new SolidColorBrush(trainingColor);

            neuroNet.MakeTest(irisFlowerSet.normalizedAllDatas, ref irisFlowerSet.trainDatas, ref irisFlowerSet.testDatas);
            neuroNet.Train(irisFlowerSet.allData, maxEpochs, learnRate, momentum, weightDecay);

            WeightList.ItemsSource = neuroNet.getWeightsList();


            Color trainedColor = Color.FromRgb(0, 200, 0);

            train.Background = new SolidColorBrush(trainedColor);
        }

        private void Normalize(double[] dataMatrix)
        {
            // normalize specified cols by computing (x - mean) / sd for each value

            double sum = 0.0;
            for (int i = 0; i < dataMatrix.Length; ++i)
                sum += dataMatrix[i];
            double mean = sum / dataMatrix.Length;
            sum = 0.0;
            for (int i = 0; i < dataMatrix.Length; ++i)
                sum += (dataMatrix[i] - mean) * (dataMatrix[i] - mean);
            // thanks to Dr. W. Winfrey, Concord Univ., for catching bug in original code
            double sd = Math.Sqrt(sum / (dataMatrix.Length - 1));
            for (int i = 0; i < dataMatrix.Length; ++i)
                dataMatrix[i] = (dataMatrix[i] - mean) / sd;

        }
        private double[] NormalizeInputs(double[] inputs, IrisFlower datas)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = -1 + ((inputs[i] - datas.normBounds[i, 0]) / datas.normBounds[i, 2]);
            }

            return inputs;
        }
    }
}
