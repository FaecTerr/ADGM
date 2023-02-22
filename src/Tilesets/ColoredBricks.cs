namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class RedBrickTileset : AutoBlock
    {
        public RedBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/RedBrick.png"))
        {
            _editorName = "Red brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class BlueBrickTileset : AutoBlock
    {
        public BlueBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/BlueBrick.png"))
        {
            _editorName = "Blue brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
}
