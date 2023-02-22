namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Tiles|Jungle")]
    public class JunglePlatform : AutoPlatform
    {
        public JunglePlatform(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Jungle/junglePlatform.png"))
        {
            _editorName = "Jungle Platform";
            physicsMaterial = PhysicsMaterial.Wood;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }

    [EditorGroup("ADGM|Tiles|Jungle")]
    public class JungleTree : AutoPlatform
    {
        public JungleTree(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Jungle/jungleTree.png"))
        {
            _editorName = "Jungle Tree";
            physicsMaterial = PhysicsMaterial.Wood;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Jungle")]
    public class JungleTileset : AutoBlock
    {
        public JungleTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Jungle/jungleTileset.png"))
        {
            _editorName = "Jungle";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Jungle")]
    public class BackgroundJungle : BackgroundTile
    {
        public BackgroundJungle(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Jungle/jungleBackground.png"), 16, 16, true);
            _opacityFromGraphic = true;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            _editorName = "Jungle background";
        }
    }
}
