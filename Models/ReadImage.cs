#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Diagnostics;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using SS14_I2P.Views;

namespace SS14_I2P.Models
{
    public struct Dimensions(int x, int y)
    {
        public int WIDTH { get; set; } = x;
        public int HEIGHT { get; set; } = y;
        public readonly override string ToString() => $"({WIDTH}, {HEIGHT})";
    }

    internal class ReadImage
    {
        private readonly string IMAGE_PATH;
        private readonly string TEXT_PATH;
        private Dimensions DIMENSIONS;
        private bool[,] IMAGE_MASK;

        private readonly StringBuilder text = new();
        private readonly StringBuilder add_text = new();
        private readonly StringBuilder preview = new();


        public ReadImage(string img_path, bool cut_transparent_borders)
        {
            IMAGE_PATH = img_path;
            TEXT_PATH = img_path + "_output.txt";
            Convert(cut_transparent_borders);
        }
        public ReadImage(string img_path)
        {
            IMAGE_PATH = img_path;
            TEXT_PATH = img_path + "_output.txt";
            Convert(false);
        }

        void Convert(bool cut_transparent_borders)
        {
            var image_data = GetImageData(IMAGE_PATH);
            if (cut_transparent_borders == false)
            {
                DIMENSIONS = new Dimensions(image_data.GetLength(0), image_data.GetLength(1));
                IMAGE_MASK = new bool[DIMENSIONS.WIDTH, DIMENSIONS.HEIGHT];
                for (int i = 0; i < DIMENSIONS.WIDTH; i++)
                {
                    for (int j = 0; j < DIMENSIONS.HEIGHT; j++)
                    {
                        Color color = Color.FromArgb(image_data[i, j]);
                        if (color.A == 0)
                        {
                            IMAGE_MASK[i, j] = color.A == 0;
                            // Setting all transparent pixels to white so char_transparent is colored correctly.
                            image_data[i, j] = ColorToARGB(0, 255, 255, 255);
                        }
                    }
                }
            }

            BuildAsciiImage(image_data);

        }


        private void BuildAsciiImage(int[,] image_data)    /// [WARN] Requires image dimensions on the X and Y parts of the plane.
        {
            string previous_color = "None";

            for (int y = 0; y < DIMENSIONS.HEIGHT; y++)
            {
                for (int x = 0; x < DIMENSIONS.WIDTH; x++)
                {
                    var pixel = Color.FromArgb(image_data[x, y]); // Get Pixel Color

                    if (pixel.A == 0)
                        add_text.Clear().Append(MainWindow.char_transparent);
                    else
                        add_text.Clear().Append(MainWindow.char_black);

                    var color_hex = $"{pixel.ToHex()}";

                    if (!color_hex.Equals(previous_color))
                    {
                        if (!previous_color.Equals("None"))
                        {
                            text.Append(MainWindow.color_close);
                        }

                        text.Append(string.Format(MainWindow.color_open, color_hex) + add_text);

                        previous_color = color_hex;
                    }
                    else
                    {
                        text.Append(add_text);
                    }
                    preview.Append(add_text);
                }
                text.Append('\n');
                preview.Append('\n');
            }
            if (!previous_color.Equals("None"))
            {
                text.Append(MainWindow.color_close).Append('\n'); // Close the last markup tag if we opened one
            }

        }

        public ReadImage Print()
        {
            SpitToFile();
            return this;
        }

        void SpitToFile()
        {
            // Using StreamWriter to write the string to the file
            using (StreamWriter writer = new(TEXT_PATH, false)) // 'false' means overwrite the file
            {
                writer.WriteLine(text.ToString()); // Write the content
            }

            Debug.WriteLine("File written successfully!");
        }

        private static unsafe int[,] GetImageData(string path)
        {
            if (path == null)
                throw new InvalidOperationException("Path not provided to GetImageData().");

            // Load file meta data with FileInfo
            Debug.WriteLine($"The path \"{path}\" is procured.");

            using var fileStream = File.OpenRead(path);
            using var bitmap = new Avalonia.Media.Imaging.Bitmap(fileStream);

            // Verify pixel size
            if (bitmap.PixelSize.Width == 0 || bitmap.PixelSize.Height == 0)
                throw new InvalidOperationException("Image has invalid dimensions.");

            // Create a 2D array to hold pixel data
            int width = bitmap.PixelSize.Width;
            int height = bitmap.PixelSize.Height;
            int[,] image_data = new int[width, height];

            var writeableBitmap = new WriteableBitmap(
                size: bitmap.PixelSize,
                dpi: bitmap.Dpi,
                PixelFormat.Bgra8888,
                alphaFormat: AlphaFormat.Opaque);

            // Copy the original bitmap to the WriteableBitmap
            using (var context = writeableBitmap.Lock())
            {
                bitmap.CopyPixels(writeableBitmap.Lock(), AlphaFormat.Opaque); // Draw original bitmap into WriteableBitmap
            }


            using var fb = writeableBitmap.Lock();  // Access the pixel data
            IntPtr pixelData = fb.Address;          // Getting Pointer
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Calculate the pixel index in BGRA format (4 bytes per pixel)
                    int index = (y * fb.RowBytes) + (x * 4);

                    // Read pixel values
                    byte blue = Marshal.ReadByte(pixelData, index);
                    byte green = Marshal.ReadByte(pixelData, index + 1);
                    byte red = Marshal.ReadByte(pixelData, index + 2);
                    byte alpha = Marshal.ReadByte(pixelData, index + 3);

                    ///Debug.WriteLine($"{red} {green} {blue} {alpha}");

                    // Store as an integer (ARGB format)
                    image_data[x, y] = (alpha << 24) | (red << 16) | (green << 8) | blue;
                }
            }
            return image_data;
        }
        static int ColorToARGB(byte alpha, byte red, byte green, byte blue)
        {
            // Combine components into a single ARGB integer
            return (alpha << 24) | (red << 16) | (green << 8) | blue;
        }

        internal string? GetText() => text.ToString();
        
    }

    public static class ColorExtensions
    {
        public static string ToHex(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
