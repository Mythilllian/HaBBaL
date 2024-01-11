using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowABillBecomesALaw
{
    public abstract class Politician
    {
        public string name { get; set; }

        public float[] viewpoints { get; internal set; }
        //Array contains answers from 0 (strongly disagree, republican belief) to 1 (strongly agree, democrat belief).
        //
        //1. The government should provide universal healthcare.
        //2. We must address inequalities of groups (race, sexuality, gender, etc.).
        //3. Investment into infrastructure is necessary for growth and modernization.
        //4. The government should allow more immigrants and support illegal immigrants.
        //5. The government should tax more heavily to support social programs.
        //6. Stricter gun control is necessary.
        //7. The criminal justice system should be based more on rehabilitation than punishment.
        //8. More restrictions should be placed against climate change.
        //9. Diplomacy should be used in most cases and military intervention as a worst scenario.
        //10 Minimum wage should be raised to a living wage.

        public bool isDem { get; set; }

        public Politician(string name)
        {
            GetViewpoints();
            this.name = name;
        }

        public void GetViewpoints()
        {
            viewpoints = new float[10];
            Random rng = new Random();

            //For issues always supported (or not for republicans), it will find a random value between 0.75 and 1 (or 0 and 0.25)
            for (int i = 0; i < viewpoints.Length; i++)
            {
                viewpoints[i] = rng.Next(i < 4 ? 70 : 35, 101) / 100;
                viewpoints[i] = isDem ? viewpoints[i] : 1 - viewpoints[i];
            }
        }

        public bool Vote(float?[] billViewpoints, float threshold, out float _alignment)
        {
            float alignment = 1;
            for (int i = 0; i < viewpoints.Length; i++)
            {
                alignment -= billViewpoints[i] != null ? Math.Abs(viewpoints[i] - billViewpoints[i].Value)/3.5f * (2 - (i/10)) : 0;
            }
            _alignment = alignment;
            return alignment >= threshold;
        }
    }
}
