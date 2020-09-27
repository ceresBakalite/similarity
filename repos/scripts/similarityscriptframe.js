let similarityframe = {};
(function(ceres)
{
    window.customElements.define('include-directive', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    ceres.onload = function(id) { onloadFrame(id); };  // public method reference

    function onloadFrame(markupId)
    {
        if (isValidSource())
        {
            invokeScrollEventListener();
            asyncPullMarkdownRequest();
        }

        function isValidSource()
        {
            if (window.top.document.getElementById('ceresbakalite')) return true;

            window.location.href = 'https://ceresbakalite.github.io/similarity/?mu=' + markupId;

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
            //const regex = /<a /gi;
            let regex = /<a (.*?)>/gi;
            const replacement = '<a target="_top" ';

            replaceTest(root, element, regex, replacement)

//            replaceShadowRootInnerHTML(root, element, regex, replacement);
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

        function replaceTest(root, element, regex, replacement)
        {

            const nodelist = document.querySelectorAll(root);

            nodelist.forEach(node => {

                let shadow = node.shadowRoot;
                if (shadow)
                {
                    let markdown = shadow.querySelector(element).innerHTML;
                    let matchAll = Array.from(markdown.matchAll(regex));

                    matchAll.forEach(item => {

                        console.log( item );

                    });

                }

            });

        }

        function replaceShadowRootInnerHTML(root, element, regex, replacement)
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
        let el = window.top.document.getElementById('site-header-display');
        let pin = window.top.document.getElementById('pin-navbar').getAttribute('state');
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
            el.style.display = attribute;
        }

    }

    function getRandomInteger(min = 10000, max = 1000000)
    {
        return Math.floor(Math.random() * (max - min) ) + min;
    }

    function setCookie(cn, cv, ex = 0)
    {
        if (cn && cv)
        {
            let dt = new Date();
            dt.setTime(dt.getTime() + (ex * 24 * 60 * 60 * 1000));
            let expires = "expires=" + dt.toUTCString();

            document.cookie = cn + "=" + cv + ";" + expires + ";path=/";
        }

    }

    function getCookie(cn)
    {
        if (cn)
        {
            let cp = cn + "=";
            let dc = decodeURIComponent(document.cookie);
            let ca = dc.split(';');

            for(let i = 0; i < ca.length; i++)
            {
                let chr = ca[i];

                while (chr.charAt(0) == String.fromCharCode(32)) chr = chr.substring(1);

                if (chr) return chr.substring(cn.length, c.length);
            }

        }

        return null;
    }

})(similarityframe);
