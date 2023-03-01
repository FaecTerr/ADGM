using System;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Guns|Grenades")]
    [BaggedProperty("isFatal", false)]
    public class FireGrenade : BaseGrenade
    {
        private int charges = 1;
        private float volume = 0.4f; 
        private float Time;
        public FireGrenade(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Items/Weapons/FireGrenade.png"), 16, 16, false);
            graphic = sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-3f, -5f);
            collisionSize = new Vec2(6f, 10f);

            HasPin = true;
            Timer = 2f;
            bouncy = 0.4f;
            friction = 0.05f;
            ammo = 1; 
            
            TimerBinding = new StateBinding("Timer", -1, false);
            HasPinBinding = new StateBinding("HasPin", -1, false);
        }

        void PourOut()
        {
            FluidStream _stream;
            FluidData _fluidData = Fluid.Gas;

            _stream = new FluidStream(x, y, new Vec2(1f, 0.0f), 2f, new Vec2());
            Level.Add(_stream);

            int pie_pieces = 16;
            for (int i = 0; i < pie_pieces; i++)
            {
                _stream.Draw();
                _stream.sprayAngle = new Vec2((float)Math.Cos(i * 360 / pie_pieces), (float)Math.Sin(i * 360 / pie_pieces)) * 1.5f;
                _stream.DoUpdate();
                _stream.position = position;
                _fluidData.amount = volume / (charges * pie_pieces);
                _stream.Feed(_fluidData);
            }

            for (int i = 0; i < 4; i++)
            {
                Spark spark = Spark.New(position.x, position.y, new Vec2(Rando.Float(-2, 2), Rando.Float(-3, -4)), 0.002f);
                Level.Add(spark);
            }
        }

        public override void Explode()
        {
            if (charges > 0)
            {
                if (Time > 0f)
                {
                    Time -= 0.02f;
                }
                else
                {
                    Time = 0.4f;
                    PourOut();
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

        public override void OnPressAction()
        {
            Level.Add(new FireGrenadePin(x, y)
            {
                hSpeed = (-offDir) * (1.5f + Rando.Float(0.5f)),
                vSpeed = -2f
            });
            base.OnPressAction();
        }
        public override void Update()
        {
            base.Update();
            if (HasExploded == true)
            {
                Explode();
            }
        }
    }

    public class FireGrenadePin : EjectedShell
    {
        public FireGrenadePin(float xpos, float ypos) : base(xpos, ypos, GetPath<C44P>("Sprites/Items/Weapons/FireGrenadePin.png"), "metalBounce")
        {
        }
    }
}