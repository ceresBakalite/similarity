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

    this.srcOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }

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

    this.getBooleanAttribute = function(obj)
    {
        if (obj === true || obj === false) return atr;
        if (this.isEmptyOrNull(obj) || !this.isString(obj)) return false;

        return this.attrib.bool.includes(obj.trim().toUpperCase());
    }

    this.getUniqueElementId = function(args = {})
    {
        if (!args.name) args.name = 'n';
        if (!args.range) args.range = 100;

        let elName = function() { return args.name + Math.floor(Math.random() * args.range) };
        while (document.getElementById(args.el = elName())) {};

        return args.el;
    }

    this.removeDuplcates = function(obj, sort)
    {
        const key = JSON.stringify;
        let ar = [...new Map (obj.map(node => [key(node), node])).values()];

        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.parseText = function(text, regex)
    {
        if (this.isEmptyOrNull(text)) return;

        if (regex || text.includes('</template>')) return text.replace(this.attrib.markup, '');

        let doc = new DOMParser().parseFromString(text, 'text/html');
        return doc.body.textContent || doc.body.innerText;
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
            [this.attrib.notify]: function() { if (diagnostic.logtrace) console.info(diagnostic.notification); },
            [this.attrib.warn]: function() { if (diagnostic.logtrace) console.warn(diagnostic.notification); },
            [this.attrib.reference]: function() { if (diagnostic.logtrace) console.log('Reference: ' + this.attrib.newline + this.attrib.newline + diagnostic.reference); },
            [this.attrib.error]: function() { errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace }); },
            [this.attrib.default]: function() { errorHandler({ notification: 'Unhandled exception' }); }
        };

        lookup[diagnostic.type]() || lookup[this.attrib.default];
    }

    this.getObjectProperties = function(string = {}, str = '')
    {
        for (let literal in string) str += literal + ': ' + string[literal] + ', ';
        return str.replace(/, +$/g,'');
    }

    this.attrib =
    {
        reference   : 1,
        notify      : 2,
        warn        : 3,
        default     : 98,
        error       : 99,
        bArray      : ['true', '1', 'enable', 'confirm', 'grant', 'active', 'on', 'yes'],
        isWindows   : (navigator.appVersion.indexOf('Win') != -1),
        nonWordChars: '/\()"\':,.;<>~!@#$%^&*|+=[]{}`?-…',
        whitespace  : /\s/g,
        markup      : /(<([^>]+)>)/ig,

        get newline() { return this.isWindows ? '\r\n' : '\n'; },
        get bool() { return this.bArray.toString().toUpperCase().split(','); },
        get metaUrl() { return import.meta.url; }
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

    this.composeElement = function(el)
    {
        const precursor = el.parent;
        const node = document.createElement(el.type);

        if (el.id) node.setAttribute('id', el.id);
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
(function() {

    'use strict'; // for conformity - strict by default

    this.available = ('caches' in window);

    this.installCache = function(cacheName, urlArray)
    {
        urlArray.forEach(url =>
        {
            fetch(url).then(response =>
            {
                if (!response.ok) { rsc.inspect({ type: rsc.attrib.warn, notification: remark.cacheWarning + url, logtrace: cfg.attrib.trace }); }
                return caches.open(cacheName).then(cache => { return cache.put(url, response); });
            });

        });

    }

    this.viewCachedRequests = function(cacheName)
    {
        caches.open(cacheName).then(function(cache)
        {
            cache.keys().then(function(cachedRequests) { console.log('exploreCache: ' + cachedRequests); });
        });

    }

    this.listExistingCacheNames = function()
    {
        caches.keys().then(function(cacheKeys) { console.log('listCache: ' + cacheKeys); });
    }

    this.deleteCacheByName = function(cacheName)
    {
        caches.delete(cacheName).then(function() { console.log(cacheName + ' - Cache successfully deleted'); });
    }

    this.deleteOldCacheVersions = function(cacheName)
    {
        caches.keys().then(function(cacheNames)
        {
            return Promise.all
            (
                cacheNames.map(function(cacheName)
                {
                    if(cacheName != cacheName) { return caches.delete(cacheName); }
                })

            );

        });

    }

}).call(caching);
