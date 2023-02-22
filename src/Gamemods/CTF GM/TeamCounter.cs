namespace DuckGame.C44P
{
    public class TeamCounter : Thing, IDrawToDifferentLayers
    {
        private BitmapFont _font;
        private bool _temp;
        public float[] count = new float[8];
        public int[] team = new int[8];

        public GMTimer Timer;

        public StateBinding _counter = new StateBinding("count", -1, false, false);
        public StateBinding _TeamBind = new StateBinding("team", -1, false, false);
        public TeamCounter(float xpos, float ypos, bool temp = false) : base(xpos, ypos, null)
        {
            _font = new BitmapFont("biosFont", 8, -1);
            _temp = temp;
            depth = 0.9f;
        }

        public void OnDrawLayer(Layer pLayer)
        {
            if (pLayer == Layer.Foreground)
            {
                if (Timer != null)
                {
                    Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);
                    Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
                    Vec2 drawPosition = camPos + camSize * new Vec2(0.5f, 0.015f);

                    Vec2 Unit = camSize / new Vec2(320, 180);
                    drawPosition += new Vec2(0, 14f) * Unit;

                    SpriteMap flag = new SpriteMap(GetPath("Sprites/Gamemods/CTF/Flag.png"), 27, 18);
                    flag.CenterOrigin();
                    flag.scale = Unit * 0.25f;

                    int[] sameAmount = new int[8];

                    for (int i = 0; i < Teams.active.Count; i++)
                    {
                        Vec2 originalPosition = drawPosition;
                        drawPosition.x += -camSize.x * 0.15f + camSize.x * 0.3f * (count[i] / Timer.progressTarget);

                        drawPosition.y += (sameAmount[(int)count[i]]) * (flag.texture.height * 0.125f) * Unit.y * 0.25f;

                        flag.frame = team[i];
                        Vec2 flagScale = flag.scale;
                        Graphics.Draw(flag, drawPosition.x, drawPosition.y);

                        flag.color = Color.Black;
                        flag.scale *= 1.1f;
                        Graphics.Draw(flag, drawPosition.x, drawPosition.y);

                        flag.color = Color.White;
                        flag.scale = flagScale;

                        //Restoring position for next flag
                        drawPosition = originalPosition;
                        sameAmount[(int)count[i]]++;
                    }
                }
            }
        }
    }
}
