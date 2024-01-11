using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowABillBecomesALaw
{
    public class Senator : Politician
    {
        public Senator(string name) : base("Senator " + name) { }

        public bool Filibuster(float?[] billViewpoints, float threshold)
        {
            float[] currentViewpoints = viewpoints;
            float[] oppositeViewpoints = new float[viewpoints.Length];
            for (int i = 0; i < viewpoints.Length; i++)
            {
                oppositeViewpoints[i] = 1 - viewpoints[i];
            }
            viewpoints = oppositeViewpoints;
            bool filibuster = Vote(billViewpoints, threshold, out _);
            viewpoints = currentViewpoints;
            return filibuster;
        }

        public bool Cloture(float alignment)
        {
            return alignment >= 0.8f;
        }
    }
}
