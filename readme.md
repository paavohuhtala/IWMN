# I Want More Names - IWMN

A mod for [Cities: Skylines](http://www.citiesskylines.com/) that allows loading custom localisation files from newline-separated plaintext files. It comes with it's own simplistic package manager, which allows installing localisation packages directly from GitHub.

The mod currently supports adding following kinds of localisation strings:
* First names
* Surnames
* Random chirps

## How to build & install
The build process is somewhat awkward at the moment.

### Windows
Requirements:
* A legal copy of the game
* A MinGW/MSYS installation (with make) in PATH
* An up-to-date installation of Mono in PATH

To build and install the mod, run **build.bat**.

### Linux - Untested

Requirements:
* A legal copy of the game
* An up-to-date installation of Mono

To build and install the mod, run **./build.sh**.

## Binaries
* Available in Steam Workshop soon(ish).

## Localisation packages
* [IWMN-Finnish](https://github.com/paavohuhtala/IWMN-Finnish)
  * About ~900 Finnish first names and 1000 Finnish last names.
* [IWMN-Chirps](https://github.com/paavohuhtala/IWMN-Chirps)
  * Community curated additional random chirps. Contributors welcome!

## Todo
* Documentation
* A Visual Studio solution / project file
  * [This could be useful](https://www.reddit.com/r/CitiesSkylinesModding/comments/2ypcl5/guide_using_visual_studio_2013_to_develop_mods/)
* More localisation options: businesses, factories, town and district names etc.
* Code quality improvements
* Better error handling in package manager

## License
See [LICENSE.txt](LICENSE.txt).
