namespace DuckGame.C44P
{
	[EditorGroup("ADGM|Guns")]
	public class Thunderbuss : TampingWeapon
	{
		public Thunderbuss(float xval, float yval) : base(xval, yval)
		{
			wideBarrel = true;
			ammo = 99;
			_ammoType = new ATShrapnel();
			_ammoType.range = 140f;
			_ammoType.rangeVariation = 40f;
			_ammoType.accuracy = 0.01f;
			_numBulletsPerFire = 4;
			_ammoType.penetration = 0.4f;
			_type = "gun";
			graphic = new Sprite(GetPath("Sprites/Items/Weapons/Newblunderbuss.png"), 0f, 0f);
			center = new Vec2(19f, 5f);
			collisionOffset = new Vec2(-8f, -3f);
			collisionSize = new Vec2(16f, 7f);
			_barrelOffsetTL = new Vec2(34f, 4f);
			_fireSound = "shotgun";
			_kickForce = 2f;
			_fireSoundPitch = 0.6f;

			_fireRumble = RumbleIntensity.Light;
			_holdOffset = new Vec2(10f, -1f);
			editorTooltip = "Old-timey shotgun, takes approximately 150 years to reload.";
		}

		public override void Update()
		{
			base.Update();
			if (!_tamped)
            {
                _canRaise = false;
				//_tampTime += 0.002f;
            }
            else
            {
				_canRaise = true;
			}

		}

		public override void OnPressAction()
		{
			if (_tamped)
			{
				base.OnPressAction();
				int num = 0;
				for (int i = 0; i < 14; i++)
				{
					MusketSmoke smoke = new MusketSmoke(barrelPosition.x - 16f + Rando.Float(32f), barrelPosition.y - 16f + Rando.Float(32f));
					smoke.depth = 0.9f + i * 0.001f;
					if (num < 6)
					{
						smoke.move -= barrelVector * Rando.Float(0.1f);
					}
					if (num > 5 && num < 10)
					{
						smoke.fly += barrelVector * (2f + Rando.Float(7.8f));
					}
					_tampBoost += 0.05f; 
					Level.Add(smoke);
					num++;
				}

				_tampInc = 0f;
				_tampTime = (infinite.value ? 0.5f : 0.5f);
				_tamped = false;
				return;
			}
			if (!_raised)
			{
				Duck duckOwner = owner as Duck;
				if (duckOwner != null)
				{
					duckOwner.sliding = false;
					_rotating = true;
				}
			}
		}
	}
}
