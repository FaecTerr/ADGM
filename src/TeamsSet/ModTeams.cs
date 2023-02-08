namespace DuckGame.C44P
{
    [EditorGroup("ADGM|Teams")]
    public class RandomCTE : CTEquipment
    {
        public RandomCTE(float xval, float yval) : base(xval, yval)
        {
        }
        public override void Equip(Duck d)
        {
            base.Equip(d);
            if (d != null)
            {
                int teamType = Rando.Int(1);
                CTEquipment cte = null;
                switch (teamType) 
                {
                    case 0:
                        cte = new Aqua(d.position.x, d.position.y);
                        break;
                    case 1:
                        cte = new Rainbow(d.position.x, d.position.y);
                        break;
                }
                if(cte != null)
                {
                    Level.Add(cte);
                    d.Equip(cte);
                }
                Level.Remove(this);
            }
        }
    }
    [EditorGroup("ADGM|Teams")]
    public class RandomTE : TEquipment
    {
        public RandomTE(float xval, float yval) : base(xval, yval)
        {
        }
        public override void Equip(Duck d)
        {
            base.Equip(d);
            if (d != null)
            {
                int teamType = Rando.Int(1);
                TEquipment te = null;
                switch (teamType)
                {
                    case 0:
                        te = new Carbon(d.position.x, d.position.y);
                        break;
                    case 1:
                        te = new Jungle(d.position.x, d.position.y);
                        break;
                }
                if (te != null)
                {
                    Level.Add(te);
                    d.Equip(te);
                }
                Level.Remove(this);
            }
        }
    }
    [EditorGroup("ADGM|Teams")]
    public class Aqua : CTEquipment
    {
        public Aqua(float xval, float yval) : base(xval, yval)
        {
            _editorName = "CTE Aqua";
        }
    }

    [EditorGroup("ADGM|Teams")]
    public class Rainbow : CTEquipment
    {
        public Rainbow(float xval, float yval) : base(xval, yval)
        {
            _editorName = "CTE Rainbow";
        }
    }

    [EditorGroup("ADGM|Teams")]
    public class Carbon : TEquipment
    {
        public Carbon(float xval, float yval) : base(xval, yval)
        {
            _editorName = "TE Carbon";
        }
    }

    [EditorGroup("ADGM|Teams")]
    public class Jungle : TEquipment
    {
        public Jungle(float xval, float yval) : base(xval, yval)
        {
            _editorName = "TE Jungle";
        }
    }
}