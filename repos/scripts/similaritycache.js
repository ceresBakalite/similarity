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
                  './README.md',
                './repos/scripts/SyncIndex.html',
                './repos/scripts/SyncRepos.html',
                './repos/scripts/SyncShell.html',
                './repos/scripts/SyncSlide.html',
                './repos/scripts/IncludeNavigation.inc',
                './repos/scripts/IncludeHeader.inc',
                './repos/scripts/IncludeFooter.inc',
                './repos/scripts/index.md',
                './repos/scripts/similarityscriptframe.js',
                './repos/scripts/similaritycache.js',
                './repos/scripts/similarityscript.js',
                './repos/scripts/cerescookies.js',
                './stylesheets/stylesheets/github-light.css',
                './stylesheets/stylesheets/normalize.css',
                './stylesheets/stylesheets/stylesheet.css',
                './stylesheets/stylesheets/similarityIndex.css',
                './images/slide/NAVScreenViews01.png',
                './images/slide/NAVScreenViews02.png',
                './images/slide/NAVScreenViews03.png',
                './images/slide/NAVScreenViews04.png',
                './images/slide/NAVScreenViews05.png',
                './images/slide/NAVScreenViews06.png',
                './images/slide/NAVScreenViews07.png',
                './images/slide/NAVScreenViews08.png',
                './images/slide/README.md',
                './images/GitHubForkMe_Right_Transparent.png',
                './images/NAVBridgeViewHeaderBackground.png',
                './images/NAVSimilarityLogoRepos.png',
                './images/NAVSimilarityLogoScripts.png',
                './images/NAVSimilarityLogoShell.png',
                './images/NAVSimilarityLogoStyle.png',
                './images/AppleTouchIcon.png',
                './images/NAVPinIconDisabled.png',
                './images/NAVPinIconEnabled.png',
                './images/NAVLampPeriscope.png',
                './images/NAVLampHalf.png',
                './images/NAVLampRing.png',
                './images/NAVLampBulb.png',
                './images/NAVCreate.png',
                './images/NAVCogs.png',
                'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.min.js',
                'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.lib.min.js',
                'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
                'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css'
              ]);

            })

          );

        });

        window.addEventListener('fetch', function(event)
        {
            event.respondWith(caches.match(event.request).then(function(response)
            {
                response.set('Cache-Control', 'public, max-age 604800, s-maxage 43200');

                // caches.match() always resolves
                // but in case of success response will have value
                if (response !== undefined)
                {

                    return response;

                } else {

                    return fetch(event.request).then(function (response)
                    {
                        // response may be used only once
                        // we need to save clone to put one copy in cache
                        // and serve second one
                        let responseClone = response.clone();

                        caches.open('similarity-cache').then(function (cache)
                        {
                            cache.put(event.request, responseClone);
                        });

                        return response;

                    }).catch(function () {

                        return caches.match('./images/NAVCogs.png');

                    });

                }

          }));

        });

    }

})(similaritycache);
