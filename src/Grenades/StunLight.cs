using System.Collections.Generic;

namespace DuckGame.C44P
{
    public class Stunlight : Thing
    {
        public StateBinding _positionStateBinding = new CompressedVec2Binding("position");
        protected SpriteMap _sprite;
        private float radius;
        private int outFrame = 0;
        private SinWave _pulse1 = Rando.Float(1f, 2f);
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

        public Stunlight(float xval, float yval, float stayTime = 2f, float radius = 160f, float alp = 1f) : base(xval, yval)
        {
            Timer = stayTime;
            
            depth = 1f;
            layer = Layer.Foreground;
            this.radius = radius;
            SetIsLocalDuckAffected();
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Items/Weapons/StunLight.png"), 32, 32);
            _sprite.alpha = alp;
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
            _sprite.xscale = Level.current.camera.width/32;
            _sprite.yscale = Level.current.camera.height/32;
            _sprite.angleDegrees = 0f + _pulse2 * 0.1f;
            if (Timer > 0)
            {
                Timer -= 0.01f;
            }
            else
            {
                _sprite.alpha -= 0.011f;
                outFrame++;
            }
            if(outFrame > 90)
            {
                Level.Remove(this);
            }
        }

        public override void Draw()
        {
            if (IsLocalDuckAffected)
            {
                Graphics.Draw(_sprite, Level.current.camera.position.x, Level.current.camera.position.y, 1f);
            }
            base.Draw();
        }
    }
}
