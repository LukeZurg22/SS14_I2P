#pragma warning disable IDE0051 // Remove unused private members
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using SS14_I2P.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;

namespace SS14_I2P.Views
{
    public partial class MainWindow : Window
    {
        public const string char_transparent = "░░";
        public const string char_black = "██";
        public const string color_open = "[color={0}]";
        public const string color_close = "[/color]";
        private ReadImage read_image;

        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.SetBitmapInterpolationMode(UploadedImageBox, BitmapInterpolationMode.None);
        }

        public FilePickerFileType ImageAll { get; } = new("All Images")
        {
            Patterns = ["*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.webp"],
            AppleUniformTypeIdentifiers = ["public.image"],
            MimeTypes = ["image/*"]
        };

        private async void Upload_Image_Clicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var image = (await GetImage())[0];
            if (image == null)
                return;
            read_image = new(image);
            ProvidedPathBox.Text = image;
            UploadedImageBox.Source = new Bitmap(image);
            ASCIIOutputBox.Text = read_image.GetText();
        }

        string last_known_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        async Task<string[]>? GetImage()
        {
            var storage_provider = GetTopLevel(this).StorageProvider;
            var well_known_folder = await storage_provider.TryGetFolderFromPathAsync(last_known_folder);

            // Check if the well-known folder was retrieved successfully
            if (well_known_folder == null)
            {
                // Handle the case when the Pictures folder is not available
                throw new Exception("Pictures folder not found.");
            }

            var file = await storage_provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select Image File",
                AllowMultiple = false,
                SuggestedStartLocation = well_known_folder,
                FileTypeFilter = [ImageAll]
            });


            // If the result is not null, extract the file paths
            if (file != null && file.Count > 0)
            {
                var path = file.Select(file => file.Path.LocalPath.ToString()).ToArray();
                last_known_folder = Path.GetDirectoryName(path[0]);
                return path;
            }

            return null;
        }

        #region Button Events
        private async void Save_To_File_Clicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            
            read_image.Print();
            await BrieflyRecontentAsync(SaveButton, "Saved!");
        }

        private async void Copy_To_Clip_Clicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await Clipboard.SetTextAsync(ASCIIOutputBox.Text
);
            });
            await BrieflyRecontentAsync(ClipboardButton, "Copied!");
        }

        public static async Task BrieflyRecontentAsync(ContentControl? ctrl, string message)
        {
            string? prev_message = ctrl.Content.ToString();
            ctrl.Content = message;
            await Task.Delay(3000); // Wait for 3 seconds (3000 ms)
            ctrl.Content = prev_message;

        }
        #endregion
    }
}
#pragma warning restore IDE0051 // Remove unused private members
