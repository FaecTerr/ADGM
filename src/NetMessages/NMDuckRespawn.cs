using System;

namespace DuckGame.C44P
{
    public class NMDuckRespawn : NMEvent
    {
        public Duck duck;
        public bool start;

        public NMDuckRespawn()
        {
        }

        public NMDuckRespawn(Duck d, bool start)
        {
            duck = d;
            this.start = start;
        }

        public override void Activate()
        {
            if (duck == null)
                return;
            Material material = null;
            duck.material = material;
            if (duck._ragdollInstance != null && duck._ragdollInstance._part1 != null && duck._ragdollInstance._part2 != null && duck._ragdollInstance._part3 != null)
            {
                duck._ragdollInstance._part1.material = duck.material;
                duck._ragdollInstance._part2.material = duck.material;
                duck._ragdollInstance._part3.material = duck.material;
            }
            if (duck.isServerForObject)
            {
                Thing.Fondle(duck, DuckNetwork.localConnection);
            }
            duck.invincible = start;
            if (start)
            {
                duck.onFire = false;
                foreach (MaterialThing t in Level.CheckCircleAll<MaterialThing>(duck.position, 32f))
                {
                    t.onFire = false;
                }
            }
            duck.Ressurect();
            base.Activate();
        }
    }
}