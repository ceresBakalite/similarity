var similaritycache = {};
(function(cache)
{
    'use strict';

    if ('caches' in window)
    {
        self.addEventListener('install', function(event)
        {
          event.waitUntil(
            caches.open('similarity-cache').then( function(cache)
            {
              return cache.addAll([
                'https://ceresbakalite.github.io/similarity/index.html',
                'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
                'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
                'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
                'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html',
                'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
                'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css',
                'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css',
                'https://ceresbakalite.github.io/similarity/stylesheets/github-light.css',
                'https://ceresbakalite.github.io/similarity/stylesheets/normalize.css',
                'https://ceresbakalite.github.io/similarity/stylesheets/stylesheet.css',
                'https://ceresbakalite.github.io/similarity/stylesheets/similarityIndex.css',
                'https://ceresbakalite.github.io/similarity/repos/scripts/similarityscriptframe.js',
                'https://ceresbakalite.github.io/similarity/repos/scripts/similarityscript.js',
                'https://ceresbakalite.github.io/similarity/repos/scripts/cerescookies.js',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews01.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews02.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews03.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews04.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews05.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews06.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews07.png',
                'https://ceresbakalite.github.io/similarity/images/slide/NAVScreenViews08.png',
                'https://ceresbakalite.github.io/similarity/images/NAVBridgeViewHeaderBackground.png',
                'https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoScripts.png',
                'https://ceresbakalite.github.io/similarity/images/GitHubForkMe_Right_Transparent.png',
                'https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoScripts.png',
                'https://ceresbakalite.github.io/similarity/images/NAVLampPeriscope.png',
                'https://ceresbakalite.github.io/similarity/images/NAVLampRing.png',
                'https://ceresbakalite.github.io/similarity/images/NAVLampBulb.png'
              ]);

            })

          );

        });

        self.addEventListener('fetch', function(event)
        {
          event.respondWith(caches.match(event.request).then(function(response)
          {
            // caches.match() always resolves
            // but in case of success response will have value
            if (response !== undefined)
            {

              return response;

            } else {

              return fetch(event.request).then(function (response) {
                // response may be used only once
                // we need to save clone to put one copy in cache
                // and serve second one
                let responseClone = response.clone();

                caches.open('similarity-cache').then(function (cache)
                {
                  cache.put(event.request, responseClone);
                });

                return response;

              }).catch(function ()
              {

                return caches.match('https://ceresbakalite.github.io/similarity/images/NAVCogs.png');

              });
            }

          }));

        });

    }

})(similaritycache);
