using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("Additional duck game modes")]

// The author of the mod
[assembly: AssemblyCompany("FaecTerr")]

// The description of the mod
[assembly: AssemblyDescription("Some new gamemods to make new maps")]

// The mod's version
[assembly: AssemblyVersion("1.0.0.0")]

namespace DuckGame.C44P
{
    public class C44P : Mod
    {
        public static float globalA;
        public static float globalB;
        public static float globalC;

		public static bool patched;
		public static bool debug;

		public static Tex2D CorrectTexture(Tex2D tex, bool recolor = false, Vec3 color = default(Vec3))
        {
            if (recolor)
                return Graphics.Recolor(tex, color);
            RenderTarget2D t = new RenderTarget2D(tex.width, tex.height);
            Graphics.SetRenderTarget(t);
            Graphics.Clear(new Color(0, 0, 0, 0));
            Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            Graphics.Draw(tex, new Vec2(), new Rectangle?(), Color.White, 0.0f, new Vec2(), new Vec2(1f, 1f), SpriteEffects.None, (Depth)0.5f);
            Graphics.screen.End();
            Graphics.device.SetRenderTarget(null);
            return t;
        }
        static updater upd;
        // The mod's priority; this property controls the load order of the mod.
        public override Priority priority
        {
            get { return base.priority; }
        }

        // This function is run before all mods are finished loading.
        protected override void OnPreInitialize()
        {
            base.OnPreInitialize();
            DevConsole.AddCommand(new CMD("teams", delegate ()
            {
                for (int i = 0; i < Teams.active.Count; i++)
                {
                    DevConsole.Log(Teams.active[i].name);
                }
            }));
            DevConsole.AddCommand(new CMD("setargA", delegate ()
            {

            }));
        }

        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
        {
            base.OnPostInitialize();
            Thread tr = new Thread(wait);
            tr.Start();
            copyLevels();

        }

        protected override void OnStart()
        {
            base.OnStart();
			debug = NetworkDebugger.enabled;
			if (debug)
			{
				Patch();
				patched = true;
			}
		}

        private byte[] GetMD5Hash(byte[] sourceBytes)
        {
            return new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
        }
        void wait()
        {
            while (Level.current == null || !(Level.current.ToString() == "DuckGame.TitleScreen") && !(Level.current.ToString() == "DuckGame.TeamSelect2"))
                Thread.Sleep(200);
            upd = new updater();
            //AutoUpdatables.Add(upd);
        }
        private static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }
            int iterations = (int)Math.Ceiling((double)first.Length / 8.0);
            using (FileStream fs = first.OpenRead())
            {
                using (FileStream fs2 = second.OpenRead())
                {
                    byte[] one = new byte[8];
                    byte[] two = new byte[8];
                    for (int i = 0; i < iterations; i++)
                    {
                        fs.Read(one, 0, 8);
                        fs2.Read(two, 0, 8);
                        if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private static void copyLevels()
        {
            string levelFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DuckGame\\Levels\\C44PMaps");
            if (!Directory.Exists(levelFolder))
            {
                Directory.CreateDirectory(levelFolder);
            }
            foreach (string sourcePath in Directory.GetFiles(Mod.GetPath<C44P>("Levels")))
            {
                string destPath = Path.Combine(levelFolder, Path.GetFileName(sourcePath));
                bool file_exists = File.Exists(destPath);
                if (!file_exists || !C44P.FilesAreEqual(new FileInfo(sourcePath), new FileInfo(destPath)))
                {
                    if (file_exists)
                    {
                        File.Delete(destPath);
                    }
                    File.Copy(sourcePath, destPath);
                }
            }
        }
		public static void AddLevels(string path = "")
		{
			foreach (string file in Directory.GetFiles(GetPath<C44P>("/Levels/" + path)))
			{
				if (!DuckNetwork.core._activatedLevels.Contains(file))
				{
					DuckNetwork.core._activatedLevels.Add(file);
				}
			}
		}
		public static void Patch()
        {
            Assembly Harmony = Assembly.Load(File.ReadAllBytes(GetPath<C44P>("HarmonyLoader") + ".dll"));
            if (Harmony != null)
            {
                try
                {
                    Type t = Harmony.GetType("HarmonyLoader.Loader"); 
                    MethodInfo Patch = t.GetMethod("Patch", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    Patch.Invoke(null, new object[] { SGMI(typeof(TeamSelect2), "DefaultSettings"), SGMI(typeof(HarmonyPatches), "TeamSelect2DefaultSettings_Prefix"), null, null });
                    Patch.Invoke(null, new object[] { SGMI(typeof(Duck), "Kill"), SGMI(typeof(HarmonyPatches), "DuckKill_Prefix"), null, null });
                    Patch.Invoke(null, new object[] { SGMI(typeof(DuckNetwork), "CreateMatchSettingsInfoWindow"), null, SGMI(typeof(HarmonyPatches), "DuckNetworkCreateMatchSettingsInfoWindow_Postfix"), null }); 
					if (debug) Patch.Invoke(null, new object[] { SGMI(typeof(DevConsole), "Update"), SGMI(typeof(SettingsInit), "Prefix"), null, null });
				}
                catch
                {
                    DevConsole.Log("Patching failed.", Color.Red, 2f, -1);
                }
            }
        }
        public static MethodInfo SGMI(Type type, string Methodname)
        {
            MethodInfo method = type.GetMethod(Methodname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            return method;
        }
	}
	internal static class SettingsInit
	{
		private static void Prefix()
		{
			if (Level.current is TeamSelect2)
			{
				if (DuckNetwork.core.matchSettings.Count() < 9)
				{
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
							id = "wallmode",
							name = "Wall Mode",
							value = false
						},
						new MatchSetting
						{
							id = "normalmaps",
							name = "@NORMALICON@|DGBLUE|Normal Levels",
							value = 90,
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
							value = 10,
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
							value = 0,
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
							valueStrings = new List<string>() { "VANILLA", "FUSE", "CAPTURE THE FLAG", "CONTROL POINT", "COLLECTIBLE", "THEFT" },
							value = 1,
							min = 0,
							max = 5,
							step = 1,
						}
					};
				}
				else if (Network.isServer)
				{
					//Send.Message(new NMChangeModeRUDE((byte)(int)TeamSelect2.GetMatchSetting("gamemode").value));
				}
			}
		}
	}
}


