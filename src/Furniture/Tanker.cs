namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Tanker : C4Furniture
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;

        public Tanker(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Tanker.png"), 16, 20);
            graphic = _sprite;
            style = new EditorProperty<int>(-1, this, -1f, 2f, 1f, null, false, false);
            _sprite.frame = 0;
            _sprite.center = new Vec2(9f, 10f);
            _canFlip = true;
            thickness = 2f;
            depth = 0.95f;
            center = new Vec2(8f, 10f);
            _collisionSize = new Vec2(16f, 20f);
            _collisionOffset = new Vec2(-8f, -10f);
            hugWalls = WallHug.Floor;
        }
        public override void Draw()
        {
            graphic.flipH = flipHorizontal;
            base.Draw();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void EditorPropertyChanged(object property)
        {
            if (style == -1)
            {
                (graphic as SpriteMap).frame = Rando.Int(0, 2);
                return;
            }
            (graphic as SpriteMap).frame = style.value;
        }
    }
}
