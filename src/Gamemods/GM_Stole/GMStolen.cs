using System;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode ST")]
    public class GM_STOLEN : Thing
    {
        private SpriteMap _sprite;
        public GMTimer _timer;

        public bool Activate = false;
        public float time = 90f;
        public bool ctWins = false;
        public bool tWins = false;
        public string _string;
        public int contestedSafes;
        public bool init = false;

        public EditorProperty<float> RoundTime;
        public EditorProperty<int> SafesToWin;
        public EditorProperty<bool> Revive;

        public StateBinding _time = new StateBinding("time", -1, false, false);
        public StateBinding _activate = new StateBinding("Activate", -1, false, false);
        public StateBinding _ctWinBinding = new StateBinding("ctWins", -1, false, false);
        public StateBinding _tWinBinding = new StateBinding("tWins", -1, false, false);

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
            SafesToWin = new EditorProperty<int>(3, this, 1, 10, 1, null) { name = "Goal points"};
            Revive = new EditorProperty<bool>(false);

            _editorName = "GM Thiefs";
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
            if (_timer != null)
            {
                if (time > 0f)
                {
                    if (_timer != null)
                    {
                        _timer.time = time;
                        Fondle(_timer);
                    }
                    _string = Convert.ToString(time);
                    time -= 0.0166666f;
                }
                _timer.progress = (float)contestedSafes;
            }
            foreach (Fuse_armor armor in Level.current.things[typeof(Fuse_armor)]) //after getting a hit by bullet in armor duck will instantly respawn at respawn point
            {
                if (Revive == true)
                {
                    if (armor.owner != null)
                    {
                        Duck d = armor.owner as Duck;
                        d.doThrow = true;
                    }
                }
            }
            if (init == false)
            {
                init = true;
                time = RoundTime;
            }
            if ((time >= 14.97f && time < 15f))
            {
                SFX.Play(GetPath("15sec.wav"), 1f, 0f, 0f, false);
            }
            if ((time >= 9.97f && time < 10f))
            {
                SFX.Play(GetPath("10sec.wav"), 1f, 0f, 0f, false);
            }
            if (((time >= 4.97f && time < 5f) || (time >= 3.97f && time < 4f) || (time >= 2.97f && time < 3f) || (time >= 1.97f && time < 2f) || (time >= 0.97f && time < 1f)))
            {
                SFX.Play(GetPath("LastSec.wav"), 1f, 0f, 0f, false);
            }
            if (!(Level.current is Editor))
            {
                if (_timer == null && !(Level.current is Editor))
                {
                    _timer = new GMTimer(position.x, position.y - 16f)
                    {
                        anchor = this,
                        depth = 0.95f,
                        progressBar = true,
                        progressBarType = ProgressBarType.KeyPoint,
                        progressTarget = SafesToWin
                    };
                    Level.Add(_timer);
                    Fondle(_timer);
                }
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
                if (time > 0f)
                {
                    if (_timer != null)
                    {
                        _timer.time = time;
                        Fondle(_timer);
                    }
                    _string = Convert.ToString(time);
                    time -= 0.0166666f;
                }
                if (ctWins == false && tWins == false && contestedSafes >= SafesToWin)
                {
                    TerroristWin();
                    tWins = true;
                    SFX.Play(GetPath("GameEnd.wav"), 1f, 0f, 0f, false);
                }
                else if (contestedSafes < SafesToWin && time <= 0f && ctWins == false && tWins == false)
                {
                    CounterTerroristWin();
                    ctWins = true;
                    SFX.Play(GetPath("GameEnd.wav"), 1f, 0f, 0f, false);
                }
            }
            if (ctWins == true)
            {
                CounterTerroristWin();
            }
            else if (tWins == true)
            {
                TerroristWin();
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
    }
}
