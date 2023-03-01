using System;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Furniture")]
    public class Upstairs : Thing
    {
        private SpriteMap sprite;
        private Upstairs connectedStairs;

        public float Cooldown;

        public EditorProperty<int> Style;

        public Upstairs(float xval, float yval) : base(xval, yval)
        {
            center = new Vec2(11f, 19f);
            collisionOffset = new Vec2(-11f, -19f);
            collisionSize = new Vec2(22f, 38f);
            sprite = new SpriteMap(GetPath("Sprites/Upstairs.png"), 22, 38, false);
            base.graphic = sprite;
            depth = -0.8f;
            hugWalls = WallHug.Floor;
            Style = new EditorProperty<int>(0, this, 0f, 7f, 1f);
            _editorName = "Stairs";
        }
        public override void Update()
        {
            base.Update();

            if(Cooldown > 0f)
            {
                Cooldown -= 0.1f;
            }

            sprite.frame = Style;
            Vec2 dir = new Vec2(0f, -1f);
            if (Style % 2 == 1)
            {
                dir = new Vec2(0f, 1f);
            }

            if (connectedStairs == null)
            {
                Upstairs up = Level.CheckLine<Upstairs>(position + dir * 6f, position + dir * 16000f, this);
                if(up != null)
                {
                    connectedStairs = up;
                }
            }
            if (connectedStairs != null)
            {
                foreach (Duck d in Level.CheckRectAll<Duck>(topLeft, bottomRight))
                {
                    if (d != null && connectedStairs.Cooldown <= 0f)
                    {
                        if (d.inputProfile.Released("UP"))
                        {
                            d.position = new Vec2(connectedStairs.position.x, connectedStairs.position.y + 4f);
                            connectedStairs.Cooldown = 3f;
                            Cooldown = 3f;
                        }
                    }
                }
            }

        }
        public override void Draw()
        {
            base.Draw();

            if (Level.current is Editor)
            {
                sprite.frame = Style;
                Vec2 dir = new Vec2(0f, -1f);
                if (Style % 2 == 1)
                {
                    dir = new Vec2(0f, 1f);
                }

                Upstairs up = Level.CheckLine<Upstairs>(position + dir * 6f, position + dir * 16000f, this);
                if (up != null)
                {
                    Graphics.DrawLine(position, up.position, Color.MediumPurple * 0.5f, 2f);
                    Vec2 point = position + (up.position - position) * DateTime.Now.Millisecond * 0.001f;
                    Graphics.DrawRect(point - new Vec2(2, 2), point + new Vec2(2, 2), Color.OrangeRed * 0.5f);
                }
            }
            
            if (connectedStairs != null)
            {
                foreach (Duck d in Level.CheckCircleAll<Duck>(position, 32))
                {
                    if (d.profile.localPlayer)
                    {
                        Graphics.DrawLine(position, connectedStairs.position, Color.MediumPurple * 0.5f, 2f);
                        Vec2 point = position + (connectedStairs.position - position) * DateTime.Now.Millisecond / 1000;
                        Graphics.DrawRect(point - new Vec2(4, 4), point + new Vec2(4, 4), Color.OrangeRed);
                    }
                }
            }
            sprite.frame = Style;
        }
    }
}
