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
            verticalWidthThick = 15f;
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
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class GreenBrickTileset : AutoBlock
    {
        public GreenBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/GreenBrick.png"))
        {
            _editorName = "Green brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class LightGreenBrickTileset : AutoBlock
    {
        public LightGreenBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/LightGreenBrick.png"))
        {
            _editorName = "LGreen brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class OrangeBrickTileset : AutoBlock
    {
        public OrangeBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/OrangeBrick.png"))
        {
            _editorName = "Orange brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class PinkBrickTileset : AutoBlock
    {
        public PinkBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/PinkBrick.png"))
        {
            _editorName = "Pink brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class VioletBrickTileset : AutoBlock
    {
        public VioletBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/VioletBrick.png"))
        {
            _editorName = "Violet brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Bricks")]
    public class YellowBrickTileset : AutoBlock
    {
        public YellowBrickTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Bricks/YellowBrick.png"))
        {
            _editorName = "Yellow brick";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
}
