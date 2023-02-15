using System;
using System.Collections.Generic;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Fuse")]
    public class C4 : Holdable
    {
        public SpriteMap _sprite;
        public List<Bullet> firedBullets = new List<Bullet>();

        public float holden = 0f;
        public float C4timer = 25f;
        public float defuse = 0f;

        public bool planted = false;
        public bool defused = false;
        public bool coZone = false;

        bool ableToPlant;
        bool ableToDefuse;
        float keyVisibility;

        public C4(float xval, float yval) : base(xval, yval)
        {
            weight = 0f;
            _editorName = "Non-GM C4";

            collisionSize = new Vec2(8f, 10f);
            collisionOffset = new Vec2(-4f, -6f);
            center = new Vec2(8f, 6f);

            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/FuseMode/C4"), 16, 12, false);
            base.graphic = new SpriteMap(GetPath("Sprites/Gamemods/FuseMode/C4.png"), 16, 12, false);
            _sprite.AddAnimation("idle", 1f, true, new int[1]);
        }

        public override void Update()
        {
            if (position.y > Level.current.lowestPoint + 400f)
            {
                GM_Fuse fuse = Level.Nearest<GM_Fuse>(x, y);
                position = fuse.position;
            }

            if (prevOwner != null)
            {
                Duck prev = prevOwner as Duck;
                if (prev.HasEquipment(typeof(CTEquipment)))
                {
                    hSpeed = 0f;
                }
            }
            if (planted)
            {
                angleDegrees = 0;
            }

            if (holden >= 2f)
            {
                planted = true;
                if (owner != null)
                {
                    Duck d = owner as Duck;
                    d.doThrow = true;
                    SFX.Play(GetPath("SFX/bombplanted.wav"), 1f, 0f, 0f, false);
                    foreach(GM_Fuse gm in Level.current.things[typeof(GM_Fuse)])
                    {
                        if(gm != null)
                        {
                            gm.time = gm.ExplosionTime;
                        }
                    }
                }
            }
            if (planted && !defused)
            {
                if (coZone)
                {
                    if(Level.CheckRect<PlantZone>(topLeft, bottomRight) == null)
                    {
                        defused = true;
                        SFX.Play(GetPath("SFX/bombdefused.wav"), 1f, 0f, 0f, false);
                        defuse = 0f;
                    }
                    
                }
            }
            if (owner != null)
            {
                Duck d = owner as Duck;
                if (d.HasEquipment(typeof(CTEquipment)))
                {
                    d.doThrow = true;
                    hSpeed *= 0f;
                    vSpeed *= 0f;
                    velocity *= new Vec2(0f, 0f);
                }
                if (d.inputProfile.Released("SHOOT") && !planted && !d.crouch)
                {
                    holden = 0f;
                }
                if (!coZone)
                {
                    if (!planted && d.crouch && d.grounded)
                    {
                        ableToPlant = true;
                        if (d.inputProfile.Down("SHOOT"))
                        {
                            d._disarmDisable = 5;
                            holden += 0.0166666f;
                            if (holden >= 2.5f)
                            {

                            }
                            if (holden % 0.3 > 0.02 && holden % 0.3 < 0.05)
                            {
                                SFX.Play(GetPath("SFX/bombbeep.wav"));
                                Level.Add(new ButtonsG(x, y - 24f));
                                d._disarmDisable = 0;
                            }
                        }
                    }
                    else
                    {
                        ableToPlant = false;
                    }
                }
                else
                {
                    PlantZone pz = Level.CheckRect<PlantZone>(topLeft, bottomRight);
                    if (pz != null)
                    {
                        if (d.crouch && d.grounded && pz.Custom && !planted)
                        {
                            ableToPlant = true;
                            if (d.inputProfile.Down("SHOOT"))
                            {
                                d._disarmDisable = 5;
                                holden += 0.0166666f;
                                if (holden % 0.3 > 0.02 && holden % 0.3 < 0.05 && holden < 3)
                                {
                                    SFX.Play(GetPath("SFX/bombbeep.wav"));
                                    Level.Add(new ButtonsG(x, y - 24f));
                                    d._disarmDisable = 5;
                                }
                            }
                        }
                        else
                        {
                            ableToPlant = false;
                        }
                    }
                }
            }
            else
            {
                ableToPlant = false;
            }
            if (planted)
            { 
                canPickUp = false;
                C4timer -= 0.0166666f;
                if (C4timer > 0 && !defused && C4timer % 1 > 0.02f && C4timer % 1 < 0.05f)
                {
                    SFX.Play(GetPath("SFX/bombbeep.wav"), 1f, 0f, 0f, false);
                }
                if (grounded && C4timer > 0f && !defused)
                {
                    ableToDefuse = false;
                    float defuseSpeed = 0;
                    foreach (CTEquipment cte in Level.CheckRectAll<CTEquipment>(new Vec2(position.x - 10f, position.y + 2f), new Vec2(position.x + 10f, position.y - 6f)))
                    {
                        if (cte.equippedDuck != null)
                        {
                            if (cte.equippedDuck.holdObject == null || cte.equippedDuck.holdObject is Defuser)
                            {
                                ableToDefuse = true;
                            }
                            if (cte.equippedDuck.inputProfile.Down("SHOOT"))
                            {
                                if (cte.equippedDuck.holdObject == null)
                                {
                                    defuseSpeed = 1;
                                }
                                if (cte.equippedDuck.holdObject is Defuser)
                                {
                                    defuseSpeed = 2;
                                }
                                cte.equippedDuck.hSpeed = 0;
                            }
                        }
                    }
                    if(defuseSpeed > 0)
                    {
                        defuse += 0.0166666f * defuseSpeed;

                        if (defuse % 0.7 > 0.02 && defuse % 0.7 < 0.05 && defuse < 6)
                        {
                            SFX.Play(GetPath("SFX/Defuse.wav"));
                            Level.Add(new DefuseFore(position.x, position.y - 6f, 0.5f, (int)defuse));
                        }

                        if (defuse >= 6f)
                        {
                            defused = true;
                            SFX.Play(GetPath("bombdefused.wav"), 1f, 0f, 0f, false);
                            defuse = 0f;
                        }
                    }
                    else
                    {
                        defuse = 0;
                    }
                }
            }
            if (defused)
            {
                ableToDefuse = false;
            }
            if(removedFromFall)
            {
                Explode();
            }
            if (defuse >= 6f)
            {
                defused = true;
                SFX.Play(GetPath("SFX/bombdefused.wav"), 1f, 0f, 0f, false);
                defuse = 0f;
            }
            if (C4timer <= 0f && !defused)
            {
                Explode();
            }
            base.Update();
        }
        public virtual void Explode()
        {
            foreach (Duck duck in Level.CheckCircleAll<Duck>(position, 160f))
            {
                duck.Kill(new DTImpact(this));
            }

            Level.Add(new ExplosionPart(position.x, position.y, true));
            int num = 6;
            if (Graphics.effectsLevel < 2)
            {
                num = 3;
            }
            for (int i = 0; i < num; i++)
            {
                float dir = i * 60f + Rando.Float(-10f, 10f);
                float dist = Rando.Float(20f, 20f);
                ExplosionPart ins = new ExplosionPart(position.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), position.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
                Level.Add(ins);
            }

            Graphics.FlashScreen();
            SFX.Play("explode", 1f, 0f, 0f, false);

            if (isServerForObject)
            {
                for (int i = 0; i < 20; i++)
                {
                    float dir = i * 18f - 5f + Rando.Float(10f);
                    ATShrapnel shrap = new ATShrapnel();
                    shrap.range = 160f + Rando.Float(8f);
                    Bullet bullet = new Bullet(position.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * 6.0), position.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * 6.0), shrap, dir, null, false, -1f, false, true);
                    Level.Add(bullet);
                    firedBullets.Add(bullet);
                    if (Network.isActive)
                    {
                        NMFireGun gunEvent = new NMFireGun(null, firedBullets, 20, false, 4, false);
                        Send.Message(gunEvent, NetMessagePriority.ReliableOrdered);
                        firedBullets.Clear();
                    }
                    Level.Remove(this);
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if (ableToPlant || ableToDefuse)
            {
                keyVisibility = Maths.LerpTowards(keyVisibility, 1, 0.04f);
            }
            else
            {
                keyVisibility = Maths.LerpTowards(keyVisibility, 0, 0.05f);
            }

            if (keyVisibility > 0)
            {
                Graphics.DrawString("@SHOOT@", position + new Vec2(-6, -36), Color.White * keyVisibility);
            }
        }
    }
}
