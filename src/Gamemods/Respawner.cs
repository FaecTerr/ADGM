using System.Linq;
using System.Collections.Generic;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM")]
    //[BaggedProperty("previewPriority", true)]
    public class TeamRespawner : Respawner
    {
        public Team dgTeam;
        public bool accesible;
        public int sequenceOrder;
        
        public TeamRespawner(float xpos, float ypos) : base(xpos, ypos)
        {
            /*collisionSize = new Vec2(16, 16);
            collisionOffset = -0.5f * collisionSize;

            graphic = new SpriteMap("respawner", 18, 10, false);
            center = new Vec2(9f, 5f);
            */
            _editorName = "Team Respawner";
            editorTooltip = "Links nearest team to it, other respawners will teleport you here if you step on them";
        }
        public static TeamRespawner GetRespawner(Duck duck)
        {
            List<Thing> list = Level.current.things[typeof(TeamRespawner)].ToList();
            if(list.Count > 1)
            {
                List<Thing> respawners = Level.current.things[typeof(Respawner)].ToList();
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
                    return (TeamRespawner)prev[Rando.Int(list.Count - 1)];
                }
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

            if (dgTeam == null)
            {
                if (Level.current.things[typeof(Duck)].Count() > 1 && Level.Nearest<Duck>(position).team != null)
                {
                    dgTeam = Level.Nearest<Duck>(position).team;
                }
            }
            else 
            {
                /*foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d.team == dgTeam)
                    {
                        d.respawnPos = position;
                    }
                    else
                    {
                        
                    }
                } */
                foreach (Duck d in Level.CheckRectAll<Duck>(topLeft + new Vec2(0, -18f), bottomRight))
                {
                    if (d.team != dgTeam)
                    {
                        foreach (TeamRespawner respawner in Level.current.things[typeof(TeamRespawner)])
                        {
                            if (d.team == respawner.dgTeam)
                            {
                                d.position = respawner.position + new Vec2(0, -16f);
                            }
                        }
                    }
                }
            }
        }
    }
}
