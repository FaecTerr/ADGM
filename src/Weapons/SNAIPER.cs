using System;

namespace DuckGame.C44P
{
	[EditorGroup("ADGM|Guns")]
	public class SNAIPER : Gun
	{
		public StateBinding _loadStateBinding = new StateBinding("_loadState", -1, false, false);
		public StateBinding _angleOffsetBinding = new StateBinding("_angleOffset", -1, false, false);
		public StateBinding _netLoadBinding = new NetSoundBinding("_netLoad");
		public NetSoundEffect _netLoad = new NetSoundEffect(new string[]
		{
			"loadSniper"
		});

		public int _loadState = -1;
		public int _loadAnimation = -1;
		public float _angleOffset;

		public Vec2 prevBarrelPos = new Vec2(37f, 3f);

		public SNAIPER(float xval, float yval) : base(xval, yval)
		{
			ammo = 5;
			_ammoType = new ATHighCalSniper();
			_type = "gun";
			graphic = new Sprite(GetPath("Sprites/Items/Weapons/awp.png"), 0f, 0f);
			center = new Vec2(16f, 4f);
			collisionOffset = new Vec2(-8f, -4f);
			collisionSize = new Vec2(16f, 8f);
			_barrelOffsetTL = new Vec2(37f, 3f);
			_fireSound = "sniper";
			_fireSoundPitch = -0.9f;
			_kickForce = 2f;
			_fireRumble = RumbleIntensity.Light;
			laserSight = true;
			_laserOffsetTL = new Vec2(37f, 4f);
			_manualLoad = true;

			_holdOffset = new Vec2(2f, -2f);

			_editorName = "AWP";
			editorTooltip = "Automatically locks on target";
		}

		public override void Update()
		{
			base.Update();
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
							_netLoad.Play(1f, 0f);
						}
					}
					else
					{
						SFX.Play("loadSniper", 1f, 0f, 0f, false);
					}
					_loadState++;
				}
				else if (_loadState == 1)
				{
					if (_angleOffset < 0.16f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0.2f, 0.15f);
					}
					else
					{
						_loadState++;
					}
				}
				else if (_loadState == 2)
				{
					handOffset.x = handOffset.x + 0.4f;
					if (handOffset.x > 4f)
					{
						_loadState++;
						Reload(true);
						loaded = false;
					}
				}
				else if (_loadState == 3)
				{
					handOffset.x = handOffset.x - 0.4f;
					if (handOffset.x <= 0f)
					{
						_loadState++;
						handOffset.x = 0f;
					}
				}
				else if (_loadState == 4)
				{
					if (_angleOffset > 0.04f)
					{
						_angleOffset = MathHelper.Lerp(_angleOffset, 0f, 0.15f);
					}
					else
					{
						_loadState = -1;
						loaded = true;
						_angleOffset = 0f;
					}
				}
			}
			if (loaded && owner != null && _loadState == -1)
			{
				laserSight = true;

				Duck nearestTarget = null;
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
					if((d.position.x > barrelPosition.x && offDir > 0 || d.position.x < barrelPosition.x && offDir < 0) 
						&& Level.CheckLine<Block>(d.position, barrelPosition) == null && Level.CheckLine<Safezone>(d.position, barrelPosition) == null)
                    {
						if(nearestTarget == null)
                        {
							nearestTarget = d;
                        }
                        else
                        {
							if((barrelPosition - d.position).length < (barrelPosition - nearestTarget.position).length)
                            {
								nearestTarget = d;
                            }
                        }
                    }
                }
				if(nearestTarget != null)
                {
					float aimAngle = (float)Math.Atan2(nearestTarget.position.y - barrelPosition.y, (nearestTarget.position.x - barrelPosition.x) * offDir);
					if (offDir < 0)
					{
						aimAngle = -aimAngle;
					}
					if (aimAngle < Math.PI * 0.05f && aimAngle > Math.PI * -0.05f)
					{
						angle = aimAngle * 0.8f;
					}
				}

				return;
			}
			laserSight = false;
		}

        public override void Fire()
		{
			if (Level.CheckLine<Safezone>(position, barrelPosition) != null || Level.CheckLine<Block>(position, barrelPosition) != null)
			{
				_barrelOffsetTL = new Vec2(20, 5);
			}
			else
			{
				_barrelOffsetTL = prevBarrelPos;
			}
			base.Fire();
		}
        public override void OnPressAction()
		{
			if (loaded)
			{
				if (Level.CheckLine<Safezone>(position, barrelPosition) != null || Level.CheckLine<Block>(position, barrelPosition) != null)
				{
					_barrelOffsetTL = new Vec2(20, 5);
				}
				else
				{
					_barrelOffsetTL = prevBarrelPos;
				}
				base.OnPressAction();
				return;
			}
			if (ammo > 0 && _loadState == -1)
			{
				_loadState = 0;
				_loadAnimation = 0;
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
