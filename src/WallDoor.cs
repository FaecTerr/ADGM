using System;
using System.Collections.Generic;

namespace DuckGame.C44P
{
	//Dirty code from duck game with some removals


	[EditorGroup("Stuff|Doors", EditorItemType.Debug)]
	public class WallDoor : Thing
	{
		protected SpriteMap _sprite;
		private List<Duck> _transportingDucks = new List<Duck>();

		private EditorProperty<int> style = new EditorProperty<int>(1, null, 1, 4, 1);

		public WallDoor(float xpos, float ypos) : base(xpos, ypos, null)
		{
			_sprite = new SpriteMap("wallDoor", 21, 30, false);
			_sprite.AddAnimation("opening", 1f, false, new int[]
			{
				1,
				2,
				3,
				4,
				5,
				6,
				6,
				6,
				6,
				6
			});
			_sprite.AddAnimation("closing", 1f, false, new int[]
			{
				5,
				4,
				3,
				2,
				1
			});
			_sprite.AddAnimation("open", 1f, false, new int[]
			{
				6
			});
			_sprite.AddAnimation("closed", 1f, false, new int[1]);
			_sprite.SetAnimation("closed");
			graphic = _sprite;
			center = new Vec2(10f, 22f);
			collisionSize = new Vec2(21f, 30f);
			collisionOffset = new Vec2(-10f, -20f);
			depth = -0.5f;
			_editorName = "Wall Door";
			_canFlip = false;
		}

		public void AddDuck(Duck d)
		{
			_transportingDucks.Add(d);
			_sprite.SetAnimation("open");

			SFX.Play("doorOpen", Rando.Float(0.8f, 0.9f), Rando.Float(-0.1f, 0.1f), 0f, false);
		}
		public void RemoveDuck(Duck d)
		{
			_transportingDucks.Remove(d);
			_sprite.SetAnimation("closing");
			SFX.Play("doorClose", Rando.Float(0.5f, 0.6f), Rando.Float(-0.1f, 0.1f), 0f, false);
		}


		public override void Update()
		{
			if(frame < 8 && style > 0)
            {
				frame += 8 * (style - 1);
            }
			foreach (Duck d in Level.CheckRectAll<Duck>(topLeft, bottomRight))
			{
				if (d.grounded && d.inputProfile.Pressed("UP", false) && !_transportingDucks.Contains(d))
				{
					_transportingDucks.Add(d);

					if (d.spriteImageIndex < 4)
					{
						SFX.Play("doorOpen", Rando.Float(0.8f, 0.9f), Rando.Float(-0.1f, 0.1f), 0f, false);
					}
					_sprite.SetAnimation("opening");
				}
			}
			if (_sprite.currentAnimation == "opening" && _sprite.finished)
			{
				_sprite.SetAnimation("open");
			}
			if (_sprite.currentAnimation == "closing" && _sprite.finished)
			{
				_sprite.SetAnimation("closed");
				SFX.Play("doorClose", Rando.Float(0.5f, 0.6f), Rando.Float(-0.1f, 0.1f), 0f, false);
			}
			if (_transportingDucks.Count == 0 && _sprite.currentAnimation != "closing" && _sprite.currentAnimation != "closed")
			{
				_sprite.SetAnimation("closing");
			}

			for (int i = 0; i < _transportingDucks.Count; i++)
			{
				Duck d2 = _transportingDucks[i];

				WallDoor transportDoor = null;
				Vec2 hit;
				if (d2.inputProfile.Pressed("LEFT", false))
				{
					transportDoor = Level.CheckRay<WallDoor>(position, position + new Vec2(-10000f, 0f), this, out hit);
				}
				if (d2.inputProfile.Pressed("RIGHT", false))
				{
					transportDoor = Level.CheckRay<WallDoor>(position, position + new Vec2(10000f, 0f), this, out hit);
				}
				if (d2.inputProfile.Pressed("UP", false))
				{
					transportDoor = Level.CheckRay<WallDoor>(position, position + new Vec2(0f, -10000f), this, out hit);
				}
				if (d2.inputProfile.Pressed("DOWN", false))
				{
					transportDoor = Level.CheckRay<WallDoor>(position, position + new Vec2(0f, 10000f), this, out hit);
				}
				if (transportDoor != null)
				{

				}
			}
				
			
			base.Update();
		}

		public override void Draw()
		{
			Graphics.DrawRect(topLeft, bottomRight, new Color(18, 25, 33), -0.6f, true, 1f);
			base.Draw();
		}
	}
}
