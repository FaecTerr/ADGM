using System;
using Microsoft.Xna.Framework.Graphics;

namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|GameMode CTF")]
    public class Flag : Thing
    {
        public new Duck owner;
        public SpriteMap _sprite;
        public SpriteMap flag;
        public Team dgTeam;

        public int Team;
        public bool based = false;
        public bool delivered = false;
        public bool OnBase = true;
        public bool dropByPlayer = false;
        public bool ToBase = false;
        public bool init;
        
        public EditorProperty<int> team;

        public int replacedFrame = -1;

        bool ableToInteract;
        float keyVisibility;

        public Flag(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/CTF/FlagPole.png"), 11, 51, false);
            base.graphic = _sprite;

            flag = new SpriteMap(GetPath("Sprites/Gamemods/CTF/Flag.png"), 27, 18, false);
            flag.CenterOrigin(); 

            center = new Vec2(5.5f, 24.5f);
            collisionOffset = new Vec2(-4.5f, -24.5f);
            collisionSize = new Vec2(9f, 49f);

            depth = -0.4f;

            _editorName = "Non-GM Flag";
            hugWalls = WallHug.Floor;
            team = new EditorProperty<int>(1, this, 1f, 8f, 1f, null, false, false);
        }

        public override void Update()
        {
            base.Update();

            if (!init)
            {
                init = true;
                Team = team;
            }

            /*if(dgTeam == null)
            {
                if (Level.Nearest<Duck>(position).team != null)
                {
                    dgTeam = Level.Nearest<Duck>(position).team;
                    Team = Teams.IndexOf(dgTeam);
                }
            }*/

            _sprite.flipH = flag.scale.x < 0;

            if (replacedFrame < 0)
            {
                flag.frame = Team;
            }
            else
            {
                flag.frame = replacedFrame;
            }

            ableToInteract = false;

            if (based == true)
            {
                if (owner != null)
                {
                    OnBase = false;
                    _sprite.frame = 1;
                    flag.scale = new Vec2(-owner.offDir, scale.y);
                    anchor = owner;
                    position.x = anchor.position.x - owner.offDir * 6;

                    ableToInteract = true;

                    if (owner.inputProfile.Pressed("STRAFE")) //drop
                    {
                        anchor = this;
                        owner = null;
                        dropByPlayer = true;
                    }
                    foreach (FlagBase flagBase in Level.CheckRectAll<FlagBase>(topLeft, bottomRight))
                    {                        
                        if (owner != null && owner.team == flagBase.dgTeam && dgTeam != flagBase.dgTeam && flagBase.flagOnBase)
                        {
                            owner = null;
                            anchor = null;
                            delivered = true;
                            flagBase.getPoint = true;
                        }
                    }
                    if (position.x > Level.current.bottomRight.x || position.x < Level.current.topLeft.x || position.y > Level.current.lowestPoint) //out of map bounds
                    {
                        owner = null;
                        dropByPlayer = true;
                        anchor = null;
                        ToBase = true;
                    }
                } 
                else
                {
                    _sprite.frame = 0;
                    flag.scale = new Vec2(flipHorizontal ? -1 : 1, flag.scale.y);

                    enablePhysics = true;
                    if (dropByPlayer == false && OnBase == false)
                    {
                        ToBase = true;
                    }
                    if (position.x > Level.current.bottomRight.x || position.x < Level.current.topLeft.x || position.y > Level.current.lowestPoint || position.y < Level.current.highestPoint - 50) //out of map bounds
                    {
                        ToBase = true;
                    }

                    foreach (Duck duckling in Level.CheckRectAll<Duck>(topLeft + new Vec2(-4, -4), bottomRight + new Vec2(4, 4)))
                    {
                        if (dgTeam != null)
                        {
                            if (dgTeam != duckling.team)
                            {
                                ableToInteract = true;
                                if (duckling.inputProfile.Pressed("STRAFE")) //pickup
                                {
                                    owner = duckling;
                                    dropByPlayer = false; 
                                    foreach (Flag f in Level.current.things[typeof(Flag)])
                                    {
                                        if(f != this && f.owner == duckling)
                                        {
                                            f.anchor = null;
                                            f.owner = null;
                                            f.dropByPlayer = true;
                                        }
                                    }
                                }
                            }
                            else //return
                            {
                                ToBase = true;
                                dropByPlayer = false;
                            }
                        }
                    }
                }
            }
        }
        public override void Draw()
        {
            base.Draw();

            if (!based) //Editor preview of flag for decoration purposes
            {
                flag.flipH = flipHorizontal;
                flag.scale = new Vec2(flipHorizontal ? -1 : 1, flag.scale.y);
                flag.frame = team - 1;
            }

            DrawKey();
            DrawFlag();
        }
        void DrawKey()
        {
            if (ableToInteract)
            {
                keyVisibility = Maths.LerpTowards(keyVisibility, 1, 0.04f);
            }
            else
            {
                keyVisibility = Maths.LerpTowards(keyVisibility, 0, 0.05f);
            }

            if (keyVisibility > 0)
            {
                string text = "@STRAFE@";
                Graphics.DrawString(text, position + new Vec2(-6, -36), Color.White * keyVisibility);
            }
        }
        void DrawFlag()
        {            
            //Taking flag sprite from spritesheet
            int TexHeight = 18;
            int TexWidth = 27;

            Tex2D prepTex = new Tex2D(TexWidth, TexHeight);
            Color[] texArray = new Color[TexWidth * TexHeight];
            Color[] origArray = new Color[flag.texture.width * flag.texture.height];

            flag.texture.GetData(origArray);

            for (int i = 0; i < TexHeight; i++)
            {
                for (int j = 0; j < TexWidth; j++)
                {
                    texArray[i * TexWidth + j] = origArray[i * TexWidth + j + TexWidth * TexHeight * flag.frame];
                }
            }

            //Setting up sprite
            prepTex.SetData(texArray);
            Sprite spr = new Sprite(prepTex);
            spr.flipH = flag.scale.x < 0;

            for (int i = 0; i < 14; i++)
            {
                float sinOffset = (float)Math.Sin(Graphics.frame / 10f + i * 0.38f);

                float flagOffsetX = 2.5f;
                float flagScale = 1;
                float flagRotation = 0;
                float flagHeight = 20;
                if (owner != null)
                {
                    flagScale = 0.5f;
                    flagOffsetX -= 1f;
                    //flagRotation = 0.8f * scale.x;
                    flagHeight = 6.5f;
                }

                Vec2 flagStart = position + new Vec2((spr.flipH ? -flagOffsetX : flagOffsetX) * spr.scale.x, -flagHeight * spr.scale.y);

                Graphics.Draw(spr.texture,
                    flagStart + new Vec2((i * 2) * scale.x * flagScale * (spr.flipH ? -1f : 1f), sinOffset * 3.8f * (i / 51f)),
                    new Rectangle?(new Rectangle((i * 2), 0f, 3f, 18f)),
                    Color.White,
                    flagRotation,
                    Vec2.Zero,
                    spr.flipH ? new Vec2(-scale.x, scale.y) * flagScale : new Vec2(scale) * flagScale,
                    SpriteEffects.None,
                    depth - 2);
            }
        }
    }
}
