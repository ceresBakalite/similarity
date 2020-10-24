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
export { ceres, cookie }

var ceres = {};
(function()
{
    'use strict';

    let rsc = new class // local scope resources
    {
        constructor()
        {
            this.protean = function() { return protean; },
            this.attribute = function() { return attribute; }
        }

    }

    let sbl = new Map(); // local scope symbols

    setPrecursors();

    this.constant = rsc.protean; // exposed local scope resources

    Object.freeze(this.constant);

    this.windowOpen = function(obj) { window.open(obj.element.getAttribute('src'), obj.type); }
    this.isString = function(obj) { return Object.prototype.toString.call(obj) == '[object String]'; }
    this.clearElement = function(el) { while (el.firstChild) el.removeChild(el.firstChild); }

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

    this.isEmptyOrNull = function(obj)
    {
        if (obj === null || obj == 'undefined') return true;

        if (this.isString(obj)) return (obj.length === 0 || !obj.trim());
        if (Array.isArray(obj)) return (obj.length === 0);
        if (obj.constructor === Object) return (Object.keys(obj).length === 0);

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

    this.getUniqueElementId = function(str = null, range = 100)
    {
        let elName = function() { return str + Math.floor(Math.random() * range) };
        let el = null;

        while (document.getElementById(el = elName())) {};

        return el;
    }

    this.inspect = function(diagnostic)
    {
        if (this.isEmptyOrNull(diagnostic)) return this.inspect({ type: this.constant.error, notification: rsc.attribute.inspect });

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
        if (this.isEmptyOrNull(error)) return this.inspect({ type: this.constant.error, notification: rsc.attribute.errorHandler });

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

        rsc.protean.reference = 1;
        rsc.protean.notify = 2;
        rsc.protean.error = 99;
        rsc.protean.isWindows = (navigator.appVersion.indexOf('Win') != -1);
        rsc.protean.newline = rsc.protean.isWindows ? '\r\n' : '\n';

        rsc.attribute.inspect = 'Error: An exception occurred in the inspect method.  The diagnostic argument was empty or null';
        rsc.attribute.errorhandler = 'Error: An exception occurred in the errorhandler method.  The error argument was empty or null';

        Object.freeze(rsc);
    }

}).call(ceres);

var cookie = {};
(function() {

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

}).call(cookie);
