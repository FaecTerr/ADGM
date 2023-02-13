namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Fuse")]
    public class Defuser : Holdable
    {
        SpriteMap _sprite;
        public EditorProperty<bool> onlyCT;
        public Defuser(float xval, float yval) : base(xval, yval)
        {
            center = new Vec2(7f, 5f);
            collisionOffset = new Vec2(-4f, -5f);
            collisionSize = new Vec2(8f, 11f);
            graphic = _sprite;
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/FuseMode/Defuser"), 14, 11, false);
            base.graphic = new SpriteMap(GetPath("Sprites/Gamemods/FuseMode/Defuser.png"), 14, 11, false);
            weight = 0f;
            onlyCT = new EditorProperty<bool>(true);
        }
        public override void Update()
        {
            if (onlyCT == true)
            {
                if (owner != null)
                {
                    Duck d = owner as Duck;
                    if (!d.HasEquipment(typeof(CTEquipment)))
                    {
                        d.doThrow = true;
                    }
                }
                if (prevOwner != null)
                {
                    Duck d = prevOwner as Duck;
                    if (!d.HasEquipment(typeof(CTEquipment)))
                    {
                        hSpeed = 0f;
                    }
                }
            }
            base.Update();
        }
    }
}
