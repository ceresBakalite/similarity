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
export { include, generic, cookies, compose, touch, caching  }

var include = {};
(function()
{
    'use strict';

    this.directive = function(el = 'include-directive')
    {
        window.customElements.get(el) || window.customElements.define(el, class extends HTMLElement
        {
            async connectedCallback()
            {
                const src = this.getAttribute('src');
                this.insertAdjacentHTML('afterbegin', await ( await fetch(src) ).text());
            }

        });

    }

}).call(include);

var generic = {};
(function()
{
    'use strict';

    const protean = function() { return attribute; }
    const resource = function() { return attribute; } // local resource
    const symbol = new Map(); // local scope symbols

    initialise();

    this.constant = protean; // exposed local scope resources

    Object.freeze(this.constant);

    this.windowOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }
    this.getImportMetaUrl = function() { return import.meta.url; }

    this.isEmptyOrNull = function(obj)
    {
        if (obj === null || obj == 'undefined') return true;

        if (this.isString(obj)) return (obj.length === 0 || !obj.trim());
        if (Array.isArray(obj)) return (obj.length === 0);
        if (obj && obj.constructor === Object) return (Object.keys(obj).length === 0);

        return !obj;
    }

    this.getBooleanAttribute = function(attribute, locale = 'en')
    {
        if (attribute === true || attribute === false) return attribute;
        if (this.isEmptyOrNull(attribute)) return false;
        if (!this.isString(attribute)) return false;

        const token = attribute.trim().toLocaleLowerCase(locale);

        return symbol.get(token) || false;
    }

    this.removeDuplcates = function(obj, key, sort)
    {
        let ar = [...new Map (obj.map(node => [key(node), node])).values()];
        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.inspect = function(diagnostic)
    {
        if (this.isEmptyOrNull(diagnostic)) return this.inspect({ type: protean.error, notification: resource.inspect });

        const lookup = {
            [protean.notify]: function() { if (diagnostic.logtrace) console.log(diagnostic.notification); },
            [protean.error]: function() { this.errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace } ); },
            [protean.reference]: function() { if (diagnostic.logtrace) console.log('Reference: ' + protean.newline + protean.newline + diagnostic.reference); },
            [protean.default]: function() { this.errorHandler({ notification: resource.errordefault, alert: diagnostic.logtrace } ); }
        };

        return lookup[diagnostic.type]() || lookup[protean.default];
    }

    this.errorHandler = function(error)
    {
        if (this.isEmptyOrNull(error)) return this.inspect({ type: protean.error, notification: resource.errorHandler });

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

    function initialise()
    {
        symbol.set('true', true);
        symbol.set('t', true);
        symbol.set('yes', true);
        symbol.set('y', true);
        symbol.set('1', true);

        protean.reference = 1;
        protean.notify = 2;
        protean.default = 98;
        protean.error = 99;
        protean.isWindows = (navigator.appVersion.indexOf('Win') != -1);
        protean.newline = protean.isWindows ? '\r\n' : '\n';

        Object.freeze(protean);

        resource.inspect = 'Error: An exception occurred in the inspect method.  The diagnostic argument was empty or null';
        resource.errorhandler = 'Error: An exception occurred in the errorhandler method.  The error argument was empty or null';
        resource.errordefault = 'An unexpected error has occurred. The inspection type was missing or invalid';

        Object.freeze(resource);
    }

}).call(generic);

var cookies = {};
(function() {

    'use strict';

    this.get = function(name)
    {
        const match = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = function(name, value, options = {})
    {
        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        if (!options.path) options.path = '/';
        if (!options.samesite) options.samesite = 'Strict; Secure';
        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        for (let item in options)
        {
            cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null);
        }

        document.cookie = cookie;
    }

}).call(cookies);

var compose = {};
(function() {

    'use strict';

    const mapNode = new Map();

    initialise();

    this.composeElement = function(el, locale = 'en')
    {
        const precursor = mapNode.get(el.typeof.toLocaleLowerCase(locale)) || el.parent;
        const node = document.createElement(el.typeof);

        node.id = el.id;

        if (el.className) node.setAttribute("class", el.className);
        if (el.onClick) node.setAttribute("onclick", el.onClick);
        if (el.src) node.setAttribute("src", el.src);
        if (el.alt) node.setAttribute("alt", el.alt);
        if (el.rel) node.setAttribute('rel', el.rel);
        if (el.type) node.setAttribute('type', el.type);  // MDN - not depricated, recommended practice to omit
        if (el.href) node.setAttribute('href', el.href);
        if (el.as) node.setAttribute('as', el.as);
        if (el.crossorigin) node.setAttribute('crossorigin', el.crossorigin);
        if (el.media) node.setAttribute('media', el.media);
        if (el.markup) node.insertAdjacentHTML('afterbegin', el.markup);

        precursor.appendChild(node);
    }

    this.composeUniqueElementId = function(str = null, range = 100)
    {
        let elName = function() { return str + Math.floor(Math.random() * range) };
        let el = null;

        while (document.getElementById(el = elName())) {};

        return el;
    }

    this.setCORSMarkdownLinks = function(el)
    {
        const nodelist = document.querySelectorAll(el.node);

        if (!el.regex) el.regex = /<a /gi;
        if (!el.replacement) el.replacement = '<a target="_top" ';

        nodelist.forEach(node => {

            let shadow = node.shadowRoot;

            if (shadow)
            {
                let markdown = shadow.querySelector(el.query).innerHTML;
                shadow.querySelector(el.query).innerHTML = markdown.replace(el.regex, el.replacement);
            }

        });

    }

    function initialise()
    {
        mapNode.set('link', document.head);
        mapNode.set('script', document.head);
        mapNode.set('style', document.head);
    }

}).call(compose);

var touch = {};
(function() {

    'use strict';

    this.setHorizontalSwipe = function(touch, callback, args)
    {
        if (!touch.act) touch.act = 80;

        touch.node.addEventListener('touchstart', e => { touch.start = e.changedTouches[0].screenX; }, { passive: true } );
        touch.node.addEventListener('touchmove', e => { e.preventDefault(); }, { passive: true });
        touch.node.addEventListener('touchend', e =>
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

var caching = {};
(function(cache) {

    'use strict';

    this.available = ('caches' in window);

    this.set = function(type = 'Cache-Control', value = 'public, max-age 604800, s-maxage 43200')
    {
        const header = new Headers();
        header.set(type, value);
    }

    this.get = function(type = 'Cache-Control')
    {
        const header = new Headers();
        header.get(type);
    }

    this.installCache = function(namedCache, urlArray, urlImage = '/images/NAVCogs.png')
    {
        window.addEventListener('install', function(e)
        {
            e.waitUntil(caches.open(namedCache).then(function(cache) { return cache.addAll(urlArray); }));
        });

        window.addEventListener('fetch', function(e)
        {
            e.respondWith(caches.match(e.request).then(function(response)
            {
                if (response !== undefined)
                {
                    return response;

                } else {

                    return fetch(e.request).then(function (response)
                    {
                        let responseClone = response.clone();

                        caches.open(namedCache).then(function (cache) { cache.put(e.request, responseClone); });

                        return response;

                    }).catch(function () {

                        return caches.match(urlImage);

                    });

                }

            }));

        });

    }

    this.viewCachedRequests = function(namedCache)
    {
        caches.open(namedCache).then(function(cache)
        {
            cache.keys().then(function(cachedRequests) { console.log('exploreCache: ' + cachedRequests); });
        });

    }

    this.listExistingCacheNames = function()
    {
        caches.keys().then(function(cacheKeys) { console.log('listCache: ' + cacheKeys); });
    }

    this.deleteCacheByName = function(namedCache)
    {
        caches.delete(namedCache).then(function() { console.log(namedCache + ' - Cache successfully deleted'); });
    }

    this.deleteOldCacheVersions = function(namedCache)
    {
        caches.keys().then(function(cacheNames)
        {
            return Promise.all
            (
                cacheNames.map(function(cacheName)
                {
                    if(cacheName != namedCache) { return caches.delete(cacheName); }
                })

            );

        });

    }

}).call(caching);
