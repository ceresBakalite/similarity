#### Command line usage

### terser CLI

https://github.com/terser/terser

**Example 1**

*recommended usage*

        1. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -c -m reserved=['export','import','include','resource','cookies','compose','touch','similarity','similaritycache','similarityframe','cache'] -f quote_style=1 --module --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts',url='cereslibrary.min.js.map'"

        2. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similarityscript.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similarityscript.min.js -c -m reserved=['export','import','include','resource','cookies','compose','touch','similarity','similaritycache','similarityframe','cache'] -f quote_style=1 --module --source-map "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts',url='similarityscript.min.js.map'"

        3. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similarityscriptframe.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similarityscriptframe.min.js -c -m reserved=['export','import','include','resource','cookies','compose','touch','similarity','similaritycache','similarityframe','cache'] -f quote_style=1 --module --source-map "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts',url='similarityscriptframe.min.js.map'"

        4. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js  -c -m reserved=['export','import','include','resource','cookies','compose','touch','similarity','similaritycache','similarityframe','cache'] -f quote_style=1 --module --source-map "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts',url='similaritycache.min.js.map'"

**Example 2**

*with source map*

        1. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\mods',url='cereslibrary.min.js.map'"

        2. terser C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -c -m --source-map  "root='C:\Users\Sandy\Documents\GitHub\similarity\repos\mods',url='similaritycache.min.js.map'"

**Example 3**

*alternatively...*

        1. terser -c -m -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js

        2. terser -c -m -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritycache.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\similaritycache.js

**Example 4**

*reserve specific words*

        1. terser -c -m reserved=['get','set'] -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\cereslibrary.min.js -- C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\cereslibrary.js

<br>

***

### clean-css CLI

https://github.com/jakubpawlowicz/clean-css-cli

**Example 1**

*recommended usage*

        1. cleancss C:\Users\Sandy\Documents\GitHub\similarity\stylesheets\similaritysheet.css -o C:\Users\Sandy\Documents\GitHub\similarity\repos\mods\similaritysheet.css

<br>

***        
