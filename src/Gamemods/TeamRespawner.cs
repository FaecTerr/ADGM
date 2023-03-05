using System;
using System.Linq;
using System.Collections.Generic;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM")]
    //[BaggedProperty("previewPriority", true)]
    public class TeamRespawner : Thing
    {
        public Team dgTeam;
        public SpriteMap _sprite;

        public bool accesible = true;
        public int sequenceOrder;

        public EditorProperty<bool> tpAccesible = new EditorProperty<bool>(true) { name = "Toggled" };

        private float _animate;

        public TeamRespawner(float xpos, float ypos) : base(xpos, ypos)
        {
            collisionSize = new Vec2(16, 8);
            collisionOffset = new Vec2(-8, -4);

            _sprite = new SpriteMap("respawner", 18, 10, false);
            graphic = _sprite;
            center = new Vec2(9f, 5f);

            _sprite.center = new Vec2(9, 8);
            layer = Layer.Foreground;

            _editorName = "Team Respawner";
            editorTooltip = "Links nearest team to it, other respawners will teleport you here if you step on them";

            hugWalls = WallHug.Floor;
        }
        public static TeamRespawner GetRespawner(Duck duck)
        {
            List<Thing> list = Level.current.things[typeof(TeamRespawner)].ToList();
            if(list.Count > 0)
            {
                List<Thing> respawners = Level.current.things[typeof(TeamRespawner)].ToList();
                List<Thing> remove = new List<Thing>();

                foreach(TeamRespawner respawner in respawners)
                {
                    if(respawner.dgTeam != duck.team)
                    {
                        remove.Add(respawner);
                    }
                }
                foreach (TeamRespawner improperRespawner in remove)
                {
                    respawners.Remove(improperRespawner);
                }
                remove.Clear();
                if(respawners.Count == 0)
                {
                    DevConsole.Log("1");
                    return (TeamRespawner)list[Rando.Int(list.Count - 1)];
                }

                List<Thing> prev = new List<Thing>(respawners);
                foreach (TeamRespawner respawner in respawners)
                {
                    if (!respawner.accesible)
                    {
                        remove.Add(respawner);
                    }
                }
                foreach (TeamRespawner improperRespawner in remove)
                {
                    respawners.Remove(improperRespawner);
                }
                remove.Clear();
                if (respawners.Count == 0)
                {
                    //DevConsole.Log("2");
                    return (TeamRespawner)prev[Rando.Int(prev.Count - 1)];
                }
                //DevConsole.Log("3");
                return (TeamRespawner)respawners[Rando.Int(respawners.Count - 1)];
            }
            
            if(list.Count > 0)
            {
                return (TeamRespawner)list[Rando.Int(list.Count - 1)];
            }

            return null;
        }

        public override void Initialize()
        {
            base.Initialize();
            accesible = tpAccesible;
        }
        public override void Update()
        {
            base.Update();

            if (dgTeam == null && Level.current.things[typeof(ForceTag)].Count() == 0)
            {
                if (Level.current.things[typeof(Duck)].Count() > 1 && Level.Nearest<Duck>(position).team != null)
                {
                    dgTeam = Level.Nearest<Duck>(position).team;
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if(dgTeam != null)
            {
                if (accesible)
                {
                    _animate += 0.05f; 
                    float y = base.y; 
                    int numLines = 6;
                    for (int i = 0; i < numLines; i++)
                    {
                        Vec2 linePos = new Vec2(x - 6f, base.y - i * 4f - _animate % 1f * 4f);
                        float dist = 1f - (base.y - linePos.y) / 24f;
                        float thick = dist * 3f;
                        linePos.y += thick / 2f;
                        Graphics.DrawLine(linePos, linePos + new Vec2(12f, 0f), Colors.DGBlue * (dist * 0.8f), thick, -0.75f);
                    }
                    Vec2 noiseSize = new Vec2(7f, 8f);
                    Vec2 noisePos = position + new Vec2(-7f, -24f);
                    int j = 0;
                    while (j < noiseSize.x * noiseSize.y)
                    {
                        Vec2 pos = new Vec2(((int)(j % noiseSize.x)), ((int)(j / noiseSize.y)));
                        float fallSpeedMult = (Noise.Generate(pos.x * 32f, 0f) + 1f) / 2f * 1.5f + 0.1f;
                        float noiseOffsetMult = ((int)(_animate * fallSpeedMult / 1f));
                        float noiseOffsetY = _animate * 0.1f - noiseOffsetMult;
                        float noise = Noise.Generate(pos.x + 100f, (pos.y + 100f - noiseOffsetY) * 0.5f);
                        if (noise > 0.25f)
                        {
                            pos.y -= _animate * fallSpeedMult % 1f;
                            float edge = 1f - Math.Abs((noiseSize.x / 2f - pos.x) / noiseSize.x * 2f);
                            float a = (noise - 0.25f) / 0.75f * edge * Math.Max(0f, Math.Min((pos.y / noiseSize.y - 0.1f) * 2f, 1f));
                            pos *= 2f;
                            pos.y *= 2f;
                            Graphics.DrawRect(pos + noisePos, pos + noisePos + new Vec2(1f, 1f), Color.White * a, -0.5f, true, 1f);
                        }
                        j++;
                    }
                }
            }
        }
    }
}
