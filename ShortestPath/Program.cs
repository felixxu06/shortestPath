using System;
using System.Linq;

namespace ShortestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(shortestPath("W.W..;P.WF.;.W.W.;.....;.WW.F"));

            Console.ReadKey();
        }
        public static string shortestPath(string mapData)
        {
            var rows = mapData.Split(';');
            string[][] records = new string[rows.Length][];
            Point current = null;
            for (int i = 0; i < rows.Length; i++)
            {
                records[i] = new string[rows[i].Length];
                // print the map
                // Console.WriteLine(rows[i]);

                if (current == null)
                {
                    var x = rows[i].IndexOf('P');
                    if (x >= 0)
                    {
                        current = new Point(x, i);
                    }
                }
            }

            if(current == null)
            {
                throw new ArgumentException("illegal input");
            }

            return MoveOut(current, rows, records);
        }

        public static string MoveOut(Point current, string[] map, string[][] records)
        {
            // out of map
            if (map.Length <= current.YAxis || current.YAxis < 0 || map[current.YAxis].Length <= current.XAxis || current.XAxis < 0)
            {
                return null;
            }

            // encounter wall
            if (map[current.YAxis][current.XAxis] == 'W')
            {
                return null;
            }
            string shorter;
            if (GetShortString(current.Path, records[current.YAxis][current.XAxis], out shorter))
            {
                return null;
            }
            records[current.YAxis][current.XAxis] = shorter;

            // print each points path
            // Console.WriteLine($"at point({current.YAxis},{current.XAxis})={records[current.YAxis][current.XAxis]}");
            if (map[current.YAxis][current.XAxis] == 'F')
            {
                return records[current.YAxis][current.XAxis];
            }
            // move up
            var upString = MoveOut(new Point(current.XAxis, current.YAxis - 1, current.Path + 'U'), map, records);

            // move down
            var downString = MoveOut(new Point(current.XAxis, current.YAxis + 1, current.Path + 'D'), map, records);

            // move left
            var leftString = MoveOut(new Point(current.XAxis - 1, current.YAxis, current.Path + 'L'), map, records);

            // move right
            var rightString = MoveOut(new Point(current.XAxis + 1, current.YAxis, current.Path + 'R'), map, records);

            var paths = new string[] { upString, downString, leftString, rightString };

            return paths.Where(_ => _ != null).OrderBy(_ => _.Length).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="shortString">is str2 shorter</param>
        /// <returns></returns>
        public static bool GetShortString(string str1, string str2, out string shortString)
        {
            var length1 = str1 == null ? int.MaxValue : str1.Length;
            var length2 = str2 == null ? int.MaxValue : str2.Length;

            var result = length1 > length2;

            shortString = result ? str2 : str1;

            return result;
        }

        public class Point
        {
            public int XAxis { get; set; }

            public int YAxis { get; set; }

            public string Path { get; set; } = string.Empty;

            public Point(int x, int y)
            {
                XAxis = x;
                YAxis = y;
            }

            public Point(int x, int y, string path)
            {
                XAxis = x;
                YAxis = y;
                Path = path;
            }
        }
    }
}
