using NeuroNetWork;
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
using System.Windows.Shapes;

namespace NeuroNetwork
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {

        public NeuroNet neuroNet { get; set; }
        public IrisFlower irisFlower { get; set; }

        public NeuroNetViewmodel _vm;
        
        public CreateWindow(NeuroNetViewmodel vm)
        {
            InitializeComponent();
            List<string> listClasses = new List<string>();
            listClasses.Add("IrisFlower");
            listClasses.Add("readnumbers");
            listClassesData.ItemsSource = listClasses;
            this.CommandBindings.Add(new CommandBinding(CreateCommand.createNet, ExecuteCreateNet, CanExecuteCreateNet));

            neuroNet = vm.neuroNet;
            irisFlower = vm.irisFlower;
            _vm = vm;
        }

        

        private void ExecuteCreateNet(object sender, ExecutedRoutedEventArgs e)
        {
            
            _vm.neuroNet.SetNeuroNet(_vm.irisFlower.inputsCount, 10, _vm.irisFlower.outputsCount);
            Console.WriteLine("done!");
            this.Close();
        }

        private void CanExecuteCreateNet(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //this.CommandBindings.Add(CreateCommand);
        }
    }
}
