namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Guns|Grenades")]
    [BaggedProperty("isFatal", false)]
    public class FlashbangGrenade : BaseGrenade
    {
        public FlashbangGrenade(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Items/Weapons/Flashbang.png"), 16, 16, false);
            graphic = sprite;
            center = new Vec2(7f, 7f);
            collisionOffset = new Vec2(-4f, -5f);
            collisionSize = new Vec2(8f, 12f);
            TimerBinding = new StateBinding("Timer", -1, false);
            HasPinBinding = new StateBinding("HasPin", -1, false);
            HasPin = true;
            Timer = 2f;
            bouncy = 0.4f;
            friction = 0.05f;
            ammo = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Explode()
        {
            QuickFlash();
            Flash();
            SFX.Play(GetPath("flashGrenadeExplode.wav"), 1f, 0.0f, 0.0f, false);
            Level.Remove(this);
            HasExploded = true;
        }

        public virtual void QuickFlash()
        {
            Graphics.flashAdd = 1.3f;
        }

        public virtual void Flash()
        {
            Level.Add(new Flashlight(position.x, position.y));
        }

        public override void OnPressAction()
        {
            Level.Add(new FlashbangPin(x, y)
            {
                hSpeed = (-offDir) * (1.5f + Rando.Float(0.5f)),
                vSpeed = -2f
            });
            base.OnPressAction();
        }
    }
    public class FlashbangPin : EjectedShell
    {
        public FlashbangPin(float xpos, float ypos) : base(xpos, ypos, GetPath<C44P>("Sprites/Items/Weapons/FlashbangPin.png"), "metalBounce")
        {
        }
    }
}