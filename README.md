# BinaryTreeDisplay
 A Binary Tree visualisation algorithm for the Windows console that I created in 2017 (our lecturer asked if we could come up with a better/clearer tree printing technique than was used in class).
 
 In short, it is not very performant: It is a brute force algorithm, so doesn't scale very well.
 
 It constructs a perfect binary tree (with all the node labels allocated a uniform length (equal to the trees longest node value [char count]).
 Empty placeholder brances are constructed with \*'s to keep the tree integrity.  When it's all constructed, a "shrinking" process goes through the '2D array' (stringBuilder[] - that is the tree), to identify and do away with any columns that only contain " ","*" or "_".
 
 Roughly, the number of 'coordinates' to process scales at (2^n * 2(n) + 2^n); where n is depth of the tree. This excludes (horizontal) multipliers from alignment/padding/formatting and the max node value's length. 
