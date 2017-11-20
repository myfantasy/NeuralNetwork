using FreeImageAPI;
using MyFantasy.NeuralNetwork.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFantasy.NeuralNetwork.PictureTools
{
    public static class FieldExtention
    {
        public static Field LoadPictureGray(Stream picture, int width = 640, int height = 480)
        {
            FreeImageBitmap fib = new FreeImageBitmap(picture);

            Field f = new Field(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var p = GetPoint(fib, width, height, i, j);
                    f[i, j] = Math.Max(1.0 - Max(p.Item1, p.Item2, p.Item3) * 1.0 / 128, -1.0);
                }
            }

            return f;
        }
        public static Field LoadPictureSimple(Stream picture, int width = 640, int height = 480)
        {
            FreeImageBitmap fib = new FreeImageBitmap(picture);

            Field f = new Field(width, height, 3);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var p = GetPoint(fib, width, height, i, j);
                    f[i, j, 0] = Math.Max(1.0 - p.Item1 * 1.0 / 128, -1.0);
                    f[i, j, 1] = Math.Max(1.0 - p.Item1 * 1.0 / 128, -1.0);
                    f[i, j, 2] = Math.Max(1.0 - p.Item1 * 1.0 / 128, -1.0);
                }
            }

            return f;
        }

        public static Tuple<byte, byte, byte, byte> GetPoint(FreeImageBitmap fib, int width, int height, int x, int y)
        {
            double px = fib.Width * 1.0 / width * x;
            double py = fib.Height * 1.0 / height * y;

            int gx1 = (int)Math.Round(px, 0);
            int gx2 = gx1;
            if (Math.Abs(px - gx1) > 0.3)
            {
                gx2 = gx2 + Math.Sign(px - gx1);
            }

            int gy1 = (int)Math.Round(py, 0);
            int gy2 = gy1;
            if (Math.Abs(py - gy1) > 0.3)
            {
                gy2 = gy2 + Math.Sign(py - gy1);
            }

            var p1 = fib.GetPixel(gx1, gy1);
            var p2 = fib.GetPixel(gx2, gy1);
            var p3 = fib.GetPixel(gx1, gy2);
            var p4 = fib.GetPixel(gx2, gy2);

            byte r = (byte)((p1.R + p2.R + p3.R + p4.R) / 4);
            byte g = (byte)((p1.G + p2.G + p3.G + p4.G) / 4);
            byte b = (byte)((p1.B + p2.B + p3.B + p4.B) / 4);
            byte a = (byte)((p1.A + p2.A + p3.A + p4.A) / 4);

            return new Tuple<byte, byte, byte, byte>(r, g, b, a);
        }

        public static byte Max(byte v1, params byte[] vo)
        {
            byte res = v1;
            foreach (byte v in vo)
            {
                res = Math.Max(res, v);
            }
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pict_width"></param>
        /// <param name="pict_height"></param>
        /// <param name="input_width"></param>
        /// <param name="input_height"></param>
        /// <param name="out_width"></param>
        /// <param name="out_height"></param>
        /// <param name="zones">x1, y1, x2, y2</param>
        /// <returns></returns>
        public static Field ResultObjectTypeFiled(
            int pict_width = 6400, int pict_height = 4800,
            int input_width = 640, int input_height = 480,
            int out_width = 64, int out_height = 48,
            List<Tuple<int, int, int, int>> zones = null)
        {
            Field f = new Field(out_width, out_height);

            for (int i = 0; i < out_width; i++)
            {
                for (int j = 0; j < out_height; j++)
                {
                    f[i, j] = -1;
                }
            }

            if (zones != null)
                foreach (var z in zones)
                {
                    for (int i = ((z.Item1 * input_width / pict_width) * out_width / input_width); i < ((z.Item3 * input_width / pict_width) * out_width / input_width); i++)
                    {
                        for (int j = ((z.Item2 * input_height / pict_height) * out_height / input_height); j < ((z.Item4 * input_height / pict_height) * out_height / input_height); j++)
                        {
                            f[i, j] = 1;
                        }
                    }
                }

            return f;
        }
    }
}
