export { similarity }

import { cookies } from 'https://ceresbakalite.github.io/similarity/repos/scripts/cerescookies.js';
import { similaritycache } from 'https://ceresbakalite.github.io/similarity/repos/scripts/similaritycache.js';

const frame = document.getElementById('frame-container');
frame.contentWindow.postMessage('test', 'http://ceresb.com');

window.addEventListener('message', event =>
{
    // IMPORTANT: check the origin of the data!
    if (event.origin.startsWith('https://ceresbakalite.github.io'))
    {
        // The data was sent from your site.
        // Data sent with postMessage is stored in event.data:
        console.log(event.data);
    } else {
        // The data was NOT sent from your site!
        // Be careful! Do not use it. This else branch is
        // here just for clarity, you usually shouldn't need it.
        return;
    }
});


var similarity = {};
(function()
{
    'use strict';

    this.onload = function() { onloadPrimary(); }; // public method reference
    this.getMarkup = function(id, el) { getMarkupDocument(id, el); };  // public method reference
    this.getPinState = function(el) { resetPinState(el); };  // public method reference

    let includeDirective = 'include-directive';

    window.customElements.get(includeDirective) || window.customElements.define(includeDirective, class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
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

    function getQueryString()
    {
        const urlParams = new URLSearchParams(window.location.search);
        const markupId = urlParams.get('mu')

        if (markupId) getMarkupDocument(markupId);
    }

    function getMarkupDocument(markupId, buttonElement)
    {
        if (resource.attribute.markupId != markupId)
        {
            resource.attribute.markupId = markupId;
            resource.attribute.markupUrl = (location.has(resource.attribute.markupId)) ? location.get(resource.attribute.markupId) : location.get('index');

            document.getElementById('frame-container').setAttribute('src', resource.attribute.markupUrl);
        }

        if (buttonElement) buttonElement.blur();
    }

    function getHeaderAttributes()
    {
        if (!cookies.get('hd')) cookies.set('hd', 'block', { 'max-age': 7200 });
        if (!cookies.get('pn')) cookies.set('pn', 'disabled', { 'max-age': 7200 });

        if (cookies.get('hd') == 'none') document.getElementById('site-header-display').style.display = 'none';
        if (cookies.get('pn') == 'enabled') setPinState(document.getElementById('pin-navbar'), 'enabled');
    }

    function onloadPrimary()
    {
        getHeaderAttributes();
        getQueryString();
    }

    function setResourcePrecursors()
    {
        pinimage.set('enabled', 'https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png');
        pinimage.set('disabled', 'https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png');

        location.set('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
        location.set('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
        location.set('slide', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html');
        location.set('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');

        resource.attribute.markupId = 'index';
        resource.attribute.markupUrl = location.get(resource.attribute.markupId);
    }

    function resetPinState(el)
    {
        if (el.getAttribute('state') == 'enabled')
        {
            setPinState(el, 'disabled');
            resetDisplayState('block');

        } else {

            setPinState(el, 'enabled');
        }

    }

    function resetDisplayState(attribute)
    {
        let header = document.getElementById('site-header-display');
        if (header.style.display != 'block') setTimeout(function() { header.style.display = 'block'; }, 250);
        cookies.set('hd', attribute, { 'max-age': 7200 });
    }

    function setPinState(el, attribute)
    {
        el.src = pinimage.get(attribute);
        el.setAttribute('state', attribute);
        cookies.set('pn', attribute, { 'max-age': 7200 });
    }

}).call(window);
