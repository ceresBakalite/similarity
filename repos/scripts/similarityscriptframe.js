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

        function setOpenHrefOnTop()
        {
            if (document.getElementById('index-md').shadowRoot)
            {

                document.getElementsByTagName('zero-md')
                .filter(element => element.shadowRoot)
                .forEach(element => {
                    console.log('shadow.mode: ' + el.mode);
                });

                const nodelist = document.querySelectorAll('zero-md');
                nodelist.forEach(node => {

                    let shadow = node.shadowRoot;
                    if (shadow) console.log('shadow.mode: ' + shadow.mode);

                    let elementlist = document.getElementsByTagName('shadow.a');
                    elementlist.forEach(el => {

                        console.log('shadow a el: ');

                    });

                });



                //let el = document.getElementById('index-md').shadowRoot;
                //if (el) console.log('shadow.mode: ' + el.mode);
            }
            /*

            console.log('markdown-body: ' + getElementsByClassName('markdown-body')[0] ? true : false);
            console.log('div.markdown-body: ' + div.getElementsByClassName('markdown-body') ? true : false);

            const nodelist = document.querySelectorAll('div.markdown-body a');
            nodelist.forEach(el => {
                console.log('hello from me: ');
            });

            */
        }

        function asyncPullMarkdownRequest()
        {
            displayFooter();
            setTimeout(function() { setOpenHrefOnTop(); }, 1000);


            setTimeout(function() { refreshMarkdown(); }, 4000);

            function refreshMarkdown()
            {
                const nodelist = document.querySelectorAll('zero-md');
                nodelist.forEach(el => { el.setAttribute('src', el.getAttribute('src') + '?' + Date.now()); });
                setTimeout(function() { setOpenHrefOnTop(); }, 1000);
            }

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
