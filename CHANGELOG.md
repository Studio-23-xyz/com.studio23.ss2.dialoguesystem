# Changelog
## [0.4.19] 
Added tool to export dialogue localization text in unity's format according to dialogue graph order 

## [0.3.10] 
1. Start from choice nodes allowed
2. Enhanced the dialogue system to support a more dynamic flow, including handling of dialogue advancement and choices.
3. Improved user interface for dialogue editing, providing clearer visuals for different node types and better error messaging.

## [0.3.2]
1. Start from choice nodes allowed

## [0.3.1] 
1. Dialogue choices added.
2. Conditions added.
3. Graph traversal refactored
   
## [v0.2.1] - Character expressions

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
