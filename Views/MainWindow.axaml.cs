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
using Avalonia.Interactivity;
using Path = System.IO.Path;

namespace SS14_I2P.Views
{
    public partial class MainWindow : Window
    {
        public const string char_transparent = "░░";
        public const string char_black = "██";
        public const string color_open = "[color={0}]";
        public const string color_close = "[/color]";
        private ReadImage? read_image;
        private string last_known_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private string last_image = "";
        
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

        private async void Upload_Image_Clicked(object? sender, RoutedEventArgs args)
        {
            var selectedImages = await GetImage();
            if (selectedImages.Length == 0)
                return;

            last_image = selectedImages[0];
            ProvidedPathBox.Text = last_image;
            UploadedImageBox.Source = new Bitmap(last_image);
            read_image = new ReadImage(last_image);
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Get modifying attribute(s).
            bool ignoreEndTags = Button_EndTags.IsChecked != null && Button_EndTags.IsChecked.Value;
            
            // Convert
            read_image?.Convert(ignoreEndTags: ignoreEndTags);
            
            // Output
            ASCIIOutputBox.Text = read_image?.GetText();
            Text_CharCount.Text = $"Character Count: {read_image?.GetText().Length.ToString()}";
        }

        private async Task<string[]> GetImage()
        {
            TopLevel? toplevel = GetTopLevel(this);
            if (toplevel == null)
                return [];

            IStorageProvider storage_provider = toplevel.StorageProvider;
            IStorageFolder? well_known_folder = await storage_provider.TryGetFolderFromPathAsync(last_known_folder);

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
            if (file.Count <= 0)
                return [];

            var path = file.Select(storageFile => storageFile.Path.LocalPath.ToString()).ToArray();

            var lastDirectory = Path.GetDirectoryName(path[0]);
            if (lastDirectory != null)
            {
                last_known_folder = lastDirectory;
            }

            return path;
        }

        #region Button Events

        private async void Save_To_File_Clicked(object? sender, RoutedEventArgs args)
        {
            read_image?.Print();
            await BrieflyRecontentAsync(SaveButton, "Saved!");
        }

        private async void Copy_To_Clip_Clicked(object? sender, RoutedEventArgs args)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (Clipboard != null) await Clipboard.SetTextAsync(ASCIIOutputBox.Text);
            });
            await BrieflyRecontentAsync(ClipboardButton, "Copied!");
        }

        /// Sets a message in a content control and reverts it back.
        public static async Task BrieflyRecontentAsync(ContentControl contentControl, string message)
        {
            if (contentControl.Content == null)
                return;

            string? prev_message = contentControl.Content.ToString();
            contentControl.Content = message;
            await Task.Delay(3000); // Wait for 3 seconds (3000 ms)
            contentControl.Content = prev_message;
        }

        #endregion

        private void Button_EndTags_OnClick(object? sender, RoutedEventArgs e) => UpdateUI();
    }
}
#pragma warning restore IDE0051 // Remove unused private members