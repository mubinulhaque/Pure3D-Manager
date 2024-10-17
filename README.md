# Pure3D Manager
Inspired by the Lucas Pure3D editor, but now cross-platform and open-source. The aim of this project is to recreate its functionality to a more modern and readily accessible standard.

## Features
- View different chunks of a `.p3d` file
- View the properties of each chunk
- View `Image`/`ImageData` chunks and the textures they represent
- Export these textures as PNG files
- View `Skeleton` chunks and the skeletons they represent
- Export these skeletons as .glTF files
- View `PrimitiveGroup` chunks and the models they represent
- Export these models as .glTF files
- View `Animation` chunks and the animations they represent
- Play animations on a previously loaded `Skeleton`

## Credits
- [Godot Engine contributors](https://godotengine.org/license/) for the Godot Engine
- [handsomematt](https://github.com/handsomematt) and [Layla](https://github.com/aylaylay) for the C# library that loads Pure3D files
    - This project makes edits to their library to add extra functionality, work better with Godot, and make it more readable for beginners.
