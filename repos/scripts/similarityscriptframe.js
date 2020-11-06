export { similarityframe };

import { cookies, include } from '../mods/cereslibrary.min.js';

var similarityframe = {};
(function()
{
    'use strict';

    include.directive();

    this.onload = function() { onloadFrame(); };  // global scope method reference

    const refreshMarkdown = false;

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

        let setMarkdownLinks = function()
        {
            const root = 'zero-md';
            const element = '.markdown-body';
            const regex = /<a /gi;
            const replacement = '<a target="_top" ';

            replaceShadowDomInnerHTML(root, element, regex, replacement);
        }

        let asyncPullMarkdownRequest = function()
        {
            let fetchMarkdown = function()
            {
                const nodelist = document.querySelectorAll('zero-md');
                nodelist.forEach(el => { el.setAttribute('src', el.getAttribute('src') + '?' + Date.now()); });
                setTimeout(function() { setMarkdownLinks(); }, 1000);
            }

            displayFooter();
            setTimeout(function() { setMarkdownLinks(); }, 1000);

            if (refreshMarkdown) setTimeout(function() { fetchMarkdown(); }, 4000);
        }

        let replaceShadowDomInnerHTML = function(root, element, regex, replacement)
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
