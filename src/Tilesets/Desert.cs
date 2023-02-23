namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Tiles|Sand")]
    public class BackgroundDesert : BackgroundTile
    {
        public BackgroundDesert(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Desert/sandbackground.png"), 16, 16, true);
            _opacityFromGraphic = true;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            _editorName = "BG Sand";
        }
    }
    [EditorGroup("ADGM|Tiles|Sand")]
    public class DesertParallax : BackgroundUpdater
    {
        public DesertParallax(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Desert/sandIcon.png"), 16, 16, false);
            center = new Vec2(8f, 8f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = 0.9f;
            layer = Layer.Foreground;
            _visibleInGame = false;
            _editorName = "Sand";
        }
        public override void Initialize()
        {
            if (Level.current is Editor)
            {
                return;
            }
            backgroundColor = new Color(74, 74, 74);
            Level.current.backgroundColor = backgroundColor;
            _parallax = new ParallaxBackground(Mod.GetPath<C44P>("Sprites/Tilesets/Desert/sandparallax.png"), 0f, 0f, 3);
            float speed = 0.4f;
            _parallax.AddZone(0, 0f, speed, false, true);
            _parallax.AddZone(1, 1f, speed, false, true);
            _parallax.AddZone(2, 1f, speed, false, true);
            _parallax.AddZone(3, 1f, speed, false, true);
            _parallax.AddZone(4, 1f, speed, false, true);
            _parallax.AddZone(5, 1f, speed, false, true);
            _parallax.AddZone(6, 3f, speed * 0.05f, true, true);
            _parallax.AddZone(7, 3f, speed * 0.05f, true, true);
            _parallax.AddZone(8, 4f, speed * 0.04f, true, true);
            _parallax.AddZone(9, 4f, speed * 0.04f, true, true);
            _parallax.AddZone(10, 0f, speed, false, true);
            _parallax.AddZone(11, 0f, speed, false, true);
            _parallax.AddZone(12, 0f, speed, false, true);
            _parallax.AddZone(13, 0f, speed, false, true);
            _parallax.AddZone(14, 0f, speed, false, true);
            _parallax.AddZone(15, 0f, speed, false, true);
            _parallax.AddZone(16, 0f, speed, false, true);
            _parallax.AddZone(17, 0f, speed, false, true);
            _parallax.AddZone(18, 0f, speed, false, true);
            _parallax.AddZone(19, 0f, speed, false, true);
            _parallax.AddZone(20, 0f, speed, false, true);
            _parallax.AddZone(21, 0f, speed, false, true);
            _parallax.AddZone(22, 0f, speed, false, true);
            _parallax.AddZone(23, 0f, speed, false, true);
            _parallax.AddZone(24, 0f, speed, false, true);
            _parallax.AddZone(25, 0f, speed, false, true);
            _parallax.AddZone(26, 0f, speed, false, true);
            _parallax.AddZone(27, 0f, speed, false, true);
            _parallax.AddZone(28, 0f, speed, false, true);
            _parallax.AddZone(29, 0f, speed, false, true);
            Level.Add(_parallax);
        }
        public override void Update()
        {
            Vec2 wallScissor = GetWallScissor();
            if (wallScissor != Vec2.Zero)
            {
                scissor = new Rectangle((float)((int)wallScissor.x), 0f, (float)((int)wallScissor.y), (float)Graphics.height);
            }
            base.Update();
        }
        public override void Terminate()
        {
            Level.Remove(_parallax);
        }
    }
    [EditorGroup("ADGM|Tiles|Sand")]
    public class DesertTileset : AutoBlock
    {
        public DesertTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Desert/sand.png"))
        {
            _editorName = "Sand";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }

    [EditorGroup("ADGM|Tiles|Sand")]
    public class PalmTree : AutoPlatform
    {
        public PalmTree(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Desert/palm.png"))
        {
            _editorName = "Palm tree";
            physicsMaterial = PhysicsMaterial.Wood;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Sand")]
    public class Palm1 : AutoPlatform
    {
        public Palm1(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Desert/sandPalm_01.png"))
        {
            _editorName = "Palm leaves 1";
            physicsMaterial = PhysicsMaterial.Wood;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Sand")]
    public class Palm2 : AutoPlatform
    {
        public Palm2(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Desert/sandPalm_02.png"))
        {
            _editorName = "Palm leaves 1";
            physicsMaterial = PhysicsMaterial.Wood;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
}
