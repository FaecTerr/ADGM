using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.C44P
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("ADGM|GameMode CTF")]
    public class FlagBase : Thing
    {
        private SpriteMap _sprite;
        public new Duck owner;
        public Flag _flag;
        public Team dgTeam;

        private bool init;
        public bool getPoint = false;
        public int Team;

        public EditorProperty<int> team; 
        public bool flagOnBase
        {
            get
            {
                if (_flag != null)
                    return _flag.OnBase;
                return false;
            }
        }

        public FlagBase(float xval, float yval, Flag flag) : base(xval, yval)
        {
            _flag = flag;

            team = new EditorProperty<int>(1, this, 1, 8, 1, null, false, false);
            _sprite = new SpriteMap(GetPath("Sprites/Gamemods/CTF/FlagBase.png"), 17, 4, false);
            base.graphic = _sprite;

            center = new Vec2(8.5f, 2f);
            collisionOffset = new Vec2(-8.5f, -3f);
            collisionSize = new Vec2(17f, 4f);
            depth = -0.8f;
            hugWalls = WallHug.Floor;
        }

        public void ReassignTeam(Team t)
        {
            if(t != null && Teams.all.Contains(dgTeam) && t != dgTeam)
            {
                init = true;

                dgTeam = t;
                _flag.dgTeam = t;

                Team = Teams.active.IndexOf(t);
                _flag.Team = Team;
                _flag.init = true;
            }
        }

        public override void Update()
        {
            base.Update();
            if (!init)
            {
                init = true;
                Team = team;
            }
            if (_flag == null)
            {
                _flag = new Flag(x, y) { Team = Team, flipHorizontal = flipHorizontal };
                //if(isServerForObject)
                Level.Add(_flag);
            }

            if (dgTeam == null && Level.current.things[typeof(Duck)].Count() > 1)
            {
                if (Level.Nearest<Duck>(position).team != null)
                {
                    dgTeam = Level.Nearest<Duck>(position).team;
                    Team = Teams.IndexOf(dgTeam);
                    if(_flag != null)
                    {
                        _flag.Team = Team;
                        _flag.dgTeam = dgTeam;
                    }
                }
            }

            if (_flag != null)
            {
                _flag.Team = Team;
                _flag.based = true;
                if (_flag.delivered)
                {
                    _flag.delivered = false;
                    _flag.position = new Vec2(position.x, position.y - 27.5f);
                    _flag.OnBase = true;
                }
                if (_flag.ToBase)
                {
                    _flag.position = new Vec2(position.x, position.y - 27.5f);
                    _flag.ToBase = false;
                    _flag.OnBase = true;
                }
            }

            foreach (Flag flag in Level.CheckLineAll<Flag>(new Vec2(position.x-4f, position.y), new Vec2(position.x + 4f, position.y)))
            {
                if (flag != null && _flag != null)
                {
                    if (flag == _flag && flag.owner == null)
                    {
                        flag.position = new Vec2(position.x, position.y - 27.5f);
                        flag.OnBase = true;
                    }
                }
                else
                {
                    if (_flag != null)
                    {
                        _flag.OnBase = false;
                    }
                }
            }
        }
    }
}