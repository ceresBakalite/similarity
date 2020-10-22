export { similarityframe }

import { cookies } from './cerescookies.js';

var similarityframe = {};
(function()
{
    'use strict';

    this.onload = function(id) { onloadFrame(id); };  // public method reference

    let includeDirective = 'include-directive';

    window.customElements.get(includeDirective) || window.customElements.define(includeDirective, class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.insertAdjacentHTML('afterbegin', await ( await fetch(src) ).text());
        }

    });

    function onloadFrame(markupId)
    {
        if (isValidSource())
        {
            invokeScrollEventListener();
            asyncPullMarkdownRequest();
        }

        function isValidSource()
        {
            if (parent.document.getElementById('ceresbakalite')) return true;

            window.location.href = '/similarity/?mu=' + markupId;

            return false;
        }

        function invokeScrollEventListener()
        {
            window.onscroll = function() { adjustHeaderDisplay(); };
        }

        function displayFooter()
        {
            setTimeout(function() {  document.getElementById('footer-content').style.display = 'block'; }, 2000);
        }

        function setMarkdownLinks()
        {
            const root = 'zero-md';
            const element = '.markdown-body';
            const regex = /<a /gi;
            const replacement = '<a target="_top" ';

            replaceShadowDomInnerHTML(root, element, regex, replacement);
        }

        function asyncPullMarkdownRequest()
        {
            displayFooter();
            setTimeout(function() { setMarkdownLinks(); }, 1000);
            setTimeout(function() { refreshMarkdown(); }, 4000);

            function refreshMarkdown()
            {
                const nodelist = document.querySelectorAll('zero-md');
                nodelist.forEach(el => { el.setAttribute('src', el.getAttribute('src') + '?' + Date.now()); });
                setTimeout(function() { setMarkdownLinks(); }, 1000);
            }

        }

        function replaceShadowDomInnerHTML(root, element, regex, replacement)
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

    }

    function adjustHeaderDisplay()
    {
        let el = parent.document.getElementById('site-header-display');
        let pin = parent.document.getElementById('pin-navbar').getAttribute('state');
        let trigger = 25;

        if (pin == 'disabled')
        {
            if (el.style.display && window.scrollY > trigger)
            {
                if (el.style.display != 'none') setTimeout(function(){ setStyleDisplay('none'); }, 250);

            } else {

                if (el.style.display != 'block') setTimeout(function(){ setStyleDisplay('block'); }, 250);
            }

        }

        function setStyleDisplay(attribute)
        {
            cookies.set('hd', attribute, { 'max-age': 7200, 'samesite': 'None; Secure' });
            el.style.display = attribute;
        }

    }

}).call(window);
