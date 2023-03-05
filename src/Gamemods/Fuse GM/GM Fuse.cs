using System;
using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Fuse")]
    public class GM_Fuse : Thing
    {
        private SpriteMap _sprite;
        public GMTimer _timer;
        public C4 _c4;

        public bool Activate = false;
        public float time = 90f;
        public bool ctWins = false;
        public bool tWins = false;
        public string _string;
        public bool init = false;
        public bool planted;
        public bool defused = false;

        private EditorProperty<float> C4_xoffset;
        private EditorProperty<float> C4_yoffset;
        public EditorProperty<float> RoundTime;
        public EditorProperty<float> ExplosionTime;
        public EditorProperty<bool> PlantZones;

        public StateBinding _time = new StateBinding("time", -1, false, false);
        public StateBinding _def = new StateBinding("defused", -1, false, false);
        public StateBinding _c4Bind = new StateBinding("_c4", -1, false, false);
        public StateBinding _activate = new StateBinding("Activate", -1, false, false);
        public StateBinding _ctWinBinding = new StateBinding("ctWins", -1, false, false);
        public StateBinding _tWinBinding = new StateBinding("tWins", -1, false, false);

        Sprite off = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusOFF.png"));
        Sprite on = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusON.png"));
        Sprite warn = new Sprite(Mod.GetPath<C44P>("Sprites/Gamemods/StatusWARN.png"));

        public GM_Fuse(float xval, float yval, C4 c4, GMTimer gmt) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-7f, -7f);
            collisionSize = new Vec2(14f, 14f);
            graphic = _sprite;
            _visibleInGame = false;
            _c4 = c4;
            _timer = gmt;
            layer = Layer.Foreground;
            _string = Convert.ToString(time);
            C4_xoffset = new EditorProperty<float>(0f, this, -160f, 160f, 1f);
            C4_yoffset = new EditorProperty<float>(0f, this, -160f, 160f, 1f);
            RoundTime = new EditorProperty<float>(90f, this, 20f, 180f, 1f, null);
            ExplosionTime = new EditorProperty<float>(25f, this, 10f, 60f, 5f, null);
            PlantZones = new EditorProperty<bool>(true) { _tooltip = "Will C4 require being placed in 'plantzone' or not"};

            _editorName = "GM Fuse";
            editorTooltip = "Inactive bomb spawns instead of this block. Check block properties and decide if you want to place 'Plant zones' or 'Defuser' on the map.";
            maxPlaceable = 1;

            off.CenterOrigin();
            on.CenterOrigin();
            warn.CenterOrigin();
        }
        public override void Update()
        {
            base.Update();
            if (_c4 == null && !(Level.current is Editor) && isServerForObject)
            {
                _c4 = new C4(x + C4_xoffset, y + C4_yoffset);
                Level.Add(_c4);
            }
            if (init == false)
            {
                init = true;
                time = RoundTime;
                if(!(Level.current is Editor) && ExplosionTime <= 5f)
                {
                    ExplosionTime = 25f;
                }
            }
            if((time >= 14.97 && time < 15))
            {
                SFX.Play(GetPath("SFX/15sec.wav"), 1f, 0f, 0f, false);
            }
            if ((time >= 9.97 && time < 10))
            {
                SFX.Play(GetPath("SFX/10sec.wav"), 1f, 0f, 0f, false);
            }
            if ((time >= 4.97 && time < 5) || (time >= 3.97 && time < 4) || (time >= 2.97 && time < 3) || (time >= 1.97 && time < 2) || (time >= 0.97 && time < 1))
            {
                SFX.Play(GetPath("SFX/LastSec.wav"), 1f, 0f, 0f, false);
            }

            if (_c4 != null)
            {
                if (PlantZones == true)
                {
                    _c4.coZone = true;
                }
                if (_timer == null && !(Level.current is Editor)) 
                {
                    _timer = (new GMTimer(_c4.position.x, _c4.position.y - 16f)
                    {
                        anchor = _c4,
                        depth = 0.95f
                    });
                    Level.Add(_timer);
                    Fondle(_timer);
                }
                foreach(GMTimer gm in Level.current.things[typeof(GMTimer)])
                {
                    if(gm != null && _timer != null)
                    {
                        if(gm != _timer)
                        {
                            Level.Remove(gm);
                        }
                    }
                }
            }
            if(ctWins)
            {
                CounterTerroristWin();
            }
            else if(tWins)
            {
                TerroristWin();
            }

            if (_c4 != null && !(Level.current is Editor))
            {
                if (_c4.planted)
                {
                    planted = true;
                    _timer.subtext = "Planted";
                }
                if (!planted)
                {
                    if (_timer != null)
                    {
                        _timer.str = "";
                    }
                }
                if (_c4.defused)
                {
                    defused = true;
                    Fondle(_timer);
                    Level.Remove(_timer);
                }
                if (defused)
                {
                    _c4.defused = true;
                }
                if (time > 0f)
                {
                    if (_timer != null)
                    {
                        _timer.time = time;
                        Fondle(_timer);
                    }
                    _string = Convert.ToString(time);
                    time -= 0.0166666f;

                    int cteCount = 0;
                    int teCount = 0;
                    foreach (CTEquipment cte in Level.current.things[typeof(CTEquipment)])
                    {
                        if (cte.equippedDuck != null && !cte.equippedDuck.dead)
                        {
                            cteCount++;
                        }
                    }
                    foreach (TEquipment te in Level.current.things[typeof(TEquipment)])
                    {
                        if (te.equippedDuck != null && !te.equippedDuck.dead)
                        {
                            teCount++;
                        }
                    }

                    if (time > 0.01f)
                    {
                        if (cteCount == 0)
                        {
                            time = 0.01f;
                        }
                        if (cteCount + teCount == 0)
                        {
                            time = 0.01f;
                        }
                        if(!planted && teCount == 0)
                        {
                            time = 0.01f;
                        }
                    }
                    GameMode mode = null;
                    if (Level.current is GameLevel)
                    {
                        mode = (GameMode)typeof(GameLevel).GetField("_mode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Level.current);
                    }
                    if (mode != null)
                    {
                        typeof(GameMode).GetField("_roundEndWait", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(mode, (float)(1 + 0.005f * 60 * time));
                        //float value = (float)typeof(GameMode).GetField("_roundEndWait", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(mode);
                        //DevConsole.Log(Convert.ToString(value));
                    }
                }
                if (_c4.planted && !Activate)
                {
                    time = ExplosionTime;
                    Activate = true;
                    _c4.C4timer = ExplosionTime;
                    GameMode mode = null;
                    if (Level.current is GameLevel)
                    {
                        mode = (GameMode)typeof(GameLevel).GetField("_mode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Level.current);
                    }
                    if (mode != null)
                    {
                        typeof(GameMode).GetField("_roundEndWait", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(mode, (float)(1 + 0.005f * 60 * ExplosionTime));
                        //float value = (float)typeof(GameMode).GetField("_roundEndWait", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(mode);
                        //DevConsole.Log(Convert.ToString(value));
                    }
                }
                if (_c4.planted && time <= 0f && !ctWins && !tWins)
                {
                    TerroristWin();
                    tWins = true;
                    SFX.Play(GetPath("SFX/GameEnd.wav"), 1f, 0f, 0f, false);
                }
                else if (_c4.planted == false && time <= 0f && !ctWins && tWins)
                {
                    CounterTerroristWin();
                    ctWins = true;
                    SFX.Play(GetPath("SFX/GameEnd.wav"), 1f, 0f, 0f, false);
                }
                if (_c4.defused == true && !ctWins && !tWins)
                {
                    CounterTerroristWin();
                    ctWins = true;
                    if(time > 0.01f)
                    {
                        time = 0.01f;
                    }
                    SFX.Play(GetPath("SFX/GameEnd.wav"), 1f, 0f, 0f, false);
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

        public void TeamWin(Team t)
        {
            foreach (Profile profile in DuckNetwork.profiles)
            {
                if (profile.team == t && !GameMode.lastWinners.Contains(profile))
                {
                    GameMode.lastWinners.Add(profile);
                }
            }
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if ((t == null || (t != null && !t.activeProfiles.Contains(d.profile))) && !d.dead)
                {
                    //d.Kill(new DTImpact(this));
                }
            }
            foreach (TeamRespawner respawner in Level.current.things[typeof(TeamRespawner)])
            {
                Level.Remove(respawner);
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (Level.current is Editor)
            {
                bool condition1 = false;
                bool condition2 = false;
                bool isTherePlantZones = false;
                bool nonGM_C4 = false;
                foreach (Equipper equipper in Level.current.things[typeof(Equipper)])
                {
                    if(equipper.GetContainedInstance() is CTEquipment)
                    {
                        condition1 = true;
                    }
                    if (equipper.GetContainedInstance() is TEquipment)
                    {
                        condition2 = true;
                    }
                }

                if (Level.current.things[typeof(C4)].Count() > 0)
                {
                    nonGM_C4 = true;
                }

                int row = 0;
                int yoffset = 16;
                int spriteYOffset = 4;
                int xoffset = 14;
                int yMove = 10;

                
                float unit = Level.current.camera.size.x / 320 * 0.4f;

                off.scale = new Vec2(unit, unit) * 0.5f;
                on.scale = off.scale;
                warn.scale = off.scale;

                string text = "CT Armor equipper";
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

                if (PlantZones)
                {
                    if(Level.current.things[typeof(PlantZone)].Count() > 0)
                    {
                        isTherePlantZones = true;
                    }

                    row++;
                    text = "Plant zone setted up";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);

                    if (isTherePlantZones)
                    {
                        Graphics.Draw(on, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                    else
                    {
                        Graphics.Draw(off, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                    }
                }
                if(Level.current.things[typeof(Defuser)].Count() == 0)
                {
                    row++;
                    text = "Defuser not placed";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
                if (nonGM_C4)
                {
                    row++;
                    text = "Non-GM C4 placed";
                    Graphics.DrawString(text, Level.current.camera.position + new Vec2(xoffset, row * yMove + yoffset) * unit, Color.White, depth, null, unit);
                    Graphics.Draw(warn, Level.current.camera.position.x + (xoffset * 0.5f) * unit, Level.current.camera.position.y + (row * yMove + yoffset + spriteYOffset) * unit);
                }
            }
        }
    }
}
