namespace DuckGame.C44P
{
    public class CTEquipment : Fuse_armor
    {
        public Defuser def;

        public SinWave _wave = 0.3f;
        private float time = 1f;
        public bool inBag;
        public bool drop = false;

        public StateBinding _bag = new StateBinding("inBag", -1, false, false);
        public StateBinding _def = new StateBinding("def", -1, false, false);
        public StateBinding _drop = new StateBinding("drop", -1, false, false);
        public CTEquipment(float xval, float yval) : base(xval, yval)
        {
            _pickupSprite = new SpriteMap(GetPath("Sprites/CTE"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/CTE"), 32, 32, false);
            center = new Vec2(16f, 16f);
            team = 1;
            base.graphic = _sprite;
            _wearOffset = new Vec2(1f, 1f);
            _sprite.AddAnimation("idle", 1f, true, new int[1]);
        }

        public virtual void Drop(Defuser defuser)
        {
            if(defuser != null)
            {
                if(defuser.owner != null)
                {
                    Duck duck = defuser.owner as Duck;
                    duck.doThrow = true;
                    defuser.position.x += offDir * 4;
                }
                else
                {
                    defuser.velocity = new Vec2(0f, 1f);
                }
                defuser.angleDegrees = 0f;
                defuser.anchor = null;
                defuser = null;
            }
        }
        public override void Update()
        {
            base.Update();
            if(drop)
            {
                if(def != null)
                {
                    Drop(def);
                    drop = false;
                    def = null;
                }
            }
            if(_equippedDuck != null)
            {         
                if(_equippedDuck.holdObject != null && _equippedDuck.holdObject is TEquipment)
                {
                    _equippedDuck.doThrow = true;
                }
                if (def != null)
                {
                    def.position = _equippedDuck.position + new Vec2(-4f * _equippedDuck.offDir, 2f);
                    if (_equippedDuck.ragdoll != null)
                    {
                        def.position = _equippedDuck.ragdoll.position;
                    }
                    def.angleDegrees = 90f;
                    if (_equippedDuck.holdObject == null && inBag)
                    {
                        drop = true;
                        inBag = false;
                    }
                }
                else if (_equippedDuck.holdObject != null)
                {
                    Defuser defuser = Level.CheckRect<Defuser>(_equippedDuck.topLeft, _equippedDuck.bottomRight, def);
                    if (defuser != null && defuser != _equippedDuck.holdObject)
                    {
                        def = defuser;
                        def.anchor = _equippedDuck;
                        inBag = true;
                    }
                }
            }
            if (prevOwner != null && prevOwner is Duck && equippedDuck == null && !(prevOwner as Duck).HasEquipment(typeof(CTEquipment)))
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
