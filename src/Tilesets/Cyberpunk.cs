namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class CyberPlat1 : AutoPlatform
    {
        public CyberPlat1(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_platform01.png"))
        {
            _editorName = "Cyberplatform v1";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class CyberPlat2 : AutoPlatform
    {
        public CyberPlat2(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_platform02.png"))
        {
            _editorName = "Cyberplatform v2";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 12f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class CyberpunkTileset : AutoBlock
    {
        public CyberpunkTileset(float xval, float yval) : base(xval, yval, GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_block.png"))
        {
            _editorName = "Cyber block";
            physicsMaterial = PhysicsMaterial.Metal;
            verticalWidth = 10f;
            verticalWidthThick = 15f;
            horizontalHeight = 14f;
        }
    }
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class BackgroundCyber : BackgroundTile
    {
        public BackgroundCyber(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_background01.png"), 16, 16, true);
            _opacityFromGraphic = true;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            _editorName = "CP Lights";
        }
    }
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class BackgroundCyber2 : BackgroundTile
    {
        public BackgroundCyber2(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_background02.png"), 16, 16, true);
            _opacityFromGraphic = true;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            _editorName = "Cyber background";
        }
    }
    [EditorGroup("ADGM|Tiles|Cyberduck 2084")]
    public class CyberParallax : BackgroundUpdater
    {
        public CyberParallax(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduckicon.png"), 16, 16, false);
            center = new Vec2(8f, 8f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            depth = 0.9f;
            layer = Layer.Foreground;
            _visibleInGame = false;
            _editorName = "Cybercity";
        }
        public override void Initialize()
        {
            if (Level.current is Editor)
            {
                return;
            }
            backgroundColor = new Color(0, 0, 0);
            Level.current.backgroundColor = backgroundColor;
            _parallax = new ParallaxBackground(Mod.GetPath<C44P>("Sprites/Tilesets/Cyberpunk/cyberduck_2084_parallax.png"), 0f, 0f, 3);
            float speed = 0.4f;
            _parallax.AddZone(0, 0.3f, speed, false, true);
            _parallax.AddZone(1, 0.3f, speed, false, true);
            _parallax.AddZone(2, 0.3f, speed, false, true);
            _parallax.AddZone(3, 0.3f, speed, false, true);
            _parallax.AddZone(4, 0.3f, speed, false, true);
            _parallax.AddZone(5, 0.3f, speed, false, true);
            _parallax.AddZone(6, -2.4f, speed, true, true);
            _parallax.AddZone(7, -2.4f, speed, true, true);
            _parallax.AddZone(8, 0f, speed, false, true);
            _parallax.AddZone(9, 0f, speed, false, true);
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
}
