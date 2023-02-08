namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode CP")]
    public class ContestZone : Thing
    {
        private SpriteMap _sprite;

        private bool up;
        private bool dow;
        private bool lef;
        private bool rig;
        public ContestZone(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/CP/ContestZone.png"), 16, 16, false);
            base.graphic = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/CP/ContestZone.png"), 16, 16, false);
            center = new Vec2(8f, 8f);
            _sprite.frame = 8;
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(16f, 16f);
            graphic = _sprite;
            depth = 0.4f;
            alpha = 0.6f;
            layer = Layer.Foreground;

        }
        public override void Update()
        {
            base.Update();
            ContestZone cup = Level.CheckLine<ContestZone>(position, position + new Vec2(0f, -16f), this);
            ContestZone cdow = Level.CheckLine<ContestZone>(position, position + new Vec2(0f, 16f), this);
            ContestZone clef = Level.CheckLine<ContestZone>(position, position + new Vec2(-16f, 0f), this);
            ContestZone crig = Level.CheckLine<ContestZone>(position, position + new Vec2(16f, 0f), this);
            foreach (Duck d in Level.CheckRectAll<Duck>(topLeft, bottomRight))
            {
                if (d != null)
                {
                    foreach (GM_CP gm in Level.current.things[typeof(GM_CP)])
                    {
                        if (gm != null)
                        {
                            if (d.HasEquipment(typeof(TEquipment)))
                            {
                                gm.contesting = true;
                            }
                            if (d.HasEquipment(typeof(CTEquipment)))
                            {
                                gm.uncontesting = true;
                            }
                        }
                    }
                }
            }
            if(cup != null)
            {
                up = true;
            }
            else
            {
                up = false;
            }
            if (cdow != null)
            {
                dow = true;
            }
            else
            {
                dow = false;
            }
            if (clef != null)
            {
                lef = true;
            }
            else
            {
                lef = false;
            }
            if (crig != null)
            {
                rig = true;
            }
            else
            {
                rig = false;
            }
            if(up == false && dow == false && lef == false && rig == false)
            {
                _sprite.frame = 8;
            }
            else if (up == true && dow == true && lef == true && rig == true)
            {
                _sprite.frame = 9;
            }
            else if(up == false)
            {
                if(dow == true && rig == true && lef == true)
                {
                    _sprite.frame = 6;
                }
                else if(rig == false && dow == true && lef == true)
                {
                    _sprite.frame = 1;
                }
                else if (rig == true && dow == true && lef == false)
                {
                    _sprite.frame = 0;
                }
            }
            else if(dow == false)
            {
                if (up == true && rig == true && lef == true)
                {
                    _sprite.frame = 4;
                }
                else if (rig == false && up == true && lef == true)
                {
                    _sprite.frame = 2;
                }
                else if (rig == true && up == true && lef == false)
                {
                    _sprite.frame = 3;
                }
            }
            else if(up == true && dow == true)
            {
                if(rig == true)
                {
                    _sprite.frame = 5;
                }
                else if(lef == true)
                {
                    _sprite.frame = 7;
                }
            }
        }
    }
}
