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

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            TextBox txtNumber = new TextBox();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Color CreatingColor = Color.FromRgb(0, 150, 150);
            createButton.Background = new SolidColorBrush(CreatingColor);

            neuroNet = new NeuroNet(Convert.ToInt16(InputCount.Text), Convert.ToInt16(NeuroneCount.Text), Convert.ToInt16(OutputCount.Text));

            irisFlowerSet = new IrisFlower();
            WeightBiasBox.ItemsSource = neuroNet.getWeightsList();

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

            irisFlowerSet.NormalizeOneInput(inputs);
            normalizedInputsBox.ItemsSource = inputs;
            List<double> listOuputResult = neuroNet.Resolve(inputs)[1].ToList();
            OutputBox.ItemsSource = listOuputResult;
        }

        private void train_Click(object sender, RoutedEventArgs e)
        {
            Color trainingColor = Color.FromRgb(0, 150, 150);
            train.Background = new SolidColorBrush(trainingColor);
         
            neuroNet.Train(irisFlowerSet.trainDatas, maxEpochs, learnRate, momentum, weightDecay);

            WeightBiasFinalBox.ItemsSource = neuroNet.getWeightsList();
            epochsBox.Text = neuroNet.GetLastEpochs().ToString();
            
            accuracyTrainBox.Text= neuroNet.Accuracy(irisFlowerSet.trainDatas).ToString();
            accuracyTestBox.Text = neuroNet.Accuracy(irisFlowerSet.testDatas).ToString();

            Color trainedColor = Color.FromRgb(0, 200, 0);

            train.Background = new SolidColorBrush(trainedColor);
        }

       
    }
}
