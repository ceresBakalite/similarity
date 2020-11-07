export { similarityframe };

import { include, cookies, compose } from '../mods/cereslibrary.min.js';

var similarityframe = {};
(function()
{
    'use strict';

    include.directive();

    this.onload = function() { onloadFrame(); };  // global scope method reference

    function onloadFrame()
    {
        const sync = document.querySelector('body');

        let isValidSource = function()
        {
            if (parent.document.querySelector('body.ceres > section.index')) return true;
            window.location.href = '/similarity/?sync=' + sync.className;

            return false;
        }

        let invokeScrollEventListener = function()
        {
            window.onscroll = function() { adjustHeaderDisplay(); };
        }

        let displayFooter = function()
        {
            setTimeout(function() {  document.querySelector('div.footer-content').style.display = 'block'; }, 2000);
        }

        let xxxresetMarkdownLinks = function()
        {
            const root = 'zero-md';
            const element = '.markdown-body';
            const regex = /<a /gi;
            const replacement = '<a target="_top" ';

            replaceShadowDomInnerHTML(root, element, regex, replacement);
        }

        let setCORSMarkdownLinks = function(el)
        {
            // el.node = 'zero.md'
            const nodelist = document.querySelectorAll(el.node);

            if (!el.regex) el.regex = /<a /gi;
            if (!el.replacement) el.replacement = '<a target="_top" ';

            nodelist.forEach(node => {

                // el.query = zero-md.markdown-body

                let shadow = node.shadowRoot;

                if (shadow)
                {
                    let markdown = shadow.querySelector(el.query).innerHTML;
                    shadow.querySelector(el.query).innerHTML = markdown.replace(el.regex, el.replacement);
                }

            });

        }

        let asyncPullMarkdownRequest = function()
        {
            setTimeout(function() { setCORSMarkdownLinks( { node: 'zero-md', query: 'div.markdown-body' } ); }, 1000);
            displayFooter();
        }

        let xxxreplaceShadowDomInnerHTML = function(root, element, regex, replacement)
        {
            const nodelist = document.querySelectorAll(root);

            nodelist.forEach(node => {

                let shadow = node.shadowRoot;

                if (shadow)
                {
                    let markdown = shadow.querySelector(element).innerHTML;
                    shadow.querySelector(element).innerHTML = markdown.replace(regex, replacement);
                }

            });

        }

        if (isValidSource())
        {
            invokeScrollEventListener();
            asyncPullMarkdownRequest();
        }

    }

    function adjustHeaderDisplay()
    {
        const header = parent.document.querySelector('div.page-header');
        const pin = parent.document.querySelector('img.pin-navbar').getAttribute('state');
        const trigger = 25;

        let setStyleDisplay = function(attribute)
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

}).call(window);
