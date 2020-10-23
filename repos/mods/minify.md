#### Command line usage

### terser CLI

https://github.com/terser/terser

**Example 1**

*recommended usage*

        terser C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv.js -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.js -c -m
        terser C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv-lib.js -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv-lib.min.js -c -m


terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -c -m
terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cerescookies.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cerescookies.min.js -c -m
terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -c -m

- [x] <sup>Update the corresponding import reference in ceres-sv.js to point to the minified ceres-sv-lib.min.js</sup>

**Example 2**

*with source map*

        terser C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv.js -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\ceres-sv\prod',url='ceres-sv.min.js.map'"
        terser C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv-lib.js -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv-lib.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\ceres-sv\prod',url='ceres-sv-lib.min.js.map'"

- [x] <sup>Update the corresponding import reference in ceres-sv.js to point to the minified ceres-sv-lib.min.js</sup>

**Example 3**

*alternatively...*

        terser -c -m -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.js -- C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv.js
        terser -c -m -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv-lib.min.js -- C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv-lib.js

- [x] <sup>Update the corresponding import reference in ceres-sv.js to point to the minified ceres-sv-lib.min.js</sup>

**Example 4**

*combine two files into one*

        terser C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv.js C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv-lib.js -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.js -c -m

- [x] <sup>Remove the corresponding import reference in ceres-sv.js</sup>

**Example 5**

*reserve specific words*

        terser -c -m reserved=['getImage','getSlide'] -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.js -- C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv.js
        terser -c -m  -o C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\prod\ceres-sv-lib.min.js -- C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\csvjs\ceres-sv-lib.js

- [x] <sup>Update the corresponding import reference in ceres-sv.js to point to the minified ceres-sv-lib.min.js</sup>

<br>

***



### clean-css CLI

https://github.com/jakubpawlowicz/clean-css-cli

**Example 1**

*recommended usage*

        cleancss C:\Users\Sandy\Documents\GitHub\ceres-sv\repos\stylesheets\ceres-sv.css -o C:\Users\Sandy\Documents\GitHub\ceres-sv\prod\ceres-sv.min.css

<br>

***        
