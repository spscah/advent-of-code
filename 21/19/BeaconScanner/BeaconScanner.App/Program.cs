using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BeaconScanner.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 19;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<Scanner> unaligned = new List<Scanner>();
            List<Scanner> aligned = new List<Scanner>();

            IList<Vector> currentList = null;

            foreach(string line in data) {
                if(line.StartsWith("---")) {
                    if(currentList != null)
                        unaligned.Add(new Scanner(currentList));
                    currentList = new List<Vector>();
                    
                    continue;
                }
                if(string.IsNullOrEmpty(line)) continue;

                var triple = line.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                currentList.Add((triple[0], triple[1], triple[2]));
            }
            if(currentList != null)
                unaligned.Add(new Scanner(currentList));

            // The first is implicitly aligned 
            aligned.Add(unaligned[0]);
            unaligned.RemoveAt(0);

            // Align the remaining scanners
            const int LIMIT = 12;
            while (unaligned.Count > 0)
            {
                bool found_alignment = false;
                for (int i = 0; !found_alignment && i < unaligned.Count; i++)
                {
                    for (int j = 0; !found_alignment && j < aligned.Count; j++)
                    {
                        (int cAligned, Scanner justAligned) = aligned[j].FindScannerWithEnoughInCommon(unaligned[i], LIMIT);

                        if (cAligned >= LIMIT)
                        {
                            found_alignment = true;

                            aligned.Add(justAligned);
                            unaligned.RemoveAt(i);

                            // heartbeat
                            Console.WriteLine($"Aligned Scanners = {aligned.Count}. Unaligned Scanners = {unaligned.Count} ...");
                        }
                    }
                }
            }

            // part one
            Console.WriteLine(aligned.SelectMany(s => s.Beacons).Distinct().Count());

            // Determine which two beacons are farthest apart

            int max_distance = 0;
            for (int i = 0; i < aligned.Count - 1; i++)
            {
                for (int j = i + 1; j < aligned.Count; j++)
                {
                    int nd = aligned[i].DistanceTo(aligned[j]);

                    if (nd > max_distance) {
                        max_distance = nd;
                    }
                }
            }


            // Output results
            Console.WriteLine(max_distance);
        }

    }

    enum Axis { X, Y, Z }

    enum Rotation { Angle_0, Angle_90, Angle_180, Angle_270 }
    
    class Vector : IEquatable<Vector> { 
        public int X => _x;
        public int Y => _y;
        public int Z => _z;
        
        readonly int _x;
        readonly int _y;
        readonly int _z;

        public Vector(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector() : this(0, 0, 0) { }

        public Vector Inverse()
        {
            return new Vector(-_x, -_y, -_z);
        }

        public int ManhattanDistance(Vector other) {
            return Math.Abs(_x - other._x) + Math.Abs(_y - other._y) + Math.Abs(_z - other._z);
        }

        public bool Equals(Vector other)
        {
            return _x == other._x && _y == other._y && _z == other._z;
        }

        public override int GetHashCode()
        {
            return 1097 * X + 101 * Y + Z;
        }

        public static implicit operator Vector((int x, int y, int z) value) => new Vector(value.x, value.y, value.z);
        
    }

    class Matrix {
        public readonly int[,] values = new int[4, 4];

        public Matrix() { }

        public Matrix(Matrix other)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    values[row, col] = other.values[row, col];
                }
            }
        }
        
        public static Matrix Identity()
        {
            Matrix result = new Matrix();
            for(int i = 0; i < 4; ++i) 
                result.values[i, i] = 1;

            return result;
        }

        public static Matrix Translate(Vector t)
        {
            Matrix result = Identity();
            result.values[0, 3] += t.X;
            result.values[1, 3] += t.Y;
            result.values[2, 3] += t.Z;

            return result;
        }

        public static Matrix NegativeTranslation(Matrix m)
        {
            Matrix result = new Matrix(m);
            for(int i = 0; i < 3; ++i)
                result.values[i, 3] = -m.values[i, 3];
            
            return result;
        }

        static int Cosine(Rotation angle)
        {
            return angle switch {
                Rotation.Angle_0 => 1,
                Rotation.Angle_90 => 0,
                Rotation.Angle_180 => -1,
                Rotation.Angle_270 => 0,
                _ => throw new Exception("impossible angle")
            };
        }

        static int Sine(Rotation angle)
        {
            return angle switch {
                Rotation.Angle_0 => 0,
                Rotation.Angle_90 => 1,
                Rotation.Angle_180 => 0,
                Rotation.Angle_270 => -1,
                _ => throw new Exception("impossible angle")
            };
        }

        public static Matrix Rotate(Axis axis, Rotation angle)
        {
            Matrix result = Identity();

            switch (axis)
            {
                case Axis.X:
                    result.values[1, 1] = Cosine(angle);
                    result.values[1, 2] = -Sine(angle);
                    result.values[2, 1] = Sine(angle);
                    result.values[2, 2] = Cosine(angle);
                    break;

                case Axis.Y:
                    result.values[0, 0] = Cosine(angle);
                    result.values[0, 2] = Sine(angle);
                    result.values[2, 0] = -Sine(angle);
                    result.values[2, 2] = Cosine(angle);
                    break;

                case Axis.Z:
                    result.values[0, 0] = Cosine(angle);
                    result.values[0, 1] = -Sine(angle);
                    result.values[1, 0] = Sine(angle);
                    result.values[1, 1] = Cosine(angle);
                    break;

                default: throw new Exception("invalid axis");
            }

            return result;
        }

        public Matrix Multiply(Matrix other)
        {
            Matrix result = new Matrix();
            for (int r = 0; r < 4; ++r)
                for (int c = 0; c < 4; ++c)
                    result.values[r, c] = Enumerable.Range(0,4).Sum(i => this.values[r, i] * other.values[i, c]);

            return result;
        }

        public Vector Multiply(Vector vector) {           
            int x = vector.X * this.values[0, 0]
                + vector.Y * this.values[0, 1]
                + vector.Z * this.values[0, 2]
                + this.values[0, 3];

            int y = vector.X * this.values[1, 0]
                + vector.Y * this.values[1, 1]
                + vector.Z * this.values[1, 2]
                + this.values[1, 3];

            int z = vector.X * this.values[2, 0]
                + vector.Y * this.values[2, 1]
                + vector.Z * this.values[2, 2]
                + this.values[2, 3];

            return (x,y,z);
        }
    }

    internal class Scanner {
        readonly Vector _scanner;
        readonly IList<Vector> _beacons;

        private Scanner(Vector s, IList<Vector> b) {
            _scanner = s;
            _beacons = b;
        }

        public Scanner(IList<Vector> data) {
            _scanner = (0, 0, 0);
            _beacons = data;
        }

        public IEnumerable<Vector> Beacons { 
            get { 
                foreach(Vector v in _beacons)
                    yield return v;
            }
        }

        Scanner Apply(Matrix operation) {
            return new Scanner(operation.Multiply(_scanner), _beacons.Select(b => operation.Multiply(b)).ToList()); 
        }

        // Those 24 mind-bending rotation operations
        static IList<Matrix> EquivalenceRotationOperations()
        {
            IList<Matrix> result = new List<Matrix>();

            IList<(Axis, Rotation)> orientation_rotations = new List<(Axis, Rotation)> {
                    (Axis.X, Rotation.Angle_0),
                    (Axis.X, Rotation.Angle_90),
                    (Axis.X, Rotation.Angle_180),
                    (Axis.X, Rotation.Angle_270),

                    (Axis.Y, Rotation.Angle_90),
                    (Axis.Y, Rotation.Angle_270),
                };

            foreach ((Axis, Rotation) orientation_rotation in orientation_rotations)
            {
                Matrix orientation_rotation_matrix = Matrix.Rotate(orientation_rotation.Item1, orientation_rotation.Item2);
                foreach (Rotation spin_rotation in (Rotation[]) Enum.GetValues(typeof(Rotation))) {
                    Matrix spin_rotation_matrix = Matrix.Rotate(Axis.Z, spin_rotation);
                    result.Add(orientation_rotation_matrix.Multiply(spin_rotation_matrix));
                }
            }

            return result;
        }

        IList<Matrix> EquivalenceTranslationOperations()
        {
            return _beacons.Select(b => Matrix.Translate(b.Inverse())).ToList();
        }

        IList<Scanner> EquivalentRotations { 
            get {

                return EquivalenceRotationOperations().Select(o => Apply(o)).ToList();
            }
        }       

        public (int, Scanner) FindScannerWithEnoughInCommon(Scanner other, int limit) {

            (int, Scanner) current = (limit-1, null);

            // Loop considering each beacon in this is the origin
            foreach(Matrix op in EquivalenceTranslationOperations()) {

                Scanner this_translated = Apply(op);

                // Loop considering each beacon in other is the origin.
                foreach(Matrix otherOp in other.EquivalenceTranslationOperations()) {
                    // Loop considering all possible rotations in other
                    foreach (Scanner equivalent_other in other.Apply(otherOp).EquivalentRotations)
                    {
                        int equivalent_beacons = this_translated._beacons.Select(f => equivalent_other._beacons.Count(second_beacon => second_beacon.Equals(f))).Sum();
                        if (equivalent_beacons > current.Item1)
                        {
                            current = (equivalent_beacons, equivalent_other.Apply(Matrix.NegativeTranslation(op)));
                        }
                    }
                }
            }

            return current;
        }

        internal int DistanceTo(Scanner other)
        {
            return this._scanner.ManhattanDistance(other._scanner);
        }
    }
}