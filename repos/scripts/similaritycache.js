/**
 * @license
 * similaritycache v1.0.0
 *
 * Minified using terser v5.3.5
 * Original file: ceresbakalite/similarity/repos/scripts/similaritycache.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { similaritycache }

var similaritycache = {};
(function(cache)
{
    'use strict';

    let getObjectProperties = function(object, str = '')
    {
        for (let property in object) str += property + ': ' + object[property] + ', ';
        return str.replace(/, +$/g,'');
    }

    let namedCache = 'similarity-cache'; // manual override only
    let wkr = function() { return attribute; } // cache worker

    wkr.installCache = true; // manual override only
    wkr.exploreCache = false; // manual override only
    wkr.listCache = false; // manual override only
    wkr.deleteCache = false; // manual override only
    wkr.replaceCache = false; // manual override only

    let viewCachedRequests = function()
    {
        caches.open(namedCache).then(function(cache)
        {
            cache.keys().then(function(cachedRequests)
            {
                console.log('exploreCache: ' + cachedRequests); // [Request, Request]
            });

        });

    }

    let listExistingCacheNames = function()
    {
        console.log(getObjectProperties(worker));

        caches.keys().then(function(cacheKeys)
        {
            console.log('listCache: ' + cacheKeys); // eg: namedCache
        });

    }

    let deleteCacheByName = function()
    {
        caches.delete(namedCache).then(function()
        {
            console.log(namedCache + ' - Cache successfully deleted');
        });

    }

    let deleteOldCacheVersions = function()
    {
        caches.keys().then(function(cacheNames)
        {
            return Promise.all(
                cacheNames.map(function(cacheName)
                {
                    if(cacheName != namedCache)
                    {
                        return caches.delete(cacheName);
                    }
                })

            );

        });

    }

    let installCache = function()
    {
        window.addEventListener('install', function(e)
        {
            e.waitUntil(
                caches.open(namedCache).then( function(cache)
                {
                    return cache.addAll([
                        '/index.html',
                        '/README.md',
                        '/shell/README.md',
                        '/repos/scripts/index.md',
                        '/repos/scripts/SyncIndex.html',
                        '/repos/scripts/SyncRepos.html',
                        '/repos/scripts/SyncShell.html',
                        '/repos/scripts/SyncSlide.html',
                        '/repos/scripts/IncludeNavigation.inc',
                        '/repos/scripts/IncludeHeader.inc',
                        '/repos/scripts/IncludeFooter.inc',
                        '/repos/scripts/similarityscriptframe.js',
                        '/repos/scripts/similarityscript.js',
                        '/repos/mods/similaritycache.min.js',
                        '/repos/mods/cereslibrary.min.js',
                        '/stylesheets/stylesheets/github-light.css',
                        '/stylesheets/stylesheets/normalize.css',
                        '/stylesheets/stylesheets/stylesheet.css',
                        '/stylesheets/stylesheets/similarityIndex.css',
                        '/images/slide/NAVScreenViews01.png',
                        '/images/slide/NAVScreenViews02.png',
                        '/images/slide/NAVScreenViews03.png',
                        '/images/slide/NAVScreenViews04.png',
                        '/images/slide/NAVScreenViews05.png',
                        '/images/slide/NAVScreenViews06.png',
                        '/images/slide/NAVScreenViews07.png',
                        '/images/slide/NAVScreenViews08.png',
                        '/images/slide/README.md',
                        '/images/GitHubForkMe_Right_Transparent.png',
                        '/images/NAVBridgeViewHeaderBackground.png',
                        '/images/NAVSimilarityLogoRepos.png',
                        '/images/NAVSimilarityLogoScripts.png',
                        '/images/NAVSimilarityLogoShell.png',
                        '/images/NAVSimilarityLogoStyle.png',
                        '/images/AppleTouchIcon.png',
                        '/images/NAVPinIconDisabled.png',
                        '/images/NAVPinIconEnabled.png',
                        '/images/NAVLampPeriscope.png',
                        '/images/NAVLampHalf.png',
                        '/images/NAVLampRing.png',
                        '/images/NAVLampBulb.png',
                        '/images/NAVCreate.png',
                        '/images/NAVCogs.png',
                        'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.min.js',
                        'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.lib.min.js',
                        'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
                        'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css'
                    ]);

                })

            );

        });

        window.addEventListener('fetch', function(e)
        {
            e.respondWith(caches.match(e.request).then(function(response)
            {
                // setting max-age here appears to be unresponsive
                response.set('Cache-Control', 'public, max-age 604800, s-maxage 43200');

                // caches.match() always resolves
                // but in case of success response will have value
                if (response !== undefined)
                {
                    return response;

                } else {

                    return fetch(e.request).then(function (response)
                    {
                        // response may be used only once
                        // we need to save clone to put one copy in cache
                        // and serve second one
                        let responseClone = response.clone();

                        // setting max-age here appears to be unresponsive
                        responseClone.set('Cache-Control', 'public, max-age 604800, s-maxage 43200');

                        caches.open(namedCache).then(function (cache)
                        {
                            cache.put(e.request, responseClone);
                        });

                        return response;

                    }).catch(function () {

                        return caches.match('/images/NAVCogs.png');

                    });

                }

            }));

        });

    }

    if ('caches' in window)
    {
        // install cache
        if (wkr.installCache) installCache();

        // view requests that have already been cached
        if (wkr.exploreCache) viewCachedRequests();

        // list existing cache names
        if (wkr.listCache) listExistingCacheNames();

        // delete cache by name
        if (wkr.deleteCache) deleteCacheByName();

        // delete old versions of cache
        if (wkr.replaceCache) deleteOldCacheVersions();

    }

})(similaritycache);
