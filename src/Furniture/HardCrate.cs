namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Furniture")]
    public class HardCrate : Crate
    {
        SpriteMap _sprite;
        public HardCrate(float xpos, float ypos) : base (xpos, ypos)
        {
            _maxHealth = 50f;
            _hitPoints = 50f;
            _sprite = new SpriteMap(GetPath("Sprites/Items/HardCrate"), 16, 16, false);
            base.graphic = _sprite;
            graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(15f, 15f);
            base.depth = -0.5f;
            _editorName = "HardCrate";
            thickness = 9f;
            weight = 6f;
            _holdOffset = new Vec2(2f, 0f);
            flammable = 0.3f;
            base.collideSounds.Add("crateHit");
        }
    }
}
