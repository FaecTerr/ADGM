namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Chair : C4Furniture
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;

        public Chair(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Chair.png"), 26, 22);
            graphic = _sprite;
            _sprite.frame = 0;
            _sprite.center = new Vec2(14f, 11f);

            center = new Vec2(13f, 10f);
            _collisionSize = new Vec2(26f, 22f);
            _collisionOffset = new Vec2(-13f, -11f);

            _canFlip = true;
            thickness = 2f;
            depth = 0.6f;
            hugWalls = WallHug.Floor;

            style = new EditorProperty<int>(-1, this, -1f, 2f, 1f, null, false, false);
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
                (graphic as SpriteMap).frame = Rando.Int(2);
                return;
            }
            (graphic as SpriteMap).frame = style.value;
        }
    }
}
