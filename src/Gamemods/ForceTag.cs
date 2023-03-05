using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM")]
    public class ForceTag : Thing
    {
        public EditorProperty<float> range = new EditorProperty<float>(64, null, 16, 160, 16) { _tooltip = "Radius of connection" };

        //The idea here is to make it obvious for map-creators and GM on where is which team, so they won't need specific armor to play gamemode safely.
        //'Why?' you might ask 'Isn't it working well with them?'... Well, it actually does, unless duck dies and armor might be problematic thing to synq if you want respawn
        //I am still not sure if I do need this thing right now, but it definetly nice to have, cause makes things bit accurate and nice, instead of workarounds
        //Some of workarounds done, so maybe they are not that cool, you might think. But if I had whole time of the world, I would definetly done this whole mod differently
        public EditorProperty<int> tagID = new EditorProperty<int>(0, null, 0, 7, 1) { _tooltip = "This tool is to help gamemode hold teams up without equipment. \nDefault: 0 - Defending (CTE), 1 - Attacking (TE). CTF and CP doesn't \nnecessarily require any tagging and maintain them on their own."};
        public EditorProperty<bool> responsibleForRespawn = new EditorProperty<bool>(false) { name = "Respawn", _tooltip = "Marks that tagID team to be able to respawn. Only one required per tagID."}; 
        public EditorProperty<bool> Trigger = new EditorProperty<bool>(false);
        public EditorProperty<int> TriggerID = new EditorProperty<int>(0, null, 0, 9, 1);

        public List<Team> team = new List<Team>();

        public bool initialized;
        public bool triggered;

        Color[] colors = new Color[] { Color.LightBlue, Color.Red, Color.Purple, Color.OrangeRed, Color.DarkGreen, Color.YellowGreen, Color.Lime, Color.Magenta};
        SpriteMap sprite = new SpriteMap(GetPath<C44P>("Sprites/Gamemods/Tag.png"), 16, 16); 
        SpriteMap marks = new SpriteMap(GetPath<C44P>("Sprites/Gamemods/TagProperties.png"), 32, 8);

        int respawnCooldown;

        public ForceTag() : base()
        {
            graphic = sprite;
            center = new Vec2(8, 8);
            collisionSize = new Vec2(16, 16); 
            collisionOffset = -collisionSize * 0.5f;

            marks.center = new Vec2(6, 4);
            marks.scale *= new Vec2(0.5f, 0.5f);

            layer = Layer.Foreground;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void ApplyEffect()
        {
            initialized = true;
            if (team.Count > 0)
            {
                foreach (TeamRespawner respawner in Level.CheckCircleAll<TeamRespawner>(position, range))
                {
                    respawner.dgTeam = team[0];
                }
                foreach (FlagBase flag in Level.CheckCircleAll<FlagBase>(position, range))
                {
                    flag.ReassignTeam(team[0]);
                    flag.replacedFrame = tagID.value;
                }
            }
        }

        public void OnTrigger()
        {
            foreach (TeamRespawner respawner in Level.CheckCircleAll<TeamRespawner>(position, range))
            {
                respawner.accesible = !respawner.accesible;
            }
        }

        public override void Update()
        {
            base.Update();
            if (!initialized && Level.current.initialized)
            {
                foreach (Duck duck in Level.CheckCircleAll<Duck>(position, range))
                {
                    if (!team.Contains(duck.team))
                    {
                        team.Add(duck.team);
                    }
                }
                if (team.Count > 0)
                {
                    foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
                    {
                        if (tag.tagID.value == tagID.value && tag != this)
                        {
                            if (!tag.team.Contains(team[0]))
                            {
                                tag.team.Add(team[0]);
                            }
                            tag.ApplyEffect();
                        }
                    }
                    ApplyEffect();
                }
                else
                {
                    initialized = true;
                }
            }
            if (Trigger && triggered)
            {
                triggered = false;
                foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
                {
                    if (tag.tagID.value == tagID.value && tag.TriggerID.value == TriggerID.value && tag.Trigger)
                    {
                        tag.OnTrigger();
                    }
                }
            }
            if (responsibleForRespawn)
            {
                if(team.Count > 0)
                {
                    if (/*Level.current is GameLevel && typeof(GameLevel).GetField("_mode", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(Level.current as GameLevel) is GameMode*/ true)
                    {
                        if (respawnCooldown <= 0)
                        {
                            for (int i = 0; i < team[0].activeProfiles.Count; i++)
                            {
                                Profile p = team[0].activeProfiles[i];
                                if (p.duck != null)
                                {
                                    Duck d = p.duck;
                                    if (d != null && d.dead)
                                    {
                                        Vec2 respawnPos = TeamRespawner.GetRespawner(d).position + new Vec2(0, -16f);
                                        if (respawnPos != null)
                                        {
                                            respawnCooldown = 80;
                                            SuperFondle(d, DuckNetwork.localConnection);
                                            d.position = respawnPos;
                                            if (d.killedByProfile == null && d.ragdoll == null)
                                            {
                                                d.GoRagdoll();
                                                if(d.ragdoll != null)
                                                {
                                                    d.ragdoll.position = respawnPos;
                                                }
                                            }
                                            d.Ressurect();
                                            if (d._cooked != null) d.position = respawnPos;
                                            if (d.onFire)
                                            {
                                                d.onFire = false;
                                                d.moveLock = false;
                                                d.dead = false;
                                            }
                                            d.dead = false;
                                            d.ResetNonServerDeathState();
                                            d.Regenerate();
                                            d.crouch = false;
                                            d.sliding = false;
                                            d.burnt = 0f;
                                            d.hSpeed = 0f;
                                            d.vSpeed = 0.1f;
                                            sbyte prevDir = d.offDir;
                                            d.offDir = 1;
                                            d.offDir = -1;
                                            d.offDir = prevDir;

                                            if (d.ragdoll != null)
                                            {
                                                d.ragdoll.Unragdoll();
                                            }
                                            if (d._trapped != null)
                                            {
                                                d._trapped.position = respawnPos;
                                                d._trapped._trapTime = 0;
                                            }
                                            d.strafing = false;
                                            if (d.holdObject != null)
                                            {
                                                Holdable holdable = d.holdObject;
                                                d.ThrowItem(false);
                                            }
                                            foreach (Equipment equipment in d._equipment)
                                            {
                                                equipment.UnEquip();
                                            }
                                            d.position = respawnPos;
                                            if (d._ragdollInstance != null)
                                            {
                                                if (d._ragdollInstance.removeFromLevel)
                                                {
                                                    d._ragdollInstance = new Ragdoll(d.x, d.y - 9999, d, false, 0, 0, Vec2.Zero);
                                                    d._ragdollInstance.npi = d.netProfileIndex;
                                                    d._ragdollInstance.RunInit();
                                                    d._ragdollInstance.active = false;
                                                    d._ragdollInstance.visible = false;
                                                    d._ragdollInstance.authority = 80;
                                                    Level.Add(d._ragdollInstance);
                                                    Fondle(d._ragdollInstance);
                                                }
                                            }
                                            foreach(Hat h in Level.current.things[typeof(Hat)])
                                            {
                                                if(h.prevOwner != null && h.prevOwner is Duck && (h.prevOwner as Duck) == d)
                                                {
                                                    d.Equip(h);
                                                }
                                            }
                                            if (d._cookedInstance != null && d._cookedInstance.visible)
                                            {
                                                d._cookedInstance.visible = false;
                                            }
                                            d.visible = true;

                                            Level.Add(new TemporaryInvincibility(d));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            respawnCooldown--;
                        }
                    }
                }
            }
        }
        public override void Draw()
        {
            frame = tagID;

            if (Level.current is Editor)
            {
                alpha = 1;

                Graphics.DrawCircle(position, range, colors[tagID] * 0.4f);

                int num = 0;
                if (responsibleForRespawn)
                {
                    marks.frame = 0;
                    marks.angle = (num + 1) * 0.3f;
                    marks.depth.Add(-1);
                    Graphics.Draw(marks, position.x - 1, position.y + 4 + num * 1);
                    num++;
                }
                if (Trigger)
                {
                    marks.frame = 1;
                    marks.angle = (num + 1) * 0.3f;
                    marks.depth.Add(-1);
                    Graphics.Draw(marks, position.x - 1, position.y + 4 + num * 1);
                    num++;
                }

                bool isParent = false;
                if (Level.CheckCircle<SpawnPoint>(position, range) != null)
                {
                    isParent = true;
                    float angle = DateTime.Now.Millisecond * 0.36f * 3.14157f / 180;
                    Vec2 spinningPos = new Vec2((float)Math.Cos(angle) * 16, (float)Math.Sin(angle) * 16);
                    Graphics.DrawRect(position + spinningPos - new Vec2(1, 1), position + spinningPos + new Vec2(1, 1), colors[tagID]);
                    Graphics.DrawRect(position - spinningPos - new Vec2(1, 1), position - spinningPos + new Vec2(1, 1), colors[tagID]);
                }
                
                if (isParent)
                {
                    foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
                    {
                        if (tag.tagID.value == tagID.value)
                        {
                            Graphics.DrawLine(position, tag.position, colors[tagID] * 0.3f, 1f);
                            Vec2 offsetPosition = new Vec2(tag.position - position) * DateTime.Now.Millisecond * 0.001f;
                            Graphics.DrawRect(position + offsetPosition - new Vec2(1f, 1), position + offsetPosition + new Vec2(1, 1), colors[tagID]);
                        }
                    }
                }
                if (Trigger)
                {
                    foreach (ForceTag tag in Level.current.things[typeof(ForceTag)])
                    {
                        if (tag.Trigger && tag.TriggerID.value == TriggerID.value && tag.tagID.value == tagID.value)
                        {
                            Graphics.DrawLine(position, tag.position, colors[tagID] * 0.3f, 1.75f);
                            Vec2 offsetPosition = new Vec2(tag.position - position) * DateTime.Now.Millisecond * 0.001f;
                            Graphics.DrawRect(position + offsetPosition - new Vec2(2, 2), position + offsetPosition + new Vec2(2, 2), colors[tagID]);
                        }
                    }
                }

                if (Mouse.positionScreen.x < position.x + 8 && Mouse.positionScreen.x > position.x - 8 && Mouse.positionScreen.y < position.y + 8 && Mouse.positionScreen.y > position.y - 8)
                {
                    foreach (SpawnPoint spawn in Level.CheckCircleAll<SpawnPoint>(position, range))
                    {
                        Graphics.DrawLine(position, spawn.position, colors[tagID], 1.5f);
                        Vec2 offsetPosition = new Vec2(spawn.position - position) * (1 - DateTime.Now.Millisecond * 0.001f);
                        Graphics.DrawRect(position + offsetPosition - new Vec2(2, 2), position + offsetPosition + new Vec2(2, 2), colors[tagID]);
                    }
                    foreach (FlagBase flagBase in Level.CheckCircleAll<FlagBase>(position, range))
                    {
                        Graphics.DrawLine(position, flagBase.position, colors[tagID], 1.5f);
                        Vec2 offsetPosition = new Vec2(flagBase.position - position) * DateTime.Now.Millisecond * 0.001f;
                        Graphics.DrawRect(position + offsetPosition - new Vec2(2, 2), position + offsetPosition + new Vec2(2, 2), colors[tagID]);
                    }
                    foreach (TeamRespawner respawner in Level.CheckCircleAll<TeamRespawner>(position, range))
                    {
                        Graphics.DrawLine(position, respawner.position, colors[tagID], 1.5f);
                        Vec2 offsetPosition = new Vec2(respawner.position - position) * DateTime.Now.Millisecond * 0.001f;
                        Graphics.DrawRect(position + offsetPosition - new Vec2(2, 2), position + offsetPosition + new Vec2(2, 2), colors[tagID]);
                    }
                    if (Trigger)
                    {
                        foreach (ContestSafe safe in Level.CheckCircleAll<ContestSafe>(position, range))
                        {
                            Graphics.DrawLine(position, safe.position, colors[tagID], 1.5f);
                            Vec2 offsetPosition = new Vec2(safe.position - position) * (1 - DateTime.Now.Millisecond * 0.001f);
                            Graphics.DrawRect(position + offsetPosition - new Vec2(2, 2), position + offsetPosition + new Vec2(2, 2), colors[tagID]);
                        }
                    }
                    if (responsibleForRespawn)
                    {
                        foreach (ForceTag f in Level.current.things[typeof(ForceTag)])
                        {
                            if (f != this && f.responsibleForRespawn.value == true && f.tagID.value == tagID.value)
                            {
                                //f.responsibleForRespawn.value = false;
                            }
                        }
                    }
                }
            }
            else
            {
                alpha = 0;
            }
            base.Draw();
        }

    }
}
