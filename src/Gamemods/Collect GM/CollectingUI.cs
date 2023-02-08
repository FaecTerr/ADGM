using System;

namespace DuckGame.C44P
{
    public class CollectingUI : Thing, IDrawToDifferentLayers
    {
        public float[] teamPoints = new float[8];
        public float pointsTarget;
        public int amountOfTeams = 2;

        public bool newTeamSystem;

        public override void Initialize()
        {
            base.Initialize();
            //TODO:
            //Calculation number of teams
        }

        public void OnDrawLayer(Layer pLayer)
        {
            if(pLayer == Layer.Foreground)
            {
                Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);
                Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
                Vec2 drawPosition = camPos + camSize * new Vec2(0.5f, 0.015f);

                Vec2 Unit = camSize / new Vec2(320, 180);
                drawPosition += new Vec2(0, 14f) * Unit;

                if (!newTeamSystem)
                {
                    Vec2 mergedBoxSize = new Vec2(112, 6);

                    //White box
                    Graphics.DrawRect(drawPosition + new Vec2(-mergedBoxSize.x * 0.5f - 0.5f, -mergedBoxSize.y * 0.5f - 0.5f) * Unit, drawPosition + new Vec2(mergedBoxSize.x * 0.5f + 0.5f, mergedBoxSize.y * 0.5f + 0.5f) * Unit, Color.White, 1f, false, 0.5f * Unit.x);
                    //Black outline
                    Graphics.DrawRect(drawPosition + new Vec2(-mergedBoxSize.x * 0.5f - 1f, -mergedBoxSize.y * 0.5f - 1f) * Unit, drawPosition + new Vec2(mergedBoxSize.x * 0.5f + 1f, mergedBoxSize.y * 0.5f + 1f) * Unit, Color.Black, 1f, false, 0.5f * Unit.x);
                    Graphics.DrawLine(drawPosition + new Vec2(0, -mergedBoxSize.y * 0.5f - 1) * Unit, drawPosition + new Vec2(0, mergedBoxSize.y * 0.5f + 1) * Unit, Color.Black, 1f * Unit.x, 1);


                    //First team score
                    if (teamPoints[0] > 0)
                    {
                        Graphics.DrawRect(drawPosition + new Vec2(-mergedBoxSize.x * 0.5f, -mergedBoxSize.y * 0.5f) * Unit, drawPosition + new Vec2(-mergedBoxSize.x * 0.5f + mergedBoxSize.x * 0.5f * (teamPoints[0] / pointsTarget), mergedBoxSize.y * 0.5f) * Unit, Color.DarkBlue, 0.95f, true);
                        Graphics.DrawRect(drawPosition + new Vec2(-mergedBoxSize.x * 0.5f + mergedBoxSize.x * 0.5f * (teamPoints[0] / pointsTarget) - 1, -mergedBoxSize.y * 0.5f) * Unit, drawPosition + new Vec2(-mergedBoxSize.x * 0.5f + mergedBoxSize.x * 0.5f * (teamPoints[0] / pointsTarget), mergedBoxSize.y * 0.5f) * Unit, Color.Blue, 0.98f, true);
                    }
                    //Second team score
                    if (teamPoints[1] > 0)
                    {
                        Graphics.DrawRect(drawPosition + new Vec2(mergedBoxSize.x * 0.5f - mergedBoxSize.x * 0.5f * (teamPoints[1] / pointsTarget), -mergedBoxSize.y * 0.5f) * Unit, drawPosition + new Vec2(mergedBoxSize.x * 0.5f, mergedBoxSize.y * 0.5f) * Unit, Color.DarkOrange, 0.95f, true);
                        Graphics.DrawRect(drawPosition + new Vec2(mergedBoxSize.x * 0.5f - mergedBoxSize.x * 0.5f * (teamPoints[1] / pointsTarget), -mergedBoxSize.y * 0.5f) * Unit, drawPosition + new Vec2(mergedBoxSize.x * 0.5f - mergedBoxSize.x * 0.5f * (teamPoints[1] / pointsTarget) - 1, mergedBoxSize.y * 0.5f) * Unit, Color.Orange, 0.98f, true);
                    }
                }
            }
        }
    }
}
