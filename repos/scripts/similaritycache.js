/**
 * @license
 * similaritycache v1.0.0
 *
 * Minified using terser v5.4.0
 * Original file: ceresbakalite/similarity/repos/scripts/similaritycache.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { similaritycache }

import { generic, cachestore } from '../mods/cereslibrary.min.js';

var similaritycache = {};
(function()
{
    'use strict';

    if (!cachestore.available) return generic.inspect({ type: generic.error, notification: 'Cache is unavailable' });

    let action = function() { return attribute; }

    action.namedCache = 'similarity-cache';
    action.installCache = true;
    action.listCache = false;
    action.listAction = false;
    action.exploreCache = false;
    action.replaceCache = false;
    action.deleteCache = false;

    Object.freeze(action);

    const urlArray = [
        '/similarity/index.html',
        '/similarity/README.md',
        '/similarity/shell/README.md',
        '/similarity/repos/scripts/index.md',
        '/similarity/repos/scripts/SyncIndex.html',
        '/similarity/repos/scripts/SyncRepos.html',
        '/similarity/repos/scripts/SyncShell.html',
        '/similarity/repos/scripts/SyncSlide.html',
        '/similarity/repos/scripts/IncludeNavigation.inc',
        '/similarity/repos/scripts/IncludeHeader.inc',
        '/similarity/repos/scripts/IncludeFooter.inc',
        '/similarity/repos/scripts/similarityscriptframe.js',
        '/similarity/repos/scripts/similarityscript.js',
        '/similarity/repos/mods/similaritycache.min.js',
        '/similarity/repos/mods/cereslibrary.min.js',
        '/similarity/stylesheets/github-light.css',
        '/similarity/stylesheets/normalize.css',
        '/similarity/stylesheets/stylesheet.css',
        '/similarity/stylesheets/similarityIndex.css',
        '/similarity/images/slide/NAVScreenViews01.png',
        '/similarity/images/slide/NAVScreenViews02.png',
        '/similarity/images/slide/NAVScreenViews03.png',
        '/similarity/images/slide/NAVScreenViews04.png',
        '/similarity/images/slide/NAVScreenViews05.png',
        '/similarity/images/slide/NAVScreenViews06.png',
        '/similarity/images/slide/NAVScreenViews07.png',
        '/similarity/images/slide/NAVScreenViews08.png',
        '/similarity/images/slide/README.md',
        '/similarity/images/GitHubForkMe_Right_Transparent.png',
        '/similarity/images/NAVBridgeViewHeaderBackground.png',
        '/similarity/images/NAVSimilarityLogoRepos.png',
        '/similarity/images/NAVSimilarityLogoScripts.png',
        '/similarity/images/NAVSimilarityLogoShell.png',
        '/similarity/images/NAVSimilarityLogoStyle.png',
        '/similarity/images/AppleTouchIcon.png',
        '/similarity/images/NAVPinIconDisabled.png',
        '/similarity/images/NAVPinIconEnabled.png',
        '/similarity/images/NAVLampPeriscope.png',
        '/similarity/images/NAVLampHalf.png',
        '/similarity/images/NAVLampRing.png',
        '/similarity/images/NAVLampBulb.png',
        '/similarity/images/NAVCreate.png',
        '/similarity/images/NAVCogs.png',
        'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.min.js',
        'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
        'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css'
    ];

    // install cache
    if (action.installCache) cachestore.installCache(action.namedCache, urlArray);

    // view requests that have already been cached
    if (action.exploreCache) cachestore.viewCachedRequests(action.namedCache);

    // list existing cache names
    if (action.listCache) cachestore.listExistingCacheNames();

    // delete old versions of cache
    if (action.replaceCache) cachestore.deleteOldCacheVersions(action.namedCache);

    // delete cache by name
    if (action.deleteCache) cachestore.deleteCacheByName(action.namedCache);

    // list the action properties
    if (action.listAction) console.log(generic.getObjectProperties(action));

}).call(similaritycache);
