using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DuckGame.C44P
{
	public class HarmonyPatches
	{
		//TeamSelect2.DefaultSettings - enables custom levels
		public static void TeamSelect2DefaultSettings_Prefix()
		{
			DuckNetwork.core.matchSettings[3].value = 0; //Default %
			DuckNetwork.core.matchSettings[4].value = 0; //Random %
			DuckNetwork.core.matchSettings[5].value = 100; //Custom %
			DuckNetwork.core.matchSettings[6].value = 0; //Workshop %
		}


		//UIComponent.Update - changing the text for gamemodes
		public static void UIComponentUpdate_Prefix(Thing __instance)
		{
			if (/*!RUDE.debug*/true)
			{
				if (__instance is UIText)
				{
					int num = (int)TeamSelect2.GetMatchSetting("gamemode").value;
					UIText text = __instance as UIText;
					FieldInfo field = typeof(UIText).GetField("_text", BindingFlags.Instance | BindingFlags.NonPublic);
					if (num == 0)
					{
						if (text.text == "Required Kills" || text.text == " Required Points")
						{
							field.SetValue(__instance, "Required Wins");
						}
						if (text.text == "|GRAY|Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "Rests Every");
						}
						if (text.text == "|GRAY|Wall Mode")
						{
							field.SetValue(__instance, "Wall Mode");
						}
						if (text.text == "@NORMALICON@|GRAY|Normal Levels")
						{
							field.SetValue(__instance, "@NORMALICON@|DGBLUE|Normal Levels");
						}
						if (text.text == "@RANDOMICON@|GRAY|Random Levels")
						{
							field.SetValue(__instance, "@RANDOMICON@|DGBLUE|Random Levels");
						}
						if (text.text == "@CUSTOMICON@|GRAY|Custom Levels")
						{
							field.SetValue(__instance, "@CUSTOMICON@|DGBLUE|Custom Levels");
						}
						if (text.text == "@RAINBOWICON@|GRAY|Internet Levels")
						{
							field.SetValue(__instance, "@RAINBOWICON@|DGBLUE|Internet Levels");
						}
					}
					else
					{
						if (text.text == "Wall Mode")
						{
							field.SetValue(__instance, "|GRAY|Wall Mode");
						}
						if (text.text == "@NORMALICON@|DGBLUE|Normal Levels")
						{
							field.SetValue(__instance, "@NORMALICON@|GRAY|Normal Levels");
						}
						if (text.text == "@RANDOMICON@|DGBLUE|Random Levels")
						{
							field.SetValue(__instance, "@RANDOMICON@|GRAY|Random Levels");
						}
						if (text.text == "@CUSTOMICON@|DGBLUE|Custom Levels")
						{
							field.SetValue(__instance, "@CUSTOMICON@|GRAY|Custom Levels");
						}
						if (text.text == "@RAINBOWICON@|DGBLUE|Internet Levels")
						{
							field.SetValue(__instance, "@RAINBOWICON@|GRAY|Internet Levels");
						}
					}
					if (num == 1)
					{
						if (text.text == "Required Wins" || text.text == " Required Points")
						{
							text.text = "Required Kills";
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}
					}
					else if (num == 2)
					{
						if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Seconds")
						{
							text.text = " Required Points";
						}
						if (text.text == "Rests Every" || text.text == "|GRAY|Rests Every")
						{
							field.SetValue(__instance, "Initial Points");
						}
					}
					else if (num == 3)
					{
						if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Points" || text.text == " Required Levels")
						{
							field.SetValue(__instance, " Required Seconds");
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}
					}
					else if (num == 4)
					{
						if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Points" || text.text == " Required Seconds")
						{
							field.SetValue(__instance, " Required Levels");
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}
					}
				}
			}
		}

		//Duck.Kill - for creating a respawn trail of duck and more
		public static void DuckKill_Prefix(Duck duck, DestroyType type = null)
		{
			List<Thing> list = Level.current.things[typeof(TeamRespawner)].ToList();

			if (duck.invincible)
			{
				if (!(type is DTImpale) && duck._trapped == null)
				{
					return;
				}
				else
				{
					duck.killingNet = false;
					duck.invincible = false;
				}
			}
			/*if ((int)TeamSelect2.GetMatchSetting("gamemode").value == 2 && duck.profile != null && __instance.profile.localPlayer && points != null)
			{
				bool flag = false;
				foreach (DuckPoints p in Level.current.things[typeof(DuckPoints)])
				{
					if (p.points >= (int)DuckNetwork.core.matchSettings[0].value)
					{
						flag = true;
					}
				}
				if (!duck.dead && !flag)
				{
					points.DeductPoint(false);
					if (DuckNetwork.localConnection != points.connection)
					{
						Send.Message(new NMDeductPoint(points.duck), points.connection);
					}
				}
			}*/

			/*if ((int)TeamSelect2.GetMatchSetting("gamemode").value == 3 && duck.profile != null && duck.profile.localPlayer && points != null)
			{
				points.wasDead = true;
			}*/

			if (list.Count > 0)
			{
				TeamHat hat = (TeamHat)duck.GetEquipment(typeof(TeamHat));
				if (hat != null)
				{
					Level.Remove(hat);
				}
			}
			if (list.Count > 0 && duck.profile != null && duck.profile.localPlayer)
			{
				if (duck.ragdoll != null)
				{
					duck.ragdoll.tongueStuck = Vec2.Zero;
					duck.ragdoll.tongueStuckThing = null;
				}
				if (duck._ragdollInstance != null)
				{
					duck._ragdollInstance.tongueStuck = Vec2.Zero;
					duck._ragdollInstance.tongueStuckThing = null;
				}
				List<Type> equippers = new List<Type>();
				foreach (Thing thing in Level.current.things[typeof(Equipper)])
				{
					Equipper e = (Equipper)thing;
					Thing t = e.GetContainedInstance(duck.position);
					if (t != null && t is Equipment && !(t is Holster))
					{
						equippers.Add(t.GetType());
					}
				}
				List<Equipment> equipment = new List<Equipment>();
				foreach (Equipment e in duck._equipment)
				{
					foreach (Type t in equippers)
					{
						if (e.GetType() == t)
						{
							equipment.Add(e);
						}
					}
				}

				if (duck._cooked != null)
					return;

				TeamRespawner nextSpawn = TeamRespawner.GetRespawner(duck);

				if (nextSpawn != null)
				{
					Vec2 position = nextSpawn.position;
					DuckNetwork.SendToEveryone(new NMDuckRespawn(duck, false));
					duck.Ressurect();
					if (Network.isActive)
					{
						if (duck._ragdollInstance != null && duck._ragdollInstance._part1 != null && duck._ragdollInstance._part2 != null && duck._ragdollInstance._part3 != null)
						{
							duck._ragdollInstance._part1.material = duck.material;
							duck._ragdollInstance._part2.material = duck.material;
							duck._ragdollInstance._part3.material = duck.material;
						}
					}
					else
					{
						if (duck.ragdoll != null && duck.ragdoll._part1 != null && duck.ragdoll._part2 != null && duck.ragdoll._part3 != null)
						{
							duck.ragdoll._part1.material = duck.material;
							duck.ragdoll._part2.material = duck.material;
							duck.ragdoll._part3.material = duck.material;
						}
					}
					if (duck.inNet)
					{
						duck.invincible = false;
					}
					duck.invincible = false; 
					if (duck.profile != null && duck.profile.localPlayer)
					{
						Thing.Fondle(duck, DuckNetwork.localConnection);
					}
				}
			}
		}

		//RagdollPart.OnDestroy - fixes a bug when ragdoll creates 2 CookedDucks
		public static bool RagdollDestroy_Prefix(RagdollPart __instance, DestroyType type = null)
		{
			if (type is DTIncinerate)
			{
				Level.Add(SmallSmoke.New(__instance._doll.x + Rando.Float(-4f, 4f), __instance._doll.y + Rando.Float(-4f, 4f)));
				Level.Add(SmallSmoke.New(__instance._doll.x + Rando.Float(-4f, 4f), __instance._doll.y + Rando.Float(-4f, 4f)));
				Level.Add(SmallSmoke.New(__instance._doll.x + Rando.Float(-4f, 4f), __instance._doll.y + Rando.Float(-4f, 4f)));
				__instance._doll.captureDuck.Kill(type);
				return false;
			}
			return true;
		}

		//Level.AddThing
		/*public static bool LevelAddThing_Prefix(Level __instance, Thing t)
		{
			if (__instance.things[typeof(Respawns)].Count() > 0)
			{
				if (t.GetType().Assembly.GetName().Name == "UFFMod")
				{
					if (t.GetType().Name == "FreezeRay" || t.GetType().Name == "HandOfMidas") // they break respawns
					{
						__instance.AddThing(new CaseRUDE(t.x, t.y));
						return false;
					}
				}
				if (t is GoodBook)
				{
					__instance.AddThing(new CaseRUDE(t.x, t.y));
					return false;
				}
			}
			return true;
		}*/

		//DuckNetwork.CreateMatchSettingsInfoWindow
		public static void DuckNetworkCreateMatchSettingsInfoWindow_Postfix(ref UIMenu __result, UIMenu openOnClose = null)
		{
			BitmapFont littleFont = new BitmapFont("biosFontUI", 8, 7);
			MatchSetting i = TeamSelect2.GetMatchSetting("gamemode");
			string textPart = "Mode";
			string textPart2 = i.valueStrings[(int)i.value];
			if ((int)i.value == 0)
			{
				textPart2 = "          " + textPart2;
			}
			else if ((int)i.value == 1)
			{
				textPart2 = "         " + textPart2;
			}
			else if ((int)i.value == 2 || (int)i.value == 3)
			{
				textPart2 = "        " + textPart2;
			}
			else
			{
				textPart2 = "         " + textPart2;
			}
			string text = textPart + " " + textPart2;
			UIText t;
			if (!i.value.Equals(i.prevValue))
			{
				t = new UIText(text, Colors.DGBlue, UIAlign.Center, 0f, null);
			}
			else
			{
				t = new UIText(text, Colors.Silver, UIAlign.Center, 0f, null);
			}
			i.prevValue = i.value;
			t.SetFont(littleFont);
			__result.Insert(t, 13, true);
		}
	}
}
