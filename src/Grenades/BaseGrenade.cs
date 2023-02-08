namespace DuckGame.C44P
{
    public class BaseGrenade : Gun
    {
        public StateBinding TimerBinding
        {
            get;
            set;
        }
        public StateBinding HasPinBinding
        {
            get;
            set;
        }
        public bool HasPin
        {
            get
            {
                return hasPin;
            }
            set
            {
                if (HasPin && !value)
                {
                    //CreatePinParticle();
                }
                hasPin = value;
            }
        }
        protected virtual void CreatePinParticle()
        {
            GrenadePin grenadePin = new GrenadePin(x, y);
            grenadePin.hSpeed = -offDir * Rando.Float(1.5f, 2f);
            grenadePin.vSpeed = -2f;
            Level.Add(grenadePin);

            SFX.Play("pullPin", 1f, 0.0f, 0.0f, false);
        }
        public float Timer
        {
            get;
            set;
        }
        public bool HasExploded
        {
            get;
            protected set;
        }
        protected SpriteMap sprite;
        bool hasPin;
        public BaseGrenade(float xval, float yval) : base(xval, yval)
        {
            ammo = 1;
            _ammoType = new ATLaser();
            TimerBinding = new StateBinding("Timer", -1, false);
            HasPinBinding = new StateBinding("HasPin", -1, false);
        }
        public override void OnPressAction()
        {
            if (HasPin)
            {
                HasPin = false;
            }
        }
        public override void Fire()
        {
        }
        protected virtual void UpdateTimer()
        {
            if (!HasPin)
            {
                if (Timer > 0)
                {
                    Timer -= 0.01666666f;
                }
                else
                {
                    if (!HasExploded)
                    {
                        Explode();
                    }
                }
            }
        }
        public virtual void Explode()
        {

        }
        public override void Update()
        {
            base.Update();
            UpdateTimer();
            UpdateFrame();
        }
        protected virtual void UpdateFrame()
        {
            sprite.frame = HasPin ? 0 : 1;
        }
    }
}
