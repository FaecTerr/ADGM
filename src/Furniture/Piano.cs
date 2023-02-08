namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Piano : C4Furniture
    {
        SpriteMap _sprite;

        public Piano(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(GetPath("Sprites/Decor/Piano.png"), 40, 32);
            _sprite = new SpriteMap(GetPath("Sprites/Decor/Piano.png"), 40, 32);
            _sprite.center = new Vec2(20f, 16f);
            _sprite.alpha = 0.8f;
            _sprite.depth = 0.9f;
            _canFlip = true;
            thickness = 1f;
            center = new Vec2(20f, 15f);
            _collisionSize = new Vec2(40f, 32f);
            _collisionOffset = new Vec2(-20f, -16f);
            base.depth = 0.1f;
            base.hugWalls = WallHug.Floor;
        }
        public override void Update()
        {
            base.Update();
           
        }
    }
}
