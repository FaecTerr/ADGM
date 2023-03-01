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
						/*if (text.text == "Required Kills" || text.text == " Required Points")
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
						}*/
					}
					else
					{
						/*if (text.text == "Wall Mode")
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
						}*/
					}
					if (num == 1) //Fuse
					{
						/*if (text.text == "Required Wins" || text.text == " Required Points")
						{
							text.text = "Required Kills";
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}*/
					}
					else if (num == 2) //CTF
					{
						/*if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Seconds")
						{
							text.text = " Required Points";
						}
						if (text.text == "Rests Every" || text.text == "|GRAY|Rests Every")
						{
							field.SetValue(__instance, "Initial Points");
						}*/
					}
					else if (num == 3) //CP
					{
						/*if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Points" || text.text == " Required Levels")
						{
							field.SetValue(__instance, " Required Seconds");
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}*/
					}
					else if (num == 4) //C
					{
						/*if (text.text == "Required Wins" || text.text == "Required Kills" || text.text == " Required Points" || text.text == " Required Seconds")
						{
							field.SetValue(__instance, " Required Levels");
						}
						if (text.text == "Rests Every" || text.text == "Initial Points")
						{
							field.SetValue(__instance, "|GRAY|Rests Every");
						}*/
					}
				}
			}
		}

		//Duck.Kill 
		public static void DuckKill_Prefix(Duck duck, DestroyType type = null)
		{
			
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
