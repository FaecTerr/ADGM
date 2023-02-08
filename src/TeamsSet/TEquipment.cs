namespace DuckGame.C44P
{
    public class TEquipment : Fuse_armor
    {
        private float time = 1f;
        public SinWave _wave = 0.3f;
        public C4 c4;
        public bool inBag;
        public bool drop = false;
        public StateBinding _bag = new StateBinding("inBag", -1, false, false);
        public StateBinding _c4 = new StateBinding("c4", -1, false, false);
        public StateBinding _drop = new StateBinding("drop", -1, false, false);
        public TEquipment(float xval, float yval) : base(xval, yval)
        {
            _pickupSprite = new SpriteMap(GetPath("Sprites/TE"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/TE"), 32, 32, false);
            center = new Vec2(16f, 16f);
            team = 2;
            base.graphic = _sprite;
            _wearOffset = new Vec2(1f, 1f);
            _sprite.AddAnimation("idle", 1f, true, new int[1]);
        }

        public virtual void Drop(C4 c)
        {
            if (c != null)
            {
                if (c.owner != null)
                {
                    Duck duck = c.owner as Duck;
                    duck.doThrow = true;
                    c.position.x += owner.offDir * 4f;
                }
                else
                {
                    c.velocity = new Vec2(0f, 1f);
                }
                c.angleDegrees = 0f;
                c.anchor = null;
                c = null;
            }
        }

        public override void Update()
        {
            base.Update();
            if (drop)
            {
                if (c4 != null)
                {
                    Drop(c4);
                    drop = false;
                    c4 = null;
                }
            }
            if (_equippedDuck != null)
            {
                if (_equippedDuck.holdObject != null && _equippedDuck.holdObject is CTEquipment)
                {
                    _equippedDuck.doThrow = true;
                }
                if (c4 != null)
                {
                    c4.position = _equippedDuck.position + new Vec2(-4f * _equippedDuck.offDir, 0f);
                    if(_equippedDuck.ragdoll != null)
                    {
                        c4.position = _equippedDuck.ragdoll.position;
                    }
                    c4.angleDegrees = 90f;
                    if (_equippedDuck.holdObject == null && inBag)
                    {
                        drop = true;
                        inBag = false;
                    }
                }
                else if(_equippedDuck.holdObject != null)
                {
                    C4 c = Level.CheckRect<C4>(_equippedDuck.topLeft, _equippedDuck.bottomRight, c4);
                    if (c != null && c != _equippedDuck.holdObject)
                    {
                        if (c.planted == false)
                        {
                            c4 = c;
                            c4.anchor = _equippedDuck;
                            inBag = true;
                        }
                    }
                }
            }
            if(prevOwner != null && prevOwner is Duck && equippedDuck == null && !(prevOwner as Duck).HasEquipment(typeof(TEquipment)))
            {
                Duck d = prevOwner as Duck;
                if (d.dead == true)
                {
                    canPickUp = false;
                    alpha = 0;
                }
                else
                {
                    alpha = 1;
                    d.Equip(this);
                }
            }
        }
    }
}