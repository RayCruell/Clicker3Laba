using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Clicker3Laba
{
    public class CPlayer
    {
        private bool canClick;
        private double timeBeforeClick;
        private CCountdownTimer countdownTimer;

        public CPlayer(double timeBeforeClick = 0.5)
        {
            this.timeBeforeClick = timeBeforeClick;
            this.canClick = true;
            this.countdownTimer = new CCountdownTimer(timeBeforeClick);
        }

        public void mouseClick(Point mousePosition)
        {
            if (canClick)
            {
                canClick = false;
                countdownTimer.Start(timeBeforeClick);
            }
        }

        public void countdownEnded()
        {
            canClick = true;
        }

        public void update(double delta)
        {
            countdownTimer.update(delta);
            if (countdownTimer.IsFinished())
            {
                countdownEnded();
            }
        }

        public void increaseSpeed(double speedModifier)
        {
            timeBeforeClick = Math.Max(0.1, timeBeforeClick * speedModifier);
            countdownTimer.SetDuration(timeBeforeClick);
        }

        public bool CanClick()
        {
            return canClick;
        }

        public double GetRemainingCooldown()
        {
            return countdownTimer.getTime();
        }
    }
}
