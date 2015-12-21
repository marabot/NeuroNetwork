using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetwork.engine.entities
{
    class Dentrite
    {
        private Neurone neuroneCible;
        private double poids;

        public Dentrite(Neurone neurone)
        {
            neuroneCible = neurone;
            poids = 0.5;
        }

    }
}
