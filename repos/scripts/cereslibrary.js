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
export { ceresIncludeDirective as include, ceresResourceMethods as resource, ceresErrorHandler as debug, ceresCookieHandler as cookies, ceresHorizontalSwipe as swipe, ceresCacheMethods as cache }

var ceresIncludeDirective = {};  // fetch HTML namespace include scripts
(function() {

    this.directive = (el = 'include-directive') => {

        globalThis.customElements.get(el) || globalThis.customElements.define(el, class extends HTMLElement
        {
            async connectedCallback()
            {
                fetch(this.getAttribute('src')).then(response => response.text()).then(text => { this.insertAdjacentHTML('afterbegin', text) });
            };

        });

    }

}).call(ceresIncludeDirective);

var ceresResourceMethods = {};
(function() {

    this.isWindows   = navigator.appVersion.indexOf('Win') != -1;
    this.bArray      = ['true', '1', 'enable', 'confirm', 'grant', 'active', 'on', 'yes'];
    this.elArray     = ['link', 'script', 'style'];
    this.commaCodes  = /,|&comma;|&#x2c;|&#44;|U+0002C/g;
    this.commaSymbol = '_&c';
    this.whitespace  = /\s/g;
    this.markup      = /(<([^>]+)>)/ig;
    this.newline     = this.isWindows ? '\r\n' : '\n';
    this.docHead     = this.elArray.map(item => item.trim().toUpperCase() );
    this.bool        = this.bArray.map(item => item.trim().toUpperCase() );

    this.fileType  = (path, type) => this.fileExt(path).toUpperCase() === type.toUpperCase();
    this.fileName  = path => path.substring(path.lastIndexOf('/')+1, path.length);
    this.fileExt   = path => path.substring(path.lastIndexOf('.')+1, path.length);
    this.mediaType = path => this.media.get(this.fileExt(path).toLowerCase());
    this.isVideo   = path => this.media.has(this.fileExt(path).toLowerCase());
    this.isString  = obj => Object.prototype.toString.call(obj) == '[object String]';
    this.srcOpen   = obj => globalThis.open(obj.element.getAttribute('src'), obj.type);
    this.nodeType  = node => node.nodeName.toLocaleLowerCase();
    this.clearNode = node => { while (node.firstChild) node.removeChild(node.firstChild); }

    this.softSanitize = (text, type = 'text/html') => this.ignore(text) ? null : new DOMParser()
        .parseFromString(text, type).documentElement.textContent
        .replace(/</g, '&lt;');

    this.ignore = obj => (obj === null || obj == 'undefined') ? true
        : this.isString(obj) ? (obj.length === 0 || !obj.trim())
        : Array.isArray(obj) ? (obj.length === 0)
        : (obj && obj.constructor === Object) ? Object.keys(obj).length === 0
        : !obj;

    this.getBoolean = obj => (obj === true || obj === false) ? obj
        : (this.ignore(obj) || !this.isString(obj)) ? false
        : this.bool.includes(obj.trim().toUpperCase());

    this.regexEscape = str => str.replace(/([.*+?^$|(){}\[\]])/gm, '\\$1');

    this.media = new Map();
    this.media.set('mp4', 'video/mp4');
    this.media.set('m4v', 'video/m4v');
    this.media.set('ogg', 'video/ogg');
    this.media.set('ogv', 'video/ogg');
    this.media.set('webm', 'video/webm');

    this.composeElement = (el, atr) => {

        if (this.ignore(el.nodeType)) return;

        const precursor = this.docHead.includes(el.nodeType.trim().toUpperCase()) ? document.head : (el.parent || document.body);
        const node = document.createElement(el.nodeType);

        Object.entries(atr).forEach(([key, value]) => { if (value) node.setAttribute(key, value); });

        if (el.markup) node.insertAdjacentHTML('afterbegin', el.markup);
        if (el.nodeType === 'video') this.composeVideo(node, el.src, el.type);

        precursor.appendChild(node);
    }

    this.composeVideo = (node, src, type) => {

        const observer = new IntersectionObserver(entries => { // play when visible

            entries.forEach(entry => { entry.isIntersecting ? node.play() : node.pause() });

        }, {});

        const source = document.createElement('source');
        source.setAttribute('src', src);
        source.setAttribute('type', type);

        node.appendChild(source);

        observer.observe(node);
    }

    this.shadowCORSLinks = (el) => {

        const nodelist = document.querySelectorAll(el.node); // shadowroot instance collection

        if (!el.regex) el.regex = /<a (?!target)/gmi;
        if (!el.replace) el.replace = '<a target="_top" ';

        nodelist.forEach(node => {

            let shadow = node.shadowRoot; // a shadowroot node of the DOM subtree

            if (shadow) {

                let shard = shadow.querySelector(el.query);
                let markup = shard.innerHTML; // the shadowdom html content we wish to alter

                this.clearNode(shard);
                shard.insertAdjacentHTML('afterbegin', markup.replace(el.regex, el.replace));
            };

        });

    }

    this.getUniqueId = obj => {

        if (!obj.name) obj.name = 'n';
        if (!obj.range) obj.range = 100;

        const elName = () => obj.name + Math.floor(Math.random() * obj.range);
        while (document.getElementById(obj.el = elName())) {};

        return obj.el;
    }

    this.removeDuplcates = (obj, sort) => {

        const key = JSON.stringify;
        const ar = [...new Map (obj.map(node => [key(node), node])).values()];

        return sort ? ar.sort((a, b) => a - b) : ar;
    }

    this.getCurrentDateTime = (obj = {}) => {

        const newDate = new Date();
        const defaultDate = this.ignore(obj);

        if (defaultDate) return newDate;

        const getOffset = value => { return (value < 10) ? '0' : ''; }

        Date.prototype.today = () => { return getOffset(this.getDate()) + this.getDate() + '/' + getOffset(this.getMonth()+1) + (this.getMonth() + 1) + '/' + this.getFullYear(); }

        Date.prototype.timeNow = () => {

            let time = getOffset(this.getHours()) + this.getHours() + ':' + getOffset(this.getMinutes()) + this.getMinutes() + ':' + getOffset(this.getSeconds()) + this.getSeconds();
            return (obj.ms) ? time + '.' + getOffset(this.getUTCMilliseconds()) + this.getUTCMilliseconds() : time;
        }

        let date = obj.date ? newDate.today() + ' ' : '';
        date = obj.time ? date + newDate.timeNow() : '';

        return date.trim();
    }

    // noddy regex comma separated value parser
    this.parseCSV = (text, symbol = {}) => {

        const textArray = text.split('\n'); // this assumes incorrectly that line breaks only occur at the end of rows
        const newArray  = new Array(textArray.length);
        const endSymbol = '_&grp;';

        const reA = new RegExp(endSymbol + '\s*?$', 'g'); // match end symbols at the end of a row
        const reB = /"[^]*?",|"[^]*?"$/gm; // match character groups in need of parsing
        const reC = /"\s*?$|"\s*?,\s*?$/; // match trailing quotes & commas & whitespace
        const reD = /^\s*?"/; // match leading quotes & whitespace
        const reE = /""/g; // match two ajoining double quotes
        const reF = /(?!\s)[,](?!\s)/g; // match whitespace surrounding a comma
        const reG = /,\s*?$/; // match trailing comma & whitespace
        const reH = /"/g; // match double quotes

        const parseGroup = group => {

            let newGroup = String(group)
                .replace(reC, '') // remove trailing quotes & commas & whitespace
                .replace(reD, ''); // remove leading quotes & whitespace

            newGroup = newGroup.replace(reE, '"'); // replace two ajoining double quotes with one double quote

            return newGroup.replace(symbol.commaCodes, symbol.commaSymbol) + endSymbol; // replace any remaining comma codes with a separator symbol
        }

        const parseRow = row => {

            let newRow = row.replace(reA, ''); // remove end symbols at the end of a row
            newRow = newRow.replaceAll(endSymbol, ', '); // replace any remaining end symbols inside character groups with a comma value separator

            return newRow.replace(reF, ', '); // replace comma & surrounding whitespace
        }

        // construct a JSON object from the CSV construct
        const composeJSON = () => {

            const nodeName = i => symbol.nodes[i] ? '"' + symbol.nodes[i] + '": ' : '"node' + i+1 + '": ';

            let str = '';

            newArray.forEach(row => {

                if (!this.ignore(row)) {

                    str += '{ ';
                    let rowArray = row.split(',');

                    rowArray.forEach((value, i) => { str += nodeName(i) + '"' + value.trim().replace(reH, '\\"') + '", '; }); // replace quotes with escaped quotes
                    str = str.replace(reG, '') + ' },\n'; // replace trailing comma & whitespace
                };

            });

            return '[' + str.replace(reG, '') + ']'; // replace trailing comma & whitespace
        }

        const objectType = () => (symbol.json || symbol.nodes) ? composeJSON() : newArray.join('\n');

        textArray.forEach(row => {

            let newRow = String(row);
            let groups = [...newRow.matchAll(reB)]; // get character groups in need of parsing

            groups.forEach(group => {

                let newGroup = parseGroup(group);
                newRow = newRow.replace(group, newGroup);
            });

            newArray.push(parseRow(newRow));
        });

        return objectType();
    }

}).call(ceresResourceMethods);

var ceresErrorHandler = {};
(function() {

    this.reference = 1;
    this.notify    = 2;
    this.warn      = 3;
    this.default   = 98;
    this.error     = 99;
    this.isWindows = navigator.appVersion.indexOf('Win') != -1;
    this.newline   = this.isWindows ? '\r\n' : '\n';

    this.inspect = diagnostic => {

        const errorHandler = error => {

            const err = error.notification + ' [ DateTime: ' + resource.getCurrentDateTime({ ms: true }) + ' ]';
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

}).call(ceresErrorHandler);

var ceresCookieHandler = {};
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

}).call(ceresCookieHandler);

var ceresHorizontalSwipe = {};
(function() {

    this.setSwipe = (touch, callback, args) => { // horizontal swipe

        if (!touch.act) touch.act = 80;

        touch.node.addEventListener('touchstart', e => { touch.start = e.changedTouches[0].screenX; }, { passive: true });
        touch.node.addEventListener('touchmove', e => { e.preventDefault(); }, { passive: true });
        touch.node.addEventListener('touchend', e => {

            touch.end = e.changedTouches[0].screenX;

            if (Math.abs(touch.start - touch.end) > touch.act) {

                args.action = (touch.start > touch.end);
                callback.call(this, args);
            }

        }, { passive: true });

    }

}).call(ceresHorizontalSwipe);

var ceresCacheMethods = {};
(function() {

    this.viewCachedRequests = cacheName => { caches.open(cacheName).then(cache => { cache.keys().then(cachedRequests => { console.log('exploreCache: ' + cachedRequests); }); }); }
    this.listExistingCacheNames = () => { caches.keys().then(cacheKeys => { console.log('listCache: ' + cacheKeys); }); }
    this.deleteCacheByName = cacheName => { caches.delete(cacheName).then(() => { console.log(cacheName + ' - Cache successfully deleted'); }); }
    this.deleteOldCacheVersions = cacheName =>  { caches.keys().then(cacheNames => { return Promise.all ( cacheNames.map(cacheName => { if (cacheName != cacheName) { return caches.delete(cacheName); } }) ); }); }

    this.installCache = (cacheName, urlArray) => {

        urlArray.forEach(url => {

            fetch(url).then(response => {

                if (!response.ok) console.warn('Warning: cache response status [' + response.status + '] - ' + url);
                return caches.open(cacheName).then(cache => { return cache.put(url, response); });

            });

        });

    }


}).call(ceresCacheMethods);
