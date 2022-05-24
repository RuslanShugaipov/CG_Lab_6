using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lab_6
{
    class Cube
    {
        public Cube(float side, int[] centerPoint)
        {
            this.side = side;
            this.centerPoint = new Matrix(new float[1, 4] { { centerPoint[0], centerPoint[1], centerPoint[2], 1 } });
            vertecies = new Matrix[]
            {
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] + side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] + side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] + side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } }),
                new Matrix(new float[1, 4] { { this.centerPoint[0, 0] - side / 2, this.centerPoint[0, 1] - side / 2, this.centerPoint[0, 2] - side / 2, this.centerPoint[0, 3] } })
            };
            V = new Matrix(4, 6);
            CheckFaces();
        }

        private void CheckFaces()
        {
            Matrix C;
            Matrix X = new Matrix(new float[3, 3]);
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    X[0, j] = vertecies[indicies2[i][0]][0, j];
                    X[1, j] = vertecies[indicies2[i][1]][0, j];
                    X[2, j] = vertecies[indicies2[i][2]][0, j];
                }
                C = coeffs(X);
                V[0, i] = C[0, 0];
                V[1, i] = C[1, 0];
                V[2, i] = C[2, 0];
                V[3, i] = -1;
            }
        }

        // TODO
        public void Draw(Graphics graphics, Pen pen, PictureBox pictureBox1, float I)
        {
            CheckFaces();
            GetNormals();
            GetIntensity(I);
            Matrix E = new Matrix(new float[1, 4] { { 0, 0, -1, 0 } });
            Matrix EV = E * V;

            for (int j = 0; j < 6; ++j)
            {
                if (EV[0, j] < 0)
                    continue;
                PointF A = new PointF(vertecies[indicies2[j][0]][0, 0], vertecies[indicies2[j][0]][0, 1]);
                PointF B = new PointF(vertecies[indicies2[j][1]][0, 0], vertecies[indicies2[j][1]][0, 1]);
                PointF C = new PointF(vertecies[indicies2[j][2]][0, 0], vertecies[indicies2[j][2]][0, 1]);
                PointF D = new PointF(vertecies[indicies2[j][3]][0, 0], vertecies[indicies2[j][3]][0, 1]);

                PointF up = A;
                if (up.Y > B.Y)
                    up = B;
                if (up.Y > C.Y)
                    up = C;
                if (up.Y > D.Y)
                    up = D;

                PointF down = A;
                if (down.Y < B.Y)
                    down = B;
                if (down.Y < C.Y)
                    down = C;
                if (down.Y < D.Y)
                    down = D;

                PointF left = A;
                if (left.X > B.X)
                    left = B;
                if (left.X > C.X)
                    left = C;
                if (left.X > D.X)
                    left = D;

                PointF right = A;
                if (right.X < B.X)
                    right = B;
                if (right.X < C.X)
                    right = C;
                if (right.X < D.X)
                    right = D;

                float Kup = (left.Y - up.Y) / (left.X - up.X);
                float Bup = -(up.X * left.Y - left.X * up.Y) / (left.X - up.X);

                float Kleft = (down.Y - left.Y) / (down.X - left.X);
                float Bleft = -(left.X * down.Y - down.X * left.Y) / (down.X - left.X);

                float Kdown = (right.Y - down.Y) / (right.X - down.X);
                float Bdown = -(down.X * right.Y - right.X * down.Y) / (right.X - down.X);

                float Kright = (up.Y - right.Y) / (up.X - right.X);
                float Bright = -(right.X * up.Y - up.X * right.Y) / (up.X - right.X);

                for (float k = up.Y; k <= down.Y; ++k)
                {
                    for (float i = left.X; i <= right.X; ++i)
                    {
                        if ((Kup * i + Bup - k <= 0) &&
                            (Kleft * i + Bleft - k >= 0) &&
                            (Kdown * i + Bdown - k >= 0) &&
                            (Kright * i + Bright - k <= 0))
                        {
                            ((Bitmap)pictureBox1.Image).SetPixel((int)(i + 120), (int)(k + 130), Color.FromArgb((int)Intensities[j], 0, 0));
                            continue;
                        }
                    }
                }
            }
        }

        public void rotationX(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationX = new Matrix(new float[4, 4]{
                {    1,                         0,                         0, 0 },
                {    0,    (float)Math.Cos(angle),    (float)Math.Sin(angle), 0 },
                {    0, -((float)Math.Sin(angle)),    (float)Math.Cos(angle), 0 },
                {    0,                         0,                         0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationX;
            }
        }

        public void rotationY(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationY = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), 0, -((float)Math.Sin(angle)), 0 },
                {                         0, 1,                         0, 0 },
                {    (float)Math.Sin(angle), 0,    (float)Math.Cos(angle), 0 },
                {                         0, 0,                         0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationY;
            }
        }

        public void rotationZ(double angle)
        {
            angle = angle * (Math.PI / 180);
            Matrix rotationZ = new Matrix(new float[4, 4]{
                {    (float)Math.Cos(angle), (float)Math.Sin(angle), 0, 0 },
                { -((float)Math.Sin(angle)), (float)Math.Cos(angle), 0, 0 },
                {                         0,                      0, 1, 0 },
                {                         0,                      0, 0, 1 }
            });
            for (int i = 0; i < 8; ++i)
            {
                vertecies[i] = vertecies[i] * rotationZ;
            }
        }

        private void GetVertexNormals()
        {
            for (int i = 0; i <= 7; ++i)
            {
                Matrix normal = new Matrix(new float[1, 3] { { 0, 0, 0 } });
                for (int j = 0; j <= 5; ++j)
                {
                    if (Array.Exists<int>(indicies2[j], (int x) => x == i))
                    {
                        normal[0, 0] += V[0, j] / 3;
                        normal[0, 1] += V[1, j] / 3;
                        normal[0, 2] += V[2, j] / 3;
                    }
                }
                vertexNormals[i] = normal;
            }
        }

        private void GetVertexIntensity()
        {
            float I = 100;
            float Kd = 0.5f;
            for (int i = 0; i <= 7; ++i)
            {
                vertexIntensities[i] = I * Kd * (vertexNormals[i][0, 2] /
                    (float)Math.Sqrt(Math.Pow(vertexNormals[i][0, 0], 2) + Math.Pow(vertexNormals[i][0, 1], 2) + Math.Pow(vertexNormals[i][0, 2], 2)));
            }
        }

        private void GetNormals()
        {
            for (int i = 0; i < 6; ++i)
            {
                Matrix normal = new Matrix(new float[1, 3] { { 0, 0, 0 } });
                normal[0, 0] = V[0, i];
                normal[0, 1] = V[1, i];
                normal[0, 2] = V[2, i];
                normals[i] = normal;
            }
        }

        private void GetIntensity(float I)
        {
            float Kd = 0.9f;
            float cosTheta = 1;
            for (int i = 0; i < 6; ++i)
            {
                cosTheta = normals[i][0, 2] / (float)Math.Sqrt(Math.Pow(normals[i][0, 0], 2) + Math.Pow(normals[i][0, 1], 2) + Math.Pow(normals[i][0, 2], 2));
                Intensities[i] = I * Kd * Math.Abs(cosTheta);
            }
        }

        private Matrix coeffs(Matrix matrix)
        {
            Matrix matrixInv = Matrix.inverse3x3(matrix);
            Matrix d = new Matrix(new float[3, 1] { { -1 }, { -1 }, { -1 } });
            return matrixInv * d;
        }

        private Matrix[] vertexNormals = new Matrix[]{
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3])
        };

        private Matrix[] normals = new Matrix[]
        {
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3]),
                new Matrix(new float[1, 3])
        };

        private float[] Intensities = new float[6] { 0, 0, 0, 0, 0, 0 };

        private float[] vertexIntensities = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

        private int[,] indicies =
        {
            //Front face
            {0, 1, 1, 2, 2, 3, 3, 0 },
            //Back face
            {4, 5, 5, 6, 6, 7, 7, 4 },
            //Top face
            {3, 2, 2, 6, 6, 7, 7, 3 },
            //Bottom face
            {0, 1, 1, 5, 5, 4, 4, 0 },
            //Right face
            {1, 5, 5, 6, 6, 2, 2, 1 },
            //Left face
            {0, 4, 4, 7, 7, 3, 3, 0 }
        };

        private int[][] indicies2 =
    {
            //Front face
            new int[] {0, 1, 2, 3},
            //Back face
            new int[] {4, 5, 6, 7},
            //Top face
            new int[] {3, 2, 6, 7},
            //Bottom face
            new int[] {0, 1, 5, 4},
            //Right face
            new int[] {1, 5, 6, 2},
            //Left face
            new int[] {0, 4, 7, 3}
        };

        private Matrix V;

        private float side;
        public float Side
        {
            get { return side; }
            set { side = value; }
        }
        private Matrix[] vertecies;
        private Matrix centerPoint;
        private SolidBrush[] brushes =
        {
            new SolidBrush(Color.Red),
            new SolidBrush(Color.Orange),
            new SolidBrush(Color.Yellow),
            new SolidBrush(Color.Green),
            new SolidBrush(Color.Blue),
            new SolidBrush(Color.Purple)
        };
    }
}