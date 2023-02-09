namespace DuckGame.C44P
{
    public class ButtonsG : Thing
    {
        SpriteMap _sprite;
        public ButtonsG(float xval, float yval) : base(xval, yval)
        {
            center = new Vec2(6f, 8f);
            collisionOffset = new Vec2(-6f, -8f);
            collisionSize = new Vec2(12f, 16f);
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/FuseMode/Buttons.png"), 12, 16, false);
            _sprite.frame = Rando.Int(0, 9);
            graphic = _sprite;
        }
        public override void Update()
        {
            base.Update();
            alpha -= 0.1f;
            if (alpha < 0.1f)
                Level.Remove(this);
        }
    }
}
