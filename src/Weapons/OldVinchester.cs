namespace DuckGame.C44P
{
	[EditorGroup("ADGM|Guns")]
	public class OldVinchester : Gun
	{
		public int _loadState = -1;
		public float _angleOffset;

		private SpriteMap _sprite;

		public StateBinding _loadStateBinding = new StateBinding("_loadState", -1, false, false);
		public OldVinchester(float xval, float yval) : base(xval, yval)
		{
			ammo = 2;
			_ammoType = new ATOldPistol();
			_type = "gun";
			_sprite = new SpriteMap(GetPath("Sprites/Items/Weapons/Newoldpistol.png"), 32, 32, false);
			graphic = _sprite;
			center = new Vec2(16f, 17f);
			collisionOffset = new Vec2(-8f, -4f);
			collisionSize = new Vec2(16f, 8f);
			_barrelOffsetTL = new Vec2(24f, 16f);
			_fireSound = "shotgun";
			_fireSoundPitch = 0.4f;

			_kickForce = 2f;
			_fireRumble = RumbleIntensity.Kick;
			_manualLoad = true;
			_fullAuto = true;
			_holdOffset = new Vec2(2f, 0f);
			editorTooltip = "A pain in the tailfeathers to reload, but it'll get the job done.";
		}
		public override void Update()
		{
			base.Update();
			if (ammo > 1)
			{
				_sprite.frame = 0;
			}
			else
			{
				_sprite.frame = 1;
			}
			if (!loaded && _loadState == -1)
			{
				_loadState = 0;
			}
			if (infinite.value)
			{
				UpdateLoadState();
				UpdateLoadState();
				return;
			}
			UpdateLoadState(); 
		}
		private void UpdateLoadState()
		{
			if (_loadState > -1)
			{
				if (owner == null)
				{
					if (_loadState == 3)
					{
						loaded = true;
					}
					_loadState = -1;
					_angleOffset = 0f;
					handOffset = Vec2.Zero;
				}
				if (_loadState == 0)
				{
					if (Network.isActive)
					{
						if (isServerForObject)
						{
							NetSoundEffect.Play("oldPistolSwipe");
						}
					}
					else
					{
						SFX.Play("swipe", 0.6f, -0.3f, 0f, false);
					}
					_loadState++;
					return;
				}
				if (_loadState == 1)
				{
					if (_angleOffset < 0.16f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0.2f, 0.08f);
						return;
					}
					_loadState++;
					return;
				}
				else if (_loadState == 2)
				{
					handOffset.y = handOffset.y - 0.28f;
					if (handOffset.y < -4f)
					{
						_loadState++;
						ammo = 2;
						loaded = false;
						if (!Network.isActive)
						{
							SFX.Play("shotgunLoad", 1f, 0f, 0f, false);
							return;
						}
						if (isServerForObject)
						{
							NetSoundEffect.Play("oldPistolLoad");
							return;
						}
					}
				}
				else if (_loadState == 3)
				{
					handOffset.y = handOffset.y + 0.15f;
					if (handOffset.y >= 0f)
					{
						_loadState++;
						handOffset.y = 0f;
						if (!Network.isActive)
						{
							SFX.Play("swipe", 0.7f, 0f, 0f, false);
							return;
						}
						if (isServerForObject)
						{
							NetSoundEffect.Play("oldPistolSwipe2");
							return;
						}
					}
				}
				else if (_loadState == 4)
				{
					if (_angleOffset > 0.04f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0f, 0.08f);
						return;
					}
					_loadState = -1;
					loaded = true;
					_angleOffset = 0f;
					if (isServerForObject && duck != null && duck.profile != null)
					{
						RumbleManager.AddRumbleEvent(duck.profile, new RumbleEvent(RumbleIntensity.Kick, RumbleDuration.Pulse, RumbleFalloff.None, RumbleType.Gameplay));
					}
					if (Network.isActive)
					{
						if (isServerForObject)
						{
							SFX.PlaySynchronized("click", 1f, 0.5f, 0f, false, true);
							return;
						}
					}
					else
					{
						SFX.Play("click", 1f, 0.5f, 0f, false);
					}
				}
			}
		}
		public override void OnPressAction()
		{
			if (loaded && ammo > 1)
			{
				base.OnPressAction();
				for (int i = 0; i < 4; i++)
				{
					Level.Add(Spark.New((offDir > 0) ? (x - 9f) : (x + 9f), y - 6f, new Vec2(Rando.Float(-1f, 1f), -0.5f), 0.05f));
				}
				for (int j = 0; j < 4; j++)
				{
					Level.Add(SmallSmoke.New(barrelPosition.x + offDir * 4f, barrelPosition.y));
				}
				ammo = 1;
				return;
			}
		}
		public override void Draw()
		{
			float ang = angle;
			if (offDir > 0)
			{
				angle -= _angleOffset;
			}
			else
			{
				angle += _angleOffset;
			}
			base.Draw();
			angle = ang;
		}
	}
}
