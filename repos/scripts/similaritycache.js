/**
 * @license
 * similaritycache v1.0.0
 *
 * Minified using terser v5.5.1
 * Original file: gh ./repos/scripts/similaritycache.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { similaritycache }

import { resource, cache } from '../mods/cereslibrary.min.js';

var similaritycache = {};
((action = { installCache: true }) => {

    if (!globalThis.hasOwnProperty('caches')) return resource.inspect({ type: resource.error, notification: 'Cache is unavailable' });

    if (!action.namedCache) action.namedCache = 'similarity-cache';

    Object.freeze(action);

    const urlArray = [
        './index.html',
        './README.md',
        './shell/README.md',
        './repos/scripts/index.md',
        './repos/scripts/SyncIndex.html',
        './repos/scripts/SyncRepos.html',
        './repos/scripts/SyncShell.html',
        './repos/scripts/SyncSlide.html',
        './repos/scripts/IncludeNavigation.inc',
        './repos/scripts/IncludeHeader.inc',
        './repos/scripts/IncludeFooter.inc',
        './repos/mods/similarityscriptframe.min.js',
        './repos/mods/similarityscript.min.js',
        './repos/mods/similaritycache.min.js',
        './repos/mods/cereslibrary.min.js',
        './stylesheets/github-light.css',
        './stylesheets/normalize.css',
        './stylesheets/stylesheet.css',
        './stylesheets/similarityIndex.css',
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
        'https://fonts.googleapis.com/css?family=Open+Sans:400,700',
        'https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.0.0/animate.min.css'
    ];

    // install cache
    if (action.installCache) cache.installCache(action.namedCache, urlArray);

    // view requests that have already been cached
    if (action.exploreCache) cache.viewCachedRequests(action.namedCache);

    // list existing cache names
    if (action.listCache) cache.listExistingCacheNames();

    // delete old versions of cache
    if (action.replaceCache) cache.deleteOldCacheVersions(action.namedCache);

    // delete cache by name
    if (action.deleteCache) cache.deleteCacheByName(action.namedCache);

    // list the action properties
    if (action.listAction) console.log(resource.getObjectProperties(action));

}).call(similaritycache);
