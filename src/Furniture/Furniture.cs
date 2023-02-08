namespace DuckGame.C44P
{
    public class C4Furniture : PhysicsObject
    {
        public EditorProperty<bool> obstacle;
        public EditorProperty<bool> disablealpha;
        public EditorProperty<float> Thickness;
        public C4Furniture(float xpos, float ypos) : base(xpos, ypos)
        {
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            obstacle = new EditorProperty<bool>(false);
            disablealpha = new EditorProperty<bool>(false);
            Thickness = new EditorProperty<float>(2f, null, 0f, 10f, 1f);
        }
        public override void Initialize()
        {
            base.Initialize();
            thickness = Thickness;
        }
        public override void Update()
        {
            base.Update();
            if(obstacle == false)
            {
                thickness = Thickness;
            }
            else
                thickness = 0f;
            if (disablealpha == false)
            {
                Duck po = Level.CheckRect<Duck>(topLeft, bottomRight);
                Duck d = Level.Nearest<Duck>(x, y, po);
                if (po != null)
                {
                    alpha = 0.3f;
                }
                else if (d != null)
                {
                    if ((d.position - position).length < 48f)
                        alpha = 0.9f - 0.1f * (48f - (d.position - position).length) / 8f;
                }
                else if (po == null && d == null)
                {
                    alpha = 0.9f;
                }
            }
            foreach (Duck d in Level.CheckRectAll<Duck>(topLeft, bottomRight))
            {
                if(d != null && obstacle == true)
                {
                    if(d.crouch)
                    {
                        thickness = Thickness;
                        if (d.position.x > position.x)
                        {
                            d.position.x = position.x - _collisionOffset.x;
                            d.offDir = 1;
                        }
                        else
                        {
                            d.position.x = position.x + _collisionOffset.x;
                            d.offDir = -1;
                        }
                    }
                }
            }
        }
    }
}
