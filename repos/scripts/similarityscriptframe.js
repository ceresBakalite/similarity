export { similarityframe };

import { include, cookies, resource } from '../mods/cereslibrary.min.js';

var testCall = (el) => {

    console.log('Hello from the shadowdom: ' + el.src);
    resource.srcOpen({ element: el, type: 'image' });
}

var similarityframe = {};
(function() {

    include.directive(); // fetch HTML namespace include scripts

    const rsc = {}; // resource namespace
    rscMethods(); // resource methods

    this.onload = () => { rsc.onloadFrame(); } // global scope method reference

    function rscMethods()
    {
        (function() {  // methods belonging to the resource object

            this.onloadFrame = () => {

                if (this.isValidSource())
                {
                    this.invokeScrollEventListener();
                    this.asyncPullMarkdownRequest();
                    this.displaySlideviewContent();
                };

            }

            this.adjustHeaderDisplay = () => {

                const header = parent.document.querySelector('div.page-header');
                const pin = parent.document.querySelector('img.pin-navbar').getAttribute('state');
                const trigger = 25;

                const setStyleDisplay = attribute => {

                    cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
                    header.style.display = attribute;
                }

                if (pin == 'disabled')
                {
                    if (header.style.display && globalThis.scrollY > trigger)
                    {
                        if (header.style.display != 'none') setTimeout(() => { setStyleDisplay('none'); }, 250);

                    } else {

                        if (header.style.display != 'block') setTimeout(() => { setStyleDisplay('block'); }, 250);
                    };

                };

            }

            this.isValidSource = () => {

                const sync = document.querySelector('body');

                if (parent.document.querySelector('body.ceres > section.index')) return true;
                globalThis.location.href = '/similarity/?sync=' + sync.className;

                return false;
            }

            this.invokeScrollEventListener = () => { globalThis.onscroll = () => { rsc.adjustHeaderDisplay(); }; }

            this.asyncPullMarkdownRequest = () => {

                setTimeout(() => { resource.shadowCORSLinks( { node: 'zero-md', query: 'div.markdown-body' } ); }, 1000);
                setTimeout(() => {  document.querySelector('div.footer-content').style.display = 'block'; }, 2000);
            }

            this.displaySlideviewContent = () => {

                let csv = document.querySelectorAll('div.slideview-content');
                csv.forEach(el => { el.className = 'slideview-content'; });
            }

        }).call(rsc); // end resource allocation

    }

}).call(globalThis);
