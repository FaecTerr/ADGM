using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<Team> team;

        SpriteMap sprite = new SpriteMap(GetPath<C44P>("Sprites/Gamemods/Tag.png"), 16, 16);

        public ForceTag() : base()
        {
            graphic = sprite;
            center = new Vec2(8, 8);
            collisionSize = new Vec2(16, 16); 
            collisionOffset = -collisionSize * 0.5f;
        }

        public override void Update()
        {
            base.Update();

        }
        public override void Draw()
        {
            frame = tagID;
            if(Level.current is Editor)
            {
                Graphics.DrawCircle(position, range, Color.Blue);
            }
            base.Draw();
        }
    }
}
