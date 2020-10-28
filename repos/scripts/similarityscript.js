export { similarity }

import { generic as gn, cookies } from '../mods/cereslibrary.min.js';
import { similaritycache } from '../mods/similaritycache.min.js';

var similarity = {};
(function()
{
    'use strict';

    this.onload = function() { onloadPrimary(); }; // global scope method reference
    this.getMarkup = function(id, el) { getMarkupDocument(id, el); };  // global scope method reference
    this.getPinState = function(el) { resetPinState(el); };  // global scope method reference

    let rsc = function() { return attribute; }  // similarity local resource attributes
    let includeDirective = 'include-directive';
    let location = new Map();
    let pinimage = new Map();

    window.customElements.get(includeDirective) || window.customElements.define(includeDirective, class extends HTMLElement
    {
        async connectedCallback()
        {
            const src = this.getAttribute('src') || null;
            if (rsc.callback = !gn.isEmptyOrNull(src)) this.insertAdjacentHTML('afterbegin', await ( await fetch(src) ).text());
        }

    });

    setResourcePrecursors();

    let getMarkupDocument = function (markupId, buttonElement)
    {
        if (rsc.markupId != markupId)
        {
            rsc.markupId = markupId;
            rsc.markupUrl = location.get(rsc.markupId) || location.get('index');

            document.getElementById('frame-container').setAttribute('src', rsc.markupUrl);
        }

        if (buttonElement) buttonElement.blur();
    }

    let resetPinState = function(el)
    {
        if (el.getAttribute('state') == 'enabled')
        {
            setPinState(el, 'disabled');
            setDisplayState('block');

        } else {

            setPinState(el, 'enabled');
        }

    }

    let setDisplayState = function(attribute)
    {
        const header = document.getElementById('site-header-display');
        if (header.style.display != 'block') setTimeout(function() { header.style.display = 'block'; }, 250);
        cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
    }

    let setPinState = function(el, attribute)
    {
        el.src = pinimage.get(attribute);
        el.setAttribute('state', attribute);
        cookies.set('pn', attribute, { 'max-Age': 7200, 'samesite': 'None; Secure' });
    }

    let getHeaderAttributes = function()
    {
        const el = document.getElementById('site-header-display');

        if (!cookies.get('hd')) cookies.set('hd', 'block', { 'max-age': 7200, 'samesite': 'None; Secure'  });
        if (!cookies.get('pn')) cookies.set('pn', 'disabled', { 'max-age': 7200, 'samesite': 'None; Secure' });

        if (el) el.style.display = (cookies.get('hd') == 'none') ? 'none' : 'block';

        if (cookies.get('pn') == 'enabled') setPinState(document.getElementById('pin-navbar'), 'enabled');
    }

    let getQueryString = function()
    {
        const urlParams = new URLSearchParams(window.location.search);
        const markupId = urlParams.get('mu')

        if (markupId) getMarkupDocument(markupId);
    }

    function onloadPrimary()
    {
        getHeaderAttributes();
        getQueryString();
    }

    function setResourcePrecursors()
    {
        pinimage.set('enabled', './images/NAVPinIconEnabled.png');
        pinimage.set('disabled', './images/NAVPinIconDisabled.png');

        location.set('index', './repos/scripts/SyncIndex.html');
        location.set('shell', './repos/scripts/SyncShell.html');
        location.set('slide', './repos/scripts/SyncSlide.html');
        location.set('repos', './repos/scripts/SyncRepos.html');

        rsc.markupId = 'index';
        rsc.markupUrl = location.get(rsc.markupId);
    }

}).call(window);
