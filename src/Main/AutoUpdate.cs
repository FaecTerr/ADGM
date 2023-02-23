using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame.C44P
{
    class updater : IEngineUpdatable
    {
        public SinWave _wave = 0.3f;
        public static RenderTarget2D _renderTarget;
        public static int Defender;
        public static int Attacker;

        public updater()
        {
            MonoMain.RegisterEngineUpdatable(this);
        }

        public void PreUpdate()
        {
            if (!C44P.patched)
            {
                C44P.Patch();
                DuckNetwork.core.matchSettings = new List<MatchSetting>
                {
                    new MatchSetting
                    {
                        id = "requiredwins",
                        name = "Required Wins",
                        value = 15,
                        min = 5,
                        max = 100,
                        step = 5,
                        stepMap = new Dictionary<int, int>
                        {
                            {
                                50,
                                1
                            },
                            {
                                100,
                                10
                            },
                            {
                                500,
                                50
                            },
                            {
                                1000,
                                100
                            }
                        }
                    },
                    new MatchSetting
                    {
                        id = "restsevery",
                        name = "Rests Every",
                        value = 10,
                        min = 0,
                        max = 100,
                        step = 5,
                        stepMap = new Dictionary<int, int>
                        {
                            {
                                50,
                                1
                            },
                            {
                                100,
                                10
                            },
                            {
                                500,
                                50
                            },
                            {
                                1000,
                                100
                            }
                        }
                    },
                    new MatchSetting
                    {
                        id = "wallmode",
                        name = "Wall Mode",
                        value = false
                    },
                    new MatchSetting
                    {
                        id = "normalmaps",
                        name = "@NORMALICON@|DGBLUE|Normal Levels",
                        value = 0,
                        suffix = "%",
                        min = 0,
                        max = 100,
                        step = 5,
                        percentageLinks = new List<string>
                        {
                            "randommaps",
                            "custommaps",
                            "workshopmaps"
                        }
                    },
                    new MatchSetting
                    {
                        id = "randommaps",
                        name = "@RANDOMICON@|DGBLUE|Random Levels",
                        value = 0,
                        suffix = "%",
                        min = 0,
                        max = 100,
                        step = 5,
                        percentageLinks = new List<string>
                        {
                            "normalmaps",
                            "workshopmaps",
                            "custommaps"
                        }
                    },
                    new MatchSetting
                    {
                        id = "custommaps",
                        name = "@CUSTOMICON@|DGBLUE|Custom Levels",
                        value = 100,
                        suffix = "%",
                        min = 0,
                        max = 100,
                        step = 5,
                        percentageLinks = new List<string>
                        {
                            "normalmaps",
                            "randommaps",
                            "workshopmaps"
                        }
                    },
                    new MatchSetting
                    {
                        id = "workshopmaps",
                        name = "@RAINBOWICON@|DGBLUE|Internet Levels",
                        value = 0,
                        suffix = "%",
                        min = 0,
                        max = 100,
                        step = 5,
                        percentageLinks = new List<string>
                        {
                            "normalmaps",
                            "custommaps",
                            "randommaps"
                        }
                    },
                    new MatchSetting
                    {
                        id = "clientlevelsenabled",
                        name = "Client Maps",
                        value = false
                    },
                    new MatchSetting
                    {
                        id = "gamemode",
                        name = "Mode",
                        valueStrings = new List<string>(){ "VANILLA", "FUSE", "CAPTURE THE FLAG", "CONTROL POINT", "COLLECTIBLE", "THEFT" },
                        value = 1,
                        min = 0,
                        max = 5,
                        step = 1,
                    }
                };
                C44P.patched = true;
            }
        }

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

        private static int prevMode;
        private static int playersCountPrev;

        public static List<LSItem> removedLevelsDM = new List<LSItem>();
        public static List<LSItem> removedLevelsFuse = new List<LSItem>();
        public static List<LSItem> removedLevelsCTF = new List<LSItem>();
        public static List<LSItem> removedLevelsCP = new List<LSItem>();
        public static List<LSItem> removedLevelsC = new List<LSItem>();
        public static List<LSItem> removedLevelsTheft = new List<LSItem>();

        public static List<string> FuseLevels = new List<string>();
        public static List<string> CTFLevels = new List<string>();
        public static List<string> CPLevels = new List<string>();
        public static List<string> CLevels = new List<string>();
        public static List<string> TheftLevels = new List<string>();
        public void PostUpdate()
        {
            if (Level.current is TeamSelect2)
            {
                TeamSelect2 level = Level.current as TeamSelect2;
                LevelSelect select = null;
                int selectedGamemode = (int)TeamSelect2.GetMatchSetting("gamemode").value;
                if (Network.isActive && !Network.isServer)
                {
                    prevMode = selectedGamemode;
                }
                LevelSelectCompanionMenu menu;
                if (Network.isActive)
                {
                    menu = DuckNetwork.core._levelSelectMenu as LevelSelectCompanionMenu;
                }
                else
                {
                    menu = (LevelSelectCompanionMenu)typeof(TeamSelect2).GetField("_levelSelectMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(level);
                }
                if (menu != null)
                {
                    select = (LevelSelect)typeof(LevelSelectCompanionMenu).GetField("_levelSelector", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
                    if (select != null)
                    {
                        List<LSItem> items = (List<LSItem>)typeof(LevelSelect).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(select); 
                        if (items != null)
                        {
                            List<LSItem> remove = new List<LSItem>();
                            List<LSItem> removeFuse = new List<LSItem>();
                            List<LSItem> removeCTF = new List<LSItem>();
                            List<LSItem> removeCP = new List<LSItem>();
                            List<LSItem> removeC = new List<LSItem>();
                            List<LSItem> removeTheft = new List<LSItem>();

                            foreach (LSItem item in items)
                            {
                                if (item != null && item.data != null && !removedLevelsDM.Contains(item) && !removedLevelsFuse.Contains(item) && !removedLevelsCTF.Contains(item)
                                     && !removedLevelsCP.Contains(item) && !removedLevelsC.Contains(item) && !removedLevelsTheft.Contains(item))
                                {
                                    bool hasDM = false;
                                    bool hasFuseGM = false;
                                    bool hasCTFGM = false;
                                    bool hasCPGM = false;
                                    bool hasCGM = false;
                                    bool hasTheftGM = false;

                                    try
                                    {
                                        foreach (BinaryClassChunk elly in item.data.objects.objects)
                                        {
                                            string typeString = (string)elly.GetProperty("type");
                                            if (typeString != null && typeString.Contains("DuckGame.C44P.GM_Fuse"))
                                            {
                                                hasFuseGM = true;
                                            }
                                            if (typeString != null && typeString.Contains("DuckGame.C44P.GM_CTF"))
                                            {
                                                hasCTFGM = true;
                                            }
                                            if (typeString != null && typeString.Contains("DuckGame.C44P.GM_CP"))
                                            {
                                                hasCPGM = true;
                                            }
                                            if (typeString != null && typeString.Contains("DuckGame.C44P.GM_Collection"))
                                            {
                                                hasCGM = true;
                                            }
                                            if (typeString != null && typeString.Contains("DuckGame.C44P.GM_STOLEN"))
                                            {
                                                hasTheftGM = true;
                                            }
                                            if(!hasFuseGM && !hasCTFGM && !hasCPGM && !hasCGM && !hasTheftGM)
                                            {
                                                hasDM = true;
                                            }
                                        }
                                    }
                                    catch { }
                                    if (hasDM)
                                    {
                                        remove.Add(item);
                                    }
                                    if (hasFuseGM)
                                    {
                                        removeFuse.Add(item);
                                    }
                                    if (hasCTFGM)
                                    {
                                        removeCTF.Add(item);
                                    }
                                    if (hasCPGM)
                                    {
                                        removeCP.Add(item);
                                    }
                                    if (hasCGM)
                                    {
                                        removeC.Add(item);
                                    }
                                    if (hasTheftGM)
                                    {
                                        removeTheft.Add(item);
                                    }
                                }
                            }
                            foreach (LSItem item in remove)
                            {
                                removedLevelsDM.Add(item);
                                items.Remove(item);
                            }
                            foreach (LSItem item in removeFuse)
                            {
                                removedLevelsFuse.Add(item);
                                items.Remove(item);
                            }
                            foreach (LSItem item in removeCTF)
                            {
                                removedLevelsCTF.Add(item);
                                items.Remove(item);
                            }
                            foreach (LSItem item in removeCP)
                            {
                                removedLevelsCP.Add(item);
                                items.Remove(item);
                            }
                            foreach (LSItem item in removeC)
                            {
                                removedLevelsC.Add(item);
                                items.Remove(item);
                            }
                            foreach (LSItem item in removeTheft)
                            {
                                removedLevelsTheft.Add(item);
                                items.Remove(item);
                            }
                            typeof(LevelSelect).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(select, items);
                        }
                    }
                }
                if (DuckNetwork.core._activatedLevels.Count > 0)
                {
                    foreach (string t in DuckNetwork.core._activatedLevels)
                    {
                        if (!FuseLevels.Contains(t) && !CTFLevels.Contains(t) && !CPLevels.Contains(t) && !CLevels.Contains(t) && !TheftLevels.Contains(t))
                        {
                            LevelData lev = DuckFile.LoadLevel(t);
                            if (lev != null)
                            {
                                bool hasFuseGM = false;
                                bool hasCTFGM = false;
                                bool hasCPGM = false;
                                bool hasCGM = false;
                                bool hasTheftGM = false;

                                foreach (BinaryClassChunk elly in lev.objects.objects)
                                {
                                    string typeString = (string)elly.GetProperty("type");
                                    if (typeString != null)
                                    {
                                        if (typeString.Contains("DuckGame.C44P.GM_Fuse"))
                                        {
                                            hasFuseGM = true;
                                        }
                                        if (typeString.Contains("DuckGame.C44P.GM_CTF"))
                                        {
                                            hasCTFGM = true;
                                        }
                                        if (typeString.Contains("DuckGame.C44P.GM_CP"))
                                        {
                                            hasCPGM = true;
                                        }
                                        if (typeString.Contains("DuckGame.C44P.GM_Collection"))
                                        {
                                            hasCGM = true;
                                        }
                                        if (typeString.Contains("DuckGame.C44P.GM_STOLEN"))
                                        {
                                            hasTheftGM = true;
                                        }
                                    }
                                }
                                if (hasFuseGM && !FuseLevels.Contains(t))
                                {
                                    FuseLevels.Add(t);
                                }
                                if (hasCTFGM && !CTFLevels.Contains(t))
                                {
                                    CTFLevels.Add(t);
                                }
                                if (hasCPGM && !CPLevels.Contains(t))
                                {
                                    CPLevels.Add(t);
                                }
                                if (hasCGM && !CLevels.Contains(t))
                                {
                                    CLevels.Add(t);
                                }
                                if (hasTheftGM && !TheftLevels.Contains(t))
                                {
                                    TheftLevels.Add(t);
                                }
                            }
                        }
                    }
                }
                if (selectedGamemode == 0) //Vanilla
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsDM)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsDM.Clear();
                    }

                    if(prevMode != 0)
                    {
                        //C44P.AddLevels("vanilla/");
                    }

                    foreach (string t in FuseLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CTFLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CPLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in TheftLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
                if (selectedGamemode == 1) //Fuse
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsFuse)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsFuse.Clear();
                    }

                    C44P.AddLevels("Fuse/");

                    foreach (string t in CTFLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CPLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in TheftLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
                if (selectedGamemode == 2) //CTF
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsCTF)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsCTF.Clear();
                    }

                    C44P.AddLevels("CTF/");

                    foreach (string t in FuseLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CPLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in TheftLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
                if (selectedGamemode == 3) //CP
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsCP)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsCP.Clear();
                    }

                    C44P.AddLevels("CP");

                    foreach (string t in FuseLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CTFLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in TheftLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
                if (selectedGamemode == 4) //C
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsC)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsC.Clear();
                    }

                    C44P.AddLevels("C/");

                    foreach (string t in FuseLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CTFLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CPLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in TheftLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
                if (selectedGamemode == 5) //Theft
                {
                    if (select != null)
                    {
                        foreach (LSItem item in removedLevelsTheft)
                        {
                            select.AddItem(item);
                        }
                        removedLevelsTheft.Clear();
                    }

                    C44P.AddLevels("Theft/");

                    foreach (string t in FuseLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CTFLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CPLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                    foreach (string t in CLevels)
                    {
                        DuckNetwork.core._activatedLevels.Remove(t);
                    }
                }
            }
            else
            {
                FuseLevels.Clear();
                CTFLevels.Clear();
                CPLevels.Clear();
                CLevels.Clear();
                TheftLevels.Clear();
            }
        }

        public void OnDrawLayer(Layer pLayer)
        {
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
