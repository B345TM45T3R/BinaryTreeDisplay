# Binary Tree Display
 A Binary Tree visualisation algorithm for the Windows console that I created in 2017 (our lecturer asked if we could come up with a better/clearer tree printing technique than was used in class).
 
 ![Letters tree (unordered) (screenshot)](docs/Letters%20tree%20(unordered).PNG)
 
 In short, it is not very performant: It is a brute force algorithm, so doesn't scale very well. But it was fun though!
 
 Depending on machine specs, and the structure of the tree being displayed - the alogorithm should handle trees with depths of about 10 to 18 nodes; before starting to take inconveniently long to complete.
 
 It constructs a perfect binary tree (with all the node labels allocated a uniform length (equal to the trees longest node value [char count]).
 Empty placeholder branches are constructed with \*'s to keep the tree integrity (or character alignment).  When it's all constructed, a "shrinking" process goes through the '2D array' (stringBuilder[] - that is the tree), to identify and do away with any columns that only contain " ","\*" or "\_". It currently replaces any \*'s with " " before printing to screen finally - this could easily be avoided by using " " as the placeholder to start with, instead of "\*". It was kept as "\*" for increased visualization.
 
  ![Animals tree (unordered) (screenshot)](docs/Animals%20tree%20(unordered).PNG)
 
 
 Roughly, the number of 'coordinates' to process scales at (2^n * 2(n) + 2^n); where n is depth of the tree. This excludes (horizontal) multipliers from alignment/padding/formatting and the max node value's length.
 
 ![Tree size with increasing depth (screenshot)](docs/Tree%20size%20with%20increasing%20depth.PNG)
