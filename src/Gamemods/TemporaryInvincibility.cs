using System;

namespace DuckGame.C44P
{
    public class TemporaryInvincibility : Thing
    {
        public StateBinding _duckBinding = new StateBinding("duck");
        public StateBinding _timerBinding = new StateBinding("timer");
        public StateBinding _positionBinding = new StateBinding("position");
        public Duck duck;
        public int frames = 80;
        public TemporaryInvincibility(Duck d)
        {
            duck = d;
        }
        public override void Update()
        {
            if (isServerForObject)
            {
                Vec2 pos = duck.position;

                if (duck.ragdoll != null)
                {
                    pos = duck.ragdoll.part2.position;
                }
                else if (duck._trapped != null)
                {
                    pos = duck._trapped.position;
                }

                position = pos;
                duck.invincible = true;

                if (frames <= 0)
                {
                    duck.invincible = false; 
                    foreach (ForceTag f in Level.current.things[typeof(ForceTag)])
                    {
                        Level.Remove(this);
                        return;
                    }
                    duck.Kill(new DTImpact(this));
                }
                else
                {
                    frames--;
                    foreach (ForceTag f in Level.current.things[typeof(ForceTag)])
                    {
                        return;
                    }
                    duck.invincible = false;
                    duck.Kill(new DTImpact(this));
                }
            }
        }
    }
}
