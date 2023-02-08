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
        public float time = 120f;
        public bool winnerDefined;
        public string _string;

        public bool[] givePoint = new bool[8];
        public bool[] giveExtra = new bool[8];
        public float[] teamCollectibles = new float[8];
        public bool init = false;

        public EditorProperty<float> maxPoints;
        public EditorProperty<float> RoundTime; 
        public EditorProperty<float> AdditionalPoints;
        public EditorProperty<bool> NewTeamSystem;
        public EditorProperty<bool> Revive;

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
            Revive = new EditorProperty<bool>(true);
            NewTeamSystem = new EditorProperty<bool>(false);
            AdditionalPoints = new EditorProperty<float>(0, this, 0, 100, 1) { name = "Extra points", _tooltip = "Amount of points team will get after opening box on their base"};

            _editorName = "GM Collection";
            editorTooltip = "Req: 'Collection base' for both teams, Collectibles infinite source";
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
                time = RoundTime;
            }

            if (_timer == null && !(Level.current is Editor) && time > 0f)
            {
                _timer = (new GMTimer(position.x, position.y - 16f)
                {
                    anchor = this,
                    depth = 0.95f
                });
                if(isServerForObject)
                    Level.Add(_timer);
            }

            if (time <= 0f)
            {
                //Win conditions
                if (!winnerDefined)
                {
                    if (!NewTeamSystem)
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
                    else
                    {
                        for (int i = 0; i < teamCollectibles.Length; i++)
                        {
                            if (teamCollectibles[i] >= maxPoints && !winnerDefined)
                            {
                                
                            }
                        }
                    }
                }
            }
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
                if (!NewTeamSystem)
                {
                    if (teamCollectibles[0] >= maxPoints && !winnerDefined)
                    {
                        CounterTerroristWin();
                        time = 0f;
                        winnerDefined = true;
                    }
                    if (teamCollectibles[1] >= maxPoints && !winnerDefined)
                    {
                        TerroristWin();
                        time = 0f;
                        winnerDefined = true;
                    }
                }
                else
                {
                    if (teamCollectibles[i] >= maxPoints && !winnerDefined)
                    {
                        //TODO
                        //Flexible team win defining
                    }
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

            if (time > 0f)
            {
                time -= 0.0166666f;
                if (_timer != null)
                {
                    _timer.time = time;
                }

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
    }
}
