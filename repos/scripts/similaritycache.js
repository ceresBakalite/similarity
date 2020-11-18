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

import { generic, store } from '../mods/cereslibrary.min.js';

var similaritycache = {};
(function()
{
    'use strict';

    if (!store.available) return generic.inspect({ type: generic.error, notification: 'Cache is unavailable' });

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
        '/similarityrepos/scripts/SyncRepos.html',
        '/similarityrepos/scripts/SyncShell.html',
        '/similarityrepos/scripts/SyncSlide.html',
        '/similarityrepos/scripts/IncludeNavigation.inc',
        '/similarityrepos/scripts/IncludeHeader.inc',
        '/similarityrepos/scripts/IncludeFooter.inc',
        '/similarityrepos/scripts/similarityscriptframe.js',
        '/similarityrepos/scripts/similarityscript.js',
        '/similarityrepos/mods/similaritycache.min.js',
        '/similarityrepos/mods/cereslibrary.min.js',
        '/similaritystylesheets/stylesheets/github-light.css',
        '/similaritystylesheets/stylesheets/normalize.css',
        '/similaritystylesheets/stylesheets/stylesheet.css',
        '/similaritystylesheets/stylesheets/similarityIndex.css',
        '/similarityimages/slide/NAVScreenViews01.png',
        '/similarityimages/slide/NAVScreenViews02.png',
        '/similarityimages/slide/NAVScreenViews03.png',
        '/similarityimages/slide/NAVScreenViews04.png',
        '/similarityimages/slide/NAVScreenViews05.png',
        '/similarityimages/slide/NAVScreenViews06.png',
        '/similarityimages/slide/NAVScreenViews07.png',
        '/similarityimages/slide/NAVScreenViews08.png',
        '/similarityimages/slide/README.md',
        '/similarityimages/GitHubForkMe_Right_Transparent.png',
        '/similarityimages/NAVBridgeViewHeaderBackground.png',
        '/similarityimages/NAVSimilarityLogoRepos.png',
        '/similarityimages/NAVSimilarityLogoScripts.png',
        '/similarityimages/NAVSimilarityLogoShell.png',
        '/similarityimages/NAVSimilarityLogoStyle.png',
        '/similarityimages/AppleTouchIcon.png',
        '/similarityimages/NAVPinIconDisabled.png',
        '/similarityimages/NAVPinIconEnabled.png',
        '/similarityimages/NAVLampPeriscope.png',
        '/similarityimages/NAVLampHalf.png',
        '/similarityimages/NAVLampRing.png',
        '/similarityimages/NAVLampBulb.png',
        '/similarityimages/NAVCreate.png',
        '/similarityimages/NAVCogs.png',
        'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.min.js',
        'https://ceresbakalite.github.io/ceres-sv/prod/ceres-sv.lib.min.js',
        'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
        'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css'
    ];

    // install cache
    if (action.installCache) store.installCache(action.namedCache, urlArray);

    // view requests that have already been cached
    if (action.exploreCache) store.viewCachedRequests(action.namedCache);

    // list existing cache names
    if (action.listCache) store.listExistingCacheNames();

    // delete old versions of cache
    if (action.replaceCache) store.deleteOldCacheVersions(action.namedCache);

    // delete cache by name
    if (action.deleteCache) store.deleteCacheByName(action.namedCache);

    // list the action properties
    if (action.listAction) console.log(generic.getObjectProperties(action));

}).call(similaritycache);
