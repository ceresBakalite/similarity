#### Command line usage

### terser CLI

https://github.com/terser/terser

**Example 1**

*recommended usage*

        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -c -m
        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cerescookies.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cerescookies.min.js -c -m
        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -c -m

**Example 2**

*with source map*

        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\mods',url='cereslibrary.min.js.map'"
        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cerescookies.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cerescookies.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\mods',url='cerescookies.min.js.map'"
        terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\mods',url='similaritycache.min.js.map'"

**Example 3**

*alternatively...*

        terser -c -m -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js
        terser -c -m -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cerescookies.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cerescookies.js
        terser -c -m -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js

<br>

***

### clean-css CLI

https://github.com/jakubpawlowicz/clean-css-cli

**Example 1**

*recommended usage*

        cleancss C:\Users\Sandy\Documents\GitHub\similarity\stylesheets\similaritysheet.css -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritysheet.css

<br>

***        
