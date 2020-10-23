export { similarity }

import { cookies } from '../mods/cerescookies.min.js';
import { cereslibrary as csl} from '../mods/cereslibrary.min.js';
import { similaritycache } from '../mods/similaritycache.min.js';

var similarity = {};
(function()
{
    'use strict';

    this.onload = function() { onloadPrimary(); }; // global scope method reference
    this.getMarkup = function(id, el) { getMarkupDocument(id, el); };  // global scope method reference
    this.getPinState = function(el) { resetPinState(el); };  // global scope method reference

    let includeDirective = 'include-directive';

    window.customElements.get(includeDirective) || window.customElements.define(includeDirective, class extends HTMLElement
    {
        async connectedCallback()
        {
            const src = this.getAttribute('src');
            this.insertAdjacentHTML('afterbegin', await ( await fetch(src) ).text());
        }

    });

    class Component
    {
        constructor()
        {
            this.attribute = function() { return attribute; }
        }

    }

    let resource = new Component();
    let location = new Map();
    let pinimage = new Map();

    setResourcePrecursors();

    let getMarkupDocument = function (markupId, buttonElement)
    {
        if (resource.attribute.markupId != markupId)
        {
            resource.attribute.markupId = markupId;
            resource.attribute.markupUrl = (location.has(resource.attribute.markupId)) ? location.get(resource.attribute.markupId) : location.get('index');

            document.getElementById('frame-container').setAttribute('src', resource.attribute.markupUrl);
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
        if (!cookies.get('hd')) cookies.set('hd', 'block', { 'max-age': 7200, 'samesite': 'None; Secure'  });
        if (!cookies.get('pn')) cookies.set('pn', 'disabled', { 'max-age': 7200, 'samesite': 'None; Secure' });

        document.getElementById('site-header-display').style.display = (cookies.get('hd') == 'none') ? 'none' : 'block';

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

        resource.attribute.markupId = 'index';
        resource.attribute.markupUrl = location.get(resource.attribute.markupId);
    }

}).call(window);
