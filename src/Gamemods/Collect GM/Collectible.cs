using System;

namespace DuckGame.C44P
{
    [EditorGroup("ADGM|GameMode Collect")]
    public class CollectibleArmor : Collectible
    {
        public CollectibleArmor(float xpos, float ypos) : base(xpos, ypos)
        {
            lootType = 2;

            _editorName = "Armor LootBox";
        }
    }
    [EditorGroup("ADGM|GameMode Collect")]
    public class CollectibleHeavy : Collectible
    {
        public CollectibleHeavy(float xpos, float ypos) : base(xpos, ypos)
        {
            lootType = 1;

            _editorName = "Heavy LootBox";
        }
    }
    [EditorGroup("ADGM|GameMode Collect")]
    public class CollectibleLight : Collectible
    {
        public CollectibleLight(float xpos, float ypos) : base(xpos, ypos)
        {
            lootType = 0;

            _editorName = "Light LootBox";
        }
    }
    [EditorGroup("ADGM|GameMode Collect")]
    public class Collectible : Holdable, IContainAThing
    {
        private SpriteMap _sprite;
        public int lootType = -1;
        public bool isSpawn = false;
        public Collectible (float xpos, float ypos) : base (xpos, ypos)
        {
            center = new Vec2(12f, 6f);
            collisionOffset = new Vec2(-6f, -6f);
            collisionSize = new Vec2(12f, 12f);
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/CollectingMode/Collectible.png"), 24, 12, false);
            thickness = 0.1f;
            _equippedDepth = 3;
            base.graphic = _sprite;
            weight = 0f;

            _editorName = "Random LootBox";
        }
        public Type contains { get; set; }
        public override void Update()
        {
            base.Update();
            int rando = Rando.Int(5);

            if (lootType == 0)
            {                
                if (rando % 2 == 0)
                {
                    contains = typeof(OldVinchester);
                }
                else
                {
                    contains = typeof(MP5);
                }
            }
            else if (lootType == 1)
            {
                if (rando % 2 == 0)
                {
                    contains = typeof(SNAIPER);
                }
                else
                {
                    contains = typeof(XM1014);
                }
            }
            else if (lootType == 2)
            {                
                if (rando % 2 == 0)
                {
                    contains = typeof(ChestPlate);
                }
                else
                {
                    contains = typeof(Helmet);
                }
            }
            else
            {
                switch (rando) 
                {
                    case 0:
                        contains = typeof(OldVinchester);
                        break;
                    case 1:
                        contains = typeof(MP5);
                        break;
                    case 2:
                        contains = typeof(SNAIPER);
                        break;
                    case 3:
                        contains = typeof(XM1014);
                        break;
                    case 4:
                        contains = typeof(ChestPlate);
                        break;
                    case 5:
                        contains = typeof(Helmet);
                        break;
                }
            }
        }
        public override void Draw()
        {
            if (lootType >= 0) 
            {
                _sprite.frame = lootType;
            }
            else
            {
                _sprite.frame = 3;
            }

            base.Draw();
        }
        public void SpawnItem()
        {
            PhysicsObject newThing;
            newThing = (Editor.CreateThing(contains) as PhysicsObject);
            if (newThing != null && isSpawn == false)
            {
                newThing.x = x;
                newThing.y = top + (newThing.y - newThing.bottom) - 6f;
                isSpawn = true;
                Level.Add(newThing);
                Level.Remove(this);
            }
        }
    }
}
