using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowABillBecomesALaw
{
    public class LegislativeProcess
    {
        //Threshold: 75%
        public List<Senator> senators { get; private set; }

        //Threshold: 60%
        public List<Representative> representatives { get; private set; }

        //Threshold: 50%
        public President president { get; private set; }

        Random rng = new Random();

        public event EventHandler Filibuster;
        public event EventHandler Cloture;
        public event EventHandler Approved;
        public event EventHandler PassSenate;
        public event EventHandler PassHouse;
        public event EventHandler PassPres;
        public event EventHandler Veto;
        public event EventHandler OverrideVeto;
        public event EventHandler Denied;

        public LegislativeProcess()
        {
            GeneratePoliticians();
        }

        public void GeneratePoliticians()
        {
            senators = new List<Senator>();
            representatives = new List<Representative>();
            RandomName nameGen = new RandomName(); 

            for (int i = 0; i < 100; i++)
            {
                senators.Add(new Senator(nameGen.Generate()));
            }

            for (int i = 0; i < 100; i++)
            {
                representatives.Add(new Representative(nameGen.Generate()));
            }

            president = new President(nameGen.Generate());
        }

        public void BillProcess(Bill bill)
        {
            bool senateOrigin = bill.senateOrigin;

            if(!VoteBill(bill, senateOrigin ? senators.ToArray() : representatives.ToArray(), senateOrigin ? 0.75f : 0.6f)) { CallEvent(Denied); return; }
            CallEvent(senateOrigin ? PassSenate : PassHouse);
            if (!VoteBill(bill, senateOrigin ? representatives.ToArray() : senators.ToArray(), senateOrigin ? 0.6f : 0.75f)) { CallEvent(Denied); return; }
            CallEvent(senateOrigin ? PassHouse : PassSenate);


            if (!president.Vote(bill.viewpoints, 0.45f, out _))
            {
                CallEvent(Veto);
                if(!VoteBill(bill, senators.ToArray(), 0.8f, 0.665f) && VoteBill(bill, representatives.ToArray(), 0.7f, 0.665f)) { CallEvent(Denied); return; }
            }

            CallEvent(PassPres);
        }

        bool VoteBill(Bill bill, Politician[] politicians, float threshold, float percentNeeded = 0.5f)
        {
            int supporters = 0;
            int clotures = 0;

            bool filibuster = false;

            foreach(var politician in politicians)
            {
                supporters += Convert.ToInt32(politician.Vote(bill.viewpoints, threshold, out float alignment));

                if(politician is Senator)
                {
                    clotures += Convert.ToInt32(alignment > 0.5 || rng.Next(0, 6) == 0);
                    filibuster = alignment < 0.3 ? true : filibuster;
                    CallEvent(Filibuster);
                }
            }

            if(politicians is Senator[])
            {
                if(clotures >= 60)
                {
                    CallEvent(Cloture);
                }
                else if (filibuster)
                {
                    return false;
                }
            }

            return supporters > senators.Count * percentNeeded;
        }

        void CallEvent(EventHandler _event){
            if(_event != null)
            {
                _event(this, EventArgs.Empty);
            }
        }
    }
}
