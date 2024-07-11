# Changelog
## [v0.4.16]
1. Startnode parameter to graph events

## [v0.4.16]
1. Additional force exit choices button

## [v0.4.4]
1. Dialogue skip added
1. Default sprites for character added

## [v0.4.3]
1. Fix dialogue node initialization when starting from graph

## [v0.4.1]
1. Fix dialogue line key generation

## [v0.4.0]
1. Timeline support added
2. Sample timeline scene added

## [v0.3.15]
1. Line Speaker Data added.

## [v0.3.13]
1. Node Editor errors fixed 

## [v0.3.10] 
1. Can write dialogue without needing to setup/open localized string
1. Dialogue is synced with localized string.
1. Custom dialogue node Editor class added.
1. Multiple start nodes for dialogue
1. Dialogue Start Helper class with custom editor that shows all start nodes and allows selecting one of them.
## [v0.3.1] 
1. Dialogue nodes now used localized strings
2. Sample scene updated with UI that supports localized strings
3. Fix incorrect choicebutton index

## [v0.3.0] 
1. Added support for dialogue choices
2. Dialogue conditions and dialogue graph conditions added.
3. Refactored traversal code to not use casts
4. Fixed sample scene.

## [v0.2.1] 

### Updated
1. Refactored codebase for a better public API
2. Now Character table is based on Charater Data which are Scriptable Objects.


## [v0.1.9] - Initial Release

### Feature List
1. Added A Dialogue System Wizard
2. It makes sure that you create a character table
3. Character table CSV template Added
4. Dialogue Graph Creator Added
5. All dialogue can now be generated via a CSV File
6. Added Dialogue Graph CSV template.

### Package Changes
1. Added Samples Showing how to use the library & Some UI Code

### Code Changes

1. Now the events are based on the Manager Class instead of Dialogue Graph
2. Removed Coupling from the UI Code so that new UI code can be easily rewritten