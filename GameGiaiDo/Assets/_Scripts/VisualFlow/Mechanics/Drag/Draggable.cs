using Lean.Touch;
using UnityEngine;

namespace VisualFlow
{
    public class Draggable : LeanDragTranslate
    {
        public bool LockX;
        public bool LockY;
        public bool LockZ;

        private Vector3 startingPos;

        protected override void Awake()
        {
            base.Awake();
            this.startingPos = this.trans.position;
            
        }

        protected override void Update()
        {
            base.Update();
            Vector3 pos = this.trans.position;

            if (this.LockX)
            {
                pos.x = this.startingPos.x;
            }

            if (this.LockY)
            {
                pos.y = this.startingPos.y;
            }

            if (this.LockZ)
            {
                pos.z = 0;
            }

            this.trans.position = Vector3.MoveTowards(this.trans.position, pos, Time.deltaTime * 50f);
        }
    }
}