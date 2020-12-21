export { similarity };

import { resource, cookies, include } from '../mods/cereslibrary.min.js';
import { similaritycache } from '../mods/similaritycache.min.js';

var similarity = {};
(function()
{
    'use strict';

    include.directive();

    const rsc = {};
    const location = new Map();
    const pinimage = new Map();

    initialise();

    this.onload = () => { rsc.onloadFrame(); }; // global scope method reference
    this.getMarkup = function(id, el) { getMarkupDocument(id, el); };  // global scope method reference
    this.getPinState = function(el) { resetPinState(el); };  // global scope method reference

    function getMarkupDocument(markupId, buttonElement)
    {
        if (rsc.markupId != markupId)
        {
            rsc.markupId = markupId;
            rsc.markupUrl = location.get(rsc.markupId) || location.get('index');

            document.querySelector('iframe.frame-container').setAttribute('src', rsc.markupUrl);
        }

        if (buttonElement) buttonElement.blur();
    }

    function resetPinState(el)
    {
        if (el.getAttribute('state') == 'enabled')
        {
            setPinState(el, 'disabled');
            setDisplayState('block');

        } else {

            setPinState(el, 'enabled');
        }

    }

    function setDisplayState(attribute)
    {
        const header = document.querySelector('div.page-header');
        if (header.style.display != 'block') setTimeout(function() { header.style.display = 'block'; }, 250);
        cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
    }

    function setPinState(el, attribute)
    {
        if (resource.ignore(el)) return;

        el.src = pinimage.get(attribute);
        el.setAttribute('state', attribute);
        cookies.set('pn', attribute, { 'max-Age': 7200, 'samesite': 'None; Secure' });
    }

    function getHeaderAttributes()
    {
        const header = document.querySelector('div.page-header');

        if (!cookies.get('hd')) cookies.set('hd', 'block', { 'max-age': 7200, 'samesite': 'None; Secure'  });
        if (!cookies.get('pn')) cookies.set('pn', 'disabled', { 'max-age': 7200, 'samesite': 'None; Secure' });

        if (header) header.style.display = (cookies.get('hd') == 'none') ? 'none' : 'block';
        if (cookies.get('pn') == 'enabled') setPinState(document.querySelector('img.pin-navbar'), 'enabled');
    }

    function getQueryString()
    {
        const urlParams = new URLSearchParams(window.location.search);
        const name = urlParams.get('sync')

        if (name) getMarkupDocument(name);
    }

    function initialise() {

        (function() { // methods belonging to the resource object

            pinimage.set('enabled', './images/NAVPinIconEnabled.png');
            pinimage.set('disabled', './images/NAVPinIconDisabled.png');

            location.set('index', './repos/scripts/SyncIndex.html');
            location.set('shell', './repos/scripts/SyncShell.html');
            location.set('slide', './repos/scripts/SyncSlide.html');
            location.set('repos', './repos/scripts/SyncRepos.html');

            location.set('test', './repos/scripts/SyncTest.html');

            this.markupUrl = location.get('index');

            this.onloadFrame = () => {

                getHeaderAttributes();
                getQueryString();
            }

        }).call(rsc); // end resource namespace

    }

}).call(window);
