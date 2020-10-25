/**
 * @license
 * cereslibrary v1.0.0
 *
 * Minified using terser v5.3.5
 * Original file: ceresbakalite/similarity/repos/scripts/cereslibrary.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { generic, touch, compose, cookies, caching }

var generic = {};
(function()
{
    'use strict';

    let protean = function() { return attribute; }
    let rsc = function() { return attribute; } // local resource
    let sbl = new Map(); // local scope symbols

    setPrecursors();

    this.constant = protean; // exposed local scope resources

    Object.freeze(this.constant);

    this.windowOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }

    this.isEmptyOrNull = function(obj)
    {
        if (obj === null || obj == 'undefined') return true;

        if (this.isString(obj)) return (obj.length === 0 || !obj.trim());
        if (Array.isArray(obj)) return (obj.length === 0);
        if (obj && obj.constructor === Object) return (Object.keys(obj).length === 0);
        return !obj;
    }

    this.getBooleanAttribute = function(attribute)
    {
        if (attribute === true || attribute === false) return attribute;
        if (this.isEmptyOrNull(attribute)) return false;
        if (!this.isString(attribute)) return false;

        const token = attribute.trim().toLowerCase();

        return sbl.has(token) ? sbl.get(token) : false;
    }

    this.inspect = function(diagnostic)
    {
        if (this.isEmptyOrNull(diagnostic)) return this.inspect({ type: this.constant.error, notification: rsc.inspect });

        const lookup = {
            [this.constant.reference]: function() { if (diagnostic.logtrace) console.log('Reference: ' + this.constant.newline + this.constant.newline + diagnostic.reference); },
            [this.constant.notify]: function() { if (diagnostic.logtrace) console.log(diagnostic.notification); },
            [this.constant.error]: function() { this.errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace } ); },
            'default': 'An unexpected error has occurred...'
        };

        return lookup[diagnostic.type]() || lookup['default'];
    }

    this.errorHandler = function(error)
    {
        if (this.isEmptyOrNull(error)) return this.inspect({ type: this.constant.error, notification: rsc.errorHandler });

        const err = error.notification + ' [ DateTime: ' + new Date().toLocaleString() + ' ]';
        console.log(err);

        if (error.alert) alert(err);

        return false;
    }

    this.getObjectProperties = function(object, str = '')
    {
        for (let property in object) str += property + ': ' + object[property] + ', ';
        return str.replace(/, +$/g,'');
    }

    function setPrecursors()
    {
        sbl.set('true', true);
        sbl.set('t', true);
        sbl.set('yes', true);
        sbl.set('y', true);
        sbl.set('1', true);
        sbl.set('default', false);

        protean.reference = 1;
        protean.notify = 2;
        protean.error = 99;
        protean.isWindows = (navigator.appVersion.indexOf('Win') != -1);
        protean.newline = protean.isWindows ? '\r\n' : '\n';

        Object.freeze(protean);

        rsc.inspect = 'Error: An exception occurred in the inspect method.  The diagnostic argument was empty or null';
        rsc.errorhandler = 'Error: An exception occurred in the errorhandler method.  The error argument was empty or null';

        Object.freeze(rsc);
    }

}).call(generic);

var compose = {};
(function() {

    'use strict';

    this.composeElement = function(element)
    {
        const el = document.createElement(element.el);

        el.id = element.id;
        element.parent.appendChild(el);

        if (element.classValue) this.composeAttribute({ id: el.id, type: 'class', value: element.classValue });
        if (element.onClickEvent) this.composeAttribute({ id: el.id, type: 'onclick', value: element.onClickEvent });
        if (element.url) this.composeAttribute({ id: el.id, type: 'src', value: element.url });
        if (element.accessibility) this.composeAttribute({ id: el.id, type: 'alt', value: element.accessibility });
        if (element.markup) document.getElementById(el.id).insertAdjacentHTML('afterbegin', element.markup);
    }

    this.composeAttribute = function(attribute)
    {
        const el = document.getElementById(attribute.id);

        if (el)
        {
            const attributeNode = document.createAttribute(attribute.type);
            attributeNode.value = attribute.value;

            el.setAttributeNode(attributeNode);
        }

    }

    this.composeLinkElement = function(attribute)
    {
        const link = document.createElement('link');

        if (attribute.rel) link.rel = attribute.rel;
        if (attribute.type) link.type = attribute.type;
        if (attribute.href) link.href = attribute.href;
        if (attribute.as) link.as = attribute.as;
        if (attribute.crossorigin) link.crossorigin = attribute.crossorigin;
        if (attribute.media) link.media = attribute.media;

        link.addEventListener('load', function() {}, false);

        document.head.appendChild(link);
    }

    this.composeElementId = function(str = null, range = 100)
    {
        let elName = function() { return str + Math.floor(Math.random() * range) };
        let el = null;

        while (document.getElementById(el = elName())) {};

        return el;
    }

}).call(compose);

var touch = {};
(function() {

    'use strict';

    this.setHorizontalSwipe = function(touch, callback, args)
    {
        const el = document.querySelector(touch.el);

        if (!touch.act) touch.act = 10;

        el.addEventListener('touchstart', e => { touch.start = e.changedTouches[0].screenX; }, { passive: true } );

        el.addEventListener('touchmove', e => { e.preventDefault(); }, { passive: true });

        el.addEventListener('touchend', e =>
        {
            touch.end = e.changedTouches[0].screenX;

            if (Math.abs(touch.start - touch.end) > touch.act)
            {
                args.action = (touch.start > touch.end);
                callback.call(this, args);
            }

        }, { passive: true });

    }

}).call(touch);

var cookies = {};
(function() {

    'use strict';

    this.get = function (name)
    {
        let match = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = function (name, value, options = {})
    {
        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        if (!options.path) options.path = '/';
        if (!options.samesite) options.samesite = 'Lax; Secure';
        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        for (let item in options)
        {
            cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null);
        }

        document.cookie = cookie;
    }

}).call(cookies);

var caching = {};
(function(cache) {

    'use strict';

    this.installCache = function(namedCache, urlArray)
    {
        window.addEventListener('install', function(e)
        {
            e.waitUntil(
                caches.open(namedCache).then( function(cache)
                {
                    return cache.addAll(urlArray);

                })

            );

        });

        window.addEventListener('fetch', function(e)
        {
            e.respondWith(caches.match(e.request).then(function(response)
            {
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

    this.viewCachedRequests = function(namedCache)
    {
        caches.open(namedCache).then(function(cache)
        {
            cache.keys().then(function(cachedRequests)
            {
                console.log('exploreCache: ' + cachedRequests); // [Request, Request]
            });

        });

    }

    this.listExistingCacheNames = function(obj)
    {
        for (let property in obj) str += property + ': ' + obj[property] + ', ';
        console.log(str.replace(/, +$/g,''));

        caches.keys().then(function(cacheKeys)
        {
            console.log('listCache: ' + cacheKeys); // eg: namedCache
        });

    }

    this.deleteCacheByName = function(namedCache)
    {
        caches.delete(namedCache).then(function()
        {
            console.log(namedCache + ' - Cache successfully deleted');
        });

    }

    this.deleteOldCacheVersions = function(namedCache)
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

}).call(caching);
