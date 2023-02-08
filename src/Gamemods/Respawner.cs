using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM")]
    //[BaggedProperty("previewPriority", true)]
    public class TeamRespawner : Respawner
    {
        public Team dgTeam;
        public TeamRespawner(float xpos, float ypos) : base(xpos, ypos)
        {
            _editorName = "Team Respawner";
            editorTooltip = "Links nearest team to it, other respawners will teleport you here if you step on them";
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
