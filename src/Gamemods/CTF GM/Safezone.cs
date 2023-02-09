namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|Furniture")]
    public class Safezone : MaterialThing
    {
        private SpriteMap _sprite;
        public Safezone(float xval, float yval) : base(xval, yval)
        {
            center = new Vec2(8f, 24f);
            collisionOffset = new Vec2(-8f, -24f);
            collisionSize = new Vec2(16f, 48f);
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/Safezone.png"), 16, 48, true);
            base.graphic = _sprite;
            depth = -0.8f;
            thickness = 10f;
        }
        public override void Draw()
        {
            graphic.flipH = flipHorizontal;
            base.Draw();
        }
        public override void Update()
        {
            base.Update();
            foreach(PhysicsObject po in Level.CheckRectAll<PhysicsObject>(topLeft, bottomRight))
            {
                if (po != null)
                {
                    if (flipHorizontal)
                    {
                        po.hSpeed = -3f;
                    }
                    else
                    {
                        po.hSpeed = 3f;
                    }
                }
            }
        }
    }
}
