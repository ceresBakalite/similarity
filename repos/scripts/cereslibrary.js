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

var include = {};
(function() {

    this.directive = (el = 'include-directive') => {

        globalThis.customElements.get(el) || globalThis.customElements.define(el, class extends HTMLElement
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
(function() {

    this.isWindows    = navigator.appVersion.indexOf('Win') != -1;
    this.whitespace   = /\s/g,
    this.markup       = /(<([^>]+)>)/ig,
    this.commaCodes   = /,|&comma;|&#x2c;|&#44;|U+0002C/g;
    this.commaSymbol  = '_&c';
    this.newline      = this.isWindows ? '\r\n' : '\n';
    this.bArray       = ['true', '1', 'enable', 'confirm', 'grant', 'active', 'on', 'yes'];
    this.elArray      = ['link', 'script', 'style'];
    this.bool         = this.bArray.map(item => { return item.trim().toUpperCase(); });
    this.docHead      = this.elArray.map(item => { return item.trim().toUpperCase(); });
    this.srcOpen      = obj => globalThis.open(obj.element.getAttribute('src'), obj.type);
    this.isString     = obj => Object.prototype.toString.call(obj) == '[object String]';
    this.clearElement = el => { while (el.firstChild) el.removeChild(el.firstChild); }
    this.fileName     = path => path.substring(path.lastIndexOf('/')+1, path.length);
    this.fileType     = (path, type) => path.substring(path.lastIndexOf('.')+1, path.length).toUpperCase() === type.toUpperCase();

    this.composeElement = (el, atr) => {

        if (this.ignore(el.type)) return;

        const precursor = this.docHead.includes(el.type.trim().toUpperCase()) ? document.head : (el.parent || document.body);
        const node = document.createElement(el.type);

        Object.entries(atr).forEach(([key, value]) => { node.setAttribute(key, value); });
        if (el.markup) node.insertAdjacentHTML('afterbegin', el.markup);

        precursor.appendChild(node);
    }

    this.composeCORSLinks = (el) => {

        const nodelist = document.querySelectorAll(el.node); // shadowroot markdown node - ie zero-md or ceres-sv

        if (!el.regex) el.regex = /<a (?!target)/gmi;
        if (!el.replace) el.replace = '<a target="_top" ';

        nodelist.forEach(node => {

            let shadow = node.shadowRoot;

            if (shadow) {

                let shade = shadow.querySelector(el.query);

                let markdown = new DOMParser().parseFromString(shade.textContent, type).documentElement.textContent;
                //let markdown = shade.innerHTML; // the content we wish to alter
                shade.textContent = markdown.replace(el.regex, el.replace);
                //let markdown = shadow.querySelector(el.query).innerHTML; // the content we wish to alter
                //shadow.querySelector(el.query).innerHTML = markdown.replace(el.regex, el.replace);
            }

        });

    }

    this.ignore = obj => {

        if (obj === null || obj == 'undefined') return true;

        if (this.isString(obj)) return (obj.length === 0 || !obj.trim());
        if (Array.isArray(obj)) return (obj.length === 0);
        if (obj && obj.constructor === Object) return (Object.keys(obj).length === 0);

        return !obj;
    }

    this.getBoolean = obj => {

        if (obj === true || obj === false) return atr;
        if (this.ignore(obj) || !this.isString(obj)) return false;

        return this.bool.includes(obj.trim().toUpperCase());
    }

    this.getUniqueId = obj => {

        if (!obj.name) obj.name = 'n';
        if (!obj.range) obj.range = 100;

        const elName = () => obj.name + Math.floor(Math.random() * obj.range);
        while (document.getElementById(obj.el = elName())) {};

        return obj.el;
    }

    this.regexEscape = (str) => {

        return str.replace(/([.*+?^$|(){}\[\]])/gm, '\\$1');
    }

    this.removeDuplcates = (obj, sort) => {

        const key = JSON.stringify;
        const ar = [...new Map (obj.map(node => [key(node), node])).values()];

        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.softSanitize = (text, type = 'text/html') => {

        return this.ignore(text) ? null : new DOMParser()
            .parseFromString(text, type).documentElement.textContent
            .replace(/</g, '&lt;');
    }

    // noddy regex comma separated value parser
    this.parseCSV = (text, symbol = {}) => {

        const textArray = text.split('\n'); // this assumes incorrectly that line breaks only occur at the end of rows
        const newArray  = new Array(textArray.length);
        const endSymbol = '_&grp;';
        const endRow    = new RegExp(endSymbol + '\s*?$', 'g'); // match end symbols at the end of a row
        const regex     = /"[^]*?",|"[^]*?"$/gm; // match character groups in need of parsing

        const parseGroup = group => {

            let newGroup = String(group)  // remove leading quotes and trailing quotes and commas
                .replace(/"\s*?$|"\s*?,\s*?$/, '')
                .replace(/^\s*?"/, '');

            newGroup = newGroup.replace(/""/g, '"'); // replace two ajoining double quotes with one double quote

            return newGroup.replace(this.commaCodes, this.commaSymbol) + endSymbol; // replace any remaining comma entities with a separator symbol
        }

        const parseRow = row => {

            let newRow = row.replace(endRow, ''); // remove end symbols at the end of a row
            newRow = newRow.replaceAll(endSymbol, ', '); // replace any remaining end symbols inside character groups with a comma value separator

            return newRow.replace(/(?!\s)[,](?!\s)/g, ', '); // tidy
        }

        // construct a JSON object from the CSV construct
        const composeJSON = () => {

            const nodeName = i => symbol.nodes[i] ? '"' + symbol.nodes[i] + '": ' : '"node' + i+1 + '": ';
            const re = /,\s*?$/; // match trailing comma whitespace

            let str = '';

            newArray.forEach(row => {

                if (!rsc.ignore(row)) {

                    str += '{ ';
                    let rowArray = row.split(',');

                    rowArray.forEach((value, i) => { str += nodeName(i) + '"' + value.trim().replace(/"/g, '\\"') + '", '; });
                    str = str.replace(re, '') + ' },\n';
                }

            });

            return '[' + str.replace(re, '') + ']';
        }

        const objectType = () => (symbol.json || symbol.nodes) ? composeJSON() : newArray.join('\n');

        textArray.forEach(row => {

            let newRow = String(row);
            let groups = [...newRow.matchAll(regex)]; // get character groups in need of parsing

            groups.forEach(group => {

                let newGroup = parseGroup(group);
                newRow = newRow.replace(group, newGroup);
            });

            newArray.push(parseRow(newRow));
        });

        return objectType();
    }

}).call(resource);

var debug = {};
(function() {

    this.reference = 1;
    this.notify    = 2;
    this.warn      = 3;
    this.default   = 98;
    this.error     = 99;
    this.isWindows =   navigator.appVersion.indexOf('Win') != -1;
    this.newline   =  this.isWindows ? '\r\n' : '\n';

    this.inspect = diagnostic => {

        const errorHandler = error => {

            const err = error.notification + ' [ DateTime: ' + this.getCurrentDateTime({ ms: true }) + ' ]';
            console.error(err);

            if (error.alert) alert(err);
        }

        const lookup = {

            [this.notify]    : () => { if (diagnostic.logtrace) console.info(diagnostic.notification); },
            [this.warn]      : () => { if (diagnostic.logtrace) console.warn(diagnostic.notification); },
            [this.reference] : () => { if (diagnostic.logtrace) console.log('Reference: ' + this.newline + this.newline + diagnostic.reference); },
            [this.error]     : () => errorHandler({ notification: diagnostic.notification, alert: diagnostic.logtrace }),
            [this.default]   : () => errorHandler({ notification: 'Unhandled exception' })
        };

        lookup[diagnostic.type]() || lookup[this.default];
    }

    this.getProperties = (string = {}, str = '') => {

        for (let literal in string) str += literal + ': ' + string[literal] + ', ';
        return str.replace(/, +$/g,'');
    }

    this.getCurrentDateTime = (obj = {}) => {

        const newDate = new Date();
        const defaultDate = resource.ignore(obj);

        if (defaultDate) return newDate;

        const getOffset = function(value) { return (value < 10) ? '0' : ''; }

        Date.prototype.today = () => { return getOffset(this.getDate()) + this.getDate() + '/' + getOffset(this.getMonth()+1) + (this.getMonth() + 1) + '/' + this.getFullYear(); }

        Date.prototype.timeNow = () => {

            let time = getOffset(this.getHours()) + this.getHours() + ':' + getOffset(this.getMinutes()) + this.getMinutes() + ':' + getOffset(this.getSeconds()) + this.getSeconds();
            return (obj.ms) ? time + '.' + getOffset(this.getUTCMilliseconds()) + this.getUTCMilliseconds() : time;
        }

        let date = obj.date ? newDate.today() + ' ' : '';
        date = obj.time ? date + newDate.timeNow() : '';

        return date.trim();
    }

}).call(debug);

var cookies = {};
(function() {

    this.get = name => {

        const match = document.cookie.match(new RegExp('(?:^|; )' + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + '=([^;]*)'));
        return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = (name, value, options = {}) => {

        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        if (!options.path) options.path = '/';
        if (!options.samesite) options.samesite = 'Strict; Secure';
        if (options.expires instanceof Date) options.expires = options.expires.toUTCString();

        for (let item in options) { cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null); }

        document.cookie = cookie;
    }

}).call(cookies);

var touch = {};
(function() {

    this.setSwipe = (touch, callback, args) => { // horizontal swipe

        if (!touch.act) touch.act = 80;

        touch.node.addEventListener('touchstart', e => { touch.start = e.changedTouches[0].screenX; }, { passive: true });
        touch.node.addEventListener('touchmove', e => { e.preventDefault(); }, { passive: true });
        touch.node.addEventListener('touchend', e =>
        {
            touch.end = e.changedTouches[0].screenX;

            if (Math.abs(touch.start - touch.end) > touch.act) {

                args.action = (touch.start > touch.end);
                callback.call(this, args);
            }

        }, { passive: true });

    }

}).call(touch);

var cache = {};
(function() {

    this.installCache = (cacheName, urlArray) => {

        urlArray.forEach(url => {

            fetch(url).then(response => {

                if (!response.ok) console.warn('Warning: cache response status [' + response.status + '] - ' + url);
                return caches.open(cacheName).then(cache => { return cache.put(url, response); });

            });

        });

    }

    this.viewCachedRequests = cacheName => {

        caches.open(cacheName).then(cache => { cache.keys().then(cachedRequests => { console.log('exploreCache: ' + cachedRequests); }); });

    }

    this.listExistingCacheNames = () => { caches.keys().then(cacheKeys => { console.log('listCache: ' + cacheKeys); }); }

    this.deleteCacheByName = cacheName => { caches.delete(cacheName).then(() => { console.log(cacheName + ' - Cache successfully deleted'); }); }

    this.deleteOldCacheVersions = cacheName =>  {

        caches.keys().then(cacheNames => {

            return Promise.all (

                cacheNames.map(cacheName => { if (cacheName != cacheName) { return caches.delete(cacheName); } })

            );

        });

    }

}).call(cache);
