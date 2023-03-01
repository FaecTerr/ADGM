using System.Linq;
using System.Collections.Generic;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM")]
    //[BaggedProperty("previewPriority", true)]
    public class TeamRespawner : Thing
    {
        public Team dgTeam;
        public bool accesible = true;
        public int sequenceOrder;
        
        public TeamRespawner(float xpos, float ypos) : base(xpos, ypos)
        {
            collisionSize = new Vec2(16, 16);
            collisionOffset = -0.5f * collisionSize;

            graphic = new SpriteMap("respawner", 18, 10, false);
            center = new Vec2(9f, 5f);

            layer = Layer.Foreground;

            _editorName = "Team Respawner";
            editorTooltip = "Links nearest team to it, other respawners will teleport you here if you step on them";
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
                    DevConsole.Log("2");
                    return (TeamRespawner)prev[Rando.Int(prev.Count - 1)];
                }
                DevConsole.Log("3");
                return (TeamRespawner)respawners[Rando.Int(respawners.Count - 1)];
            }
            
            if(list.Count > 0)
            {
                return (TeamRespawner)list[Rando.Int(list.Count - 1)];
            }

            return null;
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
                //Graphics.DrawString(dgTeam.currentDisplayName, position, Color.White);
            }
        }
    }
}
