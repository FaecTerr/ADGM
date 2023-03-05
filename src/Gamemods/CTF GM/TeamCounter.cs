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

        public override void Update()
        {
            base.Update();
            if(Timer == null)
            {
                Level.Remove(this);
            }
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

                    int[] sameAmount = new int[8];

                    for (int i = 0; i < Teams.active.Count; i++)
                    {
                        Vec2 originalPosition = drawPosition;
                        drawPosition.x += -camSize.x * 0.15f + camSize.x * 0.3f * (count[i] / Timer.progressTarget);

                        drawPosition.y += (sameAmount[(int)count[i]]) * (flag.texture.height * 0.125f) * Unit.y * 0.25f;

                        flag.frame = team[i];
                        foreach (Flag f in Level.current.things[typeof(Flag)])
                        {
                            if(f.Team == i && f.replacedFrame >= 0)
                            {
                                flag.frame = f.replacedFrame;
                            }
                        }

                        flag.color = Color.White;
                        flag.scale = Unit * 0.25f;
                        flag.depth = 0.91f;
                        Graphics.Draw(flag, drawPosition.x, drawPosition.y);

                        flag.color = Color.Black;
                        flag.scale *= 1.1f;
                        flag.depth = 0.9f;
                        Graphics.Draw(flag, drawPosition.x, drawPosition.y);

                        //Restoring position for next flag
                        drawPosition = originalPosition;
                        sameAmount[(int)count[i]]++;
                    }
                }
            }
        }
    }
}
