using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode CTF")]
    public class GM_CTF : Thing
    {
        private SpriteMap _sprite; 
        public GMTimer _timer;
        private TeamCounter teamCounter;

        public bool Activate = false;
        public float time = 90f;
        public string _string;
        public bool winnerDefined;
        public bool init;

        public int[] scores = new int[8];

        public EditorProperty<float> maxPoints;
        public EditorProperty<float> RoundTime;
        public EditorProperty<bool> Revive;

        public GM_CTF(float xval, float yval, GMTimer gmt = null) : base(xval, yval)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-7f, -7f);
            collisionSize = new Vec2(14f, 14f);
            graphic = _sprite;
            _visibleInGame = false;
            _timer = gmt;
            _string = Convert.ToString(time);
            maxPoints = new EditorProperty<float>(3, this, 1f, 8f, 1f, null, false, false) { name = "Goal points"};
            RoundTime = new EditorProperty<float>(90f, this, 30f, 180f, 1f, null, false, false);
            Revive = new EditorProperty<bool>(true);

            _editorName = "GM CTF";
            editorTooltip = "Doesn't need in-mod equipment, team registering happens automatically";
        }

        public override void Update()
        {
            base.Update();
            if (init == false)
            {
                init = true;
                time = RoundTime; 
                if (_timer == null && !(Level.current is Editor)) //adding timer to level
                {
                    _timer = new GMTimer(position.x, position.y - 16f)
                    {
                        anchor = this,
                        depth = 0.95f,
                        progressBar = true,
                        progressBarType = ProgressBarType.ScoreCompetition,
                        progressTarget = maxPoints
                    };
                    Level.Add(_timer);
                }
                
                /*foreach (Team t in Teams.active)
                {
                    foreach(Profile p in t.activeProfiles)
                    {
                        if(p.duck != null)
                        {
                            FlagBase f = Level.Nearest<FlagBase>(p.duck.position);
                            if(f != null)
                            {
                                f.ReassignTeam(t);
                            }
                        }
                    }
                }*/

                if (_timer != null)
                {
                    TeamCounter counter = new TeamCounter(position.x, position.y - 16f, false)
                    {
                        anchor = this,
                        depth = 0.95f,
                        Timer = _timer
                    };
                    teamCounter = counter;
                    for (int i = 0; i < Teams.active.Count; i++)
                    {
                        counter.team[i] = i;
                    }
                    Level.Add(counter);
                }
            }

            if (time > 0f) //Timer update and win conditions
            {
                time -= 0.0166666f;
                if (_timer != null)
                {
                    _timer.time = time;
                    Fondle(_timer);
                }

                for (int i = 0; i < Math.Min(scores.Length, Teams.active.Count); i++)
                {
                    if(scores[i] >= maxPoints && !winnerDefined)
                    {
                        TeamWin(Teams.active[i]);
                        time = 0;
                    }
                    teamCounter.count[i] = scores[i];
                }
            }
            if (time <= 0f && !winnerDefined) //Win Conditions after time is up
            {
                int currentMaxPoint = scores[0];
                List<Team> winners = new List<Team>();
                for (int i = 0; i < scores.Length; i++)
                {
                    if(scores[i] > currentMaxPoint)
                    {
                        currentMaxPoint = scores[i];
                    }                    
                }

                for (int i = 0; i < Math.Min(scores.Length, Teams.active.Count); i++)
                {
                    if (scores[i] == currentMaxPoint)
                    {
                        winners.Add(Teams.active[i]);
                    }
                }

                if (winners.Count == 1)
                {
                    TeamWin(winners[0]);
                }
                else
                {
                    TeamWin(null);
                }
            }
            foreach (FlagBase flagB in Level.current.things[typeof(FlagBase)]) //Counting of flag points
            {
                if (flagB != null)
                {
                    if(flagB._flag != null && flagB.getPoint == true && flagB.Team < scores.Length)
                    {
                        flagB.getPoint = false;
                        scores[flagB.Team]++;
                    }
                }
            }
        }
        public void TeamWin(Team t)
        {
            winnerDefined = true; 
            foreach (Profile profile in DuckNetwork.profiles)
            {
                if (profile.team == t && !GameMode.lastWinners.Contains(profile))
                {
                    GameMode.lastWinners.Add(profile);
                }
            }
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if((t == null || (t != null && !t.activeProfiles.Contains(d.profile))) && !d.dead)
                {
                    d.Kill(new DTImpact(this));
                }
            }
            foreach(TeamRespawner respawner in Level.current.things[typeof(TeamRespawner)])
            {
                Level.Remove(respawner);
            }
        }
    }
}