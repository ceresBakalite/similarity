let similarity = {id: 'index'};

window.customElements.define('include-directive', class extends HTMLElement
{
    async connectedCallback()
    {
        let src = this.getAttribute('src');
        this.innerHTML = await (await fetch(src)).text();
    }

});

function getQueryString()
{
    const urlParams = new URLSearchParams(window.location.search);
    const mu = urlParams.get('mu')

    if (mu) getMarkupDocument(mu);

    return (mu) ? mu : 'index';
}

function getMarkupDocument(mu)
{
    if (similarity.id != mu)
    {
        similarity.id = mu;
        document.getElementById('frame-container').setAttribute('src', getMarkupLocation());
    }

    document.getElementById(similarity.id).blur();

    function getMarkupLocation()
    {
       const lookup = {
           'index': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html',
           'shell': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html',
           'slide': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
           'repos': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html',
           'default': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html'
       };

       return lookup[similarity.id] || lookup['default'];
    }

}

function onloadPrimary()
{
    similarity.id = getQueryString();
}

function onloadFrame(md)
{
    if (isValidSource())
    {
        invokeScrollEventListener();

        const initialise = {
            'slide-md': function() { asyncPullMarkdownRequest(md); },
            'index-md': function() { asyncPullMarkdownRequest(md); },
            'shell-md': function() { asyncPullMarkdownRequest(md); },
            'repos-md': function() { asyncPullMarkdownRequest(md); },
            'default': function() { asyncPullMarkdownRequest('index-md'); }
        };

        initialise[md]() || initialise['default']();
    }

    function isValidSource()
    {
        if (window.top.document.getElementById('ceresbakalite')) return true;

        window.location.href = 'https://ceresbakalite.github.io/similarity/?mu=' + md;

        return false;
    }

    function invokeScrollEventListener()
    {
        window.onscroll = function() { adjustHeaderDisplay(); };
    }

    function asyncPullMarkdownRequest(md)
    {
        displayFooter();

        setTimeout(function() { refreshMarkdown(); }, 4000);

        function refreshMarkdown()
        {
            let el = (document.getElementById(md)) ? document.getElementById(md) : document.getElementsByTagName('zero-md')[0];
            if (el) el.setAttribute('src', el.getAttribute('src') + '?' + Date.now());
        }

    }

}

function displayFooter()
{
    setTimeout(function() { document.getElementById('footer-content').style.display = 'block'; }, 2000);
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

function resetPinState()
{
    let el = document.getElementById('pin-navbar');

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
