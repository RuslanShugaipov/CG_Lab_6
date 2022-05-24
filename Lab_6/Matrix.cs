using System;

namespace Lab_6
{
    class Matrix
    {
        private float[,] data;

        private int m;
        public int M
        {
            get { return m; }
        }

        private int n;
        public int N
        {
            get { return n; }
        }

        public Matrix(float[,] data)
        {
            m = data.GetUpperBound(0) + 1;
            n = data.Length / m;
            this.data = data;
        }

        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            data = new float[m, n];
        }

        public float this[int x, int y]
        {
            get { return data[x, y]; }
            set { data[x, y] = value; }
        }

        public static float determinant2x2(float[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
        }

        public static float determinant3x3(Matrix matrix)
        {
            float determinant1 = determinant2x2(new float[2, 2] { { matrix[1, 1], matrix[1, 2] }, { matrix[2, 1], matrix[2, 2] } });
            float determinant2 = determinant2x2(new float[2, 2] { { matrix[1, 0], matrix[1, 2] }, { matrix[2, 0], matrix[2, 2] } });
            float determinant3 = determinant2x2(new float[2, 2] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } });
            return matrix[0, 0] * determinant1 - matrix[0, 1] * determinant2 + matrix[0, 2] * determinant3;
        }

        public static Matrix transpose3x3(Matrix matrix)
        {
            Matrix result = matrix;
            float temp = 0f;
            temp = result[0, 1];
            result[0, 1] = result[1, 0];
            result[1, 0] = temp;

            temp = result[0, 2];
            result[0, 2] = result[2, 0];
            result[2, 0] = temp;

            temp = result[1, 2];
            result[1, 2] = result[2, 1];
            result[2, 1] = temp;

            return result;
        }

        public static Matrix algAdditions3x3(Matrix matrix)
        {
            Matrix result = new Matrix(new float[3, 3]
            {
                { 0f, 0f, 0f },
                { 0f, 0f, 0f },
                { 0f, 0f, 0f }
            });

            result[0, 0] = determinant2x2(new float[2, 2] { { matrix[1, 1], matrix[1, 2] }, { matrix[2, 1], matrix[2, 2] } });
            result[0, 1] = -determinant2x2(new float[2, 2] { { matrix[1, 0], matrix[1, 2] }, { matrix[2, 0], matrix[2, 2] } });
            result[0, 2] = determinant2x2(new float[2, 2] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } });

            result[1, 0] = -determinant2x2(new float[2, 2] { { matrix[0, 1], matrix[0, 2] }, { matrix[2, 1], matrix[2, 2] } });
            result[1, 1] = determinant2x2(new float[2, 2] { { matrix[0, 0], matrix[0, 2] }, { matrix[2, 0], matrix[2, 2] } });
            result[1, 2] = -determinant2x2(new float[2, 2] { { matrix[0, 0], matrix[0, 1] }, { matrix[2, 0], matrix[2, 1] } });

            result[2, 0] = determinant2x2(new float[2, 2] { { matrix[0, 1], matrix[0, 2] }, { matrix[1, 1], matrix[1, 2] } });
            result[2, 1] = -determinant2x2(new float[2, 2] { { matrix[0, 0], matrix[0, 2] }, { matrix[1, 0], matrix[1, 2] } });
            result[2, 2] = determinant2x2(new float[2, 2] { { matrix[0, 0], matrix[0, 1] }, { matrix[1, 0], matrix[1, 1] } });

            return result;
        }

        public static Matrix inverse3x3(Matrix matrix)
        {
            Matrix result;
            float determinant = determinant3x3(matrix);
            Matrix algAdditions = algAdditions3x3(matrix);
            Matrix algAdditionsT = transpose3x3(algAdditions);
            result = algAdditionsT;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    result[i, j] /= determinant;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix matrixA, Matrix matrixB)
        {
            if (matrixA.N != matrixB.M)
            {
                throw new ArgumentException("Matrixes cannot be multiplied!");
            }
            var result = new Matrix(matrixA.M, matrixB.N);
            for (int i = 0; i < matrixA.M; ++i)
            {
                for (int k = 0; k < matrixB.N; ++k)
                {
                    for (int j = 0; j < matrixB.M; ++j)
                    {
                        result[i, k] += matrixA[i, j] * matrixB[j, k];
                    }
                }
            }
            return result;
        }
    }
}