# SS14_I2P
"Space Station 14 Image to Paper",

## System Information
- .NET 8.0 SDK
- Developer OS: Windows
- Intended OS('s): Windows & Linux (Maybe MacOS? Not sure.)
- Framework: Avalonia (MVVM) v11
- Language(s): C# & XAML
- Executable Name (Windows): SS14_I2P.exe
- Executable Name (Linux):   SS14_I2P

*Please Note: If you intend to download and use this program casually, I'd advise using the latest stable **Release** version rather than cloning the source code.*

## Program Purpose
To provide one a sufficient UI to facilitate the conversion of pixel art into ASCII form for convienent usage in the game "Space Station 14" I've met people whom relied on hand-making ASCII images, which involves a lot of tedium and headache that this program aims to aleviate. I otherwise made this program because I could, and wanted to contribute to SS14's community in a way that benefits it, even if its a small addition.

## Project Goal
My goal for this program is to share it within the SS14 community as much as possible. Feel free to share with as many people you think would find use out of it.

## Limitations
- Honestly, not thoroughly tested.
- Large images just wont copy into SS14 paper, and images with too many varying-colored pixels wont copy either due to text limitation.
- Removing *all* end-tags saves space, but generally-speaking an image should be around 18x18, give or take a few pixels.
- More available space may be made by simply printing the .txt output of the image, but SS14's engine ensures you can't have a 5GB-sized paper, at least to my knowledge.
- **If the resulting paper image is jagged / indented** that's because the image is too big, and SS14 automatically indents it on the Paper. It's a SS14 issue with your UI. Best ways to fix are either to make the image itself smaller, or adjust the width of the paper.

## Alternatives
I wont lie, there are programs like this. Namely python scripts but i found those scripts not very easy to use. They're what inspired me to make a program with a clean UI that otherwise didn't exist anywhere else that I could find.

---

# Q&A
Questions I personally don't want to answer.

## But What If This Program Is Used For Harm / ERP?
- It's server griefing, or is just generally on average breaking server rules. This can be reported via a-help to your local admin.
- Harmful images can be made ingame anyway without the program. This just makes it easier, but personally I see potential pros outweighing the potential cons.
- Most importantly of all, i'm not responsible for whatever freakish things you people do with this program.
Note: This may be a wakeup call to SS14's devs to adjust the Admin Logs to accomodate ASCII images like this. These images can be written, copied, and etc. en masse WITHOUT the program, so it is only a matter of time until it becomes a concern to *someone* out there.

## Is This Gonna Steal My Data?
No. This doesn't even connect to the internet in any way, and it has no drawbacks besides the fact it takes up a tiny bit of disk space, like *any other program does*.

## About NOTES.txt
Those are personal notes I put in every project of mine. It's like a todo list, and a place to dump my thoughts about the program during development and to formalize a plan without actually using UML.
