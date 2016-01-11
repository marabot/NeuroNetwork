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
using Microsoft.Practices.Unity;


namespace NeuroNetwork
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
       
        ReadNumber readNumber;
        
        double[] inputs;


        private NeuroNetViewmodel _vm;
      

        [Dependency]
        public NeuroNetViewmodel vm
        {
            set
            {
                _vm = value;
               this.DataContext = _vm;
            }
        }
       
        public MainWindow()
        {
            InitializeComponent();

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Color CreatingColor = Color.FromRgb(0, 150, 150);
            createButton.Background = new SolidColorBrush(CreatingColor);

            Console.WriteLine("test");
                    
            //irisFlowerSet = new IrisFlower();


            WeightBiasBox.ItemsSource = _vm.neuroNet.getWeightsList();

            Color CreatedColor = Color.FromRgb(0, 200, 0);
            createButton.Background = new SolidColorBrush(CreatedColor);
        }

        private void compute_Click(object sender, RoutedEventArgs e)
        {

            /*
            inputs = new double[4];
            inputs[0] = Convert.ToDouble(input1.Text);
            inputs[1] = Convert.ToDouble(input2.Text);
            inputs[2] = Convert.ToDouble(input3.Text);
            inputs[3] = Convert.ToDouble(input4.Text);

            irisFlowerSet.NormalizeOneInput(inputs);
            */
            inputs=readNumber.GetinputsOneTest();
            normalizedInputsBox.ItemsSource = inputs;
            List<double> listOuputResult = _vm.neuroNet.Resolve(inputs)[1].ToList();
            OutputBox.ItemsSource = listOuputResult;
        }

        private void train_Click(object sender, RoutedEventArgs e)
        {
            Color trainingColor = Color.FromRgb(0, 150, 150);
            train.Background = new SolidColorBrush(trainingColor);
            _vm.neuroNet.Train(_vm.irisFlower);
            WeightBiasFinalBox.ItemsSource = _vm.neuroNet.getWeightsList();
            epochsBox.Text = _vm.neuroNet.GetLastEpochs().ToString();
            
            accuracyTrainBox.Text= _vm.neuroNet.Accuracy(_vm.irisFlower.trainDatas).ToString();
            accuracyTestBox.Text = _vm.neuroNet.Accuracy(_vm.irisFlower.testDatas).ToString();

            Color trainedColor = Color.FromRgb(0, 200, 0);

            train.Background = new SolidColorBrush(trainedColor);
        }

        private void ButtonMenuCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow createWin =new CreateWindow(_vm);
            createWin.Show();
        }
    }
}
