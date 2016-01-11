using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using NeuroNetwork;


namespace NeuroNetwork
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {

        private ReadNumber readNumber;
        private IrisFlower irisFlower;
        private NeuroNet neuroNet;

        protected override void OnStartup(StartupEventArgs e)
        {
           
           
            MessageBox.Show("!");
            base.OnStartup(e);

            //irisFlowerSet = new IrisFlower();
            //     readNumber = new ReadNumber();
            // neuroNet= new NeuroNet(Convert.ToInt16(readNumber.inputsCount), Convert.ToInt16(20), Convert.ToInt16(readNumber.outputsCount)); // TO DO : neuroneCount en dur, à changer dès que possible       
              
            
            IUnityContainer container = new UnityContainer();

            // NeuroNetViewmodel nnVM = new NeuroNetViewmodel();
            //  MainWindow window = new MainWindow();
            IrisFlower irisFlower = new IrisFlower();
            container.RegisterInstance<IrisFlower>(irisFlower);
            MainWindow window = container.Resolve<MainWindow>();
         
           
            Console.WriteLine("ninin");
            window.Show();

        }

        public void compute_Click()
        {
            double[]  inputs = readNumber.GetinputsOneTest();
            //normalizedInputsBox.ItemsSource = inputs;
            List<double> listOuputResult = neuroNet.Resolve(inputs)[1].ToList();
            // OutputBox.ItemsSource = listOuputResult;
        }

        
    }
}
