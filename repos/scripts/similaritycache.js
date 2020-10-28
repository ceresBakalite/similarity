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

import { generic as gn, caching as ca } from '../mods/cereslibrary.min.js'

var similaritycache = {};
(function(cache)
{
    'use strict';

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
    ];

    // install cache
    if (action.installCache) ca.installCache(action.namedCache, urlArray);

    // view requests that have already been cached
    if (action.exploreCache) ca.viewCachedRequests(action.namedCache);

    // list existing cache names
    if (action.listCache) ca.listExistingCacheNames();

    // delete old versions of cache
    if (action.replaceCache) ca.deleteOldCacheVersions(action.namedCache);

    // delete cache by name
    if (action.deleteCache) ca.deleteCacheByName(action.namedCache);

    // list the action properties
    if (action.listAction) console.log(gn.getObjectProperties(action));

})(similaritycache);
