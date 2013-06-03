using System;

namespace Cocos2D
{
    public class CCBezierBy : CCActionInterval
    {
        protected CCBezierConfig m_sConfig;
        protected CCPoint m_startPosition;

        public CCBezierBy (float t, CCBezierConfig c)
        {
            InitWithDuration(t, c);
        }

        protected CCBezierBy (CCBezierBy bezierBy) : base (bezierBy)
        {
            InitWithDuration(bezierBy.m_fDuration, bezierBy.m_sConfig);

        }

        protected bool InitWithDuration(float t, CCBezierConfig c)
        {
            if (base.InitWithDuration(t))
            {
                m_sConfig = c;
                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCBezierBy;
                base.Copy(zone);
                
                ret.InitWithDuration(m_fDuration, m_sConfig);
                
                return ret;
            }
            else
            {
                return new CCBezierBy(this);
            }

        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                float xa = 0;
                float xb = m_sConfig.ControlPoint1.X;
                float xc = m_sConfig.ControlPoint2.X;
                float xd = m_sConfig.EndPosition.X;

                float ya = 0;
                float yb = m_sConfig.ControlPoint1.Y;
                float yc = m_sConfig.ControlPoint2.Y;
                float yd = m_sConfig.EndPosition.Y;

                float x = Bezierat(xa, xb, xc, xd, time);
                float y = Bezierat(ya, yb, yc, yd, time);
                m_pTarget.PositionX = m_startPosition.X + x;
                m_pTarget.PositionY = m_startPosition.Y + y;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            CCBezierConfig r;

            r.EndPosition = -m_sConfig.EndPosition;
            r.ControlPoint1 = m_sConfig.ControlPoint2 + -m_sConfig.EndPosition;
            r.ControlPoint2 = m_sConfig.ControlPoint1 + -m_sConfig.EndPosition;

            var action = new CCBezierBy(m_fDuration, r);
            return action;
        }

        // Bezier cubic formula:
        //	((1 - t) + t)3 = 1 
        // Expands to�� 
        //   (1 - t)3 + 3t(1-t)2 + 3t2(1 - t) + t3 = 1 
        protected float Bezierat(float a, float b, float c, float d, float t)
        {
            return (float) ((Math.Pow(1 - t, 3) * a +
                             3 * t * (Math.Pow(1 - t, 2)) * b +
                             3 * Math.Pow(t, 2) * (1 - t) * c +
                             Math.Pow(t, 3) * d));
        }
    }

    public struct CCBezierConfig
    {
        public CCPoint ControlPoint1;
        public CCPoint ControlPoint2;
        public CCPoint EndPosition;
    }
}