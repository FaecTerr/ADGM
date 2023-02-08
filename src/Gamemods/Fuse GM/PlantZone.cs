namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Fuse")]
    public class PlantZone : Thing
    {
        private SpriteMap _sprite;
        public EditorProperty<int> ID;
        public EditorProperty<bool> Custom;
        public PlantZone connectedZone;

        public PlantZone(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/BlueSquare.png"), 16, 16, false);
            base.graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/BlueSquare.png"), 16, 16, false);
            center = new Vec2(8f, 8f);
            _sprite.frame = 2;
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(16f, 16f);
            graphic = _sprite;
            _visibleInGame = false;
            layer = Layer.Foreground;
            ID = new EditorProperty<int>(0, this, 0f, 9f, 1f, null, false, false);
            Custom = new EditorProperty<bool>(true, this, 0f, 1f, 1f, null, false, false);
        }
        public override void Update()
        {
            base.Update();
            if (Custom == false)
            {
                foreach (PlantZone pz in Level.current.things[typeof(PlantZone)])
                {
                    if (pz != null && pz != this)
                    {
                        if (pz.ID == ID)
                        {
                            connectedZone = pz;
                        }
                    }
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (connectedZone != null)
            {
                Graphics.DrawRect(position, connectedZone.position, Color.Yellow, 1f, true, 1f);
            }
        }
    }
}
