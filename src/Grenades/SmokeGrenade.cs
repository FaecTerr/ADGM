namespace DuckGame.C44P

{
    [EditorGroup("ADGM|Guns|Grenades")]
    [BaggedProperty("isFatal", false)]
    public class SmokeGrenade : BaseGrenade
    {
        private int charges = 30;
        public float Time;

        public SmokeGrenade(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Items/Weapons/SmokeGrenade.png"), 16, 16, false);
            graphic = sprite;
            center = new Vec2(7f, 7f);
            collisionOffset = new Vec2(-4f, -5f);
            collisionSize = new Vec2(8f, 12f);
            TimerBinding = new StateBinding("Timer", -1, false);
            HasPinBinding = new StateBinding("HasPin", -1, false);
            HasPin = true;
            Timer = 1f;
            bouncy = 0.4f;
            friction = 0.05f;
            ammo = 1;
        }

        public override void Explode()
        {
            if (charges > 0)
            {
                if (Time > 0f)
                {
                    Time -= 0.01666666f;
                }
                else
                {
                    Time = 0.5f;
                    if (charges > 25)
                    {
                        Time = 0.1f;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        Level.Add(new SmokeGR(x, y));
                    }
                    charges--;
                }
            }
            else
            {
                Level.Remove(this);
            }
        }

        public virtual void QuickFlash()
        {
            Graphics.flashAdd = 1.3f;
        }

        public override void Update()
        {
            base.Update();
            if(HasExploded == true)
            {
                Explode();
            }
        }
    }
}

