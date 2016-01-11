using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace NeuroNetwork
{
    public class NeuroNetViewmodel : DependencyObject
    {

        public NeuroNet neuroNet { get; set; }
        public IrisFlower irisFlower { get; set; }

        public NeuroNetViewmodel()
        {
            neuroNet = new NeuroNet();
            irisFlower = new IrisFlower();
           
        }

        //neuroNet.SetNeuroNet(irisFlower.inputsCount,,irisFlower.outputsCount);
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        

        public List<Double> results
        {
            get { return (List<Double>)GetValue(resultsProperty); }
            set { SetValue(resultsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for results.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty resultsProperty =
            DependencyProperty.Register("results", typeof(List<Double>), typeof(NeuroNetViewmodel), new PropertyMetadata());       


        public double[] inputs
        {
            get { return (double[])GetValue(inputsProperty); }
            set { SetValue(inputsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for inputs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty inputsProperty =
            DependencyProperty.Register("inputs", typeof(double[]), typeof(NeuroNetViewmodel), new PropertyMetadata());


        public int maxEpochs
        {
            get { return (int)GetValue(maxEpochsProperty); }
            set { SetValue(maxEpochsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty maxEpochsProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(NeuroNetViewmodel), new PropertyMetadata(0));


        public double learnRate
        {
            get { return (double)GetValue(learnRateProperty); }
            set { SetValue(learnRateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for learnRate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty learnRateProperty =
            DependencyProperty.Register("learnRate", typeof(double), typeof(NeuroNetViewmodel), new PropertyMetadata(0.0));

        public double momentum
        {
            get { return (double)GetValue(momentumProperty); }
            set { SetValue(momentumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for momentum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty momentumProperty =
            DependencyProperty.Register("momentum", typeof(double), typeof(NeuroNetViewmodel), new PropertyMetadata(0.0));

        public double weightDecay
        {
            get { return (double)GetValue(weightDecayProperty); }
            set { SetValue(weightDecayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for weightDecay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty weightDecayProperty =
            DependencyProperty.Register("weightDecay", typeof(double), typeof(NeuroNetViewmodel), new PropertyMetadata(0.0));       
           
    }
}
