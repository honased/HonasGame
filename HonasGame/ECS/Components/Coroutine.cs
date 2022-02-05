using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components
{
    public class Coroutine : Component
    {
        private IEnumerator<double> _routine;
        private double _timer;

        public bool Dead { get; private set; } = true;

        public Coroutine(Entity parent, IEnumerator<double> routine) : base(parent)
        {
            _routine = routine;
            _timer = 0.0f;
        }

        public void Start()
        {
            if(Dead)
            {
                Dead = false;
                _timer = 0.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Dead)
            {
                if (_timer <= 0.0f)
                {
                    Dead = !_routine.MoveNext();
                    if (Dead) return;

                    _timer += _routine.Current;
                }

                _timer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
