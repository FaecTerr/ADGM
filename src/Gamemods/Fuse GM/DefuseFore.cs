namespace DuckGame.C44P
{
    public class DefuseFore : Thing
    {
        protected SpriteMap _sprite;
        public float Timer;
        public DefuseFore(float xval, float yval, float stayTime = 0.1f, int frame = 0) : base(xval, yval)
        {
            Timer = stayTime;
            layer = Layer.Foreground;
            center = new Vec2(12f, 8f);
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/FuseMode/Defusing.png"), 24, 16);
            _sprite.center = new Vec2(12f, 8f);
            _sprite._frame = frame;
            _sprite.scale = new Vec2(2f, 2f);
        }
        public override void Update()
        {
            base.Update();
            if(Timer > 0f)
            {
                Timer -= 0.01f;
                depth = new Depth(depth.value - 0.01f, depth.span);
            }
            else
            {
                Level.Remove(this);
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (true)
                Graphics.Draw(_sprite, position.x, position.y, 0.9f);
        }
    }
}