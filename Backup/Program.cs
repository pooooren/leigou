﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bresenham
{
    class Program
    {
        /// <summary>
        /// Main function just dances around with a few examples
        /// of using the line enumeration and then using LINQ queries to
        /// play with a circle enumeration
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LineExamples();
            CircleExamples();
        }

        /// <summary>
        /// This function iterates over the points in some gigantic lines
        /// that you wouldn't want to be storing in memory.
        /// </summary>
        public static void LineExamples(){
            // print out details for Bresenham lines in 8 directions and a single point 'line'
            testLineCases();
            
            IEnumerable<Point> longLine; // Represent an enumerable line
            // This is an example of why I am using the iterative approach
            // We'll draw a line from 0,0 to 5000000,900000--
            longLine = BresLine.RenderLine(new Point(0, 0), new Point(5000000, 900000));
            // Now iterate over the line and perform some operation
            foreach (Point myPoint in longLine)
            {
                double dummyVar = myPoint.X * Math.Sin(myPoint.X / 90);
                // Eventually our X will exceed the boundary value of 12345 in some direction
                if (Math.Abs(dummyVar) > 12345) break;
            }

            // Now output some strings
            StringBuilder sb = new StringBuilder();
            string totalString = longLine.Aggregate(sb, (acc, x) => sb.Append(x.ToString())).ToString();
            // totalString is something like 98 million characters long at this point

            if (totalString.Length < 1000)
            {
                // We could print the 98 million character string... 
                // but you could expect an OutOfMemoryException
                Console.WriteLine(totalString);
            }

            // Accumulate the SIN of all y values for no reason in particular
            Console.WriteLine("SIN(Y) aggregate: " + longLine.Aggregate(0d, (acc, pointN) => acc += Math.Sin(pointN.Y)));
        }

        /// <summary>
        /// This method performs some operations on the Midpoint Circle Algorithm
        /// enumeration results and attempts to show how you might utilize LINQ
        /// and lambda functions on it
        /// </summary>
        public static void CircleExamples(){
            StringBuilder sb = new StringBuilder(); 

            // Use midpoint circle algorithm to generate a new circle
            Point circleOrigin = new Point(0, 0);
            IEnumerable<Point> circlePoints = MidCircle.rasterCircle(circleOrigin, 25);
            // A little LINQ demo of aggregation and averaging...

            // First just write out every point to a string the slow way
            Console.WriteLine(circlePoints.Aggregate("Circle points: ", (acc, x) => acc += x.ToString()));
            // Now try the same thing with a stringbuilder as the aggregation result
            Console.WriteLine("Stringbuilder Points: " + circlePoints.Aggregate(sb, (acc, x) => sb.Append(x.ToString())).ToString());

            // Now we'll do a little testing of the circle enumeration

            // Let's get the average of all X valus
            int xAvg = circlePoints.Aggregate(0, (acc, pt) => acc += pt.X);
            // ...and the average of all Y values
            int yAvg = circlePoints.Aggregate(0, (acc, pt) => acc += pt.Y);

            // Print out the result
            Console.WriteLine("Avg X: " + (xAvg / circlePoints.Count()).ToString());
            Console.WriteLine("Avg Y: " + (yAvg / circlePoints.Count()).ToString());

            // The average of all points along a well-formed circle should be the origin
            Debug.Assert(xAvg == circleOrigin.X);
            Debug.Assert(yAvg == circleOrigin.Y);

            // In the following section we will accumulate on X and Y at the 
            // same time using a few different methods
            Point newPoint;
            
            // Accumulate using an inline anonymous function:
            //   Although the LINQ Aggregate wasn't yet available, we would've 
            //   been inclined to use a delegate like this back in C# v2.0...
            newPoint = circlePoints.Aggregate(new Point(), delegate(Point point1, Point point2) { point1.X += point2.X; point1.Y += point2.Y; return point1; });
            Console.WriteLine("anonymous function Aggregation result: " + newPoint.ToString());

            //We could use an addPoint helper function if the operations are significantly complex
            newPoint = circlePoints.Aggregate(new Point(), (acc, x) => addPoint(acc, x));
            Console.WriteLine("addPoint Aggregation result: " + newPoint.ToString());

            // Because the operation is trivial we can use this inlined
            // 'statement lambda expression' with an explicit return
            newPoint = circlePoints.Aggregate(new Point(), 
                (acc, x) => { acc.X += x.X; acc.Y += x.Y; return acc; });
            Console.WriteLine("statement lambda function Aggregation result: " + newPoint.ToString());

            // Now find the maximum combination of X and Y in the circle
            Console.WriteLine("Max X+Y point: + " + circlePoints.Max(i => i.X + i.Y));
        }

        /// <summary>
        /// A little helper method called within some aggregate lambda functions.
        /// This will let you spy on the operations in the debug output window
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point addPoint(Point point1, Point point2)
        {
            Debug.Write("addPoint params: \t" + point1.ToString() + "\t" + point2.ToString());
            point1.X += point2.X;
            point1.Y += point2.Y;
            Debug.Write("\taddPoint result: \t" + point1.ToString() + Environment.NewLine);
            return point1;
        }

        /// <summary>
        /// Dummy function to print out 9 lines... doesn't yet provide complete coverage
        /// I plan to just add some unit tests.
        /// </summary>
        public static void testLineCases()
        {
            printPointsForLine(new Point(0, 0), new Point(0, 0)); // test origin
            printPointsForLine(new Point(0, 0), new Point(0, 10)); // Test down
            printPointsForLine(new Point(0, 10), new Point(0, 0)); // Test up
            printPointsForLine(new Point(0, 0), new Point(10, 0)); // Test right
            printPointsForLine(new Point(10, 0), new Point(0, 0)); // Test left
            printPointsForLine(new Point(1, 1), new Point(10, 10)); // test down+right
            printPointsForLine(new Point(10, 10), new Point(1, 1)); // test up+left
            printPointsForLine(new Point(10, 0), new Point(1, 10)); // test up+right
            printPointsForLine(new Point(10, 10), new Point(1, 0)); // test up+left
        }

        /// <summary>
        /// This function puts a BEGIN and END tag around a list of points from a line
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        private static void printPointsForLine(Point begin, Point end)
        {
            IEnumerable<Point> myLine = BresLine.RenderLine(begin, end);
            Console.Write("* BEGIN: " + begin.ToString() + " *\t");
            printPoints(myLine);
            Console.Write("\t* END: " + end.ToString() + " *" + Environment.NewLine);
        }

        /// <summary>
        /// This function just iterates over an IEnumerable of points and prints them all
        /// to the screen...
        /// </summary>
        /// <param name="points"></param>
        private static void printPoints(IEnumerable<Point> points)
        {
            foreach (Point myPoint in points)
            {
                Console.Write(myPoint.ToString() + " ");
            }
        }
    }


    /// <summary>
    /// The BresLine class exposes a line calculated using the Bresenham Line Algorithm
    /// in an iterative approach with proper ordering of points between beginning and end
    /// </summary>
    public static class BresLine
    {
        /// <summary>
        /// This function chooses an appropriate private method for rendering the line
        /// based on the begin and end characteristics. These separate methods could be
        /// combined into a single method but I believe that runtime performance while
        /// enumerating through the points would suffer. 
        /// 
        /// (given the overhead involved with the LINQ calls it may not make much difference)
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<Point> RenderLine(Point begin, Point end)
        {
            if (Math.Abs(end.Y - begin.Y) < Math.Abs(end.X - begin.X))
            {
                // dX > dY... not steep
                if (end.X >= begin.X)
                {
                    return BresLineOrig(begin, end);
                }
                else
                {
                    return BresLineReverseOrig(begin, end);
                }
            }
            else // steep (dY > dX)
            {
                if (end.Y >= begin.Y)
                {
                    return BresLineSteep(begin, end);
                }
                else
                {
                    return BresLineReverseSteep(begin, end);
                }
            }
        }

        /// <summary>
        /// Creates a line from Begin to End starting at (x0,y0) and ending at (x1,y1)
        /// * where x0 less than x1 and y0 less than y1
        ///   AND line is less steep than it is wide (dx less than dy)
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static IEnumerable<Point> BresLineOrig(Point begin, Point end)
        {
            Point nextPoint = begin;
            int deltax = end.X - begin.X;
            int deltay = end.Y - begin.Y;
            int error = deltax / 2;
            int ystep = 1;
            if (end.Y < begin.Y)
            {
                ystep = -1;
            }
            else if (end.Y == begin.Y)
            {
                ystep = 0;
            }

            while (nextPoint.X < end.X)
            {
                if (nextPoint != begin) yield return nextPoint;
                nextPoint.X++;

                error -= deltay;
                if (error < 0)
                {
                    nextPoint.Y += ystep;
                    error += deltax;
                }
            }
        }

        /// <summary>
        /// Whenever dy > dx the line is considered steep and we have to change
        /// which variables we increment/decrement
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static IEnumerable<Point> BresLineSteep(Point begin, Point end)
        {
            Point nextPoint = begin;
            int deltax = Math.Abs(end.X - begin.X);
            int deltay = end.Y - begin.Y;
            int error = Math.Abs(deltax / 2);
            int xstep = 1;

            if (end.X < begin.X)
            {
                xstep = -1;
            }
            else if (end.X == begin.X)
            {
                xstep = 0;
            }

            while (nextPoint.Y < end.Y)
            {
                if (nextPoint != begin) yield return nextPoint;
                nextPoint.Y++;

                error -= deltax;
                if (error < 0)
                {
                    nextPoint.X += xstep;
                    error += deltay;
                }
            }
        }

        /// <summary>
        /// If x0 > x1 then we are going from right to left instead of left to right
        /// so we have to modify our routine slightly
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static IEnumerable<Point> BresLineReverseOrig(Point begin, Point end)
        {
            Point nextPoint = begin;
            int deltax = end.X - begin.X;
            int deltay = end.Y - begin.Y;
            int error = deltax / 2;
            int ystep = 1;

            if (end.Y < begin.Y)
            {
                ystep = -1;
            }
            else if (end.Y == begin.Y)
            {
                ystep = 0;
            }

            while (nextPoint.X > end.X)
            {
                if (nextPoint != begin) yield return nextPoint;
                nextPoint.X--;

                error += deltay;
                if (error < 0)
                {
                    nextPoint.Y += ystep;
                    error -= deltax;
                }
            }
        }

        /// <summary>
        /// If x0 > x1 and dy > dx we have to go from right to left and alter the routine
        /// for a steep line
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static IEnumerable<Point> BresLineReverseSteep(Point begin, Point end)
        {
            Point nextPoint = begin;
            int deltax = end.X - begin.X;
            int deltay = end.Y - begin.Y;
            int error = deltax / 2;
            int xstep = 1;

            if (end.X < begin.X)
            {
                xstep = -1;
            }
            else if (end.X == begin.X)
            {
                xstep = 0;
            }

            while (nextPoint.Y > end.Y)
            {
                if (nextPoint != begin) yield return nextPoint;
                nextPoint.Y--;

                error += deltax;
                if (error < 0)
                {
                    nextPoint.X += xstep;
                    error -= deltay;
                }
            }
        }
    }

    /// <summary>
    /// MidCircle class just attempts to take an iterative approach to the 
    /// calculation of points involved in drawing a circle
    /// </summary>
    public static class MidCircle
    {
        /// <summary>
        /// This function enumerates over a circle without doing any particular ordering.
        /// points in all cardinal directions +/- radius are drawn first, followed by 
        /// a point for each of the connecting arcs until the circle is completed.
        ///
        /// NOTE: This is good enough for my purposes (collision detection) but 
        /// others may wish to implement this algorithm in a manner that performs
        /// proper ordering--I think you'll have to look into drawing the
        /// individual component arcs.
        /// </summary>
        /// <param name="midpoint"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static IEnumerable<Point> rasterCircle(Point midpoint, int radius)
        {
            Point returnPoint = new Point();
            int f = 1 - radius;
            int ddF_x = 1;
            int ddF_y = -2 * radius;
            int x = 0;
            int y = radius;

            returnPoint.X = midpoint.X;
            returnPoint.Y = midpoint.Y + radius;
            yield return returnPoint;
            returnPoint.Y = midpoint.Y - radius;
            yield return returnPoint;
            returnPoint.X = midpoint.X + radius;
            returnPoint.Y = midpoint.Y;
            yield return returnPoint;
            returnPoint.X = midpoint.X - radius;
            yield return returnPoint;

            while (x < y)
            {
                if (f >= 0)
                {
                    y--;
                    ddF_y += 2;
                    f += ddF_y;
                }
                x++;
                ddF_x += 2;
                f += ddF_x;
                returnPoint.X = midpoint.X + x;
                returnPoint.Y = midpoint.Y + y;
                yield return returnPoint;
                returnPoint.X = midpoint.X - x;
                returnPoint.Y = midpoint.Y + y;
                yield return returnPoint;
                returnPoint.X = midpoint.X + x;
                returnPoint.Y = midpoint.Y - y;
                yield return returnPoint;
                returnPoint.X = midpoint.X - x;
                returnPoint.Y = midpoint.Y - y;
                yield return returnPoint;
                returnPoint.X = midpoint.X + y;
                returnPoint.Y = midpoint.Y + x;
                yield return returnPoint;
                returnPoint.X = midpoint.X - y;
                returnPoint.Y = midpoint.Y + x;
                yield return returnPoint;
                returnPoint.X = midpoint.X + y;
                returnPoint.Y = midpoint.Y - x;
                yield return returnPoint;
                returnPoint.X = midpoint.X - y;
                returnPoint.Y = midpoint.Y - x;
                yield return returnPoint;
            }
        }
    }


    /// <summary>
    /// You can ignore this for now... 
    /// Work in progress on DDA (Digital Differential Analyzer) algorithm
    /// </summary>
    public static class DDAFunc
    {
        /// <summary>
        /// Interpolate int values between start (xa, ya) and end (xb, yb)
        /// incrementing the X values and interpolating on Y
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IEnumerable<Point> DDAx(Point a, Point b)
        {
            Point nextPoint = new Point(a.X, a.Y);
            int x;
            float dydx = (float)(b.Y - a.Y) / (float)(b.X - a.X);
            float y = a.Y;
            for (x = a.X; x <= b.X; x++)
            {
                nextPoint.X = x;
                nextPoint.Y = (int)Math.Round(y);
                yield return nextPoint;
                y += dydx;
            }
        }

        /// <summary>
        /// Floating point interpolation betwen start (xa, ya) and end (xb ,yb)
        /// incrementing X by dx and interpolating for Y
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="dx"></param>
        /// <returns></returns>
        public static IEnumerable<PointF> DDAf(PointF a, PointF b, float dx)
        {
            PointF nextPoint = new PointF(a.X, a.Y);
            float x;
            float dydx = (float)(b.Y - a.Y) / (float)(b.X - a.X);
            float y = a.Y;
            for (x = a.X; x <= b.X; x+=dx)
            {
                nextPoint.X = x;
                nextPoint.Y = (int)Math.Round(y);
                yield return nextPoint;
                y += dydx;
            }
        }
    }
}
