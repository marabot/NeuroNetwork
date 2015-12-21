using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork.engine.entities
{
    class Layer
    {
        private List<Neurone> listNeurone;
        private List<double> listResult;

        public Layer(int neuroneCount)
        {
            listNeurone = new List<Neurone>();
            for (int i = 0; i < neuroneCount; i++)
            {
                listNeurone.Add(new Neurone());
            }
        }

        public List<double> Resolve(Layer fromLayer)
        {
            return new List<double>();
        }

        public List<double> Resolve(List<double> Datas)
        {
            return new List<double>();
        }

    }
}
