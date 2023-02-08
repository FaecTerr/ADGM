using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.C44P
{
    public class Fuse_armor : Equipment
    {
        public Duck d;
        protected SpriteMap _sprite;
        protected Sprite _pickupSprite;

        public bool reskin;

        private float SFXplay;
        protected Vec2 _barrelOffsetTL = default(Vec2);
        public float punch;
        public int team;
        public int stepFrames = 20;

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

                //Walking sound idk for what, that left from past code
                foreach (Duck duck in Level.current.things[typeof(Duck)])
                {
                    if (duck != null)
                    {
                        if (duck.HasEquipment(typeof(Fuse_armor)))
                        {
                            Fuse_armor f = duck.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                            if (f != null)
                            {
                                if (duck.hSpeed != 0f && (duck.position - d.position).length < (480 + 80) && f.stepFrames <= 0 && duck.grounded && duck != d)
                                {
                                    int step = Rando.Int(1, 6);
                                    float silence = 0.75f;
                                    float pitch = 0.5f;
                                    f.stepFrames = 45;
                                    float panning = d.position.x - duck.position.x;
                                    if (Math.Abs(panning) <= 16f)
                                    {
                                        panning = 0;
                                    }
                                    if (Math.Abs(panning) <= 32f)
                                    {
                                        if (panning < 0)
                                        {
                                            panning = 0.35f;
                                        }
                                        else
                                        {
                                            panning = -0.35f;
                                        }
                                    }
                                    if (Math.Abs(panning) <= 48f)
                                    {
                                        if (panning < 0)
                                        {
                                            panning = 0.5f;
                                        }
                                        else
                                        {
                                            panning = -0.5f;
                                        }
                                    }
                                    else
                                    {
                                        if (panning < 0)
                                        {
                                            panning = 0.75f;
                                        }
                                        else
                                        {
                                            panning = -0.75f;
                                        }
                                    }
                                    if (Math.Abs(duck.hSpeed) > 2f)
                                    {
                                        silence = 1f;
                                        pitch = 1f;
                                        stepFrames = 20;
                                    }
                                    if (Math.Abs(duck.hSpeed) < 1f)
                                    {
                                        silence = 0.25f;
                                        pitch = 0f;
                                        stepFrames = 70;
                                    }
                                    float volume = (1f - ((duck.position - position).length / ((480 + 80)))) * silence;
                                    foreach (Block b in Level.CheckLineAll<Block>(duck.position, d.position))
                                    {
                                        volume *= 0.8f;
                                        pitch -= 0.5f;
                                        if (d.position.x > duck.position.x)
                                        {
                                            panning -= 0.1f;
                                        }
                                        else
                                        {
                                            panning += 0.1f;
                                        }
                                    }
                                    if (volume > 1)
                                    {
                                        volume = 1;
                                    }
                                    if (pitch < -1)
                                    {
                                        pitch = -1;
                                    }
                                    if (pitch > 1)
                                    {
                                        pitch = 1;
                                    }
                                    if (panning < -1)
                                    {
                                        panning = -1;
                                    }
                                    if (panning > 1)
                                    {
                                        panning = 1;
                                    }
                                    if (volume < 0)
                                    {
                                        volume = 0;
                                    }
                                    if (step == 1)
                                    {
                                        SFX.Play(GetPath("Step1.wav"), volume, pitch, panning);
                                    }
                                    if (step == 2)
                                    {
                                        SFX.Play(GetPath("Step2.wav"), volume, pitch, panning);
                                    }
                                    if (step == 3)
                                    {
                                        SFX.Play(GetPath("Step3.wav"), volume, pitch, panning);
                                    }
                                    if (step == 4)
                                    {
                                        SFX.Play(GetPath("Step4.wav"), volume, pitch, panning);
                                    }
                                    if (step == 5)
                                    {
                                        SFX.Play(GetPath("Step5.wav"), volume, pitch, panning);
                                    }
                                    if (step == 6)
                                    {
                                        SFX.Play(GetPath("Step6.wav"), volume, pitch, panning);
                                    }
                                }
                            }
                        }
                    }
                }

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
        
        public override void Draw()
        {
            base.Draw();
        }

        //Some vectors
        public Vec2 barrelOffset
        {
            get
            {
                return _barrelOffsetTL - center + _extraOffset;
            }
        }

        public Vec2 barrelVector
        {
            get
            {
                return Offset(barrelOffset) - Offset(barrelOffset + new Vec2(-1f, 0f));
            }
        }
    }
}
