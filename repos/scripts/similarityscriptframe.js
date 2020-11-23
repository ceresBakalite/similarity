export { similarityframe };

import { include, cookies, resource } from '../mods/cereslibrary.min.js';

var similarityframe = {};
(function()
{
    'use strict';

    include.directive();

    initialise();

    this.onload = function() { onloadFrame(); };  // global scope method reference

    function onloadFrame()
    {
        if (frm.isValidSource())
        {
            frm.invokeScrollEventListener();
            frm.asyncPullMarkdownRequest();
            frm.displaySlideviewContent();
        }

    }

    function initialise()
    {
        const frm = {};
        (function() {

            this.adjustHeaderDisplay = function()
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

            this.isValidSource = function()
            {
                const sync = document.querySelector('body');

                if (parent.document.querySelector('body.ceres > section.index')) return true;
                window.location.href = '/similarity/?sync=' + sync.className;

                return false;
            }

            this.invokeScrollEventListener = function()
            {
                window.onscroll = function() { this.adjustHeaderDisplay(); };
            }

            this.asyncPullMarkdownRequest = function()
            {
                setTimeout(function() { resource.composeCORSLinks( { node: 'zero-md', query: 'div.markdown-body' } ); }, 1000);
                setTimeout(function() {  document.querySelector('div.footer-content').style.display = 'block'; }, 2000);
            }

            this.displaySlideviewContent = function()
            {
                let csv = document.querySelectorAll('div.slideview-content');
                csv.forEach((el) => { el.className = 'slideview-content'; });
            }

        }).call(frm); // end resource allocation

    }

}).call(window);
