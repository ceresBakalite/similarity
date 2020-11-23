export { similarityframe };

import { include, cookies, resource } from '../mods/cereslibrary.min.js';

var similarityframe = {};
(function()
{
    'use strict';

    include.directive();

    const ceres = this;

    initialise();

    this.onload = function() { onloadFrame(); };  // global scope method reference

    function onloadFrame()
    {
        if (rsc.isValidSource())
        {
            rsc.invokeScrollEventListener();
            rsc.asyncPullMarkdownRequest();
            rsc.displaySlideviewContent();
        }

    }

    function initialise()
    {
        ceres.rsc = {};
        (function() {

            rsc.adjustHeaderDisplay = function()
            {
                const header = parent.document.querySelector('div.page-header');
                const pin = parent.document.querySelector('img.pin-navbar').getAttribute('state');
                const trigger = 25;

                const setStyleDisplay = function(attribute)
                {
                    cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
                    header.style.display = attribute;
                }

                if (pin == 'disabled')
                {
                    if (header.style.display && window.scrollY > trigger)
                    {
                        if (header.style.display != 'none') setTimeout(function(){ setStyleDisplay('none'); }, 250);

                    } else {

                        if (header.style.display != 'block') setTimeout(function(){ setStyleDisplay('block'); }, 250);
                    }

                }

            }

            rsc.isValidSource = function()
            {
                const sync = document.querySelector('body');

                if (parent.document.querySelector('body.ceres > section.index')) return true;
                window.location.href = '/similarity/?sync=' + sync.className;

                return false;
            }

            rsc.invokeScrollEventListener = function()
            {
                window.onscroll = function() { adjustHeaderDisplay(); };
            }

            rsc.asyncPullMarkdownRequest = function()
            {
                setTimeout(function() { resource.composeCORSLinks( { node: 'zero-md', query: 'div.markdown-body' } ); }, 1000);
                setTimeout(function() {  document.querySelector('div.footer-content').style.display = 'block'; }, 2000);
            }

            rsc.displaySlideviewContent = function()
            {
                let el = document.querySelector('slideview-content none');
                if (el) el.className = 'slideview-content'
            }

        })(); // end resource allocation

        Object.freeze(rsc);
    }

}).call(window);
