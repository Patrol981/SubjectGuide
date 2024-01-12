# Subject Guide
## Recruitment task for Unity Mid Developer
### Controls
- WSAD - Camera movment
- Left Mouse Button - Select Subject to be <b> Guide </b>
- Right Mouse Button - Order Subject to move to desired position

### About Game Flow
Application is being set up in this order:

- It gets data from scriptable object about map dimensions
- Then map is being resized according to this data
- Next obstacles are being rendered
- Then subjects
- And last but not least <b> nav grid </b> is being baked

After those steps, user can play the game (save, load, select guide from panel or by clicking on desired subject)

### Save Location
C:\Users\USER_NAME\Documents\SubjectGuide\Saves
