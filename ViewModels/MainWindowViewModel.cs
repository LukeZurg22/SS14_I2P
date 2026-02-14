namespace SS14_I2P.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Space Station 14 Image to ASCII Converter";
        public string Provided_Path => "A URI path will appear here after selecting an image!";
        public string ClipboardSaveButtonContent { get; set; } = "Copy To Clipboard";

#pragma warning restore CA1822 // Mark members as static
    }
}
