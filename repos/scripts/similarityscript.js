export { similarity };

import { resource, cookies, include } from '../mods/cereslibrary.min.js';
import { similaritycache } from '../mods/similaritycache.min.js';

var similarity = {};
(function() {

    include.directive(); // HTML namespace include scripts

    const rsc = {}; // resource namespace
    rscMethods(); // resource methods

    this.onload = () => { rsc.onloadFrame(); } // global scope method reference
    this.getMarkup = (id, el) => { rsc.getMarkupDocument(id, el); }  // global scope method reference
    this.getPinState = el => { rsc.resetPinState(el); }  // global scope method reference

    function rscMethods() {

        (function() { // methods belonging to the resource object

            const location = new Map();
            const pinimage = new Map();

            pinimage.set('enabled', './images/NAVPinIconEnabled.png');
            pinimage.set('disabled', './images/NAVPinIconDisabled.png');

            location.set('index', './repos/scripts/SyncIndex.html');
            location.set('shell', './repos/scripts/SyncShell.html');
            location.set('slide', './repos/scripts/SyncSlide.html');
            location.set('repos', './repos/scripts/SyncRepos.html');

            location.set('test', './repos/scripts/SyncTest.html');

            this.markupUrl = location.get('index');

            this.onloadFrame = () => {

                this.getHeaderAttributes();
                this.getQueryString();
            }

            this.getMarkupDocument = (markupId, buttonElement) => {

                if (this.markupId != markupId)
                {
                    this.markupId = markupId;
                    this.markupUrl = location.get(this.markupId) || location.get('index');

                    document.querySelector('iframe.frame-container').setAttribute('src', this.markupUrl);
                };

                if (buttonElement) buttonElement.blur();
            }

            this.resetPinState = el =>  {

                if (el.getAttribute('state') == 'enabled')
                {
                    this.setPinState(el, 'disabled');
                    this.setDisplayState('block');

                } else {

                    this.setPinState(el, 'enabled');
                };

            }

            this.setDisplayState = attribute =>  {

                const header = document.querySelector('div.page-header');
                if (header.style.display != 'block') setTimeout(() => { header.style.display = 'block'; }, 250);
                cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
            }

            this.setPinState = (el, attribute) =>  {

                if (resource.ignore(el)) return;

                el.src = pinimage.get(attribute);
                el.setAttribute('state', attribute);
                cookies.set('pn', attribute, { 'max-Age': 7200, 'samesite': 'None; Secure' });
            }

            this.getHeaderAttributes = () =>  {

                const header = document.querySelector('div.page-header');

                if (!cookies.get('hd')) cookies.set('hd', 'block', { 'max-age': 7200, 'samesite': 'None; Secure'  });
                if (!cookies.get('pn')) cookies.set('pn', 'disabled', { 'max-age': 7200, 'samesite': 'None; Secure' });

                if (header) header.style.display = (cookies.get('hd') == 'none') ? 'none' : 'block';
                if (cookies.get('pn') == 'enabled') this.setPinState(document.querySelector('img.pin-navbar'), 'enabled');
            }

            this.getQueryString = () =>  {

                const urlParams = new URLSearchParams(globalThis.location.search);
                const name = urlParams.get('sync')

                if (name) this.getMarkupDocument(name);
            }

        }).call(rsc); // end resource namespace

    }

}).call(globalThis);
