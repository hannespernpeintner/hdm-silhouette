using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Silhouette.Engine;

namespace Silhouette.GameMechs.Move
{
    public class Move
    {
        private Tom _tom;

        private Vector2 _targetDistance;
        public Vector2 TargetDistance
        {
            get { return _targetDistance; }
            set { _targetDistance = value; }
        }
        public Vector2 Step { get; set; }

        public int DurationLeft { get; set; }

        // Expects the distance to move (relative to object)
        // and the duration for the distance
        public Move(Tom tom, Vector2 targetDistance, int duration) 
        {
            _tom = tom;
            Step = targetDistance / duration;
            DurationLeft = duration;
        }

        public void Update(int ms) 
        {
            DurationLeft -= ms;
            if (!TargetDistance.Equals(Vector2.Zero)) 
            {
                _tom.CharFix.Body.Position += ((Step * ms) / Level.PixelPerMeter);
            }
            
        }

        public bool Finished()
        { 
            return (DurationLeft <= 0);
        }
    }

    public class MoveStack
    {
        private List<Move> _moves = new List<Move>();

        public void push(Move move) 
        {
            _moves.Add(move);
        }

        public bool Finished()
        {
            return (_moves.Count == 0);
        }

        public void Update(GameTime gt) 
        {
            int ms = gt.ElapsedGameTime.Milliseconds;

            if(Finished())
            {
                return;
            }

            Move stackTop = _moves.ElementAt(_moves.Count - 1);
            if (stackTop != null && !stackTop.Finished()) 
            {
                stackTop.Update(ms);
            }
            else if (stackTop != null && stackTop.Finished())
            {
                _moves.Remove(stackTop);
                Update(gt);
            }
        }
        
    }
}
