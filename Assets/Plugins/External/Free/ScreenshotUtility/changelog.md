1.0.4

Added:
- Option under preferences to use Unity's native screenshot capturing (game view only). This captures an exact image as it appears on screen (including UI)
    * When enabled, resolution and aspect ratio option are hidden. This must be set in the game view itself.
    
Changed:
- Cameras whose GameObject is hidden will no longer appear in the camera list
- If multiple cameras exist, the main camera will automatically be selected when first opening the window

1.0.3

Added:
- Package description and assembly definition, allowing the tool to be used as a package

Changed:
- Capturing now takes into account if HDR is used, allowing Bloom effects to also be captured
- Cinemachine is now automatically taken into account, if installed through the Package Manager

Fixed:
- Screenshot only showing the skybox in some specific cases

1.0.2 

Added:
- Dropdown for aspect ratio (16:9/21:9/32:9) 
- Preference menu options:    
* Allow the "Print-screen" keyboard button for capturing    
* Use 24 Hour or AM/PM timestamps  

Changed:
- If the camera has clear flags set to "Depth Only", transparency is kept in the screenshot (requires post-processing to be disabled) 

1.0.1 

Added:
- "Open Settings" shortcut to window's context menu.

Fixed:
- Issue where target folder was not respected, and screenshots were saved in the project folder. 
- Error about missing icon in Unity 2020.1+  