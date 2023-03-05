using System;
using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode ST")]
    public class GM_STOLEN : Thing
    {
        private SpriteMap _sprite;
        public GMTimer _timer;

        public bool Activate = false;
        public bool winnerDefined;
        public int contestedSafes;
        public bool init = false;

        public EditorProperty<float> RoundTime;
        public EditorProperty<int> SafesToWin;
        public EditorProperty<int> BonusTime;

        public StateBinding _activate = new StateBinding("Activate", -1, false, false);
        public StateBinding _ctWinBinding = new StateBinding("ctWins", -1, false, false);
        public StateBinding _tWinBinding = new StateBinding("tWins", -1, false, false);

        Sprite off = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusOFF.png"));
        Sprite on = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusON.png"));
        Sprite warn = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusWARN.png"));

        public GM_STOLEN(float xval, float yval, GMTimer gmt) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-7f, -7f);
            collisionSize = new Vec2(14f, 14f);
            graphic = _sprite;
            _visibleInGame = false;
            _timer = gmt;
            layer = Layer.Foreground;

            RoundTime = new EditorProperty<float>(90f, this, 20f, 180f, 1f, null);
            SafesToWin = new EditorProperty<int>(3, this, 1, 10, 1, null) { name = "Goal points" };
            BonusTime = new EditorProperty<int>(0, this, 0, 90, 5);

            _editorName = "GM Theft";
            maxPlaceable = 1;

            off.CenterOrigin();
            on.CenterOrigin();
            warn.CenterOrigin();
        }
        public override void Update()
        {
            base.Update();
            contestedSafes = 0;
            foreach (ContestSafe cs in Level.current.things[typeof(ContestSafe)])
            {
                if (cs.contested == true)
                {
                    contestedSafes += 1;
                }
            }

            if (_timer == null && !(Level.current is Editor))
            {
                _timer = new GMTimer(position.x, position.y - 16f)
                {
                    anchor = this,
                    depth = 0.95f,
                    progressBar = true,
                    progressBarType = ProgressBarType.KeyPoint,
                    progressTarget = SafesToWin,
                    time = RoundTime
                };
                Level.Add(_timer);
                Fondle(_timer);
                _timer.Resume();
            }
            if (_timer != null)
            {
                if (_timer.time > 0f)
                {
                    _timer.time -= 0.0166666f;
                }
                _timer.progress = (float)contestedSafes;


                if (_timer.time >= 14.97f && _timer.time < 15f)
                {
                    SFX.Play(GetPath("15sec.wav"), 1f, 0f, 0f, false);
                }
                if (_timer.time >= 9.97f && _timer.time < 10f)
                {
                    SFX.Play(GetPath("10sec.wav"), 1f, 0f, 0f, false);
                }
                if (_timer.time % 1 >= 0.97f && (_timer.time + 1) % 1 < 1f && _timer.time < 5)
                {
                    SFX.Play(GetPath("LastSec.wav"), 1f, 0f, 0f, false);
                }
                if (!(Level.current is Editor))
                {
                    foreach (GMTimer gm in Level.current.things[typeof(GMTimer)])
                    {
                        if (gm != null && _timer != null)
                        {
                            if (gm != _timer)
                            {
                                Level.Remove(gm);
                            }
                        }
                    }
                    if (!winnerDefined && contestedSafes >= SafesToWin)
                    {
                        TerroristWin();
                        SFX.Play(GetPath("GameEnd.wav"), 1f, 0f, 0f, false);
                    }
                    else if (contestedSafes < SafesToWin && _timer.time <= 0f && !winnerDefined)
                    {
                        CounterTerroristWin();
                        SFX.Play(GetPath("GameEnd.wav"), 1f, 0f, 0f, false);
                    }
                }
            }
        }
        public void TerroristWin()
        {
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if (d != null)
                {
                    if (d._equipment != null && d.HasEquipment(typeof(CTEquipment)) == true)
                    {
                        d.Kill(new DTImpact(this));
                    }
                }
            }
            foreach (TEquipment te in Level.current.things[typeof(TEquipment)])
            {
                if (te.duck != null && te.duck.team != null)
                {
                    TeamWin(te.duck.team);
                    return;
                }
            }
        }
        public void CounterTerroristWin()
        {
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if (d != null)
                {
                    if (d._equipment != null && d.HasEquipment(typeof(TEquipment)) == true)
                    {
                        d.Kill(new DTImpact(this));
                    }
                }
            }
            foreach (CTEquipment cte in Level.current.things[typeof(CTEquipment)])
            {
                if (cte.duck != null && cte.duck.team != null)
                {
                    TeamWin(cte.duck.team);
                    return;
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
                if ((t == null || (t != null && !t.activeProfiles.Contains(d.profile))) && !d.dead)
                {
                    //d.Kill(new DTImpact(this));
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
                    if (teams[i] > 0)
                    {
                        taggedTeams++;
                    }
                }

                bool condition1 = false;
                bool condition2 = false;
                foreach (Equipper equipper in Level.current.things[typeof(Equipper)])
                {
                    if (equipper.GetContainedInstance() is CTEquipment)
                    {
                        condition1 = true;
                    }
                    if (equipper.GetContainedInstance() is TEquipment)
                    {
                        condition2 = true;
                    }
                }

                text = "Contest safes placed (" + Level.current.things[typeof(ContestSafe)].Count() + " / " + SafesToWin.value + ")";
                Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                if (Level.current.things[typeof(ContestSafe)].Count() < SafesToWin)
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
                else
                {
                    Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                row++;
                text = "CT Armor equipper";
                Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                if (condition1)
                {
                    Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
                else
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                row++;
                text = "T Armor equipper";
                Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                if (condition2)
                {
                    Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
                else
                {
                    Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }

                row++;
                text = "Team spawns placed";
                Graphics.DrawString(text + " (" + Level.current.things[typeof(TeamSpawn)].Count() + " / " + 2 + ")", Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                if (Level.current.things[typeof(TeamSpawn)].Count() > 0)
                {
                    if (Level.current.things[typeof(TeamSpawn)].Count() < 2)
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
                Graphics.DrawString(text + " (" + taggedTeams + " / " + 2 + ")", Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                if (taggedTeams > 0)
                {
                    if (taggedTeams < 2 || taggedTeams > 2)
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
                if (respawnTags > Level.current.things[typeof(TeamRespawner)].Count())
                {
                    row++;
                    text = "Not enough respawners";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
            }
        }
    }
}
