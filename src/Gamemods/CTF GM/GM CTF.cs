using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode CTF")]
    public class GM_CTF : Thing
    {
        private SpriteMap _sprite; 
        public GMTimer _timer;
        private TeamCounter teamCounter = null;

        public bool Activate = false;
        public bool winnerDefined;
        public bool init;

        public int[] scores = new int[8];

        public EditorProperty<float> maxPoints;
        public EditorProperty<float> RoundTime;

        Sprite off = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusOFF.png"));
        Sprite on = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusON.png"));
        Sprite warn = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusWARN.png"));
        public GM_CTF(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-7f, -7f);
            collisionSize = new Vec2(14f, 14f);
            graphic = _sprite;
            _visibleInGame = false;

            maxPoints = new EditorProperty<float>(3, this, 1f, 8f, 1f, null, false, false) { name = "Goal points"};
            RoundTime = new EditorProperty<float>(90f, this, 30f, 180f, 1f, null, false, false);

            _editorName = "GM CTF";
            editorTooltip = "Doesn't need in-mod equipment, team registering happens automatically";
            maxPlaceable = 1;

            off.CenterOrigin();
            on.CenterOrigin();
            warn.CenterOrigin();
        }

        public override void Update()
        {
            base.Update();
            if (init == false)
            {
                init = true;
                if (_timer == null && !(Level.current is Editor)) //adding timer to level
                {
                    _timer = new GMTimer(position.x, position.y - 16f)
                    {
                        anchor = this,
                        depth = 0.95f,
                        progressBar = true,
                        progressBarType = ProgressBarType.ScoreCompetition,
                        progressTarget = maxPoints,
                        time = RoundTime
                    };
                    Level.Add(_timer);
                    _timer.Resume();
                }

                if (_timer != null && teamCounter == null)
                {
                    teamCounter = new TeamCounter(position.x, position.y - 16f, false)
                    {
                        anchor = this,
                        depth = 0.95f,
                        Timer = _timer
                    };
                    for (int i = 0; i < Teams.active.Count; i++)
                    {
                        teamCounter.team[i] = i;
                    }
                    Level.Add(teamCounter);
                }
            }

            if (Level.current.things[typeof(TeamCounter)].Count() > 1)
            {
                foreach (TeamCounter counter in Level.current.things[typeof(TeamCounter)])
                {
                    if(teamCounter == null)
                    {
                        teamCounter = counter;
                    }
                    if (counter != teamCounter)
                    {
                        Level.Remove(counter);
                    }
                }
            }

            if (_timer != null)
            {
                if (_timer.time > 0f) //Timer update and win conditions
                {
                    _timer.time -= 0.0166666f;

                    for (int i = 0; i < Math.Min(scores.Length, Teams.active.Count); i++)
                    {
                        if (scores[i] >= maxPoints && !winnerDefined)
                        {
                            TeamWin(Teams.active[i]);
                            _timer.time = 0;
                        }
                        teamCounter.count[i] = scores[i];
                    }
                }
                else if (!winnerDefined) //Win Conditions after time is up
                {
                    int currentMaxPoint = scores[0];
                    List<Team> winners = new List<Team>();
                    for (int i = 0; i < scores.Length; i++)
                    {
                        if (scores[i] > currentMaxPoint)
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
            }
            foreach (FlagBase flagB in Level.current.things[typeof(FlagBase)]) //Counting of flag points
            {
                if (flagB != null)
                {
                    if(flagB._flag != null && flagB.getPoint && flagB.Team < scores.Length)
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
            foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
            {
                Level.Remove(tag);
            }
            foreach (TeamRespawner respawner in Level.current.things[typeof(TeamRespawner)])
            {
                Level.Remove(respawner);
            }
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if((t == null || (t != null && !t.activeProfiles.Contains(d.profile))) && !d.dead)
                {
                    d.Kill(new DTImpact(this));
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (Level.current is Editor)
            {
                int row = 0;
                int yoffset = 16;
                int spriteYOffset = 4;
                int xoffset = 14;
                int yMove = 10;

                string text = "";

                float unit = Level.current.camera.size.x / 320 * 0.4f;

                off.scale = new Vec2(unit, unit) * 0.5f;
                on.scale = off.scale;
                warn.scale = off.scale;

                int[] teams = new int[8];
                int taggedTeams = 0; 
                int respawnTags = 0;
                foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
                {
                    if (tag.responsibleForRespawn)
                    {
                        respawnTags++;
                    }
                    teams[tag.tagID.value]++;
                }

                for (int i = 0; i < 8; i++)
                {
                    if(teams[i] > 0)
                    {
                        taggedTeams++;
                    }
                }
                int maxTeamsTarget = Math.Max(Level.current.things[typeof(FlagBase)].Count(), Math.Max(Level.current.things[typeof(TeamSpawn)].Count(), taggedTeams));

                text = "Flag Base placed";
                Graphics.DrawString(text + " (" + Level.current.things[typeof(FlagBase)].Count() + " / " + maxTeamsTarget + ")", Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                if (Level.current.things[typeof(FlagBase)].Count() > 0)
                {
                    if (Level.current.things[typeof(FlagBase)].Count() < maxTeamsTarget)
                    {
                        Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                    else
                    {
                        Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                }
                else
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                row++;
                text = "Team spawns placed";
                Graphics.DrawString(text + " (" + Level.current.things[typeof(TeamSpawn)].Count() + " / " + maxTeamsTarget + ")", Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                if (Level.current.things[typeof(TeamSpawn)].Count() > 0)
                {
                    if (Level.current.things[typeof(TeamSpawn)].Count() < maxTeamsTarget)
                    {
                        Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                    else
                    {
                        Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                }
                else
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                row++;
                text = "Force tags placed";
                Graphics.DrawString(text + " (" + taggedTeams + " / " + maxTeamsTarget + ")", Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                if (taggedTeams > 0)
                {
                    if (taggedTeams < maxTeamsTarget || taggedTeams > maxTeamsTarget)
                    {
                        Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                    else
                    {
                        Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                }
                else
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                bool respawners = Level.current.things[typeof(TeamRespawner)].Count() > 0;

                if (respawners)
                {
                    if (respawnTags < Level.current.things[typeof(TeamSpawn)].Count())
                    {
                        row++;
                        text = "Not enough ForceTags with 'Respawn' parameter";
                        Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                        Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                    if (respawnTags > Level.current.things[typeof(TeamSpawn)].Count())
                    {
                        row++;
                        text = "Too many ForceTags with 'Respawn' parameter";
                        Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                        Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                }
                if(respawnTags > Level.current.things[typeof(TeamRespawner)].Count())
                {

                    row++;
                    text = "Not enough respawners";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                if (maxTeamsTarget > 8)
                {
                    row++;
                    text = "Too many teams planned (Team respawns/Flag Bases)";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
                if (maxTeamsTarget < 2)
                {
                    row++;
                    text = "Not enough teams planned (Team respawns/Flag Bases/Tag IDs)";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
            }
        }
    }
}