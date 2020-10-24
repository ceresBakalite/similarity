export { similarityframe }

import { cookies} from '../mods/cereslibrary.min.js';

var similarityframe = {};
(function()
{
    'use strict';

    this.onload = function(id) { onloadFrame(id); };  // global scope method reference

    const includeDirective = 'include-directive';
    const refreshMarkdown = false;

    window.customElements.get(includeDirective) || window.customElements.define(includeDirective, class extends HTMLElement
    {
        async connectedCallback()
        {
            const src = this.getAttribute('src');
            this.insertAdjacentHTML('afterbegin', await ( await fetch(src) ).text());
        }

    });

    function onloadFrame(markupId)
    {
        let isValidSource = function()
        {
            if (parent.document.getElementById('ceresbakalite')) return true;

            window.location.href = '/similarity/?mu=' + markupId;

            return false;
        }

        let invokeScrollEventListener = function()
        {
            window.onscroll = function() { adjustHeaderDisplay(); };
        }

        let displayFooter = function()
        {
            setTimeout(function() {  document.getElementById('footer-content').style.display = 'block'; }, 2000);
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
        let setStyleDisplay = function(attribute)
        {
            cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
            el.style.display = attribute;
        }

        const el = parent.document.getElementById('site-header-display');
        const pin = parent.document.getElementById('pin-navbar').getAttribute('state');
        const trigger = 25;

        if (pin == 'disabled')
        {
            if (el.style.display && window.scrollY > trigger)
            {
                if (el.style.display != 'none') setTimeout(function(){ setStyleDisplay('none'); }, 250);

            } else {

                if (el.style.display != 'block') setTimeout(function(){ setStyleDisplay('block'); }, 250);
            }

        }

    }

}).call(window);
