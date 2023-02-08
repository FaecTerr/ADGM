namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Sofa : C4Furniture
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;

        public Sofa(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Sofa.png"), 42, 21);
            graphic = _sprite;
            _sprite.frame = 0;
            _sprite.center = new Vec2(20f, 10.5f);

            center = new Vec2(20.5f, 10.5f);
            _collisionSize = new Vec2(38f, 20f);
            _collisionOffset = new Vec2(-19f, -11f);

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
