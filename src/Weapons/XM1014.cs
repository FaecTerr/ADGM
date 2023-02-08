namespace DuckGame.C44P
{
	[EditorGroup("ADGM|Guns")]
	public class XM1014 : Gun
	{
		public XM1014(float xval, float yval) : base(xval, yval)
		{
			ammo = 7;
			_ammoType = new ATShotgun();
			wideBarrel = true;
			_type = "gun";
			graphic = new Sprite(GetPath("Sprites/Items/Weapons/xm1014.png"), 0f, 0f);
			center = new Vec2(16f, 16f);
			collisionOffset = new Vec2(-8f, -3f);
			collisionSize = new Vec2(16f, 6f);
			_barrelOffsetTL = new Vec2(30f, 14f);
			_fireSound = "shotgunFire2";
			_kickForce = 4f;
			_fireRumble = RumbleIntensity.Light;
			_numBulletsPerFire = 6;

			_holdOffset = new Vec2(3f, 1f);
		}

		public override void Update()
		{
			base.Update();
		}

		public override void OnPressAction()
		{
			if (ammo > 0)
			{
				PopShell(false);
			}
			base.OnPressAction();
			return;
		}

		public override void Draw()
		{
			base.Draw();
			Vec2 bOffset = new Vec2(13f, -2f);
		}
	}
}
