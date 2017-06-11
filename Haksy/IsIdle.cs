using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haksy
{
    class Idle
    {

        int counter = 0;
        DateTime lastInput;
        TimeSpan timeToIdle;

        public Idle(TimeSpan timeToIdle)
        {
            this.timeToIdle = timeToIdle;
            KeyboardHook.UserInput += () => {
                lastInput = DateTime.Now;
            };
            MouseHax.UserInput += () => {
                lastInput = DateTime.Now;
            };

        }

        public bool IsIdle() {
            if (DateTime.Now - lastInput > timeToIdle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
