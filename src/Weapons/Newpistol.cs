namespace DuckGame.C44P
{
	[EditorGroup("ADGM|Guns")]
	public class NewPistol : Gun
	{
		private SpriteMap _sprite;
		public NewPistol(float xval, float yval) : base(xval, yval)
		{
			ammo = 9;
			_ammoType = new AT9mm();
			wideBarrel = true;
			barrelInsertOffset = new Vec2(0f, -1f);
			_type = "gun";
			_sprite = new SpriteMap(GetPath("Sprites/Items/Weapons/Newpistol.png"), 32, 16, false);
			_sprite.AddAnimation("idle", 1f, true, new int[1]);
			_sprite.AddAnimation("fire", 0.8f, false, new int[]
			{
				1,
				2,
				2,
				3,
				3
			});
			_sprite.AddAnimation("empty", 1f, true, new int[]
			{
				2
			});
			graphic = _sprite;
			center = new Vec2(11f, 6f);
			collisionOffset = new Vec2(-8f, -3f);
			collisionSize = new Vec2(16f, 9f);
			_barrelOffsetTL = new Vec2(18f, 2f);
			_fireSound = "pistolFire";
			_fireSoundPitch = -0.6f;
			_kickForce = 5f;
			_fireRumble = RumbleIntensity.Kick;
			_holdOffset = new Vec2(0f, 0f);
			loseAccuracy = 0.1f;
			maxAccuracyLost = 0.6f;
			//_bio = "Old faithful, the 9MM pistol.";
			_editorName = "Piston";

			_holdOffset = new Vec2(0f, -1f);
			//editorTooltip = "Your average everyday pistol. Just workin' to keep its kids fed, never bothered nobody.";
			physicsMaterial = PhysicsMaterial.Metal;
		}

		public override void Update()
		{
			if (_sprite.currentAnimation == "fire" && _sprite.finished)
			{
				_sprite.SetAnimation("idle");
			}
			base.Update();
		}

		public override void OnPressAction()
		{
			if (ammo > 0)
			{
				_sprite.SetAnimation("fire");
				for (int i = 0; i < 3; i++)
				{
					Vec2 pos = Offset(new Vec2(-9f, 0f));
					Vec2 rot = barrelVector.Rotate(Rando.Float(1f), Vec2.Zero);
					Level.Add(Spark.New(pos.x, pos.y, rot, 0.1f));
				}
			}
			else
			{
				_sprite.SetAnimation("empty");
			}
			base.Fire();
		}
	}
}
