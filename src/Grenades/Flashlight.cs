using System.Collections.Generic;

namespace DuckGame.C44P
{
    public class Flashlight : Thing
    {
        public StateBinding _positionStateBinding = new CompressedVec2Binding("position");
        protected SpriteMap _sprite;
        private float radius;
        private int outFrame = 0;
        private SinWave _pulse2 = Rando.Float(0.5f, 4f);
        public bool IsLocalDuckAffected
        {
            get;
            set;
        }
        public float Timer
        {
            get;
            set;
        }

        public Flashlight(float xval, float yval, float stayTime = 2f, float radius = 160f, float alp = 1f) : base(xval, yval)
        {
            Timer = stayTime;
            this.depth = 1f;
            this.layer = Layer.Foreground;
            this.radius = radius;
            SetIsLocalDuckAffected();
            if (IsLocalDuckAffected)
            {
                SFX.Play(GetPath("flashBeep.wav"), 1f, 0.0f, 0.0f, false);
            }
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Items/Weapons/FlashbangLight"), 32, 32);
            this._sprite.alpha = alp;
        }
  
        public virtual void SetIsLocalDuckAffected()
        {
            List<Duck> ducks = new List<Duck>();
            foreach (Duck duck in Level.CheckCircleAll<Duck>(position, radius))
            {
                if (!ducks.Contains(duck))
                {
                    ducks.Add(duck);
                }
            }
            foreach (Ragdoll ragdoll in Level.CheckCircleAll<Ragdoll>(position, radius))
            {
                if (!ducks.Contains(ragdoll._duck))
                {
                    ducks.Add(ragdoll._duck);
                }
            }
            foreach (Duck duck in ducks)
            {
                if (duck.profile.localPlayer)
                {
                    if (Level.CheckLine<Block>(position, duck.position, duck) == null)
                    {
                        IsLocalDuckAffected = true;
                        return;
                    }
                }
            }
            IsLocalDuckAffected = false;
        }

        public override void Update()
        {
            base.Update();
            this._sprite.xscale = Level.current.camera.width/32;
            this._sprite.yscale = Level.current.camera.height/32;
            this._sprite.angleDegrees = 0f + _pulse2*0.1f;
            if (Timer > 0)
            {
                Timer -= 0.01f;
            }
            else
            {
                this._sprite.alpha -= 0.011f;
                this.outFrame++;
            }
            //SoundEffect.SpeedOfSound = 1f - this.alpha;
            if (this.outFrame > 90)
            {
                Level.Remove(this);
            }
        }

        public override void Draw()
        {
            if (IsLocalDuckAffected)
            {
                //updater.ShaderController();
                Graphics.Draw(this._sprite, Level.current.camera.position.x, Level.current.camera.position.y, 1f);
            }
            base.Draw();
        }
    }
}
