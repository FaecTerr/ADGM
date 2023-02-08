namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Seats : C4Furniture
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;

        public Seats(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Seats.png"), 38, 21);
            style = new EditorProperty<int>(-1, this, -1f, 2f, 1f, null, false, false);
            graphic = _sprite;
            _sprite.frame = 0;
            _sprite.center = new Vec2(19f, 10.5f);
            _canFlip = true;
            thickness = 2f;
            depth = 0.6f;
            center = new Vec2(19f, 10.5f);
            _collisionSize = new Vec2(38f, 20f);
            _collisionOffset = new Vec2(-19f, -11f);
            hugWalls = WallHug.Floor;
            _editorName = "Cabinets";
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
