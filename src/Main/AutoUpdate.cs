using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame.C44P
{
    class updater : IAutoUpdate
    {
        public SinWave _wave = 0.3f;
        public static RenderTarget2D _renderTarget;
        public static int Defender;
        public static int Attacker;

        public void Update()
        {
            string path = "Sprites/Items/Weapons/";
            foreach (Thunderbuss gun in Level.current.things[typeof(Thunderbuss)])
            {
                string gunFileName = "/Newblunderbuss.png";
                if (gun != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 29, 10);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 29, 10);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 29, 10);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 29, 10);
                            }
                        }
                    }
                }
            }
            foreach (MagnumOpus gun in Level.current.things[typeof(MagnumOpus)])
            {
                string gunFileName = "/Newmagnum.png";
                if (gun != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 32, 32);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 32, 32);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 32, 32);
                            }
                            if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                            {
                                gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 32, 32);
                            }
                        }
                    }
                }
            }
            foreach (M16 gun in Level.current.things[typeof(M16)])
            {
                string gunFileName = "/m16.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 32, 32);
                        }
                    }
                }
            }
            foreach (XM1014 gun in Level.current.things[typeof(XM1014)])
            {
                string gunFileName = "/xm1014.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 32, 32);
                        }
                    }
                }
            }
            foreach (NewPistol gun in Level.current.things[typeof(NewPistol)])
            {
                string gunFileName = "/Newpistol.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 32, 16);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 32, 16);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 32, 16);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 32, 16);
                        }
                    }
                }
            }
            foreach (MP5 gun in Level.current.things[typeof(MP5)])
            {
                string gunFileName = "/Newsmg.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 20, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 20, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 20, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 20, 10);
                        }
                    }
                }
            }
            foreach (OldVinchester gun in Level.current.things[typeof(OldVinchester)])
            {
                string gunFileName = "/Newoldpistol.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 32, 32);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 32, 32);
                        }
                    }
                }
            }
            foreach (SNAIPER gun in Level.current.things[typeof(SNAIPER)])
            {
                string gunFileName = "/awp.png";
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d != null)
                    {
                        if (d._equipment != null && d.HasEquipment(typeof(Rainbow)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Rainbow" + gunFileName), 40, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Carbon)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Carbon" + gunFileName), 40, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Jungle)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Jungle" + gunFileName), 40, 10);
                        }
                        if (d._equipment != null && d.HasEquipment(typeof(Aqua)) == true && d.holdObject == gun)
                        {
                            gun.graphic = new SpriteMap(Mod.GetPath<C44P>(path + "Aqua" + gunFileName), 40, 10);
                        }
                    }
                }
            }
        }
        
        static IEnumerable<SpriteMap> getSprites(Holdable e)
        {
            if (e == null)
                return null;
            var ret = e.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.FieldType == typeof(SpriteMap)).Select<FieldInfo, SpriteMap>(fi => (SpriteMap)fi.GetValue(e));
            return ret;
        }
    }
}
