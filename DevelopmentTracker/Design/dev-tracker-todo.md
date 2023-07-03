# Development Tracker ToDo

## Functionality

--------------------------------------------------------------------

- [X] Navigation Buttons Doesn't return to it's state after clicking on other items.
- [X] Deleting a Progress should be confirmed.
- [X] Progress label should be splitted from stackProgress by a Grid for convience.
- [X] All validation errors should be handled!.
- [ ] Deselection algorithm is relatively slow.
- [ ] Finding a better structure for View in MVVM.
	- [X] Splitting `GridUpdateTracker` & `GridCreateTracker` as a standalone UserControls.
	- [ ] Make `ListTrackers` a standalone UserControl and stop using GridView because of it's rigidity.

<br />

## Design

--------------------------------------------------------------------

- [X] Progress should be represented as a Progress bar.
- [ ] Design Improvements.
- [ ] Change `ListTrackers` to a Grid of items with pictures which will provide the needed info when clicking on it, much similar to a filesystem.

<br />

## Suggestions

--------------------------------------------------------------------

- [ ] Mark Development as Complete.
	- [ ] Adding "Mark as Complete inside Context menu that will open on right click of tracker.
	- [ ] View Completed Developments.
- [X] Development class should be a Super class which will be specialized as new type later (ex. *CourseDev*, *BookDev*, *LifeDev*, ...).
	- [X] Every Development type will have a unique way of opening it's contents (*virtual method*).
- [ ] Export Journal which a button in context menu of each tracker which will export it as `.md`.
- [X] In the menu of Create Tracker there should be a `DropDownList` for `TrackerType`.
- [X] `YoutubeCourseTracker` should be added.
- [ ] Generation of UI should be dynamic in the future.
- [ ] Make letters prohibited in `TextBox`es that are for numbers OR creating a `UserControl` that can handle such behaviour.
- [ ] Try to make a Progress Calendar like in github, leetcode or codeforces.
