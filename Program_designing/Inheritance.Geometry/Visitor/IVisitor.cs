using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.Geometry.Visitor
{
    public interface IVisitor
    {
        Body VisitBall(Ball ball);

        Body VisitRectangularCuboid(RectangularCuboid ball);

        Body VisitCylinder(Cylinder ball);

        Body VisitCompoundBody(CompoundBody ball);
    }
}
