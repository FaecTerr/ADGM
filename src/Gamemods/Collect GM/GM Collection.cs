using System;
using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Collect")]
    public class GM_Collection : Thing
    {
        public GMTimer _timer;
        private SpriteMap _sprite;
        public CollectBase _cb;
        private CollectingUI collecting;

        public bool Activate = false;
        public bool winnerDefined;
        public string _string;

        public bool[] givePoint = new bool[8];
        public bool[] giveExtra = new bool[8];
        public float[] teamCollectibles = new float[8];
        public bool init = false;

        public EditorProperty<float> maxPoints;
        public EditorProperty<float> RoundTime; 
        public EditorProperty<float> AdditionalPoints;

        Sprite off = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusOFF.png"));
        Sprite on = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusON.png"));
        Sprite warn = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusWARN.png"));

        public GM_Collection(float xval, float yval, CollectBase cb = null, GMTimer gmt = null) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-7f, -7f);
            collisionSize = new Vec2(14f, 14f);
            graphic = _sprite;
            _visibleInGame = false;
            _cb = cb;
            _timer = gmt;
            maxPoints = new EditorProperty<float>(100, this, 10f, 100f, 1f, null, false, false) { name = "Goal points"};
            RoundTime = new EditorProperty<float>(120f, this, 20f, 180f, 1f, null);
            AdditionalPoints = new EditorProperty<float>(0, this, 0, 100, 1) { name = "Extra points", _tooltip = "Amount of points team will get after opening box on their base"};

            _editorName = "GM Collection";
            editorTooltip = "Req: 'Collection base' for both teams, Collectibles infinite source";
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
                if (collecting == null)
                {
                    collecting = new CollectingUI();
                    Level.Add(collecting);
                    collecting.pointsTarget = maxPoints;
                }
                init = true;
            }

            if (_timer == null && !(Level.current is Editor))
            {
                _timer = (new GMTimer(position.x, position.y - 16f)
                {
                    anchor = this,
                    depth = 0.95f,
                    time = RoundTime,
                });
                if(isServerForObject)
                    Level.Add(_timer);
                _timer.Resume();
            }

            if (_timer != null)
            {
                //Win conditions
                if (_timer.time <= 0f)
                {
                    if (!winnerDefined)
                    {
                        if (teamCollectibles[0] > teamCollectibles[1])
                        {
                            CounterTerroristWin();
                            winnerDefined = true;
                        }
                        else if (teamCollectibles[1] > teamCollectibles[0])
                        {
                            TerroristWin();
                            winnerDefined = true;
                        }
                        else
                        {
                            CounterTerroristWin();
                            TerroristWin();
                        }
                    }
                }
                //Points system
                for (int i = 0; i < teamCollectibles.Length; i++)
                {
                    if (givePoint[i])
                    {
                        teamCollectibles[i] += 0.01666666f;
                        givePoint[i] = false;
                    }
                    if (giveExtra[i])
                    {
                        if (collecting != null)
                        {
                            teamCollectibles[i] += AdditionalPoints;
                        }
                        giveExtra[i] = false;
                    }
                    if (teamCollectibles[0] >= maxPoints && !winnerDefined)
                    {
                        CounterTerroristWin();
                        _timer.time = 0f;
                        winnerDefined = true;
                    }
                    if (teamCollectibles[1] >= maxPoints && !winnerDefined)
                    {
                        TerroristWin();
                        _timer.time = 0f;
                        winnerDefined = true;
                    }
                    if (collecting != null)
                    {
                        if (teamCollectibles[i] > maxPoints)
                        {
                            teamCollectibles[i] = maxPoints;
                        }
                        collecting.teamPoints[i] = teamCollectibles[i];
                    }
                }

                if (_timer.time > 0f)
                {
                    //That is super ugly piece of code tbh
                    foreach (CollectBase collectb in Level.current.things[typeof(CollectBase)])
                    {
                        foreach (Collectible collectible in Level.current.things[typeof(Collectible)])
                        {
                            foreach (Duck d in Level.CheckRectAll<Duck>(collectb.topLeft, collectb.bottomRight))
                            {
                                if (d != null)
                                {
                                    if (d.inputProfile.Released("SHOOT") && d.holdObject == collectible)
                                    {
                                        collectible.SpawnItem();
                                        giveExtra[collectb.team] = true;
                                    }
                                }
                            }
                        }
                        foreach (Collectible collectible in Level.CheckRectAll<Collectible>(collectb.topLeft, collectb.bottomRight))
                        {
                            if (collectible != null)
                            {
                                givePoint[collectb.team] = true;
                            }
                        }
                    }
                }
            }
        }
        public void Winner(Profile[] winners)
        {
            foreach (Profile profile in winners)
            {
                if (!GameMode.lastWinners.Contains(profile))
                {
                    GameMode.lastWinners.Add(profile);
                }
            }
        }
        public void TeamWin(Team t)
        {
            winnerDefined = true;
            Profile[] winners = new Profile[t.activeProfiles.Count];

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
        public void TerroristWin()
        {
            foreach (CTEquipment cte in Level.current.things[typeof(CTEquipment)])
            {
                if (cte != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(cte) == true)
                            {
                                d.Kill(new DTImpact(this));
                            }
                        }
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
            foreach (TEquipment te in Level.current.things[typeof(TEquipment)])
            {
                if (te != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(te) == true)
                            {
                                d.Kill(new DTImpact(this));
                            }
                        }
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

                int[] teamBases = new int[8];
                int bases = 0;

                foreach(CollectBase cbase in Level.current.things[typeof(CollectBase)])
                {
                    teamBases[cbase.team.value]++;
                }
                for (int i = 0; i < 8; i++)
                {
                    if(teamBases[i] > 0)
                    {
                        bases++;
                    }
                }

                text = "Collect bases placed (" + bases + " / " + "2" + ")";
                Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                if (bases > 0)
                {
                    if(bases < 2)
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
                    if (taggedTeams < 2)
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

                bool infiniteSource = false;
                foreach (ItemSpawner spawner in Level.current.things[typeof(ItemSpawner)])
                {
                    if(Editor.GetThing(spawner.contains) is Collectible && spawner.spawnNum < 0)
                    {
                        infiniteSource = true;
                    }
                }
                foreach(ItemBox box in Level.current.things[typeof(ItemBox)])
                {
                    if(!(box is ItemBoxRandom) && !(box is ItemBoxOneTime) && Editor.GetThing(box.contains) is Collectible)
                    {
                        infiniteSource = true;
                    }
                }

                if (!infiniteSource)
                {
                    row++;
                    text = "No infinite source of collectibles";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
            }
        }
    }
}
