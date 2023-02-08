namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Sign : Thing
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;
        public Sign(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Signs.png"), 32, 32);
            center = new Vec2(16f, 16f);
            _collisionSize = new Vec2(32f, 32f);
            _collisionOffset = new Vec2(-16f, -16f);
            graphic = _sprite;
            style = new EditorProperty<int>(0, this, 0f, 15f, 1f, null, false, false);
            _sprite.frame = 0;
            _canFlip = true;
            depth = -0.6f;
            base.hugWalls = WallHug.Floor;
        }
        public override void Draw()
        {
            graphic.flipH = flipHorizontal;
            base.Draw();
        }
        public override void EditorPropertyChanged(object property)
        {
            (graphic as SpriteMap).frame = style.value;
        }
    }
}
