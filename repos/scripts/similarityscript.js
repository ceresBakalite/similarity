let similarity = {};
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

    class component
    {
        constructor()
        {
            this.type = function() { return type; },
            this.object = function() { return object; }
            this.attribute = function() { return attribute; }
        }

    }

    let resource = new component();

    function setResourcePrecursors()
    {
        resource.type.current = 1;
        resource.type.content = 2;
        resource.type.element = 3;

        resource.object.markdown = [];
        resource.attribute.markupId = 'index';
        resource.attribute.markdownId = null;
    }

    let current = { markupId: 'index', markdownId: null };

    setResourcePrecursors();

    ceres.onloadPrimary = function() { onloadPrimary(); }; // public method reference
    ceres.onloadFrame = function(id) { onloadFrame(id); };  // public method reference
    ceres.getMarkupDocument = function(id, el) { getMarkupDocument(id, el); };  // public method reference
    ceres.resetPinState = function(el) { resetPinState(el); };  // public method reference

    function getQueryString()
    {
        const urlParams = new URLSearchParams(window.location.search);
        const markupId = urlParams.get('mu')

        if (markupId) getMarkupDocument(markupId);
    }

    function getMarkupDocument(markupId, buttonElement)
    {
        if (resource.attribute.markupId != markupId)
        {
            resource.attribute.markupId = markupId;
            document.getElementById('frame-container').setAttribute('src', getMarkupLocation());
        }

        if (buttonElement) buttonElement.blur();

        function getMarkupLocation()
        {
            const lookup = {
                'index': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html',
                'shell': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html',
                'slide': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
                'repos': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html',
                'default': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html'
            };

            return lookup[resource.attribute.markupId] || lookup['default'];
        }

    }

    function onloadPrimary()
    {
        getQueryString();
    }

    function onloadFrame(markupId)
    {
        if (isValidSource())
        {
            invokeScrollEventListener();

            const initialise = {
                'index': function() { asyncPullMarkdownRequest('index-md'); },
                'shell': function() { asyncPullMarkdownRequest('shell-md'); },
                'slide': function() { asyncPullMarkdownRequest('slide-md'); },
                'repos': function() { asyncPullMarkdownRequest('repos-md'); },
                'default': function() { asyncPullMarkdownRequest('index-md'); }
            };

            initialise[markupId]() || initialise['default']();
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

        function asyncPullMarkdownRequest(markdownId)
        {
            displayFooter();

            setTimeout(function() { refreshMarkdown(); }, 4000);

            function refreshMarkdown()
            {
                let el = (document.getElementById(markdownId)) ? document.getElementById(markdownId) : document.getElementsByTagName('zero-md')[0];
                if (el) el.setAttribute('src', el.getAttribute('src') + '?' + Date.now());
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

    function resetPinState(el)
    {
        //let el = document.getElementById(el.getAttributeById('id'));

        if (el.getAttribute('state') == 'enabled')
        {
            el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png";
            el.setAttribute('state', 'disabled');
            adjustHeaderDisplay();

        } else {

            el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png";
            el.setAttribute('state', 'enabled');

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

})(similarity);
