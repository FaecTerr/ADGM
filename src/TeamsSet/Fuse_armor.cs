using System;

namespace DuckGame.C44P
{
    public class Fuse_armor : Equipment
    {
        public Duck d;
        protected SpriteMap _sprite;
        protected Sprite _pickupSprite;

        public bool reskin;
        public int team;

        public Fuse_armor(float xval, float yval) : base(xval, yval)
        {
            thickness = 10f;
            _equippedDepth = 4;
            center = new Vec2(16, 16);
            collisionOffset = new Vec2(-12f, -12f);
            collisionSize = new Vec2(24f, 42f);
            _equippedCollisionOffset = new Vec2(-12f, -12f);
            _equippedCollisionSize = new Vec2(24f, 24f);
            _equippedDepth = 3;
            team = 0;
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            return false;
        }

        public Sprite pickupSprite
        {
            get
            {
                return _pickupSprite;
            }
            set
            {
                _pickupSprite = value;
            }
        }

        public override void Update()
        {
            if (owner != null)
            {
                Duck d = owner as Duck;
                collisionSize = d.collisionSize;
                collisionOffset = d.collisionOffset;

                base.Update();
            }
            else
            {
                if(d != null && _equippedDuck == null && !d.dead)
                {
                    d.Equip(this);
                    foreach(Duck du in Level.current.things[typeof(Duck)])
                    {
                        if(du.profile.lastKnownName == d.profile.lastKnownName)
                        {
                            d = du;
                        }
                    }
                }
            }
        }
    }
}
