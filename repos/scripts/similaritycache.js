export { similaritycache }

var similaritycache = {};
(function(cache)
{
    'use strict';

    if ('caches' in window)
    {
        window.addEventListener('install', function(event)
        {
          event.waitUntil(
            caches.open('similarity-cache').then( function(cache)
            {
              return cache.addAll([
                './index.html',
                './repos/scripts/SyncIndex.html',
                './repos/scripts/SyncRepos.html',
                './repos/scripts/SyncShell.html',
                './repos/scripts/SyncSlide.html',
                'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
                'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css',
                'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css',
                './stylesheets/github-light.css',
                './stylesheets/normalize.css',
                './stylesheets/stylesheet.css',
                './stylesheets/similarityIndex.css',
                './similarityscriptframe.js',
                './similarityscript.js',
                './cerescookies.js',
                './images/slide/NAVScreenViews01.png',
                './images/slide/NAVScreenViews02.png',
                './images/slide/NAVScreenViews03.png',
                './images/slide/NAVScreenViews04.png',
                './images/slide/NAVScreenViews05.png',
                './images/slide/NAVScreenViews06.png',
                './images/slide/NAVScreenViews07.png',
                './images/slide/NAVScreenViews08.png',
                './images/GitHubForkMe_Right_Transparent.png',
                './images/NAVBridgeViewHeaderBackground.png',
                './images/NAVSimilarityLogoScripts.png',
                './images/NAVPinIconDisabled.png',
                './images/NAVPinIconEnabled.png',
                './images/NAVLampPeriscope.png',
                './images/AppleTouchIcon.png',
                './images/NAVLampRing.png',
                './images/NAVLampBulb.png',
                './images/NAVCogs.png'
              ]);

            })

          );

        });

        window.addEventListener('fetch', function(event)
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

                return caches.match('./images/NAVCogs.png');

              });
            }

          }));

        });

    }

})(similaritycache);
