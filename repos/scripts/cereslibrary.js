/**
 * @license
 * cereslibrary v1.0.0
 *
 * Minified using terser v5.5.1
 * Original file: gh ./repos/scripts/cereslibrary.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { include, resource, debug, cookies, touch, cache }

const remark = {
    documentError    : 'Error: Unable to find the document element',
    cacheWarning     : 'Warning: cache response status '
};

var include = {};
(function()
{
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

var resource = {};
(function()
{
    this.srcOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }
    this.fileName = function(path) { return path.substring(path.lastIndexOf('/')+1, path.length); }
    this.fileType = function(path, type) { return path.substring(path.lastIndexOf('.'), path.length).toUpperCase() === type.toUpperCase(); }

    this.composeElement = function(el, attribute)
    {
        if (!el.type) return;

        const precursor = this.attrib.tag.includes(el.type.trim().toUpperCase()) ? document.head : (el.parent || document.body);
        const node = document.createElement(el.type);

        Object.entries(attribute).forEach(([key, value]) => { node.setAttribute(key, value); });
        if (el.markup) node.insertAdjacentHTML('afterbegin', el.markup);

        precursor.appendChild(node);
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

    this.ignore = function(obj)
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
        if (this.ignore(obj) || !this.isString(obj)) return false;

        return this.attrib.bool.includes(obj.trim().toUpperCase());
    }

    this.getUniqueId = function(obj)
    {
        if (!obj.name) obj.name = 'n';
        if (!obj.range) obj.range = 100;

        let elName = function() { return obj.name + Math.floor(Math.random() * obj.range) };
        while (document.getElementById(obj.el = elName())) {};

        return obj.el;
    }

    this.getCurrentDateTime = function(obj = {})
    {
        const newDate = new Date();
        const defaultDate = this.ignore(obj);

        if (defaultDate) return newDate;

        const getOffset = function(value) { return (value < 10) ? '0' : ''; }

        Date.prototype.today = function () {
            return getOffset(this.getDate()) + this.getDate() + '/' + getOffset(this.getMonth()+1) + (this.getMonth() + 1) + '/' + this.getFullYear();
        }

        Date.prototype.timeNow = function () {
            let time = getOffset(this.getHours()) + this.getHours() + ':' + getOffset(this.getMinutes()) + this.getMinutes() + ':' + getOffset(this.getSeconds()) + this.getSeconds();
            return (obj.ms) ? time + '.' + getOffset(this.getUTCMilliseconds()) + this.getUTCMilliseconds() : time;
        }

        let date = (obj.date) ? newDate.today() + ' ' : '';
        date = (obj.time) ? date + newDate.timeNow() : '';

        return date.trim();
    }

    this.removeDuplcates = function(obj, sort)
    {
        const key = JSON.stringify;
        let ar = [...new Map (obj.map(node => [key(node), node])).values()];

        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.parseText = function(obj)
    {
        if (this.ignore(obj.text)) return;

        if (obj.regex || obj.text.includes('</template>')) return obj.text.replace(this.attrib.markup, '');

        let doc = new DOMParser().parseFromString(obj.text, 'text/html');
        return doc.body.textContent || doc.body.innerText;
    }

    this.attrib =
    {
        bArray       : ['true', '1', 'enable', 'confirm', 'grant', 'active', 'on', 'yes'],
        pArray       : ['color', 'font', 'padding', 'top', 'bottom'],
        tArray       : ['link', 'script', 'style'],
        isWindows    : (navigator.appVersion.indexOf('Win') != -1),
        whitespace   : /\s/g,
        markup       : /(<([^>]+)>)/ig,

        get newline() { return this.isWindows ? '\r\n' : '\n'; },
        get bool() { return this.bArray.map(item => { return item.trim().toUpperCase(); }) },
        get tagName() { return this.tArray.map(item => { return item.trim().toUpperCase(); }) },
        get metaUrl() { return import.meta.url; }
    }

}).call(resource);

var debug = {};
(function()
{
    this.inspect = function(diagnostic)
    {
        const errorHandler = function(error)
        {
            let err = error.notification + ' [ DateTime: ' + new Date().toLocaleString() + ' ]';
            console.error(err);

            if (error.alert) alert(err);
        }

        const lookup = {
            [this.attrib.notify]    : function() { if (diagnostic.logtrace) console.info(diagnostic.notification); },
            [this.attrib.warn]      : function() { if (diagnostic.logtrace) console.warn(diagnostic.notification); },
            [this.attrib.reference] : function() { if (diagnostic.logtrace) console.log('Reference: ' + this.attrib.newline + this.attrib.newline + diagnostic.reference); },
            [this.attrib.error]     : function() { errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace }); },
            [this.attrib.default]   : function() { errorHandler({ notification: 'Unhandled exception' }); }
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
        reference    : 1,
        notify       : 2,
        warn         : 3,
        default      : 98,
        error        : 99,
        isWindows    : (navigator.appVersion.indexOf('Win') != -1),

        get newline() { return this.isWindows ? '\r\n' : '\n'; },
    }

}).call(debug);

var cookies = {};
(function()
{
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

var touch = {};
(function()
{
    this.setSwipe = function(touch, callback, args) // horizontal swipe
    {
        if (!touch.act) touch.act = 80;

        touch.node.addEventListener('touchstart', e => { touch.start = e.changedTouches[0].screenX; }, { passive: true });
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

var cache = {};
(function()
{
    this.available = ('caches' in window);

    this.installCache = function(cacheName, urlArray)
    {
        urlArray.forEach(url =>
        {
            fetch(url).then(response =>
            {
                if (!response.ok) { console.warn(remark.cacheWarning + '[' + response.status + '] - ' + url); }
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

}).call(cache);
