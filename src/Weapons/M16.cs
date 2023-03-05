namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Guns")]
    public class M16 : Gun
    {
		public M16(float xval, float yval) : base(xval, yval)
		{
			ammo = 20;
			_ammoType = new ATHighCalMachinegun();
			_type = "gun";
			graphic = new Sprite(GetPath("Sprites/Items/Weapons/m16.png"), 0f, 0f);
			center = new Vec2(16f, 15f);
			collisionOffset = new Vec2(-8f, -3f);
			collisionSize = new Vec2(18f, 10f);
			_barrelOffsetTL = new Vec2(32f, 14f);
			_fireSound = "deepMachineGun2";
			_fullAuto = true;
			_fireWait = 0.8f;
			_kickForce = 1.5f;
			_fireRumble = RumbleIntensity.Kick;
			loseAccuracy = 0.08f;
			maxAccuracyLost = 0.6f;

			_fireSoundPitch = 1.25f;

			_holdOffset = new Vec2(0f, -1f);
		}
	}
}
