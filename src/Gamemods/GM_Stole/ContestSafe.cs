using System;

namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|GameMode ST")]
    public class ContestSafe : PhysicsObject, IPlatform
    {
        public SpriteMap _sprite;
        public SpriteMap _letter;

        public bool contested;
        public float contesting;

        public EditorProperty<int> Letter;
        public ContestSafe(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/SafeStealing/Safe.png"), 32, 32, false);
            graphic = _sprite;
            _sprite.frame = 0;
            _letter = new SpriteMap(Mod.GetPath<C44P>("Sprites/Gamemods/SafeStealing/PointLoader.png"), 17, 17, false);
            center = new Vec2(16f, 21.5f);
            collisionSize = new Vec2(16f, 19f);
            collisionOffset = new Vec2(-8f, -9.5f);
            thickness = 2f;
            _sprite.AddAnimation("idle", 1f, false, new int[] {
                0
            });
            _sprite.AddAnimation("opening", 0.1f, true, new int[] {
                0,
                1
            });
            _sprite.AddAnimation("left", 1f, false, new int[] {
                2
            });
            _sprite.AddAnimation("right", 1f, false, new int[] {
                3
            });
            _letter.CenterOrigin();
            Letter = new EditorProperty<int>(0, this, 0, 4, 1, null);

            hugWalls = WallHug.Floor;
        }
        public override void Update()
        {
            if (contested == false) 
            {
                Duck d = Level.CheckCircle<Duck>(position, 32f);
                if (d != null)
                {
                    if (d.inputProfile.Down("SHOOT") && d.holdObject == null && d.HasEquipment(typeof(TEquipment)))
                    {
                        if(contesting == 0)
                        {
                            SFX.Play(GetPath("SFX/SafeOpen.wav"), 1f, 0f, 0f);
                        }
                        contesting += 0.01666666f;
                        _sprite.SetAnimation("opening");

                    }
                    else
                    {
                        contesting = 0f;
                        _sprite.SetAnimation("idle");
                    }
                    if (contesting > 4f)
                    {
                        contested = true;
                        if (d.position.x < position.x)
                        {
                            _sprite.SetAnimation("left");
                        }
                        if (d.position.x > position.x)
                        {
                            _sprite.SetAnimation("right");
                        }
                    }
                }
                else
                {
                    contesting = 0f;
                    _sprite.SetAnimation("idle");
                }
            }
            if (contesting < 0)
            {
                contesting = 0;
            }
            base.Update();
        }
        public override void Draw()
        {
            base.Draw();

            if (contesting > 0 && !contested)
            {
                int TexHeight = 17;
                int TexWidth = 17;

                Tex2D tex = new Tex2D(TexWidth, TexHeight);

                Color[] texArray = new Color[TexWidth * TexHeight];

                for (int i = 0; i < TexHeight; i++)
                {
                    for (int j = 0; j < TexWidth; j++)
                    {
                        float pixAlpha = 0.7f;

                        double polarAngle = 0;
                        double polarX = (i - TexHeight * 0.5f);
                        double polarY = (j - TexWidth * 0.5f);

                        double polarRange = Math.Sqrt(polarX * polarX + polarY * polarY);

                        if (polarX != 0)
                        {
                            if (polarX > 0 && polarY >= 0)
                            {
                                polarAngle = Math.Atan(polarY / polarX);
                            }
                            if (polarX > 0 && polarY < 0)
                            {
                                polarAngle = Math.Atan(polarY / polarX) + Math.PI * 2;
                            }
                            if (polarX < 0)
                            {
                                polarAngle = Math.Atan(polarY / polarX) + Math.PI;
                            }
                        }
                        else
                        {
                            if (polarY != 0)
                            {
                                polarAngle = Math.PI * (1 - 0.5f * Math.Sign(polarY));
                            }
                        }


                        if (polarAngle > (contesting / 4f) * Math.PI * 2 || polarRange > 8.5f || polarRange < 6.5f)
                        {
                            pixAlpha = 0;
                        }

                        texArray[i * TexWidth + j] = new Color(255, 255, 255) * pixAlpha;
                    }
                }
                tex.SetData(texArray);

                Sprite circle = new Sprite(tex);
                circle.CenterOrigin();

                Graphics.Draw(circle, position.x, position.y - 21f);
            }
            _letter.frame = Letter;
            Graphics.Draw(_letter, position.x, position.y - 21f);
        }
    }
}
