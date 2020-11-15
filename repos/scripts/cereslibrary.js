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
export { include, generic, cookies, compose, touch, caching }

var include = {};
(function()
{
    'use strict'; // for conformity - strict by default

    this.directive = function(el = 'include-directive')
    {
        window.customElements.get(el) || window.customElements.define(el, class extends HTMLElement
        {
            async connectedCallback()
            {
                const src = this.getAttribute('src');
                fetch(src).then(response => response.text()).then(str => { this.insertAdjacentHTML('afterbegin', str) });
            }

        });

    }

}).call(include);

var generic = {};
(function()
{
    'use strict'; // for conformity - strict by default

    this.reference = 1;
    this.notify = 2;
    this.default = 98;
    this.error = 99;
    this.nonWordChars = '/\()"\':,.;<>~!@#$%^&*|+=[]{}`?-…';
    this.strBoolean = ['TRUE','1','YES','ON','ACTIVE','ENABLE'];
    this.isWindows = (navigator.appVersion.indexOf('Win') != -1);
    this.newline = this.isWindows ? '\r\n' : '\n';
    this.whitespace = /\s/g;
    this.markup = /(<([^>]+)>)/ig;

    this.windowOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }
    this.getImportMetaUrl = function() { return import.meta.url; }

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
        if (this.isEmptyOrNull(attribute) || !this.isString(attribute)) return false;

        return this.strBoolean.includes(attribute.trim().toUpperCase());
    }

    this.getUniqueElementId = function(str = null, range = 100)
    {
        let elName = function() { return str + Math.floor(Math.random() * range) };
        let el = null;

        while (document.getElementById(el = elName())) {};

        return el;
    }

    this.removeDuplcates = function(obj, sort)
    {
        const key = JSON.stringify;
        let ar = [...new Map (obj.map(node => [key(node), node])).values()];

        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.htmlToText = function(html, regex)
    {
        if (this.isEmptyOrNull(html)) return;
        if (regex) return html.replace(this.markup, '');

        let el = document.createElement('div');
        el.innerHTML = html;

        return el.textContent || el.innerText;
    }


    this.inspect = function(diagnostic)
    {
        const errorHandler = function(error)
        {
            let err = error.notification + ' [ DateTime: ' + new Date().toLocaleString() + ' ]';
            console.error(err);

            if (error.alert) alert(err);
        }

        const lookup = {
            [this.notify]: function() { if (diagnostic.logtrace) console.info(diagnostic.notification); },
            [this.error]: function() { errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace } ); },
            [this.reference]: function() { if (diagnostic.logtrace) console.log('Reference: ' + this.newline + this.newline + diagnostic.reference); },
            [this.default]: function() { errorHandler({ notification: errordefault, alert: diagnostic.logtrace } ); }
        };

        lookup[diagnostic.type]() || lookup[this.default];
    }

    this.getObjectProperties = function(object, str = '')
    {
        for (let property in object) str += property + ': ' + object[property] + ', ';
        return str.replace(/, +$/g,'');
    }

}).call(generic);

var cookies = {};
(function() {

    'use strict'; // for conformity - strict by default

    this.get = function(name)
    {
        const match = document.cookie.match(new RegExp('(?:^|; )' + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'));
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

    'use strict'; // for conformity - strict by default

    const mapNode = new Map();

    initialise();

    this.composeElement = function(el, locale = 'en')
    {
        const precursor = mapNode.get(el.typeof.toLocaleLowerCase(locale)) || el.parent;
        const node = document.createElement(el.typeof);

        node.id = el.id;

        if (el.className) node.setAttribute('class', el.className);
        if (el.onClick) node.setAttribute('onclick', el.onClick);
        if (el.src) node.setAttribute('src', el.src);
        if (el.alt) node.setAttribute('alt', el.alt);
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

    this.composeCORSLinks = function(el)
    {
        const nodelist = document.querySelectorAll(el.node); // shadowroot markdown node - ie zero-md or ceres-sv

        if (!el.regex) el.regex = /<a (?!target)/gmi;
        if (!el.replace) el.replace = '<a target="_top" ';

        nodelist.forEach(node => {

            let shadow = node.shadowRoot;

            if (shadow)
            {
                let markdown = shadow.querySelector(el.query).innerHTML; // the content we wish to alter
                shadow.querySelector(el.query).innerHTML = markdown.replace(el.regex, el.replace);
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

    'use strict'; // for conformity - strict by default

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

    'use strict'; // for conformity - strict by default

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
