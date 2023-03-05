namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Collect")]
    public class CollectBase : Thing
    {
        private SpriteMap _sprite;
        public EditorProperty<int> team;
        public CollectBase(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/BlueSquare.png"), 16, 16, false);
            base.graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/BlueSquare.png"), 16, 16, false);
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(15f, 15f);
            graphic = _sprite;
            _visibleInGame = false;
            layer = Layer.Foreground;
            team = new EditorProperty<int>(0, this, 0f, 8f, 1f, null, false, false);
        }
        public override void Draw()
        {
            base.Draw();
            _sprite.frame = team;
        }
    }
}
